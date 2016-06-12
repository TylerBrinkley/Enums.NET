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

namespace EnumsNET.Unsafe
{
    /// <summary>
    /// Identical to <see cref="Enums"/> but is not type safe which is useful when dealing with generics
    /// and instead throws an <see cref="ArgumentException"/> if TEnum is not an enum/>
    /// </summary>
    public static class UnsafeEnums
    {
        #region "Properties"
        /// <summary>
        /// Indicates if <typeparamref name="TEnum"/>'s defined values are contiguous.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns>Indication if <typeparamref name="TEnum"/>'s defined values are contiguous.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsContiguous<TEnum>()
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.IsContiguous;
        }

        /// <summary>
        /// Retrieves the underlying type of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns>The underlying type of <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static Type GetUnderlyingType<TEnum>()
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.UnderlyingType;
        }

        /// <summary>
        ///  Gets <typeparamref name="TEnum"/>'s underlying type's <see cref="TypeCode"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TypeCode GetTypeCode<TEnum>()
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.TypeCode;
        }
        #endregion

        #region Type Methods
        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s count of defined constants.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns><typeparamref name="TEnum"/>'s count of defined constants.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static int GetDefinedCount<TEnum>(bool uniqueValued = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetDefinedCount(uniqueValued);
        }

        /// <summary>
        /// Retrieves in value order an array of info on <typeparamref name="TEnum"/>'s members.
        /// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<EnumMember<TEnum>> GetEnumMembers<TEnum>(bool uniqueValued = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetEnumMembers(uniqueValued);
        }

        /// <summary>
        /// Retrieves an array of <typeparamref name="TEnum"/>'s defined constants' names in value order.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <typeparamref name="TEnum"/>'s defined constants' names in value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<string> GetNames<TEnum>(bool uniqueValued = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetNames(uniqueValued);
        }

        /// <summary>
        /// Retrieves an array of <typeparamref name="TEnum"/> defined constants' values in value order.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <typeparamref name="TEnum"/> defined constants' values in value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<TEnum> GetValues<TEnum>(bool uniqueValued = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetValues(uniqueValued);
        }

        /// <summary>
        /// Returns -1 if <paramref name="x"/> is less than <paramref name="y"/> and returns 0 if <paramref name="x"/> equals <paramref name="y"/>
        /// and returns 1 if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static int Compare<TEnum>(TEnum x, TEnum y)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.Compare(x, y);
        }

        /// <summary>
        /// Registers a custom enum format for <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="formatter"></param>
        /// <returns></returns>
        /// /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static EnumFormat RegisterCustomEnumFormat<TEnum>(Func<EnumMember<TEnum>, string> formatter)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.RegisterCustomEnumFormat(formatter);
        }
        #endregion

        #region IsValid
        /// <summary>
        /// Indicates whether <paramref name="value"/> is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid<TEnum>(object value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.IsValid(value);
        }

        /// <summary>
        /// Indicates whether <paramref name="value"/> is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether the specified <paramref name="value"/> is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.IsValid(value);
        }

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsValid<TEnum>(sbyte value) => IsValid<TEnum>((long)value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid<TEnum>(byte value) => IsValid<TEnum>((ulong)value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid<TEnum>(short value) => IsValid<TEnum>((long)value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsValid<TEnum>(ushort value) => IsValid<TEnum>((ulong)value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid<TEnum>(int value) => IsValid<TEnum>((long)value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsValid<TEnum>(uint value) => IsValid<TEnum>((ulong)value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid<TEnum>(long value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.IsValid(value);
        }

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsValid<TEnum>(ulong value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.IsValid(value);
        }
        #endregion

        #region IsDefined
        /// <summary>
        /// Indicates whether <paramref name="value"/> is defined in <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined<TEnum>(object value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.IsDefined(value);
        }

        /// <summary>
        /// Indicates whether <paramref name="value"/> is defined in <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> is defined in <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.IsDefined(value);
        }

        /// <summary>
        /// Indicates whether a constant with the specified <paramref name="name"/> exists in <typeparamref name="TEnum"/>.
        /// <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="name"></param>
        /// <param name="ignoreCase">Specifies whether the operation is case-insensitive.</param>
        /// <returns>Indication whether a constant with the specified <paramref name="name"/> exists in <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        [Pure]
        public static bool IsDefined<TEnum>(string name, bool ignoreCase = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.IsDefined(name, ignoreCase);
        }

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsDefined<TEnum>(sbyte value) => IsDefined<TEnum>((long)value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined<TEnum>(byte value) => IsDefined<TEnum>((ulong)value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined<TEnum>(short value) => IsDefined<TEnum>((long)value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsDefined<TEnum>(ushort value) => IsDefined<TEnum>((ulong)value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined<TEnum>(int value) => IsDefined<TEnum>((long)value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsDefined<TEnum>(uint value) => IsDefined<TEnum>((ulong)value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined<TEnum>(long value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.IsDefined(value);
        }

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsDefined<TEnum>(ulong value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.IsDefined(value);
        }
        #endregion

        #region IsInValueRange
        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsInValueRange<TEnum>(sbyte value) => IsInValueRange<TEnum>((long)value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsInValueRange<TEnum>(byte value) => IsInValueRange<TEnum>((ulong)value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsInValueRange<TEnum>(short value) => IsInValueRange<TEnum>((long)value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsInValueRange<TEnum>(ushort value) => IsInValueRange<TEnum>((ulong)value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsInValueRange<TEnum>(int value) => IsInValueRange<TEnum>((long)value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsInValueRange<TEnum>(uint value) => IsInValueRange<TEnum>((ulong)value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsInValueRange<TEnum>(long value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.IsInValueRange(value);
        }

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsInValueRange<TEnum>(ulong value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.IsInValueRange(value);
        }
        #endregion

        #region ToObject
        /// <summary>
        /// Converts the specified <paramref name="value"/> to an enumeration member while checking that the result is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not a valid type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static TEnum ToObject<TEnum>(object value, bool validate = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.ToObject(value, validate);
        }

        /// <summary>
        /// Converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(sbyte value, bool validate = false) => ToObject<TEnum>((long)value, validate);

        /// <summary>
        /// Converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static TEnum ToObject<TEnum>(byte value, bool validate = false) => ToObject<TEnum>((ulong)value, validate);

        /// <summary>
        /// Converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static TEnum ToObject<TEnum>(short value, bool validate = false) => ToObject<TEnum>((long)value, validate);

        /// <summary>
        /// Converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(ushort value, bool validate = false) => ToObject<TEnum>((ulong)value, validate);

        /// <summary>
        /// Converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static TEnum ToObject<TEnum>(int value, bool validate = false) => ToObject<TEnum>((long)value, validate);

        /// <summary>
        /// Converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(uint value, bool validate = false) => ToObject<TEnum>((ulong)value, validate);

        /// <summary>
        /// Converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static TEnum ToObject<TEnum>(long value, bool validate = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.ToObject(value, validate);
        }

        /// <summary>
        /// Converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(ulong value, bool validate = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.ToObject(value, validate);
        }

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ToObjectOrDefault<TEnum>(object value, TEnum defaultValue, bool validate = false)
        {
            TEnum result;
            return TryToObject(value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<TEnum>(sbyte value, TEnum defaultValue, bool validate = false) => ToObjectOrDefault((long)value, defaultValue, validate);

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ToObjectOrDefault<TEnum>(byte value, TEnum defaultValue, bool validate = false) => ToObjectOrDefault((ulong)value, defaultValue, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ToObjectOrDefault<TEnum>(short value, TEnum defaultValue, bool validate = false) => ToObjectOrDefault((long)value, defaultValue, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<TEnum>(ushort value, TEnum defaultValue, bool validate = false) => ToObjectOrDefault((ulong)value, defaultValue, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ToObjectOrDefault<TEnum>(int value, TEnum defaultValue, bool validate = false) => ToObjectOrDefault((long)value, defaultValue, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<TEnum>(uint value, TEnum defaultValue, bool validate = false) => ToObjectOrDefault((ulong)value, defaultValue, validate);

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ToObjectOrDefault<TEnum>(long value, TEnum defaultValue, bool validate = false)
        {
            TEnum result;
            return TryToObject(value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<TEnum>(ulong value, TEnum defaultValue, bool validate = false)
        {
            TEnum result;
            return TryToObject(value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to an enumeration member while checking that the result is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject<TEnum>(object value, out TEnum result, bool validate = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.TryToObject(value, out result, validate);
        }

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(sbyte value, out TEnum result, bool validate = false) => TryToObject((long)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject<TEnum>(byte value, out TEnum result, bool validate = false) => TryToObject((ulong)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject<TEnum>(short value, out TEnum result, bool validate = false) => TryToObject((long)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(ushort value, out TEnum result, bool validate = false) => TryToObject((ulong)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject<TEnum>(int value, out TEnum result, bool validate = false) => TryToObject((long)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(uint value, out TEnum result, bool validate = false) => TryToObject((ulong)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject<TEnum>(long value, out TEnum result, bool validate = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.TryToObject(value, out result, validate);
        }

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(ulong value, out TEnum result, bool validate = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.TryToObject(value, out result, validate);
        }
        #endregion

        #region All Values Main Methods
        /// <summary>
        /// Validates that <paramref name="value"/> is valid. If it's not it throws an <see cref="ArgumentException"/> with the given <paramref name="paramName"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <returns><paramref name="value"/> for use in constructor initializers and fluent API's</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        [Pure]
        public static TEnum Validate<TEnum>(TEnum value, string paramName)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.Validate(value, paramName);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static string AsString<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.AsString(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value</exception>
        [Pure]
        public static string AsString<TEnum>(TEnum value, string format)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.AsString(value, format);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="formatOrder"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="formatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static string AsString<TEnum>(TEnum value, params EnumFormat[] formatOrder)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.AsString(value, formatOrder);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        [Pure]
        public static string Format<TEnum>(TEnum value, string format)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.Format(value, format);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static string Format<TEnum>(TEnum value, EnumFormat format)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.Format(value, format);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation in the specified format order.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="format0"></param>
        /// <param name="format1"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static string Format<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.Format(value, format0, format1);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation in the specified format order.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="format0"></param>
        /// <param name="format1"></param>
        /// <param name="format2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static string Format<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.Format(value, format0, format1, format2);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="formatOrder"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="formatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="formatOrder"/> is null.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Pure]
        public static string Format<TEnum>(TEnum value, params EnumFormat[] formatOrder)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.Format(value, formatOrder);
        }

        /// <summary>
        /// Returns an object with the enum's underlying value.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static object GetUnderlyingValue<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetUnderlyingValue(value);
        }

        /// <summary>
        /// Tries to convert <paramref name="value"/> to an <see cref="sbyte"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="sbyte"/>'s value range without overflowing</exception>
        [Pure]
        [CLSCompliant(false)]
        public static sbyte ToSByte<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.ToSByte(value);
        }

        /// <summary>
        /// Tries to convert <paramref name="value"/> to a <see cref="byte"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="byte"/>'s value range without overflowing</exception>
        [Pure]
        public static byte ToByte<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.ToByte(value);
        }

        /// <summary>
        /// Tries to convert <paramref name="value"/> to an <see cref="short"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="short"/>'s value range without overflowing</exception>
        [Pure]
        public static short ToInt16<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.ToInt16(value);
        }

        /// <summary>
        /// Tries to convert <paramref name="value"/> to a <see cref="ushort"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ushort"/>'s value range without overflowing</exception>
        [Pure]
        [CLSCompliant(false)]
        public static ushort ToUInt16<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.ToUInt16(value);
        }

        /// <summary>
        /// Tries to convert <paramref name="value"/> to an <see cref="int"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="int"/>'s value range without overflowing</exception>
        [Pure]
        public static int ToInt32<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.ToInt32(value);
        }

        /// <summary>
        /// Tries to convert <paramref name="value"/> to a <see cref="uint"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="uint"/>'s value range without overflowing</exception>
        [Pure]
        [CLSCompliant(false)]
        public static uint ToUInt32<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.ToUInt32(value);
        }

        /// <summary>
        /// Tries to convert <paramref name="value"/> to an <see cref="long"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="long"/>'s value range without overflowing</exception>
        [Pure]
        public static long ToInt64<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.ToInt64(value);
        }

        /// <summary>
        /// Tries to convert <paramref name="value"/> to a <see cref="ulong"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ulong"/>'s value range without overflowing</exception>
        [Pure]
        [CLSCompliant(false)]
        public static ulong ToUInt64<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.ToUInt64(value);
        }

        /// <summary>
        /// A more efficient GetHashCode method as it doesn't require boxing and unboxing of <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static int GetHashCode<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetHashCode(value);
        }

        /// <summary>
        /// Indicates if the two values are equal to each other
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool Equals<TEnum>(TEnum value, TEnum other)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.Equals(value, other);
        }
        #endregion

        #region Defined Values Main Methods
        /// <summary>
        /// Gets the enum member info, which consists of the name, value, and attributes, with the given <paramref name="value"/>.
        /// If no enum member exists with the given <paramref name="value"/> <see langref="null"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static EnumMember<TEnum> GetEnumMember<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetEnumMember(value);
        }

        /// <summary>
        /// Gets the enum member info, which consists of the name, value, and attributes, with the given <paramref name="name"/>.
        /// If no enum member exists with the given <paramref name="name"/> null is returned.
        /// The optional parameter <paramref name="ignoreCase"/> indicates if the name comparison should ignore the casing.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="name"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static EnumMember<TEnum> GetEnumMember<TEnum>(string name, bool ignoreCase = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetEnumMember(name, ignoreCase);
        }

        /// <summary>
        /// Retrieves the name of the constant in <typeparamref name="TEnum"/> that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined null is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns>Name of the constant in <typeparamref name="TEnum"/> that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined null is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static string GetName<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetName(value);
        }

        /// <summary>
        /// Retrieves the description of the constant in the enumeration that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined or no associated <see cref="DescriptionAttribute"/> is found then null is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns>Description of the constant in the enumeration that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined or no associated <see cref="DescriptionAttribute"/> is found then null is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static string GetDescription<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetDescription(value);
        }
        #endregion

        #region Attributes
        /// <summary>
        /// Indicates if the enumerated constant with the specified <paramref name="value"/> has a <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="value"></param>
        /// <returns>Indication if the enumerated constant with the specified <paramref name="value"/> has a <typeparamref name="TAttribute"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool HasAttribute<TEnum, TAttribute>(TEnum value)
            where TAttribute : Attribute => GetAttribute<TEnum, TAttribute>(value) != null;

        /// <summary>
        /// Retrieves the <typeparamref name="TAttribute"/> if it exists of the enumerated constant with the specified <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="value"></param>
        /// <returns><typeparamref name="TAttribute"/> of the enumerated constant with the specified <paramref name="value"/> if defined and has attribute, else null</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TAttribute GetAttribute<TEnum, TAttribute>(TEnum value)
            where TAttribute : Attribute
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetAttribute<TAttribute>(value);
        }

        /// <summary>
        /// Retrieves the <typeparamref name="TAttribute"/> if it exists of the enumerated constant with the specified <paramref name="value"/>
        /// and then applies the <paramref name="selector"/> else it returns <paramref name="defaultValue"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <param name="selector"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is null</exception>
        [Pure]
        public static TResult GetAttributeSelect<TEnum, TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, TResult defaultValue = default(TResult))
            where TAttribute : Attribute
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetAttributeSelect(value, selector, defaultValue);
        }

        /// <summary>
        /// Tries to retrieve the first <typeparamref name="TAttribute"/> if it exists of the enumeration constant with the specified <paramref name="value"/>
        /// and sets <paramref name="result"/> to the result of applying the <paramref name="selector"/> to the <typeparamref name="TAttribute"/>.
        /// Returns true if a <typeparamref name="TAttribute"/> is found else false.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <param name="selector"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is null</exception>
        [Pure]
        public static bool TryGetAttributeSelect<TEnum, TAttribute, TResult>(this TEnum value, Func<TAttribute, TResult> selector, out TResult result)
            where TAttribute : Attribute
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.TryGetAttributeSelect(value, selector, out result);
        }

        /// <summary>
        /// Retrieves an array of <typeparamref name="TAttribute"/>'s of the constant in the enumeration that has the specified <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="value"></param>
        /// <returns><typeparamref name="TAttribute"/> array</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<TAttribute> GetAttributes<TEnum, TAttribute>(TEnum value)
            where TAttribute : Attribute
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetAttributes<TAttribute>(value);
        }

        /// <summary>
        /// Retrieves an array of all the <see cref="Attribute"/>'s of the constant in the enumeration that has the specified <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns><see cref="Attribute"/> array if value is defined, else null</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<Attribute> GetAttributes<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.GetAttributes(value);
        }
        #endregion

        #region Parsing
        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/></exception>
        [Pure]
        public static TEnum Parse<TEnum>(string value) => Parse<TEnum>(value, false, null);

        /// <summary>
        /// Converts the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/></exception>
        [Pure]
        public static TEnum Parse<TEnum>(string value, params EnumFormat[] parseFormatOrder) => Parse<TEnum>(value, false, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/></exception>
        [Pure]
        public static TEnum Parse<TEnum>(string value, bool ignoreCase) => Parse<TEnum>(value, ignoreCase);

        /// <summary>
        /// Converts the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/></exception>
        [Pure]
        public static TEnum Parse<TEnum>(string value, bool ignoreCase, params EnumFormat[] parseFormatOrder)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.Parse(value, ignoreCase, parseFormatOrder);
        }

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified default enumerated value.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, TEnum defaultValue) => ParseOrDefault(value, false, defaultValue, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated value.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, TEnum defaultValue, params EnumFormat[] parseFormatOrder) => ParseOrDefault(value, false, defaultValue, parseFormatOrder);

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified default enumerated value.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, bool ignoreCase, TEnum defaultValue) => ParseOrDefault(value, ignoreCase, defaultValue, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated value. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultValue"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, bool ignoreCase, TEnum defaultValue, params EnumFormat[] parseFormatOrder)
        {
            TEnum result;
            return TryParse(value, ignoreCase, out result, parseFormatOrder) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool TryParse<TEnum>(string value, out TEnum result) => TryParse(value, false, out result, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool TryParse<TEnum>(string value, out TEnum result, params EnumFormat[] parseFormatOrder) => TryParse(value, false, out result, parseFormatOrder);

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) => TryParse(value, ignoreCase, out result, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// The return value indicates whether the conversion succeeded. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="result"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result, params EnumFormat[] parseFormatOrder)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.Info.TryParse(value, ignoreCase, out result, parseFormatOrder);
        }
        #endregion

        #region Internal Methods
        internal static void VerifyTypeIsEnum(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type argument TEnum must be an enum");
            }
        }
        #endregion
    }
}
