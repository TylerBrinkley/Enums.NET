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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace EnumsNET.NonGeneric
{
    /// <summary>
    /// A non-generic implementation of the static class <see cref="Enums"/>.
    /// When the type is known at compile-time the <see cref="Enums"/> class should be used instead, to provide type safety and to avoid boxing.
    /// </summary>
    public static class NonGenericEnums
    {
        private static readonly ConcurrentDictionary<Type, NonGenericEnumInfo> _nonGenericEnumInfos = new ConcurrentDictionary<Type, NonGenericEnumInfo>();

        internal static NonGenericEnumInfo GetNonGenericEnumInfo(Type enumType)
        {
            Preconditions.NotNull(enumType, nameof(enumType));
            
            NonGenericEnumInfo info;
            if (!_nonGenericEnumInfos.TryGetValue(enumType, out info))
            {
                if (enumType.IsEnum())
                {
                    var closedEnumsType = typeof(Enums<>).MakeGenericType(enumType);
                    info = new NonGenericEnumInfo((IEnumInfo)closedEnumsType.
#if TYPE_REFLECTION
                        GetField("Info", BindingFlags.Static | BindingFlags.Public)
#else
                        GetTypeInfo().GetDeclaredField("Info")
#endif
                    .GetValue(null), false);
                }
                else
                {
                    var nonNullableEnumType = Nullable.GetUnderlyingType(enumType);
                    if (nonNullableEnumType?.IsEnum() != true)
                    {
                        throw new ArgumentException("must be an enum type", nameof(enumType));
                    }
                    info = new NonGenericEnumInfo(GetInfo(nonNullableEnumType), true);
                }
                if (!_nonGenericEnumInfos.TryAdd(enumType, info))
                {
                    info = _nonGenericEnumInfos[enumType];
                }
            }
            return info;
        }

        internal static IEnumInfo GetInfo(Type enumType) => GetNonGenericEnumInfo(enumType).EnumInfo;

        #region Type Methods
        /// <summary>
        /// Indicates if <paramref name="enumType"/>'s defined values are contiguous.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns>Indication if <paramref name="enumType"/>'s defined values are contiguous.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool IsContiguous(Type enumType) => GetInfo(enumType).IsContiguous;

        /// <summary>
        /// Retrieves the underlying type of <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns>The underlying type of <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static Type GetUnderlyingType(Type enumType) => GetInfo(enumType).UnderlyingType;

#if ICONVERTIBLE
        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s underlying type's <see cref="TypeCode"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns><paramref name="enumType"/>'s underlying type's <see cref="TypeCode"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static TypeCode GetTypeCode(Type enumType) => GetInfo(enumType).TypeCode;
#endif

        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s member count.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns><paramref name="enumType"/>'s member count.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static int GetEnumMemberCount(Type enumType) => GetInfo(enumType).GetEnumMemberCount();

        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s member count.
        /// The parameter <paramref name="excludeDuplicates"/> indicates whether to exclude duplicate value enum members.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="excludeDuplicates">Exclude duplicate value enum members.</param>
        /// <returns><paramref name="enumType"/>'s member count.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static int GetEnumMemberCount(Type enumType, bool excludeDuplicates) => GetInfo(enumType).GetEnumMemberCount(excludeDuplicates);

        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s members in increasing value order.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns><paramref name="enumType"/>'s members in increasing value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static IEnumerable<EnumMember> GetEnumMembers(Type enumType) => GetInfo(enumType).GetEnumMembers();

        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s members in increasing value order.
        /// The parameter <paramref name="excludeDuplicates"/> indicates whether to exclude duplicate value enum members.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="excludeDuplicates">Exclude duplicate value enum members.</param>
        /// <returns><paramref name="enumType"/>'s members in increasing value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static IEnumerable<EnumMember> GetEnumMembers(Type enumType, bool excludeDuplicates) => GetInfo(enumType).GetEnumMembers(excludeDuplicates);

        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s members' names in increasing value order.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns><paramref name="enumType"/>'s members' names in increasing value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static IEnumerable<string> GetNames(Type enumType) => GetInfo(enumType).GetNames();

        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s members' names in increasing value order.
        /// The parameter <paramref name="excludeDuplicates"/> indicates whether to exclude duplicate value enum members.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="excludeDuplicates">Exclude duplicate value enum members.</param>
        /// <returns><paramref name="enumType"/>'s members' names in increasing value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static IEnumerable<string> GetNames(Type enumType, bool excludeDuplicates) => GetInfo(enumType).GetNames(excludeDuplicates);

        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s members' values in increasing value order.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns><paramref name="enumType"/>'s members' values in increasing value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static IEnumerable<object> GetValues(Type enumType) => GetInfo(enumType).GetValues();

        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s members' values in increasing value order.
        /// The parameter <paramref name="excludeDuplicates"/> indicates whether to exclude duplicate value enum members.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="excludeDuplicates">Exclude duplicate value enum members.</param>
        /// <returns><paramref name="enumType"/>'s members' values in increasing value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static IEnumerable<object> GetValues(Type enumType, bool excludeDuplicates) => GetInfo(enumType).GetValues(excludeDuplicates);
        #endregion

        #region ToObject
        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <paramref name="enumType"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not a valid type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, object value) => ToObject(enumType, value, false);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <paramref name="enumType"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not a valid type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, object value, bool validate)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.ToObject(value, validate);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, sbyte value) => GetInfo(enumType).ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, sbyte value, bool validate) => GetInfo(enumType).ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, byte value) => GetInfo(enumType).ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, byte value, bool validate) => GetInfo(enumType).ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, short value) => GetInfo(enumType).ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, short value, bool validate) => GetInfo(enumType).ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ushort value) => GetInfo(enumType).ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ushort value, bool validate) => GetInfo(enumType).ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, int value) => GetInfo(enumType).ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, int value, bool validate) => GetInfo(enumType).ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, uint value) => GetInfo(enumType).ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, uint value, bool validate) => GetInfo(enumType).ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, long value) => GetInfo(enumType).ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static object ToObject(Type enumType, long value, bool validate) => GetInfo(enumType).ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ulong value) => GetInfo(enumType).ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ulong value, bool validate) => GetInfo(enumType).ToObject(value, validate);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <paramref name="enumType"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryToObject(Type enumType, object value, out object result) => TryToObject(enumType, value, out result, false);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <paramref name="enumType"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryToObject(Type enumType, object value, out object result, bool validate)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                result = null;
                return true;
            }

            return info.EnumInfo.TryToObject(value, out result, validate);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, sbyte value, out object result) => GetInfo(enumType).TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, sbyte value, out object result, bool validate) => GetInfo(enumType).TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryToObject(Type enumType, byte value, out object result) => GetInfo(enumType).TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryToObject(Type enumType, byte value, out object result, bool validate) => GetInfo(enumType).TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryToObject(Type enumType, short value, out object result) => GetInfo(enumType).TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryToObject(Type enumType, short value, out object result, bool validate) => GetInfo(enumType).TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ushort value, out object result) => GetInfo(enumType).TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ushort value, out object result, bool validate) => GetInfo(enumType).TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryToObject(Type enumType, int value, out object result) => GetInfo(enumType).TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryToObject(Type enumType, int value, out object result, bool validate) => GetInfo(enumType).TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, uint value, out object result) => GetInfo(enumType).TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, uint value, out object result, bool validate) => GetInfo(enumType).TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryToObject(Type enumType, long value, out object result) => GetInfo(enumType).TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryToObject(Type enumType, long value, out object result, bool validate) => GetInfo(enumType).TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ulong value, out object result) => GetInfo(enumType).TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a value of type <paramref name="enumType"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ulong value, out object result, bool validate) => GetInfo(enumType).TryToObject(value, out result, validate);
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
        /// <returns>Indication if <paramref name="value"/> is valid.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static bool IsValid(Type enumType, object value)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return true;
            }

            return info.EnumInfo.IsValid(value);
        }

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
        public static bool IsDefined(Type enumType, object value)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return false;
            }

            return info.EnumInfo.IsDefined(value);
        }

        /// <summary>
        /// Validates that <paramref name="value"/> is valid. If it's not it throws an <see cref="ArgumentException"/> with the specified <paramref name="paramName"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="paramName">The parameter name to be used if throwing an <see cref="ArgumentException"/>.</param>
        /// <returns><paramref name="value"/> for use in fluent API's and base constructor method calls.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type
        /// -or-
        /// <paramref name="value"/> is invalid.</exception>
        public static object Validate(Type enumType, object value, string paramName)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.Validate(value, paramName);
        }

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
        public static string AsString(Type enumType, object value)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.AsString(value);
        }

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
        public static string AsString(Type enumType, object value, string format)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.AsString(value, format);
        }

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
        public static string AsString(Type enumType, object value, EnumFormat format)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.AsString(value, format);
        }

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
        public static string AsString(Type enumType, object value, EnumFormat format0, EnumFormat format1)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.AsString(value, format0, format1);
        }

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
        public static string AsString(Type enumType, object value, EnumFormat format0, EnumFormat format1, EnumFormat format2)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.AsString(value, format0, format1, format2);
        }

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
        public static string AsString(Type enumType, object value, params EnumFormat[] formats)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.AsString(value, formats);
        }

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
        public static string Format(Type enumType, object value, string format)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.Format(value, format);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="formats"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="formats">The output formats to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="formats"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static string Format(Type enumType, object value, params EnumFormat[] formats)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.Format(value, formats);
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
        public static object GetUnderlyingValue(Type enumType, object value)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.GetUnderlyingValue(value);
        }

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
        public static sbyte ToSByte(Type enumType, object value) => GetInfo(enumType).ToSByte(value);

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
        public static byte ToByte(Type enumType, object value) => GetInfo(enumType).ToByte(value);

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
        public static short ToInt16(Type enumType, object value) => GetInfo(enumType).ToInt16(value);

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
        public static ushort ToUInt16(Type enumType, object value) => GetInfo(enumType).ToUInt16(value);

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
        public static int ToInt32(Type enumType, object value) => GetInfo(enumType).ToInt32(value);

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
        public static uint ToUInt32(Type enumType, object value) => GetInfo(enumType).ToUInt32(value);

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
        public static long ToInt64(Type enumType, object value) => GetInfo(enumType).ToInt64(value);

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
        public static ulong ToUInt64(Type enumType, object value) => GetInfo(enumType).ToUInt64(value);

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
        public static bool Equals(Type enumType, object value, object other)
        {
            var info = GetNonGenericEnumInfo(enumType);
            var enumInfo = info.EnumInfo;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    if (other == null)
                    {
                        return true;
                    }
                    enumInfo.ToObject(other);
                    return false;
                }
                if (other == null)
                {
                    enumInfo.ToObject(value);
                    return false;
                }
            }

            return enumInfo.Equals(value, other);
        }

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
        public static int CompareTo(Type enumType, object value, object other)
        {
            var info = GetNonGenericEnumInfo(enumType);
            var enumInfo = info.EnumInfo;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    if (other == null)
                    {
                        return 0;
                    }
                    enumInfo.ToObject(other);
                    return -1;
                }
                if (other == null)
                {
                    enumInfo.ToObject(value);
                    return 1;
                }
            }

            return enumInfo.CompareTo(value, other);
        }
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
        public static string GetName(Type enumType, object value)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.GetName(value);
        }

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
        public static EnumMember GetEnumMember(Type enumType, object value)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.GetEnumMember(value);
        }

        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// Is case-sensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="name">The enum member name.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static EnumMember GetEnumMember(Type enumType, string name) => GetInfo(enumType).GetEnumMember(name);

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
        public static EnumMember GetEnumMember(Type enumType, string name, bool ignoreCase) => GetInfo(enumType).GetEnumMember(name, ignoreCase);

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="formats"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static EnumMember GetEnumMember(Type enumType, string value, params EnumFormat[] formats) => GetInfo(enumType).GetEnumMember(value, false, formats);

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
        public static EnumMember GetEnumMember(Type enumType, string value, bool ignoreCase, params EnumFormat[] formats) => GetInfo(enumType).GetEnumMember(value, ignoreCase, formats);
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
        public static object Parse(Type enumType, string value) => Parse(enumType, value, false, null);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>A <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object Parse(Type enumType, string value, params EnumFormat[] formats) => Parse(enumType, value, false, formats);

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
        public static object Parse(Type enumType, string value, bool ignoreCase) => Parse(enumType, value, ignoreCase, null);

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
        public static object Parse(Type enumType, string value, bool ignoreCase, params EnumFormat[] formats)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (info.IsNullable && string.IsNullOrEmpty(value))
            {
                return null;
            }

            return info.EnumInfo.Parse(value, ignoreCase, formats);
        }

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
        public static bool TryParse(Type enumType, string value, out object result) => TryParse(enumType, value, false, out result, null);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParse(Type enumType, string value, out object result, params EnumFormat[] formats) => TryParse(enumType, value, false, out result, formats);

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
        public static bool TryParse(Type enumType, string value, bool ignoreCase, out object result) => TryParse(enumType, value, ignoreCase, out result, null);

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
        public static bool TryParse(Type enumType, string value, bool ignoreCase, out object result, params EnumFormat[] formats)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (string.IsNullOrEmpty(value) && info.IsNullable)
            {
                result = null;
                return true;
            }

            return info.EnumInfo.TryParse(value, ignoreCase, out result, formats);
        }
        #endregion
    }
}
