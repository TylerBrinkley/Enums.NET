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
using EnumsNET.Utilities;

#if SPAN
using ParseType = System.ReadOnlySpan<char>;
#else
using ParseType = System.String;
#endif

namespace EnumsNET.Unsafe
{
    /// <summary>
    /// A type-unsafe version of <see cref="FlagEnums"/> which is useful when dealing with generics
    /// and instead throws an <see cref="ArgumentException"/> if TEnum is not an enum type.
    /// </summary>
    public static class UnsafeFlagEnums
    {
        #region "Properties"
        /// <summary>
        /// Indicates if <typeparamref name="TEnum"/> is marked with the <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>Indication if <typeparamref name="TEnum"/> is marked with the <see cref="FlagsAttribute"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool IsFlagEnum<TEnum>() => UnsafeEnums.GetCache<TEnum>().IsFlagEnum;

        /// <summary>
        /// Retrieves all the flags defined by <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>All the flags defined by <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum GetAllFlags<TEnum>()
        {
            TEnum result = default;
            UnsafeEnums.GetCache<TEnum>().GetAllFlags(ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// Indicates whether <paramref name="value"/> is a valid flag combination of <typeparamref name="TEnum"/>'s defined flags.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>Indication of whether <paramref name="value"/> is a valid flag combination of <typeparamref name="TEnum"/>'s defined flags.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool IsValidFlagCombination<TEnum>(TEnum value) => UnsafeEnums.GetCache<TEnum>().IsValidFlagCombination(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Retrieves the names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static string FormatFlags<TEnum>(TEnum value) => FormatFlags(value, null, default(ValueCollection<EnumFormat>));

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with <paramref name="format"/> and delimited with commas
        /// or if empty returns the zero flag formatted with <paramref name="format"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with <paramref name="format"/> and delimited with commas
        /// or if empty returns the zero flag formatted with <paramref name="format"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static string FormatFlags<TEnum>(TEnum value, EnumFormat format) => FormatFlags(value, null, format);

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with formats and delimited with commas
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with formats and delimited with commas
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static string FormatFlags<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1) => FormatFlags(value, null, format0, format1);

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with formats and delimited with commas
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use.</param>
        /// <param name="format2">The third output format to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with formats and delimited with commas
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static string FormatFlags<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FormatFlags(value, null, format0, format1, format2);

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with <paramref name="formats"/> and delimited with commas
        /// or if empty returns the zero flag formatted with <paramref name="formats"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="formats">The output formats to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with <paramref name="formats"/> and delimited with commas
        /// or if empty returns the zero flag formatted with <paramref name="formats"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static string FormatFlags<TEnum>(TEnum value, params EnumFormat[] formats) => FormatFlags(value, null, formats);

        /// <summary>
        /// Retrieves the names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="delimiter">The delimiter to use to separate individual flags.</param>
        /// <returns>The names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static string FormatFlags<TEnum>(TEnum value, string delimiter) => FormatFlags(value, delimiter, default(ValueCollection<EnumFormat>));

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with <paramref name="format"/> and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with <paramref name="format"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="delimiter">The delimiter to use to separate individual flags.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with <paramref name="format"/> and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with <paramref name="format"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static string FormatFlags<TEnum>(TEnum value, string delimiter, EnumFormat format) => FormatFlags(value, delimiter, new ValueCollection<EnumFormat>(format));

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with formats and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="delimiter">The delimiter to use to separate individual flags.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with formats and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static string FormatFlags<TEnum>(TEnum value, string delimiter, EnumFormat format0, EnumFormat format1) => FormatFlags(value, delimiter, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with formats and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="delimiter">The delimiter to use to separate individual flags.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use.</param>
        /// <param name="format2">The third output format to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with formats and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with formats.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static string FormatFlags<TEnum>(TEnum value, string delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FormatFlags(value, delimiter, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Retrieves <paramref name="value"/>'s flags formatted with <paramref name="formats"/> and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with <paramref name="formats"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="delimiter">The delimiter to use to separate individual flags.</param>
        /// <param name="formats">The output formats to use.</param>
        /// <returns><paramref name="value"/>'s flags formatted with <paramref name="formats"/> and delimited with <paramref name="delimiter"/>
        /// or if empty returns the zero flag formatted with <paramref name="formats"/>.
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static string FormatFlags<TEnum>(TEnum value, string delimiter, params EnumFormat[] formats) => FormatFlags(value, delimiter, new ValueCollection<EnumFormat>(formats));

        private static string FormatFlags<TEnum>(TEnum value, string delimiter, ValueCollection<EnumFormat> formats) => UnsafeEnums.GetCache<TEnum>().FormatFlags(ref UnsafeUtility.As<TEnum, byte>(ref value), delimiter, formats);

        /// <summary>
        /// Retrieves the flags that compose <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The flags that compose <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static IEnumerable<TEnum> GetFlags<TEnum>(TEnum value) => UnsafeEnums.GetBridge<TEnum>().GetFlags(value);

        /// <summary>
        /// Retrieves the <see cref="EnumMember{TEnum}"/>s of the flags that compose <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The <see cref="EnumMember{TEnum}"/>s of the flags that compose <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static IEnumerable<EnumMember<TEnum>> GetFlagMembers<TEnum>(TEnum value) => UnsafeEnums.GetCache<TEnum>().GetFlagMembers(ref UnsafeUtility.As<TEnum, byte>(ref value)).Select(m => (EnumMember<TEnum>)m);

        /// <summary>
        /// Retrieves the flag count of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>The flag count of <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static int GetFlagCount<TEnum>() => UnsafeEnums.GetCache<TEnum>().GetFlagCount();

        /// <summary>
        /// Retrieves the flag count of <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The flag count of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static int GetFlagCount<TEnum>(TEnum value) => UnsafeEnums.GetCache<TEnum>().GetFlagCount(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Retrieves the flag count of <paramref name="otherFlags"/> that <paramref name="value"/> has.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>The flag count of <paramref name="otherFlags"/> that <paramref name="value"/> has.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static int GetFlagCount<TEnum>(TEnum value, TEnum otherFlags) => UnsafeEnums.GetCache<TEnum>().GetFlagCount(ref UnsafeUtility.As<TEnum, byte>(ref value), ref UnsafeUtility.As<TEnum, byte>(ref otherFlags));

        /// <summary>
        /// Indicates if <paramref name="value"/> has any flags.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has any flags.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool HasAnyFlags<TEnum>(TEnum value) => UnsafeEnums.GetCache<TEnum>().HasAnyFlags(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Indicates if <paramref name="value"/> has any flags that are in <paramref name="otherFlags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has any flags that are in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool HasAnyFlags<TEnum>(TEnum value, TEnum otherFlags) => UnsafeEnums.GetCache<TEnum>().HasAnyFlags(ref UnsafeUtility.As<TEnum, byte>(ref value), ref UnsafeUtility.As<TEnum, byte>(ref otherFlags));

        /// <summary>
        /// Indicates if <paramref name="value"/> has all of the flags that are defined in <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has all of the flags that are defined in <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool HasAllFlags<TEnum>(TEnum value) => UnsafeEnums.GetCache<TEnum>().HasAllFlags(ref UnsafeUtility.As<TEnum, byte>(ref value));

        /// <summary>
        /// Indicates if <paramref name="value"/> has all of the flags that are in <paramref name="otherFlags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has all of the flags that are in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool HasAllFlags<TEnum>(TEnum value, TEnum otherFlags) => UnsafeEnums.GetCache<TEnum>().HasAllFlags(ref UnsafeUtility.As<TEnum, byte>(ref value), ref UnsafeUtility.As<TEnum, byte>(ref otherFlags));

        /// <summary>
        /// Returns <paramref name="value"/> with all of it's flags toggled. Equivalent to the bitwise "xor" operator with <see cref="GetAllFlags{TEnum}()"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns><paramref name="value"/> with all of it's flags toggled.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum ToggleFlags<TEnum>(TEnum value)
        {
            TEnum result = default;
            UnsafeEnums.GetCache<TEnum>().ToggleFlags(ref UnsafeUtility.As<TEnum, byte>(ref value), ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Returns <paramref name="value"/> while toggling the flags that are in <paramref name="otherFlags"/>. Equivalent to the bitwise "xor" operator.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns><paramref name="value"/> while toggling the flags that are in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum ToggleFlags<TEnum>(TEnum value, TEnum otherFlags)
        {
            TEnum result = default;
            UnsafeEnums.GetCache<TEnum>().ToggleFlags(ref UnsafeUtility.As<TEnum, byte>(ref value), ref UnsafeUtility.As<TEnum, byte>(ref otherFlags), ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Returns <paramref name="value"/> with only the flags that are also in <paramref name="otherFlags"/>. Equivalent to the bitwise "and" operation.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns><paramref name="value"/> with only the flags that are also in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum CommonFlags<TEnum>(TEnum value, TEnum otherFlags)
        {
            TEnum result = default;
            UnsafeEnums.GetCache<TEnum>().CommonFlags(ref UnsafeUtility.As<TEnum, byte>(ref value), ref UnsafeUtility.As<TEnum, byte>(ref otherFlags), ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Combines the flags of <paramref name="value"/> and <paramref name="otherFlags"/>. Equivalent to the bitwise "or" operation.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>Combination of <paramref name="value"/> with the flags in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum CombineFlags<TEnum>(TEnum value, TEnum otherFlags)
        {
            TEnum result = default;
            UnsafeEnums.GetCache<TEnum>().CombineFlags(ref UnsafeUtility.As<TEnum, byte>(ref value), ref UnsafeUtility.As<TEnum, byte>(ref otherFlags), ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Combines the flags of <paramref name="flag0"/>, <paramref name="flag1"/>, and <paramref name="flag2"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="flag0">The first flags enum value.</param>
        /// <param name="flag1">The second flags enum value.</param>
        /// <param name="flag2">The third flags enum value.</param>
        /// <returns>Combination of the flags of <paramref name="flag0"/>, <paramref name="flag1"/>, and <paramref name="flag2"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2)
        {
            TEnum result = default;
            UnsafeEnums.GetCache<TEnum>().CombineFlags(ref UnsafeUtility.As<TEnum, byte>(ref flag0), ref UnsafeUtility.As<TEnum, byte>(ref flag1), ref UnsafeUtility.As<TEnum, byte>(ref flag2), ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Combines the flags of <paramref name="flag0"/>, <paramref name="flag1"/>, <paramref name="flag2"/>, and <paramref name="flag3"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="flag0">The first flags enum value.</param>
        /// <param name="flag1">The second flags enum value.</param>
        /// <param name="flag2">The third flags enum value.</param>
        /// <param name="flag3">The fourth flags enum value.</param>
        /// <returns>Combination of the flags of <paramref name="flag0"/>, <paramref name="flag1"/>, <paramref name="flag2"/>, and <paramref name="flag3"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3)
        {
            TEnum result = default;
            UnsafeEnums.GetCache<TEnum>().CombineFlags(ref UnsafeUtility.As<TEnum, byte>(ref flag0), ref UnsafeUtility.As<TEnum, byte>(ref flag1), ref UnsafeUtility.As<TEnum, byte>(ref flag2), ref UnsafeUtility.As<TEnum, byte>(ref flag3), ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Combines the flags of <paramref name="flag0"/>, <paramref name="flag1"/>, <paramref name="flag2"/>, <paramref name="flag3"/>, and <paramref name="flag4"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="flag0">The first flags enum value.</param>
        /// <param name="flag1">The second flags enum value.</param>
        /// <param name="flag2">The third flags enum value.</param>
        /// <param name="flag3">The fourth flags enum value.</param>
        /// <param name="flag4">The fifth flags enum value.</param>
        /// <returns>Combination of the flags of <paramref name="flag0"/>, <paramref name="flag1"/>, <paramref name="flag2"/>, <paramref name="flag3"/>, and <paramref name="flag4"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4)
        {
            TEnum result = default;
            UnsafeEnums.GetCache<TEnum>().CombineFlags(ref UnsafeUtility.As<TEnum, byte>(ref flag0), ref UnsafeUtility.As<TEnum, byte>(ref flag1), ref UnsafeUtility.As<TEnum, byte>(ref flag2), ref UnsafeUtility.As<TEnum, byte>(ref flag3), ref UnsafeUtility.As<TEnum, byte>(ref flag4), ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Combines all of the flags of <paramref name="flags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="flags">The flags enum values.</param>
        /// <returns>Combination of all of the flags of <paramref name="flags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum CombineFlags<TEnum>(params TEnum[] flags) => UnsafeEnums.GetBridge<TEnum>().CombineFlags(flags);

        /// <summary>
        /// Combines all of the flags of <paramref name="flags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="flags">The flags enum values.</param>
        /// <returns>Combination of all of the flags of <paramref name="flags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum CombineFlags<TEnum>(IEnumerable<TEnum> flags) => UnsafeEnums.GetBridge<TEnum>().CombineFlags(flags);

        /// <summary>
        /// Returns <paramref name="value"/> without the flags specified in <paramref name="otherFlags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns><paramref name="value"/> without the flags specified in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum RemoveFlags<TEnum>(TEnum value, TEnum otherFlags)
        {
            TEnum result = default;
            UnsafeEnums.GetCache<TEnum>().RemoveFlags(ref UnsafeUtility.As<TEnum, byte>(ref value), ref UnsafeUtility.As<TEnum, byte>(ref otherFlags), ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }
        #endregion

        #region Parsing
        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <typeparamref name="TEnum"/>'s underlying type.</exception>
        public static TEnum ParseFlags<TEnum>(string value) => ParseFlags<TEnum>(value, value, false, null, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, EnumFormat format) => ParseFlags<TEnum>(value, false, null, format);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, EnumFormat format0, EnumFormat format1) => ParseFlags<TEnum>(value, false, null, format0, format1);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags<TEnum>(value, false, null, format0, format1, format2);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, params EnumFormat[] formats) => ParseFlags<TEnum>(value, false, null, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase) => ParseFlags<TEnum>(value, value, ignoreCase, null, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, EnumFormat format) => ParseFlags<TEnum>(value, ignoreCase, null, format);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => ParseFlags<TEnum>(value, ignoreCase, null, format0, format1);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags<TEnum>(value, ignoreCase, null, format0, format1, format2);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, params EnumFormat[] formats) => ParseFlags<TEnum>(value, ignoreCase, null, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <typeparamref name="TEnum"/>'s underlying type.</exception>
        public static TEnum ParseFlags<TEnum>(string value, string delimiter) => ParseFlags<TEnum>(value, value, false, delimiter, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, string delimiter, EnumFormat format) => ParseFlags<TEnum>(value, false, delimiter, format);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, string delimiter, EnumFormat format0, EnumFormat format1) => ParseFlags<TEnum>(value, false, delimiter, format0, format1);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, string delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags<TEnum>(value, false, delimiter, format0, format1, format2);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, string delimiter, params EnumFormat[] formats) => ParseFlags<TEnum>(value, false, delimiter, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, string delimiter) => ParseFlags<TEnum>(value, value, ignoreCase, delimiter, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, string delimiter, EnumFormat format) => ParseFlags<TEnum>(value, value, ignoreCase, delimiter, new ValueCollection<EnumFormat>(format));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, string delimiter, EnumFormat format0, EnumFormat format1) => ParseFlags<TEnum>(value, value, ignoreCase, delimiter, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, string delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags<TEnum>(value, value, ignoreCase, delimiter, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, string delimiter, params EnumFormat[] formats) => ParseFlags<TEnum>(value, value, ignoreCase, delimiter, new ValueCollection<EnumFormat>(formats));

#if SPAN
        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <typeparamref name="TEnum"/>'s underlying type.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value) => ParseFlags<TEnum>(value, string.Empty, false, null, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, EnumFormat format) => ParseFlags<TEnum>(value, false, null, format);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, EnumFormat format0, EnumFormat format1) => ParseFlags<TEnum>(value, false, null, format0, format1);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags<TEnum>(value, false, null, format0, format1, format2);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, params EnumFormat[] formats) => ParseFlags<TEnum>(value, false, null, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase) => ParseFlags<TEnum>(value, string.Empty, ignoreCase, null, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format) => ParseFlags<TEnum>(value, ignoreCase, null, format);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => ParseFlags<TEnum>(value, ignoreCase, null, format0, format1);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags<TEnum>(value, ignoreCase, null, format0, format1, format2);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, params EnumFormat[] formats) => ParseFlags<TEnum>(value, ignoreCase, null, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <typeparamref name="TEnum"/>'s underlying type.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, string delimiter) => ParseFlags<TEnum>(value, string.Empty, false, delimiter, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, string delimiter, EnumFormat format) => ParseFlags<TEnum>(value, false, delimiter, format);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, string delimiter, EnumFormat format0, EnumFormat format1) => ParseFlags<TEnum>(value, false, delimiter, format0, format1);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, string delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags<TEnum>(value, false, delimiter, format0, format1, format2);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, string delimiter, params EnumFormat[] formats) => ParseFlags<TEnum>(value, false, delimiter, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, string delimiter) => ParseFlags<TEnum>(value, string.Empty, ignoreCase, delimiter, default);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, string delimiter, EnumFormat format) => ParseFlags<TEnum>(value, string.Empty, ignoreCase, delimiter, new ValueCollection<EnumFormat>(format));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, string delimiter, EnumFormat format0, EnumFormat format1) => ParseFlags<TEnum>(value, string.Empty, ignoreCase, delimiter, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, string delimiter, EnumFormat format0, EnumFormat format1, EnumFormat format2) => ParseFlags<TEnum>(value, string.Empty, ignoreCase, delimiter, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, string delimiter, params EnumFormat[] formats) => ParseFlags<TEnum>(value, string.Empty, ignoreCase, delimiter, new ValueCollection<EnumFormat>(formats));
#endif

        private static TEnum ParseFlags<TEnum>(ParseType value, string strValue, bool ignoreCase, string delimiter, ValueCollection<EnumFormat> formats)
        {
            Preconditions.NotNull(strValue, nameof(value));

            TEnum result = default;
            UnsafeEnums.GetCache<TEnum>().ParseFlags(value, ignoreCase, delimiter, formats, ref UnsafeUtility.As<TEnum, byte>(ref result));
            return result;
        }

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParseFlags<TEnum>(string value, out TEnum result) => TryParseFlags(value, false, null, out result, default(ValueCollection<EnumFormat>));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, out TEnum result, EnumFormat format) => TryParseFlags(value, false, null, out result, format);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, out TEnum result, EnumFormat format0, EnumFormat format1) => TryParseFlags(value, false, null, out result, format0, format1);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(value, false, null, out result, format0, format1, format2);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, out TEnum result, params EnumFormat[] formats) => TryParseFlags(value, false, null, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, out TEnum result) => TryParseFlags(value, ignoreCase, null, out result, default(ValueCollection<EnumFormat>));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, out TEnum result, EnumFormat format) => TryParseFlags(value, ignoreCase, null, out result, format);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1) => TryParseFlags(value, ignoreCase, null, out result, format0, format1);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(value, ignoreCase, null, out result, format0, format1, format2);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, out TEnum result, params EnumFormat[] formats) => TryParseFlags(value, ignoreCase, null, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParseFlags<TEnum>(string value, string delimiter, out TEnum result) => TryParseFlags(value, false, delimiter, out result, default(ValueCollection<EnumFormat>));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, string delimiter, out TEnum result, EnumFormat format) => TryParseFlags(value, false, delimiter, out result, format);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, string delimiter, out TEnum result, EnumFormat format0, EnumFormat format1) => TryParseFlags(value, false, delimiter, out result, format0, format1);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, string delimiter, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(value, false, delimiter, out result, format0, format1, format2);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, string delimiter, out TEnum result, params EnumFormat[] formats) => TryParseFlags(value, false, delimiter, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result) => TryParseFlags(value, ignoreCase, delimiter, out result);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat format) => TryParseFlags(value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(format));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat format0, EnumFormat format1) => TryParseFlags(value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result, params EnumFormat[] formats) => TryParseFlags(value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(formats));

#if SPAN
        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, out TEnum result) => TryParseFlags(value, false, null, out result, default(ValueCollection<EnumFormat>));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, out TEnum result, EnumFormat format) => TryParseFlags(value, false, null, out result, format);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, out TEnum result, EnumFormat format0, EnumFormat format1) => TryParseFlags(value, false, null, out result, format0, format1);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(value, false, null, out result, format0, format1, format2);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, out TEnum result, params EnumFormat[] formats) => TryParseFlags(value, false, null, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result) => TryParseFlags(value, ignoreCase, null, out result, default(ValueCollection<EnumFormat>));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result, EnumFormat format) => TryParseFlags(value, ignoreCase, null, out result, format);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1) => TryParseFlags(value, ignoreCase, null, out result, format0, format1);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(value, ignoreCase, null, out result, format0, format1, format2);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result, params EnumFormat[] formats) => TryParseFlags(value, ignoreCase, null, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, string delimiter, out TEnum result) => TryParseFlags(value, false, delimiter, out result, default(ValueCollection<EnumFormat>));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, string delimiter, out TEnum result, EnumFormat format) => TryParseFlags(value, false, delimiter, out result, format);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, string delimiter, out TEnum result, EnumFormat format0, EnumFormat format1) => TryParseFlags(value, false, delimiter, out result, format0, format1);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, string delimiter, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(value, false, delimiter, out result, format0, format1, format2);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, string delimiter, out TEnum result, params EnumFormat[] formats) => TryParseFlags(value, false, delimiter, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, string delimiter, out TEnum result) => TryParseFlags(value, ignoreCase, delimiter, out result);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat format) => TryParseFlags(value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(format));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat format0, EnumFormat format1) => TryParseFlags(value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParseFlags(value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, string delimiter, out TEnum result, params EnumFormat[] formats) => TryParseFlags(value, ignoreCase, delimiter, out result, new ValueCollection<EnumFormat>(formats));
#endif

        private static bool TryParseFlags<TEnum>(ParseType value, bool ignoreCase, string delimiter, out TEnum result, ValueCollection<EnumFormat> formats)
        {
            result = default;
            return UnsafeEnums.GetCache<TEnum>().TryParseFlags(value, ignoreCase, delimiter, ref UnsafeUtility.As<TEnum, byte>(ref result), formats);
        }
        #endregion
    }
}