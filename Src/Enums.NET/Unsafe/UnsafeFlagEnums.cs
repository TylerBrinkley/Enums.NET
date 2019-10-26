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

namespace EnumsNET.Unsafe
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Obsolete("NonGenericFlagEnums members have moved to the FlagEnums static class")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class UnsafeFlagEnums
    {
        public static bool IsFlagEnum<TEnum>() => FlagEnums.IsFlagEnumUnsafe<TEnum>();

        public static TEnum GetAllFlags<TEnum>() => FlagEnums.GetAllFlagsUnsafe<TEnum>();

        public static bool IsValidFlagCombination<TEnum>(TEnum value) => FlagEnums.IsValidFlagCombinationUnsafe(value);

        public static string FormatFlags<TEnum>(TEnum value) => FlagEnums.FormatFlagsUnsafe(value);

        public static string? FormatFlags<TEnum>(TEnum value, EnumFormat format) => FlagEnums.FormatFlagsUnsafe(value, format);

        public static string? FormatFlags<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1) => FlagEnums.FormatFlagsUnsafe(value, format0, format1);

        public static string? FormatFlags<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.FormatFlagsUnsafe(value, format0, format1, format2);

        public static string? FormatFlags<TEnum>(TEnum value, params EnumFormat[]? formats) => FlagEnums.FormatFlagsUnsafe(value, formats);

        public static string FormatFlags<TEnum>(TEnum value, string? delimiter) => FlagEnums.FormatFlagsUnsafe(value, delimiter);

        public static string? FormatFlags<TEnum>(TEnum value, string? delimiter, EnumFormat format) => FlagEnums.FormatFlagsUnsafe(value, delimiter, format);

        public static string? FormatFlags<TEnum>(TEnum value, string? delimiter, EnumFormat format0, EnumFormat format1) => FlagEnums.FormatFlagsUnsafe(value, delimiter, format0, format1);

        public static string? FormatFlags<TEnum>(TEnum value, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.FormatFlagsUnsafe(value, delimiter, format0, format1, format2);

        public static string? FormatFlags<TEnum>(TEnum value, string? delimiter, params EnumFormat[]? formats) => FlagEnums.FormatFlagsUnsafe(value, delimiter, formats);

        public static IEnumerable<TEnum> GetFlags<TEnum>(TEnum value) => FlagEnums.GetFlagsUnsafe(value);

        public static IEnumerable<EnumMember<TEnum>> GetFlagMembers<TEnum>(TEnum value) => FlagEnums.GetFlagMembersUnsafe(value);

        public static int GetFlagCount<TEnum>() => FlagEnums.GetFlagCountUnsafe<TEnum>();

        public static int GetFlagCount<TEnum>(TEnum value) => FlagEnums.GetFlagCountUnsafe(value);

        public static int GetFlagCount<TEnum>(TEnum value, TEnum otherFlags) => FlagEnums.GetFlagCountUnsafe(value, otherFlags);

        public static bool HasAnyFlags<TEnum>(TEnum value) => FlagEnums.HasAnyFlagsUnsafe(value);

        public static bool HasAnyFlags<TEnum>(TEnum value, TEnum otherFlags) => FlagEnums.HasAnyFlagsUnsafe(value, otherFlags);

        public static bool HasAllFlags<TEnum>(TEnum value) => FlagEnums.HasAllFlagsUnsafe(value);

        public static bool HasAllFlags<TEnum>(TEnum value, TEnum otherFlags) => FlagEnums.HasAllFlagsUnsafe(value, otherFlags);

        public static TEnum ToggleFlags<TEnum>(TEnum value) => FlagEnums.ToggleFlagsUnsafe(value);

        public static TEnum ToggleFlags<TEnum>(TEnum value, TEnum otherFlags) => FlagEnums.ToggleFlagsUnsafe(value, otherFlags);

        public static TEnum CommonFlags<TEnum>(TEnum value, TEnum otherFlags) => FlagEnums.CommonFlagsUnsafe(value, otherFlags);

        public static TEnum CombineFlags<TEnum>(TEnum value, TEnum otherFlags) => FlagEnums.CombineFlagsUnsafe(value, otherFlags);

        public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2) => FlagEnums.CombineFlagsUnsafe(flag0, flag1, flag2);

        public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3) => FlagEnums.CombineFlagsUnsafe(flag0, flag1, flag2, flag3);

        public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4) => FlagEnums.CombineFlagsUnsafe(flag0, flag1, flag2, flag3, flag4);

        public static TEnum CombineFlags<TEnum>(params TEnum[]? flags) => FlagEnums.CombineFlagsUnsafe(flags);

        public static TEnum CombineFlags<TEnum>(IEnumerable<TEnum>? flags) => FlagEnums.CombineFlagsUnsafe(flags);

        public static TEnum RemoveFlags<TEnum>(TEnum value, TEnum otherFlags) => FlagEnums.RemoveFlagsUnsafe(value, otherFlags);

        public static TEnum ParseFlags<TEnum>(string value) => FlagEnums.ParseFlagsUnsafe<TEnum>(value);

        public static TEnum ParseFlags<TEnum>(string value, EnumFormat format) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, format);

        public static TEnum ParseFlags<TEnum>(string value, EnumFormat format0, EnumFormat format1) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, format0, format1);

        public static TEnum ParseFlags<TEnum>(string value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, format0, format1, format2);

        public static TEnum ParseFlags<TEnum>(string value, params EnumFormat[]? formats) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, formats);

        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, ignoreCase);

        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, EnumFormat format) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, ignoreCase, format);

        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, ignoreCase, format0, format1);

        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, ignoreCase, format0, format1, format2);

        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, params EnumFormat[]? formats) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, ignoreCase, formats);

        public static TEnum ParseFlags<TEnum>(string value, string? delimiter) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, delimiter);

        public static TEnum ParseFlags<TEnum>(string value, string? delimiter, EnumFormat format) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, delimiter, format);

        public static TEnum ParseFlags<TEnum>(string value, string? delimiter, EnumFormat format0, EnumFormat format1) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, delimiter, format0, format1);

        public static TEnum ParseFlags<TEnum>(string value, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, delimiter, format0, format1, format2);

        public static TEnum ParseFlags<TEnum>(string value, string? delimiter, params EnumFormat[]? formats) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, delimiter, formats);

        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, string? delimiter) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, ignoreCase, delimiter);

        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, string? delimiter, EnumFormat format) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, ignoreCase, delimiter, format);

        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, string? delimiter, EnumFormat format0, EnumFormat format1) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, ignoreCase, delimiter, format0, format1);

        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, ignoreCase, delimiter, format0, format1, format2);

        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, string? delimiter, params EnumFormat[]? formats) => FlagEnums.ParseFlagsUnsafe<TEnum>(value, ignoreCase, delimiter, formats);

        public static bool TryParseFlags<TEnum>(string? value, out TEnum result) => FlagEnums.TryParseFlagsUnsafe(value, out result);

        public static bool TryParseFlags<TEnum>(string? value, out TEnum result, EnumFormat format) => FlagEnums.TryParseFlagsUnsafe(value, out result, format);

        public static bool TryParseFlags<TEnum>(string? value, out TEnum result, EnumFormat format0, EnumFormat format1) => FlagEnums.TryParseFlagsUnsafe(value, out result, format0, format1);

        public static bool TryParseFlags<TEnum>(string? value, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.TryParseFlagsUnsafe(value, out result, format0, format1, format2);

        public static bool TryParseFlags<TEnum>(string? value, out TEnum result, params EnumFormat[]? formats) => FlagEnums.TryParseFlagsUnsafe(value, out result, formats);

        public static bool TryParseFlags<TEnum>(string? value, bool ignoreCase, out TEnum result) => FlagEnums.TryParseFlagsUnsafe(value, ignoreCase, out result);

        public static bool TryParseFlags<TEnum>(string? value, bool ignoreCase, out TEnum result, EnumFormat format) => FlagEnums.TryParseFlagsUnsafe(value, ignoreCase, out result, format);

        public static bool TryParseFlags<TEnum>(string? value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1) => FlagEnums.TryParseFlagsUnsafe(value, ignoreCase, out result, format0, format1);

        public static bool TryParseFlags<TEnum>(string? value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.TryParseFlagsUnsafe(value, ignoreCase, out result, format0, format1, format2);

        public static bool TryParseFlags<TEnum>(string? value, bool ignoreCase, out TEnum result, params EnumFormat[]? formats) => FlagEnums.TryParseFlagsUnsafe(value, ignoreCase, out result, formats);

        public static bool TryParseFlags<TEnum>(string? value, string? delimiter, out TEnum result) => FlagEnums.TryParseFlagsUnsafe(value, delimiter, out result);

        public static bool TryParseFlags<TEnum>(string? value, string? delimiter, out TEnum result, EnumFormat format) => FlagEnums.TryParseFlagsUnsafe(value, delimiter, out result, format);

        public static bool TryParseFlags<TEnum>(string? value, string? delimiter, out TEnum result, EnumFormat format0, EnumFormat format1) => FlagEnums.TryParseFlagsUnsafe(value, delimiter, out result, format0, format1);

        public static bool TryParseFlags<TEnum>(string? value, string? delimiter, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.TryParseFlagsUnsafe(value, delimiter, out result, format0, format1, format2);

        public static bool TryParseFlags<TEnum>(string? value, string? delimiter, out TEnum result, params EnumFormat[]? formats) => FlagEnums.TryParseFlagsUnsafe(value, delimiter, out result, formats);

        public static bool TryParseFlags<TEnum>(string? value, bool ignoreCase, string? delimiter, out TEnum result) => FlagEnums.TryParseFlagsUnsafe(value, ignoreCase, delimiter, out result);

        public static bool TryParseFlags<TEnum>(string? value, bool ignoreCase, string? delimiter, out TEnum result, EnumFormat format) => FlagEnums.TryParseFlagsUnsafe(value, ignoreCase, delimiter, out result, format);

        public static bool TryParseFlags<TEnum>(string? value, bool ignoreCase, string? delimiter, out TEnum result, EnumFormat format0, EnumFormat format1) => FlagEnums.TryParseFlagsUnsafe(value, ignoreCase, delimiter, out result, format0, format1);

        public static bool TryParseFlags<TEnum>(string? value, bool ignoreCase, string? delimiter, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FlagEnums.TryParseFlagsUnsafe(value, ignoreCase, delimiter, out result, format0, format1, format2);

        public static bool TryParseFlags<TEnum>(string? value, bool ignoreCase, string? delimiter, out TEnum result, params EnumFormat[]? formats) => FlagEnums.TryParseFlagsUnsafe(value, ignoreCase, delimiter, out result, formats);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}