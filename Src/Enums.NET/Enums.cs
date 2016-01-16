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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Threading;
using ExtraConstraints;

namespace EnumsNET
{
	/// <summary>
	/// Static class that provides efficient type-safe enum operations through the use of cached enum names, values, and attributes.
	/// Many operations are exposed as extension methods for convenience.
	/// </summary>
	public static class Enums
	{
		internal const int StartingCustomEnumFormatValue = 100;

		internal const int StartingGenericCustomEnumFormatValue = 200;

		private static int _lastCustomEnumFormatIndex = -1;

		internal static List<Func<IEnumMemberInfo, string>> CustomEnumFormatters;

		internal static readonly EnumFormat[] DefaultParseFormatOrder = { EnumFormat.Name, EnumFormat.DecimalValue };

		internal static readonly Attribute[] ZeroLengthAttributes = { };

		public static EnumFormat RegisterCustomEnumFormat(Func<IClsEnumMemberInfo, string> formatter) => RegisterCustomEnumFormat((Func<IEnumMemberInfo, string>)formatter);

		[CLSCompliant(false)]
		public static EnumFormat RegisterCustomEnumFormat(Func<IEnumMemberInfo, string> formatter)
		{
			var index = Interlocked.Increment(ref _lastCustomEnumFormatIndex);
			if (index == 0)
			{
				CustomEnumFormatters = new List<Func<IEnumMemberInfo, string>>();
			}
			else
			{
				while (CustomEnumFormatters.Count < index)
				{
				}
			}
			CustomEnumFormatters.Insert(index, formatter);
			return ToObject<EnumFormat>(index + StartingCustomEnumFormatValue, false);
		}

		public static EnumFormat RegisterCustomEnumFormat<TEnum>(Func<IClsEnumMemberInfo<TEnum>, string> formatter) => EnumsCache<TEnum>.RegisterCustomEnumFormat(formatter);

		[CLSCompliant(false)]
		public static EnumFormat RegisterCustomEnumFormat<TEnum>(Func<IEnumMemberInfo<TEnum>, string> formatter) => EnumsCache<TEnum>.RegisterCustomEnumFormat(formatter);

		#region "Properties"
		/// <summary>
		/// Indicates if <typeparamref name="TEnum"/>'s defined values are contiguous.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <returns>Indication if <typeparamref name="TEnum"/>'s defined values are contiguous.</returns>
		[Pure]
		public static bool IsContiguous<[EnumConstraint] TEnum>() where TEnum : struct => EnumsCache<TEnum>.IsContiguous;

		/// <summary>
		/// Retrieves the underlying type of <typeparamref name="TEnum"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <returns>The underlying type of <typeparamref name="TEnum"/>.</returns>
		[Pure]
		public static Type GetUnderlyingType<[EnumConstraint] TEnum>() where TEnum : struct => EnumsCache<TEnum>.UnderlyingType;

		[Pure]
		public static TypeCode GetTypeCode<[EnumConstraint] TEnum>() where TEnum : struct => EnumsCache<TEnum>.TypeCode;
		#endregion

		#region Type Methods
		/// <summary>
		/// Retrieves <typeparamref name="TEnum"/>'s members count.
		/// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="uniqueValued"></param>
		/// <returns><typeparamref name="TEnum"/>'s members count.</returns>
		[Pure]
		public static int GetDefinedCount<[EnumConstraint] TEnum>(bool uniqueValued = false) where TEnum : struct => EnumsCache<TEnum>.GetDefinedCount(uniqueValued);

		/// <summary>
		/// Retrieves in value order an array of info on <typeparamref name="TEnum"/>'s members.
		/// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="uniqueValued"></param>
		/// <returns></returns>
		[Pure]
		public static IEnumerable<EnumMemberInfo<TEnum>> GetEnumMemberInfos<[EnumConstraint] TEnum>(bool uniqueValued = false) where TEnum : struct => EnumsCache<TEnum>.GetEnumMemberInfos(uniqueValued);

		/// <summary>
		/// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' names.
		/// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="uniqueValued"></param>
		/// <returns>Array of <typeparamref name="TEnum"/>'s members' names in value order.</returns>
		[Pure]
		public static IEnumerable<string> GetNames<[EnumConstraint] TEnum>(bool uniqueValued = false) where TEnum : struct => EnumsCache<TEnum>.GetNames(uniqueValued);

		/// <summary>
		/// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' values.
		/// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="uniqueValued"></param>
		/// <returns>Array of <typeparamref name="TEnum"/>'s members' values in value order.</returns>
		[Pure]
		public static IEnumerable<TEnum> GetValues<[EnumConstraint] TEnum>(bool uniqueValued = false) where TEnum : struct => EnumsCache<TEnum>.GetValues(uniqueValued);

		/// <summary>
		/// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' <see cref="DescriptionAttribute.Description"/>s.
		/// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="uniqueValued"></param>
		/// <returns>Array of <typeparamref name="TEnum"/>'s members' <see cref="DescriptionAttribute.Description"/>s in value order.</returns>
		[Pure]
		public static IEnumerable<string> GetDescriptions<[EnumConstraint] TEnum>(bool uniqueValued = false) where TEnum : struct => EnumsCache<TEnum>.GetDescriptions(uniqueValued);

		/// <summary>
		/// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' descriptions else names.
		/// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="uniqueValued"></param>
		/// <returns></returns>
		[Pure]
		public static IEnumerable<string> GetDescriptionsOrNames<[EnumConstraint] TEnum>(bool uniqueValued = false) where TEnum : struct => EnumsCache<TEnum>.GetDescriptionsOrNames(uniqueValued);

		/// <summary>
		/// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' descriptions else names formatted
		/// with <paramref name="nameFormatter"/>.
		/// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="nameFormatter"></param>
		/// <param name="uniqueValued"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetDescriptionsOrNames<[EnumConstraint] TEnum>(Func<string, string> nameFormatter, bool uniqueValued = false) where TEnum : struct => EnumsCache<TEnum>.GetDescriptionsOrNames(nameFormatter, uniqueValued);

		/// <summary>
		/// Retrieves in value order an array of all of <typeparamref name="TEnum"/>'s members' attributes.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="uniqueValued"></param>
		/// <returns>Array of all of <typeparamref name="TEnum"/>'s members' attributes in value order.</returns>
		[Pure]
		public static IEnumerable<Attribute[]> GetAllAttributes<[EnumConstraint] TEnum>(bool uniqueValued = false) where TEnum : struct => EnumsCache<TEnum>.GetAllAttributes(uniqueValued);

		/// <summary>
		/// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' <typeparamref name="TAttribute"/>s.
		/// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <typeparam name="TAttribute"></typeparam>
		/// <param name="uniqueValued"></param>
		/// <returns>Array of <typeparamref name="TEnum"/>'s members' <typeparamref name="TAttribute"/> in value order.</returns>
		[Pure]
		public static IEnumerable<TAttribute> GetAttributes<[EnumConstraint] TEnum, TAttribute>(bool uniqueValued = false)
			where TAttribute : Attribute where TEnum : struct => EnumsCache<TEnum>.GetAttributes<TAttribute>(uniqueValued);

		/// <summary>
		/// Compares two <typeparamref name="TEnum"/>'s for ordering.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>1 if <paramref name="x"/> is greater than <paramref name="y"/>, 0 if <paramref name="x"/> equals <paramref name="y"/>,
		/// and -1 if <paramref name="x"/> is less than <paramref name="y"/>.</returns>
		[Pure]
		public static int Compare<[EnumConstraint] TEnum>(TEnum x, TEnum y) where TEnum : struct => EnumsCache<TEnum>.Compare(x, y);

		[Pure]
		public static bool Equals<[EnumConstraint] TEnum>(TEnum x, TEnum y) where TEnum : struct => EnumsCache<TEnum>.EqualsMethod(x, y);
		#endregion

		#region IsValid
		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
		/// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
		[Pure]
		public static bool IsValid<[EnumConstraint] TEnum>(object value) where TEnum : struct => EnumsCache<TEnum>.IsValid(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">The enum value.</param>
		/// <returns>Indication whether <paramref name="value"/> is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
		[Pure]
		public static bool IsValid<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.IsValid(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsValid<[EnumConstraint] TEnum>(sbyte value) where TEnum : struct => EnumsCache<TEnum>.IsValid(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
		[Pure]
		public static bool IsValid<[EnumConstraint] TEnum>(byte value) where TEnum : struct => EnumsCache<TEnum>.IsValid(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
		[Pure]
		public static bool IsValid<[EnumConstraint] TEnum>(short value) where TEnum : struct => EnumsCache<TEnum>.IsValid(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsValid<[EnumConstraint] TEnum>(ushort value) where TEnum : struct => EnumsCache<TEnum>.IsValid(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
		[Pure]
		public static bool IsValid<[EnumConstraint] TEnum>(int value) where TEnum : struct => EnumsCache<TEnum>.IsValid(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsValid<[EnumConstraint] TEnum>(uint value) where TEnum : struct => EnumsCache<TEnum>.IsValid(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
		[Pure]
		public static bool IsValid<[EnumConstraint] TEnum>(long value) where TEnum : struct => EnumsCache<TEnum>.IsValid(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
		/// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsValid<[EnumConstraint] TEnum>(ulong value) where TEnum : struct => EnumsCache<TEnum>.IsValid(value);
		#endregion

		#region IsDefined
		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to <typeparamref name="TEnum"/> and is defined.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
		/// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to <typeparamref name="TEnum"/> and is defined.</returns>
		[Pure]
		public static bool IsDefined<[EnumConstraint] TEnum>(object value) where TEnum : struct => EnumsCache<TEnum>.IsDefined(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> is defined.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">The enum value.</param>
		/// <returns>Indication whether <paramref name="value"/> is defined.</returns>
		[Pure]
		public static bool IsDefined<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.IsDefined(value);

		/// <summary>
		/// Indicates whether a constant with the specified <paramref name="name"/> exists in <typeparamref name="TEnum"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="name">The name to check existence.</param>
		/// <returns>Indication whether a constant with the specified <paramref name="name"/> exists in <typeparamref name="TEnum"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
		[Pure]
		public static bool IsDefined<[EnumConstraint] TEnum>(string name) where TEnum : struct => EnumsCache<TEnum>.IsDefined(name);

		/// <summary>
		/// Indicates whether a constant with the specified <paramref name="name"/> exists in <typeparamref name="TEnum"/>.
		/// <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="name"></param>
		/// <param name="ignoreCase">Specifies whether the operation is case-insensitive.</param>
		/// <returns>Indication whether a constant with the specified <paramref name="name"/> exists in <typeparamref name="TEnum"/>.
		/// <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
		[Pure]
		public static bool IsDefined<[EnumConstraint] TEnum>(string name, bool ignoreCase) where TEnum : struct => EnumsCache<TEnum>.IsDefined(name, ignoreCase);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsDefined<[EnumConstraint] TEnum>(sbyte value) where TEnum : struct => EnumsCache<TEnum>.IsDefined(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.</returns>
		[Pure]
		public static bool IsDefined<[EnumConstraint] TEnum>(byte value) where TEnum : struct => EnumsCache<TEnum>.IsDefined(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.</returns>
		[Pure]
		public static bool IsDefined<[EnumConstraint] TEnum>(short value) where TEnum : struct => EnumsCache<TEnum>.IsDefined(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsDefined<[EnumConstraint] TEnum>(ushort value) where TEnum : struct => EnumsCache<TEnum>.IsDefined(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.</returns>
		[Pure]
		public static bool IsDefined<[EnumConstraint] TEnum>(int value) where TEnum : struct => EnumsCache<TEnum>.IsDefined(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsDefined<[EnumConstraint] TEnum>(uint value) where TEnum : struct => EnumsCache<TEnum>.IsDefined(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.</returns>
		[Pure]
		public static bool IsDefined<[EnumConstraint] TEnum>(long value) where TEnum : struct => EnumsCache<TEnum>.IsDefined(value);

		/// <summary>
		/// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
		/// and that that value is defined.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsDefined<[EnumConstraint] TEnum>(ulong value) where TEnum : struct => EnumsCache<TEnum>.IsDefined(value);
		#endregion

		#region IsInValueRange
		/// <summary>
		/// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsInValueRange<[EnumConstraint] TEnum>(sbyte value) where TEnum : struct => EnumsCache<TEnum>.IsInValueRange(value);

		/// <summary>
		/// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
		[Pure]
		public static bool IsInValueRange<[EnumConstraint] TEnum>(byte value) where TEnum : struct => EnumsCache<TEnum>.IsInValueRange(value);

		/// <summary>
		/// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
		[Pure]
		public static bool IsInValueRange<[EnumConstraint] TEnum>(short value) where TEnum : struct => EnumsCache<TEnum>.IsInValueRange(value);

		/// <summary>
		/// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsInValueRange<[EnumConstraint] TEnum>(ushort value) where TEnum : struct => EnumsCache<TEnum>.IsInValueRange(value);

		/// <summary>
		/// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
		[Pure]
		public static bool IsInValueRange<[EnumConstraint] TEnum>(int value) where TEnum : struct => EnumsCache<TEnum>.IsInValueRange(value);

		/// <summary>
		/// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsInValueRange<[EnumConstraint] TEnum>(uint value) where TEnum : struct => EnumsCache<TEnum>.IsInValueRange(value);

		/// <summary>
		/// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
		[Pure]
		public static bool IsInValueRange<[EnumConstraint] TEnum>(long value) where TEnum : struct => EnumsCache<TEnum>.IsInValueRange(value);

		/// <summary>
		/// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool IsInValueRange<[EnumConstraint] TEnum>(ulong value) where TEnum : struct => EnumsCache<TEnum>.IsInValueRange(value);
		#endregion

		#region ToObject
		/// <summary>
		/// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that the result is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
		/// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/>.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="value"/> is not a valid type
		/// -or-
		/// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
		[Pure]
		public static TEnum ToObject<[EnumConstraint] TEnum>(object value, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObject(value, validate);

		/// <summary>
		/// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to convert.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
		[Pure]
		[CLSCompliant(false)]
		public static TEnum ToObject<[EnumConstraint] TEnum>(sbyte value, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObject(value, validate);

		/// <summary>
		/// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to convert.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
		[Pure]
		public static TEnum ToObject<[EnumConstraint] TEnum>(byte value, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObject(value, validate);

		/// <summary>
		/// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to convert.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
		[Pure]
		public static TEnum ToObject<[EnumConstraint] TEnum>(short value, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObject(value, validate);

		/// <summary>
		/// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to convert.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
		[Pure]
		[CLSCompliant(false)]
		public static TEnum ToObject<[EnumConstraint] TEnum>(ushort value, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObject(value, validate);

		/// <summary>
		/// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to convert.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
		[Pure]
		public static TEnum ToObject<[EnumConstraint] TEnum>(int value, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObject(value, validate);

		/// <summary>
		/// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to convert.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
		[Pure]
		[CLSCompliant(false)]
		public static TEnum ToObject<[EnumConstraint] TEnum>(uint value, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObject(value, validate);

		/// <summary>
		/// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to convert.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
		[Pure]
		public static TEnum ToObject<[EnumConstraint] TEnum>(long value, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObject(value, validate);

		/// <summary>
		/// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to convert.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
		/// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
		[Pure]
		[CLSCompliant(false)]
		public static TEnum ToObject<[EnumConstraint] TEnum>(ulong value, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObject(value, validate);

		/// <summary>
		/// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
		/// <paramref name="defaultEnum"/> is returned.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
		/// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
		/// <param name="defaultEnum">The fallback value to return.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
		[Pure]
		public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(object value, TEnum defaultEnum, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObjectOrDefault(value, defaultEnum, validate);

		/// <summary>
		/// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
		/// <paramref name="defaultEnum"/> is returned.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="defaultEnum">The fallback value to return.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(sbyte value, TEnum defaultEnum, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObjectOrDefault(value, defaultEnum, validate);

		/// <summary>
		/// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
		/// <paramref name="defaultEnum"/> is returned.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="defaultEnum">The fallback value to return.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
		[Pure]
		public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(byte value, TEnum defaultEnum, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObjectOrDefault(value, defaultEnum, validate);

		/// <summary>
		/// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
		/// <paramref name="defaultEnum"/> is returned.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="defaultEnum">The fallback value to return.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
		[Pure]
		public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(short value, TEnum defaultEnum, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObjectOrDefault(value, defaultEnum, validate);

		/// <summary>
		/// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
		/// <paramref name="defaultEnum"/> is returned.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="defaultEnum">The fallback value to return.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(ushort value, TEnum defaultEnum, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObjectOrDefault(value, defaultEnum, validate);

		/// <summary>
		/// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
		/// <paramref name="defaultEnum"/> is returned.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="defaultEnum">The fallback value to return.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
		[Pure]
		public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(int value, TEnum defaultEnum, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObjectOrDefault(value, defaultEnum, validate);

		/// <summary>
		/// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
		/// <paramref name="defaultEnum"/> is returned.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="defaultEnum">The fallback value to return.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(uint value, TEnum defaultEnum, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObjectOrDefault(value, defaultEnum, validate);

		/// <summary>
		/// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
		/// <paramref name="defaultEnum"/> is returned.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="defaultEnum">The fallback value to return.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
		[Pure]
		public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(long value, TEnum defaultEnum, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObjectOrDefault(value, defaultEnum, validate);

		/// <summary>
		/// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
		/// <paramref name="defaultEnum"/> is returned.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="defaultEnum">The fallback value to return.</param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
		[Pure]
		[CLSCompliant(false)]
		public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(ulong value, TEnum defaultEnum, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.ToObjectOrDefault(value, defaultEnum, validate);
		
		// TODO: Check documentation from here to the end

		/// <summary>
		/// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
		/// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is returned in the output parameter <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
		/// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
		/// <param name="result"></param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns></returns>
		[Pure]
		public static bool TryToObject<[EnumConstraint] TEnum>(object value, out TEnum result, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.TryToObject(value, out result, validate);

		/// <summary>
		/// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
		/// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="result"></param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns></returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool TryToObject<[EnumConstraint] TEnum>(sbyte value, out TEnum result, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.TryToObject(value, out result, validate);

		/// <summary>
		/// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
		/// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="result"></param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns></returns>
		[Pure]
		public static bool TryToObject<[EnumConstraint] TEnum>(byte value, out TEnum result, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.TryToObject(value, out result, validate);

		/// <summary>
		/// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
		/// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="result"></param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns></returns>
		[Pure]
		public static bool TryToObject<[EnumConstraint] TEnum>(short value, out TEnum result, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.TryToObject(value, out result, validate);

		/// <summary>
		/// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
		/// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="result"></param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns></returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool TryToObject<[EnumConstraint] TEnum>(ushort value, out TEnum result, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.TryToObject(value, out result, validate);

		/// <summary>
		/// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
		/// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="result"></param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns></returns>
		[Pure]
		public static bool TryToObject<[EnumConstraint] TEnum>(int value, out TEnum result, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.TryToObject(value, out result, validate);

		/// <summary>
		/// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
		/// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="result"></param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns></returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool TryToObject<[EnumConstraint] TEnum>(uint value, out TEnum result, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.TryToObject(value, out result, validate);

		/// <summary>
		/// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
		/// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="result"></param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns></returns>
		[Pure]
		public static bool TryToObject<[EnumConstraint] TEnum>(long value, out TEnum result, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.TryToObject(value, out result, validate);

		/// <summary>
		/// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
		/// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
		/// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value">Value to try to convert.</param>
		/// <param name="result"></param>
		/// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
		/// <returns></returns>
		[Pure]
		[CLSCompliant(false)]
		public static bool TryToObject<[EnumConstraint] TEnum>(ulong value, out TEnum result, bool validate = true) where TEnum : struct => EnumsCache<TEnum>.TryToObject(value, out result, validate);
		#endregion

		#region All Values Main Methods
		/// <summary>
		/// Validates that <paramref name="value"/> is valid. If it's not it throws an <see cref="ArgumentException"/> with the given <paramref name="paramName"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="paramName"></param>
		/// <returns><paramref name="value"/> for use in constructor initializers and fluent API's</returns>
		/// <exception cref="ArgumentException"><paramref name="value"/> is invalid</exception>
		[Pure]
		public static TEnum Validate<[EnumConstraint] TEnum>(this TEnum value, string paramName) where TEnum : struct => EnumsCache<TEnum>.Validate(value, paramName);

		/// <summary>
		/// Converts the specified <paramref name="value"/> to its equivalent string representation.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		[Pure]
		public static string AsString<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.AsString(value);

		/// <summary>
		/// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		/// <exception cref="FormatException"><paramref name="format"/> is an invalid value</exception>
		[Pure]
		public static string AsString<[EnumConstraint] TEnum>(this TEnum value, string format) where TEnum : struct => EnumsCache<TEnum>.AsString(value, format);

		/// <summary>
		/// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="formats"/>.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="formats"></param>
		/// <returns></returns>
		[Pure]
		public static string AsString<[EnumConstraint] TEnum>(this TEnum value, params EnumFormat[] formats) where TEnum : struct => EnumsCache<TEnum>.AsString(value, formats);

		/// <summary>
		/// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
		/// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
		[Pure]
		public static string Format<[EnumConstraint] TEnum>(this TEnum value, string format) where TEnum : struct => EnumsCache<TEnum>.Format(value, format);

		[Pure]
		public static string Format<[EnumConstraint] TEnum>(this TEnum value, EnumFormat format) where TEnum : struct => EnumsCache<TEnum>.Format(value, format);

		[Pure]
		public static string Format<[EnumConstraint] TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1) where TEnum : struct => EnumsCache<TEnum>.Format(value, format0, format1);

		[Pure]
		public static string Format<[EnumConstraint] TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2) where TEnum : struct => EnumsCache<TEnum>.Format(value, format0, format1, format2);

		[Pure]
		public static string Format<[EnumConstraint] TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3) where TEnum : struct => EnumsCache<TEnum>.Format(value, format0, format1, format2, format3);

		[Pure]
		public static string Format<[EnumConstraint] TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4) where TEnum : struct => EnumsCache<TEnum>.Format(value, format0, format1, format2, format3, format4);

		/// <summary>
		/// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="formats"/>.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="formats"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="formats"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="formats"/> is empty.</exception>
		[Pure]
		public static string Format<[EnumConstraint] TEnum>(this TEnum value, params EnumFormat[] formats) where TEnum : struct => EnumsCache<TEnum>.Format(value, formats);

		/// <summary>
		/// Returns an object with the enum's underlying value.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		[Pure]
		public static object GetUnderlyingValue<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.GetUnderlyingValue(value);

		/// <summary>
		/// Converts <paramref name="value"/> to an <see cref="sbyte"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="sbyte"/>'s value range without overflowing</exception>
		[Pure]
		[CLSCompliant(false)]
		public static sbyte ToSByte<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.ToSByte(value);

		/// <summary>
		/// Converts <paramref name="value"/> to a <see cref="byte"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="byte"/>'s value range without overflowing</exception>
		[Pure]
		public static byte ToByte<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.ToByte(value);

		/// <summary>
		/// Converts <paramref name="value"/> to an <see cref="short"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="short"/>'s value range without overflowing</exception>
		[Pure]
		public static short ToInt16<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.ToInt16(value);

		/// <summary>
		/// Converts <paramref name="value"/> to a <see cref="ushort"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ushort"/>'s value range without overflowing</exception>
		[Pure]
		[CLSCompliant(false)]
		public static ushort ToUInt16<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.ToUInt16(value);

		/// <summary>
		/// Converts <paramref name="value"/> to an <see cref="int"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="int"/>'s value range without overflowing</exception>
		[Pure]
		public static int ToInt32<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.ToInt32(value);

		/// <summary>
		/// Converts <paramref name="value"/> to a <see cref="uint"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="uint"/>'s value range without overflowing</exception>
		[Pure]
		[CLSCompliant(false)]
		public static uint ToUInt32<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.ToUInt32(value);

		/// <summary>
		/// Converts <paramref name="value"/> to an <see cref="long"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="long"/>'s value range without overflowing</exception>
		[Pure]
		public static long ToInt64<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.ToInt64(value);

		/// <summary>
		/// Converts <paramref name="value"/> to a <see cref="ulong"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ulong"/>'s value range without overflowing</exception>
		[Pure]
		[CLSCompliant(false)]
		public static ulong ToUInt64<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.ToUInt64(value);

		[Pure]
		public static int GetHashCode<[EnumConstraint] TEnum>(TEnum value) where TEnum : struct => EnumsCache<TEnum>.GetHashCodeMethod(value);
		#endregion

		#region Defined Values Main Methods
		[Pure]
		public static EnumMemberInfo<TEnum> GetEnumMemberInfo<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.GetEnumMemberInfo(value);

		[Pure]
		public static EnumMemberInfo<TEnum> GetEnumMemberInfo<[EnumConstraint] TEnum>(string name) where TEnum : struct => EnumsCache<TEnum>.GetEnumMemberInfo(name);

		[Pure]
		public static EnumMemberInfo<TEnum> GetEnumMemberInfo<[EnumConstraint] TEnum>(string name, bool ignoreCase) where TEnum : struct => EnumsCache<TEnum>.GetEnumMemberInfo(name, ignoreCase);

		/// <summary>
		/// Retrieves the name of the constant in <typeparamref name="TEnum"/> that has the specified <paramref name="value"/>. If <paramref name="value"/>
		/// is not defined null is returned.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Name of the constant in <typeparamref name="TEnum"/> that has the specified <paramref name="value"/>. If <paramref name="value"/>
		/// is not defined null is returned.</returns>
		[Pure]
		public static string GetName<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.GetName(value);

		/// <summary>
		/// Retrieves the description of the constant in the enumeration that has the specified <paramref name="value"/>. If <paramref name="value"/>
		/// is not defined or no associated <see cref="DescriptionAttribute"/> is found then null is returned.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns>Description of the constant in the enumeration that has the specified <paramref name="value"/>. If <paramref name="value"/>
		/// is not defined or no associated <see cref="DescriptionAttribute"/> is found then null is returned.</returns>
		[Pure]
		public static string GetDescription<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.GetDescription(value);

		[Pure]
		public static string GetDescriptionOrName<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.GetDescriptionOrName(value);

		public static string GetDescriptionOrName<[EnumConstraint] TEnum>(this TEnum value, Func<string, string> nameFormatter) where TEnum : struct => EnumsCache<TEnum>.GetDescriptionOrName(value, nameFormatter);
		#endregion

		#region Attributes
		/// <summary>
		/// Indicates if the enumerated constant with the specified <paramref name="value"/> has a <typeparamref name="TAttribute"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <typeparam name="TAttribute"></typeparam>
		/// <param name="value"></param>
		/// <returns>Indication if the enumerated constant with the specified <paramref name="value"/> has a <typeparamref name="TAttribute"/>.</returns>
		[Pure]
		public static bool HasAttribute<[EnumConstraint] TEnum, TAttribute>(this TEnum value)
			where TAttribute : Attribute where TEnum : struct => EnumsCache<TEnum>.HasAttribute<TAttribute>(value);

		/// <summary>
		/// Retrieves the <typeparamref name="TAttribute"/> if it exists of the enumerated constant with the specified <paramref name="value"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <typeparam name="TAttribute"></typeparam>
		/// <param name="value"></param>
		/// <returns><typeparamref name="TAttribute"/> of the enumerated constant with the specified <paramref name="value"/> if defined and has attribute, else null</returns>
		[Pure]
		public static TAttribute GetAttribute<[EnumConstraint] TEnum, TAttribute>(this TEnum value)
			where TAttribute : Attribute where TEnum : struct => EnumsCache<TEnum>.GetAttribute<TAttribute>(value);

		/// <summary>
		/// Retrieves the <typeparamref name="TAttribute"/> if it exists of the enumerated constant with the specified <paramref name="value"/>
		/// and then applies the <paramref name="selector"/> else it returns <paramref name="defaultValue"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <typeparam name="TAttribute"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="value"></param>
		/// <param name="selector"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="selector"/> is null</exception>
		[Pure]
		public static TResult GetAttributeSelect<[EnumConstraint] TEnum, TAttribute, TResult>(this TEnum value, Func<TAttribute, TResult> selector, TResult defaultValue = default(TResult))
			where TAttribute : Attribute where TEnum : struct => EnumsCache<TEnum>.GetAttributeSelect(value, selector, defaultValue);

		[Pure]
		public static bool TryGetAttributeSelect<[EnumConstraint] TEnum, TAttribute, TResult>(this TEnum value, Func<TAttribute, TResult> selector, out TResult result)
			where TAttribute : Attribute where TEnum : struct => EnumsCache<TEnum>.TryGetAttributeSelect(value, selector, out result);

		/// <summary>
		/// Retrieves an array of <typeparamref name="TAttribute"/>'s of the constant in the enumeration that has the specified <paramref name="value"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <typeparam name="TAttribute"></typeparam>
		/// <param name="value"></param>
		/// <returns><typeparamref name="TAttribute"/> array</returns>
		[Pure]
		public static IEnumerable<TAttribute> GetAttributes<[EnumConstraint] TEnum, TAttribute>(this TEnum value)
			where TAttribute : Attribute where TEnum : struct => EnumsCache<TEnum>.GetAttributes<TAttribute>(value);

		/// <summary>
		/// Retrieves an array of all the <see cref="Attribute"/>'s of the constant in the enumeration that has the specified <paramref name="value"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns><see cref="Attribute"/> array if value is defined, else null</returns>
		[Pure]
		public static Attribute[] GetAllAttributes<[EnumConstraint] TEnum>(this TEnum value) where TEnum : struct => EnumsCache<TEnum>.GetAllAttributes(value);
		#endregion

		#region Parsing
		/// <summary>
		/// Converts the string representation of the name or numeric value of one or more enumerated constants
		/// to an equivalent enumerated object.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space.
		/// -or-
		/// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/></exception>
		[Pure]
		public static TEnum Parse<[EnumConstraint] TEnum>(string value) where TEnum : struct => EnumsCache<TEnum>.Parse(value);

		/// <summary>
		/// Converts the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="parseFormatOrder"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space.
		/// -or-
		/// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/></exception>
		[Pure]
		public static TEnum Parse<[EnumConstraint] TEnum>(string value, params EnumFormat[] parseFormatOrder) where TEnum : struct => EnumsCache<TEnum>.Parse(value, parseFormatOrder);

		/// <summary>
		/// Converts the string representation of the name or numeric value of one or more enumerated constants
		/// to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space.
		/// -or-
		/// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/></exception>
		[Pure]
		public static TEnum Parse<[EnumConstraint] TEnum>(string value, bool ignoreCase) where TEnum : struct => EnumsCache<TEnum>.Parse(value, ignoreCase);

		/// <summary>
		/// Converts the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
		/// A parameter specifies whether the operation is case-insensitive.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="parseFormatOrder"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space.
		/// -or-
		/// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
		/// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/></exception>
		[Pure]
		public static TEnum Parse<[EnumConstraint] TEnum>(string value, bool ignoreCase, params EnumFormat[] parseFormatOrder) where TEnum : struct => EnumsCache<TEnum>.Parse(value, ignoreCase, parseFormatOrder);

		/// <summary>
		/// Tries to convert the string representation of the name or numeric value of one or more enumerated
		/// constants to an equivalent enumerated object but if it fails returns the specified default enumerated value.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="defaultEnum"></param>
		/// <returns></returns>
		[Pure]
		public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, TEnum defaultEnum) where TEnum : struct => EnumsCache<TEnum>.ParseOrDefault(value, defaultEnum);

		/// <summary>
		/// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
		/// but if it fails returns the specified default enumerated value.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="defaultEnum"></param>
		/// <param name="parseFormatOrder"></param>
		/// <returns></returns>
		[Pure]
		public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, TEnum defaultEnum, params EnumFormat[] parseFormatOrder) where TEnum : struct => EnumsCache<TEnum>.ParseOrDefault(value, defaultEnum, parseFormatOrder);

		/// <summary>
		/// Tries to convert the string representation of the name or numeric value of one or more enumerated
		/// constants to an equivalent enumerated object but if it fails returns the specified default enumerated value.
		/// A parameter specifies whether the operation is case-insensitive.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="defaultEnum"></param>
		/// <returns></returns>
		[Pure]
		public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, TEnum defaultEnum) where TEnum : struct => EnumsCache<TEnum>.ParseOrDefault(value, ignoreCase, defaultEnum);

		/// <summary>
		/// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
		/// but if it fails returns the specified default enumerated value. A parameter specifies whether the operation is case-insensitive.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="defaultEnum"></param>
		/// <param name="parseFormatOrder"></param>
		/// <returns></returns>
		[Pure]
		public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, TEnum defaultEnum, params EnumFormat[] parseFormatOrder) where TEnum : struct => EnumsCache<TEnum>.ParseOrDefault(value, ignoreCase, defaultEnum, parseFormatOrder);

		/// <summary>
		/// Tries to convert the string representation of the name or numeric value of one or more enumerated
		/// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		[Pure]
		public static bool TryParse<[EnumConstraint] TEnum>(string value, out TEnum result) where TEnum : struct => EnumsCache<TEnum>.TryParse(value, out result);

		/// <summary>
		/// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
		/// The return value indicates whether the conversion succeeded.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="result"></param>
		/// <param name="parseFormatOrder"></param>
		/// <returns></returns>
		[Pure]
		public static bool TryParse<[EnumConstraint] TEnum>(string value, out TEnum result, params EnumFormat[] parseFormatOrder) where TEnum : struct => EnumsCache<TEnum>.TryParse(value, out result, parseFormatOrder);

		/// <summary>
		/// Tries to convert the string representation of the name or numeric value of one or more enumerated
		/// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
		/// A parameter specifies whether the operation is case-insensitive.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		[Pure]
		public static bool TryParse<[EnumConstraint] TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct => EnumsCache<TEnum>.TryParse(value, ignoreCase, out result);

		/// <summary>
		/// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
		/// The return value indicates whether the conversion succeeded. A parameter specifies whether the operation is case-insensitive.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <param name="result"></param>
		/// <param name="parseFormatOrder"></param>
		/// <returns></returns>
		[Pure]
		public static bool TryParse<[EnumConstraint] TEnum>(string value, bool ignoreCase, out TEnum result, params EnumFormat[] parseFormatOrder) where TEnum : struct => EnumsCache<TEnum>.TryParse(value, ignoreCase, out result, parseFormatOrder);
		#endregion

		#region Internal Methods
		internal static string DescriptionEnumFormatter(IEnumMemberInfo info) => info.Description;

		internal static string GetDescription(Attribute[] attributes)
		{
			return attributes.Length > 0 ? (attributes[0] as DescriptionAttribute)?.Description : null;
		}

		internal static TAttribute GetAttribute<TAttribute>(Attribute[] attributes)
			where TAttribute : Attribute
		{
			foreach (var attribute in attributes)
			{
				var castedAttr = attribute as TAttribute;
				if (castedAttr != null)
				{
					return castedAttr;
				}
			}
			return null;
		}

		internal static IEnumerable<TAttribute> GetAttributes<TAttribute>(Attribute[] attributes)
			where TAttribute : Attribute
		{
			foreach (var attribute in attributes)
			{
				var castedAttr = attribute as TAttribute;
				if (castedAttr != null)
				{
					yield return castedAttr;
				}
			}
		}

		internal static bool IsNumeric(string value) => char.IsDigit(value[0]) || value[0] == '-' || value[0] == '+';

		internal static OverflowException GetOverflowException() => new OverflowException("value is outside the underlying type's value range");
		#endregion
	}
}