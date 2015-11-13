// Enums.NET
// Copyright 2015 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Diagnostics.Contracts;

using ExtraConstraints;

namespace EnumsNET
{
	/// <summary>
	/// Static class that provides efficient type-safe flag enum operations through the use of cached enum names, values, and attributes.
	/// Many operations are exposed as extension methods for convenience.
	/// </summary>
	public static class FlagEnums
	{
		internal const string DefaultDelimiter = ", ";

		#region "Properties"
		/// <summary>
		/// Indicates if <typeparamref name="TEnum"/> is marked with the <see cref="FlagsAttribute"/>.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <returns>Indication if <typeparamref name="TEnum"/> is marked with the <see cref="FlagsAttribute"/>.</returns>
		[Pure]
		public static bool IsFlagEnum<[EnumConstraint] TEnum>() => EnumsCache<TEnum>.IsFlagEnum;

		/// <summary>
		/// Retrieves all the flags defined by <typeparamref name="TEnum"/>.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <returns>All the flags defined by <typeparamref name="TEnum"/>.</returns>
		[Pure]
		public static TEnum GetAllFlags<[EnumConstraint] TEnum>() => EnumsCache<TEnum>.AllFlags;
		#endregion

		#region Main Methods
		/// <summary>
		/// Indicates whether <paramref name="value"/> is a valid flag combination of the defined enum values.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <returns>Indication of whether <paramref name="value"/> is a valid flag combination of the defined enum values.</returns>
		[Pure]
		public static bool IsValidFlagCombination<[EnumConstraint] TEnum>(TEnum value) => EnumsCache<TEnum>.IsValidFlagCombination(value);

		/// <summary>
		/// Returns the names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
		/// If <paramref name="value"/> is not a valid flag combination null is returned.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">Should be a valid flag combination.</param>
		/// <returns>The names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
		/// If <paramref name="value"/> is not a valid flag combination null is returned.</returns>
		[Pure]
		public static string FormatAsFlags<[EnumConstraint] TEnum>(this TEnum value) => EnumsCache<TEnum>.FormatAsFlags(value);

		/// <summary>
		/// Returns the names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">Should be a valid flag combination.</param>
		/// <param name="delimiter">The delimiter to use to separate individual flag names. Cannot be null or empty.</param>
		/// <returns>The names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
		/// If <paramref name="value"/> is not a valid flag combination null is returned.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="delimiter"/> is empty.</exception>
		[Pure]
		public static string FormatAsFlags<[EnumConstraint] TEnum>(this TEnum value, string delimiter) => EnumsCache<TEnum>.FormatAsFlags(value, delimiter);

		[Pure]
		public static string FormatAsFlags<[EnumConstraint] TEnum>(this TEnum value, params EnumFormat[] formats) => EnumsCache<TEnum>.FormatAsFlags(value, formats);

		[Pure]
		public static string FormatAsFlags<[EnumConstraint] TEnum>(this TEnum value, string delimiter, params EnumFormat[] formats) => EnumsCache<TEnum>.FormatAsFlags(value, delimiter, formats);

		/// <summary>
		/// Returns an array of the flags that compose <paramref name="value"/>.
		/// If <paramref name="value"/> is not a valid flag combination null is returned.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">Should be a valid flag combination.</param>
		/// <returns>Array of the flags that compose <paramref name="value"/>.
		/// If <paramref name="value"/> is not a valid flag combination null is returned.</returns>
		[Pure]
		public static TEnum[] GetFlags<[EnumConstraint] TEnum>(this TEnum value) => EnumsCache<TEnum>.GetFlags(value);

		/// <summary>
		/// Indicates if <paramref name="value"/> has any flags set.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">Must be a valid flag combination.</param>
		/// <returns>Indication if <paramref name="value"/> has any flags set.</returns>
		/// <exception cref="ArgumentException"><paramref name="value"/> is not a valid flag combination.</exception>
		[Pure]
		public static bool HasAnyFlags<[EnumConstraint] TEnum>(this TEnum value) => EnumsCache<TEnum>.HasAnyFlags(value);

		/// <summary>
		/// Indicates if <paramref name="value"/> has any flags set that are also set in <paramref name="flagMask"/>.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">Must be a valid flag combination.</param>
		/// <param name="flagMask">Must be a valid flag combination.</param>
		/// <returns>Indication if <paramref name="value"/> has any flags set that are also set in <paramref name="flagMask"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
		[Pure]
		public static bool HasAnyFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum flagMask) => EnumsCache<TEnum>.HasAnyFlags(value, flagMask);

		/// <summary>
		/// Indicates if <paramref name="value"/> has all flags set that are defined in <typeparamref name="TEnum"/>.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">Must be a valid flag combination.</param>
		/// <returns>Indication if <paramref name="value"/> has all flags set that are defined in <typeparamref name="TEnum"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="value"/> is not a valid flag combination.</exception>
		[Pure]
		public static bool HasAllFlags<[EnumConstraint] TEnum>(this TEnum value) => EnumsCache<TEnum>.HasAllFlags(value);

		/// <summary>
		/// Indicates if <paramref name="value"/> has all of the flags set that are also set in <paramref name="flagMask"/>.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">Must be a valid flag combination.</param>
		/// <param name="flagMask">Must be a valid flag combination.</param>
		/// <returns>Indication if <paramref name="value"/> has all of the flags set that are also set in <paramref name="flagMask"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
		[Pure]
		public static bool HasAllFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum flagMask) => EnumsCache<TEnum>.HasAllFlags(value, flagMask);

		/// <summary>
		/// Returns <paramref name="value"/> with all of it's flags inverted. Equivalent to the bitwise "xor" operator with <see cref="GetAllFlags{TEnum}()"/>.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">Must be a valid flag combination.</param>
		/// <returns><paramref name="value"/> with all of it's flags inverted.</returns>
		/// <exception cref="ArgumentException"><paramref name="value"/> is not a valid flag combination.</exception>
		[Pure]
		public static TEnum InvertFlags<[EnumConstraint] TEnum>(this TEnum value) => EnumsCache<TEnum>.InvertFlags(value);

		/// <summary>
		/// Returns <paramref name="value"/> while inverting the flags that are set in <paramref name="flagMask"/>. Equivalent to the bitwise "xor" operator.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">Must be a valid flag combination.</param>
		/// <param name="flagMask">Must be a valid flag combination.</param>
		/// <returns><paramref name="value"/> while inverting the flags that are set in <paramref name="flagMask"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
		[Pure]
		public static TEnum InvertFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum flagMask) => EnumsCache<TEnum>.InvertFlags(value, flagMask);

		/// <summary>
		/// Returns <paramref name="value"/> with only the flags that are also set in <paramref name="flagMask"/>. Equivalent to the bitwise "and" operation.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">Must be a valid flag combination.</param>
		/// <param name="flagMask">Must be a valid flag combination.</param>
		/// <returns><paramref name="value"/> with only the flags that are also set in <paramref name="flagMask"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
		[Pure]
		public static TEnum CommonFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum flagMask) => EnumsCache<TEnum>.CommonFlags(value, flagMask);

		/// <summary>
		/// Returns <paramref name="value"/> with the flags specified in <paramref name="flagMask"/> set. Equivalent to the bitwise "or" operation.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">Must be a valid flag combination.</param>
		/// <param name="flagMask">Must be a valid flag combination.</param>
		/// <returns><paramref name="value"/> with the flags specified in <paramref name="flagMask"/> set.</returns>
		/// <exception cref="ArgumentException"><paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
		[Pure]
		public static TEnum SetFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum flagMask) => EnumsCache<TEnum>.SetFlags(value, flagMask);

		/// <summary>
		/// Returns <paramref name="value"/> with the flags specified in <paramref name="flagMask"/> cleared.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">Must be a valid flag combination.</param>
		/// <param name="flagMask">Must be a valid flag combination.</param>
		/// <returns><paramref name="value"/> with the flags specified in <paramref name="flagMask"/> cleared.</returns>
		/// <exception cref="ArgumentException"><paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
		[Pure]
		public static TEnum ClearFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum flagMask) => EnumsCache<TEnum>.ClearFlags(value, flagMask);
		#endregion

		#region Parsing
		/// <summary>
		/// Converts the string representation of the name or numeric value of one or more enumerated constants
		/// to an equivalent enumerated object.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space
		/// -or-
		/// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
		[Pure]
		public static TEnum Parse<[EnumConstraint] TEnum>(string value) => EnumsCache<TEnum>.ParseFlags(value);

		/// <summary>
		/// Converts the string representation of the name or numeric value of one or more enumerated constants
		/// to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space
		/// -or-
		/// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
		[Pure]
		public static TEnum Parse<[EnumConstraint] TEnum>(string value, bool ignoreCase) => EnumsCache<TEnum>.ParseFlags(value, ignoreCase);

		/// <summary>
		/// Converts the string representation of the name or numeric value of one or more enumerated constants
		/// delimited with the specified delimiter to an equivalent enumerated object.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="delimiter">The effective delimiter will be the <paramref name="delimiter"/> trimmed if it's not all whitespace.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> or <paramref name="delimiter"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space
		/// -or-
		/// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
		/// -or-
		/// <paramref name="delimiter"/> is an empty string.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
		[Pure]
		public static TEnum Parse<[EnumConstraint] TEnum>(string value, string delimiter) => EnumsCache<TEnum>.ParseFlags(value, delimiter);

		/// <summary>
		/// Converts the string representation of the name or numeric value of one or more enumerated constants
		/// delimited with the specified delimiter to an equivalent enumerated object.
		/// A parameter specifies whether the operation is case-insensitive.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="delimiter">The effective delimiter will be the <paramref name="delimiter"/> trimmed if it's not all whitespace.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> or <paramref name="delimiter"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space
		/// -or-
		/// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
		/// -or-
		/// <paramref name="delimiter"/> is an empty string.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
		[Pure]
		public static TEnum Parse<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter) => EnumsCache<TEnum>.ParseFlags(value, ignoreCase, delimiter);

		[Pure]
		public static TEnum Parse<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter, params EnumFormat[] parseFormatOrder) => EnumsCache<TEnum>.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder);

		/// <summary>
		/// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
		/// constants to an equivalent enumerated object but if it fails returns the specified enumerated value.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="defaultEnum"></param>
		/// <returns></returns>
		[Pure]
		public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, TEnum defaultEnum) => EnumsCache<TEnum>.ParseFlagsOrDefault(value, defaultEnum);

		/// <summary>
		/// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
		/// constants to an equivalent enumerated object but if it fails returns the specified enumerated value.
		/// A parameter specifies whether the operation is case-insensitive.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="defaultEnum"></param>
		/// <returns></returns>
		[Pure]
		public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, TEnum defaultEnum) => EnumsCache<TEnum>.ParseFlagsOrDefault(value, ignoreCase, defaultEnum);

		/// <summary>
		/// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
		/// constants delimited with the specified delimiter to an equivalent enumerated object but if it fails
		/// returns the specified enumerated value.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="delimiter"></param>
		/// <param name="defaultEnum"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="delimiter"/> is an empty string.</exception>
		[Pure]
		public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, string delimiter, TEnum defaultEnum) => EnumsCache<TEnum>.ParseFlagsOrDefault(value, delimiter, defaultEnum);

		/// <summary>
		/// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
		/// constants delimited with the specified delimiter to an equivalent enumerated object but if it fails
		/// returns the specified enumerated value. A parameter specifies whether the operation is case-insensitive.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="delimiter"></param>
		/// <param name="defaultEnum"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="delimiter"/> is an empty string.</exception>
		[Pure]
		public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter, TEnum defaultEnum) => EnumsCache<TEnum>.ParseFlagsOrDefault(value, ignoreCase, delimiter, defaultEnum);

		/// <summary>
		/// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
		/// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		[Pure]
		public static bool TryParse<[EnumConstraint] TEnum>(string value, out TEnum result) => EnumsCache<TEnum>.TryParseFlags(value, out result);

		/// <summary>
		/// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
		/// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
		/// A parameter specifies whether the operation is case-insensitive.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		[Pure]
		public static bool TryParse<[EnumConstraint] TEnum>(string value, bool ignoreCase, out TEnum result) => EnumsCache<TEnum>.TryParseFlags(value, ignoreCase, out result);

		/// <summary>
		/// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
		/// constants delimited with the specified delimiter to an equivalent enumerated object. The return value
		/// indicates whether the conversion succeeded.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="delimiter"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="delimiter"/> is an empty string.</exception>
		[Pure]
		public static bool TryParse<[EnumConstraint] TEnum>(string value, string delimiter, out TEnum result) => EnumsCache<TEnum>.TryParseFlags(value, delimiter, out result);

		/// <summary>
		/// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
		/// constants delimited with the specified delimiter to an equivalent enumerated object. The return value
		/// indicates whether the conversion succeeded. A parameter specifies whether the operation is case-insensitive.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="delimiter"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="delimiter"/> is an empty string.</exception>
		[Pure]
		public static bool TryParse<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result) => EnumsCache<TEnum>.TryParseFlags(value, ignoreCase, delimiter, out result);
		#endregion
	}
}
