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

#define USE_EMIT

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

#if NET20 || USE_EMIT
using System.Reflection.Emit;
#else
using System.Linq.Expressions;
#endif

namespace EnumsNET
{
    internal class EnumInfo<TEnum, TInt> : IEnumInfo<TEnum>
        where TInt : struct
    {
        private static readonly TypeCode _typeCode = Type.GetTypeCode(typeof(TInt));

        private static readonly Type _underlyingType = typeof(TInt);

        private static readonly EnumsCache<TInt> _cache = (EnumsCache<TInt>)EnumInfo.Cache(typeof(TEnum), typeof(TInt), (Func<EnumFormat, Func<InternalEnumMemberInfo<TInt>, string>>)InternalGetCustomEnumFormatter);

        private static readonly Func<TEnum, TInt> _toInt = (Func<TEnum, TInt>)EnumInfo.ToInt(typeof(TEnum), typeof(TInt));

        private static int _lastCustomEnumFormatIndex = -1;

        private static List<Func<EnumMemberInfo<TEnum>, string>> _customEnumFormatters;

        internal static readonly Func<TInt, TEnum> ToEnum = (Func<TInt, TEnum>)EnumInfo.ToEnum(typeof(TEnum), typeof(TInt));

        #region Enums
        #region Properties
        public TypeCode TypeCode => _typeCode;

        public Type UnderlyingType => _underlyingType;

        public bool IsContiguous => _cache.IsContiguous;
        #endregion

        #region Type Methods
        public int GetDefinedCount(bool uniqueValued) => _cache.GetDefinedCount(uniqueValued);

        public IEnumerable<EnumMemberInfo<TEnum>> GetEnumMemberInfos(bool uniqueValued) => _cache.GetEnumMemberInfos(uniqueValued).Select(info => new EnumMemberInfo<TEnum, TInt>(info));

        public IEnumerable<string> GetNames(bool uniqueValued) => _cache.GetNames(uniqueValued);

        public IEnumerable<TEnum> GetValues(bool uniqueValued) => _cache.GetValues(uniqueValued).Select(value => ToEnum(value));

        public int Compare(TEnum x, TEnum y) => EnumsCache<TInt>.Compare(_toInt(x), _toInt(y));
        #endregion

        #region IsValid
        public bool IsValid(object value) => _cache.IsValid(value);

        public bool IsValid(TEnum value) => _cache.IsValid(_toInt(value));

        public bool IsValid(long value) => _cache.IsValid(value);

        public bool IsValid(ulong value) => _cache.IsValid(value);
        #endregion

        #region IsDefined
        public bool IsDefined(object value) => _cache.IsDefined(value);

        public bool IsDefined(TEnum value) => _cache.IsDefined(_toInt(value));

        public bool IsDefined(string name, bool ignoreCase) => _cache.IsDefined(name, ignoreCase);

        public bool IsDefined(long value) => _cache.IsDefined(value);

        public bool IsDefined(ulong value) => _cache.IsDefined(value);
        #endregion

        #region IsInValueRange
        public bool IsInValueRange(long value) => EnumsCache<TInt>.Int64IsInValueRange(value);

        public bool IsInValueRange(ulong value) => EnumsCache<TInt>.UInt64IsInValueRange(value);
        #endregion

        #region ToObject
        public TEnum ToObject(object value, bool validate = false) => ToEnum(_cache.ToObject(value, validate));

        public TEnum ToObject(long value, bool validate) => ToEnum(_cache.ToObject(value, validate));

        public TEnum ToObject(ulong value, bool validate) => ToEnum(_cache.ToObject(value, validate));

        public bool TryToObject(object value, out TEnum result, bool validate)
        {
            TInt resultAsInt;
            var success = _cache.TryToObject(value, out resultAsInt, validate);
            result = ToEnum(resultAsInt);
            return success;
        }

        public bool TryToObject(long value, out TEnum result, bool validate)
        {
            TInt resultAsInt;
            var success = _cache.TryToObject(value, out resultAsInt, validate);
            result = ToEnum(resultAsInt);
            return success;
        }

        public bool TryToObject(ulong value, out TEnum result, bool validate)
        {
            TInt resultAsInt;
            var success = _cache.TryToObject(value, out resultAsInt, validate);
            result = ToEnum(resultAsInt);
            return success;
        }
        #endregion

        #region All Values Main Methods
        public TEnum Validate(TEnum value, string paramName)
        {
            _cache.Validate(_toInt(value), paramName);
            return value;
        }

        public string AsString(TEnum value) => _cache.AsString(_toInt(value));

        public string AsString(TEnum value, string format) => _cache.AsString(_toInt(value), format);

        public string AsString(TEnum value, EnumFormat[] formats) => _cache.AsString(_toInt(value), formats);

        public string Format(TEnum value, string format) => _cache.Format(_toInt(value), format);

        public string Format(TEnum value, EnumFormat format) => _cache.Format(_toInt(value), format);

        public string Format(TEnum value, EnumFormat format0, EnumFormat format1) => _cache.Format(_toInt(value), format0, format1);

        public string Format(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => _cache.Format(_toInt(value), format0, format1, format2);

        public string Format(TEnum value, EnumFormat[] formats) => _cache.Format(_toInt(value), formats);

        public object GetUnderlyingValue(TEnum value) => _toInt(value);

        public sbyte ToSByte(TEnum value) => EnumsCache<TInt>.ToSByte(_toInt(value));

        public byte ToByte(TEnum value) => EnumsCache<TInt>.ToByte(_toInt(value));

        public short ToInt16(TEnum value) => EnumsCache<TInt>.ToInt16(_toInt(value));

        public ushort ToUInt16(TEnum value) => EnumsCache<TInt>.ToUInt16(_toInt(value));

        public int ToInt32(TEnum value) => EnumsCache<TInt>.ToInt32(_toInt(value));

        public uint ToUInt32(TEnum value) => EnumsCache<TInt>.ToUInt32(_toInt(value));

        public long ToInt64(TEnum value) => EnumsCache<TInt>.ToInt64(_toInt(value));

        public ulong ToUInt64(TEnum value) => EnumsCache<TInt>.ToUInt64(_toInt(value));

        public int GetHashCode(TEnum value) => _toInt(value).GetHashCode();

        public bool Equals(TEnum value, TEnum other) => EnumsCache<TInt>.Equals(_toInt(value), _toInt(other));
        #endregion

        #region Defined Values Main Methods
        public EnumMemberInfo<TEnum> GetEnumMemberInfo(TEnum value)
        {
            var info = _cache.GetEnumMemberInfo(_toInt(value));
            return info.IsDefined ? new EnumMemberInfo<TEnum, TInt>(info) : null;
        }

        public EnumMemberInfo<TEnum> GetEnumMemberInfo(string name, bool ignoreCase)
        {
            var info = _cache.GetEnumMemberInfo(name, ignoreCase);
            return info.IsDefined ? new EnumMemberInfo<TEnum, TInt>(info) : null;
        }

        public string GetName(TEnum value) => _cache.GetName(_toInt(value));

        public string GetDescription(TEnum value) => _cache.GetDescription(_toInt(value));
        #endregion

        #region Attributes
        public TAttribute GetAttribute<TAttribute>(TEnum value)
            where TAttribute : Attribute => _cache.GetAttribute<TAttribute>(_toInt(value));

        public TResult GetAttributeSelect<TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, TResult defaultValue)
            where TAttribute : Attribute => _cache.GetAttributeSelect(_toInt(value), selector, defaultValue);

        public bool TryGetAttributeSelect<TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, out TResult result)
            where TAttribute : Attribute => _cache.TryGetAttributeSelect(_toInt(value), selector, out result);

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(TEnum value)
            where TAttribute : Attribute => _cache.GetAttributes<TAttribute>(_toInt(value));

        public Attribute[] GetAttributes(TEnum value) => _cache.GetAttributes(_toInt(value));
        #endregion

        #region Parsing
        public TEnum Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder) => ToEnum(_cache.Parse(value, ignoreCase, parseFormatOrder));

        public bool TryParse(string value, bool ignoreCase, out TEnum result, EnumFormat[] parseFormatOrder)
        {
            TInt resultAsInt;
            var success = _cache.TryParse(value, ignoreCase, out resultAsInt, parseFormatOrder);
            result = ToEnum(resultAsInt);
            return success;
        }
        #endregion
        #endregion

        #region FlagEnums
        #region Properties
        public bool IsFlagEnum => _cache.IsFlagEnum;

        public TEnum AllFlags => ToEnum(_cache.AllFlags);
        #endregion

        #region Main Methods
        public bool IsValidFlagCombination(TEnum value) => _cache.IsValidFlagCombination(_toInt(value));

        public string FormatAsFlags(TEnum value, string delimiter, EnumFormat[] formats) => _cache.FormatAsFlags(_toInt(value), delimiter, formats);

        public TEnum[] GetFlags(TEnum value) => _cache.GetFlags(_toInt(value))?.Select(flag => ToEnum(flag)).ToArray();

        public bool HasAnyFlags(TEnum value) => _cache.HasAnyFlags(_toInt(value));

        public bool HasAnyFlags(TEnum value, TEnum flagMask) => _cache.HasAnyFlags(_toInt(value), _toInt(flagMask));

        public bool HasAllFlags(TEnum value) => _cache.HasAllFlags(_toInt(value));

        public bool HasAllFlags(TEnum value, TEnum flagMask) => _cache.HasAllFlags(_toInt(value), _toInt(flagMask));

        public TEnum ToggleFlags(TEnum value) => ToEnum(_cache.ToggleFlags(_toInt(value)));

        public TEnum ToggleFlags(TEnum value, TEnum flagMask) => ToEnum(_cache.ToggleFlags(_toInt(value), _toInt(flagMask)));

        public TEnum CommonFlags(TEnum value, TEnum flagMask) => ToEnum(_cache.CommonFlags(_toInt(value), _toInt(flagMask)));

        public TEnum SetFlags(TEnum flag0, TEnum flag1) => ToEnum(_cache.SetFlags(_toInt(flag0), _toInt(flag1)));

        public TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2) => ToEnum(_cache.SetFlags(_toInt(flag0), _toInt(flag1), _toInt(flag2)));

        public TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3) => ToEnum(_cache.SetFlags(_toInt(flag0), _toInt(flag1), _toInt(flag2), _toInt(flag3)));

        public TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4) => ToEnum(_cache.SetFlags(_toInt(flag0), _toInt(flag1), _toInt(flag2), _toInt(flag3), _toInt(flag4)));

        public TEnum SetFlags(TEnum[] flags) => ToEnum(_cache.SetFlags(flags.Select(flag => _toInt(flag))));

        public TEnum ClearFlags(TEnum value, TEnum flagMask) => ToEnum(_cache.ClearFlags(_toInt(value), _toInt(flagMask)));
        #endregion

        #region Parsing
        public TEnum Parse(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder) => ToEnum(_cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));

        public bool TryParse(string value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat[] parseFormatOrder)
        {
            TInt resultAsInt;
            var success = _cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsInt, parseFormatOrder);
            result = ToEnum(resultAsInt);
            return success;
        }
        #endregion
        #endregion

        #region NonGeneric
        object IEnumInfo.AllFlags => AllFlags;

        public string AsString(object value) => AsString(ToObject(value));

        public string AsString(object value, string format) => AsString(ToObject(value), format);

        public string AsString(object value, EnumFormat[] formats) => AsString(ToObject(value), formats);

        public object ClearFlags(object value, object flagMask) => ClearFlags(ToObject(value), ToObject(flagMask));

        public object CommonFlags(object value, object flagMask) => CommonFlags(ToObject(value), ToObject(flagMask));

        public int Compare(object x, object y) => Compare(ToObject(x), ToObject(y));

        public new bool Equals(object value, object other) => Equals(ToObject(value), ToObject(other));

        public string Format(object value, string format) => Format(ToObject(value), format);

        public string Format(object value, EnumFormat[] formats) => Format(ToObject(value), formats);

        public string Format(object value, EnumFormat format) => Format(ToObject(value), format);

        public string Format(object value, EnumFormat format0, EnumFormat format1) => Format(ToObject(value), format0, format1);

        public string Format(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Format(ToObject(value), format0, format1, format2);

        public string FormatAsFlags(object value, string delimiter, EnumFormat[] formats) => FormatAsFlags(ToObject(value), delimiter, formats);

        public Attribute[] GetAttributes(object value) => GetAttributes(ToObject(value));

        public string GetDescription(object value) => GetDescription(ToObject(value));

        public EnumMemberInfo GetEnumMemberInfo(object value) => GetEnumMemberInfo(ToObject(value));

        EnumMemberInfo IEnumInfo.GetEnumMemberInfo(string name, bool ignoreCase) => GetEnumMemberInfo(name, ignoreCase);

        IEnumerable<EnumMemberInfo> IEnumInfo.GetEnumMemberInfos(bool uniqueValued) => GetEnumMemberInfos(uniqueValued);

        public object[] GetFlags(object value) => GetFlags(ToObject(value)).Select(flag => (object)flag).ToArray();

        public string GetName(object value) => GetName(ToObject(value));

        public object GetUnderlyingValue(object value) => _cache.ToObject(value, false);

        IEnumerable<object> IEnumInfo.GetValues(bool uniqueValued) => GetValues(uniqueValued).Select(value => (object)value);

        public bool HasAllFlags(object value) => HasAllFlags(ToObject(value));

        public bool HasAllFlags(object value, object flagMask) => HasAllFlags(ToObject(value), ToObject(flagMask));

        public bool HasAnyFlags(object value) => HasAnyFlags(ToObject(value));

        public bool HasAnyFlags(object value, object flagMask) => HasAnyFlags(ToObject(value), ToObject(flagMask));

        public bool IsValidFlagCombination(object value) => IsValidFlagCombination(ToObject(value));

        object IEnumInfo.Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder) => Parse(value, ignoreCase, parseFormatOrder);

        object IEnumInfo.Parse(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder) => Parse(value, ignoreCase, delimiter, parseFormatOrder);

        public EnumFormat RegisterCustomEnumFormat(Func<EnumMemberInfo, string> formatter) => RegisterCustomEnumFormat(formatter);

        public object SetFlags(IEnumerable<object> flags) => _cache.SetFlags(flags.Select(flag => _cache.ToObject(flag, false)));

        public object SetFlags(object flag0, object flag1) => SetFlags(ToObject(flag0), ToObject(flag1));

        public byte ToByte(object value) => ToByte(ToObject(value));

        public object ToggleFlags(object value) => ToggleFlags(ToObject(value));

        public object ToggleFlags(object value, object flagMask) => ToggleFlags(ToObject(value), ToObject(flagMask));

        public short ToInt16(object value) => ToInt16(ToObject(value));

        public int ToInt32(object value) => ToInt32(ToObject(value));

        public long ToInt64(object value) => ToInt64(ToObject(value));

        object IEnumInfo.ToObject(ulong value, bool validate) => ToObject(value, validate);

        object IEnumInfo.ToObject(object value, bool validate) => ToObject(value, validate);

        object IEnumInfo.ToObject(long value, bool validate) => ToObject(value, validate);

        public sbyte ToSByte(object value) => ToSByte(ToObject(value));

        public ushort ToUInt16(object value) => ToUInt16(ToObject(value));

        public uint ToUInt32(object value) => ToUInt32(ToObject(value));

        public ulong ToUInt64(object value) => ToUInt64(ToObject(value));

        public bool TryParse(string value, bool ignoreCase, out object result, EnumFormat[] parseFormatOrder)
        {
            TEnum resultAsTEnum;
            var success = TryParse(value, ignoreCase, out resultAsTEnum, parseFormatOrder);
            result = resultAsTEnum;
            return success;
        }

        public bool TryParse(string value, bool ignoreCase, string delimiter, out object result, EnumFormat[] parseFormatOrder)
        {
            TEnum resultAsTEnum;
            var success = TryParse(value, ignoreCase, delimiter, out resultAsTEnum, parseFormatOrder);
            result = resultAsTEnum;
            return success;
        }

        public bool TryToObject(ulong value, out object result, bool validate)
        {
            TEnum resultAsTEnum;
            var success = TryToObject(value, out resultAsTEnum, validate);
            result = resultAsTEnum;
            return success;
        }

        public bool TryToObject(object value, out object result, bool validate)
        {
            TEnum resultAsTEnum;
            var success = TryToObject(value, out resultAsTEnum, validate);
            result = resultAsTEnum;
            return success;
        }

        public bool TryToObject(long value, out object result, bool validate)
        {
            TEnum resultAsTEnum;
            var success = TryToObject(value, out resultAsTEnum, validate);
            result = resultAsTEnum;
            return success;
        }

        public object Validate(object value, string paramName) => Validate(ToObject(value), paramName);
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

        internal static Func<InternalEnumMemberInfo<TInt>, string> InternalGetCustomEnumFormatter(EnumFormat format)
        {
            var formatter = Enums.GetCustomEnumFormatter(format) ?? GetCustomEnumFormatter(format);
            return formatter != null ? info => formatter(new EnumMemberInfo<TEnum, TInt>(info)) : (Func<InternalEnumMemberInfo<TInt>, string>)null;
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

    internal static class EnumInfo
    {
        internal static object Cache(Type enumType, Type underlyingType, Delegate getCustomEnumFormatter)
        {
            switch (Type.GetTypeCode(underlyingType))
            {
                case TypeCode.Int32:
                    return new EnumsCache<int>(enumType, (Func<EnumFormat, Func<InternalEnumMemberInfo<int>, string>>)getCustomEnumFormatter);
                case TypeCode.UInt32:
                    return new EnumsCache<uint>(enumType, (Func<EnumFormat, Func<InternalEnumMemberInfo<uint>, string>>)getCustomEnumFormatter);
                case TypeCode.Int64:
                    return new EnumsCache<long>(enumType, (Func<EnumFormat, Func<InternalEnumMemberInfo<long>, string>>)getCustomEnumFormatter);
                case TypeCode.UInt64:
                    return new EnumsCache<ulong>(enumType, (Func<EnumFormat, Func<InternalEnumMemberInfo<ulong>, string>>)getCustomEnumFormatter);
                case TypeCode.SByte:
                    return new EnumsCache<sbyte>(enumType, (Func<EnumFormat, Func<InternalEnumMemberInfo<sbyte>, string>>)getCustomEnumFormatter);
                case TypeCode.Byte:
                    return new EnumsCache<byte>(enumType, (Func<EnumFormat, Func<InternalEnumMemberInfo<byte>, string>>)getCustomEnumFormatter);
                case TypeCode.Int16:
                    return new EnumsCache<short>(enumType, (Func<EnumFormat, Func<InternalEnumMemberInfo<short>, string>>)getCustomEnumFormatter);
                case TypeCode.UInt16:
                    return new EnumsCache<ushort>(enumType, (Func<EnumFormat, Func<InternalEnumMemberInfo<ushort>, string>>)getCustomEnumFormatter);
                default:
                    Debug.Fail("Unknown Enum TypeCode");
                    return null;
            }
        }

        internal static Delegate ToEnum(Type enumType, Type underlyingType)
        {
#if NET20 || USE_EMIT
            var toEnumMethod = new DynamicMethod(underlyingType.Name + "_ToEnum",
                                       enumType,
                                       new[] { underlyingType },
                                       underlyingType, true);
            var toEnumGenerator = toEnumMethod.GetILGenerator();
            toEnumGenerator.DeclareLocal(enumType);
            toEnumGenerator.Emit(OpCodes.Ldarg_0);
            toEnumGenerator.Emit(OpCodes.Stloc_0);
            toEnumGenerator.Emit(OpCodes.Ldloc_0);
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
            toIntGenerator.DeclareLocal(underlyingType);
            toIntGenerator.Emit(OpCodes.Ldarg_0);
            toIntGenerator.Emit(OpCodes.Stloc_0);
            toIntGenerator.Emit(OpCodes.Ldloc_0);
            toIntGenerator.Emit(OpCodes.Ret);
            return toIntMethod.CreateDelegate(typeof(Func<,>).MakeGenericType(enumType, underlyingType));
#else
            var enumParam = Expression.Parameter(enumType, "x");
            var enumParamConvert = Expression.Convert(enumParam, underlyingType);
            return Expression.Lambda(enumParamConvert, enumParam).Compile();
#endif
        }
    }
}
