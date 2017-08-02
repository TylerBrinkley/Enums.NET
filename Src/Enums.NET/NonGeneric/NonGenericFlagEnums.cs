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
using System.Linq;

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
        public static bool IsFlagEnum(Type enumType) => NonGenericEnums.GetInfo(enumType).IsFlagEnum;

        /// <summary>
        /// Retrieves all the flags defined by <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns>All the flags defined by <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static object GetAllFlags(Type enumType) => NonGenericEnums.GetInfo(enumType).AllFlags;
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
        public static bool IsValidFlagCombination(Type enumType, object value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return true;
            }

            return info.EnumInfo.IsValidFlagCombination(value);
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
        public static string FormatFlags(Type enumType, object value) => FormatFlags(enumType, value, null, null);

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
        public static string FormatFlags(Type enumType, object value, params EnumFormat[] formats) => FormatFlags(enumType, value, null, formats);

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
        public static string FormatFlags(Type enumType, object value, string delimiter) => FormatFlags(enumType, value, delimiter, null);

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
        public static string FormatFlags(Type enumType, object value, string delimiter, params EnumFormat[] formats)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.FormatFlags(value, delimiter, formats);
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
        public static IEnumerable<object> GetFlags(Type enumType, object value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return new object[0];
            }

            return info.EnumInfo.GetFlags(value);
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
        public static IEnumerable<EnumMember> GetFlagMembers(Type enumType, object value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return new EnumMember[0];
            }

            return info.EnumInfo.GetFlagMembers(value);
        }

        /// <summary>
        /// Retrieves the flag count of <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns>The flag count of <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static int GetFlagCount(Type enumType) => NonGenericEnums.GetNonGenericEnumInfo(enumType).EnumInfo.GetFlagCount();

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
        public static int GetFlagCount(Type enumType, object value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return 0;
            }

            return info.EnumInfo.GetFlagCount(value);
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
        public static int GetFlagCount(Type enumType, object value, object otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var enumInfo = info.EnumInfo;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    if (otherFlags != null)
                    {
                        enumInfo.ToObject(otherFlags);
                    }
                    return 0;
                }
                if (otherFlags == null)
                {
                    enumInfo.ToObject(value);
                    return 0;
                }
            }

            return enumInfo.GetFlagCount(value, otherFlags);
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
        public static bool HasAnyFlags(Type enumType, object value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return false;
            }

            return info.EnumInfo.HasAnyFlags(value);
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
        public static bool HasAnyFlags(Type enumType, object value, object otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var enumInfo = info.EnumInfo;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    return !enumInfo.HasAnyFlags(otherFlags);
                }
                if (otherFlags == null)
                {
                    enumInfo.ToObject(value);
                    return true;
                }
            }

            return enumInfo.HasAnyFlags(value, otherFlags);
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
        public static bool HasAllFlags(Type enumType, object value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return false;
            }

            return info.EnumInfo.HasAllFlags(value);
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
        public static bool HasAllFlags(Type enumType, object value, object otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var enumInfo = info.EnumInfo;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    return !enumInfo.HasAnyFlags(otherFlags);
                }
                if (otherFlags == null)
                {
                    enumInfo.ToObject(value);
                    return true;
                }
            }

            return enumInfo.HasAllFlags(value, otherFlags);
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
        public static object ToggleFlags(Type enumType, object value)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var enumInfo = info.EnumInfo;

            if (value == null && info.IsNullable)
            {
                return enumInfo.AllFlags;
            }

            return enumInfo.ToggleFlags(value);
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
        public static object ToggleFlags(Type enumType, object value, object otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var enumInfo = info.EnumInfo;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    return enumInfo.ToObject(otherFlags);
                }
                if (otherFlags == null)
                {
                    return enumInfo.ToObject(value);
                }
            }

            return enumInfo.ToggleFlags(value, otherFlags);
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
        public static object CommonFlags(Type enumType, object value, object otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var enumInfo = info.EnumInfo;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    if (otherFlags != null)
                    {
                        enumInfo.ToObject(otherFlags);
                    }
                    return null;
                }
                if (otherFlags == null)
                {
                    enumInfo.ToObject(value);
                    return null;
                }
            }

            return enumInfo.CommonFlags(value, otherFlags);
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
        public static object CombineFlags(Type enumType, object value, object otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var enumInfo = info.EnumInfo;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    return enumInfo.ToObject(otherFlags);
                }
                if (otherFlags == null)
                {
                    return enumInfo.ToObject(value);
                }
            }

            return enumInfo.CombineFlags(value, otherFlags);
        }

        /// <summary>
        /// Combines all of the flags of <paramref name="flags"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="flags">The flags enum values.</param>
        /// <returns>Combination of all of the flags of <paramref name="flags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="flags"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="flags"/> contains a value that is of an invalid type.</exception>
        public static object CombineFlags(Type enumType, params object[] flags) => CombineFlags(enumType, (IEnumerable<object>)flags);

        /// <summary>
        /// Combines all of the flags of <paramref name="flags"/>.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <param name="flags">The flags enum values.</param>
        /// <returns>Combination of all of the flags of <paramref name="flags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="flags"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="flags"/> contains a value that is of an invalid type.</exception>
        public static object CombineFlags(Type enumType, IEnumerable<object> flags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            return info.EnumInfo.CombineFlags(info.IsNullable ? flags?.Where(flag => flag != null) : flags);
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
        public static object RemoveFlags(Type enumType, object value, object otherFlags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            var enumInfo = info.EnumInfo;

            if (info.IsNullable)
            {
                if (value == null)
                {
                    if (otherFlags != null)
                    {
                        enumInfo.ToObject(otherFlags);
                    }
                    return null;
                }
                if (otherFlags == null)
                {
                    return enumInfo.ToObject(value);
                }
            }

            return enumInfo.RemoveFlags(value, otherFlags);
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
        public static object ParseFlags(Type enumType, string value) => ParseFlags(enumType, value, false, null, null);

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
        public static object ParseFlags(Type enumType, string value, params EnumFormat[] formats) => ParseFlags(enumType, value, false, null, formats);

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
        public static object ParseFlags(Type enumType, string value, bool ignoreCase) => ParseFlags(enumType, value, ignoreCase, null, null);

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
        public static object ParseFlags(Type enumType, string value, bool ignoreCase, params EnumFormat[] formats) => ParseFlags(enumType, value, ignoreCase, null, formats);

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
        public static object ParseFlags(Type enumType, string value, string delimiter) => ParseFlags(enumType, value, false, delimiter, null);

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
        public static object ParseFlags(Type enumType, string value, string delimiter, params EnumFormat[] formats) => ParseFlags(enumType, value, false, delimiter, formats);

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
        public static object ParseFlags(Type enumType, string value, bool ignoreCase, string delimiter) => ParseFlags(enumType, value, ignoreCase, delimiter, null);

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
        public static object ParseFlags(Type enumType, string value, bool ignoreCase, string delimiter, params EnumFormat[] formats)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (string.IsNullOrEmpty(value) && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.ParseFlags(value, ignoreCase, delimiter, formats);
        }

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
        public static bool TryParseFlags(Type enumType, string value, out object result) => TryParseFlags(enumType, value, false, null, out result, null);

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
        public static bool TryParseFlags(Type enumType, string value, out object result, params EnumFormat[] formats) => TryParseFlags(enumType, value, false, null, out result, formats);

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
        public static bool TryParseFlags(Type enumType, string value, bool ignoreCase, out object result) => TryParseFlags(enumType, value, ignoreCase, null, out result, null);

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
        public static bool TryParseFlags(Type enumType, string value, bool ignoreCase, out object result, params EnumFormat[] formats) => TryParseFlags(enumType, value, ignoreCase, null, out result, formats);

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
        public static bool TryParseFlags(Type enumType, string value, string delimiter, out object result) => TryParseFlags(enumType, value, false, delimiter, out result, null);

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
        public static bool TryParseFlags(Type enumType, string value, string delimiter, out object result, params EnumFormat[] formats) => TryParseFlags(enumType, value, false, delimiter, out result, formats);

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
        public static bool TryParseFlags(Type enumType, string value, bool ignoreCase, string delimiter, out object result) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, null);

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
        public static bool TryParseFlags(Type enumType, string value, bool ignoreCase, string delimiter, out object result, params EnumFormat[] formats)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (string.IsNullOrEmpty(value) && info.IsNullable)
            {
                result = null;
                return true;
            }

            return info.EnumInfo.TryParseFlags(value, ignoreCase, delimiter, out result, formats);
        }
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
