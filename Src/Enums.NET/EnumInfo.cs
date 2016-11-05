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
using System.Runtime.CompilerServices;
using System.Security;
using EnumsNET.Numerics;
using ExtraConstraints;

namespace EnumsNET
{
    // Class that acts as a bridge from the enum type to the underlying type
    // through the use of the implemented interfaces IEnumInfo<TEnum> and IEnumInfo.
    // Also acts as a bridge in the reverse from the underlying type to the enum type
    // through the use of the implemented interface IEnumInfoInternal<TInt, TIntProvider>
    // Putting the logic in EnumCache<TInt, TIntProvider> reduces memory usage
    // because it doesn't have the enum type as a generic type parameter.
    internal sealed class EnumInfo<[EnumConstraint] TEnum, TInt, TIntProvider> : IEnumInfo<TEnum>, IEnumInfo, IEnumInfoInternal<TInt, TIntProvider>
        where TEnum : struct
        where TInt : struct, IFormattable, IComparable<TInt>, IEquatable<TInt>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TIntProvider : struct, INumericProvider<TInt>
    {
#if NET45
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
#elif !NETSTANDARD
        [MethodImpl(MethodImplOptions.ForwardRef)]
#endif
#if SECURITY_SAFE_CRITICAL
        [SecuritySafeCritical]
#endif
        private static extern TInt ToInt(TEnum value);

#if NET45
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
#elif !NETSTANDARD
        [MethodImpl(MethodImplOptions.ForwardRef)]
#endif
#if SECURITY_SAFE_CRITICAL
        [SecuritySafeCritical]
#endif
        internal static extern TEnum ToEnum(TInt value);

        private readonly EnumCache<TInt, TIntProvider> _cache;
        private readonly IEnumValidatorAttribute<TEnum> _customEnumValidator = (IEnumValidatorAttribute<TEnum>)Enums.GetCustomEnumValidator(typeof(TEnum));

        public EnumInfo()
        {
            _cache = new EnumCache<TInt, TIntProvider>(typeof(TEnum), this);
        }

        #region Enums
        #region Properties
#if ICONVERTIBLE
        public TypeCode TypeCode => new TInt().GetTypeCode();
#endif

        public Type UnderlyingType => typeof(TInt);
        #endregion

        #region Type Methods
        public int GetEnumMemberCount(bool excludeDuplicates) => _cache.GetEnumMemberCount(excludeDuplicates);

        public IEnumerable<EnumMember<TEnum>> GetEnumMembers(bool excludeDuplicates) => _cache.GetEnumMembers(excludeDuplicates).Select(member => (EnumMember<TEnum>)member.EnumMember);

        public IEnumerable<string> GetNames(bool excludeDuplicates) => _cache.GetNames(excludeDuplicates);

        public IEnumerable<TEnum> GetValues(bool excludeDuplicates) => _cache.GetEnumMembers(excludeDuplicates).Select(member => ToEnum(member.Value));
        #endregion

        #region ToObject
        public TEnum ToObject(object value, bool validate = false) => value is TEnum || value is TEnum? ? (validate ? Validate((TEnum)value, nameof(value)) : (TEnum)value) : ToEnum(_cache.ToObject(value, validate));

        public TEnum ToObject(long value, bool validate) => ToEnum(_cache.ToObject(value, validate));

        public TEnum ToObject(ulong value, bool validate) => ToEnum(_cache.ToObject(value, validate));

        public bool TryToObject(object value, out TEnum result, bool validate)
        {
            if (value is TEnum || value is TEnum?)
            {
                result = (TEnum)value;
                return !validate || IsValid(result);
            }
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
        public bool IsValid(TEnum value) => _customEnumValidator?.IsValid(value) ?? _cache.IsValidSimple(ToInt(value));

        public bool IsDefined(TEnum value) => _cache.IsDefined(ToInt(value));

        public TEnum Validate(TEnum value, string paramName)
        {
            _cache.Validate(ToInt(value), paramName);
            return value;
        }

        public string AsString(TEnum value) => _cache.AsString(ToInt(value));

        public string AsString(TEnum value, string format) => _cache.AsString(ToInt(value), format);

        public string AsString(TEnum value, EnumFormat[] formats) => _cache.AsString(ToInt(value), formats);

        public string Format(TEnum value, string format) => _cache.Format(ToInt(value), format);

        public string AsString(TEnum value, EnumFormat format) => _cache.AsString(ToInt(value), format);

        public string AsString(TEnum value, EnumFormat format0, EnumFormat format1) => _cache.AsString(ToInt(value), format0, format1);

        public string AsString(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => _cache.AsString(ToInt(value), format0, format1, format2);

        public string Format(TEnum value, EnumFormat[] formats) => _cache.Format(ToInt(value), formats);

        public object GetUnderlyingValue(TEnum value) => ToInt(value);

#if ICONVERTIBLE
        public sbyte ToSByte(TEnum value) => ToInt(value).ToSByte(null);

        public byte ToByte(TEnum value) => ToInt(value).ToByte(null);

        public short ToInt16(TEnum value) => ToInt(value).ToInt16(null);

        public ushort ToUInt16(TEnum value) => ToInt(value).ToUInt16(null);

        public int ToInt32(TEnum value) => ToInt(value).ToInt32(null);

        public uint ToUInt32(TEnum value) => ToInt(value).ToUInt32(null);

        public long ToInt64(TEnum value) => ToInt(value).ToInt64(null);

        public ulong ToUInt64(TEnum value) => ToInt(value).ToUInt64(null);
#else
        public sbyte ToSByte(TEnum value) => EnumCache<TInt, TIntProvider>.Provider.ToSByte(ToInt(value));

        public byte ToByte(TEnum value) => EnumCache<TInt, TIntProvider>.Provider.ToByte(ToInt(value));

        public short ToInt16(TEnum value) => EnumCache<TInt, TIntProvider>.Provider.ToInt16(ToInt(value));

        public ushort ToUInt16(TEnum value) => EnumCache<TInt, TIntProvider>.Provider.ToUInt16(ToInt(value));

        public int ToInt32(TEnum value) => EnumCache<TInt, TIntProvider>.Provider.ToInt32(ToInt(value));

        public uint ToUInt32(TEnum value) => EnumCache<TInt, TIntProvider>.Provider.ToUInt32(ToInt(value));

        public long ToInt64(TEnum value) => EnumCache<TInt, TIntProvider>.Provider.ToInt64(ToInt(value));

        public ulong ToUInt64(TEnum value) => EnumCache<TInt, TIntProvider>.Provider.ToUInt64(ToInt(value));
#endif

        public int GetHashCode(TEnum value) => ToInt(value).GetHashCode();

        public bool Equals(TEnum value, TEnum other) => ToInt(value).Equals(ToInt(other));

        public int CompareTo(TEnum value, TEnum other) => ToInt(value).CompareTo(ToInt(other));
        #endregion

        #region Defined Values Main Methods
        public string GetName(TEnum value) => _cache.GetName(ToInt(value));

        public EnumMember<TEnum> GetEnumMember(TEnum value) => (EnumMember<TEnum>)_cache.GetEnumMember(ToInt(value))?.EnumMember;

        public EnumMember<TEnum> GetEnumMember(string value, bool ignoreCase, EnumFormat[] formats) => (EnumMember<TEnum>)_cache.GetEnumMember(value, ignoreCase, formats)?.EnumMember;
        #endregion

        #region Parsing
        public TEnum Parse(string value, bool ignoreCase, EnumFormat[] formats) => ToEnum(_cache.Parse(value, ignoreCase, formats));

        public bool TryParse(string value, bool ignoreCase, out TEnum result, EnumFormat[] formats)
        {
            TInt resultAsInt;
            var success = _cache.TryParse(value, ignoreCase, out resultAsInt, formats);
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
        public bool IsValidFlagCombination(TEnum value) => _cache.IsValidFlagCombination(ToInt(value));

        public string FormatFlags(TEnum value, string delimiter, EnumFormat[] formats) => _cache.FormatFlags(ToInt(value), delimiter, formats);

        public IEnumerable<TEnum> GetFlags(TEnum value) => _cache.GetFlags(ToInt(value)).Select(flag => ToEnum(flag));

        public IEnumerable<EnumMember<TEnum>> GetFlagMembers(TEnum value) => _cache.GetFlagMembers(ToInt(value)).Select(flag => (EnumMember<TEnum>)flag.EnumMember);

        public bool HasAnyFlags(TEnum value) => _cache.HasAnyFlags(ToInt(value));

        public bool HasAnyFlags(TEnum value, TEnum otherFlags) => _cache.HasAnyFlags(ToInt(value), ToInt(otherFlags));

        public bool HasAllFlags(TEnum value) => _cache.HasAllFlags(ToInt(value));

        public bool HasAllFlags(TEnum value, TEnum otherFlags) => _cache.HasAllFlags(ToInt(value), ToInt(otherFlags));

        public TEnum ToggleFlags(TEnum value) => ToEnum(_cache.ToggleFlags(ToInt(value)));

        public TEnum ToggleFlags(TEnum value, TEnum otherFlags) => ToEnum(_cache.ToggleFlags(ToInt(value), ToInt(otherFlags)));

        public TEnum CommonFlags(TEnum value, TEnum otherFlags) => ToEnum(_cache.CommonFlags(ToInt(value), ToInt(otherFlags)));

        public TEnum CombineFlags(TEnum value, TEnum otherFlags) => ToEnum(_cache.CombineFlags(ToInt(value), ToInt(otherFlags)));

        public TEnum CombineFlags(TEnum flag0, TEnum flag1, TEnum flag2) => ToEnum(_cache.CombineFlags(ToInt(flag0), ToInt(flag1), ToInt(flag2)));

        public TEnum CombineFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3) => ToEnum(_cache.CombineFlags(ToInt(flag0), ToInt(flag1), ToInt(flag2), ToInt(flag3)));

        public TEnum CombineFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4) => ToEnum(_cache.CombineFlags(ToInt(flag0), ToInt(flag1), ToInt(flag2), ToInt(flag3), ToInt(flag4)));

        public TEnum CombineFlags(IEnumerable<TEnum> flags) => ToEnum(_cache.CombineFlags(flags?.Select(flag => ToInt(flag))));

        public TEnum RemoveFlags(TEnum value, TEnum otherFlags) => ToEnum(_cache.RemoveFlags(ToInt(value), ToInt(otherFlags)));
        #endregion

        #region Parsing
        public TEnum ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] formats) => ToEnum(_cache.ParseFlags(value, ignoreCase, delimiter, formats));

        public bool TryParseFlags(string value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat[] formats)
        {
            TInt resultAsInt;
            var success = _cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsInt, formats);
            result = ToEnum(resultAsInt);
            return success;
        }
        #endregion
        #endregion

        #region NonGeneric
        object IEnumInfo.AllFlags => AllFlags;

        public string AsString(object value) => AsString(ToObject(value));

        public string AsString(object value, string format) => AsString(ToObject(value), format);

        public string AsString(object value, EnumFormat format) => AsString(ToObject(value), format);

        public string AsString(object value, EnumFormat format0, EnumFormat format1) => AsString(ToObject(value), format0, format1);

        public string AsString(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => AsString(ToObject(value), format0, format1, format2);

        public string AsString(object value, EnumFormat[] formats) => AsString(ToObject(value), formats);

        public object RemoveFlags(object value, object otherFlags) => RemoveFlags(ToObject(value), ToObject(otherFlags));

        public object CommonFlags(object value, object otherFlags) => CommonFlags(ToObject(value), ToObject(otherFlags));

        public int CompareTo(object value, object other) => CompareTo(ToObject(value), ToObject(other));

        public new bool Equals(object value, object other) => Equals(ToObject(value), ToObject(other));

        public string Format(object value, string format) => Format(ToObject(value), format);

        public string Format(object value, EnumFormat[] formats) => Format(ToObject(value), formats);

        public string FormatFlags(object value, string delimiter, EnumFormat[] formats) => FormatFlags(ToObject(value), delimiter, formats);

        public IEnumerable<Attribute> GetAttributes(object value) => GetAttributes(ToObject(value));

        public EnumMember GetEnumMember(object value) => GetEnumMember(ToObject(value));

        EnumMember IEnumInfo.GetEnumMember(string value, bool ignoreCase, EnumFormat[] formats) => GetEnumMember(value, ignoreCase, formats);

        IEnumerable<EnumMember> IEnumInfo.GetEnumMembers(bool excludeDuplicates) => GetEnumMembers(excludeDuplicates)
#if !COVARIANCE
            .Select(member => (EnumMember)member)
#endif
            ;

        public IEnumerable<object> GetFlags(object value) => GetFlags(ToObject(value)).Select(flag => (object)flag);

        public IEnumerable<EnumMember> GetFlagMembers(object value) => GetFlagMembers(ToObject(value))
#if !COVARIANCE
            .Select(flag => (EnumMember)flag)
#endif
            ;

        public string GetName(object value) => GetName(ToObject(value));

        public object GetUnderlyingValue(object value) => GetUnderlyingValue(ToObject(value));

        IEnumerable<object> IEnumInfo.GetValues(bool excludeDuplicates) => GetValues(excludeDuplicates).Select(value => (object)value);

        public bool HasAllFlags(object value) => HasAllFlags(ToObject(value));

        public bool HasAllFlags(object value, object otherFlags) => HasAllFlags(ToObject(value), ToObject(otherFlags));

        public bool HasAnyFlags(object value) => HasAnyFlags(ToObject(value));

        public bool HasAnyFlags(object value, object otherFlags) => HasAnyFlags(ToObject(value), ToObject(otherFlags));

        public bool IsDefined(object value) => IsDefined(ToObject(value));

        public bool IsValid(object value) => IsValid(ToObject(value));

        public bool IsValidFlagCombination(object value) => IsValidFlagCombination(ToObject(value));

        object IEnumInfo.Parse(string value, bool ignoreCase, EnumFormat[] formats) => Parse(value, ignoreCase, formats);

        object IEnumInfo.ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] formats) => ParseFlags(value, ignoreCase, delimiter, formats);

        public object CombineFlags(IEnumerable<object> flags) => CombineFlags(flags?.Select(flag => ToObject(flag)));

        public object CombineFlags(object value, object otherFlags) => CombineFlags(ToObject(value), ToObject(otherFlags));

        public byte ToByte(object value) => ToByte(ToObject(value));

        public object ToggleFlags(object value) => ToggleFlags(ToObject(value));

        public object ToggleFlags(object value, object otherFlags) => ToggleFlags(ToObject(value), ToObject(otherFlags));

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

        public bool TryParse(string value, bool ignoreCase, out object result, EnumFormat[] formats)
        {
            TEnum resultAsTEnum;
            var success = TryParse(value, ignoreCase, out resultAsTEnum, formats);
            result = resultAsTEnum;
            return success;
        }

        public bool TryParseFlags(string value, bool ignoreCase, string delimiter, out object result, EnumFormat[] formats)
        {
            TEnum resultAsTEnum;
            var success = TryParseFlags(value, ignoreCase, delimiter, out resultAsTEnum, formats);
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

        #region IEnumInfoInternal
        public bool HasCustomValidator => _customEnumValidator != null;

        public bool CustomValidate(TInt value) => _customEnumValidator.IsValid(ToEnum(value));

        public EnumMember CreateEnumMember(EnumMemberInternal<TInt, TIntProvider> member) => new EnumMember<TEnum, TInt, TIntProvider>(member);
        #endregion
    }
}
