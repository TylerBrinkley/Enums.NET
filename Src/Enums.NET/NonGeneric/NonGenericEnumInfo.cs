using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumsNET.NonGeneric
{
    internal class NonGenericEnumInfo<TEnum, TInt> : IEnumInfo
        where TInt : struct
    {
        private static readonly TypeCode _typeCode = Type.GetTypeCode(typeof(TInt));

        private static readonly Type _underlyingType = typeof(TInt);

        private static readonly EnumCache<TInt> Cache = EnumInfo<TEnum, TInt>.Cache;

        private static readonly Func<TEnum, TInt> ToInt = EnumInfo<TEnum, TInt>.ToInt;

        private static readonly Func<TInt, TEnum> ToEnum = EnumInfo<TEnum, TInt>.ToEnum;

        #region Properties
        public TypeCode TypeCode => _typeCode;

        public Type UnderlyingType => _underlyingType;

        public bool IsContiguous => Cache.IsContiguous;

        public bool IsFlagEnum => Cache.IsFlagEnum;
        #endregion

        #region Type Methods
        public int GetDefinedCount(bool uniqueValued) => Cache.GetDefinedCount(uniqueValued);

        public IEnumerable<string> GetNames(bool uniqueValued) => Cache.GetNames(uniqueValued);
        #endregion

        #region IsValid
        public bool IsValid(object value) => Cache.IsValid(value);

        public bool IsValid(long value) => Cache.IsValid(value);

        public bool IsValid(ulong value) => Cache.IsValid(value);
        #endregion

        #region IsDefined
        public bool IsDefined(object value) => Cache.IsDefined(value);

        public bool IsDefined(string name, bool ignoreCase) => Cache.IsDefined(name, ignoreCase);

        public bool IsDefined(long value) => Cache.IsDefined(value);

        public bool IsDefined(ulong value) => Cache.IsDefined(value);
        #endregion

        #region IsInValueRange
        public bool IsInValueRange(long value) => EnumCache<TInt>.Int64IsInValueRange(value);

        public bool IsInValueRange(ulong value) => EnumCache<TInt>.UInt64IsInValueRange(value);
        #endregion

        #region NonGeneric
        public object AllFlags => ToEnum(Cache.AllFlags);

        public string AsString(object value) => Cache.AsString(ToObject(value));

        public string AsString(object value, string format) => Cache.AsString(ToObject(value), format);

        public string AsString(object value, EnumFormat[] formats) => Cache.AsString(ToObject(value), formats);

        public object ClearFlags(object value, object flagMask) => ToEnum(Cache.ClearFlags(ToObject(value), ToObject(flagMask)));

        public object CommonFlags(object value, object flagMask) => ToEnum(Cache.CommonFlags(ToObject(value), ToObject(flagMask)));

        public int Compare(object x, object y) => EnumCache<TInt>.Compare(ToObject(x), ToObject(y));

        public new bool Equals(object value, object other) => EnumCache<TInt>.Equals(ToObject(value), ToObject(other));

        public string Format(object value, string format) => Cache.Format(ToObject(value), format);

        public string Format(object value, EnumFormat[] formats) => Cache.Format(ToObject(value), formats);

        public string Format(object value, EnumFormat format) => Cache.Format(ToObject(value), format);

        public string Format(object value, EnumFormat format0, EnumFormat format1) => Cache.Format(ToObject(value), format0, format1);

        public string Format(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Cache.Format(ToObject(value), format0, format1, format2);

        public string FormatAsFlags(object value, string delimiter, EnumFormat[] formats) => Cache.FormatAsFlags(ToObject(value), delimiter, formats);

        public IEnumerable<Attribute> GetAttributes(object value) => Cache.GetAttributes(ToObject(value));

        public string GetDescription(object value) => Cache.GetDescription(ToObject(value));

        public EnumMemberInfo GetEnumMemberInfo(object value)
        {
            var info = Cache.GetEnumMemberInfo(ToObject(value));
            return info.IsDefined ? new EnumMemberInfo<TEnum, TInt>(info) : null;
        }

        public EnumMemberInfo GetEnumMemberInfo(string name, bool ignoreCase)
        {
            var info = Cache.GetEnumMemberInfo(name, ignoreCase);
            return info.IsDefined ? new EnumMemberInfo<TEnum, TInt>(info) : null;
        }

        public IEnumerable<EnumMemberInfo> GetEnumMemberInfos(bool uniqueValued) => Cache.GetEnumMemberInfos(uniqueValued).Select(info => new EnumMemberInfo<TEnum, TInt>(info));

        public IEnumerable<object> GetFlags(object value) => Cache.GetFlags(ToObject(value)).Select(flag => (object)ToEnum(flag));

        public string GetName(object value) => Cache.GetName(ToObject(value));

        public object GetUnderlyingValue(object value) => ToObject(value);

        public IEnumerable<object> GetValues(bool uniqueValued) => Cache.GetValues(uniqueValued).Select(value => (object)ToEnum(value));

        public bool HasAllFlags(object value) => Cache.HasAllFlags(ToObject(value));

        public bool HasAllFlags(object value, object flagMask) => Cache.HasAllFlags(ToObject(value), ToObject(flagMask));

        public bool HasAnyFlags(object value) => Cache.HasAnyFlags(ToObject(value));

        public bool HasAnyFlags(object value, object flagMask) => Cache.HasAnyFlags(ToObject(value), ToObject(flagMask));

        public bool IsValidFlagCombination(object value) => Cache.IsValidFlagCombination(ToObject(value));

        public object Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder) => ToEnum(Cache.Parse(value, ignoreCase, parseFormatOrder));

        public object Parse(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder) => ToEnum(Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));

        public EnumFormat RegisterCustomEnumFormat(Func<EnumMemberInfo, string> formatter) => Enums<TEnum>.Info.RegisterCustomEnumFormat(formatter);

        public object SetFlags(IEnumerable<object> flags) => ToEnum(Cache.SetFlags(flags.Select(flag => ToObject(flag))));

        public object SetFlags(object flag0, object flag1) => ToEnum(Cache.SetFlags(ToObject(flag0), ToObject(flag1)));

        public byte ToByte(object value) => EnumCache<TInt>.ToByte(ToObject(value));

        public object ToggleFlags(object value) => ToEnum(Cache.ToggleFlags(ToObject(value)));

        public object ToggleFlags(object value, object flagMask) => ToEnum(Cache.ToggleFlags(ToObject(value), ToObject(flagMask)));

        public short ToInt16(object value) => EnumCache<TInt>.ToInt16(ToObject(value));

        public int ToInt32(object value) => EnumCache<TInt>.ToInt32(ToObject(value));

        public long ToInt64(object value) => EnumCache<TInt>.ToInt64(ToObject(value));

        public object ToObject(ulong value, bool validate) => ToEnum(Cache.ToObject(value, validate));

        public object ToObject(object value, bool validate) => ToEnum(Cache.ToObject(value, validate));

        public object ToObject(long value, bool validate) => ToEnum(Cache.ToObject(value, validate));

        public sbyte ToSByte(object value) => EnumCache<TInt>.ToSByte(ToObject(value));

        public ushort ToUInt16(object value) => EnumCache<TInt>.ToUInt16(ToObject(value));

        public uint ToUInt32(object value) => EnumCache<TInt>.ToUInt32(ToObject(value));

        public ulong ToUInt64(object value) => EnumCache<TInt>.ToUInt64(ToObject(value));

        public bool TryParse(string value, bool ignoreCase, out object result, EnumFormat[] parseFormatOrder)
        {
            TInt resultAsTInt;
            var success = Cache.TryParse(value, ignoreCase, out resultAsTInt, parseFormatOrder);
            result = ToEnum(resultAsTInt);
            return success;
        }

        public bool TryParse(string value, bool ignoreCase, string delimiter, out object result, EnumFormat[] parseFormatOrder)
        {
            TInt resultAsTInt;
            var success = Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsTInt, parseFormatOrder);
            result = ToEnum(resultAsTInt);
            return success;
        }

        public bool TryToObject(ulong value, out object result, bool validate)
        {
            TInt resultAsTInt;
            var success = Cache.TryToObject(value, out resultAsTInt, validate);
            result = ToEnum(resultAsTInt);
            return success;
        }

        public bool TryToObject(object value, out object result, bool validate)
        {
            TInt resultAsTInt;
            var success = Cache.TryToObject(value, out resultAsTInt, validate);
            result = ToEnum(resultAsTInt);
            return success;
        }

        public bool TryToObject(long value, out object result, bool validate)
        {
            TInt resultAsTInt;
            var success = Cache.TryToObject(value, out resultAsTInt, validate);
            result = ToEnum(resultAsTInt);
            return success;
        }

        public object Validate(object value, string paramName)
        {
            var valueAsTInt = ToObject(value);
            Cache.Validate(valueAsTInt, paramName);
            return ToEnum(valueAsTInt);
        }
        #endregion

        private TInt ToObject(object value) => Cache.ToObject(value, false);
    }
}
