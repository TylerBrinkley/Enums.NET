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
using System.Diagnostics.CodeAnalysis;
using EnumsNET.Utilities;

namespace EnumsNET.NonGeneric
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Obsolete("NonGenericFlagEnums members have moved to the FlagEnums static class")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class NonGenericFlagEnums
    {
        public static bool IsFlagEnum(Type enumType) => NonGenericEnums.GetCache(enumType).IsFlagEnum;

        public static object GetAllFlags(Type enumType) => NonGenericEnums.GetCache(enumType).GetAllFlags();

        public static bool IsValidFlagCombination(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? true : info.EnumCache.IsValidFlagCombination(value!);
        }

        [return: NotNullIfNotNull("value")]
        public static string? FormatFlags(Type enumType, object? value) => FormatFlags(enumType, value, (string?)null);

        public static string? FormatFlags(Type enumType, object? value, EnumFormat format) => FormatFlags(enumType, value, null, format);

        public static string? FormatFlags(Type enumType, object? value, EnumFormat format0, EnumFormat format1) => FormatFlags(enumType, value, null, format0, format1);

        public static string? FormatFlags(Type enumType, object? value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FormatFlags(enumType, value, null, format0, format1, format2);

        public static string? FormatFlags(Type enumType, object? value, params EnumFormat[]? formats) => FormatFlags(enumType, value, null, formats);

        [return: NotNullIfNotNull("value")]
        public static string? FormatFlags(Type enumType, object? value, string? delimiter) => FormatFlags(enumType, value, delimiter, default(ValueCollection<EnumFormat>));

        public static string? FormatFlags(Type enumType, object? value, string? delimiter, EnumFormat format) => FormatFlags(enumType, value, delimiter, ValueCollection.Create(format));

        public static string? FormatFlags(Type enumType, object? value, string? delimiter, EnumFormat format0, EnumFormat format1) => FormatFlags(enumType, value, delimiter, ValueCollection.Create(format0, format1));

        public static string? FormatFlags(Type enumType, object? value, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FormatFlags(enumType, value, delimiter, ValueCollection.Create(format0, format1, format2));

        public static string? FormatFlags(Type enumType, object? value, string? delimiter, params EnumFormat[]? formats) => FormatFlags(enumType, value, delimiter, ValueCollection.Create(formats));

        private static string? FormatFlags(Type enumType, object? value, string? delimiter, ValueCollection<EnumFormat> formats)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? null : info.EnumCache.FormatFlags(value!, delimiter, formats);
        }

        public static IEnumerable<object> GetFlags(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? ArrayHelper.Empty<object>() : info.EnumCache.GetFlags(value!);
        }

        public static IEnumerable<EnumMember> GetFlagMembers(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? ArrayHelper.Empty<EnumMember>() : info.EnumCache.GetFlagMembers(value!);
        }

        public static int GetFlagCount(Type enumType) => NonGenericEnums.GetCache(enumType).GetFlagCount(enumType);

        public static int GetFlagCount(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? 0 : info.EnumCache.GetFlagCount(value!);
        }

        public static int GetFlagCount(Type enumType, object? value, object? otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var cache = info.EnumCache;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    if (otherFlags != null)
                    {
                        cache.ToObject(otherFlags, EnumValidation.None);
                    }
                    return 0;
                }
                if (otherFlags == null)
                {
                    cache.ToObject(value, EnumValidation.None);
                    return 0;
                }
            }

            return cache.GetFlagCount(value!, otherFlags!);
        }

        public static bool HasAnyFlags(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? false : info.EnumCache.HasAnyFlags(value!);
        }

        public static bool HasAnyFlags(Type enumType, object? value, object? otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var cache = info.EnumCache;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    if (otherFlags != null)
                    {
                        cache.ToObject(otherFlags, EnumValidation.None);
                    }
                    return false;
                }
                if (otherFlags == null)
                {
                    cache.ToObject(value, EnumValidation.None);
                    return false;
                }
            }

            return cache.HasAnyFlags(value!, otherFlags!);
        }

        public static bool HasAllFlags(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? false : info.EnumCache.HasAllFlags(value!);
        }

        public static bool HasAllFlags(Type enumType, object? value, object? otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var cache = info.EnumCache;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    return otherFlags == null || !cache.HasAnyFlags(otherFlags);
                }
                if (otherFlags == null)
                {
                    cache.ToObject(value, EnumValidation.None);
                    return true;
                }
            }

            return cache.HasAllFlags(value!, otherFlags!);
        }

        public static object ToggleFlags(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? info.EnumCache.GetAllFlags() : info.EnumCache.ToggleFlags(value!);
        }

        public static object? ToggleFlags(Type enumType, object? value, object? otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var cache = info.EnumCache;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    return otherFlags == null ? null : cache.ToObject(otherFlags, EnumValidation.None);
                }
                if (otherFlags == null)
                {
                    return cache.ToObject(value, EnumValidation.None);
                }
            }

            return cache.ToggleFlags(value!, otherFlags!);
        }

        public static object? CommonFlags(Type enumType, object? value, object? otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var cache = info.EnumCache;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    if (otherFlags != null)
                    {
                        cache.ToObject(otherFlags, EnumValidation.None);
                    }
                    return null;
                }
                if (otherFlags == null)
                {
                    cache.ToObject(value, EnumValidation.None);
                    return null;
                }
            }

            return cache.CommonFlags(value!, otherFlags!);
        }

        public static object CombineFlags(Type enumType, object? value, object? otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var cache = info.EnumCache;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    return otherFlags == null ? cache.CombineFlags(null, true) : cache.ToObject(otherFlags, EnumValidation.None);
                }
                if (otherFlags == null)
                {
                    return cache.ToObject(value, EnumValidation.None);
                }
            }

            return cache.CombineFlags(value!, otherFlags!);
        }

        public static object CombineFlags(Type enumType, params object?[]? flags) => CombineFlags(enumType, (IEnumerable<object?>?)flags);

        public static object CombineFlags(Type enumType, IEnumerable<object?>? flags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            return info.EnumCache.CombineFlags(flags, info.IsNullable);
        }

        public static object? RemoveFlags(Type enumType, object? value, object? otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var cache = info.EnumCache;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    if (otherFlags != null)
                    {
                        cache.ToObject(otherFlags, EnumValidation.None);
                    }
                    return null;
                }
                if (otherFlags == null)
                {
                    return cache.ToObject(value, EnumValidation.None);
                }
            }

            return cache.RemoveFlags(value!, otherFlags!);
        }

        public static object? ParseFlags(Type enumType, string? value) => ParseFlags(enumType, value, false, (string?)null);

        public static object? ParseFlags(Type enumType, string? value, EnumFormat format) => ParseFlags(enumType, value, false, null, format);

        public static object? ParseFlags(Type enumType, string? value, EnumFormat format0, EnumFormat format1) => ParseFlags(enumType, value, false, null, format0, format1);

        public static object? ParseFlags(Type enumType, string? value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags(enumType, value, false, null, format0, format1, format2);

        public static object? ParseFlags(Type enumType, string? value, params EnumFormat[]? formats) => ParseFlags(enumType, value, false, null, formats);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase) => ParseFlags(enumType, value, ignoreCase, (string?)null);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, EnumFormat format) => ParseFlags(enumType, value, ignoreCase, null, format);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => ParseFlags(enumType, value, ignoreCase, null, format0, format1);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags(enumType, value, ignoreCase, null, format0, format1, format2);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, params EnumFormat[]? formats) => ParseFlags(enumType, value, ignoreCase, null, formats);

        public static object? ParseFlags(Type enumType, string? value, string? delimiter) => ParseFlags(enumType, value, false, delimiter);

        public static object? ParseFlags(Type enumType, string? value, string? delimiter, EnumFormat format) => ParseFlags(enumType, value, false, delimiter, format);

        public static object? ParseFlags(Type enumType, string? value, string? delimiter, EnumFormat format0, EnumFormat format1) => ParseFlags(enumType, value, false, delimiter, format0, format1);

        public static object? ParseFlags(Type enumType, string? value, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags(enumType, value, false, delimiter, format0, format1, format2);

        public static object? ParseFlags(Type enumType, string? value, string? delimiter, params EnumFormat[]? formats) => ParseFlags(enumType, value, false, delimiter, formats);

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter) => ParseFlags(enumType, value, ignoreCase, delimiter, default(ValueCollection<EnumFormat>));

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, EnumFormat format) => ParseFlags(enumType, value, ignoreCase, delimiter, ValueCollection.Create(format));

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, EnumFormat format0, EnumFormat format1) => ParseFlags(enumType, value, ignoreCase, delimiter, ValueCollection.Create(format0, format1));

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags(enumType, value, ignoreCase, delimiter, ValueCollection.Create(format0, format1, format2));

        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, params EnumFormat[]? formats) => ParseFlags(enumType, value, ignoreCase, delimiter, ValueCollection.Create(formats));

        private static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, ValueCollection<EnumFormat> formats)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (info.IsNullable && string.IsNullOrEmpty(value))
            {
                return null;
            }

            Preconditions.NotNull(value, nameof(value));

            return info.EnumCache.ParseFlags(value!, ignoreCase, delimiter, formats);
        }

        public static bool TryParseFlags(Type enumType, string? value, out object? result) => TryParseFlags(enumType, value, false, null, out result);

        public static bool TryParseFlags(Type enumType, string? value, out object? result, EnumFormat format) => TryParseFlags(enumType, value, false, null, out result, format);

        public static bool TryParseFlags(Type enumType, string? value, out object? result, EnumFormat format0, EnumFormat format1) => TryParseFlags(enumType, value, false, null, out result, format0, format1);

        public static bool TryParseFlags(Type enumType, string? value, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(enumType, value, false, null, out result, format0, format1, format2);

        public static bool TryParseFlags(Type enumType, string? value, out object? result, params EnumFormat[]? formats) => TryParseFlags(enumType, value, false, null, out result, formats);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result) => TryParseFlags(enumType, value, ignoreCase, null, out result);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format) => TryParseFlags(enumType, value, ignoreCase, null, out result, format);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1) => TryParseFlags(enumType, value, ignoreCase, null, out result, format0, format1);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(enumType, value, ignoreCase, null, out result, format0, format1, format2);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result, params EnumFormat[]? formats) => TryParseFlags(enumType, value, ignoreCase, null, out result, formats);

        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result) => TryParseFlags(enumType, value, false, delimiter, out result);

        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result, EnumFormat format) => TryParseFlags(enumType, value, false, delimiter, out result, format);

        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1) => TryParseFlags(enumType, value, false, delimiter, out result, format0, format1);

        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(enumType, value, false, delimiter, out result, format0, format1, format2);

        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result, params EnumFormat[]? formats) => TryParseFlags(enumType, value, false, delimiter, out result, formats);

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, default(ValueCollection<EnumFormat>));

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result, EnumFormat format) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, ValueCollection.Create(format));

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, ValueCollection.Create(format0, format1));

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, ValueCollection.Create(format0, format1, format2));

        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result, params EnumFormat[]? formats) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, ValueCollection.Create(formats));

        private static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result, ValueCollection<EnumFormat> formats)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (info.IsNullable && string.IsNullOrEmpty(value))
            {
                result = null;
                return true;
            }

            return info.EnumCache.TryParseFlags(value, ignoreCase, delimiter, out result, formats);
        }

        public static IEnumerable<object> GetFlags(EnumMember member) => FlagEnums.GetFlags(member);

        public static IEnumerable<EnumMember> GetFlagMembers(EnumMember member) => FlagEnums.GetFlagMembers(member);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}