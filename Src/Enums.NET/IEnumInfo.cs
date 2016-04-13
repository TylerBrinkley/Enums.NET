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

        int GetDefinedCount(bool uniqueValued);
        IEnumerable<string> GetNames(bool uniqueValued);
        bool IsDefined(object value);
        bool IsDefined(ulong value);
        bool IsDefined(long value);
        bool IsDefined(string name, bool ignoreCase);
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
        string AsString(object value, EnumFormat[] formats);
        object ClearFlags(object value, object flagMask);
        object CommonFlags(object value, object flagMask);
        int Compare(object x, object y);
        bool Equals(object value, object other);
        string Format(object value, string format);
        string Format(object value, EnumFormat[] formats);
        string Format(object value, EnumFormat format);
        string Format(object value, EnumFormat format0, EnumFormat format1);
        string Format(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2);
        string FormatAsFlags(object value, string delimiter, EnumFormat[] formats);
        IEnumerable<Attribute> GetAttributes(object value);
        string GetDescription(object value);
        EnumMemberInfo GetEnumMemberInfo(object value);
        EnumMemberInfo GetEnumMemberInfo(string name, bool ignoreCase);
        IEnumerable<EnumMemberInfo> GetEnumMemberInfos(bool uniqueValued);
        IEnumerable<object> GetFlags(object value);
        string GetName(object value);
        object GetUnderlyingValue(object value);
        IEnumerable<object> GetValues(bool uniqueValued);
        bool HasAllFlags(object value);
        bool HasAllFlags(object value, object flagMask);
        bool HasAnyFlags(object value);
        bool HasAnyFlags(object value, object flagMask);
        bool IsValidFlagCombination(object value);
        object Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder);
        object ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder);
        EnumFormat RegisterCustomEnumFormat(Func<EnumMemberInfo, string> formatter);
        object SetFlags(IEnumerable<object> flags);
        object SetFlags(object flag0, object flag1);
        byte ToByte(object value);
        object ToggleFlags(object value);
        object ToggleFlags(object value, object flagMask);
        short ToInt16(object value);
        int ToInt32(object value);
        long ToInt64(object value);
        object ToObject(ulong value, bool validate);
        object ToObject(object value, bool validate = false);
        object ToObject(long value, bool validate);
        sbyte ToSByte(object value);
        ushort ToUInt16(object value);
        uint ToUInt32(object value);
        ulong ToUInt64(object value);
        bool TryParse(string value, bool ignoreCase, out object result, EnumFormat[] parseFormatOrder);
        bool TryParseFlags(string value, bool ignoreCase, string delimiter, out object result, EnumFormat[] parseFormatOrder);
        bool TryToObject(ulong value, out object result, bool validate);
        bool TryToObject(object value, out object result, bool validate);
        bool TryToObject(long value, out object result, bool validate);
        object Validate(object value, string paramName);
    }

    internal interface IEnumInfo<TEnum> : ICommonEnumInfo
    {
        TEnum AllFlags { get; }

        string AsString(TEnum value);
        string AsString(TEnum value, string format);
        string AsString(TEnum value, EnumFormat[] formats);
        TEnum ClearFlags(TEnum value, TEnum flagMask);
        TEnum CommonFlags(TEnum value, TEnum flagMask);
        int Compare(TEnum x, TEnum y);
        bool Equals(TEnum value, TEnum other);
        string Format(TEnum value, string format);
        string Format(TEnum value, EnumFormat[] formats);
        string Format(TEnum value, EnumFormat format);
        string Format(TEnum value, EnumFormat format0, EnumFormat format1);
        string Format(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2);
        string FormatAsFlags(TEnum value, string delimiter, EnumFormat[] formats);
        TAttribute GetAttribute<TAttribute>(TEnum value) where TAttribute : Attribute;
        IEnumerable<Attribute> GetAttributes(TEnum value);
        IEnumerable<TAttribute> GetAttributes<TAttribute>(TEnum value) where TAttribute : Attribute;
        TResult GetAttributeSelect<TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, TResult defaultValue) where TAttribute : Attribute;
        string GetDescription(TEnum value);
        EnumMemberInfo<TEnum> GetEnumMemberInfo(TEnum value);
        EnumMemberInfo<TEnum> GetEnumMemberInfo(string name, bool ignoreCase);
        IEnumerable<EnumMemberInfo<TEnum>> GetEnumMemberInfos(bool uniqueValued);
        IEnumerable<TEnum> GetFlags(TEnum value);
        int GetHashCode(TEnum value);
        string GetName(TEnum value);
        object GetUnderlyingValue(TEnum value);
        IEnumerable<TEnum> GetValues(bool uniqueValued);
        bool HasAllFlags(TEnum value);
        bool HasAllFlags(TEnum value, TEnum flagMask);
        bool HasAnyFlags(TEnum value);
        bool HasAnyFlags(TEnum value, TEnum flagMask);
        bool IsDefined(TEnum value);
        bool IsValid(TEnum value);
        bool IsValidFlagCombination(TEnum value);
        TEnum Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder);
        TEnum ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder);
        EnumFormat RegisterCustomEnumFormat(Func<EnumMemberInfo<TEnum>, string> formatter);
        TEnum SetFlags(TEnum[] flags);
        TEnum SetFlags(TEnum flag0, TEnum flag1);
        TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2);
        TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3);
        TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4);
        byte ToByte(TEnum value);
        TEnum ToggleFlags(TEnum value);
        TEnum ToggleFlags(TEnum value, TEnum flagMask);
        short ToInt16(TEnum value);
        int ToInt32(TEnum value);
        long ToInt64(TEnum value);
        TEnum ToObject(ulong value, bool validate);
        TEnum ToObject(object value, bool validate);
        TEnum ToObject(long value, bool validate);
        sbyte ToSByte(TEnum value);
        ushort ToUInt16(TEnum value);
        uint ToUInt32(TEnum value);
        ulong ToUInt64(TEnum value);
        bool TryGetAttributeSelect<TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, out TResult result) where TAttribute : Attribute;
        bool TryParse(string value, bool ignoreCase, out TEnum result, EnumFormat[] parseFormatOrder);
        bool TryParseFlags(string value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat[] parseFormatOrder);
        bool TryToObject(ulong value, out TEnum result, bool validate);
        bool TryToObject(object value, out TEnum result, bool validate);
        bool TryToObject(long value, out TEnum result, bool validate);
        TEnum Validate(TEnum value, string paramName);
    }
}