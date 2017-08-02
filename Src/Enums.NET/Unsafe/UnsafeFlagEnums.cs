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
        public static bool IsFlagEnum<TEnum>() => UnsafeEnums.GetInfo<TEnum>().IsFlagEnum;

        /// <summary>
        /// Retrieves all the flags defined by <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>All the flags defined by <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum GetAllFlags<TEnum>() => UnsafeEnums.GetInfo<TEnum>().AllFlags;
        #endregion

        #region Main Methods
        /// <summary>
        /// Indicates whether <paramref name="value"/> is a valid flag combination of <typeparamref name="TEnum"/>'s defined flags.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>Indication of whether <paramref name="value"/> is a valid flag combination of <typeparamref name="TEnum"/>'s defined flags.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool IsValidFlagCombination<TEnum>(TEnum value) => UnsafeEnums.GetInfo<TEnum>().IsValidFlagCombination(value);

        /// <summary>
        /// Retrieves the names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static string FormatFlags<TEnum>(TEnum value) => UnsafeEnums.GetInfo<TEnum>().FormatFlags(value);

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
        public static string FormatFlags<TEnum>(TEnum value, params EnumFormat[] formats) => UnsafeEnums.GetInfo<TEnum>().FormatFlags(value, null, formats);

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
        public static string FormatFlags<TEnum>(TEnum value, string delimiter) => UnsafeEnums.GetInfo<TEnum>().FormatFlags(value, delimiter);

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
        public static string FormatFlags<TEnum>(TEnum value, string delimiter, params EnumFormat[] formats) => UnsafeEnums.GetInfo<TEnum>().FormatFlags(value, delimiter, formats);

        /// <summary>
        /// Retrieves the flags that compose <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The flags that compose <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static IEnumerable<TEnum> GetFlags<TEnum>(TEnum value) => UnsafeEnums.GetInfo<TEnum>().GetFlags(value);

        /// <summary>
        /// Retrieves the <see cref="EnumMember{TEnum}"/>s of the flags that compose <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The <see cref="EnumMember{TEnum}"/>s of the flags that compose <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static IEnumerable<EnumMember<TEnum>> GetFlagMembers<TEnum>(TEnum value) => UnsafeEnums.GetInfo<TEnum>().GetFlagMembers(value);

        /// <summary>
        /// Retrieves the flag count of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>The flag count of <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static int GetFlagCount<TEnum>() => UnsafeEnums.GetInfo<TEnum>().GetFlagCount();

        /// <summary>
        /// Retrieves the flag count of <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The flag count of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static int GetFlagCount<TEnum>(TEnum value) => UnsafeEnums.GetInfo<TEnum>().GetFlagCount(value);

        /// <summary>
        /// Retrieves the flag count of <paramref name="otherFlags"/> that <paramref name="value"/> has.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>The flag count of <paramref name="otherFlags"/> that <paramref name="value"/> has.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static int GetFlagCount<TEnum>(TEnum value, TEnum otherFlags) => UnsafeEnums.GetInfo<TEnum>().GetFlagCount(value, otherFlags);

        /// <summary>
        /// Indicates if <paramref name="value"/> has any flags.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has any flags.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool HasAnyFlags<TEnum>(TEnum value) => UnsafeEnums.GetInfo<TEnum>().HasAnyFlags(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> has any flags that are in <paramref name="otherFlags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has any flags that are in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool HasAnyFlags<TEnum>(TEnum value, TEnum otherFlags) => UnsafeEnums.GetInfo<TEnum>().HasAnyFlags(value, otherFlags);

        /// <summary>
        /// Indicates if <paramref name="value"/> has all of the flags that are defined in <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has all of the flags that are defined in <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool HasAllFlags<TEnum>(TEnum value) => UnsafeEnums.GetInfo<TEnum>().HasAllFlags(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> has all of the flags that are in <paramref name="otherFlags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has all of the flags that are in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool HasAllFlags<TEnum>(TEnum value, TEnum otherFlags) => UnsafeEnums.GetInfo<TEnum>().HasAllFlags(value, otherFlags);

        /// <summary>
        /// Returns <paramref name="value"/> with all of it's flags toggled. Equivalent to the bitwise "xor" operator with <see cref="GetAllFlags{TEnum}()"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns><paramref name="value"/> with all of it's flags toggled.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum ToggleFlags<TEnum>(TEnum value) => UnsafeEnums.GetInfo<TEnum>().ToggleFlags(value);

        /// <summary>
        /// Returns <paramref name="value"/> while toggling the flags that are in <paramref name="otherFlags"/>. Equivalent to the bitwise "xor" operator.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns><paramref name="value"/> while toggling the flags that are in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum ToggleFlags<TEnum>(TEnum value, TEnum otherFlags) => UnsafeEnums.GetInfo<TEnum>().ToggleFlags(value, otherFlags);

        /// <summary>
        /// Returns <paramref name="value"/> with only the flags that are also in <paramref name="otherFlags"/>. Equivalent to the bitwise "and" operation.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns><paramref name="value"/> with only the flags that are also in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum CommonFlags<TEnum>(TEnum value, TEnum otherFlags) => UnsafeEnums.GetInfo<TEnum>().CommonFlags(value, otherFlags);

        /// <summary>
        /// Combines the flags of <paramref name="value"/> and <paramref name="otherFlags"/>. Equivalent to the bitwise "or" operation.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>Combination of <paramref name="value"/> with the flags in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum CombineFlags<TEnum>(TEnum value, TEnum otherFlags) => UnsafeEnums.GetInfo<TEnum>().CombineFlags(value, otherFlags);

        /// <summary>
        /// Combines the flags of <paramref name="flag0"/>, <paramref name="flag1"/>, and <paramref name="flag2"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="flag0">The first flags enum value.</param>
        /// <param name="flag1">The second flags enum value.</param>
        /// <param name="flag2">The third flags enum value.</param>
        /// <returns>Combination of the flags of <paramref name="flag0"/>, <paramref name="flag1"/>, and <paramref name="flag2"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2) => UnsafeEnums.GetInfo<TEnum>().CombineFlags(flag0, flag1, flag2);

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
        public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3) => UnsafeEnums.GetInfo<TEnum>().CombineFlags(flag0, flag1, flag2, flag3);

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
        public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4) => UnsafeEnums.GetInfo<TEnum>().CombineFlags(flag0, flag1, flag2, flag3, flag4);

        /// <summary>
        /// Combines all of the flags of <paramref name="flags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="flags">The flags enum values.</param>
        /// <returns>Combination of all of the flags of <paramref name="flags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum CombineFlags<TEnum>(params TEnum[] flags) => UnsafeEnums.GetInfo<TEnum>().CombineFlags(flags);

        /// <summary>
        /// Combines all of the flags of <paramref name="flags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="flags">The flags enum values.</param>
        /// <returns>Combination of all of the flags of <paramref name="flags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum CombineFlags<TEnum>(IEnumerable<TEnum> flags) => UnsafeEnums.GetInfo<TEnum>().CombineFlags(flags);

        /// <summary>
        /// Returns <paramref name="value"/> without the flags specified in <paramref name="otherFlags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns><paramref name="value"/> without the flags specified in <paramref name="otherFlags"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TEnum RemoveFlags<TEnum>(TEnum value, TEnum otherFlags) => UnsafeEnums.GetInfo<TEnum>().RemoveFlags(value, otherFlags);
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
        public static TEnum ParseFlags<TEnum>(string value) => UnsafeEnums.GetInfo<TEnum>().ParseFlags(value);

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
        public static TEnum ParseFlags<TEnum>(string value, params EnumFormat[] formats) => UnsafeEnums.GetInfo<TEnum>().ParseFlags(value, false, null, formats);

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
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase) => UnsafeEnums.GetInfo<TEnum>().ParseFlags(value, ignoreCase);

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
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, params EnumFormat[] formats) => UnsafeEnums.GetInfo<TEnum>().ParseFlags(value, ignoreCase, null, formats);

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
        public static TEnum ParseFlags<TEnum>(string value, string delimiter) => UnsafeEnums.GetInfo<TEnum>().ParseFlags(value, false, delimiter);

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
        public static TEnum ParseFlags<TEnum>(string value, string delimiter, params EnumFormat[] formats) => UnsafeEnums.GetInfo<TEnum>().ParseFlags(value, false, delimiter, formats);

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
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, string delimiter) => UnsafeEnums.GetInfo<TEnum>().ParseFlags(value, ignoreCase, delimiter);

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
        public static TEnum ParseFlags<TEnum>(string value, bool ignoreCase, string delimiter, params EnumFormat[] formats) => UnsafeEnums.GetInfo<TEnum>().ParseFlags(value, ignoreCase, delimiter, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParseFlags<TEnum>(string value, out TEnum result) => UnsafeEnums.GetInfo<TEnum>().TryParseFlags(value, false, null, out result);

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
        public static bool TryParseFlags<TEnum>(string value, out TEnum result, params EnumFormat[] formats) => UnsafeEnums.GetInfo<TEnum>().TryParseFlags(value, false, null, out result, formats);

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
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, out TEnum result) => UnsafeEnums.GetInfo<TEnum>().TryParseFlags(value, ignoreCase, null, out result);

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
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, out TEnum result, params EnumFormat[] formats) => UnsafeEnums.GetInfo<TEnum>().TryParseFlags(value, ignoreCase, null, out result, formats);

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
        public static bool TryParseFlags<TEnum>(string value, string delimiter, out TEnum result) => UnsafeEnums.GetInfo<TEnum>().TryParseFlags(value, false, delimiter, out result);

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
        public static bool TryParseFlags<TEnum>(string value, string delimiter, out TEnum result, params EnumFormat[] formats) => UnsafeEnums.GetInfo<TEnum>().TryParseFlags(value, false, delimiter, out result, formats);

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
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result) => UnsafeEnums.GetInfo<TEnum>().TryParseFlags(value, ignoreCase, delimiter, out result);

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
        public static bool TryParseFlags<TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result, params EnumFormat[] formats) => UnsafeEnums.GetInfo<TEnum>().TryParseFlags(value, ignoreCase, delimiter, out result, formats);
        #endregion
    }
}
