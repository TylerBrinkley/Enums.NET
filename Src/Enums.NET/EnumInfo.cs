// Enums.NET
// Copyright 2016 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//	  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#define USE_IL
//#define USE_EMIT
// else uses expression trees

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EnumsNET.Numerics;

#if USE_IL
using System.Runtime.CompilerServices;
#elif NET20 || USE_EMIT
using System.Reflection.Emit;
#else
using System.Linq.Expressions;
#endif

namespace EnumsNET
{
    /// <summary>
    /// Class that acts as a bridge from the enum type to the underlying type
    /// through the use of <see cref="IEnumInfo{TEnum}"/> and <see cref="IEnumInfo"/>.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="TInt"></typeparam>
    /// <typeparam name="TIntProvider"></typeparam>
    internal class EnumInfo<TEnum, TInt, TIntProvider> : IEnumInfo<TEnum>
        where TInt : struct, IFormattable, IConvertible, IComparable<TInt>, IEquatable<TInt>
        where TIntProvider : struct, INumericProvider<TInt>
    {
        private static int _lastCustomEnumFormatIndex = -1;

        private static List<Func<EnumMemberInfo<TEnum>, string>> _customEnumFormatters;

        internal static readonly EnumCache<TInt, TIntProvider> Cache = new EnumCache<TInt, TIntProvider>(typeof(TEnum), InternalGetCustomEnumFormatter);

#if USE_IL
        [MethodImpl(MethodImplOptions.ForwardRef)]
        internal static extern TInt ToInt(TEnum value);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        internal static extern TEnum ToEnum(TInt value);
#else
        internal static readonly Func<TEnum, TInt> ToInt = (Func<TEnum, TInt>)EnumInfo.ToInt(typeof(TEnum), typeof(TInt));
        internal static readonly Func<TInt, TEnum> ToEnum = (Func<TInt, TEnum>)EnumInfo.ToEnum(typeof(TEnum), typeof(TInt));
#endif

        #region Enums
        #region Properties
        public TypeCode TypeCode => new TInt().GetTypeCode();

        public Type UnderlyingType => typeof(TInt);

        public bool IsContiguous => Cache.IsContiguous;
        #endregion

        #region Type Methods
        public int GetDefinedCount(bool uniqueValued) => Cache.GetDefinedCount(uniqueValued);

        public IEnumerable<EnumMemberInfo<TEnum>> GetEnumMemberInfos(bool uniqueValued) => Cache.GetEnumMemberInfos(uniqueValued).Select(info => new EnumMemberInfo<TEnum, TInt, TIntProvider>(info));

        public IEnumerable<string> GetNames(bool uniqueValued) => Cache.GetNames(uniqueValued);

        public IEnumerable<TEnum> GetValues(bool uniqueValued) => Cache.GetValues(uniqueValued).Select(value => ToEnum(value));

        public int Compare(TEnum x, TEnum y) => ToInt(x).CompareTo(ToInt(y));
        #endregion

        #region IsValid
        public bool IsValid(object value) => Cache.IsValid(value);

        public bool IsValid(TEnum value) => Cache.IsValid(ToInt(value));

        public bool IsValid(long value) => Cache.IsValid(value);

        public bool IsValid(ulong value) => Cache.IsValid(value);
        #endregion

        #region IsDefined
        public bool IsDefined(object value) => Cache.IsDefined(value);

        public bool IsDefined(TEnum value) => Cache.IsDefined(ToInt(value));

        public bool IsDefined(string name, bool ignoreCase) => Cache.IsDefined(name, ignoreCase);

        public bool IsDefined(long value) => Cache.IsDefined(value);

        public bool IsDefined(ulong value) => Cache.IsDefined(value);
        #endregion

        #region IsInValueRange
        public bool IsInValueRange(long value) => EnumCache<TInt, TIntProvider>.Provider.IsInValueRange(value);

        public bool IsInValueRange(ulong value) => EnumCache<TInt, TIntProvider>.Provider.IsInValueRange(value);
        #endregion

        #region ToObject
        public TEnum ToObject(object value, bool validate) => ToEnum(Cache.ToObject(value, validate));

        public TEnum ToObject(long value, bool validate) => ToEnum(Cache.ToObject(value, validate));

        public TEnum ToObject(ulong value, bool validate) => ToEnum(Cache.ToObject(value, validate));

        public bool TryToObject(object value, out TEnum result, bool validate)
        {
            TInt resultAsInt;
            var success = Cache.TryToObject(value, out resultAsInt, validate);
            result = ToEnum(resultAsInt);
            return success;
        }

        public bool TryToObject(long value, out TEnum result, bool validate)
        {
            TInt resultAsInt;
            var success = Cache.TryToObject(value, out resultAsInt, validate);
            result = ToEnum(resultAsInt);
            return success;
        }

        public bool TryToObject(ulong value, out TEnum result, bool validate)
        {
            TInt resultAsInt;
            var success = Cache.TryToObject(value, out resultAsInt, validate);
            result = ToEnum(resultAsInt);
            return success;
        }
        #endregion

        #region All Values Main Methods
        public TEnum Validate(TEnum value, string paramName)
        {
            Cache.Validate(ToInt(value), paramName);
            return value;
        }

        public string AsString(TEnum value) => Cache.AsString(ToInt(value));

        public string AsString(TEnum value, string format) => Cache.AsString(ToInt(value), format);

        public string AsString(TEnum value, EnumFormat[] formats) => Cache.AsString(ToInt(value), formats);

        public string Format(TEnum value, string format) => Cache.Format(ToInt(value), format);

        public string Format(TEnum value, EnumFormat format) => Cache.Format(ToInt(value), format);

        public string Format(TEnum value, EnumFormat format0, EnumFormat format1) => Cache.Format(ToInt(value), format0, format1);

        public string Format(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Cache.Format(ToInt(value), format0, format1, format2);

        public string Format(TEnum value, EnumFormat[] formats) => Cache.Format(ToInt(value), formats);

        public object GetUnderlyingValue(TEnum value) => ToInt(value);

        public sbyte ToSByte(TEnum value) => ToInt(value).ToSByte(null);

        public byte ToByte(TEnum value) => ToInt(value).ToByte(null);

        public short ToInt16(TEnum value) => ToInt(value).ToInt16(null);

        public ushort ToUInt16(TEnum value) => ToInt(value).ToUInt16(null);

        public int ToInt32(TEnum value) => ToInt(value).ToInt32(null);

        public uint ToUInt32(TEnum value) => ToInt(value).ToUInt32(null);

        public long ToInt64(TEnum value) => ToInt(value).ToInt64(null);

        public ulong ToUInt64(TEnum value) => ToInt(value).ToUInt64(null);

        public int GetHashCode(TEnum value) => ToInt(value).GetHashCode();

        public bool Equals(TEnum value, TEnum other) => ToInt(value).Equals(ToInt(other));
        #endregion

        #region Defined Values Main Methods
        public EnumMemberInfo<TEnum> GetEnumMemberInfo(TEnum value)
        {
            var info = Cache.GetEnumMemberInfo(ToInt(value));
            return info.IsDefined ? new EnumMemberInfo<TEnum, TInt, TIntProvider>(info) : null;
        }

        public EnumMemberInfo<TEnum> GetEnumMemberInfo(string name, bool ignoreCase)
        {
            var info = Cache.GetEnumMemberInfo(name, ignoreCase);
            return info.IsDefined ? new EnumMemberInfo<TEnum, TInt, TIntProvider>(info) : null;
        }

        public string GetName(TEnum value) => Cache.GetName(ToInt(value));

        public string GetDescription(TEnum value) => Cache.GetDescription(ToInt(value));
        #endregion

        #region Attributes
        public TAttribute GetAttribute<TAttribute>(TEnum value)
            where TAttribute : Attribute => Cache.GetAttribute<TAttribute>(ToInt(value));

        public TResult GetAttributeSelect<TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, TResult defaultValue)
            where TAttribute : Attribute => Cache.GetAttributeSelect(ToInt(value), selector, defaultValue);

        public bool TryGetAttributeSelect<TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, out TResult result)
            where TAttribute : Attribute => Cache.TryGetAttributeSelect(ToInt(value), selector, out result);

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(TEnum value)
            where TAttribute : Attribute => Cache.GetAttributes<TAttribute>(ToInt(value));

        public IEnumerable<Attribute> GetAttributes(TEnum value) => Cache.GetAttributes(ToInt(value));
        #endregion

        #region Parsing
        public TEnum Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder) => ToEnum(Cache.Parse(value, ignoreCase, parseFormatOrder));

        public bool TryParse(string value, bool ignoreCase, out TEnum result, EnumFormat[] parseFormatOrder)
        {
            TInt resultAsInt;
            var success = Cache.TryParse(value, ignoreCase, out resultAsInt, parseFormatOrder);
            result = ToEnum(resultAsInt);
            return success;
        }
        #endregion
        #endregion

        #region FlagEnums
        #region Properties
        public bool IsFlagEnum => Cache.IsFlagEnum;

        public TEnum AllFlags => ToEnum(Cache.AllFlags);
        #endregion

        #region Main Methods
        public bool IsValidFlagCombination(TEnum value) => Cache.IsValidFlagCombination(ToInt(value));

        public string FormatAsFlags(TEnum value, string delimiter, EnumFormat[] formats) => Cache.FormatAsFlags(ToInt(value), delimiter, formats);

        public IEnumerable<TEnum> GetFlags(TEnum value) => Cache.GetFlags(ToInt(value))?.Select(flag => ToEnum(flag));

        public bool HasAnyFlags(TEnum value) => Cache.HasAnyFlags(ToInt(value));

        public bool HasAnyFlags(TEnum value, TEnum flagMask) => Cache.HasAnyFlags(ToInt(value), ToInt(flagMask));

        public bool HasAllFlags(TEnum value) => Cache.HasAllFlags(ToInt(value));

        public bool HasAllFlags(TEnum value, TEnum flagMask) => Cache.HasAllFlags(ToInt(value), ToInt(flagMask));

        public TEnum ToggleFlags(TEnum value) => ToEnum(Cache.ToggleFlags(ToInt(value)));

        public TEnum ToggleFlags(TEnum value, TEnum flagMask) => ToEnum(Cache.ToggleFlags(ToInt(value), ToInt(flagMask)));

        public TEnum CommonFlags(TEnum value, TEnum flagMask) => ToEnum(Cache.CommonFlags(ToInt(value), ToInt(flagMask)));

        public TEnum SetFlags(TEnum flag0, TEnum flag1) => ToEnum(Cache.SetFlags(ToInt(flag0), ToInt(flag1)));

        public TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2) => ToEnum(Cache.SetFlags(ToInt(flag0), ToInt(flag1), ToInt(flag2)));

        public TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3) => ToEnum(Cache.SetFlags(ToInt(flag0), ToInt(flag1), ToInt(flag2), ToInt(flag3)));

        public TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4) => ToEnum(Cache.SetFlags(ToInt(flag0), ToInt(flag1), ToInt(flag2), ToInt(flag3), ToInt(flag4)));

        public TEnum SetFlags(TEnum[] flags) => ToEnum(Cache.SetFlags(flags.Select(flag => ToInt(flag))));

        public TEnum ClearFlags(TEnum value, TEnum flagMask) => ToEnum(Cache.ClearFlags(ToInt(value), ToInt(flagMask)));
        #endregion

        #region Parsing
        public TEnum Parse(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder) => ToEnum(Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));

        public bool TryParse(string value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat[] parseFormatOrder)
        {
            TInt resultAsInt;
            var success = Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsInt, parseFormatOrder);
            result = ToEnum(resultAsInt);
            return success;
        }
        #endregion
        #endregion

        #region CustomEnumFormatters
        public EnumFormat RegisterCustomEnumFormat(Func<EnumMemberInfo<TEnum>, string> formatter)
        {
            Preconditions.NotNull(formatter, nameof(formatter));

            var index = Interlocked.Increment(ref _lastCustomEnumFormatIndex);
            if (index == 0)
            {
                _customEnumFormatters = new List<Func<EnumMemberInfo<TEnum>, string>>();
            }
            else
            {
                while (_customEnumFormatters?.Count != index)
                {
                }
            }
            _customEnumFormatters.Add(formatter);
            return (EnumFormat)(index + Enums.StartingGenericCustomEnumFormatValue);
        }

        internal static Func<InternalEnumMemberInfo<TInt, TIntProvider>, string> InternalGetCustomEnumFormatter(EnumFormat format)
        {
            var formatter = Enums.GetCustomEnumFormatter(format) ?? GetCustomEnumFormatter(format);
            return formatter != null ? info => formatter(new EnumMemberInfo<TEnum, TInt, TIntProvider>(info)) : (Func<InternalEnumMemberInfo<TInt, TIntProvider>, string>)null;
        }

        private static Func<EnumMemberInfo<TEnum>, string> GetCustomEnumFormatter(EnumFormat format)
        {
            var index = (int)format - Enums.StartingGenericCustomEnumFormatValue;
            if (index >= 0 && index < _customEnumFormatters?.Count)
            {
                return _customEnumFormatters[index];
            }
            return null;
        }
        #endregion
    }

#if !USE_IL
    internal static class EnumInfo
    {
        internal static Delegate ToEnum(Type enumType, Type underlyingType)
        {
#if NET20 || USE_EMIT
            var toEnumMethod = new DynamicMethod(underlyingType.Name + "_ToEnum",
                                       enumType,
                                       new[] { underlyingType },
                                       underlyingType, true);
            var toEnumGenerator = toEnumMethod.GetILGenerator();
            toEnumGenerator.Emit(OpCodes.Ldarg_0);
            toEnumGenerator.Emit(OpCodes.Ret);
            return toEnumMethod.CreateDelegate(typeof(Func<,>).MakeGenericType(underlyingType, enumType));
#else
            var intParam = Expression.Parameter(underlyingType, "y");
            var intParamConvert = Expression.Convert(intParam, enumType);
            return Expression.Lambda(intParamConvert, intParam).Compile();
#endif
        }

        internal static Delegate ToInt(Type enumType, Type underlyingType)
        {
#if NET20 || USE_EMIT
            var toIntMethod = new DynamicMethod(enumType.Name + "_ToInt",
                                       underlyingType,
                                       new[] { enumType },
                                       enumType, true);
            var toIntGenerator = toIntMethod.GetILGenerator();
            toIntGenerator.Emit(OpCodes.Ldarg_0);
            toIntGenerator.Emit(OpCodes.Ret);
            return toIntMethod.CreateDelegate(typeof(Func<,>).MakeGenericType(enumType, underlyingType));
#else
            var enumParam = Expression.Parameter(enumType, "x");
            var enumParamConvert = Expression.Convert(enumParam, underlyingType);
            return Expression.Lambda(enumParamConvert, enumParam).Compile();
#endif
        }
    }
#endif
}
