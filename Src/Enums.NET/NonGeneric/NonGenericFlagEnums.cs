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
using System.Diagnostics.Contracts;
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
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsFlagEnum(Type enumType) => NonGenericEnums.GetInfo(enumType).IsFlagEnum;

        /// <summary>
        /// Retrieves all the flags defined by <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns>All the flags defined by <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object GetAllFlags(Type enumType) => NonGenericEnums.GetInfo(enumType).AllFlags;
        #endregion

        #region Main Methods
        /// <summary>
        /// Indicates whether <paramref name="value"/> is a valid flag combination of the defined enum values.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication of whether <paramref name="value"/> is a valid flag combination of the defined enum values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type</exception>
        [Pure]
        public static bool IsValidFlagCombination(Type enumType, object value)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (value == null && isNullable)
            {
                return true;
            }

            return enumInfo.IsValidFlagCombination(value);
        }

        /// <summary>
        /// Returns the names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination null is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type</exception>
        [Pure]
        public static string FormatAsFlags(Type enumType, object value) => FormatAsFlags(enumType, value, null, null);

        /// <summary>
        /// Returns <paramref name="value"/>'s flags formatted with <paramref name="formats"/> and delimited with commas
        /// or if empty return the zero flag formatted with <paramref name="formats"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type</exception>
        [Pure]
        public static string FormatAsFlags(Type enumType, object value, params EnumFormat[] formats) => FormatAsFlags(enumType, value, null, formats);

        /// <summary>
        /// Returns the names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type</exception>
        [Pure]
        public static string FormatAsFlags(Type enumType, object value, string delimiter) => FormatAsFlags(enumType, value, delimiter, null);

        /// <summary>
        /// Returns <paramref name="value"/>'s flags formatted with <paramref name="formats"/> and delimited with <paramref name="delimiter"/>
        /// or if empty return the zero flag formatted with <paramref name="formats"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="delimiter"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type</exception>
        [Pure]
        public static string FormatAsFlags(Type enumType, object value, string delimiter, params EnumFormat[] formats)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }

            return enumInfo.FormatAsFlags(value, delimiter, formats);
        }

        /// <summary>
        /// Returns an array of the flags that compose <paramref name="value"/>.
        /// If <paramref name="value"/> is not a valid flag combination null is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">Should be a valid flag combination.</param>
        /// <returns>Array of the flags that compose <paramref name="value"/>.
        /// If <paramref name="value"/> is not a valid flag combination null is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type</exception>
        [Pure]
        public static IEnumerable<object> GetFlags(Type enumType, object value)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (value == null && isNullable)
            {
                return new object[0];
            }

            return enumInfo.GetFlags(value);
        }

        /// <summary>
        /// Indicates if <paramref name="value"/> has any flags set.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <returns>Indication if <paramref name="value"/> has any flags set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type
        /// -or-
        /// <paramref name="value"/> is not a valid flag combination.</exception>
        [Pure]
        public static bool HasAnyFlags(Type enumType, object value)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (value == null && isNullable)
            {
                return false;
            }

            return enumInfo.HasAnyFlags(value);
        }

        /// <summary>
        /// Indicates if <paramref name="value"/> has any flags set that are also set in <paramref name="flagMask"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <param name="flagMask">Must be a valid flag combination.</param>
        /// <returns>Indication if <paramref name="value"/> has any flags set that are also set in <paramref name="flagMask"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type
        /// -or-
        /// <paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static bool HasAnyFlags(Type enumType, object value, object flagMask)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (isNullable)
            {
                if (value == null)
                {
                    return !enumInfo.HasAnyFlags(flagMask);
                }
                if (flagMask == null)
                {
                    enumInfo.ToObject(value);
                    return true;
                }
            }

            return enumInfo.HasAnyFlags(value, flagMask);
        }

        /// <summary>
        /// Indicates if <paramref name="value"/> has all flags set that are defined in <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <returns>Indication if <paramref name="value"/> has all flags set that are defined in <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type
        /// -or-
        /// <paramref name="value"/> is not a valid flag combination.</exception>
        [Pure]
        public static bool HasAllFlags(Type enumType, object value)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (value == null && isNullable)
            {
                return false;
            }

            return enumInfo.HasAllFlags(value);
        }

        /// <summary>
        /// Indicates if <paramref name="value"/> has all of the flags set that are also set in <paramref name="flagMask"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <param name="flagMask">Must be a valid flag combination.</param>
        /// <returns>Indication if <paramref name="value"/> has all of the flags set that are also set in <paramref name="flagMask"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type
        /// -or-
        /// <paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static bool HasAllFlags(Type enumType, object value, object flagMask)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (isNullable)
            {
                if (value == null)
                {
                    return !enumInfo.HasAnyFlags(flagMask);
                }
                if (flagMask == null)
                {
                    enumInfo.ToObject(value);
                    return true;
                }
            }

            return enumInfo.HasAllFlags(value, flagMask);
        }

        /// <summary>
        /// Returns <paramref name="value"/> with all of it's flags toggled. Equivalent to the bitwise "xor" operator with <see cref="GetAllFlags(Type)"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <returns><paramref name="value"/> with all of it's flags toggled.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type
        /// -or-
        /// <paramref name="value"/> is not a valid flag combination.</exception>
        [Pure]
        public static object ToggleFlags(Type enumType, object value)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (value == null && isNullable)
            {
                return enumInfo.AllFlags;
            }

            return enumInfo.ToggleFlags(value);
        }

        /// <summary>
        /// Returns <paramref name="value"/> while toggling the flags that are set in <paramref name="flagMask"/>. Equivalent to the bitwise "xor" operator.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <param name="flagMask">Must be a valid flag combination.</param>
        /// <returns><paramref name="value"/> while toggling the flags that are set in <paramref name="flagMask"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type
        /// -or-
        /// <paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static object ToggleFlags(Type enumType, object value, object flagMask)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (isNullable)
            {
                if (value == null)
                {
                    return enumInfo.ToObject(flagMask);
                }
                if (flagMask == null)
                {
                    return enumInfo.ToObject(value);
                }
            }

            return enumInfo.ToggleFlags(value, flagMask);
        }

        /// <summary>
        /// Returns <paramref name="value"/> with only the flags that are also set in <paramref name="flagMask"/>. Equivalent to the bitwise "and" operation.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <param name="flagMask">Must be a valid flag combination.</param>
        /// <returns><paramref name="value"/> with only the flags that are also set in <paramref name="flagMask"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type
        /// -or-
        /// <paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static object CommonFlags(Type enumType, object value, object flagMask)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (isNullable)
            {
                if (value == null)
                {
                    if (flagMask != null)
                    {
                        enumInfo.ToObject(flagMask);
                    }
                    return null;
                }
                if (flagMask == null)
                {
                    enumInfo.ToObject(value);
                    return null;
                }
            }

            return enumInfo.CommonFlags(value, flagMask);
        }

        /// <summary>
        /// Returns <paramref name="flag0"/> with the flags specified in <paramref name="flag1"/> set. Equivalent to the bitwise "or" operation.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="flag0">Must be a valid flag combination.</param>
        /// <param name="flag1">Must be a valid flag combination.</param>
        /// <returns><paramref name="flag0"/> with the flags specified in <paramref name="flag1"/> set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="flag0"/> is an invalid type
        /// -or-
        /// <paramref name="flag0"/> or <paramref name="flag1"/> is not a valid flag combination.</exception>
        [Pure]
        public static object CombineFlags(Type enumType, object flag0, object flag1)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (isNullable)
            {
                if (flag0 == null)
                {
                    return enumInfo.ToObject(flag1);
                }
                if (flag1 == null)
                {
                    return enumInfo.ToObject(flag0);
                }
            }

            return enumInfo.CombineFlags(flag0, flag1);
        }

        /// <summary>
        /// Returns all of <paramref name="flags"/> combined with the bitwise "or" operation.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="flags">Must be valid flag combinations.</param>
        /// <returns></returns>
        [Pure]
        public static object CombineFlags(Type enumType, params object[] flags)
        {
            Preconditions.NotNull(flags, nameof(flags));

            var isNullable = new OptionalOutParameter<bool>();
            return NonGenericEnums.GetInfo(enumType, isNullable).CombineFlags(isNullable ? flags.Where(flag => flag != null) : flags);
        }

        /// <summary>
        /// Returns <paramref name="value"/> with the flags specified in <paramref name="flagMask"/> cleared.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <param name="flagMask">Must be a valid flag combination.</param>
        /// <returns><paramref name="value"/> with the flags specified in <paramref name="flagMask"/> cleared.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type
        /// -or-
        /// <paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static object ExcludeFlags(Type enumType, object value, object flagMask)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (isNullable)
            {
                if (value == null)
                {
                    if (flagMask != null)
                    {
                        enumInfo.ToObject(flagMask);
                    }
                    return null;
                }
                if (flagMask == null)
                {
                    return enumInfo.ToObject(value);
                }
            }

            return enumInfo.ExcludeFlags(value, flagMask);
        }
        #endregion

        #region Parsing
        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// to an equivalent enumerated object.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        [Pure]
        public static object Parse(Type enumType, string value) => Parse(enumType, value, false, null, null);

        [Pure]
        public static object Parse(Type enumType, string value, params EnumFormat[] parseFormatOrder) => Parse(enumType, value, false, null, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        [Pure]
        public static object Parse(Type enumType, string value, bool ignoreCase) => Parse(enumType, value, ignoreCase, null, null);

        [Pure]
        public static object Parse(Type enumType, string value, bool ignoreCase, params EnumFormat[] parseFormatOrder) => Parse(enumType, value, ignoreCase, null, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// delimited with the specified delimiter to an equivalent enumerated object.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="delimiter">The effective delimiter will be the <paramref name="delimiter"/> trimmed if it's not all whitespace.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="delimiter"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
        /// -or-
        /// <paramref name="delimiter"/> is an empty string.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        [Pure]
        public static object Parse(Type enumType, string value, string delimiter) => Parse(enumType, value, false, delimiter, null);

        [Pure]
        public static object Parse(Type enumType, string value, string delimiter, params EnumFormat[] parseFormatOrder) => Parse(enumType, value, false, delimiter, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// delimited with the specified delimiter to an equivalent enumerated object.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="delimiter">The effective delimiter will be the <paramref name="delimiter"/> trimmed if it's not all whitespace.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="delimiter"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
        /// -or-
        /// <paramref name="delimiter"/> is an empty string.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        [Pure]
        public static object Parse(Type enumType, string value, bool ignoreCase, string delimiter) => Parse(enumType, value, ignoreCase, delimiter, null);

        [Pure]
        public static object Parse(Type enumType, string value, bool ignoreCase, string delimiter, params EnumFormat[] parseFormatOrder)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (string.IsNullOrEmpty(value) && isNullable)
            {
                return null;
            }

            return enumInfo.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder);
        }

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified enumerated value.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ParseOrDefault(Type enumType, string value, object defaultEnum = null) => ParseOrDefault(enumType, value, false, null, defaultEnum, null);

        [Pure]
        public static object ParseOrDefault(Type enumType, string value, object defaultEnum, params EnumFormat[] parseFormatOrder) => ParseOrDefault(enumType, value, false, null, defaultEnum, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified enumerated value.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, object defaultEnum = null) => ParseOrDefault(enumType, value, ignoreCase, null, defaultEnum, null);

        [Pure]
        public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, object defaultEnum, params EnumFormat[] parseFormatOrder) => ParseOrDefault(enumType, value, ignoreCase, null, defaultEnum, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants delimited with the specified delimiter to an equivalent enumerated object but if it fails
        /// returns the specified enumerated value.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="delimiter"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="delimiter"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="delimiter"/> is an empty string.</exception>
        [Pure]
        public static object ParseOrDefault(Type enumType, string value, string delimiter, object defaultEnum = null) => ParseOrDefault(enumType, value, false, delimiter, defaultEnum, null);

        [Pure]
        public static object ParseOrDefault(Type enumType, string value, string delimiter, object defaultEnum, params EnumFormat[] parseFormatOrder) => ParseOrDefault(enumType, value, false, delimiter, defaultEnum, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants delimited with the specified delimiter to an equivalent enumerated object but if it fails
        /// returns the specified enumerated value. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="delimiter"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="delimiter"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="delimiter"/> is an empty string.</exception>
        [Pure]
        public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, string delimiter, object defaultEnum = null) => ParseOrDefault(enumType, value, ignoreCase, delimiter, defaultEnum, null);

        [Pure]
        public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, string delimiter, object defaultEnum, params EnumFormat[] parseFormatOrder)
        {
            object result;
            return TryParse(enumType, value, ignoreCase, delimiter, out result, parseFormatOrder) ? result : defaultEnum;
        }

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryParse(Type enumType, string value, out object result) => TryParse(enumType, value, false, null, out result, null);

        [Pure]
        public static bool TryParse(Type enumType, string value, out object result, params EnumFormat[] parseFormatOrder) => TryParse(enumType, value, false, null, out result, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryParse(Type enumType, string value, bool ignoreCase, out object result) => TryParse(enumType, value, ignoreCase, null, out result, null);

        [Pure]
        public static bool TryParse(Type enumType, string value, bool ignoreCase, out object result, params EnumFormat[] parseFormatOrder) => TryParse(enumType, value, ignoreCase, null, out result, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants delimited with the specified delimiter to an equivalent enumerated object. The return value
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="delimiter"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="delimiter"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="delimiter"/> is an empty string.</exception>
        [Pure]
        public static bool TryParse(Type enumType, string value, string delimiter, out object result) => TryParse(enumType, value, false, delimiter, out result, null);

        [Pure]
        public static bool TryParse(Type enumType, string value, string delimiter, out object result, params EnumFormat[] parseFormatOrder) => TryParse(enumType, value, false, delimiter, out result, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants delimited with the specified delimiter to an equivalent enumerated object. The return value
        /// indicates whether the conversion succeeded. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="delimiter"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="delimiter"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="delimiter"/> is an empty string.</exception>
        [Pure]
        public static bool TryParse(Type enumType, string value, bool ignoreCase, string delimiter, out object result) => TryParse(enumType, value, ignoreCase, delimiter, out result, null);

        [Pure]
        public static bool TryParse(Type enumType, string value, bool ignoreCase, string delimiter, out object result, params EnumFormat[] parseFormatOrder)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnums.GetInfo(enumType, isNullable);

            if (string.IsNullOrEmpty(value) && isNullable)
            {
                result = null;
                return true;
            }

            return enumInfo.TryParseFlags(value, ignoreCase, delimiter, out result, parseFormatOrder);
        }
        #endregion
    }
}
