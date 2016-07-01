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

namespace EnumsNET
{
    internal interface ICommonEnumInfo
    {
        TypeCode TypeCode { get; }
        Type UnderlyingType { get; }
        bool IsContiguous { get; }
        bool IsFlagEnum { get; }

        int GetDefinedCount(bool uniqueValued = false);
        IEnumerable<string> GetNames(bool uniqueValued = false);
        bool IsDefined(object value);
        bool IsDefined(ulong value);
        bool IsDefined(long value);
        bool IsDefined(string name, bool ignoreCase = false);
        bool IsInValueRange(ulong value);
        bool IsInValueRange(long value);
        bool IsValid(ulong value);
        bool IsValid(object value);
        bool IsValid(long value);
    }

    internal interface IEnumInfo : ICommonEnumInfo
    {
        object AllFlags { get; }

        string AsString(object value);
        string AsString(object value, string format);
        string AsString(object value, EnumFormat[] formatOrder);
        object ExcludeFlags(object value, object otherFlags);
        object CommonFlags(object value, object otherFlags);
        int Compare(object x, object y);
        bool Equals(object value, object other);
        string Format(object value, string format);
        string Format(object value, EnumFormat[] formatOrder);
        string Format(object value, EnumFormat format);
        string Format(object value, EnumFormat format0, EnumFormat format1);
        string Format(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2);
        string FormatAsFlags(object value, string delimiter, EnumFormat[] formatOrder);
        IEnumerable<Attribute> GetAttributes(object value);
        string GetDescription(object value);
        string GetDescriptionOrName(object value);
        string GetDescriptionOrName(object value, Func<string, string> nameFormatter);
        EnumMember GetEnumMember(object value);
        EnumMember GetEnumMember(string name, bool ignoreCase = false);
        IEnumerable<EnumMember> GetEnumMembers(bool uniqueValued = false);
        IEnumerable<object> GetFlags(object value);
        string GetName(object value);
        object GetUnderlyingValue(object value);
        IEnumerable<object> GetValues(bool uniqueValued = false);
        bool HasAllFlags(object value);
        bool HasAllFlags(object value, object otherFlags);
        bool HasAnyFlags(object value);
        bool HasAnyFlags(object value, object otherFlags);
        bool IsValidFlagCombination(object value);
        object Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder);
        object ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder);
        EnumFormat RegisterCustomEnumFormat(Func<EnumMember, string> formatter);
        object CombineFlags(IEnumerable<object> flags);
        object CombineFlags(object value, object otherFlags);
        byte ToByte(object value);
        object ToggleFlags(object value, bool toggleValidFlagsOnly = true);
        object ToggleFlags(object value, object otherFlags);
        short ToInt16(object value);
        int ToInt32(object value);
        long ToInt64(object value);
        object ToObject(ulong value, bool validate = false);
        object ToObject(object value, bool validate = false);
        object ToObject(long value, bool validate = false);
        sbyte ToSByte(object value);
        ushort ToUInt16(object value);
        uint ToUInt32(object value);
        ulong ToUInt64(object value);
        bool TryParse(string value, bool ignoreCase, out object result, EnumFormat[] parseFormatOrder);
        bool TryParseFlags(string value, bool ignoreCase, string delimiter, out object result, EnumFormat[] parseFormatOrder);
        bool TryToObject(ulong value, out object result, bool validate = false);
        bool TryToObject(object value, out object result, bool validate = false);
        bool TryToObject(long value, out object result, bool validate = false);
        object Validate(object value, string paramName);
    }

    internal interface IEnumInfo<TEnum> : ICommonEnumInfo
    {
        TEnum AllFlags { get; }

        string AsString(TEnum value);
        string AsString(TEnum value, string format);
        string AsString(TEnum value, EnumFormat format);
        string AsString(TEnum value, EnumFormat format0, EnumFormat format1);
        string AsString(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2);
        string AsString(TEnum value, EnumFormat[] formatOrder);
        TEnum ExcludeFlags(TEnum value, TEnum otherFlags);
        TEnum CommonFlags(TEnum value, TEnum otherFlags);
        int Compare(TEnum x, TEnum y);
        bool Equals(TEnum value, TEnum other);
        string Format(TEnum value, string format);
        string Format(TEnum value, EnumFormat[] formatOrder);
        string FormatAsFlags(TEnum value, string delimiter, EnumFormat[] formatOrder);
        TAttribute GetAttribute<TAttribute>(TEnum value) where TAttribute : Attribute;
        IEnumerable<Attribute> GetAttributes(TEnum value);
        IEnumerable<TAttribute> GetAttributes<TAttribute>(TEnum value) where TAttribute : Attribute;
        TResult GetAttributeSelect<TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, TResult defaultValue = default(TResult)) where TAttribute : Attribute;
        string GetDescription(TEnum value);
        string GetDescriptionOrName(TEnum value);
        string GetDescriptionOrName(TEnum value, Func<string, string> nameFormatter);
        EnumMember<TEnum> GetEnumMember(TEnum value);
        EnumMember<TEnum> GetEnumMember(string name, bool ignoreCase = false);
        IEnumerable<EnumMember<TEnum>> GetEnumMembers(bool uniqueValued = false);
        IEnumerable<TEnum> GetFlags(TEnum value);
        int GetHashCode(TEnum value);
        string GetName(TEnum value);
        object GetUnderlyingValue(TEnum value);
        IEnumerable<TEnum> GetValues(bool uniqueValued = false);
        bool HasAllFlags(TEnum value);
        bool HasAllFlags(TEnum value, TEnum otherFlags);
        bool HasAnyFlags(TEnum value);
        bool HasAnyFlags(TEnum value, TEnum otherFlags);
        bool IsDefined(TEnum value);
        bool IsValid(TEnum value);
        bool IsValidFlagCombination(TEnum value);
        TEnum Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder);
        TEnum ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder);
        EnumFormat RegisterCustomEnumFormat(Func<EnumMember<TEnum>, string> formatter);
        TEnum CombineFlags(TEnum[] flags);
        TEnum CombineFlags(TEnum value, TEnum otherFlags);
        byte ToByte(TEnum value);
        TEnum ToggleFlags(TEnum value, bool toggleValidFlagsOnly = true);
        TEnum ToggleFlags(TEnum value, TEnum otherFlags);
        short ToInt16(TEnum value);
        int ToInt32(TEnum value);
        long ToInt64(TEnum value);
        TEnum ToObject(ulong value, bool validate = false);
        TEnum ToObject(object value, bool validate = false);
        TEnum ToObject(long value, bool validate = false);
        sbyte ToSByte(TEnum value);
        ushort ToUInt16(TEnum value);
        uint ToUInt32(TEnum value);
        ulong ToUInt64(TEnum value);
        bool TryGetAttributeSelect<TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, out TResult result) where TAttribute : Attribute;
        bool TryParse(string value, bool ignoreCase, out TEnum result, EnumFormat[] parseFormatOrder);
        bool TryParseFlags(string value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat[] parseFormatOrder);
        bool TryToObject(ulong value, out TEnum result, bool validate = false);
        bool TryToObject(object value, out TEnum result, bool validate = false);
        bool TryToObject(long value, out TEnum result, bool validate = false);
        TEnum Validate(TEnum value, string paramName);
    }
}