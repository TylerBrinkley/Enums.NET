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
using EnumsNET.Numerics;

namespace EnumsNET
{
    /// <summary>
    /// Class that acts as a bridge from the enum type to the underlying type
    /// through the use of <see cref="IEnumInfo{TEnum}"/> and <see cref="IEnumInfo"/>.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="TInt"></typeparam>
    /// <typeparam name="TIntProvider"></typeparam>
    internal sealed class EnumInfo<TEnum, TInt, TIntProvider> : IEnumInfo<TEnum>, IEnumInfo, IInternalEnumInfo<TInt, TIntProvider>
        where TEnum : struct
        where TInt : struct, IFormattable, IConvertible, IComparable<TInt>, IEquatable<TInt>
        where TIntProvider : struct, INumericProvider<TInt>
    {
#if NET45
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
#else
        [MethodImpl(MethodImplOptions.ForwardRef)]
#endif
        private static extern TInt ToInt(TEnum value);

#if NET45
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
#else
        [MethodImpl(MethodImplOptions.ForwardRef)]
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
        public TypeCode TypeCode => new TInt().GetTypeCode();

        public Type UnderlyingType => typeof(TInt);

        public bool IsContiguous => _cache.IsContiguous;
        #endregion

        #region Type Methods
        public int GetEnumMemberCount(bool uniqueValued) => _cache.GetEnumMemberCount(uniqueValued);

        public IEnumerable<EnumMember<TEnum>> GetEnumMembers(bool uniqueValued) => _cache.GetEnumMembers(uniqueValued).Select(member =>
#if NET20 || NET35
            (EnumMember<TEnum>)
#endif
            new EnumMember<TEnum, TInt, TIntProvider>(member));

        public IEnumerable<string> GetNames(bool uniqueValued) => _cache.GetNames(uniqueValued);

        public IEnumerable<TEnum> GetValues(bool uniqueValued) => _cache.GetEnumMembers(uniqueValued).Select(member => ToEnum(member.Value));

        public int CompareTo(TEnum value, TEnum other) => ToInt(value).CompareTo(ToInt(other));
        #endregion

        #region IsValid
        public bool IsValid(TEnum value) => _cache.IsValid(ToInt(value));
        #endregion

        #region IsDefined
        public bool IsDefined(TEnum value) => _cache.IsDefined(ToInt(value));
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
        public TEnum Validate(TEnum value, string paramName)
        {
            _cache.Validate(ToInt(value), paramName);
            return value;
        }

        public string AsString(TEnum value) => _cache.AsString(ToInt(value));

        public string AsString(TEnum value, string format) => _cache.AsString(ToInt(value), format);

        public string AsString(TEnum value, EnumFormat[] formatOrder) => _cache.AsString(ToInt(value), formatOrder);

        public string Format(TEnum value, string format) => _cache.Format(ToInt(value), format);

        public string AsString(TEnum value, EnumFormat format) => _cache.AsString(ToInt(value), format);

        public string AsString(TEnum value, EnumFormat format0, EnumFormat format1) => _cache.AsString(ToInt(value), format0, format1);

        public string AsString(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => _cache.AsString(ToInt(value), format0, format1, format2);

        public string Format(TEnum value, EnumFormat[] formatOrder) => _cache.Format(ToInt(value), formatOrder);

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
        public EnumMember<TEnum> GetEnumMember(TEnum value) => InternalGetEnumMember(_cache.GetEnumMember(ToInt(value)));

        public EnumMember<TEnum> GetEnumMember(string name, bool ignoreCase) => InternalGetEnumMember(_cache.GetEnumMember(name, ignoreCase));

        private static EnumMember<TEnum> InternalGetEnumMember(InternalEnumMember<TInt, TIntProvider> member) => member.IsDefined ? new EnumMember<TEnum, TInt, TIntProvider>(member) : null;

        public string GetName(TEnum value) => _cache.GetName(ToInt(value));
        #endregion

        #region Attributes
        public TAttribute GetAttribute<TAttribute>(TEnum value)
            where TAttribute : Attribute => _cache.GetAttribute<TAttribute>(ToInt(value));

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(TEnum value)
            where TAttribute : Attribute => _cache.GetAttributes<TAttribute>(ToInt(value));

        public IEnumerable<Attribute> GetAttributes(TEnum value) => _cache.GetAttributes(ToInt(value));
        #endregion

        #region Parsing
        public TEnum Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder) => ToEnum(_cache.Parse(value, ignoreCase, parseFormatOrder));

        public EnumMember<TEnum> ParseMember(string value, bool ignoreCase, EnumFormat[] parseFormatOrder) => new EnumMember<TEnum, TInt, TIntProvider>(_cache.ParseMember(value, ignoreCase, parseFormatOrder));

        public bool TryParse(string value, bool ignoreCase, out TEnum result, EnumFormat[] parseFormatOrder)
        {
            TInt resultAsInt;
            var success = _cache.TryParse(value, ignoreCase, out resultAsInt, parseFormatOrder);
            result = ToEnum(resultAsInt);
            return success;
        }

        public bool TryParseMember(string value, bool ignoreCase, out EnumMember<TEnum> result, EnumFormat[] parseFormatOrder)
        {
            InternalEnumMember<TInt, TIntProvider> member;
            if (_cache.TryParseMember(value, ignoreCase, out member, parseFormatOrder))
            {
                result = new EnumMember<TEnum, TInt, TIntProvider>(member);
                return true;
            }
            result = null;
            return false;
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

        public string FormatFlags(TEnum value, string delimiter, EnumFormat[] formatOrder) => _cache.FormatFlags(ToInt(value), delimiter, formatOrder);

        public IEnumerable<TEnum> GetFlags(TEnum value) => _cache.GetFlags(ToInt(value)).Select(flag => ToEnum(flag));

        public IEnumerable<EnumMember<TEnum>> GetFlagMembers(TEnum value) => _cache.GetFlagMembers(ToInt(value)).Select(flag =>
#if NET20 || NET35
            (EnumMember<TEnum>)
#endif
            new EnumMember<TEnum, TInt, TIntProvider>(flag));

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

        public TEnum ExcludeFlags(TEnum value, TEnum otherFlags) => ToEnum(_cache.ExcludeFlags(ToInt(value), ToInt(otherFlags)));
        #endregion

        #region Parsing
        public TEnum ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder) => ToEnum(_cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));

        public bool TryParseFlags(string value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat[] parseFormatOrder)
        {
            TInt resultAsInt;
            var success = _cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsInt, parseFormatOrder);
            result = ToEnum(resultAsInt);
            return success;
        }
        #endregion
        #endregion

        #region CustomEnumFormatters
        public string CustomEnumMemberFormat(InternalEnumMember<TInt, TIntProvider> member, EnumFormat format) => Enums.CustomEnumMemberFormat(new EnumMember<TEnum, TInt, TIntProvider>(member), format);
        #endregion

        #region CustomEnumValidator
        public bool? CustomValidate(TInt value) => _customEnumValidator?.IsValid(ToEnum(value));
        #endregion

        #region NonGeneric
        object IEnumInfo.AllFlags => AllFlags;

        public string AsString(object value) => AsString(ToObject(value));

        public string AsString(object value, string format) => AsString(ToObject(value), format);

        public string AsString(object value, EnumFormat format) => AsString(ToObject(value), format);

        public string AsString(object value, EnumFormat format0, EnumFormat format1) => AsString(ToObject(value), format0, format1);

        public string AsString(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => AsString(ToObject(value), format0, format1, format2);

        public string AsString(object value, EnumFormat[] formatOrder) => AsString(ToObject(value), formatOrder);

        public object ExcludeFlags(object value, object otherFlags) => ExcludeFlags(ToObject(value), ToObject(otherFlags));

        public object CommonFlags(object value, object otherFlags) => CommonFlags(ToObject(value), ToObject(otherFlags));

        public int CompareTo(object value, object other) => CompareTo(ToObject(value), ToObject(other));

        public new bool Equals(object value, object other) => Equals(ToObject(value), ToObject(other));

        public string Format(object value, string format) => Format(ToObject(value), format);

        public string Format(object value, EnumFormat[] formatOrder) => Format(ToObject(value), formatOrder);

        public string FormatFlags(object value, string delimiter, EnumFormat[] formatOrder) => FormatFlags(ToObject(value), delimiter, formatOrder);

        public IEnumerable<Attribute> GetAttributes(object value) => GetAttributes(ToObject(value));

        public EnumMember GetEnumMember(object value) => GetEnumMember(ToObject(value));

        EnumMember IEnumInfo.GetEnumMember(string name, bool ignoreCase) => GetEnumMember(name, ignoreCase);

        IEnumerable<EnumMember> IEnumInfo.GetEnumMembers(bool uniqueValued) => GetEnumMembers(uniqueValued)
#if NET20 || NET35
            .Select(member => (EnumMember)member)
#endif
            ;

        public IEnumerable<object> GetFlags(object value) => GetFlags(ToObject(value)).Select(flag => (object)flag);

        public IEnumerable<EnumMember> GetFlagMembers(object value) => GetFlagMembers(ToObject(value))
#if NET20 || NET35
            .Select(flag => (EnumMember)flag)
#endif
            ;

        public string GetName(object value) => GetName(ToObject(value));

        public object GetUnderlyingValue(object value) => GetUnderlyingValue(ToObject(value));

        IEnumerable<object> IEnumInfo.GetValues(bool uniqueValued) => GetValues(uniqueValued).Select(value => (object)value);

        public bool HasAllFlags(object value) => HasAllFlags(ToObject(value));

        public bool HasAllFlags(object value, object otherFlags) => HasAllFlags(ToObject(value), ToObject(otherFlags));

        public bool HasAnyFlags(object value) => HasAnyFlags(ToObject(value));

        public bool HasAnyFlags(object value, object otherFlags) => HasAnyFlags(ToObject(value), ToObject(otherFlags));

        public bool IsDefined(object value) => IsDefined(ToObject(value));

        public bool IsValid(object value) => IsValid(ToObject(value));

        public bool IsValidFlagCombination(object value) => IsValidFlagCombination(ToObject(value));

        object IEnumInfo.Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder) => Parse(value, ignoreCase, parseFormatOrder);

        EnumMember IEnumInfo.ParseMember(string value, bool ignoreCase, EnumFormat[] parseFormatOrder) => ParseMember(value, ignoreCase, parseFormatOrder);

        object IEnumInfo.ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder) => ParseFlags(value, ignoreCase, delimiter, parseFormatOrder);

        public object CombineFlags(IEnumerable<object> flags) => CombineFlags(flags.Select(flag => ToObject(flag)));

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

        public bool TryParse(string value, bool ignoreCase, out object result, EnumFormat[] parseFormatOrder)
        {
            TEnum resultAsTEnum;
            var success = TryParse(value, ignoreCase, out resultAsTEnum, parseFormatOrder);
            result = resultAsTEnum;
            return success;
        }

        public bool TryParseMember(string value, bool ignoreCase, out EnumMember result, EnumFormat[] parseFormatOrder)
        {
            EnumMember<TEnum> member;
            var success = TryParseMember(value, ignoreCase, out member, parseFormatOrder);
            result = member;
            return success;
        }

        public bool TryParseFlags(string value, bool ignoreCase, string delimiter, out object result, EnumFormat[] parseFormatOrder)
        {
            TEnum resultAsTEnum;
            var success = TryParseFlags(value, ignoreCase, delimiter, out resultAsTEnum, parseFormatOrder);
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
    }
}
