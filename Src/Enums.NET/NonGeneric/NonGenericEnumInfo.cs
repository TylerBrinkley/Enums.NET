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
using System.Linq;
using EnumsNET.Numerics;

namespace EnumsNET.NonGeneric
{
    internal sealed class NonGenericEnumInfo<TEnum, TInt, TIntProvider> : IEnumInfo
        where TEnum : struct
        where TInt : struct, IFormattable, IConvertible, IComparable<TInt>, IEquatable<TInt>
        where TIntProvider : struct, INumericProvider<TInt>
    {
        private static EnumCache<TInt, TIntProvider> Cache => EnumInfo<TEnum, TInt, TIntProvider>.Cache;

        private static TEnum ToEnum(TInt value) => EnumInfo<TEnum, TInt, TIntProvider>.ToEnum(value);

        private static TInt ToInt(object value) => value is TEnum || value is TEnum? ? EnumInfo<TEnum, TInt, TIntProvider>.ToInt((TEnum)value) : Cache.ToObject(value, false);

        #region Properties
        public TypeCode TypeCode => new TInt().GetTypeCode();

        public Type UnderlyingType => typeof(TInt);

        public bool IsContiguous => Cache.IsContiguous;

        public bool IsFlagEnum => Cache.IsFlagEnum;
        #endregion

        #region Type Methods
        public int GetDefinedCount(bool uniqueValued) => Cache.GetDefinedCount(uniqueValued);

        public IEnumerable<string> GetNames(bool uniqueValued) => Cache.GetNames(uniqueValued);
        #endregion

        #region IsValid
        public bool IsValid(object value) => value is TEnum || value is TEnum? ? Cache.IsValid(EnumInfo<TEnum, TInt, TIntProvider>.ToInt((TEnum)value)) : Cache.IsValid(value);

        public bool IsValid(long value) => Cache.IsValid(value);

        public bool IsValid(ulong value) => Cache.IsValid(value);
        #endregion

        #region IsDefined
        public bool IsDefined(object value) => value is TEnum || value is TEnum? ? Cache.IsDefined(EnumInfo<TEnum, TInt, TIntProvider>.ToInt((TEnum)value)) : Cache.IsDefined(value);

        public bool IsDefined(string name, bool ignoreCase) => Cache.IsDefined(name, ignoreCase);

        public bool IsDefined(long value) => Cache.IsDefined(value);

        public bool IsDefined(ulong value) => Cache.IsDefined(value);
        #endregion

        #region IsInValueRange
        public bool IsInValueRange(long value) => EnumCache<TInt, TIntProvider>.Provider.IsInValueRange(value);

        public bool IsInValueRange(ulong value) => EnumCache<TInt, TIntProvider>.Provider.IsInValueRange(value);
        #endregion

        #region NonGeneric
        public object AllFlags => ToEnum(Cache.AllFlags);

        public string AsString(object value) => Cache.AsString(ToInt(value));

        public string AsString(object value, string format) => Cache.AsString(ToInt(value), format);

        public string AsString(object value, EnumFormat[] formats) => Cache.AsString(ToInt(value), formats);

        public object ExcludeFlags(object value, object flagMask) => ToEnum(Cache.ExcludeFlags(ToInt(value), ToInt(flagMask)));

        public object CommonFlags(object value, object flagMask) => ToEnum(Cache.CommonFlags(ToInt(value), ToInt(flagMask)));

        public int Compare(object x, object y) => ToInt(x).CompareTo(ToInt(y));

        public new bool Equals(object value, object other) => ToInt(value).Equals(ToInt(other));

        public string Format(object value, string format) => Cache.Format(ToInt(value), format);

        public string Format(object value, EnumFormat[] formats) => Cache.Format(ToInt(value), formats);

        public string Format(object value, EnumFormat format) => Cache.Format(ToInt(value), format);

        public string Format(object value, EnumFormat format0, EnumFormat format1) => Cache.Format(ToInt(value), format0, format1);

        public string Format(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Cache.Format(ToInt(value), format0, format1, format2);

        public string FormatAsFlags(object value, string delimiter, EnumFormat[] formats) => Cache.FormatAsFlags(ToInt(value), delimiter, formats);

        public IEnumerable<Attribute> GetAttributes(object value) => Cache.GetAttributes(ToInt(value));

        public string GetDescription(object value) => Cache.GetDescription(ToInt(value));

        public EnumMember GetEnumMember(object value)
        {
            var member = Cache.GetEnumMember(ToInt(value));
            return member.IsDefined ? new EnumMember<TEnum, TInt, TIntProvider>(member) : null;
        }

        public EnumMember GetEnumMember(string name, bool ignoreCase)
        {
            var member = Cache.GetEnumMember(name, ignoreCase);
            return member.IsDefined ? new EnumMember<TEnum, TInt, TIntProvider>(member) : null;
        }

        public IEnumerable<EnumMember> GetEnumMembers(bool uniqueValued) => Cache.GetEnumMembers(uniqueValued).Select(member =>
#if NET20 || NET35
            (EnumMember)
#endif
            new EnumMember<TEnum, TInt, TIntProvider>(member));

        public IEnumerable<object> GetFlags(object value) => Cache.GetFlags(ToInt(value)).Select(flag => (object)ToEnum(flag));

        public string GetName(object value) => Cache.GetName(ToInt(value));

        public object GetUnderlyingValue(object value) => ToInt(value);

        public IEnumerable<object> GetValues(bool uniqueValued) => Cache.GetValues(uniqueValued).Select(value => (object)ToEnum(value));

        public bool HasAllFlags(object value) => Cache.HasAllFlags(ToInt(value));

        public bool HasAllFlags(object value, object flagMask) => Cache.HasAllFlags(ToInt(value), ToInt(flagMask));

        public bool HasAnyFlags(object value) => Cache.HasAnyFlags(ToInt(value));

        public bool HasAnyFlags(object value, object flagMask) => Cache.HasAnyFlags(ToInt(value), ToInt(flagMask));

        public bool IsValidFlagCombination(object value) => Cache.IsValidFlagCombination(ToInt(value));

        public object Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder) => ToEnum(Cache.Parse(value, ignoreCase, parseFormatOrder));

        public object ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder) => ToEnum(Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));

        public EnumFormat RegisterCustomEnumFormat(Func<EnumMember, string> formatter)
        {
            return EnumInfo<TEnum, TInt, TIntProvider>.InternalRegisterCustomEnumFormat(
#if NET20 || NET35
                member => formatter(member));
#else
                formatter);
#endif
        }

        public object CombineFlags(IEnumerable<object> flags) => ToEnum(Cache.CombineFlags(flags.Select(flag => ToInt(flag))));

        public object CombineFlags(object flag0, object flag1) => ToEnum(Cache.CombineFlags(ToInt(flag0), ToInt(flag1)));

        public byte ToByte(object value) => ToInt(value).ToByte(null);

        public object ToggleFlags(object value, bool toggleValidFlagsOnly) => ToEnum(Cache.ToggleFlags(ToInt(value), toggleValidFlagsOnly));

        public object ToggleFlags(object value, object flagMask) => ToEnum(Cache.ToggleFlags(ToInt(value), ToInt(flagMask)));

        public short ToInt16(object value) => ToInt(value).ToInt16(null);

        public int ToInt32(object value) => ToInt(value).ToInt32(null);

        public long ToInt64(object value) => ToInt(value).ToInt64(null);

        public object ToObject(ulong value, bool validate) => ToEnum(Cache.ToObject(value, validate));

        public object ToObject(object value, bool validate) => value is TEnum || value is TEnum? ? (TEnum)value : ToEnum(Cache.ToObject(value, validate));

        public object ToObject(long value, bool validate) => ToEnum(Cache.ToObject(value, validate));

        public sbyte ToSByte(object value) => ToInt(value).ToSByte(null);

        public ushort ToUInt16(object value) => ToInt(value).ToUInt16(null);

        public uint ToUInt32(object value) => ToInt(value).ToUInt32(null);

        public ulong ToUInt64(object value) => ToInt(value).ToUInt64(null);

        public bool TryParse(string value, bool ignoreCase, out object result, EnumFormat[] parseFormatOrder)
        {
            TInt resultAsTInt;
            var success = Cache.TryParse(value, ignoreCase, out resultAsTInt, parseFormatOrder);
            result = ToEnum(resultAsTInt);
            return success;
        }

        public bool TryParseFlags(string value, bool ignoreCase, string delimiter, out object result, EnumFormat[] parseFormatOrder)
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
            if (value is TEnum || value is TEnum?)
            {
                result = (TEnum)value;
                return true;
            }
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
            var valueAsTInt = ToInt(value);
            Cache.Validate(valueAsTInt, paramName);
            return ToEnum(valueAsTInt);
        }
        #endregion
    }
}
