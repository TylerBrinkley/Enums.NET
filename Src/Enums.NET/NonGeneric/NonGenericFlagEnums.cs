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
using System.ComponentModel;

namespace EnumsNET.NonGeneric
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Obsolete("NonGenericFlagEnums members have moved to the FlagEnums static class")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class NonGenericFlagEnums
    {
        public static bool IsFlagEnum(Type enumType) => FlagEnums.IsFlagEnum(enumType);

        public static object GetAllFlags(Type enumType) => FlagEnums.GetAllFlags(enumType);

        public static bool IsValidFlagCombination(Type enumType, object? value) => FlagEnums.IsValidFlagCombination(enumType, value);

        public static string? FormatFlags(Type enumType, object? value) => FlagEnums.FormatFlags(enumType, value);

        public static string? FormatFlags(Type enumType, object? value, EnumFormat format) => FlagEnums.FormatFlags(enumType, value, format);

        public static string? FormatFlags(Type enumType, object? value, EnumFormat format0, EnumFormat format1) => FlagEnums.FormatFlags(enumType, value, format0, format1);

        public static string? FormatFlags(Type enumType, object? value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.FormatFlags(enumType, value, format0, format1, format2);

        public static string? FormatFlags(Type enumType, object? value, params EnumFormat[]? formats) => FlagEnums.FormatFlags(enumType, value, formats);

        public static string? FormatFlags(Type enumType, object? value, string? delimiter) => FlagEnums.FormatFlags(enumType, value, delimiter);

        public static string? FormatFlags(Type enumType, object? value, string? delimiter, EnumFormat format) => FlagEnums.FormatFlags(enumType, value, delimiter, format);

        public static string? FormatFlags(Type enumType, object? value, string? delimiter, EnumFormat format0, EnumFormat format1) => FlagEnums.FormatFlags(enumType, value, delimiter, format0, format1);

        public static string? FormatFlags(Type enumType, object? value, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.FormatFlags(enumType, value, delimiter, format0, format1, format2);

        public static string? FormatFlags(Type enumType, object? value, string? delimiter, params EnumFormat[]? formats) => FlagEnums.FormatFlags(enumType, value, delimiter, formats);

        public static IEnumerable<object> GetFlags(Type enumType, object? value) => FlagEnums.GetFlags(enumType, value);

        public static IEnumerable<EnumMember> GetFlagMembers(Type enumType, object? value) => FlagEnums.GetFlagMembers(enumType, value);

        public static int GetFlagCount(Type enumType) => FlagEnums.GetFlagCount(enumType);

        public static int GetFlagCount(Type enumType, object? value) => FlagEnums.GetFlagCount(enumType, value);

        public static int GetFlagCount(Type enumType, object? value, object? otherFlags) => FlagEnums.GetFlagCount(enumType, value, otherFlags);

        public static bool HasAnyFlags(Type enumType, object? value) => FlagEnums.HasAnyFlags(enumType, value);

        public static bool HasAnyFlags(Type enumType, object? value, object? otherFlags) => FlagEnums.HasAnyFlags(enumType, value, otherFlags);

        public static bool HasAllFlags(Type enumType, object? value) => FlagEnums.HasAllFlags(enumType, value);

        public static bool HasAllFlags(Type enumType, object? value, object? otherFlags) => FlagEnums.HasAllFlags(enumType, value, otherFlags);

        public static object ToggleFlags(Type enumType, object? value) => FlagEnums.ToggleFlags(enumType, value);

        public static object? ToggleFlags(Type enumType, object? value, object? otherFlags) => FlagEnums.ToggleFlags(enumType, value, otherFlags);

        public static object? CommonFlags(Type enumType, object? value, object? otherFlags) => FlagEnums.CommonFlags(enumType, value, otherFlags);

        public static object CombineFlags(Type enumType, object? value, object? otherFlags) => FlagEnums.CombineFlags(enumType, value, otherFlags);

        public static object CombineFlags(Type enumType, params object?[]? flags) => FlagEnums.CombineFlags(enumType, flags);

        public static object CombineFlags(Type enumType, IEnumerable<object?>? flags) => FlagEnums.CombineFlags(enumType, flags);

        public static object? RemoveFlags(Type enumType, object? value, object? otherFlags) => FlagEnums.RemoveFlags(enumType, value, otherFlags);

        public static object? ParseFlags(Type enumType, string? value) => FlagEnums.ParseFlags(enumType, value);

        public static object? ParseFlags(Type enumType, string? value, EnumFormat format) => FlagEnums.ParseFlags(enumType, value, format);

        public static object? ParseFlags(Type enumType, string? value, EnumFormat format0, EnumFormat format1) => FlagEnums.ParseFlags(enumType, value, format0, format1);

        public static object? ParseFlags(Type enumType, string? value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.ParseFlags(enumType, value, format0, format1, format2);

        public static object? ParseFlags(Type enumType, string? value, params EnumFormat[]? formats) => FlagEnums.ParseFlags(enumType, value, formats);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase) => FlagEnums.ParseFlags(enumType, value, ignoreCase);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, EnumFormat format) => FlagEnums.ParseFlags(enumType, value, ignoreCase, format);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => FlagEnums.ParseFlags(enumType, value, ignoreCase, format0, format1);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.ParseFlags(enumType, value, ignoreCase, format0, format1, format2);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, params EnumFormat[]? formats) => FlagEnums.ParseFlags(enumType, value, ignoreCase, formats);

        public static object? ParseFlags(Type enumType, string? value, string? delimiter) => FlagEnums.ParseFlags(enumType, value, delimiter);

        public static object? ParseFlags(Type enumType, string? value, string? delimiter, EnumFormat format) => FlagEnums.ParseFlags(enumType, value, delimiter, format);

        public static object? ParseFlags(Type enumType, string? value, string? delimiter, EnumFormat format0, EnumFormat format1) => FlagEnums.ParseFlags(enumType, value, delimiter, format0, format1);

        public static object? ParseFlags(Type enumType, string? value, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.ParseFlags(enumType, value, delimiter, format0, format1, format2);

        public static object? ParseFlags(Type enumType, string? value, string? delimiter, params EnumFormat[]? formats) => FlagEnums.ParseFlags(enumType, value, delimiter, formats);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter) => FlagEnums.ParseFlags(enumType, value, ignoreCase, delimiter);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, EnumFormat format) => FlagEnums.ParseFlags(enumType, value, ignoreCase, delimiter, format);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, EnumFormat format0, EnumFormat format1) => FlagEnums.ParseFlags(enumType, value, ignoreCase, delimiter, format0, format1);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.ParseFlags(enumType, value, ignoreCase, delimiter, format0, format1, format2);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, params EnumFormat[]? formats) => FlagEnums.ParseFlags(enumType, value, ignoreCase, delimiter, formats);

        public static bool TryParseFlags(Type enumType, string? value, out object? result) => FlagEnums.TryParseFlags(enumType, value, out result);

        public static bool TryParseFlags(Type enumType, string? value, out object? result, EnumFormat format) => FlagEnums.TryParseFlags(enumType, value, out result, format);

        public static bool TryParseFlags(Type enumType, string? value, out object? result, EnumFormat format0, EnumFormat format1) => FlagEnums.TryParseFlags(enumType, value, out result, format0, format1);

        public static bool TryParseFlags(Type enumType, string? value, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.TryParseFlags(enumType, value, out result, format0, format1, format2);

        public static bool TryParseFlags(Type enumType, string? value, out object? result, params EnumFormat[]? formats) => FlagEnums.TryParseFlags(enumType, value, out result, formats);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result) => FlagEnums.TryParseFlags(enumType, value, ignoreCase, out result);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format) => FlagEnums.TryParseFlags(enumType, value, ignoreCase, out result, format);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1) => FlagEnums.TryParseFlags(enumType, value, ignoreCase, out result, format0, format1);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.TryParseFlags(enumType, value, ignoreCase, out result, format0, format1, format2);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result, params EnumFormat[]? formats) => FlagEnums.TryParseFlags(enumType, value, ignoreCase, out result, formats);

        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result) => FlagEnums.TryParseFlags(enumType, value, delimiter, out result);

        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result, EnumFormat format) => FlagEnums.TryParseFlags(enumType, value, delimiter, out result, format);

        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1) => FlagEnums.TryParseFlags(enumType, value, delimiter, out result, format0, format1);

        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.TryParseFlags(enumType, value, delimiter, out result, format0, format1, format2);

        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result, params EnumFormat[]? formats) => FlagEnums.TryParseFlags(enumType, value, delimiter, out result, formats);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result) => FlagEnums.TryParseFlags(enumType, value, ignoreCase, delimiter, out result);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result, EnumFormat format) => FlagEnums.TryParseFlags(enumType, value, ignoreCase, delimiter, out result, format);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1) => FlagEnums.TryParseFlags(enumType, value, ignoreCase, delimiter, out result, format0, format1);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.TryParseFlags(enumType, value, ignoreCase, delimiter, out result, format0, format1, format2);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result, params EnumFormat[]? formats) => FlagEnums.TryParseFlags(enumType, value, ignoreCase, delimiter, out result, formats);

        public static IEnumerable<object> GetFlags(EnumMember member) => FlagEnums.GetFlags(member);

        public static IEnumerable<EnumMember> GetFlagMembers(EnumMember member) => FlagEnums.GetFlagMembers(member);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}