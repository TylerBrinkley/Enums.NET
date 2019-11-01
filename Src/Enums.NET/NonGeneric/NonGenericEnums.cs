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
using System.Runtime.CompilerServices;

namespace EnumsNET.NonGeneric
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Obsolete("NonGenericEnums members have moved to the Enums static class")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class NonGenericEnums
    {
        private static readonly ConcurrentTypeDictionary<NonGenericEnumInfo> s_nonGenericEnumInfos = new ConcurrentTypeDictionary<NonGenericEnumInfo>();

        private static readonly Func<Type, NonGenericEnumInfo> s_nonGenericEnumInfoFactory = enumType =>
        {
            if (enumType.IsEnum())
            {
                return new NonGenericEnumInfo(Enums.GetCache(enumType), false);
            }
            else
            {
                var nonNullableEnumType = Nullable.GetUnderlyingType(enumType);
                if (nonNullableEnumType?.IsEnum() != true)
                {
                    throw new ArgumentException("must be an enum type", nameof(enumType));
                }
                return new NonGenericEnumInfo(Enums.GetCache(nonNullableEnumType), true);
            }
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static NonGenericEnumInfo GetNonGenericEnumInfo(Type enumType)
        {
            Preconditions.NotNull(enumType, nameof(enumType));

            return s_nonGenericEnumInfos.GetOrAdd(enumType, s_nonGenericEnumInfoFactory);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static EnumCache GetCache(Type enumType) => GetNonGenericEnumInfo(enumType).EnumCache;

        public static Type GetUnderlyingType(Type enumType) => GetCache(enumType).UnderlyingType;

#if ICONVERTIBLE
        public static TypeCode GetTypeCode(Type enumType) => GetCache(enumType).TypeCode;
#endif

        public static int GetMemberCount(Type enumType, EnumMemberSelection selection = EnumMemberSelection.All) => GetCache(enumType).GetMemberCount(selection);

        public static IEnumerable<EnumMember> GetMembers(Type enumType, EnumMemberSelection selection = EnumMemberSelection.All) => GetCache(enumType).GetMembers(selection);

        public static IEnumerable<string> GetNames(Type enumType, EnumMemberSelection selection = EnumMemberSelection.All) => GetCache(enumType).GetNames(selection);

        public static IEnumerable<object> GetValues(Type enumType, EnumMemberSelection selection = EnumMemberSelection.All) => GetCache(enumType).GetValues(selection).GetNonGenericContainer();

        [return: NotNullIfNotNull("value")]
        public static object? ToObject(Type enumType, object? value, EnumValidation validation = EnumValidation.None)
        {
            var info = GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? null : info.EnumCache.ToObject(value!, validation);
        }

        [CLSCompliant(false)]
        public static object ToObject(Type enumType, sbyte value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        public static object ToObject(Type enumType, byte value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        public static object ToObject(Type enumType, short value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ushort value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        public static object ToObject(Type enumType, int value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        [CLSCompliant(false)]
        public static object ToObject(Type enumType, uint value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        public static object ToObject(Type enumType, long value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ulong value, EnumValidation validation = EnumValidation.None) => GetCache(enumType).ToObject(value, validation);

        public static bool TryToObject(Type enumType, object? value, out object? result, EnumValidation validation = EnumValidation.None)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                result = null;
                return true;
            }

            return info.EnumCache.TryToObject(value, out result, validation);
        }

        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, sbyte value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        public static bool TryToObject(Type enumType, byte value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        public static bool TryToObject(Type enumType, short value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ushort value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        public static bool TryToObject(Type enumType, int value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, uint value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        public static bool TryToObject(Type enumType, long value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ulong value, out object? result, EnumValidation validation = EnumValidation.None) => GetCache(enumType).TryToObject(value, out result, validation);

        public static bool IsValid(Type enumType, object? value, EnumValidation validation = EnumValidation.Default)
        {
            var info = GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? true : info.EnumCache.IsValid(value!, validation);
        }

        public static bool IsDefined(Type enumType, object? value)
        {
            var info = GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? false : info.EnumCache.IsDefined(value!);
        }

        [return: NotNullIfNotNull("value")]
        public static object? Validate(Type enumType, object? value, string paramName, EnumValidation validation = EnumValidation.Default)
        {
            var info = GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? null : info.EnumCache.Validate(value!, paramName, validation);
        }

        [return: NotNullIfNotNull("value")]
        public static string? AsString(Type enumType, object? value)
        {
            var info = GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? null : info.EnumCache.AsString(value!);
        }

        [return: NotNullIfNotNull("value")]
        public static string? AsString(Type enumType, object? value, string? format)
        {
            var info = GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? null : info.EnumCache.AsString(value!, string.IsNullOrEmpty(format) ? "G" : format!);
        }

        public static string? AsString(Type enumType, object? value, EnumFormat format)
        {
            var info = GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? null : info.EnumCache.AsString(value!, format);
        }

        public static string? AsString(Type enumType, object? value, EnumFormat format0, EnumFormat format1) => AsString(enumType, value, ValueCollection.Create(format0, format1));

        public static string? AsString(Type enumType, object? value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => AsString(enumType, value, ValueCollection.Create(format0, format1, format2));

        public static string? AsString(Type enumType, object? value, params EnumFormat[]? formats) => AsString(enumType, value, formats?.Length > 0 ? ValueCollection.Create(formats) : Enums.DefaultFormats);

        private static string? AsString(Type enumType, object? value, ValueCollection<EnumFormat> formats)
        {
            var info = GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? null : info.EnumCache.AsString(value!, formats);
        }

        public static string? Format(Type enumType, object? value, string format)
        {
            Preconditions.NotNull(format, nameof(format));
            var info = GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? null : info.EnumCache.AsString(value!, format);
        }

        public static string? Format(Type enumType, object? value, params EnumFormat[] formats)
        {
            Preconditions.NotNull(formats, nameof(formats));
            return AsString(enumType, value, formats);
        }

        public static object? GetUnderlyingValue(Type enumType, object? value)
        {
            var info = GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? null : info.EnumCache.GetUnderlyingValue(value!);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(Type enumType, object value) => GetCache(enumType).ToSByte(value);

        public static byte ToByte(Type enumType, object value) => GetCache(enumType).ToByte(value);

        public static short ToInt16(Type enumType, object value) => GetCache(enumType).ToInt16(value);

        [CLSCompliant(false)]
        public static ushort ToUInt16(Type enumType, object value) => GetCache(enumType).ToUInt16(value);

        public static int ToInt32(Type enumType, object value) => GetCache(enumType).ToInt32(value);

        [CLSCompliant(false)]
        public static uint ToUInt32(Type enumType, object value) => GetCache(enumType).ToUInt32(value);

        public static long ToInt64(Type enumType, object value) => GetCache(enumType).ToInt64(value);

        [CLSCompliant(false)]
        public static ulong ToUInt64(Type enumType, object value) => GetCache(enumType).ToUInt64(value);

        public static bool Equals(Type enumType, object? value, object? other)
        {
            var info = GetNonGenericEnumInfo(enumType);
            var cache = info.EnumCache;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    if (other == null)
                    {
                        return true;
                    }
                    cache.ToObject(other, EnumValidation.None);
                    return false;
                }
                if (other == null)
                {
                    cache.ToObject(value, EnumValidation.None);
                    return false;
                }
            }

            return cache.Equals(value!, other!);
        }

        public static int CompareTo(Type enumType, object? value, object? other)
        {
            var info = GetNonGenericEnumInfo(enumType);
            var cache = info.EnumCache;
            if (info.IsNullable)
            {
                if (value == null)
                {
                    if (other == null)
                    {
                        return 0;
                    }
                    cache.ToObject(other, EnumValidation.None);
                    return -1;
                }
                if (other == null)
                {
                    cache.ToObject(value, EnumValidation.None);
                    return 1;
                }
            }

            return cache.CompareTo(value!, other!);
        }

        public static string? GetName(Type enumType, object? value)
        {
            var info = GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? null : info.EnumCache.GetMember(value!)?.Name;
        }

        public static AttributeCollection? GetAttributes(Type enumType, object? value)
        {
            var info = GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? null : info.EnumCache.GetMember(value!)?.Attributes;
        }

        public static EnumMember? GetMember(Type enumType, object? value)
        {
            var info = GetNonGenericEnumInfo(enumType);
            return value == null && info.IsNullable ? null : info.EnumCache.GetMember(value!)?.EnumMember;
        }

        public static EnumMember? GetMember(Type enumType, string name) => GetMember(enumType, name, false);

        public static EnumMember? GetMember(Type enumType, string name, bool ignoreCase) => GetCache(enumType).GetMember(name, ignoreCase, Enums.NameFormat);

        public static EnumMember? GetMember(Type enumType, string value, EnumFormat format) => GetMember(enumType, value, false, format);

        public static EnumMember? GetMember(Type enumType, string value, EnumFormat format0, EnumFormat format1) => GetMember(enumType, value, false, format0, format1);

        public static EnumMember? GetMember(Type enumType, string value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetMember(enumType, value, false, format0, format1, format2);

        public static EnumMember? GetMember(Type enumType, string value, params EnumFormat[]? formats) => GetMember(enumType, value, false, formats);

        public static EnumMember? GetMember(Type enumType, string value, bool ignoreCase, EnumFormat format) => GetCache(enumType).GetMember(value, ignoreCase, ValueCollection.Create(format));

        public static EnumMember? GetMember(Type enumType, string value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => GetCache(enumType).GetMember(value, ignoreCase, ValueCollection.Create(format0, format1));

        public static EnumMember? GetMember(Type enumType, string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetCache(enumType).GetMember(value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        public static EnumMember? GetMember(Type enumType, string value, bool ignoreCase, params EnumFormat[]? formats) => GetCache(enumType).GetMember(value, ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : Enums.NameFormat);

        public static object? Parse(Type enumType, string? value) => Parse(enumType, value, false, Enums.DefaultFormats);

        public static object? Parse(Type enumType, string? value, EnumFormat format) => Parse(enumType, value, false, ValueCollection.Create(format));

        public static object? Parse(Type enumType, string? value, EnumFormat format0, EnumFormat format1) => Parse(enumType, value, false, ValueCollection.Create(format0, format1));

        public static object? Parse(Type enumType, string? value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Parse(enumType, value, false, ValueCollection.Create(format0, format1, format2));

        public static object? Parse(Type enumType, string? value, params EnumFormat[]? formats) => Parse(enumType, value, false, formats?.Length > 0 ? ValueCollection.Create(formats) : Enums.DefaultFormats);

        public static object? Parse(Type enumType, string? value, bool ignoreCase) => Parse(enumType, value, ignoreCase, Enums.DefaultFormats);

        public static object? Parse(Type enumType, string? value, bool ignoreCase, EnumFormat format) => Parse(enumType, value, ignoreCase, ValueCollection.Create(format));

        public static object? Parse(Type enumType, string? value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => Parse(enumType, value, ignoreCase, ValueCollection.Create(format0, format1));

        public static object? Parse(Type enumType, string? value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Parse(enumType, value, ignoreCase, ValueCollection.Create(format0, format1, format2));

        public static object? Parse(Type enumType, string? value, bool ignoreCase, params EnumFormat[]? formats) => Parse(enumType, value, ignoreCase, formats?.Length > 0 ? ValueCollection.Create(formats) : Enums.DefaultFormats);

        private static object? Parse(Type enumType, string? value, bool ignoreCase, ValueCollection<EnumFormat> formats)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (info.IsNullable && string.IsNullOrEmpty(value))
            {
                return null;
            }

            Preconditions.NotNull(value, nameof(value));

            return info.EnumCache.Parse(value!, ignoreCase, formats);
        }

        public static bool TryParse(Type enumType, string? value, out object? result) => TryParse(enumType, value, false, out result, Enums.DefaultFormats);

        public static bool TryParse(Type enumType, string? value, out object? result, EnumFormat format) => TryParse(enumType, value, false, out result, ValueCollection.Create(format));

        public static bool TryParse(Type enumType, string? value, out object? result, EnumFormat format0, EnumFormat format1) => TryParse(enumType, value, false, out result, ValueCollection.Create(format0, format1));

        public static bool TryParse(Type enumType, string? value, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParse(enumType, value, false, out result, ValueCollection.Create(format0, format1, format2));

        public static bool TryParse(Type enumType, string? value, out object? result, params EnumFormat[]? formats) => TryParse(enumType, value, false, out result, formats?.Length > 0 ? ValueCollection.Create(formats) : Enums.DefaultFormats);

        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result) => TryParse(enumType, value, ignoreCase, out result, Enums.DefaultFormats);

        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format) => TryParse(enumType, value, ignoreCase, out result, ValueCollection.Create(format));

        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1) => TryParse(enumType, value, ignoreCase, out result, ValueCollection.Create(format0, format1));

        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParse(enumType, value, ignoreCase, out result, ValueCollection.Create(format0, format1, format2));

        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result, params EnumFormat[]? formats) => TryParse(enumType, value, ignoreCase, out result, formats?.Length > 0 ? ValueCollection.Create(formats) : Enums.DefaultFormats);

        private static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result, ValueCollection<EnumFormat> formats)
        {
            var info = GetNonGenericEnumInfo(enumType);

            if (info.IsNullable && string.IsNullOrEmpty(value))
            {
                result = null;
                return true;
            }

            return info.EnumCache.TryParse(value, ignoreCase, out result, formats);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}