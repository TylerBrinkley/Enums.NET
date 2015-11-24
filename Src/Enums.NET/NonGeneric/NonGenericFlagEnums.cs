// Enums.NET
// Copyright 2015 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//	  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Diagnostics.Contracts;

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
		public static bool IsFlagEnum(Type enumType) => NonGenericEnums.GetEnumsCache(enumType).IsFlagEnum;

		/// <summary>
		/// Retrieves all the flags defined by <paramref name="enumType"/>.
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns>All the flags defined by <paramref name="enumType"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
		/// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
		[Pure]
		public static object GetAllFlags(Type enumType) => NonGenericEnums.GetEnumsCache(enumType).AllFlags;
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
		public static bool IsValidFlagCombination(Type enumType, object value) => NonGenericEnums.GetEnumsCache(enumType).IsValidFlagCombination(value);

		[Pure]
		public static string FormatAsFlags(Type enumType, object value, params EnumFormat[] formats) => NonGenericEnums.GetEnumsCache(enumType).FormatAsFlags(value, formats);

		[Pure]
		public static string FormatAsFlags(Type enumType, object value, string delimiter, params EnumFormat[] formats) => NonGenericEnums.GetEnumsCache(enumType).FormatAsFlags(value, delimiter, formats);

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
		public static object[] GetFlags(Type enumType, object value) => NonGenericEnums.GetEnumsCache(enumType).GetFlags(value);

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
		public static bool HasAnyFlags(Type enumType, object value) => NonGenericEnums.GetEnumsCache(enumType).HasAnyFlags(value);

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
		public static bool HasAnyFlags(Type enumType, object value, object flagMask) => NonGenericEnums.GetEnumsCache(enumType).HasAnyFlags(value, flagMask);

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
		public static bool HasAllFlags(Type enumType, object value) => NonGenericEnums.GetEnumsCache(enumType).HasAllFlags(value);

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
		public static bool HasAllFlags(Type enumType, object value, object flagMask) => NonGenericEnums.GetEnumsCache(enumType).HasAllFlags(value, flagMask);

		/// <summary>
		/// Returns <paramref name="value"/> with all of it's flags inverted. Equivalent to the bitwise "xor" operator with <see cref="GetAllFlags(Type)"/>.
		/// </summary>
		/// <param name="enumType"></param>
		/// <param name="value">Must be a valid flag combination.</param>
		/// <returns><paramref name="value"/> with all of it's flags inverted.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
		/// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
		/// -or-
		/// <paramref name="value"/> is an invalid type
		/// -or-
		/// <paramref name="value"/> is not a valid flag combination.</exception>
		[Pure]
		public static object InvertFlags(Type enumType, object value) => NonGenericEnums.GetEnumsCache(enumType).InvertFlags(value);

		/// <summary>
		/// Returns <paramref name="value"/> while inverting the flags that are set in <paramref name="flagMask"/>. Equivalent to the bitwise "xor" operator.
		/// </summary>
		/// <param name="enumType"></param>
		/// <param name="value">Must be a valid flag combination.</param>
		/// <param name="flagMask">Must be a valid flag combination.</param>
		/// <returns><paramref name="value"/> while inverting the flags that are set in <paramref name="flagMask"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
		/// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
		/// -or-
		/// <paramref name="value"/> is an invalid type
		/// -or-
		/// <paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
		[Pure]
		public static object InvertFlags(Type enumType, object value, object flagMask) => NonGenericEnums.GetEnumsCache(enumType).InvertFlags(value, flagMask);

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
		public static object CommonFlags(Type enumType, object value, object flagMask) => NonGenericEnums.GetEnumsCache(enumType).CommonFlags(value, flagMask);

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
		public static object SetFlags(Type enumType, object flag0, object flag1) => NonGenericEnums.GetEnumsCache(enumType).SetFlags(flag0, flag1);

		[Pure]
		public static object SetFlags(Type enumType, object flag0, object flag1, object flag2) => NonGenericEnums.GetEnumsCache(enumType).SetFlags(flag0, flag1, flag2);

		[Pure]
		public static object SetFlags(Type enumType, object flag0, object flag1, object flag2, object flag3) => NonGenericEnums.GetEnumsCache(enumType).SetFlags(flag0, flag1, flag2, flag3);

		[Pure]
		public static object SetFlags(Type enumType, object flag0, object flag1, object flag2, object flag3, object flag4) => NonGenericEnums.GetEnumsCache(enumType).SetFlags(flag0, flag1, flag2, flag3, flag4);

		[Pure]
		public static object SetFlags(Type enumType, params object[] flags) => NonGenericEnums.GetEnumsCache(enumType).SetFlags(flags);

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
		public static object ClearFlags(Type enumType, object value, object flagMask) => NonGenericEnums.GetEnumsCache(enumType).ClearFlags(value, flagMask);
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
		public static object Parse(Type enumType, string value) => NonGenericEnums.GetEnumsCache(enumType).ParseFlags(value);

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
		public static object Parse(Type enumType, string value, bool ignoreCase) => NonGenericEnums.GetEnumsCache(enumType).ParseFlags(value, ignoreCase);

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
		public static object Parse(Type enumType, string value, string delimiter) => NonGenericEnums.GetEnumsCache(enumType).ParseFlags(value, delimiter);

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
		public static object Parse(Type enumType, string value, bool ignoreCase, string delimiter) => NonGenericEnums.GetEnumsCache(enumType).ParseFlags(value, ignoreCase, delimiter);

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
		public static object ParseOrDefault(Type enumType, string value, object defaultEnum) => NonGenericEnums.GetEnumsCache(enumType).ParseFlagsOrDefault(value, defaultEnum);

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
		public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, object defaultEnum) => NonGenericEnums.GetEnumsCache(enumType).ParseFlagsOrDefault(value, ignoreCase, defaultEnum);

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
		public static object ParseOrDefault(Type enumType, string value, string delimiter, object defaultEnum) => NonGenericEnums.GetEnumsCache(enumType).ParseFlagsOrDefault(value, delimiter, defaultEnum);

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
		public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, string delimiter, object defaultEnum) => NonGenericEnums.GetEnumsCache(enumType).ParseFlagsOrDefault(value, ignoreCase, delimiter, defaultEnum);

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
		public static bool TryParse(Type enumType, string value, out object result) => NonGenericEnums.GetEnumsCache(enumType).TryParseFlags(value, out result);

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
		public static bool TryParse(Type enumType, string value, bool ignoreCase, out object result) => NonGenericEnums.GetEnumsCache(enumType).TryParseFlags(value, ignoreCase, out result);

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
		public static bool TryParse(Type enumType, string value, string delimiter, out object result) => NonGenericEnums.GetEnumsCache(enumType).TryParseFlags(value, delimiter, out result);

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
		public static bool TryParse(Type enumType, string value, bool ignoreCase, string delimiter, out object result) => NonGenericEnums.GetEnumsCache(enumType).TryParseFlags(value, ignoreCase, delimiter, out result);
		#endregion
	}
}
