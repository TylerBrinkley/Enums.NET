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
    [Obsolete("NonGenericEnums members have moved to the Enums static class")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class NonGenericEnums
    {
        public static Type GetUnderlyingType(Type enumType) => Enums.GetUnderlyingType(enumType);

#if ICONVERTIBLE
        public static TypeCode GetTypeCode(Type enumType) => Enums.GetTypeCode(enumType);
#endif

        public static int GetMemberCount(Type enumType) => Enums.GetMemberCount(enumType);

        public static int GetMemberCount(Type enumType, EnumMemberSelection selection) => Enums.GetMemberCount(enumType, selection);

        public static IEnumerable<EnumMember> GetMembers(Type enumType) => Enums.GetMembers(enumType);

        public static IEnumerable<EnumMember> GetMembers(Type enumType, EnumMemberSelection selection) => Enums.GetMembers(enumType, selection);

        public static IEnumerable<string> GetNames(Type enumType) => Enums.GetNames(enumType);

        public static IEnumerable<string> GetNames(Type enumType, EnumMemberSelection selection) => Enums.GetNames(enumType, selection);

        public static IEnumerable<object> GetValues(Type enumType) => Enums.GetValues(enumType);

        public static IEnumerable<object> GetValues(Type enumType, EnumMemberSelection selection) => Enums.GetValues(enumType, selection);

        public static object? ToObject(Type enumType, object? value) => Enums.ToObject(enumType, value);

        public static object? ToObject(Type enumType, object? value, EnumValidation validation) => Enums.ToObject(enumType, value, validation);

        [CLSCompliant(false)]
        public static object ToObject(Type enumType, sbyte value) => Enums.ToObject(enumType, value);

        [CLSCompliant(false)]
        public static object ToObject(Type enumType, sbyte value, EnumValidation validation) => Enums.ToObject(enumType, value, validation);

        public static object ToObject(Type enumType, byte value) => Enums.ToObject(enumType, value);

        public static object ToObject(Type enumType, byte value, EnumValidation validation) => Enums.ToObject(enumType, value, validation);

        public static object ToObject(Type enumType, short value) => Enums.ToObject(enumType, value);

        public static object ToObject(Type enumType, short value, EnumValidation validation) => Enums.ToObject(enumType, value, validation);

        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ushort value) => Enums.ToObject(enumType, value);

        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ushort value, EnumValidation validation) => Enums.ToObject(enumType, value, validation);

        public static object ToObject(Type enumType, int value) => Enums.ToObject(enumType, value);

        public static object ToObject(Type enumType, int value, EnumValidation validation) => Enums.ToObject(enumType, value, validation);

        [CLSCompliant(false)]
        public static object ToObject(Type enumType, uint value) => Enums.ToObject(enumType, value);

        [CLSCompliant(false)]
        public static object ToObject(Type enumType, uint value, EnumValidation validation) => Enums.ToObject(enumType, value, validation);

        public static object ToObject(Type enumType, long value) => Enums.ToObject(enumType, value);

        public static object ToObject(Type enumType, long value, EnumValidation validation) => Enums.ToObject(enumType, value, validation);

        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ulong value) => Enums.ToObject(enumType, value);

        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ulong value, EnumValidation validation) => Enums.ToObject(enumType, value, validation);

        public static bool TryToObject(Type enumType, object? value, out object? result) => Enums.TryToObject(enumType, value, out result);

        public static bool TryToObject(Type enumType, object? value, EnumValidation validation, out object? result) => Enums.TryToObject(enumType, value, validation, out result);

        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, sbyte value, out object? result) => Enums.TryToObject(enumType, value, out result);

        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, sbyte value, EnumValidation validation, out object? result) => Enums.TryToObject(enumType, value, validation, out result);

        public static bool TryToObject(Type enumType, byte value, out object? result) => Enums.TryToObject(enumType, value, out result);

        public static bool TryToObject(Type enumType, byte value, EnumValidation validation, out object? result) => Enums.TryToObject(enumType, value, validation, out result);

        public static bool TryToObject(Type enumType, short value, out object? result) => Enums.TryToObject(enumType, value, out result);

        public static bool TryToObject(Type enumType, short value, EnumValidation validation, out object? result) => Enums.TryToObject(enumType, value, validation, out result);

        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ushort value, out object? result) => Enums.TryToObject(enumType, value, out result);

        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ushort value, EnumValidation validation, out object? result) => Enums.TryToObject(enumType, value, validation, out result);

        public static bool TryToObject(Type enumType, int value, out object? result) => Enums.TryToObject(enumType, value, out result);

        public static bool TryToObject(Type enumType, int value, EnumValidation validation, out object? result) => Enums.TryToObject(enumType, value, validation, out result);

        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, uint value, out object? result) => Enums.TryToObject(enumType, value, out result);

        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, uint value, EnumValidation validation, out object? result) => Enums.TryToObject(enumType, value, validation, out result);

        public static bool TryToObject(Type enumType, long value, out object? result) => Enums.TryToObject(enumType, value, out result);

        public static bool TryToObject(Type enumType, long value, EnumValidation validation, out object? result) => Enums.TryToObject(enumType, value, validation, out result);

        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ulong value, out object? result) => Enums.TryToObject(enumType, value, out result);

        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ulong value, EnumValidation validation, out object? result) => Enums.TryToObject(enumType, value, validation, out result);

        public static bool IsValid(Type enumType, object? value) => Enums.IsValid(enumType, value);

        public static bool IsValid(Type enumType, object? value, EnumValidation validation) => Enums.IsValid(enumType, value, validation);

        public static bool IsDefined(Type enumType, object? value) => Enums.IsDefined(enumType, value);

        public static object? Validate(Type enumType, object? value, string paramName) => Enums.Validate(enumType, value, paramName);

        public static object? Validate(Type enumType, object? value, string paramName, EnumValidation validation) => Enums.Validate(enumType, value, paramName, validation);

        public static string? AsString(Type enumType, object? value) => Enums.AsString(enumType, value);

        public static string? AsString(Type enumType, object? value, string? format) => Enums.AsString(enumType, value, format);

        public static string? AsString(Type enumType, object? value, EnumFormat format) => Enums.AsString(enumType, value, format);

        public static string? AsString(Type enumType, object? value, EnumFormat format0, EnumFormat format1) => Enums.AsString(enumType, value, format0, format1);

        public static string? AsString(Type enumType, object? value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.AsString(enumType, value, format0, format1, format2);

        public static string? AsString(Type enumType, object? value, params EnumFormat[]? formats) => Enums.AsString(enumType, value, formats);

        public static string? Format(Type enumType, object? value, string format) => Enums.Format(enumType, value, format);

        public static string? Format(Type enumType, object? value, params EnumFormat[] formats) => Enums.Format(enumType, value, formats);

        public static object? GetUnderlyingValue(Type enumType, object? value) => Enums.GetUnderlyingValue(enumType, value);

        [CLSCompliant(false)]
        public static sbyte ToSByte(Type enumType, object value) => Enums.ToSByte(enumType, value);

        public static byte ToByte(Type enumType, object value) => Enums.ToByte(enumType, value);

        public static short ToInt16(Type enumType, object value) => Enums.ToInt16(enumType, value);

        [CLSCompliant(false)]
        public static ushort ToUInt16(Type enumType, object value) => Enums.ToUInt16(enumType, value);

        public static int ToInt32(Type enumType, object value) => Enums.ToInt32(enumType, value);

        [CLSCompliant(false)]
        public static uint ToUInt32(Type enumType, object value) => Enums.ToUInt32(enumType, value);

        public static long ToInt64(Type enumType, object value) => Enums.ToInt64(enumType, value);

        [CLSCompliant(false)]
        public static ulong ToUInt64(Type enumType, object value) => Enums.ToUInt64(enumType, value);

        public static bool Equals(Type enumType, object? value, object? other) => Enums.Equals(enumType, value, other);

        public static int CompareTo(Type enumType, object? value, object? other) => Enums.CompareTo(enumType, value, other);

        public static string? GetName(Type enumType, object? value) => Enums.GetName(enumType, value);

        public static AttributeCollection? GetAttributes(Type enumType, object? value) => Enums.GetAttributes(enumType, value);

        public static EnumMember? GetMember(Type enumType, object? value) => Enums.GetMember(enumType, value);

        public static EnumMember? GetMember(Type enumType, string name) => Enums.GetMember(enumType, name);

        public static EnumMember? GetMember(Type enumType, string name, bool ignoreCase) => Enums.GetMember(enumType, name, ignoreCase);

        public static EnumMember? GetMember(Type enumType, string value, EnumFormat format) => Enums.GetMember(enumType, value, format);

        public static EnumMember? GetMember(Type enumType, string value, EnumFormat format0, EnumFormat format1) => Enums.GetMember(enumType, value, format0, format1);

        public static EnumMember? GetMember(Type enumType, string value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.GetMember(enumType, value, format0, format1, format2);

        public static EnumMember? GetMember(Type enumType, string value, params EnumFormat[]? formats) => Enums.GetMember(enumType, value, formats);

        public static EnumMember? GetMember(Type enumType, string value, bool ignoreCase, EnumFormat format) => Enums.GetMember(enumType, value, ignoreCase, format);

        public static EnumMember? GetMember(Type enumType, string value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => Enums.GetMember(enumType, value, ignoreCase, format0, format1);

        public static EnumMember? GetMember(Type enumType, string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.GetMember(enumType, value, ignoreCase, format0, format1, format2);

        public static EnumMember? GetMember(Type enumType, string value, bool ignoreCase, params EnumFormat[]? formats) => Enums.GetMember(enumType, value, ignoreCase, formats);

        public static object? Parse(Type enumType, string? value) => Enums.Parse(enumType, value);

        public static object? Parse(Type enumType, string? value, EnumFormat format) => Enums.Parse(enumType, value, format);

        public static object? Parse(Type enumType, string? value, EnumFormat format0, EnumFormat format1) => Enums.Parse(enumType, value, format0, format1);

        public static object? Parse(Type enumType, string? value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.Parse(enumType, value, format0, format1, format2);

        public static object? Parse(Type enumType, string? value, params EnumFormat[]? formats) => Enums.Parse(enumType, value, formats);

        public static object? Parse(Type enumType, string? value, bool ignoreCase) => Enums.Parse(enumType, value, ignoreCase);

        public static object? Parse(Type enumType, string? value, bool ignoreCase, EnumFormat format) => Enums.Parse(enumType, value, ignoreCase, format);

        public static object? Parse(Type enumType, string? value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => Enums.Parse(enumType, value, ignoreCase, format0, format1);

        public static object? Parse(Type enumType, string? value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.Parse(enumType, value, ignoreCase, format0, format1, format2);

        public static object? Parse(Type enumType, string? value, bool ignoreCase, params EnumFormat[]? formats) => Enums.Parse(enumType, value, ignoreCase, formats);

        public static bool TryParse(Type enumType, string? value, out object? result) => Enums.TryParse(enumType, value, out result);

        public static bool TryParse(Type enumType, string? value, out object? result, EnumFormat format) => Enums.TryParse(enumType, value, out result, format);

        public static bool TryParse(Type enumType, string? value, out object? result, EnumFormat format0, EnumFormat format1) => Enums.TryParse(enumType, value, out result, format0, format1);

        public static bool TryParse(Type enumType, string? value, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.TryParse(enumType, value, out result, format0, format1, format2);

        public static bool TryParse(Type enumType, string? value, out object? result, params EnumFormat[]? formats) => Enums.TryParse(enumType, value, out result, formats);

        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result) => Enums.TryParse(enumType, value, ignoreCase, out result);

        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format) => Enums.TryParse(enumType, value, ignoreCase, out result, format);

        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1) => Enums.TryParse(enumType, value, ignoreCase, out result, format0, format1);

        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.TryParse(enumType, value, ignoreCase, out result, format0, format1, format2);

        public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result, params EnumFormat[]? formats) => Enums.TryParse(enumType, value, ignoreCase, out result, formats);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}