// Enums.NET
// Copyright 2016 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
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
    internal interface IEnumsCache
    {
        #region Properties
        bool IsContiguous { get; }

        Type UnderlyingType { get; }

        TypeCode TypeCode { get; }

        bool IsFlagEnum { get; }

        object AllFlags { get; }
        #endregion

        #region Standard Enum Operations
        #region Type Methods
        int GetDefinedCount(bool uniqueValued);

        IEnumerable<string> GetNames(bool uniqueValued);

        IEnumerable<object> GetValues(bool uniqueValued);

        IEnumerable<string> GetDescriptions(bool uniqueValued);

        IEnumerable<Attribute[]> GetAllAttributes(bool uniqueValued);

        IEnumerable<string> GetDescriptionsOrNames(bool uniqueValued);

        IEnumerable<string> GetDescriptionsOrNames(Func<string, string> nameFormatter, bool uniqueValued);

        IEnumerable<EnumMemberInfo> GetEnumMemberInfos(bool uniqueValued);

        int Compare(object x, object y);

        bool Equals(object x, object y);

        EnumFormat RegisterCustomEnumFormat(Func<IEnumMemberInfo, string> formatter);
        #endregion

        #region IsValid
        bool IsValid(object value);

        bool IsValid(long value);

        bool IsValid(ulong value);
        #endregion

        #region IsDefined
        bool IsDefined(object value);

        bool IsDefined(string name, bool ignoreCase = false);

        bool IsDefined(long value);

        bool IsDefined(ulong value);
        #endregion

        #region IsInValueRange
        bool IsInValueRange(long value);

        bool IsInValueRange(ulong value);
        #endregion

        #region ToEnum
        object ToObject(object value, bool validate);

        object ToObject(long value, bool validate);

        object ToObject(ulong value, bool validate);

        object ToObjectOrDefault(object value, object defaultEnum, bool validate);

        object ToObjectOrDefault(long value, object defaultEnum, bool validate);

        object ToObjectOrDefault(ulong value, object defaultEnum, bool validate);

        bool TryToObject(object value, out object result, bool validate);

        bool TryToObject(long value, out object result, bool validate);

        bool TryToObject(ulong value, out object result, bool validate);
        #endregion

        #region All Values Main Methods
        object Validate(object value, string paramName);

        string AsString(object value);

        string AsString(object value, string format);

        string AsString(object value, EnumFormat[] formats);

        string Format(object value, string format);

        string Format(object value, EnumFormat format);

        string Format(object value, EnumFormat format0, EnumFormat format1);

        string Format(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2);

        string Format(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3);

        string Format(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4);

        string Format(object value, EnumFormat[] formats);

        object GetUnderlyingValue(object value);

        sbyte ToSByte(object value);

        byte ToByte(object value);

        short ToInt16(object value);

        ushort ToUInt16(object value);

        int ToInt32(object value);

        uint ToUInt32(object value);

        long ToInt64(object value);

        ulong ToUInt64(object value);
        #endregion

        #region Defined Values Main Methods
        EnumMemberInfo GetEnumMemberInfo(object value);

        EnumMemberInfo GetEnumMemberInfo(string name, bool ignoreCase = false);

        string GetName(object value);

        string GetDescription(object value);

        string GetDescriptionOrName(object value);

        string GetDescriptionOrName(object value, Func<string, string> nameFormatter);
        #endregion

        #region Attributes
        Attribute[] GetAllAttributes(object value);
        #endregion

        #region Parsing
        object Parse(string value);

        object Parse(string value, EnumFormat[] parseFormatOrder);

        object Parse(string value, bool ignoreCase);

        object Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder);

        object ParseOrDefault(string value, object defaultEnum);

        object ParseOrDefault(string value, object defaultEnum, EnumFormat[] parseFormatOrder);

        object ParseOrDefault(string value, bool ignoreCase, object defaultEnum);

        object ParseOrDefault(string value, bool ignoreCase, object defaultEnum, EnumFormat[] parseFormatOrder);

        bool TryParse(string value, out object result);

        bool TryParse(string value, out object result, EnumFormat[] parseFormatOrder);

        bool TryParse(string value, bool ignoreCase, out object result);

        bool TryParse(string value, bool ignoreCase, out object result, EnumFormat[] parseFormatOrder);
        #endregion
        #endregion

        #region Flag Enum Operations
        #region Main Methods
        bool IsValidFlagCombination(object value);

        string FormatAsFlags(object value);

        string FormatAsFlags(object value, string delimiter);

        string FormatAsFlags(object value, EnumFormat[] formats);

        string FormatAsFlags(object value, string delimiter, EnumFormat[] formats);

        object[] GetFlags(object value);

        bool HasAnyFlags(object value);

        bool HasAnyFlags(object value, object flagMask);

        bool HasAllFlags(object value);

        bool HasAllFlags(object value, object flagMask);

        object InvertFlags(object value);

        object InvertFlags(object value, object flagMask);

        object CommonFlags(object value, object flagMask);

        object SetFlags(object flag0, object flag1);

        object SetFlags(object flag0, object flag1, object flag2);

        object SetFlags(object flag0, object flag1, object flag2, object flag3);

        object SetFlags(object flag0, object flag1, object flag2, object flag3, object flag4);

        object SetFlags(object[] flags);

        object ClearFlags(object value, object flagMask);
        #endregion

        #region Parsing
        object ParseFlags(string value);

        object ParseFlags(string value, EnumFormat[] parseFormatOrder);

        object ParseFlags(string value, bool ignoreCase);

        object ParseFlags(string value, bool ignoreCase, EnumFormat[] parseFormatOrder);

        object ParseFlags(string value, string delimiter);

        object ParseFlags(string value, string delimiter, EnumFormat[] parseFormatOrder);

        object ParseFlags(string value, bool ignoreCase, string delimiter);

        object ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder);

        object ParseFlagsOrDefault(string value, object defaultEnum);

        object ParseFlagsOrDefault(string value, object defaultEnum, EnumFormat[] parseFormatOrder);

        object ParseFlagsOrDefault(string value, bool ignoreCase, object defaultEnum);

        object ParseFlagsOrDefault(string value, bool ignoreCase, object defaultEnum, EnumFormat[] parseFormatOrder);

        object ParseFlagsOrDefault(string value, string delimiter, object defaultEnum);

        object ParseFlagsOrDefault(string value, string delimiter, object defaultEnum, EnumFormat[] parseFormatOrder);

        object ParseFlagsOrDefault(string value, bool ignoreCase, string delimiter, object defaultEnum);

        object ParseFlagsOrDefault(string value, bool ignoreCase, string delimiter, object defaultEnum, EnumFormat[] parseFormatOrder);

        bool TryParseFlags(string value, out object result);

        bool TryParseFlags(string value, out object result, EnumFormat[] parseFormatOrder);

        bool TryParseFlags(string value, bool ignoreCase, out object result);

        bool TryParseFlags(string value, bool ignoreCase, out object result, EnumFormat[] parseFormatOrder);

        bool TryParseFlags(string value, string delimiter, out object result);

        bool TryParseFlags(string value, string delimiter, out object result, EnumFormat[] parseFormatOrder);

        bool TryParseFlags(string value, bool ignoreCase, string delimiter, out object result);

        bool TryParseFlags(string value, bool ignoreCase, string delimiter, out object result, EnumFormat[] parseFormatOrder);
        #endregion
        #endregion
    }

    internal interface IEnumsCache<TEnum> : IEnumsCache
    {
        #region Properties
        new TEnum AllFlags { get; }
        #endregion

        #region Standard Enum Operations
        #region Type Methods
        new IEnumerable<TEnum> GetValues(bool uniqueValued);

        new IEnumerable<EnumMemberInfo<TEnum>> GetEnumMemberInfos(bool uniqueValued);

        IEnumerable<TAttribute> GetAttributes<TAttribute>(bool uniqueValued) where TAttribute : Attribute;

        int Compare(TEnum x, TEnum y);

        bool Equals(TEnum x, TEnum y);

        int GetHashCode(TEnum x);

        EnumFormat RegisterCustomEnumFormat(Func<IEnumMemberInfo<TEnum>, string> formatter);
        #endregion

        #region IsValid
        bool IsValid(TEnum value);
        #endregion

        #region IsDefined
        bool IsDefined(TEnum value);
        #endregion

        #region ToEnum
        new TEnum ToObject(object value, bool validate);

        new TEnum ToObject(long value, bool validate);

        new TEnum ToObject(ulong value, bool validate);

        TEnum ToObjectOrDefault(object value, TEnum defaultEnum, bool validate);

        TEnum ToObjectOrDefault(long value, TEnum defaultEnum, bool validate);

        TEnum ToObjectOrDefault(ulong value, TEnum defaultEnum, bool validate);

        bool TryToObject(object value, out TEnum result, bool validate);

        bool TryToObject(long value, out TEnum result, bool validate);

        bool TryToObject(ulong value, out TEnum result, bool validate);
        #endregion

        #region All Values Main Methods
        TEnum Validate(TEnum value, string paramName);

        string AsString(TEnum value);

        string AsString(TEnum value, string format);

        string AsString(TEnum value, EnumFormat[] formats);

        string Format(TEnum value, string format);

        string Format(TEnum value, EnumFormat format);

        string Format(TEnum value, EnumFormat format0, EnumFormat format1);

        string Format(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2);

        string Format(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3);

        string Format(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4);

        string Format(TEnum value, EnumFormat[] formats);

        object GetUnderlyingValue(TEnum value);

        sbyte ToSByte(TEnum value);

        byte ToByte(TEnum value);

        short ToInt16(TEnum value);

        ushort ToUInt16(TEnum value);

        int ToInt32(TEnum value);

        uint ToUInt32(TEnum value);

        long ToInt64(TEnum value);

        ulong ToUInt64(TEnum value);
        #endregion

        #region Defined Values Main Methods
        EnumMemberInfo<TEnum> GetEnumMemberInfo(TEnum value);

        new EnumMemberInfo<TEnum> GetEnumMemberInfo(string name, bool ignoreCase = false);

        string GetName(TEnum value);

        string GetDescription(TEnum value);

        string GetDescriptionOrName(TEnum value);

        string GetDescriptionOrName(TEnum value, Func<string, string> nameFormatter);
        #endregion

        #region Attributes
        bool HasAttribute<TAttribute>(TEnum value) where TAttribute : Attribute;

        TAttribute GetAttribute<TAttribute>(TEnum value) where TAttribute : Attribute;

        TResult GetAttributeSelect<TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, TResult defaultValue) where TAttribute : Attribute;

        bool TryGetAttributeSelect<TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, out TResult result) where TAttribute : Attribute;

        IEnumerable<TAttribute> GetAttributes<TAttribute>(TEnum value) where TAttribute : Attribute;

        Attribute[] GetAllAttributes(TEnum value);
        #endregion

        #region Parsing
        new TEnum Parse(string value);

        new TEnum Parse(string value, EnumFormat[] parseFormatOrder);

        new TEnum Parse(string value, bool ignoreCase);

        new TEnum Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder);

        TEnum ParseOrDefault(string value, TEnum defaultEnum);

        TEnum ParseOrDefault(string value, TEnum defaultEnum, EnumFormat[] parseFormatOrder);

        TEnum ParseOrDefault(string value, bool ignoreCase, TEnum defaultEnum);

        TEnum ParseOrDefault(string value, bool ignoreCase, TEnum defaultEnum, EnumFormat[] parseFormatOrder);

        bool TryParse(string value, out TEnum result);

        bool TryParse(string value, out TEnum result, EnumFormat[] parseFormatOrder);

        bool TryParse(string value, bool ignoreCase, out TEnum result);

        bool TryParse(string value, bool ignoreCase, out TEnum result, EnumFormat[] parseFormatOrder);
        #endregion
        #endregion

        #region Flag Enum Operations
        #region Main Methods
        bool IsValidFlagCombination(TEnum value);

        string FormatAsFlags(TEnum value);

        string FormatAsFlags(TEnum value, string delimiter);

        string FormatAsFlags(TEnum value, EnumFormat[] formats);

        string FormatAsFlags(TEnum value, string delimiter, EnumFormat[] formats);

        TEnum[] GetFlags(TEnum value);

        bool HasAnyFlags(TEnum value);

        bool HasAnyFlags(TEnum value, TEnum flagMask);

        bool HasAllFlags(TEnum value);

        bool HasAllFlags(TEnum value, TEnum flagMask);

        TEnum InvertFlags(TEnum value);

        TEnum InvertFlags(TEnum value, TEnum flagMask);

        TEnum CommonFlags(TEnum value, TEnum flagMask);

        TEnum SetFlags(TEnum flag0, TEnum flag1);

        TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2);

        TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3);

        TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4);

        TEnum SetFlags(TEnum[] flags);

        TEnum ClearFlags(TEnum value, TEnum flagMask);
        #endregion

        #region Parsing
        new TEnum ParseFlags(string value);

        new TEnum ParseFlags(string value, EnumFormat[] parseFormatOrder);

        new TEnum ParseFlags(string value, bool ignoreCase);

        new TEnum ParseFlags(string value, bool ignoreCase, EnumFormat[] parseFormatOrder);

        new TEnum ParseFlags(string value, string delimiter);

        new TEnum ParseFlags(string value, string delimiter, EnumFormat[] parseFormatOrder);

        new TEnum ParseFlags(string value, bool ignoreCase, string delimiter);

        new TEnum ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder);

        TEnum ParseFlagsOrDefault(string value, TEnum defaultEnum);

        TEnum ParseFlagsOrDefault(string value, TEnum defaultEnum, EnumFormat[] parseFormatOrder);

        TEnum ParseFlagsOrDefault(string value, bool ignoreCase, TEnum defaultEnum);

        TEnum ParseFlagsOrDefault(string value, bool ignoreCase, TEnum defaultEnum, EnumFormat[] parseFormatOrder);

        TEnum ParseFlagsOrDefault(string value, string delimiter, TEnum defaultEnum);

        TEnum ParseFlagsOrDefault(string value, string delimiter, TEnum defaultEnum, EnumFormat[] parseFormatOrder);

        TEnum ParseFlagsOrDefault(string value, bool ignoreCase, string delimiter, TEnum defaultEnum);

        TEnum ParseFlagsOrDefault(string value, bool ignoreCase, string delimiter, TEnum defaultEnum, EnumFormat[] parseFormatOrder);

        bool TryParseFlags(string value, out TEnum result);

        bool TryParseFlags(string value, out TEnum result, EnumFormat[] parseFormatOrder);

        bool TryParseFlags(string value, bool ignoreCase, out TEnum result);

        bool TryParseFlags(string value, bool ignoreCase, out TEnum result, EnumFormat[] parseFormatOrder);

        bool TryParseFlags(string value, string delimiter, out TEnum result);

        bool TryParseFlags(string value, string delimiter, out TEnum result, EnumFormat[] parseFormatOrder);

        bool TryParseFlags(string value, bool ignoreCase, string delimiter, out TEnum result);

        bool TryParseFlags(string value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat[] parseFormatOrder);
        #endregion
        #endregion
    }
}
