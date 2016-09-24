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
    /// A non-generic implementation of the static class <see cref="FlagEnums"/>. When the type is known the <see cref="FlagEnums"/> class should be used instead, to provide type safety and to avoid boxing.
    /// </summary>
    public static class NonGenericFlagEnums
    {
        #region "Properties"
        /// <summary>
        /// Indicates if <paramref name="enumType"/> is marked with the <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns>Indication if <paramref name="enumType"/> is marked with the <see cref="FlagsAttribute"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool IsFlagEnum(Type enumType) => NonGenericEnums.GetInfo(enumType).IsFlagEnum;

        /// <summary>
        /// Retrieves all the flags defined by <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns>All the flags defined by <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static object GetAllFlags(Type enumType) => NonGenericEnums.GetInfo(enumType).AllFlags;
        #endregion

        #region Main Methods
        /// <summary>
        /// Indicates whether <paramref name="value"/> is a valid flag combination of the defined enum values.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication of whether <paramref name="value"/> is a valid flag combination of the defined enum values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
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
        /// Returns the names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination null is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
        public static string FormatFlags(Type enumType, object value) => FormatFlags(enumType, value, null, null);

        /// <summary>
        /// Returns <paramref name="value"/>'s flags formatted with <paramref name="formatOrder"/> and delimited with commas
        /// or if empty return the zero flag formatted with <paramref name="formatOrder"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="formatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type
        /// -or-
        /// <paramref name="formatOrder"/> contains an invalid value.</exception>
        public static string FormatFlags(Type enumType, object value, params EnumFormat[] formatOrder) => FormatFlags(enumType, value, null, formatOrder);

        /// <summary>
        /// Returns the names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
        public static string FormatFlags(Type enumType, object value, string delimiter) => FormatFlags(enumType, value, delimiter, null);

        /// <summary>
        /// Returns <paramref name="value"/>'s flags formatted with <paramref name="formatOrder"/> and delimited with <paramref name="delimiter"/>
        /// or if empty return the zero flag formatted with <paramref name="formatOrder"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="delimiter"></param>
        /// <param name="formatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type
        /// -or-
        /// <paramref name="formatOrder"/> contains an invalid value.</exception>
        public static string FormatFlags(Type enumType, object value, string delimiter, params EnumFormat[] formatOrder)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.FormatFlags(value, delimiter, formatOrder);
        }

        /// <summary>
        /// Returns an IEnumerable of the flags that compose <paramref name="value"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>IEnumerable of the flags that compose <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
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
        /// Returns an IEnumerable of the flags that compose <paramref name="value"/> as EnumMember's.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>IEnumerable of the flags that compose <paramref name="value"/> as EnumMember's.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
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
        /// Indicates if <paramref name="value"/> has any flags set.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if <paramref name="value"/> has any flags set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
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
        /// Indicates if <paramref name="value"/> has any flags set that are also set in <paramref name="otherFlags"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="otherFlags"></param>
        /// <returns>Indication if <paramref name="value"/> has any flags set that are also set in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
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
        /// Indicates if <paramref name="value"/> has all flags set that are defined in <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if <paramref name="value"/> has all flags set that are defined in <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
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
        /// Indicates if <paramref name="value"/> has all of the flags set that are also set in <paramref name="otherFlags"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="otherFlags"></param>
        /// <returns>Indication if <paramref name="value"/> has all of the flags set that are also set in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
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
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns><paramref name="value"/> with all of it's flags toggled.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
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
        /// Returns <paramref name="value"/> while toggling the flags that are set in <paramref name="otherFlags"/>. Equivalent to the bitwise "xor" operator.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="otherFlags"></param>
        /// <returns><paramref name="value"/> while toggling the flags that are set in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
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
        /// Returns <paramref name="value"/> with only the flags that are also set in <paramref name="otherFlags"/>. Equivalent to the bitwise "and" operation.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="otherFlags"></param>
        /// <returns><paramref name="value"/> with only the flags that are also set in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
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
        /// Returns <paramref name="value"/> with the flags specified in <paramref name="otherFlags"/> set. Equivalent to the bitwise "or" operation.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="otherFlags"></param>
        /// <returns><paramref name="value"/> with the flags specified in <paramref name="otherFlags"/> set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
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
        /// Returns all of <paramref name="flags"/> combined with the bitwise "or" operation.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="flags">Must be valid flag combinations.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="flags"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="flags"/> contains an object that is an invalid type.</exception>
        public static object CombineFlags(Type enumType, params object[] flags)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
            
            return info.EnumInfo.CombineFlags(info.IsNullable ? flags?.Where(flag => flag != null) : flags);
        }

        /// <summary>
        /// Returns <paramref name="value"/> with the flags specified in <paramref name="otherFlags"/> cleared.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="otherFlags"></param>
        /// <returns><paramref name="value"/> with the flags specified in <paramref name="otherFlags"/> cleared.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type.</exception>
        public static object ExcludeFlags(Type enumType, object value, object otherFlags)
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

            return enumInfo.ExcludeFlags(value, otherFlags);
        }
        #endregion

        #region Parsing
        /// <summary>
        /// Converts the string representation of one or more enum members to an equivalent enum value.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object ParseFlags(Type enumType, string value) => ParseFlags(enumType, value, false, null, null);

        /// <summary>
        /// Converts the string representation of one or more enum members to an equivalent enum value.
        /// The parameter <paramref name="parseFormatOrder"/> specifies the order of enum parsing formats.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
        /// -or-
        /// <paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object ParseFlags(Type enumType, string value, params EnumFormat[] parseFormatOrder) => ParseFlags(enumType, value, false, null, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of one or more enum members to an equivalent enum value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object ParseFlags(Type enumType, string value, bool ignoreCase) => ParseFlags(enumType, value, ignoreCase, null, null);

        /// <summary>
        /// Converts the string representation of one or more enum members to an equivalent enum value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The parameter <paramref name="parseFormatOrder"/> specifies the order of enum parsing formats.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
        /// -or-
        /// <paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object ParseFlags(Type enumType, string value, bool ignoreCase, params EnumFormat[] parseFormatOrder) => ParseFlags(enumType, value, ignoreCase, null, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of one or more enum members delimited with the specified <paramref name="delimiter"/> to an equivalent enum value.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="delimiter">The effective delimiter will be the <paramref name="delimiter"/> trimmed if it's not all whitespace.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="delimiter"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
        /// -or-
        /// <paramref name="delimiter"/> is an empty string.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object ParseFlags(Type enumType, string value, string delimiter) => ParseFlags(enumType, value, false, delimiter, null);

        /// <summary>
        /// Converts the string representation of one or more enum members delimited with the specified <paramref name="delimiter"/> to an equivalent enum value.
        /// The parameter <paramref name="parseFormatOrder"/> specifies the order of enum parsing formats.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="delimiter"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
        /// -or-
        /// <paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object ParseFlags(Type enumType, string value, string delimiter, params EnumFormat[] parseFormatOrder) => ParseFlags(enumType, value, false, delimiter, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of one or more enum members delimited with the specified <paramref name="delimiter"/> to an equivalent enum value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="delimiter">The effective delimiter will be the <paramref name="delimiter"/> trimmed if it's not all whitespace.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="delimiter"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
        /// -or-
        /// <paramref name="delimiter"/> is an empty string.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object ParseFlags(Type enumType, string value, bool ignoreCase, string delimiter) => ParseFlags(enumType, value, ignoreCase, delimiter, null);

        /// <summary>
        /// Converts the string representation of one or more enum members delimited with the specified <paramref name="delimiter"/> to an equivalent enum value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The parameter <paramref name="parseFormatOrder"/> specifies the order of enum parsing formats.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="delimiter"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
        /// -or-
        /// <paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        public static object ParseFlags(Type enumType, string value, bool ignoreCase, string delimiter, params EnumFormat[] parseFormatOrder)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (string.IsNullOrEmpty(value) && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder);
        }

        /// <summary>
        /// Tries to convert the specified string representation of one or more enum members to an equivalent enum value. The return value
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParseFlags(Type enumType, string value, out object result) => TryParseFlags(enumType, value, false, null, out result, null);

        /// <summary>
        /// Tries to convert the specified string representation of one or more enum members to an equivalent enum value. The return value
        /// indicates whether the conversion succeeded. The parameter <paramref name="parseFormatOrder"/> specifies the order of enum parsing formats.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string value, out object result, params EnumFormat[] parseFormatOrder) => TryParseFlags(enumType, value, false, null, out result, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of one or more enum members to an equivalent enum value. The return value
        /// indicates whether the conversion succeeded. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParseFlags(Type enumType, string value, bool ignoreCase, out object result) => TryParseFlags(enumType, value, ignoreCase, null, out result, null);

        /// <summary>
        /// Tries to convert the specified string representation of one or more enum members to an equivalent enum value. The return value
        /// indicates whether the conversion succeeded. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The parameter <paramref name="parseFormatOrder"/> specifies the order of enum parsing formats.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="result"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string value, bool ignoreCase, out object result, params EnumFormat[] parseFormatOrder) => TryParseFlags(enumType, value, ignoreCase, null, out result, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of one or more enum members delimited with the specified <paramref name="delimiter"/> to an equivalent enum value. The return value
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="delimiter"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParseFlags(Type enumType, string value, string delimiter, out object result) => TryParseFlags(enumType, value, false, delimiter, out result, null);

        /// <summary>
        /// Tries to convert the specified string representation of one or more enum members delimited with the specified <paramref name="delimiter"/> to an equivalent enum value. The return value
        /// indicates whether the conversion succeeded. The parameter <paramref name="parseFormatOrder"/> specifies the order of enum parsing formats.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="delimiter"></param>
        /// <param name="result"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string value, string delimiter, out object result, params EnumFormat[] parseFormatOrder) => TryParseFlags(enumType, value, false, delimiter, out result, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of one or more enum members delimited with the specified <paramref name="delimiter"/> to an equivalent enum value. The return value
        /// indicates whether the conversion succeeded. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="delimiter"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static bool TryParseFlags(Type enumType, string value, bool ignoreCase, string delimiter, out object result) => TryParseFlags(enumType, value, ignoreCase, delimiter, out result, null);

        /// <summary>
        /// Tries to convert the specified string representation of one or more enum members delimited with the specified <paramref name="delimiter"/> to an equivalent enum value. The return value
        /// indicates whether the conversion succeeded. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The parameter <paramref name="parseFormatOrder"/> specifies the order of enum parsing formats.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="delimiter"></param>
        /// <param name="result"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        public static bool TryParseFlags(Type enumType, string value, bool ignoreCase, string delimiter, out object result, params EnumFormat[] parseFormatOrder)
        {
            var info = NonGenericEnums.GetNonGenericEnumInfo(enumType);

            if (string.IsNullOrEmpty(value) && info.IsNullable)
            {
                result = null;
                return true;
            }

            return info.EnumInfo.TryParseFlags(value, ignoreCase, delimiter, out result, parseFormatOrder);
        }
        #endregion

        #region EnumMember Extensions
        /// <summary>
        /// Returns an IEnumerable of the flags that compose <paramref name="member"/>.
        /// </summary>
        /// <param name="member"></param>
        /// <returns>IEnumerable of the flags that compose <paramref name="member"/>.</returns>
        public static IEnumerable<object> GetFlags(this EnumMember member)
        {
            Preconditions.NotNull(member, nameof(member));
            return member.GetFlags();
        }

        /// <summary>
        /// Returns an IEnumerable of the flags that compose <paramref name="member"/> as EnumMember's.
        /// </summary>
        /// <param name="member"></param>
        /// <returns>IEnumerable of the flags that compose <paramref name="member"/> as EnumMember's.</returns>
        public static IEnumerable<EnumMember> GetFlagMembers(this EnumMember member)
        {
            Preconditions.NotNull(member, nameof(member));
            return member.GetFlagMembers();
        }
        #endregion
    }
}
