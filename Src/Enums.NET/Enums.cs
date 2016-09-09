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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Threading;
using EnumsNET.Numerics;
using ExtraConstraints;

namespace EnumsNET
{
    /// <summary>
    /// Static class that provides efficient type-safe enum operations through the use of cached enum names, values, and attributes.
    /// Many operations are exposed as C# extension methods for convenience.
    /// </summary>
    public static class Enums
    {
        internal static readonly EnumFormat[] DefaultFormatOrder = { EnumFormat.Name, EnumFormat.DecimalValue };

        internal static readonly Attribute[] EmptyAttributes = { };

        internal const int StartingCustomEnumFormatValue = 100;

        internal static int HighestCustomEnumFormatIndex = -1;

        private static List<Func<EnumMember, string>> _customEnumMemberFormatters;
        
        /// <summary>
        /// Registers a global custom enum format
        /// </summary>
        /// <param name="enumMemberFormatter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumMemberFormatter"/> is null.</exception>
        public static EnumFormat RegisterCustomEnumFormat(Func<EnumMember, string> enumMemberFormatter)
        {
            Preconditions.NotNull(enumMemberFormatter, nameof(enumMemberFormatter));

            var index = Interlocked.Increment(ref HighestCustomEnumFormatIndex);
            if (index == 0)
            {
                _customEnumMemberFormatters = new List<Func<EnumMember, string>>();
            }
            else
            {
                while (_customEnumMemberFormatters?.Count != index)
                {
                }
            }
            _customEnumMemberFormatters.Add(enumMemberFormatter);
            return (EnumFormat)(index + StartingCustomEnumFormatValue);
        }

        #region "Properties"
        /// <summary>
        /// Indicates if <typeparamref name="TEnum"/>'s defined values are contiguous.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>Indication if <typeparamref name="TEnum"/>'s defined values are contiguous.</returns>
        [Pure]
        public static bool IsContiguous<[EnumConstraint] TEnum>()
            where TEnum : struct => Enums<TEnum>.Info.IsContiguous;

        /// <summary>
        /// Retrieves the underlying type of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>The underlying type of <typeparamref name="TEnum"/>.</returns>
        [Pure]
        public static Type GetUnderlyingType<[EnumConstraint] TEnum>()
            where TEnum : struct => Enums<TEnum>.Info.UnderlyingType;

        /// <summary>
        /// Gets <typeparamref name="TEnum"/>'s underlying type's <see cref="TypeCode"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        [Pure]
        public static TypeCode GetTypeCode<[EnumConstraint] TEnum>()
            where TEnum : struct => Enums<TEnum>.Info.TypeCode;
        #endregion

        #region Type Methods
        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members count.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns><typeparamref name="TEnum"/>'s members count.</returns>
        [Pure]
        public static int GetEnumMemberCount<[EnumConstraint] TEnum>()
            where TEnum : struct => Enums<TEnum>.Info.GetEnumMemberCount();

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members count.
        /// The parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns><typeparamref name="TEnum"/>'s members count.</returns>
        [Pure]
        public static int GetEnumMemberCount<[EnumConstraint] TEnum>(bool uniqueValued)
            where TEnum : struct => Enums<TEnum>.Info.GetEnumMemberCount(uniqueValued);

        /// <summary>
        /// Retrieves in value order an array of info on <typeparamref name="TEnum"/>'s members.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        [Pure]
        public static IEnumerable<EnumMember<TEnum>> GetEnumMembers<[EnumConstraint] TEnum>()
            where TEnum : struct => Enums<TEnum>.Info.GetEnumMembers();

        /// <summary>
        /// Retrieves in value order an array of info on <typeparamref name="TEnum"/>'s members.
        /// The parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns></returns>
        [Pure]
        public static IEnumerable<EnumMember<TEnum>> GetEnumMembers<[EnumConstraint] TEnum>(bool uniqueValued)
            where TEnum : struct => Enums<TEnum>.Info.GetEnumMembers(uniqueValued);

        /// <summary>
        /// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' names.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>Array of <typeparamref name="TEnum"/>'s members' names in value order.</returns>
        [Pure]
        public static IEnumerable<string> GetNames<[EnumConstraint] TEnum>()
            where TEnum : struct => Enums<TEnum>.Info.GetNames();

        /// <summary>
        /// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' names.
        /// The parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <typeparamref name="TEnum"/>'s members' names in value order.</returns>
        [Pure]
        public static IEnumerable<string> GetNames<[EnumConstraint] TEnum>(bool uniqueValued)
            where TEnum : struct => Enums<TEnum>.Info.GetNames(uniqueValued);

        /// <summary>
        /// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>Array of <typeparamref name="TEnum"/>'s members' values in value order.</returns>
        [Pure]
        public static IEnumerable<TEnum> GetValues<[EnumConstraint] TEnum>()
            where TEnum : struct => Enums<TEnum>.Info.GetValues();

        /// <summary>
        /// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' values.
        /// The parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <typeparamref name="TEnum"/>'s members' values in value order.</returns>
        [Pure]
        public static IEnumerable<TEnum> GetValues<[EnumConstraint] TEnum>(bool uniqueValued)
            where TEnum : struct => Enums<TEnum>.Info.GetValues(uniqueValued);
        #endregion

        #region IsValid
        /// <summary>
        /// Indicates whether <paramref name="value"/> is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values. If <typeparamref name="TEnum"/> has an attribute
        /// that implements <see cref="IEnumValidatorAttribute{TEnum}"/> then that attribute's validation logic is used instead.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Indication whether <paramref name="value"/> is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values. If <typeparamref name="TEnum"/> has an attribute
        /// that implements <see cref="IEnumValidatorAttribute{TEnum}"/> then that attribute's validation logic is used instead.</returns>
        [Pure]
        public static bool IsValid<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.IsValid(value);
        #endregion

        #region IsDefined
        /// <summary>
        /// Indicates whether <paramref name="value"/> is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Indication whether <paramref name="value"/> is defined.</returns>
        [Pure]
        public static bool IsDefined<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.IsDefined(value);
        #endregion

        #region ToObject
        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that the result is within the
        /// underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/>.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not a valid type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(object value)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that the result is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/>.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not a valid type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(object value, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<[EnumConstraint] TEnum>(sbyte value)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<[EnumConstraint] TEnum>(sbyte value, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(byte value)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(byte value, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(short value)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(short value, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<[EnumConstraint] TEnum>(ushort value)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<[EnumConstraint] TEnum>(ushort value, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(int value)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(int value, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<[EnumConstraint] TEnum>(uint value)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<[EnumConstraint] TEnum>(uint value, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(long value)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(long value, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<[EnumConstraint] TEnum>(ulong value)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<[EnumConstraint] TEnum>(ulong value, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.ToObject(value, validate);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(object value, TEnum defaultValue)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(object value, TEnum defaultValue, bool validate)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(sbyte value, TEnum defaultValue)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(sbyte value, TEnum defaultValue, bool validate)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(byte value, TEnum defaultValue)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(byte value, TEnum defaultValue, bool validate)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(short value, TEnum defaultValue)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(short value, TEnum defaultValue, bool validate)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(ushort value, TEnum defaultValue)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(ushort value, TEnum defaultValue, bool validate)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(int value, TEnum defaultValue)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(int value, TEnum defaultValue, bool validate)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(uint value, TEnum defaultValue)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(uint value, TEnum defaultValue, bool validate)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(long value, TEnum defaultValue)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(long value, TEnum defaultValue, bool validate)
            where TEnum : struct
        {
            TEnum result;
            return TryToObject(value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(ulong value, TEnum defaultValue)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryToObject(value, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultValue">The fallback value to return.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultValue"/>.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(ulong value, TEnum defaultValue, bool validate)
            where TEnum : struct
        {
            TEnum result;
            return TryToObject(value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is returned in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(object value, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is returned in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="result"></param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(object value, out TEnum result, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<[EnumConstraint] TEnum>(sbyte value, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<[EnumConstraint] TEnum>(sbyte value, out TEnum result, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(byte value, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(byte value, out TEnum result, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(short value, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(short value, out TEnum result, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<[EnumConstraint] TEnum>(ushort value, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<[EnumConstraint] TEnum>(ushort value, out TEnum result, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(int value, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(int value, out TEnum result, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<[EnumConstraint] TEnum>(uint value, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<[EnumConstraint] TEnum>(uint value, out TEnum result, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(long value, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(long value, out TEnum result, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<[EnumConstraint] TEnum>(ulong value, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<[EnumConstraint] TEnum>(ulong value, out TEnum result, bool validate)
            where TEnum : struct => Enums<TEnum>.Info.TryToObject(value, out result, validate);
        #endregion

        #region All Values Main Methods
        /// <summary>
        /// Validates that <paramref name="value"/> is valid. If it's not it throws an <see cref="ArgumentException"/> with the given <paramref name="paramName"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <returns><paramref name="value"/> for use in constructor initializers and fluent API's</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is invalid.</exception>
        [Pure]
        public static TEnum Validate<[EnumConstraint] TEnum>(TEnum value, string paramName)
            where TEnum : struct => Enums<TEnum>.Info.Validate(value, paramName);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        public static string AsString<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.AsString(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        [Pure]
        public static string AsString<[EnumConstraint] TEnum>(this TEnum value, string format)
            where TEnum : struct => Enums<TEnum>.Info.AsString(value, format);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="format"/> is an invalid value.</exception>
        [Pure]
        public static string AsString<[EnumConstraint] TEnum>(this TEnum value, EnumFormat format)
            where TEnum : struct => Enums<TEnum>.Info.AsString(value, format);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation in the specified format order.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="format0"></param>
        /// <param name="format1"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        [Pure]
        public static string AsString<[EnumConstraint] TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1)
            where TEnum : struct => Enums<TEnum>.Info.AsString(value, format0, format1);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation in the specified format order.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="format0"></param>
        /// <param name="format1"></param>
        /// <param name="format2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        [Pure]
        public static string AsString<[EnumConstraint] TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2)
            where TEnum : struct => Enums<TEnum>.Info.AsString(value, format0, format1, format2);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="formatOrder"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="formatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="formatOrder"/> contains an invalid value.</exception>
        [Pure]
        public static string AsString<[EnumConstraint] TEnum>(this TEnum value, params EnumFormat[] formatOrder)
            where TEnum : struct => Enums<TEnum>.Info.AsString(value, formatOrder);

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
        public static string Format<[EnumConstraint] TEnum>(TEnum value, string format)
            where TEnum : struct => Enums<TEnum>.Info.Format(value, format);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="formatOrder"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="formatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="formatOrder"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="formatOrder"/> contains an invalid value.</exception>
        [Pure]
        public static string Format<[EnumConstraint] TEnum>(TEnum value, params EnumFormat[] formatOrder)
            where TEnum : struct => Enums<TEnum>.Info.Format(value, formatOrder);

        /// <summary>
        /// Returns an object with the enum's underlying value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        public static object GetUnderlyingValue<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.GetUnderlyingValue(value);

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="sbyte"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="sbyte"/>'s value range without overflowing.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static sbyte ToSByte<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.ToSByte(value);

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="byte"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="byte"/>'s value range without overflowing.</exception>
        [Pure]
        public static byte ToByte<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.ToByte(value);

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="short"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="short"/>'s value range without overflowing.</exception>
        [Pure]
        public static short ToInt16<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.ToInt16(value);

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="ushort"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ushort"/>'s value range without overflowing.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static ushort ToUInt16<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.ToUInt16(value);

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="int"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="int"/>'s value range without overflowing.</exception>
        [Pure]
        public static int ToInt32<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.ToInt32(value);

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="uint"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="uint"/>'s value range without overflowing.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static uint ToUInt32<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.ToUInt32(value);

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="long"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="long"/>'s value range without overflowing.</exception>
        [Pure]
        public static long ToInt64<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.ToInt64(value);

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="ulong"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ulong"/>'s value range without overflowing.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static ulong ToUInt64<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.ToUInt64(value);

        /// <summary>
        /// A more efficient GetHashCode method as it doesn't require boxing and unboxing of <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        public static int GetHashCode<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.GetHashCode(value);

        /// <summary>
        /// Indicates if the two values are equal to each other
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        [Pure]
        public static bool Equals<[EnumConstraint] TEnum>(TEnum value, TEnum other)
            where TEnum : struct => Enums<TEnum>.Info.Equals(value, other);

        /// <summary>
        /// Compares two <typeparamref name="TEnum"/>'s for ordering.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns>1 if <paramref name="value"/> is greater than <paramref name="other"/>, 0 if <paramref name="value"/> equals <paramref name="other"/>,
        /// and -1 if <paramref name="value"/> is less than <paramref name="other"/>.</returns>
        [Pure]
        public static int CompareTo<[EnumConstraint] TEnum>(TEnum value, TEnum other)
            where TEnum : struct => Enums<TEnum>.Info.CompareTo(value, other);
        #endregion

        #region Defined Values Main Methods
        /// <summary>
        /// Gets the enum member info, which consists of the name, value, and attributes, with the given <paramref name="value"/>.
        /// If no enum member exists with the given <paramref name="value"/> null is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        public static EnumMember<TEnum> GetEnumMember<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.GetEnumMember(value);

        /// <summary>
        /// Gets the enum member info, which consists of the name, value, and attributes, with the given <paramref name="name"/>.
        /// If no enum member exists with the given <paramref name="name"/> null is returned.
        /// Is case-sensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        [Pure]
        public static EnumMember<TEnum> GetEnumMember<[EnumConstraint] TEnum>(string name)
            where TEnum : struct => Enums<TEnum>.Info.GetEnumMember(name);

        /// <summary>
        /// Gets the enum member info, which consists of the name, value, and attributes, with the given <paramref name="name"/>.
        /// If no enum member exists with the given <paramref name="name"/> null is returned.
        /// The parameter <paramref name="ignoreCase"/> indicates if the name comparison should ignore the casing.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="name"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        [Pure]
        public static EnumMember<TEnum> GetEnumMember<[EnumConstraint] TEnum>(string name, bool ignoreCase)
            where TEnum : struct => Enums<TEnum>.Info.GetEnumMember(name, ignoreCase);

        /// <summary>
        /// Retrieves the name of the constant in <typeparamref name="TEnum"/> that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined null is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Name of the constant in <typeparamref name="TEnum"/> that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined null is returned.</returns>
        [Pure]
        public static string GetName<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.GetName(value);

        /// <summary>
        /// Retrieves the description of the constant in the enumeration that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined or no associated <see cref="DescriptionAttribute"/> is found then null is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Description of the constant in the enumeration that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined or no associated <see cref="DescriptionAttribute"/> is found then null is returned.</returns>
        [Pure]
        public static string GetDescription<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.GetDescription(value);

        /// <summary>
        /// Retrieves the description if not null else the name of the specified <paramref name="value"/> if defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        public static string GetDescriptionOrName<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.GetDescriptionOrName(value);

        /// <summary>
        /// Retrieves the description if not null else the name formatted with <paramref name="nameFormatter"/> of the specified <paramref name="value"/> if defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="nameFormatter"></param>
        /// <returns></returns>
        public static string GetDescriptionOrName<[EnumConstraint] TEnum>(TEnum value, Func<string, string> nameFormatter)
            where TEnum : struct => Enums<TEnum>.Info.GetDescriptionOrName(value, nameFormatter);
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
        public static bool HasAttribute<[EnumConstraint] TEnum, TAttribute>(TEnum value)
            where TAttribute : Attribute
            where TEnum : struct => GetAttribute<TEnum, TAttribute>(value) != null;

        /// <summary>
        /// Retrieves the <typeparamref name="TAttribute"/> if it exists of the enumerated constant with the specified <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="value"></param>
        /// <returns><typeparamref name="TAttribute"/> of the enumerated constant with the specified <paramref name="value"/> if defined and has attribute, else null</returns>
        [Pure]
        public static TAttribute GetAttribute<[EnumConstraint] TEnum, TAttribute>(TEnum value)
            where TAttribute : Attribute
            where TEnum : struct => Enums<TEnum>.Info.GetAttribute<TAttribute>(value);

        /// <summary>
        /// Retrieves the <typeparamref name="TAttribute"/> if it exists of the enumerated constant with the specified <paramref name="value"/>
        /// and then applies the <paramref name="selector"/> else it returns <c>default{TResult}</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is null.</exception>
        [Pure]
        public static TResult GetAttributeSelect<[EnumConstraint] TEnum, TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector)
            where TAttribute : Attribute
            where TEnum : struct => Enums<TEnum>.Info.GetAttributeSelect(value, selector);

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
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is null.</exception>
        [Pure]
        public static TResult GetAttributeSelect<[EnumConstraint] TEnum, TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, TResult defaultValue)
            where TAttribute : Attribute
            where TEnum : struct => Enums<TEnum>.Info.GetAttributeSelect(value, selector, defaultValue);

        /// <summary>
        /// Retrieves an array of <typeparamref name="TAttribute"/>'s of the constant in the enumeration that has the specified <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="value"></param>
        /// <returns><typeparamref name="TAttribute"/> array</returns>
        [Pure]
        public static IEnumerable<TAttribute> GetAttributes<[EnumConstraint] TEnum, TAttribute>(TEnum value)
            where TAttribute : Attribute
            where TEnum : struct => Enums<TEnum>.Info.GetAttributes<TAttribute>(value);

        /// <summary>
        /// Retrieves an array of all the <see cref="Attribute"/>'s of the constant in the enumeration that has the specified <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns><see cref="Attribute"/> array if value is defined, else null</returns>
        [Pure]
        public static IEnumerable<Attribute> GetAttributes<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct => Enums<TEnum>.Info.GetAttributes(value);
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
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value)
            where TEnum : struct => Enums<TEnum>.Info.Parse(value);

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
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
        /// -or-
        /// <paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => Enums<TEnum>.Info.Parse(value, false, parseFormatOrder);

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
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value, bool ignoreCase)
            where TEnum : struct => Enums<TEnum>.Info.Parse(value, ignoreCase);

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
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
        /// -or-
        /// <paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value, bool ignoreCase, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => Enums<TEnum>.Info.Parse(value, ignoreCase, parseFormatOrder);

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified default enumerated value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, TEnum defaultValue)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryParse(value, false, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, TEnum defaultValue, params EnumFormat[] parseFormatOrder)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryParse(value, false, out result, parseFormatOrder) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified default enumerated value.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, TEnum defaultValue)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryParse(value, ignoreCase, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated value. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultValue"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, TEnum defaultValue, params EnumFormat[] parseFormatOrder)
            where TEnum : struct
        {
            TEnum result;
            return Enums<TEnum>.Info.TryParse(value, ignoreCase, out result, parseFormatOrder) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryParse(value, false, out result);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, out TEnum result, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => Enums<TEnum>.Info.TryParse(value, false, out result, parseFormatOrder);

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
        public static bool TryParse<[EnumConstraint] TEnum>(string value, bool ignoreCase, out TEnum result)
            where TEnum : struct => Enums<TEnum>.Info.TryParse(value, ignoreCase, out result);

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
        /// <exception cref="ArgumentException"><paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, bool ignoreCase, out TEnum result, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => Enums<TEnum>.Info.TryParse(value, ignoreCase, out result, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// to an equivalent enumerated member object.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static EnumMember<TEnum> ParseMember<[EnumConstraint] TEnum>(string value)
            where TEnum : struct => Enums<TEnum>.Info.ParseMember(value);

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
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
        /// -or-
        /// <paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static EnumMember<TEnum> ParseMember<[EnumConstraint] TEnum>(string value, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => Enums<TEnum>.Info.ParseMember(value, false, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// to an equivalent enumerated member object. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static EnumMember<TEnum> ParseMember<[EnumConstraint] TEnum>(string value, bool ignoreCase)
            where TEnum : struct => Enums<TEnum>.Info.ParseMember(value, ignoreCase);

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
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration
        /// -or-
        /// <paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static EnumMember<TEnum> ParseMember<[EnumConstraint] TEnum>(string value, bool ignoreCase, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => Enums<TEnum>.Info.ParseMember(value, ignoreCase, parseFormatOrder);

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated member object but if it fails returns the specified default enumerated value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [Pure]
        public static EnumMember<TEnum> ParseMemberOrDefault<[EnumConstraint] TEnum>(string value, EnumMember<TEnum> defaultValue)
            where TEnum : struct
        {
            EnumMember<TEnum> result;
            return Enums<TEnum>.Info.TryParseMember(value, false, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated member value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        [Pure]
        public static EnumMember<TEnum> ParseMemberOrDefault<[EnumConstraint] TEnum>(string value, EnumMember<TEnum> defaultValue, params EnumFormat[] parseFormatOrder)
            where TEnum : struct
        {
            EnumMember<TEnum> result;
            return Enums<TEnum>.Info.TryParseMember(value, false, out result, parseFormatOrder) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated member object but if it fails returns the specified default enumerated member value.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [Pure]
        public static EnumMember<TEnum> ParseMemberOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, EnumMember<TEnum> defaultValue)
            where TEnum : struct
        {
            EnumMember<TEnum> result;
            return Enums<TEnum>.Info.TryParseMember(value, ignoreCase, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated member value. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultValue"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        [Pure]
        public static EnumMember<TEnum> ParseMemberOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, EnumMember<TEnum> defaultValue, params EnumFormat[] parseFormatOrder)
            where TEnum : struct
        {
            EnumMember<TEnum> result;
            return Enums<TEnum>.Info.TryParseMember(value, ignoreCase, out result, parseFormatOrder) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated member object. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        public static bool TryParseMember<[EnumConstraint] TEnum>(string value, out EnumMember<TEnum> result)
            where TEnum : struct => Enums<TEnum>.Info.TryParseMember(value, false, out result);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        [Pure]
        public static bool TryParseMember<[EnumConstraint] TEnum>(string value, out EnumMember<TEnum> result, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => Enums<TEnum>.Info.TryParseMember(value, false, out result, parseFormatOrder);

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated member object. The return value indicates whether the conversion succeeded.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        public static bool TryParseMember<[EnumConstraint] TEnum>(string value, bool ignoreCase, out EnumMember<TEnum> result)
            where TEnum : struct => Enums<TEnum>.Info.TryParseMember(value, ignoreCase, out result);

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
        /// <exception cref="ArgumentException"><paramref name="parseFormatOrder"/> contains an invalid value.</exception>
        [Pure]
        public static bool TryParseMember<[EnumConstraint] TEnum>(string value, bool ignoreCase, out EnumMember<TEnum> result, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => Enums<TEnum>.Info.TryParseMember(value, ignoreCase, out result, parseFormatOrder);
        #endregion

        #region Internal Methods
        internal static Func<EnumMember, string> GetCustomEnumMemberFormatter(EnumFormat format)
        {
            var index = (int)format - StartingCustomEnumFormatValue;
            return index >= 0 && index < _customEnumMemberFormatters?.Count ? _customEnumMemberFormatters[index] : null;
        }

        internal static object GetEnumInfo(Type enumType)
        {
            var underlyingType = Enum.GetUnderlyingType(enumType);
            var numericProviderType = GetNumericProviderType(underlyingType);
            return Activator.CreateInstance(typeof(EnumInfo<,,>).MakeGenericType(enumType, underlyingType, numericProviderType));
        }

        internal static Type GetNumericProviderType(Type underlyingType)
        {
            switch (Type.GetTypeCode(underlyingType))
            {
                case TypeCode.SByte:
                    return typeof(SByteNumericProvider);
                case TypeCode.Byte:
                    return typeof(ByteNumericProvider);
                case TypeCode.Int16:
                    return typeof(Int16NumericProvider);
                case TypeCode.UInt16:
                    return typeof(UInt16NumericProvider);
                case TypeCode.Int32:
                    return typeof(Int32NumericProvider);
                case TypeCode.UInt32:
                    return typeof(UInt32NumericProvider);
                case TypeCode.Int64:
                    return typeof(Int64NumericProvider);
                case TypeCode.UInt64:
                    return typeof(UInt64NumericProvider);
            }
            throw new NotSupportedException($"Enum underlying type of {underlyingType} is not supported");
        }
        #endregion
    }

    internal static class Enums<TEnum>
    {
        internal static readonly IEnumInfo<TEnum> Info = (IEnumInfo<TEnum>)Enums.GetEnumInfo(typeof(TEnum));
    }
}