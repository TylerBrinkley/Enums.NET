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
using System.Diagnostics.CodeAnalysis;

namespace EnumsNET.NonGeneric
{
    /// <summary>
    /// A non-generic implementation of the static class <see cref="FlagEnums"/>.
    /// When the type is known at compile-time the <see cref="FlagEnums"/> class should be used instead, to provide type safety and to avoid boxing.
    /// </summary>
    public static class NonGenericFlagEnums
    {
        #region "Properties"
        /// <summary>
        /// Indicates if <paramref name="enumType"/> is marked with the <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns>Indication if <paramref name="enumType"/> is marked with the <see cref="FlagsAttribute"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool IsFlagEnum(Type enumType) => NonGenericEnums.GetCache(enumType).IsFlagEnum;

        /// <summary>
        /// Retrieves all the flags defined by <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns>All the flags defined by <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static object GetAllFlags(Type enumType) => NonGenericEnums.GetCache(enumType).GetAllFlags();
        #endregion

        #region Main Methods
        /// <summary>
        /// Indicates whether <paramref name="value"/> is a valid flag combination of <paramref name="enumType"/>'s defined flags.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <returns>Indication of whether <paramref name="value"/> is a valid flag combination of <paramref name="enumType"/>'s defined flags.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static bool IsValidFlagCombination(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return true;
            }

            return info.EnumCache.IsValidFlagCombination(value!);
        }

        /// <summary>
        /// Retrieves the names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        [return: NotNullIfNotNull("value")]
        public static string? FormatFlags(Type enumType, object? value) => FormatFlags(enumType, value, null, null);

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with <paramref name="format"/> and delimited with commas
        /// or if empty returns the zero flag formatted with <paramref name="format"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with <paramref name="format"/> and delimited with commas
        /// or if empty returns the zero flag formatted with <paramref name="format"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static string? FormatFlags(Type enumType, object? value, EnumFormat format) => FormatFlags(enumType, value, null, format);

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with formats and delimited with commas
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with formats and delimited with commas
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static string? FormatFlags(Type enumType, object? value, EnumFormat format0, EnumFormat format1) => FormatFlags(enumType, value, null, format0, format1);

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with formats and delimited with commas
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use.</param>
        /// <param name="format2">The third output format to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with formats and delimited with commas
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static string? FormatFlags(Type enumType, object? value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FormatFlags(enumType, value, null, format0, format1, format2);

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with <paramref name="formats"/> and delimited with commas
        /// or if empty returns the zero flag formatted with <paramref name="formats"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="formats">The output formats to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with <paramref name="formats"/> and delimited with commas
        /// or if empty returns the zero flag formatted with <paramref name="formats"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static string? FormatFlags(Type enumType, object? value, params EnumFormat[]? formats) => FormatFlags(enumType, value, null, formats);

        /// <summary>
        /// Retrieves the names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="delimiter">The delimiter to use to separate individual flags.</param>
        /// <returns>The names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        [return: NotNullIfNotNull("value")]
        public static string? FormatFlags(Type enumType, object? value, string? delimiter) => FormatFlags(enumType, value, delimiter, null);

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with <paramref name="format"/> and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with <paramref name="format"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="delimiter">The delimiter to use to separate individual flags.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with <paramref name="format"/> and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with <paramref name="format"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static string? FormatFlags(Type enumType, object? value, string? delimiter, EnumFormat format) => FormatFlags(enumType, value, delimiter, new ValueCollection<EnumFormat>(format));

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with formats and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="delimiter">The delimiter to use to separate individual flags.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with formats and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static string? FormatFlags(Type enumType, object? value, string? delimiter, EnumFormat format0, EnumFormat format1) => FormatFlags(enumType, value, delimiter, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with formats and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="delimiter">The delimiter to use to separate individual flags.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use.</param>
        /// <param name="format2">The third output format to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with formats and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static string? FormatFlags(Type enumType, object? value, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FormatFlags(enumType, value, delimiter, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with <paramref name="formats"/> and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with <paramref name="formats"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="delimiter">The delimiter to use to separate individual flags.</param>
        /// <param name="formats">The output formats to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with <paramref name="formats"/> and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with <paramref name="formats"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static string? FormatFlags(Type enumType, object? value, string? delimiter, params EnumFormat[]? formats) => FormatFlags(enumType, value, delimiter, new ValueCollection<EnumFormat>(formats));

        private static string? FormatFlags(Type enumType, object? value, string? delimiter, ValueCollection<EnumFormat> formats)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumCache.FormatFlags(value!, delimiter, formats);
        }

        /// <summary>
        /// Retrieves the flags that compose <paramref name="value"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The flags that compose <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static IEnumerable<object> GetFlags(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return new object[0];
            }

            return info.EnumCache.GetFlags(value!);
        }

        /// <summary>
        /// Retrieves the <see cref="EnumMember"/>s of the flags that compose <paramref name="value"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The <see cref="EnumMember"/>s of the flags that compose <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static IEnumerable<EnumMember> GetFlagMembers(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return new EnumMember[0];
            }

            return info.EnumCache.GetFlagMembers(value!);
        }

        /// <summary>
        /// Retrieves the flag count of <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns>The flag count of <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static int GetFlagCount(Type enumType) => NonGenericEnums.GetCache(enumType).GetFlagCount();

        /// <summary>
        /// Retrieves the flag count of <paramref name="value"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The flag count of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static int GetFlagCount(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return 0;
            }

            return info.EnumCache.GetFlagCount(value!);
        }

        /// <summary>
        /// Retrieves the flag count of <paramref name="otherFlags"/> that <paramref name="value"/> has.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>The flag count of <paramref name="otherFlags"/> that <paramref name="value"/> has.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="otherFlags"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="otherFlags"/> is of an invalid type.</exception>
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

        /// <summary>
        /// Indicates if <paramref name="value"/> has any flags.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has any flags.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static bool HasAnyFlags(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return false;
            }

            return info.EnumCache.HasAnyFlags(value!);
        }

        /// <summary>
        /// Indicates if <paramref name="value"/> has any flags that are in <paramref name="otherFlags"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has any flags that are in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="otherFlags"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="otherFlags"/> is of an invalid type.</exception>
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

        /// <summary>
        /// Indicates if <paramref name="value"/> has all of the flags that are defined in <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has all of the flags that are defined in <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static bool HasAllFlags(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return false;
            }

            return info.EnumCache.HasAllFlags(value!);
        }

        /// <summary>
        /// Indicates if <paramref name="value"/> has all of the flags that are in <paramref name="otherFlags"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has all of the flags that are in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="otherFlags"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="otherFlags"/> is of an invalid type.</exception>
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

        /// <summary>
        /// Returns <paramref name="value"/> with all of it's flags toggled. Equivalent to the bitwise "xor" operator with <see cref="GetAllFlags(Type)"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <returns><paramref name="value"/> with all of it's flags toggled.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is of an invalid type.</exception>
        public static object ToggleFlags(Type enumType, object? value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var cache = info.EnumCache;

            if (value == null && info.IsNullable)
            {
                return cache.GetAllFlags();
            }

            return cache.ToggleFlags(value!);
        }

        /// <summary>
        /// Returns <paramref name="value"/> while toggling the flags that are in <paramref name="otherFlags"/>. Equivalent to the bitwise "xor" operator.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns><paramref name="value"/> while toggling the flags that are in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="otherFlags"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="otherFlags"/> is of an invalid type.</exception>
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

        /// <summary>
        /// Returns <paramref name="value"/> with only the flags that are also in <paramref name="otherFlags"/>. Equivalent to the bitwise "and" operation.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns><paramref name="value"/> with only the flags that are also in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="otherFlags"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="otherFlags"/> is of an invalid type.</exception>
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

        /// <summary>
        /// Combines the flags of <paramref name="value"/> and <paramref name="otherFlags"/>. Equivalent to the bitwise "or" operation.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>Combination of <paramref name="value"/> with the flags in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="otherFlags"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="otherFlags"/> is of an invalid type.</exception>
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

        /// <summary>
        /// Combines all of the flags of <paramref name="flags"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="flags">The flags enum values.</param>
        /// <returns>Combination of all of the flags of <paramref name="flags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or one of the <paramref name="flags"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="flags"/> contains a value that is of an invalid type.</exception>
        public static object CombineFlags(Type enumType, params object?[]? flags) => CombineFlags(enumType, (IEnumerable<object?>?)flags);

        /// <summary>
        /// Combines all of the flags of <paramref name="flags"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="flags">The flags enum values.</param>
        /// <returns>Combination of all of the flags of <paramref name="flags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or one of the <paramref name="flags"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="flags"/> contains a value that is of an invalid type.</exception>
        public static object CombineFlags(Type enumType, IEnumerable<object?>? flags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            return info.EnumCache.CombineFlags(flags, info.IsNullable);
        }

        /// <summary>
        /// Returns <paramref name="value"/> without the flags specified in <paramref name="otherFlags"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns><paramref name="value"/> without the flags specified in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="otherFlags"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="otherFlags"/> is of an invalid type.</exception>
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
        #endregion

        #region Parsing
        /// <summary>
        /// Converts the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <returns>A <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <paramref name="enumType"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <paramref name="enumType"/>'s underlying type.</exception>
        public static object? ParseFlags(Type enumType, string? value) => ParseFlags(enumType, value, value, false, null, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, EnumFormat format) => ParseFlags(enumType, value, false, null, format);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, EnumFormat format0, EnumFormat format1) => ParseFlags(enumType, value, false, null, format0, format1);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags(enumType, value, false, null, format0, format1, format2);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>A <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, params EnumFormat[]? formats) => ParseFlags(enumType, value, false, null, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <paramref name="enumType"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase) => ParseFlags(enumType, value, value, ignoreCase, null, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, EnumFormat format) => ParseFlags(enumType, value, ignoreCase, null, format);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => ParseFlags(enumType, value, ignoreCase, null, format0, format1);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags(enumType, value, ignoreCase, null, format0, format1, format2);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, params EnumFormat[]? formats) => ParseFlags(enumType, value, ignoreCase, null, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <returns>A <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <paramref name="enumType"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <paramref name="enumType"/>'s underlying type.</exception>
        public static object? ParseFlags(Type enumType, string? value, string? delimiter) => ParseFlags(enumType, value, false, delimiter, null);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, string? delimiter, EnumFormat format) => ParseFlags(enumType, value, false, delimiter, format);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, string? delimiter, EnumFormat format0, EnumFormat format1) => ParseFlags(enumType, value, false, delimiter, format0, format1);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags(enumType, value, false, delimiter, format0, format1, format2);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>A <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, string? delimiter, params EnumFormat[]? formats) => ParseFlags(enumType, value, false, delimiter, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <paramref name="enumType"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter) => ParseFlags(enumType, value, value, ignoreCase, delimiter, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, EnumFormat format) => ParseFlags(enumType, value, value, ignoreCase, delimiter, new ValueCollection<EnumFormat>(format));

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, EnumFormat format0, EnumFormat format1) => ParseFlags(enumType, value, value, ignoreCase, delimiter, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags(enumType, value, value, ignoreCase, delimiter, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, params EnumFormat[]? formats) => ParseFlags(enumType, value, value, ignoreCase, delimiter, new ValueCollection<EnumFormat>(formats));

#if SPAN
        /// <summary>
        /// Converts the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <returns>A <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <paramref name="enumType"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <paramref name="enumType"/>'s underlying type.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value) => ParseFlags(enumType, value, string.Empty, false, null, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, EnumFormat format) => ParseFlags(enumType, value, false, null, format);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, EnumFormat format0, EnumFormat format1) => ParseFlags(enumType, value, false, null, format0, format1);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags(enumType, value, false, null, format0, format1, format2);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>A <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, params EnumFormat[]? formats) => ParseFlags(enumType, value, false, null, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <paramref name="enumType"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase) => ParseFlags(enumType, value, string.Empty, ignoreCase, null, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format) => ParseFlags(enumType, value, ignoreCase, null, format);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => ParseFlags(enumType, value, ignoreCase, null, format0, format1);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags(enumType, value, ignoreCase, null, format0, format1, format2);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, params EnumFormat[]? formats) => ParseFlags(enumType, value, ignoreCase, null, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <returns>A <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <paramref name="enumType"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <paramref name="enumType"/>'s underlying type.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, string? delimiter) => ParseFlags(enumType, value, false, delimiter, null);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, string? delimiter, EnumFormat format) => ParseFlags(enumType, value, false, delimiter, format);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, string? delimiter, EnumFormat format0, EnumFormat format1) => ParseFlags(enumType, value, false, delimiter, format0, format1);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags(enumType, value, false, delimiter, format0, format1, format2);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>A <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, string? delimiter, params EnumFormat[]? formats) => ParseFlags(enumType, value, false, delimiter, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <paramref name="enumType"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, string? delimiter) => ParseFlags(enumType, value, string.Empty, ignoreCase, delimiter, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, string? delimiter, EnumFormat format) => ParseFlags(enumType, value, string.Empty, ignoreCase, delimiter, new ValueCollection<EnumFormat>(format));

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, string? delimiter, EnumFormat format0, EnumFormat format1) => ParseFlags(enumType, value, string.Empty, ignoreCase, delimiter, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, string? delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags(enumType, value, string.Empty, ignoreCase, delimiter, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Converts the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <paramref name="enumType"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, string? delimiter, params EnumFormat[]? formats) => ParseFlags(enumType, value, string.Empty, ignoreCase, delimiter, new ValueCollection<EnumFormat>(formats));
#endif

#if SPAN
        private static object? ParseFlags(Type enumType, ReadOnlySpan<char> value, string? strValue, bool ignoreCase, string? delimiter, ValueCollection<EnumFormat> formats)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (info.IsNullable && value.Length == 0)
            {
                return null;
            }

            Preconditions.NotNull(strValue, nameof(value));

            return info.EnumCache.ParseFlags(value, ignoreCase, delimiter, formats);
        }
#else
        private static object? ParseFlags(Type enumType, string? value, string? strValue, bool ignoreCase, string? delimiter, ValueCollection<EnumFormat> formats)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (info.IsNullable && string.IsNullOrEmpty(value))
            {
                return null;
            }

            Preconditions.NotNull(strValue, nameof(value));

            return info.EnumCache.ParseFlags(value!, ignoreCase, delimiter, formats);
        }
#endif

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParseFlags(Type enumType, string? value, out object? result) => TryParseFlags(enumType, value, false, null, out result, null);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, out object? result, EnumFormat format) => TryParseFlags(enumType, value, false, null, out result, format);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, out object? result, EnumFormat format0, EnumFormat format1) => TryParseFlags(enumType, value, false, null, out result, format0, format1);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(enumType, value, false, null, out result, format0, format1, format2);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, out object? result, params EnumFormat[]? formats) => TryParseFlags(enumType, value, false, null, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result) => TryParseFlags(enumType, value, ignoreCase, null, out result, null);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format) => TryParseFlags(enumType, value, ignoreCase, null, out result, format);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1) => TryParseFlags(enumType, value, ignoreCase, null, out result, format0, format1);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(enumType, value, ignoreCase, null, out result, format0, format1, format2);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, out object? result, params EnumFormat[]? formats) => TryParseFlags(enumType, value, ignoreCase, null, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result) => TryParseFlags(enumType, value, false, delimiter, out result, null);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result, EnumFormat format) => TryParseFlags(enumType, value, false, delimiter, out result, format);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1) => TryParseFlags(enumType, value, false, delimiter, out result, format0, format1);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(enumType, value, false, delimiter, out result, format0, format1, format2);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, string? delimiter, out object? result, params EnumFormat[]? formats) => TryParseFlags(enumType, value, false, delimiter, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, null);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result, EnumFormat format) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(format));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string? value, bool ignoreCase, string? delimiter, out object? result, params EnumFormat[]? formats) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(formats));

#if SPAN
        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, out object? result) => TryParseFlags(enumType, value, false, null, out result, null);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, out object? result, EnumFormat format) => TryParseFlags(enumType, value, false, null, out result, format);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, out object? result, EnumFormat format0, EnumFormat format1) => TryParseFlags(enumType, value, false, null, out result, format0, format1);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(enumType, value, false, null, out result, format0, format1, format2);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, out object? result, params EnumFormat[]? formats) => TryParseFlags(enumType, value, false, null, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, out object? result) => TryParseFlags(enumType, value, ignoreCase, null, out result, null);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, out object? result, EnumFormat format) => TryParseFlags(enumType, value, ignoreCase, null, out result, format);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1) => TryParseFlags(enumType, value, ignoreCase, null, out result, format0, format1);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(enumType, value, ignoreCase, null, out result, format0, format1, format2);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, out object? result, params EnumFormat[]? formats) => TryParseFlags(enumType, value, ignoreCase, null, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, string? delimiter, out object? result) => TryParseFlags(enumType, value, false, delimiter, out result, null);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, string? delimiter, out object? result, EnumFormat format) => TryParseFlags(enumType, value, false, delimiter, out result, format);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1) => TryParseFlags(enumType, value, false, delimiter, out result, format0, format1);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(enumType, value, false, delimiter, out result, format0, format1, format2);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, string? delimiter, out object? result, params EnumFormat[]? formats) => TryParseFlags(enumType, value, false, delimiter, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, string? delimiter, out object? result) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, null);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, string? delimiter, out object? result, EnumFormat format) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(format));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, string? delimiter, out object? result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <paramref name="enumType"/> delimited with <paramref name="delimiter"/> to its respective value of type <paramref name="enumType"/>
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a value of type <paramref name="enumType"/> that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, string? delimiter, out object? result, params EnumFormat[]? formats) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(formats));
#endif

#if SPAN
        private static bool TryParseFlags(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, string? delimiter, out object? result, ValueCollection<EnumFormat> formats)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (info.IsNullable && value.Length == 0)
            {
                result = null;
                return true;
            }

            return info.EnumCache.TryParseFlags(value, ignoreCase, delimiter, out result, formats);
        }
#else
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
#endif
        #endregion

        #region EnumMember Extensions
        /// <summary>
        /// Retrieves the flags that compose <paramref name="member"/>'s value.
        /// </summary>
        /// <param name="member">The enum member.</param>
        /// <returns>The flags that compose <paramref name="member"/>'s value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <c>null</c>.</exception>
        public static IEnumerable<object> GetFlags(this EnumMember member)
        {
            Preconditions.NotNull(member, nameof(member));
            return member.GetFlags();
        }

        /// <summary>
        /// Retrieves the <see cref="EnumMember"/>s of the flags that compose <paramref name="member"/>'s value.
        /// </summary>
        /// <param name="member">The enum member.</param>
        /// <returns>The <see cref="EnumMember"/>s of the flags that compose <paramref name="member"/>'s value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <c>null</c>.</exception>
        public static IEnumerable<EnumMember> GetFlagMembers(this EnumMember member)
        {
            Preconditions.NotNull(member, nameof(member));
            return member.GetFlagMembers();
        }
        #endregion
    }
}