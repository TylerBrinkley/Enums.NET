﻿#region License
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
using EnumsNET.Numerics;

namespace EnumsNET
{
    internal interface ICommonEnumInfo
    {
#if ICONVERTIBLE
        TypeCode TypeCode { get; }
#endif
        Type UnderlyingType { get; }
        bool IsFlagEnum { get; }

        int GetMemberCount(EnumMemberSelection selection = EnumMemberSelection.All);
        IEnumerable<string> GetNames(EnumMemberSelection selection = EnumMemberSelection.All);
    }

    internal interface IEnumInfo : ICommonEnumInfo
    {
        object AllFlags { get; }

        string AsString(object value);
        string AsString(object value, string format);
        string AsString(object value, EnumFormat format);
        string AsString(object value, EnumFormat format0, EnumFormat format1);
        string AsString(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2);
        string AsString(object value, EnumFormat[] formats);
        object RemoveFlags(object value, object otherFlags);
        object CommonFlags(object value, object otherFlags);
        int CompareTo(object value, object other);
        bool Equals(object value, object other);
        string Format(object value, string format);
        string Format(object value, EnumFormat[] formats);
        string FormatFlags(object value, string delimiter, EnumFormat[] formats);
        EnumMember GetMember(object value);
        EnumMember GetMember(string value, bool ignoreCase = false, EnumFormat[] formats = null);
        IEnumerable<EnumMember> GetMembers(EnumMemberSelection selection = EnumMemberSelection.All);
        IEnumerable<object> GetFlags(object value);
        IEnumerable<EnumMember> GetFlagMembers(object value);
        string GetName(object value);
        object GetUnderlyingValue(object value);
        IEnumerable<object> GetValues(EnumMemberSelection selection = EnumMemberSelection.All);
        bool HasAllFlags(object value);
        bool HasAllFlags(object value, object otherFlags);
        bool HasAnyFlags(object value);
        bool HasAnyFlags(object value, object otherFlags);
        bool IsDefined(object value);
        bool IsValid(object value);
        bool IsValidFlagCombination(object value);
        object Parse(string value, bool ignoreCase, EnumFormat[] formats);
        object ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] formats);
        object CombineFlags(IEnumerable<object> flags);
        object CombineFlags(object value, object otherFlags);
        byte ToByte(object value);
        object ToggleFlags(object value);
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
        bool TryParse(string value, bool ignoreCase, out object result, EnumFormat[] formats);
        bool TryParseFlags(string value, bool ignoreCase, string delimiter, out object result, EnumFormat[] formats);
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
        string AsString(TEnum value, EnumFormat[] formats);
        TEnum RemoveFlags(TEnum value, TEnum otherFlags);
        TEnum CommonFlags(TEnum value, TEnum otherFlags);
        int CompareTo(TEnum value, TEnum other);
        bool Equals(TEnum value, TEnum other);
        string Format(TEnum value, string format);
        string Format(TEnum value, EnumFormat[] formats);
        string FormatFlags(TEnum value, string delimiter = null, EnumFormat[] formats = null);
        EnumMember<TEnum> GetMember(TEnum value);
        EnumMember<TEnum> GetMember(string value, bool ignoreCase = false, EnumFormat[] formats = null);
        IEnumerable<EnumMember<TEnum>> GetMembers(EnumMemberSelection selection = EnumMemberSelection.All);
        IEnumerable<TEnum> GetFlags(TEnum value);
        IEnumerable<EnumMember<TEnum>> GetFlagMembers(TEnum value);
        int GetHashCode(TEnum value);
        string GetName(TEnum value);
        object GetUnderlyingValue(TEnum value);
        IEnumerable<TEnum> GetValues(EnumMemberSelection selection = EnumMemberSelection.All);
        bool HasAllFlags(TEnum value);
        bool HasAllFlags(TEnum value, TEnum otherFlags);
        bool HasAnyFlags(TEnum value);
        bool HasAnyFlags(TEnum value, TEnum otherFlags);
        bool IsDefined(TEnum value);
        bool IsValid(TEnum value);
        bool IsValidFlagCombination(TEnum value);
        TEnum Parse(string value, bool ignoreCase = false, EnumFormat[] formats = null);
        TEnum ParseFlags(string value, bool ignoreCase = false, string delimiter = null, EnumFormat[] formats = null);
        TEnum CombineFlags(IEnumerable<TEnum> flags);
        TEnum CombineFlags(TEnum value, TEnum otherFlags);
        TEnum CombineFlags(TEnum flag0, TEnum flag1, TEnum flag2);
        TEnum CombineFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3);
        TEnum CombineFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4);
        byte ToByte(TEnum value);
        TEnum ToggleFlags(TEnum value);
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
        bool TryParse(string value, bool ignoreCase, out TEnum result, EnumFormat[] formats = null);
        bool TryParseFlags(string value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat[] formats = null);
        bool TryToObject(ulong value, out TEnum result, bool validate = false);
        bool TryToObject(object value, out TEnum result, bool validate = false);
        bool TryToObject(long value, out TEnum result, bool validate = false);
        TEnum Validate(TEnum value, string paramName);
    }

    internal interface IEnumInfoInternal<TInt, TIntProvider>
        where TInt : struct, IComparable<TInt>, IEquatable<TInt>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TIntProvider : struct, INumericProvider<TInt>
    {
        bool HasCustomValidator { get; }

        EnumMember CreateEnumMember(EnumMemberInternal<TInt, TIntProvider> member);
        bool CustomValidate(TInt value);
    }
}