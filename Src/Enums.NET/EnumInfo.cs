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
using System.Threading;
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
    internal sealed class EnumInfo<TEnum, TInt, TIntProvider> : IEnumInfo<TEnum>
        where TEnum : struct
        where TInt : struct, IFormattable, IConvertible, IComparable<TInt>, IEquatable<TInt>
        where TIntProvider : struct, INumericProvider<TInt>
    {
        private static int _lastCustomEnumFormatIndex = -1;

        private static List<Func<EnumMember<TEnum>, string>> _customEnumFormatters;

        internal static readonly EnumCache<TInt, TIntProvider> Cache = new EnumCache<TInt, TIntProvider>(typeof(TEnum), InternalGetCustomEnumFormatter);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        internal static extern TInt ToInt(TEnum value);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        internal static extern TEnum ToEnum(TInt value);

        #region Enums
        #region Properties
        public TypeCode TypeCode => new TInt().GetTypeCode();

        public Type UnderlyingType => typeof(TInt);

        public bool IsContiguous => Cache.IsContiguous;
        #endregion

        #region Type Methods
        public int GetEnumMemberCount(bool uniqueValued) => Cache.GetEnumMemberCount(uniqueValued);

        public IEnumerable<EnumMember<TEnum>> GetEnumMembers(bool uniqueValued) => Cache.GetEnumMembers(uniqueValued).Select(member =>
#if NET20 || NET35
            (EnumMember<TEnum>)
#endif
            new EnumMember<TEnum, TInt, TIntProvider>(member));

        public IEnumerable<string> GetNames(bool uniqueValued) => Cache.GetNames(uniqueValued);

        public IEnumerable<TEnum> GetValues(bool uniqueValued) => Cache.GetEnumMembers(uniqueValued).Select(member => ToEnum(member.Value));

        public int CompareTo(TEnum value, TEnum other) => ToInt(value).CompareTo(ToInt(other));
        #endregion

        #region IsValid
        public bool IsValid(object value) => value is TEnum || value is TEnum? ? Cache.IsValid(ToInt((TEnum)value)) : Cache.IsValid(value);

        public bool IsValid(TEnum value) => Cache.IsValid(ToInt(value));

        public bool IsValid(long value) => Cache.IsValid(value);

        public bool IsValid(ulong value) => Cache.IsValid(value);
        #endregion

        #region IsDefined
        public bool IsDefined(object value) => value is TEnum || value is TEnum? ? Cache.IsDefined(ToInt((TEnum)value)) : Cache.IsDefined(value);

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
        public TEnum ToObject(object value, bool validate) => value is TEnum || value is TEnum? ? (TEnum)value : ToEnum(Cache.ToObject(value, validate));

        public TEnum ToObject(long value, bool validate) => ToEnum(Cache.ToObject(value, validate));

        public TEnum ToObject(ulong value, bool validate) => ToEnum(Cache.ToObject(value, validate));

        public bool TryToObject(object value, out TEnum result, bool validate)
        {
            if (value is TEnum || value is TEnum?)
            {
                result = (TEnum)value;
                return true;
            }
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

        public string AsString(TEnum value, EnumFormat[] formatOrder) => Cache.AsString(ToInt(value), formatOrder);

        public string Format(TEnum value, string format) => Cache.Format(ToInt(value), format);

        public string AsString(TEnum value, EnumFormat format) => Cache.AsString(ToInt(value), format);

        public string AsString(TEnum value, EnumFormat format0, EnumFormat format1) => Cache.AsString(ToInt(value), format0, format1);

        public string AsString(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Cache.AsString(ToInt(value), format0, format1, format2);

        public string Format(TEnum value, EnumFormat[] formatOrder) => Cache.Format(ToInt(value), formatOrder);

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
        public EnumMember<TEnum> GetEnumMember(TEnum value)
        {
            var member = Cache.GetEnumMember(ToInt(value));
            return member.IsDefined ? new EnumMember<TEnum, TInt, TIntProvider>(member) : null;
        }

        public EnumMember<TEnum> GetEnumMember(string name, bool ignoreCase)
        {
            var member = Cache.GetEnumMember(name, ignoreCase);
            return member.IsDefined ? new EnumMember<TEnum, TInt, TIntProvider>(member) : null;
        }

        public string GetName(TEnum value) => Cache.GetName(ToInt(value));

        public string GetDescription(TEnum value) => Cache.GetDescription(ToInt(value));

        public string GetDescriptionOrName(TEnum value) => Cache.GetDescriptionOrName(ToInt(value));

        public string GetDescriptionOrName(TEnum value, Func<string, string> nameFormatter) => Cache.GetDescriptionOrName(ToInt(value), nameFormatter);
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

        public string FormatFlags(TEnum value, string delimiter, EnumFormat[] formatOrder) => Cache.FormatFlags(ToInt(value), delimiter, formatOrder);

        public IEnumerable<TEnum> GetFlags(TEnum value) => Cache.GetFlags(ToInt(value)).Select(flag => ToEnum(flag));

        public IEnumerable<EnumMember<TEnum>> GetFlagMembers(TEnum value) => Cache.GetFlagMembers(ToInt(value)).Select(flag =>
#if NET20 || NET35
            (EnumMember<TEnum>)
#endif
            new EnumMember<TEnum, TInt, TIntProvider>(flag));

        public bool HasAnyFlags(TEnum value) => Cache.HasAnyFlags(ToInt(value));

        public bool HasAnyFlags(TEnum value, TEnum otherFlags) => Cache.HasAnyFlags(ToInt(value), ToInt(otherFlags));

        public bool HasAllFlags(TEnum value) => Cache.HasAllFlags(ToInt(value));

        public bool HasAllFlags(TEnum value, TEnum otherFlags) => Cache.HasAllFlags(ToInt(value), ToInt(otherFlags));

        public TEnum ToggleFlags(TEnum value) => ToEnum(Cache.ToggleFlags(ToInt(value)));

        public TEnum ToggleFlags(TEnum value, TEnum otherFlags) => ToEnum(Cache.ToggleFlags(ToInt(value), ToInt(otherFlags)));

        public TEnum CommonFlags(TEnum value, TEnum otherFlags) => ToEnum(Cache.CommonFlags(ToInt(value), ToInt(otherFlags)));

        public TEnum CombineFlags(TEnum value, TEnum otherFlags) => ToEnum(Cache.CombineFlags(ToInt(value), ToInt(otherFlags)));

        public TEnum CombineFlags(TEnum flag0, TEnum flag1, TEnum flag2) => ToEnum(Cache.CombineFlags(ToInt(flag0), ToInt(flag1), ToInt(flag2)));

        public TEnum CombineFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3) => ToEnum(Cache.CombineFlags(ToInt(flag0), ToInt(flag1), ToInt(flag2), ToInt(flag3)));

        public TEnum CombineFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4) => ToEnum(Cache.CombineFlags(ToInt(flag0), ToInt(flag1), ToInt(flag2), ToInt(flag3), ToInt(flag4)));

        public TEnum CombineFlags(TEnum[] flags) => ToEnum(Cache.CombineFlags(flags?.Select(flag => ToInt(flag))));

        public TEnum ExcludeFlags(TEnum value, TEnum otherFlags) => ToEnum(Cache.ExcludeFlags(ToInt(value), ToInt(otherFlags)));
        #endregion

        #region Parsing
        public TEnum ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder) => ToEnum(Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));

        public bool TryParseFlags(string value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat[] parseFormatOrder)
        {
            TInt resultAsInt;
            var success = Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsInt, parseFormatOrder);
            result = ToEnum(resultAsInt);
            return success;
        }
        #endregion
        #endregion

        #region CustomEnumFormatters
        public EnumFormat RegisterCustomEnumFormat(Func<EnumMember<TEnum>, string> formatter) => InternalRegisterCustomEnumFormat(formatter);

        internal static EnumFormat InternalRegisterCustomEnumFormat(Func<EnumMember<TEnum>, string> formatter)
        {
            Preconditions.NotNull(formatter, nameof(formatter));

            var index = Interlocked.Increment(ref _lastCustomEnumFormatIndex);
            if (index == 0)
            {
                _customEnumFormatters = new List<Func<EnumMember<TEnum>, string>>();
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

        private static Func<InternalEnumMember<TInt, TIntProvider>, string> InternalGetCustomEnumFormatter(EnumFormat format)
        {
#if NET20 || NET35
            var formatter1 = Enums.GetCustomEnumFormatter(format);
            if (formatter1 != null)
            {
                return member => formatter1(new EnumMember<TEnum, TInt, TIntProvider>(member));
            }
            var formatter2 = GetCustomEnumFormatter(format);
            return formatter2 != null ? member => formatter2(new EnumMember<TEnum, TInt, TIntProvider>(member)) : (Func<InternalEnumMember<TInt, TIntProvider>, string>)null;
#else
            var formatter = Enums.GetCustomEnumFormatter(format) ?? GetCustomEnumFormatter(format);
            return formatter != null ? member => formatter(new EnumMember<TEnum, TInt, TIntProvider>(member)) : (Func<InternalEnumMember<TInt, TIntProvider>, string>)null;
#endif
        }

        private static Func<EnumMember<TEnum>, string> GetCustomEnumFormatter(EnumFormat format)
        {
            var index = (int)format - Enums.StartingGenericCustomEnumFormatValue;
            return index >= 0 && index < _customEnumFormatters?.Count ? _customEnumFormatters[index] : null;
        }
        #endregion
    }
}
