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

namespace EnumsNET
{
    /// <summary>
    /// Static class that provides efficient type-safe flag enum operations through the use of cached enum names, values, and attributes.
    /// Many operations are exposed as C# extension methods for convenience.
    /// </summary>
    public static class FlagEnums
    {
        internal const string DefaultDelimiter = ", ";

        #region "Properties"
        /// <summary>
        /// Indicates if <typeparamref name="TEnum"/> is marked with the <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>Indication if <typeparamref name="TEnum"/> is marked with the <see cref="FlagsAttribute"/>.</returns>
        public static bool IsFlagEnum<[EnumConstraint] TEnum>()
            where TEnum : struct => Enums<TEnum>.Info.IsFlagEnum;

        /// <summary>
        /// Retrieves all the flags defined by <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>All the flags defined by <typeparamref name="TEnum"/>.</returns>
        public static TEnum GetAllFlags<[EnumConstraint] TEnum>()
            where TEnum : struct => Enums<TEnum>.Info.AllFlags;
        #endregion

        #region Main Methods
        /// <summary>
        /// Indicates whether <paramref name="value"/> is a valid flag combination of <typeparamref name="TEnum"/>'s defined flags.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>Indication of whether <paramref name="value"/> is a valid flag combination of <typeparamref name="TEnum"/>'s defined flags.</returns>
        public static bool IsValidFlagCombination<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.IsValidFlagCombination(value);

        /// <summary>
        /// Retrieves the names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        public static string FormatFlags<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.FormatFlags(value);

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
        /// <exception cref="ArgumentException"><paramref name="formats"/> contains an invalid value.</exception>
        public static string FormatFlags<[EnumConstraint] TEnum>(TEnum value, params EnumFormat[] formats)
            where TEnum : struct => Enums<TEnum>.Info.FormatFlags(value, null, formats);

        /// <summary>
        /// Retrieves the names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="delimiter">The delimiter to use to separate individual flags.</param>
        /// <returns>The names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination <c>null</c> is returned.</returns>
        public static string FormatFlags<[EnumConstraint] TEnum>(TEnum value, string delimiter)
            where TEnum : struct => Enums<TEnum>.Info.FormatFlags(value, delimiter);

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
        /// <exception cref="ArgumentException"><paramref name="formats"/> contains an invalid value.</exception>
        public static string FormatFlags<[EnumConstraint] TEnum>(TEnum value, string delimiter, params EnumFormat[] formats)
            where TEnum : struct => Enums<TEnum>.Info.FormatFlags(value, delimiter, formats);

        /// <summary>
        /// Retrieves the flags that compose <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The flags that compose <paramref name="value"/>.</returns>
        public static IEnumerable<TEnum> GetFlags<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.GetFlags(value);

        /// <summary>
        /// Retrieves the <see cref="EnumMember{TEnum}"/>s of the flags that compose <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>The <see cref="EnumMember{TEnum}"/>s of the flags that compose <paramref name="value"/>.</returns>
        public static IEnumerable<EnumMember<TEnum>> GetFlagMembers<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.GetFlagMembers(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> has any flags.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has any flags.</returns>
        public static bool HasAnyFlags<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.HasAnyFlags(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> has any flags that are in <paramref name="otherFlags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has any flags that are in <paramref name="otherFlags"/>.</returns>
        public static bool HasAnyFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum otherFlags)
            where TEnum : struct => Enums<TEnum>.Info.HasAnyFlags(value, otherFlags);

        /// <summary>
        /// Indicates if <paramref name="value"/> has all of the flags that are defined in <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has all of the flags that are defined in <typeparamref name="TEnum"/>.</returns>
        public static bool HasAllFlags<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.HasAllFlags(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> has all of the flags that are in <paramref name="otherFlags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>Indication if <paramref name="value"/> has all of the flags that are in <paramref name="otherFlags"/>.</returns>
        public static bool HasAllFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum otherFlags)
            where TEnum : struct => Enums<TEnum>.Info.HasAllFlags(value, otherFlags);

        /// <summary>
        /// Returns <paramref name="value"/> with all of it's flags toggled. Equivalent to the bitwise "xor" operator with <see cref="GetAllFlags{TEnum}()"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <returns><paramref name="value"/> with all of it's flags toggled.</returns>
        public static TEnum ToggleFlags<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.ToggleFlags(value);

        /// <summary>
        /// Returns <paramref name="value"/> while toggling the flags that are in <paramref name="otherFlags"/>. Equivalent to the bitwise "xor" operator.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns><paramref name="value"/> while toggling the flags that are in <paramref name="otherFlags"/>.</returns>
        public static TEnum ToggleFlags<[EnumConstraint] TEnum>(TEnum value, TEnum otherFlags)
            where TEnum : struct => Enums<TEnum>.Info.ToggleFlags(value, otherFlags);

        /// <summary>
        /// Returns <paramref name="value"/> with only the flags that are also in <paramref name="otherFlags"/>. Equivalent to the bitwise "and" operation.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns><paramref name="value"/> with only the flags that are also in <paramref name="otherFlags"/>.</returns>
        public static TEnum CommonFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum otherFlags)
            where TEnum : struct => Enums<TEnum>.Info.CommonFlags(value, otherFlags);

        /// <summary>
        /// Combines the flags of <paramref name="value"/> and <paramref name="otherFlags"/>. Equivalent to the bitwise "or" operation.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns>Combination of <paramref name="value"/> with the flags in <paramref name="otherFlags"/>.</returns>
        public static TEnum CombineFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum otherFlags)
            where TEnum : struct => Enums<TEnum>.Info.CombineFlags(value, otherFlags);

        /// <summary>
        /// Combines the flags of <paramref name="flag0"/>, <paramref name="flag1"/>, and <paramref name="flag2"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="flag0">The first flags enum value.</param>
        /// <param name="flag1">The second flags enum value.</param>
        /// <param name="flag2">The third flags enum value.</param>
        /// <returns>Combination of the flags of <paramref name="flag0"/>, <paramref name="flag1"/>, and <paramref name="flag2"/>.</returns>
        public static TEnum CombineFlags<[EnumConstraint] TEnum>(TEnum flag0, TEnum flag1, TEnum flag2)
            where TEnum : struct => Enums<TEnum>.Info.CombineFlags(flag0, flag1, flag2);

        /// <summary>
        /// Combines the flags of <paramref name="flag0"/>, <paramref name="flag1"/>, <paramref name="flag2"/>, and <paramref name="flag3"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="flag0">The first flags enum value.</param>
        /// <param name="flag1">The second flags enum value.</param>
        /// <param name="flag2">The third flags enum value.</param>
        /// <param name="flag3">The fourth flags enum value.</param>
        /// <returns>Combination of the flags of <paramref name="flag0"/>, <paramref name="flag1"/>, <paramref name="flag2"/>, and <paramref name="flag3"/>.</returns>
        public static TEnum CombineFlags<[EnumConstraint] TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3)
            where TEnum : struct => Enums<TEnum>.Info.CombineFlags(flag0, flag1, flag2, flag3);

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
        public static TEnum CombineFlags<[EnumConstraint] TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4)
            where TEnum : struct => Enums<TEnum>.Info.CombineFlags(flag0, flag1, flag2, flag3, flag4);

        /// <summary>
        /// Combines all of the flags of <paramref name="flags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="flags">The flags enum values.</param>
        /// <returns>Combination of all of the flags of <paramref name="flags"/>.</returns>
        public static TEnum CombineFlags<[EnumConstraint] TEnum>(params TEnum[] flags)
            where TEnum : struct => Enums<TEnum>.Info.CombineFlags(flags);

        /// <summary>
        /// Returns <paramref name="value"/> without the flags specified in <paramref name="otherFlags"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The flags enum value.</param>
        /// <param name="otherFlags">The other flags enum value.</param>
        /// <returns><paramref name="value"/> without the flags specified in <paramref name="otherFlags"/>.</returns>
        public static TEnum RemoveFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum otherFlags)
            where TEnum : struct => Enums<TEnum>.Info.RemoveFlags(value, otherFlags);
        #endregion

        #region Parsing
        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <typeparamref name="TEnum"/>'s underlying type.</exception>
        public static TEnum ParseFlags<[EnumConstraint] TEnum>(string value)
            where TEnum : struct => Enums<TEnum>.Info.ParseFlags(value);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<[EnumConstraint] TEnum>(string value, params EnumFormat[] formats)
            where TEnum : struct => Enums<TEnum>.Info.ParseFlags(value, false, null, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<[EnumConstraint] TEnum>(string value, bool ignoreCase)
            where TEnum : struct => Enums<TEnum>.Info.ParseFlags(value, ignoreCase);

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
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<[EnumConstraint] TEnum>(string value, bool ignoreCase, params EnumFormat[] formats)
            where TEnum : struct => Enums<TEnum>.Info.ParseFlags(value, ignoreCase, null, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <typeparamref name="TEnum"/>'s underlying type.</exception>
        public static TEnum ParseFlags<[EnumConstraint] TEnum>(string value, string delimiter)
            where TEnum : struct => Enums<TEnum>.Info.ParseFlags(value, false, delimiter);

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
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<[EnumConstraint] TEnum>(string value, string delimiter, params EnumFormat[] formats)
            where TEnum : struct => Enums<TEnum>.Info.ParseFlags(value, false, delimiter, formats);

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
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter)
            where TEnum : struct => Enums<TEnum>.Info.ParseFlags(value, ignoreCase, delimiter);

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
        /// <exception cref="ArgumentException"><paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum ParseFlags<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter, params EnumFormat[] formats)
            where TEnum : struct => Enums<TEnum>.Info.ParseFlags(value, ignoreCase, delimiter, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        public static bool TryParseFlags<[EnumConstraint] TEnum>(string value, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryParseFlags(value, false, null, out result);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags<[EnumConstraint] TEnum>(string value, out TEnum result, params EnumFormat[] formats)
            where TEnum : struct => Enums<TEnum>.Info.TryParseFlags(value, false, null, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        public static bool TryParseFlags<[EnumConstraint] TEnum>(string value, bool ignoreCase, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryParseFlags(value, ignoreCase, null, out result);

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
        /// <exception cref="ArgumentException"><paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags<[EnumConstraint] TEnum>(string value, bool ignoreCase, out TEnum result, params EnumFormat[] formats)
            where TEnum : struct => Enums<TEnum>.Info.TryParseFlags(value, ignoreCase, null, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> delimited with <paramref name="delimiter"/> to its respective <typeparamref name="TEnum"/> value.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="delimiter">The delimiter used to separate individual flags.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        public static bool TryParseFlags<[EnumConstraint] TEnum>(string value, string delimiter, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryParseFlags(value, false, delimiter, out result);

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
        /// <exception cref="ArgumentException"><paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags<[EnumConstraint] TEnum>(string value, string delimiter, out TEnum result, params EnumFormat[] formats)
            where TEnum : struct => Enums<TEnum>.Info.TryParseFlags(value, false, delimiter, out result, formats);

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
        public static bool TryParseFlags<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryParseFlags(value, ignoreCase, delimiter, out result);

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
        /// <exception cref="ArgumentException"><paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParseFlags<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result, params EnumFormat[] formats)
            where TEnum : struct => Enums<TEnum>.Info.TryParseFlags(value, ignoreCase, delimiter, out result, formats);
        #endregion

        #region EnumMember Extensions
        /// <summary>
        /// Indicates whether <paramref name="member"/>'s value is a valid flag combination of its enum's defined values.
        /// </summary>
        /// <param name="member">The enum member.</param>
        /// <returns>Indication of whether <paramref name="member"/>'s value is a valid flag combination of its enum's defined values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <c>null</c>.</exception>
        public static bool IsValidFlagCombination(this EnumMember member)
        {
            Preconditions.NotNull(member, nameof(member));
            return member.IsValidFlagCombination();
        }

        /// <summary>
        /// Retrieves the flags that compose <paramref name="member"/>'s value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="member">The enum member.</param>
        /// <returns>The flags that compose <paramref name="member"/>'s value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <c>null</c>.</exception>
        public static IEnumerable<TEnum> GetFlags<TEnum>(this EnumMember<TEnum> member)
        {
            Preconditions.NotNull(member, nameof(member));
            return member.GetGenericFlags();
        }

        /// <summary>
        /// Retrieves the <see cref="EnumMember{TEnum}"/>s of the flags that compose <paramref name="member"/>'s value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="member">The enum member.</param>
        /// <returns>The <see cref="EnumMember{TEnum}"/>s of the flags that compose <paramref name="member"/>'s value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <c>null</c>.</exception>
        public static IEnumerable<EnumMember<TEnum>> GetFlagMembers<TEnum>(this EnumMember<TEnum> member)
        {
            Preconditions.NotNull(member, nameof(member));
            return member.GetGenericFlagMembers();
        }

        /// <summary>
        /// Indicates if <paramref name="member"/>'s value has any flags.
        /// </summary>
        /// <param name="member">The enum member.</param>
        /// <returns>Indication if <paramref name="member"/>'s has any flags.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <c>null</c>.</exception>
        public static bool HasAnyFlags(this EnumMember member)
        {
            Preconditions.NotNull(member, nameof(member));
            return member.HasAnyFlags();
        }

        /// <summary>
        /// Indicates if <paramref name="member"/>'s value has all of the flags that are defined in its enum type.
        /// </summary>
        /// <param name="member">The enum member.</param>
        /// <returns>Indication if <paramref name="member"/> has all of the flags that are defined in its enum type.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <c>null</c>.</exception>
        public static bool HasAllFlags(this EnumMember member)
        {
            Preconditions.NotNull(member, nameof(member));
            return member.HasAllFlags();
        }
        #endregion
    }
}
