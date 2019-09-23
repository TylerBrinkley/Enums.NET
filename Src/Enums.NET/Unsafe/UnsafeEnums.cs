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
    [Obsolete("UnsafeEnums members have moved to the Enums static class")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class UnsafeEnums
    {
        public static Type GetUnderlyingType<TEnum>() => Enums.GetUnderlyingTypeUnsafe<TEnum>();

#if ICONVERTIBLE
        public static TypeCode GetTypeCode<TEnum>() => Enums.GetTypeCodeUnsafe<TEnum>();
#endif

        public static int GetMemberCount<TEnum>() => Enums.GetMemberCountUnsafe<TEnum>();

        public static int GetMemberCount<TEnum>(EnumMemberSelection selection) => Enums.GetMemberCountUnsafe<TEnum>(selection);

        public static IEnumerable<EnumMember<TEnum>> GetMembers<TEnum>() => Enums.GetMembersUnsafe<TEnum>();

        public static IEnumerable<EnumMember<TEnum>> GetMembers<TEnum>(EnumMemberSelection selection) => Enums.GetMembersUnsafe<TEnum>(selection);

        public static IEnumerable<string> GetNames<TEnum>() => Enums.GetNamesUnsafe<TEnum>();

        public static IEnumerable<string> GetNames<TEnum>(EnumMemberSelection selection) => Enums.GetNamesUnsafe<TEnum>(selection);

        public static IEnumerable<TEnum> GetValues<TEnum>() => Enums.GetValuesUnsafe<TEnum>();

        public static IEnumerable<TEnum> GetValues<TEnum>(EnumMemberSelection selection) => Enums.GetValuesUnsafe<TEnum>(selection);

        public static TEnum ToObject<TEnum>(object value) => Enums.ToObjectUnsafe<TEnum>(value);

        public static TEnum ToObject<TEnum>(object value, EnumValidation validation) => Enums.ToObjectUnsafe<TEnum>(value, validation);

        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(sbyte value) => Enums.ToObjectUnsafe<TEnum>(value);

        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(sbyte value, EnumValidation validation) => Enums.ToObjectUnsafe<TEnum>(value, validation);

        public static TEnum ToObject<TEnum>(byte value) => Enums.ToObjectUnsafe<TEnum>(value);

        public static TEnum ToObject<TEnum>(byte value, EnumValidation validation) => Enums.ToObjectUnsafe<TEnum>(value, validation);

        public static TEnum ToObject<TEnum>(short value) => Enums.ToObjectUnsafe<TEnum>(value);

        public static TEnum ToObject<TEnum>(short value, EnumValidation validation) => Enums.ToObjectUnsafe<TEnum>(value, validation);

        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(ushort value) => Enums.ToObjectUnsafe<TEnum>(value);

        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(ushort value, EnumValidation validation) => Enums.ToObjectUnsafe<TEnum>(value, validation);

        public static TEnum ToObject<TEnum>(int value) => Enums.ToObjectUnsafe<TEnum>(value);

        public static TEnum ToObject<TEnum>(int value, EnumValidation validation) => Enums.ToObjectUnsafe<TEnum>(value, validation);

        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(uint value) => Enums.ToObjectUnsafe<TEnum>(value);

        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(uint value, EnumValidation validation) => Enums.ToObjectUnsafe<TEnum>(value, validation);

        public static TEnum ToObject<TEnum>(long value) => Enums.ToObjectUnsafe<TEnum>(value);

        public static TEnum ToObject<TEnum>(long value, EnumValidation validation) => Enums.ToObjectUnsafe<TEnum>(value, validation);

        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(ulong value) => Enums.ToObjectUnsafe<TEnum>(value);

        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(ulong value, EnumValidation validation) => Enums.ToObjectUnsafe<TEnum>(value, validation);

        public static bool TryToObject<TEnum>(object? value, out TEnum result) => Enums.TryToObjectUnsafe(value, out result);

        public static bool TryToObject<TEnum>(object? value, EnumValidation validation, out TEnum result) => Enums.TryToObjectUnsafe(value, validation, out result);

        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(sbyte value, out TEnum result) => Enums.TryToObjectUnsafe(value, out result);

        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(sbyte value, EnumValidation validation, out TEnum result) => Enums.TryToObjectUnsafe(value, validation, out result);

        public static bool TryToObject<TEnum>(byte value, out TEnum result) => Enums.TryToObjectUnsafe(value, out result);

        public static bool TryToObject<TEnum>(byte value, EnumValidation validation, out TEnum result) => Enums.TryToObjectUnsafe(value, validation, out result);

        public static bool TryToObject<TEnum>(short value, out TEnum result) => Enums.TryToObjectUnsafe(value, out result);

        public static bool TryToObject<TEnum>(short value, EnumValidation validation, out TEnum result) => Enums.TryToObjectUnsafe(value, validation, out result);

        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(ushort value, out TEnum result) => Enums.TryToObjectUnsafe(value, out result);

        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(ushort value, EnumValidation validation, out TEnum result) => Enums.TryToObjectUnsafe(value, validation, out result);

        public static bool TryToObject<TEnum>(int value, out TEnum result) => Enums.TryToObjectUnsafe(value, out result);

        public static bool TryToObject<TEnum>(int value, EnumValidation validation, out TEnum result) => Enums.TryToObjectUnsafe(value, validation, out result);

        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(uint value, out TEnum result) => Enums.TryToObjectUnsafe(value, out result);

        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(uint value, EnumValidation validation, out TEnum result) => Enums.TryToObjectUnsafe(value, validation, out result);

        public static bool TryToObject<TEnum>(long value, out TEnum result) => Enums.TryToObjectUnsafe(value, out result);

        public static bool TryToObject<TEnum>(long value, EnumValidation validation, out TEnum result) => Enums.TryToObjectUnsafe(value, validation, out result);

        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(ulong value, out TEnum result) => Enums.TryToObjectUnsafe(value, out result);

        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(ulong value, EnumValidation validation, out TEnum result) => Enums.TryToObjectUnsafe(value, validation, out result);

        public static bool IsValid<TEnum>(TEnum value) => Enums.IsValidUnsafe(value);

        public static bool IsValid<TEnum>(TEnum value, EnumValidation validation) => Enums.IsValidUnsafe(value, validation);

        public static bool IsDefined<TEnum>(TEnum value) => Enums.IsDefinedUnsafe(value);

        public static TEnum Validate<TEnum>(TEnum value, string paramName) => Enums.ValidateUnsafe(value, paramName);

        public static TEnum Validate<TEnum>(TEnum value, string paramName, EnumValidation validation) => Enums.ValidateUnsafe(value, paramName, validation);

        public static string AsString<TEnum>(TEnum value) => Enums.AsStringUnsafe(value);

        public static string AsString<TEnum>(TEnum value, string? format) => Enums.AsStringUnsafe(value, format);

        public static string? AsString<TEnum>(TEnum value, EnumFormat format) => Enums.AsStringUnsafe(value, format);

        public static string? AsString<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1) => Enums.AsStringUnsafe(value, format0, format1);

        public static string? AsString<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.AsStringUnsafe(value, format0, format1, format2);

        public static string? AsString<TEnum>(TEnum value, params EnumFormat[]? formats) => Enums.AsStringUnsafe(value, formats);

        public static string Format<TEnum>(TEnum value, string format) => Enums.FormatUnsafe(value, format);

        public static string? Format<TEnum>(TEnum value, params EnumFormat[] formats) => Enums.FormatUnsafe(value, formats);

        public static object GetUnderlyingValue<TEnum>(TEnum value) => Enums.GetUnderlyingValueUnsafe(value);

        [CLSCompliant(false)]
        public static sbyte ToSByte<TEnum>(TEnum value) => Enums.ToSByteUnsafe(value);

        public static byte ToByte<TEnum>(TEnum value) => Enums.ToByteUnsafe(value);

        public static short ToInt16<TEnum>(TEnum value) => Enums.ToInt16Unsafe(value);

        [CLSCompliant(false)]
        public static ushort ToUInt16<TEnum>(TEnum value) => Enums.ToUInt16Unsafe(value);

        public static int ToInt32<TEnum>(TEnum value) => Enums.ToInt32Unsafe(value);

        [CLSCompliant(false)]
        public static uint ToUInt32<TEnum>(TEnum value) => Enums.ToUInt32Unsafe(value);

        public static long ToInt64<TEnum>(TEnum value) => Enums.ToInt64Unsafe(value);

        [CLSCompliant(false)]
        public static ulong ToUInt64<TEnum>(TEnum value) => Enums.ToUInt64Unsafe(value);

        public static int GetHashCode<TEnum>(TEnum value) => Enums.GetHashCodeUnsafe(value);

        public static bool Equals<TEnum>(TEnum value, TEnum other) => Enums.EqualsUnsafe(value, other);

        public static int CompareTo<TEnum>(TEnum value, TEnum other) => Enums.CompareToUnsafe(value, other);

        public static string? GetName<TEnum>(TEnum value) => Enums.GetNameUnsafe(value);

        public static AttributeCollection? GetAttributes<TEnum>(TEnum value) => Enums.GetAttributesUnsafe(value);

        public static EnumMember<TEnum>? GetMember<TEnum>(TEnum value) => Enums.GetMemberUnsafe(value);

        public static EnumMember<TEnum>? GetMember<TEnum>(string name) => Enums.GetMemberUnsafe<TEnum>(name);

        public static EnumMember<TEnum>? GetMember<TEnum>(string name, bool ignoreCase) => Enums.GetMemberUnsafe<TEnum>(name, ignoreCase);

        public static EnumMember<TEnum>? GetMember<TEnum>(string value, EnumFormat format) => Enums.GetMemberUnsafe<TEnum>(value, format);

        public static EnumMember<TEnum>? GetMember<TEnum>(string value, EnumFormat format0, EnumFormat format1) => Enums.GetMemberUnsafe<TEnum>(value, format0, format1);

        public static EnumMember<TEnum>? GetMember<TEnum>(string value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.GetMemberUnsafe<TEnum>(value, format0, format1, format2);

        public static EnumMember<TEnum>? GetMember<TEnum>(string value, params EnumFormat[]? formats) => Enums.GetMemberUnsafe<TEnum>(value, formats);

        public static EnumMember<TEnum>? GetMember<TEnum>(string value, bool ignoreCase, EnumFormat format) => Enums.GetMemberUnsafe<TEnum>(value, ignoreCase, format);

        public static EnumMember<TEnum>? GetMember<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => Enums.GetMemberUnsafe<TEnum>(value, ignoreCase, format0, format1);

        public static EnumMember<TEnum>? GetMember<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.GetMemberUnsafe<TEnum>(value, ignoreCase, format0, format1, format2);

        public static EnumMember<TEnum>? GetMember<TEnum>(string value, bool ignoreCase, params EnumFormat[]? formats) => Enums.GetMemberUnsafe<TEnum>(value, ignoreCase, formats);

        public static TEnum Parse<TEnum>(string value) => Enums.ParseUnsafe<TEnum>(value);

        public static TEnum Parse<TEnum>(string value, EnumFormat format) => Enums.ParseUnsafe<TEnum>(value, format);

        public static TEnum Parse<TEnum>(string value, EnumFormat format0, EnumFormat format1) => Enums.ParseUnsafe<TEnum>(value, format0, format1);

        public static TEnum Parse<TEnum>(string value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.ParseUnsafe<TEnum>(value, format0, format1, format2);

        public static TEnum Parse<TEnum>(string value, params EnumFormat[]? formats) => Enums.ParseUnsafe<TEnum>(value, formats);

        public static TEnum Parse<TEnum>(string value, bool ignoreCase) => Enums.ParseUnsafe<TEnum>(value, ignoreCase);

        public static TEnum Parse<TEnum>(string value, bool ignoreCase, EnumFormat format) => Enums.ParseUnsafe<TEnum>(value, ignoreCase, format);

        public static TEnum Parse<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => Enums.ParseUnsafe<TEnum>(value, ignoreCase, format0, format1);

        public static TEnum Parse<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.ParseUnsafe<TEnum>(value, ignoreCase, format0, format1, format2);

        public static TEnum Parse<TEnum>(string value, bool ignoreCase, params EnumFormat[]? formats) => Enums.ParseUnsafe<TEnum>(value, ignoreCase, formats);

        public static bool TryParse<TEnum>(string? value, out TEnum result) => Enums.TryParseUnsafe(value, out result);

        public static bool TryParse<TEnum>(string? value, out TEnum result, EnumFormat format) => Enums.TryParseUnsafe(value, out result, format);

        public static bool TryParse<TEnum>(string? value, out TEnum result, EnumFormat format0, EnumFormat format1) => Enums.TryParseUnsafe(value, out result, format0, format1);

        public static bool TryParse<TEnum>(string? value, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.TryParseUnsafe(value, out result, format0, format1, format2);

        public static bool TryParse<TEnum>(string? value, out TEnum result, params EnumFormat[]? formats) => Enums.TryParseUnsafe(value, out result, formats);

        public static bool TryParse<TEnum>(string? value, bool ignoreCase, out TEnum result) => Enums.TryParseUnsafe(value, ignoreCase, out result);

        public static bool TryParse<TEnum>(string? value, bool ignoreCase, out TEnum result, EnumFormat format) => Enums.TryParseUnsafe(value, ignoreCase, out result, format);

        public static bool TryParse<TEnum>(string? value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1) => Enums.TryParseUnsafe(value, ignoreCase, out result, format0, format1);

        public static bool TryParse<TEnum>(string? value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums.TryParseUnsafe(value, ignoreCase, out result, format0, format1, format2);

        public static bool TryParse<TEnum>(string? value, bool ignoreCase, out TEnum result, params EnumFormat[]? formats) => Enums.TryParseUnsafe(value, ignoreCase, out result, formats);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}