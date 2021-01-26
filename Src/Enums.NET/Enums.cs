#region License
// Copyright (c) 2016 Tyler Brinkley
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using EnumsNET.Numerics;
using EnumsNET.Utilities;

namespace EnumsNET
{
    /// <summary>
    /// Static class that provides efficient type-safe enum operations through the use of cached enum members.
    /// Many operations are exposed as C# extension methods for convenience.
    /// </summary>
    public static class Enums
    {
        internal static class Cache<TEnum>
            where TEnum : struct, Enum
        {
            public static readonly EnumCache Instance = Cacher.Create(typeof(TEnum)).CreateCache<TEnum>();
        }

        internal abstract class Cacher
        {
            public readonly Type EnumType;

            protected Cacher(Type enumType)
            {
                EnumType = enumType;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            public static Cacher Create(Type enumType) =>
#if GET_TYPE_CODE
                Type.GetTypeCode(enumType)
#else
                Enum.GetUnderlyingType(enumType).GetTypeCode()
#endif
            switch
            {
                TypeCode.Boolean => new Cacher<bool, UnderlyingOperations>(enumType),
                TypeCode.Char => new Cacher<char, UnderlyingOperations>(enumType),
                TypeCode.SByte => new Cacher<sbyte, UnderlyingOperations>(enumType),
                TypeCode.Byte => new Cacher<byte, UnderlyingOperations>(enumType),
                TypeCode.Int16 => new Cacher<short, UnderlyingOperations>(enumType),
                TypeCode.UInt16 => new Cacher<ushort, UnderlyingOperations>(enumType),
                TypeCode.Int32 => new Cacher<int, UnderlyingOperations>(enumType),
                TypeCode.UInt32 => new Cacher<uint, UnderlyingOperations>(enumType),
                TypeCode.Int64 => new Cacher<long, UnderlyingOperations>(enumType),
                TypeCode.UInt64 => new Cacher<ulong, UnderlyingOperations>(enumType),
                _ => ThrowUnderlyingTypeNotSupportedException(enumType)
            };

            [MethodImpl(MethodImplOptions.NoInlining)]
            private static Cacher ThrowUnderlyingTypeNotSupportedException(Type enumType) => throw new NotSupportedException($"Enum underlying type of {Enum.GetUnderlyingType(enumType)} is not supported");

            public abstract EnumCache CreateCache<TEnum>()
                where TEnum : struct, Enum;
        }

        internal sealed class Cacher<TUnderlying, TUnderlyingOperations> : Cacher
            where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
            where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
        {
            public Cacher(Type enumType)
                : base(enumType)
            {
            }

            public override EnumCache CreateCache<TEnum>() => CreateCache(new EnumBridge<TEnum, TUnderlying, TUnderlyingOperations>());

            private EnumCache CreateCache(IEnumBridge<TUnderlying, TUnderlyingOperations> enumBridge)
            {
                var enumType = EnumType;
                var fields =
#if TYPE_REFLECTION
                enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
#else
                enumType.GetTypeInfo().DeclaredFields.Where(fieldInfo => (fieldInfo.Attributes & (FieldAttributes.Static | FieldAttributes.Public)) == (FieldAttributes.Static | FieldAttributes.Public)).ToArray();
#endif
                var size = HashHelpers.PowerOf2(Math.Max(fields.Length, 1));
                var buckets = new EnumMemberInternal<TUnderlying, TUnderlyingOperations>?[size];
                var members = new EnumMemberInternal<TUnderlying, TUnderlyingOperations>[fields.Length];

                // This is necessary due to a .NET reflection bug with retrieving Boolean Enum values
                Dictionary<string, TUnderlying>? fieldDictionary = null;
                if (typeof(TUnderlying) == typeof(bool))
                {
                    fieldDictionary = new Dictionary<string, TUnderlying>();
                    var values = (TUnderlying[])Enum.GetValues(enumType);
                    var names = Enum.GetNames(enumType);
                    for (var j = 0; j < names.Length; ++j)
                    {
                        fieldDictionary.Add(names[j], values[j]);
                    }
                }

                TUnderlyingOperations operations = default;
                var distinctCount = 0;
                TUnderlying allFlags = default;
                for (var i = 0; i < fields.Length; ++i)
                {
                    var field = fields[i];
                    var name = field.Name;
                    var value = fieldDictionary != null ? fieldDictionary[name] : (TUnderlying)field.GetValue(null)!;
                    var attributesArray =
#if TYPE_REFLECTION
                        Attribute.GetCustomAttributes(field, false);
#else
                        field.GetCustomAttributes(false).ToArray();
#endif
                    var attributes = attributesArray.Length == 0 ? AttributeCollection.Empty : new AttributeCollection(attributesArray);

                    var member = new EnumMemberInternal<TUnderlying, TUnderlyingOperations>(value, name, attributes);
                    var index = i;
                    var isPrimary = attributes.Has<PrimaryEnumMemberAttribute>();
                    while (index > 0 && (isPrimary ? !operations.LessThan(members[index - 1].Value, value) : operations.LessThan(value, members[index - 1].Value)))
                    {
                        --index;
                    }
                    if (index < i)
                    {
                        Array.Copy(members, index, members, index + 1, i - index);
                    }
                    members[index] = member;

                    // Try add to buckets
                    ref var next = ref buckets[value.GetHashCode() & (size - 1)];
                    while (next != null)
                    {
                        if (next.Value.Equals(value))
                        {
                            if (isPrimary)
                            {
                                member.Next = next.Next;
                                next.Next = null;
                                next = member;
                            }
                            break;
                        }
                        next = ref next.Next;
                    }
                    if (next == null)
                    {
                        next = member;
                        ++distinctCount;
                        if (operations.BitCount(value) == 1)
                        {
                            allFlags = operations.Or(allFlags, value);
                        }
                    }
                }

                var customValidator = GetEnumValidatorAttribute(enumType);
                var isContiguous = members.Length > 0 && operations.Subtract(members[members.Length - 1].Value, operations.Create(distinctCount - 1)).Equals(members[0].Value);
                var isFlagEnum = enumType.IsDefined(typeof(FlagsAttribute), false);
                return isFlagEnum
                    ? new FlagEnumCache<TUnderlying, TUnderlyingOperations>(enumType, enumBridge, members, buckets, allFlags, distinctCount, isContiguous, customValidator)
                    : isContiguous
                    ? (EnumCache)new ContiguousStandardEnumCache<TUnderlying, TUnderlyingOperations>(enumType, enumBridge, members, buckets, allFlags, distinctCount, customValidator)
                    : new NonContiguousStandardEnumCache<TUnderlying, TUnderlyingOperations>(enumType, enumBridge, members, buckets, allFlags, distinctCount, customValidator);
            }
        }

        internal static ValueCollection<EnumFormat> DefaultFormats
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ValueCollection.Create(EnumFormat.Name, EnumFormat.UnderlyingValue);
        }

        internal static ValueCollection<EnumFormat> NameFormat
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ValueCollection.Create(EnumFormat.Name);
        }

        #region Custom EnumFormat
        private const int s_startingCustomEnumFormatValue = (int)EnumFormat.
#if DISPLAY_ATTRIBUTE
            DisplayName
#else
            EnumMemberValue
#endif
            + 1;

        private static Func<EnumMember, string?>[] s_customEnumMemberFormatters = ArrayHelper.Empty<Func<EnumMember, string?>>();

        /// <summary>
        /// Registers a custom <see cref="EnumFormat"/> with the specified <see cref="EnumMember"/> formatter.
        /// </summary>
        /// <param name="enumMemberFormatter">The <see cref="EnumMember"/> formatter.</param>
        /// <returns>A custom <see cref="EnumFormat"/> that is registered with the specified <see cref="EnumMember"/> formatter.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumMemberFormatter"/> is <c>null</c>.</exception>
        public static EnumFormat RegisterCustomEnumFormat(Func<EnumMember, string?> enumMemberFormatter)
        {
            Preconditions.NotNull(enumMemberFormatter, nameof(enumMemberFormatter));
            
            var customEnumMemberFormatters = s_customEnumMemberFormatters;
            Func<EnumMember, string?>[] oldCustomEnumMemberFormatters;
            do
            {
                oldCustomEnumMemberFormatters = customEnumMemberFormatters;
                customEnumMemberFormatters = new Func<EnumMember, string?>[oldCustomEnumMemberFormatters.Length + 1];
                oldCustomEnumMemberFormatters.CopyTo(customEnumMemberFormatters, 0);
                customEnumMemberFormatters[oldCustomEnumMemberFormatters.Length] = enumMemberFormatter;
            } while ((customEnumMemberFormatters = Interlocked.CompareExchange(ref s_customEnumMemberFormatters, customEnumMemberFormatters, oldCustomEnumMemberFormatters)) != oldCustomEnumMemberFormatters);
            return (EnumFormat)(oldCustomEnumMemberFormatters.Length + s_startingCustomEnumFormatValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool EnumFormatIsValid(EnumFormat format) => (uint)format <= (uint)(s_customEnumMemberFormatters.Length - 1 + s_startingCustomEnumFormatValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string? CustomEnumMemberFormat(EnumMember? member, EnumFormat format) => member != null ? s_customEnumMemberFormatters[(int)format - s_startingCustomEnumFormatValue](member) : null;
        #endregion

        #region Type Methods
        /// <summary>
        /// Retrieves the underlying type of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>The underlying type of <typeparamref name="TEnum"/>.</returns>
        public static Type GetUnderlyingType<TEnum>()
            where TEnum : struct, Enum => Cache<TEnum>.Instance.UnderlyingType;

#if ICONVERTIBLE
        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s underlying type's <see cref="TypeCode"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns><typeparamref name="TEnum"/>'s underlying type's <see cref="TypeCode"/>.</returns>
        public static TypeCode GetTypeCode<TEnum>()
            where TEnum : struct, Enum => Cache<TEnum>.Instance.TypeCode;
#endif

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s member count.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><typeparamref name="TEnum"/>'s member count.</returns>
        /// <exception cref="ArgumentException"><paramref name="selection"/> is an invalid value.</exception>
        public static int GetMemberCount<TEnum>(EnumMemberSelection selection = EnumMemberSelection.All)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.GetMemberCount(selection);

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members in increasing value order.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><typeparamref name="TEnum"/>'s members in increasing value order.</returns>
        /// <exception cref="ArgumentException"><paramref name="selection"/> is an invalid value.</exception>
        public static IReadOnlyList<EnumMember<TEnum>> GetMembers<TEnum>(EnumMemberSelection selection = EnumMemberSelection.All)
            where TEnum : struct, Enum => UnsafeUtility.As<IReadOnlyList<EnumMember<TEnum>>>(Cache<TEnum>.Instance.GetMembers(selection));

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members' names in increasing value order.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><typeparamref name="TEnum"/>'s members' names in increasing value order.</returns>
        /// <exception cref="ArgumentException"><paramref name="selection"/> is an invalid value.</exception>
        public static IReadOnlyList<string> GetNames<TEnum>(EnumMemberSelection selection = EnumMemberSelection.All)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.GetNames(selection);

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members' values in increasing value order.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><typeparamref name="TEnum"/>'s members' values in increasing value order.</returns>
        /// <exception cref="ArgumentException"><paramref name="selection"/> is an invalid value.</exception>
        public static IReadOnlyList<TEnum> GetValues<TEnum>(EnumMemberSelection selection = EnumMemberSelection.All)
            where TEnum : struct, Enum => UnsafeUtility.As<IReadOnlyList<TEnum>>(Cache<TEnum>.Instance.GetValues(selection));
        #endregion

        #region ToObject
        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <typeparamref name="TEnum"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not a valid type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(object value, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            TEnum result = default;
            Cache<TEnum>.Instance.ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(sbyte value, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            TEnum result = default;
            Cache<TEnum>.Instance.ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(byte value, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            TEnum result = default;
            Cache<TEnum>.Instance.ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(short value, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            TEnum result = default;
            Cache<TEnum>.Instance.ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(ushort value, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            TEnum result = default;
            Cache<TEnum>.Instance.ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(int value, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            TEnum result = default;
            Cache<TEnum>.Instance.ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(uint value, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            TEnum result = default;
            Cache<TEnum>.Instance.ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(long value, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            TEnum result = default;
            Cache<TEnum>.Instance.ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(ulong value, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            TEnum result = default;
            Cache<TEnum>.Instance.ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.  The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <typeparamref name="TEnum"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject<TEnum>(object? value, out TEnum result, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            result = default;
            return Cache<TEnum>.Instance.TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.  The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(sbyte value, out TEnum result, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            result = default;
            return Cache<TEnum>.Instance.TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.  The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject<TEnum>(byte value, out TEnum result, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            result = default;
            return Cache<TEnum>.Instance.TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.  The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject<TEnum>(short value, out TEnum result, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            result = default;
            return Cache<TEnum>.Instance.TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.  The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(ushort value, out TEnum result, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            result = default;
            return Cache<TEnum>.Instance.TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.  The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject<TEnum>(int value, out TEnum result, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            result = default;
            return Cache<TEnum>.Instance.TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.  The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(uint value, out TEnum result, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            result = default;
            return Cache<TEnum>.Instance.TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.  The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject<TEnum>(long value, out TEnum result, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            result = default;
            return Cache<TEnum>.Instance.TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.  The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(ulong value, out TEnum result, EnumValidation validation = EnumValidation.None)
            where TEnum : struct, Enum
        {
            result = default;
            return Cache<TEnum>.Instance.TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }
        #endregion

        #region All Values Main Methods
        /// <summary>
        /// Indicates if <paramref name="value"/> is valid using the specified <paramref name="validation"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="validation">The validation to perform on the value.</param>
        /// <returns>Indication if <paramref name="value"/> is valid.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value.</exception>
        public static bool IsValid<TEnum>(this TEnum value, EnumValidation validation = EnumValidation.Default)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.IsValid(ref UnsafeUtility.As<TEnum, byte>(ref value), validation);

        /// <summary>
        /// Indicates if <paramref name="value"/> is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Indication if <paramref name="value"/> is defined.</returns>
        public static bool IsDefined<TEnum>(this TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.IsDefined(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Validates that <paramref name="value"/> is valid using the specified <paramref name="validation"/>.
        /// If it's not it throws an <see cref="ArgumentException"/> with the specified <paramref name="paramName"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="paramName">The parameter name to be used if throwing an <see cref="ArgumentException"/>.</param>
        /// <param name="validation">The validation to perform on the value.</param>
        /// <returns><paramref name="value"/> for use in fluent API's and base constructor method calls.</returns>
        /// <exception cref="ArgumentException"><paramref name="validation"/> is an invalid value
        /// -or-
        /// <paramref name="value"/> is invalid.</exception>
        public static TEnum Validate<TEnum>(this TEnum value, string paramName, EnumValidation validation = EnumValidation.Default)
            where TEnum : struct, Enum
        {
            Cache<TEnum>.Instance.Validate(ref UnsafeUtility.As<TEnum, byte>(ref value), paramName, validation);
            return value;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        public static string AsString<TEnum>(this TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.AsString(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        public static string AsString<TEnum>(this TEnum value, string? format)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.AsString(ref UnsafeUtility.As<TEnum, byte>(ref value), string.IsNullOrEmpty(format) ? "G" : format!);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="format"/> is an invalid value.</exception>
        public static string? AsString<TEnum>(this TEnum value, EnumFormat format)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.AsString(ref UnsafeUtility.As<TEnum, byte>(ref value), format);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use if using the first resolves to <c>null</c>.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static string? AsString<TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.AsString(ref UnsafeUtility.As<TEnum, byte>(ref value), ValueCollection.Create(format0, format1));

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use if using the first resolves to <c>null</c>.</param>
        /// <param name="format2">The third output format to use if using the first and second both resolve to <c>null</c>.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static string? AsString<TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.AsString(ref UnsafeUtility.As<TEnum, byte>(ref value), ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="formats"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="formats">The output formats to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="formats"/> contains an invalid value.</exception>
        public static string? AsString<TEnum>(this TEnum value, params EnumFormat[]? formats)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.AsString(ref UnsafeUtility.As<TEnum, byte>(ref value), formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);

#if SPAN
        public static bool TryFormat<TEnum>(this TEnum value, Span<char> destination, out int charsWritten)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.TryFormat(ref UnsafeUtility.As<TEnum, byte>(ref value), destination, out charsWritten);

        public static bool TryFormat<TEnum>(this TEnum value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.TryFormat(ref UnsafeUtility.As<TEnum, byte>(ref value), destination, out charsWritten, format);

        public static bool TryFormat<TEnum>(this TEnum value, Span<char> destination, out int charsWritten, params EnumFormat[]? formats)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.TryFormat(ref UnsafeUtility.As<TEnum, byte>(ref value), destination, out charsWritten, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);
#endif

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        public static string Format<TEnum>(TEnum value, string format)
            where TEnum : struct, Enum
        {
            Preconditions.NotNull(format, nameof(format));

            return Cache<TEnum>.Instance.AsString(ref UnsafeUtility.As<TEnum, byte>(ref value), format);
        }

        /// <summary>
        /// Returns <paramref name="value"/>'s underlying integral value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/>'s underlying integral value.</returns>
        public static object GetUnderlyingValue<TEnum>(TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.GetUnderlyingValue(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="sbyte"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="sbyte"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="sbyte"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static sbyte ToSByte<TEnum>(TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.ToSByte(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="byte"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="byte"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="byte"/>'s value range without overflowing.</exception>
        public static byte ToByte<TEnum>(TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.ToByte(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="short"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="short"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="short"/>'s value range without overflowing.</exception>
        public static short ToInt16<TEnum>(TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.ToInt16(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="ushort"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="ushort"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ushort"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static ushort ToUInt16<TEnum>(TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.ToUInt16(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="int"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="int"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="int"/>'s value range without overflowing.</exception>
        public static int ToInt32<TEnum>(TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.ToInt32(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="uint"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="uint"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="uint"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static uint ToUInt32<TEnum>(TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.ToUInt32(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="long"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="long"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="long"/>'s value range without overflowing.</exception>
        public static long ToInt64<TEnum>(TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.ToInt64(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="ulong"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="ulong"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ulong"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static ulong ToUInt64<TEnum>(TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.ToUInt64(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Retrieves the hash code of <paramref name="value"/>. It's more efficient as it doesn't require boxing and unboxing of <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Hash code of <paramref name="value"/>.</returns>
        public static int GetHashCode<TEnum>(TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.GetHashCode(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Indicates if <paramref name="value"/> equals <paramref name="other"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="other">The other enum value.</param>
        /// <returns>Indication if <paramref name="value"/> equals <paramref name="other"/>.</returns>
        public static bool Equals<TEnum>(this TEnum value, TEnum other)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.Equals(ref UnsafeUtility.As<TEnum, byte>(ref value), ref UnsafeUtility.As<TEnum, byte>(ref other));

        /// <summary>
        /// Compares <paramref name="value"/> to <paramref name="other"/> for ordering.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="other">The other enum value.</param>
        /// <returns>1 if <paramref name="value"/> is greater than <paramref name="other"/>, 0 if <paramref name="value"/> equals <paramref name="other"/>,
        /// and -1 if <paramref name="value"/> is less than <paramref name="other"/>.</returns>
        public static int CompareTo<TEnum>(this TEnum value, TEnum other)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.CompareTo(ref UnsafeUtility.As<TEnum, byte>(ref value), ref UnsafeUtility.As<TEnum, byte>(ref other));
        #endregion

        #region Defined Values Main Methods
        /// <summary>
        /// Retrieves <paramref name="value"/>'s enum member name if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/>'s enum member name if defined otherwise <c>null</c>.</returns>
        public static string? GetName<TEnum>(this TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.GetMember(ref UnsafeUtility.As<TEnum, byte>(ref value))?.Name;

        /// <summary>
        /// Retrieves <paramref name="value"/>'s enum member attributes if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/>'s enum member attributes if defined otherwise <c>null</c>.</returns>
        public static AttributeCollection? GetAttributes<TEnum>(this TEnum value)
            where TEnum : struct, Enum => Cache<TEnum>.Instance.GetMember(ref UnsafeUtility.As<TEnum, byte>(ref value))?.Attributes;

        /// <summary>
        /// Retrieves an enum member with the specified <paramref name="value"/> if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Enum member with the specified <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        public static EnumMember<TEnum>? GetMember<TEnum>(this TEnum value)
            where TEnum : struct, Enum => UnsafeUtility.As<EnumMember<TEnum>>(Cache<TEnum>.Instance.GetMember(ref UnsafeUtility.As<TEnum, byte>(ref value))?.EnumMember);

        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="name">The enum member name.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        public static EnumMember<TEnum>? GetMember<TEnum>(string name)
            where TEnum : struct, Enum => GetMember<TEnum>(name, false);

        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="name">The enum member name.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        public static EnumMember<TEnum>? GetMember<TEnum>(string name, bool ignoreCase)
            where TEnum : struct, Enum
        {
            Preconditions.NotNull(name, nameof(name));

            return UnsafeUtility.As<EnumMember<TEnum>>(Cache<TEnum>.Instance.GetMember(name, ignoreCase, NameFormat));
        }

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="format"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="format"/> is an invalid value.</exception>
        public static EnumMember<TEnum>? GetMember<TEnum>(string value, bool ignoreCase, EnumFormat format)
            where TEnum : struct, Enum => GetMember<TEnum>(value, ignoreCase, ValueCollection.Create(format));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static EnumMember<TEnum>? GetMember<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1)
            where TEnum : struct, Enum => GetMember<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static EnumMember<TEnum>? GetMember<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2)
            where TEnum : struct, Enum => GetMember<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="formats"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="formats"/> contains an invalid value.</exception>
        public static EnumMember<TEnum>? GetMember<TEnum>(string value, bool ignoreCase, params EnumFormat[]? formats)
            where TEnum : struct, Enum => GetMember<TEnum>(value, ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : Enums.NameFormat);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static EnumMember<TEnum>? GetMember<TEnum>(string value, bool ignoreCase, ValueCollection<EnumFormat> formats)
            where TEnum : struct, Enum
        {
            Preconditions.NotNull(value, nameof(value));

            return UnsafeUtility.As<EnumMember<TEnum>>(Cache<TEnum>.Instance.GetMember(value, ignoreCase, formats));
        }

#if SPAN
        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="name">The enum member name.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        public static EnumMember<TEnum>? GetMember<TEnum>(ReadOnlySpan<char> name, bool ignoreCase = false)
            where TEnum : struct, Enum => GetMember<TEnum>(name, ignoreCase, NameFormat);

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="format"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="format"/> is an invalid value.</exception>
        public static EnumMember<TEnum>? GetMember<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format)
            where TEnum : struct, Enum => GetMember<TEnum>(value, ignoreCase, ValueCollection.Create(format));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static EnumMember<TEnum>? GetMember<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1)
            where TEnum : struct, Enum => GetMember<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static EnumMember<TEnum>? GetMember<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2)
            where TEnum : struct, Enum => GetMember<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="formats"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="formats"/> contains an invalid value.</exception>
        public static EnumMember<TEnum>? GetMember<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, params EnumFormat[]? formats)
            where TEnum : struct, Enum => GetMember<TEnum>(value, ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : NameFormat);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static EnumMember<TEnum>? GetMember<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, ValueCollection<EnumFormat> formats)
            where TEnum : struct, Enum => UnsafeUtility.As<EnumMember<TEnum>>(Cache<TEnum>.Instance.GetMember(value, ignoreCase, formats));
#endif
        #endregion

        #region Parsing
        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value)
            where TEnum : struct, Enum => Parse<TEnum>(value, false);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase)
            where TEnum : struct, Enum
        {
            Preconditions.NotNull(value, nameof(value));

            TEnum result = default;
            Cache<TEnum>.Instance.Parse(value, ignoreCase, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase, EnumFormat format)
            where TEnum : struct, Enum => Parse<TEnum>(value, ignoreCase, ValueCollection.Create(format));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1)
            where TEnum : struct, Enum => Parse<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2)
            where TEnum : struct, Enum => Parse<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase, params EnumFormat[]? formats)
            where TEnum : struct, Enum => Parse<TEnum>(value, ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TEnum Parse<TEnum>(string value, bool ignoreCase, ValueCollection<EnumFormat> formats)
            where TEnum : struct, Enum
        {
            Preconditions.NotNull(value, nameof(value));

            TEnum result = default;
            Cache<TEnum>.Instance.Parse(value, ignoreCase, formats, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

#if SPAN
        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase = false)
            where TEnum : struct, Enum
        {
            TEnum result = default;
            Cache<TEnum>.Instance.Parse(value, ignoreCase, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format)
            where TEnum : struct, Enum => ParseInternal<TEnum>(value, ignoreCase, ValueCollection.Create(format));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1)
            where TEnum : struct, Enum => ParseInternal<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2)
            where TEnum : struct, Enum => ParseInternal<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, params EnumFormat[]? formats)
            where TEnum : struct, Enum => ParseInternal<TEnum>(value, ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TEnum ParseInternal<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, ValueCollection<EnumFormat> formats = default)
            where TEnum : struct, Enum
        {
            TEnum result = default;
            Cache<TEnum>.Instance.Parse(value, ignoreCase, formats, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }
#endif

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        public static bool TryParse<TEnum>(string? value, out TEnum result)
            where TEnum : struct, Enum => TryParse(value, false, out result);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        public static bool TryParse<TEnum>(string? value, bool ignoreCase, out TEnum result)
            where TEnum : struct, Enum
        {
            result = default;
            return Cache<TEnum>.Instance.TryParse(value, ignoreCase, ref UnsafeUtility.As<TEnum, byte>(ref result));
        }

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="format"/> is an invalid value.</exception>
        public static bool TryParse<TEnum>(string? value, bool ignoreCase, out TEnum result, EnumFormat format)
            where TEnum : struct, Enum => TryParse(value, ignoreCase, out result, ValueCollection.Create(format));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParse<TEnum>(string? value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1)
            where TEnum : struct, Enum => TryParse(value, ignoreCase, out result, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParse<TEnum>(string? value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2)
            where TEnum : struct, Enum => TryParse(value, ignoreCase, out result, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParse<TEnum>(string? value, bool ignoreCase, out TEnum result, params EnumFormat[]? formats)
            where TEnum : struct, Enum => TryParse(value, ignoreCase, out result, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);

#if SPAN
        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        public static bool TryParse<TEnum>(ReadOnlySpan<char> value, out TEnum result)
            where TEnum : struct, Enum => TryParse(value, false, out result);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        public static bool TryParse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result)
            where TEnum : struct, Enum
        {
            result = default;
            return Cache<TEnum>.Instance.TryParse(value, ignoreCase, ref UnsafeUtility.As<TEnum, byte>(ref result));
        }

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="format"/> is an invalid value.</exception>
        public static bool TryParse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result, EnumFormat format)
            where TEnum : struct, Enum => TryParse(value, ignoreCase, out result, ValueCollection.Create(format));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1)
            where TEnum : struct, Enum => TryParse(value, ignoreCase, out result, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2)
            where TEnum : struct, Enum => TryParse(value, ignoreCase, out result, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result, params EnumFormat[]? formats)
            where TEnum : struct, Enum => TryParse(value, ignoreCase, out result, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryParse<TEnum>(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, out TEnum result, ValueCollection<EnumFormat> formats)
            where TEnum : struct, Enum
        {
            result = default;
            return Cache<TEnum>.Instance.TryParse(value, ignoreCase, ref UnsafeUtility.As<TEnum, byte>(ref result), formats);
        }
        #endregion

        #region Unsafe
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static EnumCache GetCacheUnsafe<TEnum>() => UnsafeCache<TEnum>.Instance ?? throw new ArgumentException("Type argument TEnum must be an enum");

        internal static class UnsafeCache<TEnum>
        {
            public static readonly EnumCache? Instance = GetEnumCache(typeof(TEnum));
        }

        #region Type Methods
        /// <summary>
        /// Retrieves the underlying type of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>The underlying type of <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static Type GetUnderlyingTypeUnsafe<TEnum>() => GetCacheUnsafe<TEnum>().UnderlyingType;

#if ICONVERTIBLE
        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s underlying type's <see cref="TypeCode"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns><typeparamref name="TEnum"/>'s underlying type's <see cref="TypeCode"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TypeCode GetTypeCodeUnsafe<TEnum>() => GetCacheUnsafe<TEnum>().TypeCode;
#endif

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s member count.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><typeparamref name="TEnum"/>'s member count.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="selection"/> is an invalid value.</exception>
        public static int GetMemberCountUnsafe<TEnum>(EnumMemberSelection selection = EnumMemberSelection.All) => GetCacheUnsafe<TEnum>().GetMemberCount(selection);

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members in increasing value order.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><typeparamref name="TEnum"/>'s members in increasing value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="selection"/> is an invalid value.</exception>
        public static IReadOnlyList<EnumMember<TEnum>> GetMembersUnsafe<TEnum>(EnumMemberSelection selection = EnumMemberSelection.All) => UnsafeUtility.As<IReadOnlyList<EnumMember<TEnum>>>(GetCacheUnsafe<TEnum>().GetMembers(selection));

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members' names in increasing value order.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><typeparamref name="TEnum"/>'s members' names in increasing value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="selection"/> is an invalid value.</exception>
        public static IReadOnlyList<string> GetNamesUnsafe<TEnum>(EnumMemberSelection selection = EnumMemberSelection.All) => GetCacheUnsafe<TEnum>().GetNames(selection);

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members' values in increasing value order.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><typeparamref name="TEnum"/>'s members' values in increasing value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="selection"/> is an invalid value.</exception>
        public static IReadOnlyList<TEnum> GetValuesUnsafe<TEnum>(EnumMemberSelection selection = EnumMemberSelection.All) => UnsafeUtility.As<IReadOnlyList<TEnum>>(GetCacheUnsafe<TEnum>().GetValues(selection));
        #endregion

        #region ToObject
        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <typeparamref name="TEnum"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not a valid type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObjectUnsafe<TEnum>(object value, EnumValidation validation = EnumValidation.None)
        {
            TEnum result = default!;
            GetCacheUnsafe<TEnum>().ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObjectUnsafe<TEnum>(sbyte value, EnumValidation validation = EnumValidation.None)
        {
            TEnum result = default!;
            GetCacheUnsafe<TEnum>().ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObjectUnsafe<TEnum>(byte value, EnumValidation validation = EnumValidation.None)
        {
            TEnum result = default!;
            GetCacheUnsafe<TEnum>().ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObjectUnsafe<TEnum>(short value, EnumValidation validation = EnumValidation.None)
        {
            TEnum result = default!;
            GetCacheUnsafe<TEnum>().ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObjectUnsafe<TEnum>(ushort value, EnumValidation validation = EnumValidation.None)
        {
            TEnum result = default!;
            GetCacheUnsafe<TEnum>().ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObjectUnsafe<TEnum>(int value, EnumValidation validation = EnumValidation.None)
        {
            TEnum result = default!;
            GetCacheUnsafe<TEnum>().ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObjectUnsafe<TEnum>(uint value, EnumValidation validation = EnumValidation.None)
        {
            TEnum result = default!;
            GetCacheUnsafe<TEnum>().ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObjectUnsafe<TEnum>(long value, EnumValidation validation = EnumValidation.None)
        {
            TEnum result = default!;
            GetCacheUnsafe<TEnum>().ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObjectUnsafe<TEnum>(ulong value, EnumValidation validation = EnumValidation.None)
        {
            TEnum result = default!;
            GetCacheUnsafe<TEnum>().ToObject(value, validation, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <typeparamref name="TEnum"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObjectUnsafe<TEnum>(object? value, out TEnum result, EnumValidation validation = EnumValidation.None)
        {
            result = default!;
            return GetCacheUnsafe<TEnum>().TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObjectUnsafe<TEnum>(sbyte value, out TEnum result, EnumValidation validation = EnumValidation.None)
        {
            result = default!;
            return GetCacheUnsafe<TEnum>().TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObjectUnsafe<TEnum>(byte value, out TEnum result, EnumValidation validation = EnumValidation.None)
        {
            result = default!;
            return GetCacheUnsafe<TEnum>().TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObjectUnsafe<TEnum>(short value, out TEnum result, EnumValidation validation = EnumValidation.None)
        {
            result = default!;
            return GetCacheUnsafe<TEnum>().TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObjectUnsafe<TEnum>(ushort value, out TEnum result, EnumValidation validation = EnumValidation.None)
        {
            result = default!;
            return GetCacheUnsafe<TEnum>().TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObjectUnsafe<TEnum>(int value, out TEnum result, EnumValidation validation = EnumValidation.None)
        {
            result = default!;
            return GetCacheUnsafe<TEnum>().TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObjectUnsafe<TEnum>(uint value, out TEnum result, EnumValidation validation = EnumValidation.None)
        {
            result = default!;
            return GetCacheUnsafe<TEnum>().TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObjectUnsafe<TEnum>(long value, out TEnum result, EnumValidation validation = EnumValidation.None)
        {
            result = default!;
            return GetCacheUnsafe<TEnum>().TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObjectUnsafe<TEnum>(ulong value, out TEnum result, EnumValidation validation = EnumValidation.None)
        {
            result = default!;
            return GetCacheUnsafe<TEnum>().TryToObject(value, ref UnsafeUtility.As<TEnum, byte>(ref result), validation);
        }
        #endregion

        #region All Values Main Methods
        /// <summary>
        /// Indicates if <paramref name="value"/> is valid using the specified <paramref name="validation"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="validation">The validation to perform on the value.</param>
        /// <returns>Indication if <paramref name="value"/> is valid.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool IsValidUnsafe<TEnum>(TEnum value, EnumValidation validation = EnumValidation.Default) => GetCacheUnsafe<TEnum>().IsValid(ref UnsafeUtility.As<TEnum, byte>(ref value), validation);

        /// <summary>
        /// Indicates if <paramref name="value"/> is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Indication if <paramref name="value"/> is defined.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool IsDefinedUnsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().IsDefined(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Validates that <paramref name="value"/> is valid using the specified <paramref name="validation"/>.
        /// If it's not it throws an <see cref="ArgumentException"/> with the specified <paramref name="paramName"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="paramName">The parameter name to be used if throwing an <see cref="ArgumentException"/>.</param>
        /// <param name="validation">The validation to perform on the value.</param>
        /// <returns><paramref name="value"/> for use in fluent API's and base constructor method calls.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// <paramref name="value"/> is invalid.</exception>
        public static TEnum ValidateUnsafe<TEnum>(TEnum value, string paramName, EnumValidation validation = EnumValidation.Default)
        {
            GetCacheUnsafe<TEnum>().Validate(ref UnsafeUtility.As<TEnum, byte>(ref value), paramName, validation);
            return value;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static string AsStringUnsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().AsString(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        public static string AsStringUnsafe<TEnum>(TEnum value, string? format) => GetCacheUnsafe<TEnum>().AsString(ref UnsafeUtility.As<TEnum, byte>(ref value), string.IsNullOrEmpty(format) ? "G" : format!);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static string? AsStringUnsafe<TEnum>(TEnum value, EnumFormat format) => GetCacheUnsafe<TEnum>().AsString(ref UnsafeUtility.As<TEnum, byte>(ref value), format);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use if using the first resolves to <c>null</c>.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static string? AsStringUnsafe<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1) => GetCacheUnsafe<TEnum>().AsString(ref UnsafeUtility.As<TEnum, byte>(ref value), ValueCollection.Create(format0, format1));

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use if using the first resolves to <c>null</c>.</param>
        /// <param name="format2">The third output format to use if using the first and second both resolve to <c>null</c>.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static string? AsStringUnsafe<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetCacheUnsafe<TEnum>().AsString(ref UnsafeUtility.As<TEnum, byte>(ref value), ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="formats"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="formats">The output formats to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static string? AsStringUnsafe<TEnum>(TEnum value, params EnumFormat[]? formats) => GetCacheUnsafe<TEnum>().AsString(ref UnsafeUtility.As<TEnum, byte>(ref value), formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);

#if SPAN
        public static bool TryFormatUnsafe<TEnum>(this TEnum value, Span<char> destination, out int charsWritten) => GetCacheUnsafe<TEnum>().TryFormat(ref UnsafeUtility.As<TEnum, byte>(ref value), destination, out charsWritten);

        public static bool TryFormatUnsafe<TEnum>(this TEnum value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default) => GetCacheUnsafe<TEnum>().TryFormat(ref UnsafeUtility.As<TEnum, byte>(ref value), destination, out charsWritten, format);

        public static bool TryFormatUnsafe<TEnum>(this TEnum value, Span<char> destination, out int charsWritten, params EnumFormat[]? formats) => GetCacheUnsafe<TEnum>().TryFormat(ref UnsafeUtility.As<TEnum, byte>(ref value), destination, out charsWritten, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);
#endif

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        public static string FormatUnsafe<TEnum>(TEnum value, string format)
        {
            Preconditions.NotNull(format, nameof(format));

            return GetCacheUnsafe<TEnum>().AsString(ref UnsafeUtility.As<TEnum, byte>(ref value), format);
        }

        /// <summary>
        /// Returns <paramref name="value"/>'s underlying integral value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/>'s underlying integral value.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static object GetUnderlyingValueUnsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().GetUnderlyingValue(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="sbyte"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="sbyte"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="sbyte"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static sbyte ToSByteUnsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().ToSByte(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="byte"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="byte"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="byte"/>'s value range without overflowing.</exception>
        public static byte ToByteUnsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().ToByte(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="short"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="short"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="short"/>'s value range without overflowing.</exception>
        public static short ToInt16Unsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().ToInt16(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="ushort"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="ushort"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ushort"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static ushort ToUInt16Unsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().ToUInt16(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="int"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="int"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="int"/>'s value range without overflowing.</exception>
        public static int ToInt32Unsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().ToInt32(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="uint"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="uint"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="uint"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static uint ToUInt32Unsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().ToUInt32(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="long"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="long"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="long"/>'s value range without overflowing.</exception>
        public static long ToInt64Unsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().ToInt64(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="ulong"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="ulong"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ulong"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static ulong ToUInt64Unsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().ToUInt64(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Retrieves the hash code of <paramref name="value"/>. It's more efficient as it doesn't require boxing and unboxing of <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Hash code of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static int GetHashCodeUnsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().GetHashCode(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Indicates if <paramref name="value"/> equals <paramref name="other"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="other">The other enum value.</param>
        /// <returns>Indication if <paramref name="value"/> equals <paramref name="other"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool EqualsUnsafe<TEnum>(TEnum value, TEnum other) => GetCacheUnsafe<TEnum>().Equals(ref UnsafeUtility.As<TEnum, byte>(ref value), ref UnsafeUtility.As<TEnum, byte>(ref other));

        /// <summary>
        /// Compares <paramref name="value"/> to <paramref name="other"/> for ordering.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="other">The other enum value.</param>
        /// <returns>1 if <paramref name="value"/> is greater than <paramref name="other"/>, 0 if <paramref name="value"/> equals <paramref name="other"/>,
        /// and -1 if <paramref name="value"/> is less than <paramref name="other"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static int CompareToUnsafe<TEnum>(TEnum value, TEnum other) => GetCacheUnsafe<TEnum>().CompareTo(ref UnsafeUtility.As<TEnum, byte>(ref value), ref UnsafeUtility.As<TEnum, byte>(ref other));
        #endregion

        #region Defined Values Main Methods
        /// <summary>
        /// Retrieves <paramref name="value"/>'s enum member name if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/>'s enum member name if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static string? GetNameUnsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().GetMember(ref UnsafeUtility.As<TEnum, byte>(ref value))?.Name;

        /// <summary>
        /// Retrieves <paramref name="value"/>'s enum member attributes if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/>'s enum member attributes if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static AttributeCollection? GetAttributesUnsafe<TEnum>(TEnum value) => GetCacheUnsafe<TEnum>().GetMember(ref UnsafeUtility.As<TEnum, byte>(ref value))?.Attributes;

        /// <summary>
        /// Retrieves an enum member with the specified <paramref name="value"/> if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Enum member with the specified <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(TEnum value) => UnsafeUtility.As<EnumMember<TEnum>>(GetCacheUnsafe<TEnum>().GetMember(ref UnsafeUtility.As<TEnum, byte>(ref value))?.EnumMember);

        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// Is case-sensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="name">The enum member name.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(string name) => GetMemberUnsafe<TEnum>(name, false);

        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="name">The enum member name.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(string name, bool ignoreCase)
        {
            Preconditions.NotNull(name, nameof(name));

            return UnsafeUtility.As<EnumMember<TEnum>>(GetCacheUnsafe<TEnum>().GetMember(name, ignoreCase, NameFormat));
        }

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="format"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(string value, bool ignoreCase, EnumFormat format) => GetMemberUnsafe<TEnum>(value, ignoreCase, ValueCollection.Create(format));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => GetMemberUnsafe<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetMemberUnsafe<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="formats"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(string value, bool ignoreCase, params EnumFormat[]? formats) => GetMemberUnsafe<TEnum>(value, ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : NameFormat);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(string value, bool ignoreCase, ValueCollection<EnumFormat> formats)
        {
            Preconditions.NotNull(value, nameof(value));

            return UnsafeUtility.As<EnumMember<TEnum>>(GetCacheUnsafe<TEnum>().GetMember(value, ignoreCase, formats));
        }

#if SPAN
        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="name">The enum member name.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(ReadOnlySpan<char> name, bool ignoreCase = false) => GetMemberUnsafe<TEnum>(name, ignoreCase, DefaultFormats);

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="format"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format) => GetMemberUnsafe<TEnum>(value, ignoreCase, ValueCollection.Create(format));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => GetMemberUnsafe<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetMemberUnsafe<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="formats"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, params EnumFormat[]? formats) => GetMemberUnsafe<TEnum>(value, ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : Enums.NameFormat);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static EnumMember<TEnum>? GetMemberUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, ValueCollection<EnumFormat> formats) => UnsafeUtility.As<EnumMember<TEnum>>(GetCacheUnsafe<TEnum>().GetMember(value, ignoreCase, formats));
#endif
        #endregion

        #region Parsing
        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <typeparamref name="TEnum"/>'s underlying type.</exception>
        public static TEnum ParseUnsafe<TEnum>(string value) => ParseUnsafe<TEnum>(value, false);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseUnsafe<TEnum>(string value, bool ignoreCase)
        {
            Preconditions.NotNull(value, nameof(value));

            TEnum result = default!;
            GetCacheUnsafe<TEnum>().Parse(value, ignoreCase, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseUnsafe<TEnum>(string value, bool ignoreCase, EnumFormat format) => ParseUnsafe<TEnum>(value, ignoreCase, ValueCollection.Create(format));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseUnsafe<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => ParseUnsafe<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseUnsafe<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseUnsafe<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseUnsafe<TEnum>(string value, bool ignoreCase, params EnumFormat[]? formats) => ParseUnsafe<TEnum>(value, ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TEnum ParseUnsafe<TEnum>(string value, bool ignoreCase, ValueCollection<EnumFormat> formats)
        {
            Preconditions.NotNull(value, nameof(value));

            TEnum result = default!;
            GetCacheUnsafe<TEnum>().Parse(value, ignoreCase, formats, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

#if SPAN
        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase = false)
        {
            TEnum result = default!;
            GetCacheUnsafe<TEnum>().Parse(value, ignoreCase, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format) => ParseUnsafe<TEnum>(value, ignoreCase, ValueCollection.Create(format));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => ParseUnsafe<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseUnsafe<TEnum>(value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, params EnumFormat[]? formats) => ParseUnsafe<TEnum>(value,  ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TEnum ParseUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, ValueCollection<EnumFormat> formats)
        {
            TEnum result = default!;
            GetCacheUnsafe<TEnum>().Parse(value, ignoreCase, formats, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }
#endif

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParseUnsafe<TEnum>(string? value, out TEnum result) => TryParseUnsafe(value, false, out result);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParseUnsafe<TEnum>(string? value, bool ignoreCase, out TEnum result)
        {
            result = default!;
            return GetCacheUnsafe<TEnum>().TryParse(value, ignoreCase, ref UnsafeUtility.As<TEnum, byte>(ref result));
        }

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseUnsafe<TEnum>(string? value, bool ignoreCase, out TEnum result, EnumFormat format) => TryParseUnsafe(value, ignoreCase, out result, ValueCollection.Create(format));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseUnsafe<TEnum>(string? value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1) => TryParseUnsafe(value, ignoreCase, out result, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseUnsafe<TEnum>(string? value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseUnsafe(value, ignoreCase, out result, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseUnsafe<TEnum>(string? value, bool ignoreCase, out TEnum result, params EnumFormat[]? formats) => TryParseUnsafe(value, ignoreCase, out result, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);

#if SPAN
        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParseUnsafe<TEnum>(ReadOnlySpan<char> value, out TEnum result) => TryParseUnsafe(value, false, out result);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParseUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result)
        {
            result = default!;
            return GetCacheUnsafe<TEnum>().TryParse(value, ignoreCase, ref UnsafeUtility.As<TEnum, byte>(ref result));
        }

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result, EnumFormat format) => TryParseUnsafe(value, ignoreCase, out result, ValueCollection.Create(format));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1) => TryParseUnsafe(value, ignoreCase, out result, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseUnsafe(value, ignoreCase, out result, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseUnsafe<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result, params EnumFormat[]? formats) => TryParseUnsafe(value, ignoreCase, out result, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryParseUnsafe<TEnum>(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, out TEnum result, ValueCollection<EnumFormat> formats)
        {
            result = default!;
            return GetCacheUnsafe<TEnum>().TryParse(value, ignoreCase, ref UnsafeUtility.As<TEnum, byte>(ref result), formats);
        }
        #endregion
        #endregion

        #region NonGeneric
        private static EnumCache?[] s_enumCacheBuckets = new EnumCache?[4];
        private static readonly object s_lockObject = new object();
        private static int s_enumCacheCount;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static EnumCache GetCache(Type enumType)
        {
            Preconditions.NotNull(enumType, nameof(enumType));

            var buckets = s_enumCacheBuckets;
            var cache = buckets[enumType.GetHashCode() & (buckets.Length - 1)];
            while (cache != null)
            {
                if (cache.EnumType.Equals(enumType))
                {
                    return cache;
                }
                cache = cache.Next;
            }
            return GetOrAddCache(enumType);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static EnumCache GetOrAddCache(Type enumType)
        {
            lock (s_lockObject)
            {
                var buckets = s_enumCacheBuckets;
                ref var next = ref buckets[enumType.GetHashCode() & (buckets.Length - 1)];
                var n = next;
                while (n != null)
                {
                    if (n.EnumType.Equals(enumType))
                    {
                        return n;
                    }
                    n = n.Next;
                }
                var enumCacheCount = s_enumCacheCount;
                if (buckets.Length == enumCacheCount)
                {
                    var newSize = enumCacheCount << 1;
                    var newBuckets = new EnumCache?[newSize];
                    for (var i = 0; i < buckets.Length; ++i)
                    {
                        var c = buckets[i];
                        while (c != null)
                        {
                            var nextItem = c.Next;
                            ref var b = ref newBuckets[c.EnumType.GetHashCode() & (newSize - 1)];
                            c.Next = b;
                            b = c;
                            c = nextItem;
                        }
                    }
                    s_enumCacheBuckets = newBuckets;
                    next = ref newBuckets[enumType.GetHashCode() & (newBuckets.Length - 1)];
                }
                var cache = GetEnumCache(enumType) ?? throw new ArgumentException("must be an enum type", nameof(enumType));
                cache.Next = next;
                next = cache;
                ++s_enumCacheCount;
                return cache;
            }
        }

        #region Type Methods
        /// <summary>
        /// Retrieves the underlying type of <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns>The underlying type of <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static Type GetUnderlyingType(Type enumType) => GetCache(enumType).UnderlyingType;

#if ICONVERTIBLE
        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s underlying type's <see cref="TypeCode"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns><paramref name="enumType"/>'s underlying type's <see cref="TypeCode"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static TypeCode GetTypeCode(Type enumType) => GetCache(enumType).TypeCode;
#endif

        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s member count.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><paramref name="enumType"/>'s member count.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="selection"/> is an invalid value.</exception>
        public static int GetMemberCount(Type enumType, EnumMemberSelection selection = EnumMemberSelection.All) => GetCache(enumType).GetMemberCount(selection);

        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s members in increasing value order.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><paramref name="enumType"/>'s members in increasing value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="selection"/> is an invalid value.</exception>
        public static IReadOnlyList<EnumMember> GetMembers(Type enumType, EnumMemberSelection selection = EnumMemberSelection.All) => GetCache(enumType).GetMembers(selection);

        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s members' names in increasing value order.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><paramref name="enumType"/>'s members' names in increasing value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="selection"/> is an invalid value.</exception>
        public static IReadOnlyList<string> GetNames(Type enumType, EnumMemberSelection selection = EnumMemberSelection.All) => GetCache(enumType).GetNames(selection);

        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s members' values in increasing value order.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><paramref name="enumType"/>'s members' values in increasing value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="selection"/> is an invalid value.</exception>
        public static IReadOnlyList<object> GetValues(Type enumType, EnumMemberSelection selection = EnumMemberSelection.All) => GetCache(enumType).GetValues(selection).GetNonGenericContainer();
        #endregion

        #region ToObject
        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <paramref name="enumType"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not a valid type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, object value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, sbyte value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, byte value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, short value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ushort value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, int value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, uint value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, long value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ulong value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <paramref name="enumType"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject(Type enumType, object? value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, sbyte value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject(Type enumType, byte value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject(Type enumType, short value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ushort value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject(Type enumType, int value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, uint value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject(Type enumType, long value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ulong value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);
        #endregion

        #region All Values Main Methods
        /// <summary>
        /// Indicates if <paramref name="value"/> is valid. If <paramref name="enumType"/> is a standard enum it returns whether the value is defined.
        /// If <paramref name="enumType"/> is marked with <see cref="FlagsAttribute"/> it returns whether it's a valid flag combination of <paramref name="enumType"/>'s defined values
        /// or is defined. Or if <paramref name="enumType"/> has an attribute that implements <see cref="IEnumValidatorAttribute{TEnum}"/>
        /// then that attribute's <see cref="IEnumValidatorAttribute{TEnum}.IsValid(TEnum)"/> method is used.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="validation">The validation to perform on the value.</param>
        /// <returns>Indication if <paramref name="value"/> is valid.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool IsValid(Type enumType, object value, EnumValidation validation = EnumValidation.Default) => GetCache(enumType).IsValid(value, validation);

        /// <summary>
        /// Indicates if <paramref name="value"/> is defined.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns>Indication if <paramref name="value"/> is defined.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static bool IsDefined(Type enumType, object value) => GetCache(enumType).IsDefined(value);

        /// <summary>
        /// Validates that <paramref name="value"/> is valid. If it's not it throws an <see cref="ArgumentException"/> with the specified <paramref name="paramName"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="paramName">The parameter name to be used if throwing an <see cref="ArgumentException"/>.</param>
        /// <param name="validation">The validation to perform on the value.</param>
        /// <returns><paramref name="value"/> for use in fluent API's and base constructor method calls.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// <paramref name="value"/> is invalid.</exception>
        public static object Validate(Type enumType, object value, string paramName, EnumValidation validation = EnumValidation.Default) => GetCache(enumType).Validate(value, paramName, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static string AsString(Type enumType, object value) => GetCache(enumType).AsString(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        public static string AsString(Type enumType, object value, string? format) => GetCache(enumType).AsString(value, string.IsNullOrEmpty(format) ? "G" : format!);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static string? AsString(Type enumType, object value, EnumFormat format) => GetCache(enumType).AsString(value, format);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use if using the first resolves to <c>null</c>.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static string? AsString(Type enumType, object value, EnumFormat format0, EnumFormat format1) => GetCache(enumType).AsString(value, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use if using the first resolves to <c>null</c>.</param>
        /// <param name="format2">The third output format to use if using the first and second both resolve to <c>null</c>.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static string? AsString(Type enumType, object value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetCache(enumType).AsString(value, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="formats"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="formats">The output formats to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static string? AsString(Type enumType, object value, params EnumFormat[]? formats) => GetCache(enumType).AsString(value, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);

#if SPAN
        public static bool TryFormat(Type enumType, object value, Span<char> destination, out int charsWritten) => GetCache(enumType).TryFormat(value, destination, out charsWritten);

        public static bool TryFormat(Type enumType, object value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default) => GetCache(enumType).TryFormat(value, destination, out charsWritten, format);

        public static bool TryFormat(Type enumType, object value, Span<char> destination, out int charsWritten, params EnumFormat[]? formats) => GetCache(enumType).TryFormat(value, destination, out charsWritten, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);
#endif

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="format"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        [return: NotNullIfNotNull("value")]
        public static string? Format(Type enumType, object value, string format)
        {
            Preconditions.NotNull(format, nameof(format));

            return GetCache(enumType).AsString(value, format);
        }

        /// <summary>
        /// Returns <paramref name="value"/>'s underlying integral value.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/>'s underlying integral value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static object GetUnderlyingValue(Type enumType, object value) => GetCache(enumType).GetUnderlyingValue(value);

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="sbyte"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="sbyte"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="sbyte"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static sbyte ToSByte(Type enumType, object value) => GetCache(enumType).ToSByte(value);

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="byte"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="byte"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="byte"/>'s value range without overflowing.</exception>
        public static byte ToByte(Type enumType, object value) => GetCache(enumType).ToByte(value);

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="short"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="short"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="short"/>'s value range without overflowing.</exception>
        public static short ToInt16(Type enumType, object value) => GetCache(enumType).ToInt16(value);

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="ushort"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="ushort"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ushort"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static ushort ToUInt16(Type enumType, object value) => GetCache(enumType).ToUInt16(value);

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="int"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="int"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="int"/>'s value range without overflowing.</exception>
        public static int ToInt32(Type enumType, object value) => GetCache(enumType).ToInt32(value);

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="uint"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="uint"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="uint"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static uint ToUInt32(Type enumType, object value) => GetCache(enumType).ToUInt32(value);

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="long"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="long"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="long"/>'s value range without overflowing.</exception>
        public static long ToInt64(Type enumType, object value) => GetCache(enumType).ToInt64(value);

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="ulong"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="ulong"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ulong"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static ulong ToUInt64(Type enumType, object value) => GetCache(enumType).ToUInt64(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> equals <paramref name="other"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="other">The other enum value.</param>
        /// <returns>Indication if <paramref name="value"/> equals <paramref name="other"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="other"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="other"/> is of an invalid type.</exception>
        public static bool Equals(Type enumType, object value, object other) => GetCache(enumType).Equals(value, other);

        /// <summary>
        /// Compares <paramref name="value"/> to <paramref name="other"/> for ordering.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="other">The other enum value.</param>
        /// <returns>1 if <paramref name="value"/> is greater than <paramref name="other"/>, 0 if <paramref name="value"/> equals <paramref name="other"/>,
        /// and -1 if <paramref name="value"/> is less than <paramref name="other"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="other"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="other"/> is of an invalid type.</exception>
        public static int CompareTo(Type enumType, object value, object other) => GetCache(enumType).CompareTo(value, other);
        #endregion

        #region Defined Values Main Methods
        /// <summary>
        /// Retrieves <paramref name="value"/>'s enum member name if defined otherwise <c>null</c>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/>'s enum member name if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static string? GetName(Type enumType, object value) => GetCache(enumType).GetMember(value)?.Name;

        /// <summary>
        /// Retrieves <paramref name="value"/>'s enum member attributes if defined otherwise <c>null</c>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/>'s enum member attributes if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static AttributeCollection? GetAttributes(Type enumType, object value) => GetCache(enumType).GetMember(value)?.Attributes;

        /// <summary>
        /// Retrieves an enum member with the specified <paramref name="value"/> if defined otherwise <c>null</c>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns>Enum member with the specified <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static EnumMember? GetMember(Type enumType, object value) => GetCache(enumType).GetMember(value)?.EnumMember;

        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// Is case-sensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="name">The enum member name.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static EnumMember? GetMember(Type enumType, string name) => GetMember(enumType, name, false);

        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="name">The enum member name.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static EnumMember? GetMember(Type enumType, string name, bool ignoreCase)
        {
            Preconditions.NotNull(name, nameof(name));

            return GetCache(enumType).GetMember(name, ignoreCase, NameFormat);
        }

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="format"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static EnumMember? GetMember(Type enumType, string value, bool ignoreCase, EnumFormat format) => GetMember(enumType, value, ignoreCase, ValueCollection.Create(format));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static EnumMember? GetMember(Type enumType, string value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => GetMember(enumType, value, ignoreCase, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static EnumMember? GetMember(Type enumType, string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetMember(enumType, value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="formats"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static EnumMember? GetMember(Type enumType, string value, bool ignoreCase, params EnumFormat[]? formats) => GetMember(enumType, value, ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : Enums.NameFormat);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static EnumMember? GetMember(Type enumType, string value, bool ignoreCase, ValueCollection<EnumFormat> formats)
        {
            Preconditions.NotNull(value, nameof(value));

            return GetCache(enumType).GetMember(value, ignoreCase, formats);
        }

#if SPAN
        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="name">The enum member name.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static EnumMember? GetMember(Type enumType, ReadOnlySpan<char> name, bool ignoreCase = false) => GetCache(enumType).GetMember(name, ignoreCase, NameFormat);

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="format"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static EnumMember? GetMember(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format) => GetCache(enumType).GetMember(value, ignoreCase, ValueCollection.Create(format));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static EnumMember? GetMember(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => GetCache(enumType).GetMember(value, ignoreCase, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static EnumMember? GetMember(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetCache(enumType).GetMember(value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="formats"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static EnumMember? GetMember(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, params EnumFormat[]? formats) => GetCache(enumType).GetMember(value, ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : NameFormat);
#endif
        #endregion

        #region Parsing
        /// <summary>
        /// Converts the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <returns>A <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <paramref name="enumType"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <paramref name="enumType"/>'s underlying type.</exception>
        public static object Parse(Type enumType, string value) => Parse(enumType, value, false);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <paramref name="enumType"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object Parse(Type enumType, string value, bool ignoreCase)
        {
            Preconditions.NotNull(value, nameof(value));

            return GetCache(enumType).Parse(value, ignoreCase);
        }

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object Parse(Type enumType, string value, bool ignoreCase, EnumFormat format) => Parse(enumType, value, ignoreCase, ValueCollection.Create(format));

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object Parse(Type enumType, string value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => Parse(enumType, value, ignoreCase, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object Parse(Type enumType, string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Parse(enumType, value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object Parse(Type enumType, string value, bool ignoreCase, params EnumFormat[]? formats) => Parse(enumType, value, ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static object Parse(Type enumType, string value, bool ignoreCase, ValueCollection<EnumFormat> formats)
        {
            Preconditions.NotNull(value, nameof(value));

            return GetCache(enumType).Parse(value, ignoreCase, formats);
        }

#if SPAN
        /// <summary>
        /// Converts the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <paramref name="enumType"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object Parse(Type enumType, ReadOnlySpan<char> value, bool ignoreCase = false) => GetCache(enumType).Parse(value, ignoreCase);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object Parse(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format) => GetCache(enumType).Parse(value, ignoreCase, ValueCollection.Create(format));

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object Parse(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => GetCache(enumType).Parse(value, ignoreCase, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object Parse(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetCache(enumType).Parse(value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object Parse(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, params EnumFormat[]? formats) => GetCache(enumType).Parse(value, ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);
#endif

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParse(Type enumType, string? value, out object? result) => TryParse(enumType, value, false, out result);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result) => GetCache(enumType).TryParse(value, ignoreCase, out result);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format) => GetCache(enumType).TryParse(value, ignoreCase, out result, ValueCollection.Create(format));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1) => GetCache(enumType).TryParse(value, ignoreCase, out result, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetCache(enumType).TryParse(value, ignoreCase, out result, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result, params EnumFormat[]? formats) => GetCache(enumType).TryParse(value, ignoreCase, out result, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);

#if SPAN
        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParse(Type enumType, ReadOnlySpan<char> value, out object? result) => GetCache(enumType).TryParse(value, false, out result);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParse(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, out object? result) => GetCache(enumType).TryParse(value, ignoreCase, out result);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParse(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, out object? result, EnumFormat format) => GetCache(enumType).TryParse(value, ignoreCase, out result, ValueCollection.Create(format));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParse(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1) => GetCache(enumType).TryParse(value, ignoreCase, out result, ValueCollection.Create(format0, format1));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParse(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetCache(enumType).TryParse(value, ignoreCase, out result, ValueCollection.Create(format0, format1, format2));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParse(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, out object? result, params EnumFormat[]? formats) => GetCache(enumType).TryParse(value, ignoreCase, out result, formats?.Length > 0 ? ValueCollection.Create(formats) : DefaultFormats);
#endif
        #endregion
        #endregion

        #region Internal Methods
        internal static EnumCache? GetEnumCache(Type enumType)
        {
            if (!enumType.IsEnum())
            {
                return null;
            }

            return (EnumCache)typeof(Cache<>).MakeGenericType(enumType)
#if TYPE_REFLECTION
                .GetField(nameof(Cache<DayOfWeek>.Instance), BindingFlags.Static | BindingFlags.Public)!
#else
                .GetTypeInfo().GetDeclaredField(nameof(Cache<DayOfWeek>.Instance))
#endif
                .GetValue(null)!;
        }

        private static object? GetEnumValidatorAttribute(Type enumType)
        {
            var interfaceType = typeof(IEnumValidatorAttribute<>).MakeGenericType(enumType);
            foreach (var attribute in enumType.GetCustomAttributes(false))
            {
                foreach (var attributeInterface in attribute.GetType().GetInterfaces())
                {
                    if (attributeInterface == interfaceType)
                    {
                        return attribute;
                    }
                }
            }
            return null;
        }
        #endregion
    }
}