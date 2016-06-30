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

#if NET20 || NET35 || NETSTANDARD10
using EnumsNET.Collections;
#else
using System.Collections.Concurrent;
#endif

namespace EnumsNET.NonGeneric
{
    /// <summary>
    /// A non-generic implementation of the static class <see cref="Enums"/>, sort of a superset of .NET's built-in <see cref="Enum"/> class.
    /// When the type is known at compile-time the <see cref="Enums"/> class should be used instead, to provide type safety and to avoid boxing.
    /// </summary>
    public static class NonGenericEnums
    {
#if NET20 || NET35 || NETSTANDARD10
        private static readonly ThreadSafeDictionary<Type, EnumInfoAndIsNullable> _enumInfosDictionary = new ThreadSafeDictionary<Type, EnumInfoAndIsNullable>();
#else
        private static readonly ConcurrentDictionary<Type, EnumInfoAndIsNullable> _enumInfosDictionary = new ConcurrentDictionary<Type, EnumInfoAndIsNullable>();
#endif

        internal static EnumInfoAndIsNullable GetInfo(Type enumType)
        {
            Preconditions.NotNull(enumType, nameof(enumType));
            
            EnumInfoAndIsNullable info;
            if (!_enumInfosDictionary.TryGetValue(enumType, out info))
            {
                info = new EnumInfoAndIsNullable();
                if (enumType.IsEnum)
                {
                    var underlyingType = Enum.GetUnderlyingType(enumType);
                    var numericProviderType = Enums.GetNumericProviderType(underlyingType);
                    info.EnumInfo = (IEnumInfo)Activator.CreateInstance(typeof(NonGenericEnumInfo<,,>).MakeGenericType(enumType, underlyingType, numericProviderType));
                }
                else
                {
                    var nonNullableEnumType = Nullable.GetUnderlyingType(enumType);
                    if (nonNullableEnumType?.IsEnum != true)
                    {
                        throw new ArgumentException("must be an enum type", nameof(enumType));
                    }
                    info.EnumInfo = GetInfo(nonNullableEnumType).EnumInfo;
                    info.IsNullable = true;
                }
                if (!_enumInfosDictionary.TryAdd(enumType, info))
                {
                    info = _enumInfosDictionary[enumType];
                }
            }
            return info;
        }

        #region "Properties"
        /// <summary>
        /// Indicates if <paramref name="enumType"/> is contiguous.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns>Indication if <paramref name="enumType"/> is contiguous.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsContiguous(Type enumType) => GetInfo(enumType).EnumInfo.IsContiguous;

        /// <summary>
        /// Retrieves the underlying type of <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns>The underlying type of <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static Type GetUnderlyingType(Type enumType) => GetInfo(enumType).EnumInfo.UnderlyingType;

        /// <summary>
        /// Gets <paramref name="enumType"/>'s underlying type's <see cref="TypeCode"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static TypeCode GetTypeCode(Type enumType) => GetInfo(enumType).EnumInfo.TypeCode;
        #endregion

        #region Type Methods
        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s count of defined constants.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns><paramref name="enumType"/>'s count of defined constants.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static int GetDefinedCount(Type enumType) => GetInfo(enumType).EnumInfo.GetDefinedCount();

        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s count of defined constants.
        /// The parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="uniqueValued"></param>
        /// <returns><paramref name="enumType"/>'s count of defined constants.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static int GetDefinedCount(Type enumType, bool uniqueValued) => GetInfo(enumType).EnumInfo.GetDefinedCount(uniqueValued);

        /// <summary>
        /// Retrieves in value order an array of info on <paramref name="enumType"/>'s members.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<EnumMember> GetEnumMembers(Type enumType) => GetInfo(enumType).EnumInfo.GetEnumMembers();

        /// <summary>
        /// Retrieves in value order an array of info on <paramref name="enumType"/>'s members.
        /// The parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="uniqueValued"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<EnumMember> GetEnumMembers(Type enumType, bool uniqueValued) => GetInfo(enumType).EnumInfo.GetEnumMembers(uniqueValued);

        /// <summary>
        /// Retrieves an array of <paramref name="enumType"/>'s defined constants' names in value order.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns>Array of <paramref name="enumType"/>'s defined constants' names in value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<string> GetNames(Type enumType) => GetInfo(enumType).EnumInfo.GetNames();

        /// <summary>
        /// Retrieves an array of <paramref name="enumType"/>'s defined constants' names in value order.
        /// The parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <paramref name="enumType"/>'s defined constants' names in value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<string> GetNames(Type enumType, bool uniqueValued) => GetInfo(enumType).EnumInfo.GetNames(uniqueValued);

        /// <summary>
        /// Retrieves an array of <paramref name="enumType"/> defined constants' values in value order.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns>Array of <paramref name="enumType"/> defined constants' values in value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<object> GetValues(Type enumType) => GetInfo(enumType).EnumInfo.GetValues();

        /// <summary>
        /// Retrieves an array of <paramref name="enumType"/> defined constants' values in value order.
        /// The parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <paramref name="enumType"/> defined constants' values in value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<object> GetValues(Type enumType, bool uniqueValued) => GetInfo(enumType).EnumInfo.GetValues(uniqueValued);

        /// <summary>
        /// Returns -1 if <paramref name="x"/> is less than <paramref name="y"/> and returns 0 if <paramref name="x"/> equals <paramref name="y"/>
        /// and returns 1 if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="x"/>, or <paramref name="y"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="x"/> is not or cannot be converted to enum of <paramref name="enumType"/>
        /// -or-
        /// <paramref name="y"/> is not or cannot be converted to enum of <paramref name="enumType"/></exception>
        [Pure]
        public static int Compare(Type enumType, object x, object y)
        {
            var info = GetInfo(enumType);
            var enumInfo = info.EnumInfo;

            if (info.IsNullable)
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        return 0;
                    }
                    enumInfo.ToObject(y);
                    return -1;
                }
                if (y == null)
                {
                    enumInfo.ToObject(x);
                    return 1;
                }
            }

            return enumInfo.Compare(x, y);
        }

        /// <summary>
        /// Registers a custom enum format for <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static EnumFormat RegisterCustomEnumFormat(Type enumType, Func<EnumMember, string> formatter) => GetInfo(enumType).EnumInfo.RegisterCustomEnumFormat(formatter);
        #endregion

        #region IsValid
        /// <summary>
        /// Indicates whether <paramref name="value"/> is defined or if <paramref name="enumType"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <paramref name="enumType"/>'s defined values.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">must be an enum value, <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <returns>Indication whether the specified <paramref name="value"/> is defined or if <paramref name="enumType"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <paramref name="enumType"/>'s defined values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not or cannot be converted to enum of <paramref name="enumType"/></exception>
        [Pure]
        public static bool IsValid(Type enumType, object value)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return true;
            }

            return info.EnumInfo.IsValid(value);
        }

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsValid(Type enumType, sbyte value) => GetInfo(enumType).EnumInfo.IsValid(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid(Type enumType, byte value) => GetInfo(enumType).EnumInfo.IsValid(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid(Type enumType, short value) => GetInfo(enumType).EnumInfo.IsValid(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsValid(Type enumType, ushort value) => GetInfo(enumType).EnumInfo.IsValid(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid(Type enumType, int value) => GetInfo(enumType).EnumInfo.IsValid(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsValid(Type enumType, uint value) => GetInfo(enumType).EnumInfo.IsValid(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid(Type enumType, long value) => GetInfo(enumType).EnumInfo.IsValid(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsValid(Type enumType, ulong value) => GetInfo(enumType).EnumInfo.IsValid(value);
        #endregion

        #region IsDefined
        /// <summary>
        /// Indicates whether <paramref name="value"/> is defined in <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">must be an enum value, <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <returns>Indication whether <paramref name="value"/> is defined in <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not or cannot be converted to enum of <paramref name="enumType"/></exception>
        [Pure]
        public static bool IsDefined(Type enumType, object value)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return false;
            }

            return info.EnumInfo.IsDefined(value);
        }

        /// <summary>
        /// Indicates whether a constant with the specified <paramref name="name"/> exists in <paramref name="enumType"/>.
        /// Is case-sensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="name"></param>
        /// <returns>Indication whether a constant with the specified <paramref name="name"/> exists in <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="name"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined(Type enumType, string name) => GetInfo(enumType).EnumInfo.IsDefined(name);

        /// <summary>
        /// Indicates whether a constant with the specified <paramref name="name"/> exists in <paramref name="enumType"/>.
        /// <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="name"></param>
        /// <param name="ignoreCase">Specifies whether the operation is case-insensitive.</param>
        /// <returns>Indication whether a constant with the specified <paramref name="name"/> exists in <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="name"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined(Type enumType, string name, bool ignoreCase) => GetInfo(enumType).EnumInfo.IsDefined(name, ignoreCase);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsDefined(Type enumType, sbyte value) => GetInfo(enumType).EnumInfo.IsDefined(value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined(Type enumType, byte value) => GetInfo(enumType).EnumInfo.IsDefined(value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined(Type enumType, short value) => GetInfo(enumType).EnumInfo.IsDefined(value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsDefined(Type enumType, ushort value) => GetInfo(enumType).EnumInfo.IsDefined(value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined(Type enumType, int value) => GetInfo(enumType).EnumInfo.IsDefined(value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsDefined(Type enumType, uint value) => GetInfo(enumType).EnumInfo.IsDefined(value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined(Type enumType, long value) => GetInfo(enumType).EnumInfo.IsDefined(value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsDefined(Type enumType, ulong value) => GetInfo(enumType).EnumInfo.IsDefined(value);
        #endregion

        #region IsInValueRange
        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsInValueRange(Type enumType, sbyte value) => GetInfo(enumType).EnumInfo.IsInValueRange(value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsInValueRange(Type enumType, byte value) => GetInfo(enumType).EnumInfo.IsInValueRange(value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsInValueRange(Type enumType, short value) => GetInfo(enumType).EnumInfo.IsInValueRange(value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsInValueRange(Type enumType, ushort value) => GetInfo(enumType).EnumInfo.IsInValueRange(value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsInValueRange(Type enumType, int value) => GetInfo(enumType).EnumInfo.IsInValueRange(value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsInValueRange(Type enumType, uint value) => GetInfo(enumType).EnumInfo.IsInValueRange(value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsInValueRange(Type enumType, long value) => GetInfo(enumType).EnumInfo.IsInValueRange(value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsInValueRange(Type enumType, ulong value) => GetInfo(enumType).EnumInfo.IsInValueRange(value);
        #endregion

        #region ToObject
        /// <summary>
        /// Converts the specified <paramref name="value"/> to an enumeration member while checking that the result is within the
        /// underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not a valid type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static object ToObject(Type enumType, object value) => ToObject(enumType, value, false);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to an enumeration member while checking that the result is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not a valid type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static object ToObject(Type enumType, object value, bool validate)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.ToObject(value, validate);
        }

        /// <summary>
        /// Converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, sbyte value) => GetInfo(enumType).EnumInfo.ToObject(value);

        /// <summary>
        /// Converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, sbyte value, bool validate) => GetInfo(enumType).EnumInfo.ToObject(value, validate);

        /// <summary>
        /// Converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static object ToObject(Type enumType, byte value) => GetInfo(enumType).EnumInfo.ToObject(value);

        /// <summary>
        /// Converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static object ToObject(Type enumType, byte value, bool validate) => GetInfo(enumType).EnumInfo.ToObject(value, validate);

        /// <summary>
        /// Converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static object ToObject(Type enumType, short value) => GetInfo(enumType).EnumInfo.ToObject(value);

        /// <summary>
        /// Converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static object ToObject(Type enumType, short value, bool validate) => GetInfo(enumType).EnumInfo.ToObject(value, validate);

        /// <summary>
        /// Converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ushort value) => GetInfo(enumType).EnumInfo.ToObject(value);

        /// <summary>
        /// Converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ushort value, bool validate) => GetInfo(enumType).EnumInfo.ToObject(value, validate);

        /// <summary>
        /// Converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static object ToObject(Type enumType, int value) => GetInfo(enumType).EnumInfo.ToObject(value);

        /// <summary>
        /// Converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static object ToObject(Type enumType, int value, bool validate) => GetInfo(enumType).EnumInfo.ToObject(value, validate);

        /// <summary>
        /// Converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, uint value) => GetInfo(enumType).EnumInfo.ToObject(value);

        /// <summary>
        /// Converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, uint value, bool validate) => GetInfo(enumType).EnumInfo.ToObject(value, validate);

        /// <summary>
        /// Converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static object ToObject(Type enumType, long value) => GetInfo(enumType).EnumInfo.ToObject(value);

        /// <summary>
        /// Converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        public static object ToObject(Type enumType, long value, bool validate) => GetInfo(enumType).EnumInfo.ToObject(value, validate);

        /// <summary>
        /// Converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ulong value) => GetInfo(enumType).EnumInfo.ToObject(value);

        /// <summary>
        /// Converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObject(Type enumType, ulong value, bool validate) => GetInfo(enumType).EnumInfo.ToObject(value, validate);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, object value, object defaultValue)
        {
            object result;
            return TryToObject(enumType, value, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, object value, object defaultValue, bool validate)
        {
            object result;
            return TryToObject(enumType, value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObjectOrDefault(Type enumType, sbyte value, object defaultValue) => ToObjectOrDefault(enumType, (long)value, defaultValue);

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObjectOrDefault(Type enumType, sbyte value, object defaultValue, bool validate) => ToObjectOrDefault(enumType, (long)value, defaultValue, validate);

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, byte value, object defaultValue) => ToObjectOrDefault(enumType, (ulong)value, defaultValue);

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, byte value, object defaultValue, bool validate) => ToObjectOrDefault(enumType, (ulong)value, defaultValue, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, short value, object defaultValue) => ToObjectOrDefault(enumType, (long)value, defaultValue);

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, short value, object defaultValue, bool validate) => ToObjectOrDefault(enumType, (long)value, defaultValue, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObjectOrDefault(Type enumType, ushort value, object defaultValue) => ToObjectOrDefault(enumType, (ulong)value, defaultValue);

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObjectOrDefault(Type enumType, ushort value, object defaultValue, bool validate) => ToObjectOrDefault(enumType, (ulong)value, defaultValue, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, int value, object defaultValue) => ToObjectOrDefault(enumType, (long)value, defaultValue);

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, int value, object defaultValue, bool validate) => ToObjectOrDefault(enumType, (long)value, defaultValue, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObjectOrDefault(Type enumType, uint value, object defaultValue) => ToObjectOrDefault(enumType, (ulong)value, defaultValue);

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObjectOrDefault(Type enumType, uint value, object defaultValue, bool validate) => ToObjectOrDefault(enumType, (ulong)value, defaultValue, validate);

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, long value, object defaultValue)
        {
            object result;
            return TryToObject(enumType, value, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, long value, object defaultValue, bool validate)
        {
            object result;
            return TryToObject(enumType, value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. If it fails <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObjectOrDefault(Type enumType, ulong value, object defaultValue)
        {
            object result;
            return TryToObject(enumType, value, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObjectOrDefault(Type enumType, ulong value, object defaultValue, bool validate)
        {
            object result;
            return TryToObject(enumType, value, out result, validate) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to an enumeration member while checking that the result is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject(Type enumType, object value, out object result) => TryToObject(enumType, value, out result, false);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to an enumeration member while checking that the result is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject(Type enumType, object value, out object result, bool validate)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                result = null;
                return true;
            }

            return info.EnumInfo.TryToObject(value, out result, validate);
        }

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, sbyte value, out object result) => GetInfo(enumType).EnumInfo.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, sbyte value, out object result, bool validate) => GetInfo(enumType).EnumInfo.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject(Type enumType, byte value, out object result) => GetInfo(enumType).EnumInfo.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject(Type enumType, byte value, out object result, bool validate) => GetInfo(enumType).EnumInfo.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject(Type enumType, short value, out object result) => GetInfo(enumType).EnumInfo.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject(Type enumType, short value, out object result, bool validate) => GetInfo(enumType).EnumInfo.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ushort value, out object result) => GetInfo(enumType).EnumInfo.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ushort value, out object result, bool validate) => GetInfo(enumType).EnumInfo.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject(Type enumType, int value, out object result) => GetInfo(enumType).EnumInfo.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject(Type enumType, int value, out object result, bool validate) => GetInfo(enumType).EnumInfo.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, uint value, out object result) => GetInfo(enumType).EnumInfo.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, uint value, out object result, bool validate) => GetInfo(enumType).EnumInfo.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject(Type enumType, long value, out object result) => GetInfo(enumType).EnumInfo.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryToObject(Type enumType, long value, out object result, bool validate) => GetInfo(enumType).EnumInfo.TryToObject(value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. An indication if the operation succeeded is returned and the result of the operation or if it fails
        /// the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ulong value, out object result) => GetInfo(enumType).EnumInfo.TryToObject(value, out result);

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject(Type enumType, ulong value, out object result, bool validate) => GetInfo(enumType).EnumInfo.TryToObject(value, out result, validate);
        #endregion

        #region All Values Main Methods
        /// <summary>
        /// Validates that <paramref name="value"/> is valid. If it's not it throws an <see cref="ArgumentException"/> with the given <paramref name="paramName"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <returns><paramref name="value"/> for use in constructor initializers and fluent API's</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        [Pure]
        public static object Validate(Type enumType, object value, string paramName)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.Validate(value, paramName);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        [Pure]
        public static string AsString(Type enumType, object value)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.AsString(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value</exception>
        [Pure]
        public static string AsString(Type enumType, object value, string format)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.AsString(value, format);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="formatOrder"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="formatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static string AsString(Type enumType, object value, params EnumFormat[] formatOrder)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.AsString(value, formatOrder);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="format"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        [Pure]
        public static string Format(Type enumType, object value, string format)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.Format(value, format);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        [Pure]
        public static string Format(Type enumType, object value, EnumFormat format)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.Format(value, format);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation in the specified format order.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="format0"></param>
        /// <param name="format1"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        [Pure]
        public static string Format(Type enumType, object value, EnumFormat format0, EnumFormat format1)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.Format(value, format0, format1);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation in the specified format order.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="format0"></param>
        /// <param name="format1"></param>
        /// <param name="format2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        [Pure]
        public static string Format(Type enumType, object value, EnumFormat format0, EnumFormat format1, EnumFormat format2)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.Format(value, format0, format1, format2);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="formatOrder"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="formatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/>, <paramref name="value"/>, or <paramref name="formatOrder"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid.</exception>
        [Pure]
        public static string Format(Type enumType, object value, params EnumFormat[] formatOrder)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.Format(value, formatOrder);
        }

        /// <summary>
        /// Returns an object with the enum's underlying value.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        [Pure]
        public static object GetUnderlyingValue(Type enumType, object value)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.GetUnderlyingValue(value);
        }

        /// <summary>
        /// Tries to convert <paramref name="value"/> to an <see cref="sbyte"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="sbyte"/>'s value range without overflowing</exception>
        [Pure]
        [CLSCompliant(false)]
        public static sbyte ToSByte(Type enumType, object value) => GetInfo(enumType).EnumInfo.ToSByte(value);

        /// <summary>
        /// Tries to convert <paramref name="value"/> to a <see cref="byte"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="byte"/>'s value range without overflowing</exception>
        [Pure]
        public static byte ToByte(Type enumType, object value) => GetInfo(enumType).EnumInfo.ToByte(value);

        /// <summary>
        /// Tries to convert <paramref name="value"/> to an <see cref="short"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="short"/>'s value range without overflowing</exception>
        [Pure]
        public static short ToInt16(Type enumType, object value) => GetInfo(enumType).EnumInfo.ToInt16(value);

        /// <summary>
        /// Tries to convert <paramref name="value"/> to a <see cref="ushort"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ushort"/>'s value range without overflowing</exception>
        [Pure]
        [CLSCompliant(false)]
        public static ushort ToUInt16(Type enumType, object value) => GetInfo(enumType).EnumInfo.ToUInt16(value);

        /// <summary>
        /// Tries to convert <paramref name="value"/> to an <see cref="int"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="int"/>'s value range without overflowing</exception>
        [Pure]
        public static int ToInt32(Type enumType, object value) => GetInfo(enumType).EnumInfo.ToInt32(value);

        /// <summary>
        /// Tries to convert <paramref name="value"/> to a <see cref="uint"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="uint"/>'s value range without overflowing</exception>
        [Pure]
        [CLSCompliant(false)]
        public static uint ToUInt32(Type enumType, object value) => GetInfo(enumType).EnumInfo.ToUInt32(value);

        /// <summary>
        /// Tries to convert <paramref name="value"/> to an <see cref="long"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="long"/>'s value range without overflowing</exception>
        [Pure]
        public static long ToInt64(Type enumType, object value) => GetInfo(enumType).EnumInfo.ToInt64(value);

        /// <summary>
        /// Tries to convert <paramref name="value"/> to a <see cref="ulong"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ulong"/>'s value range without overflowing</exception>
        [Pure]
        [CLSCompliant(false)]
        public static ulong ToUInt64(Type enumType, object value) => GetInfo(enumType).EnumInfo.ToUInt64(value);

        /// <summary>
        /// Indicates if the two values are equal to each other
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [Pure]
        public static bool Equals(Type enumType, object x, object y)
        {
            var info = GetInfo(enumType);
            var enumInfo = info.EnumInfo;

            if (info.IsNullable)
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        return true;
                    }
                    enumInfo.ToObject(y);
                    return false;
                }
                if (y == null)
                {
                    enumInfo.ToObject(x);
                    return false;
                }
            }

            return enumInfo.Equals(x, y);
        }
        #endregion

        #region Defined Values Main Methods
        /// <summary>
        /// Gets the enum member info, which consists of the name, value, and attributes, with the given <paramref name="value"/>.
        /// If no enum member exists with the given <paramref name="value"/> null is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        [Pure]
        public static EnumMember GetEnumMember(Type enumType, object value)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.GetEnumMember(value);
        }

        /// <summary>
        /// Gets the enum member info, which consists of the name, value, and attributes, with the given <paramref name="name"/>.
        /// If no enum member exists with the given <paramref name="name"/> null is returned. Is case-sensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        [Pure]
        public static EnumMember GetEnumMember(Type enumType, string name) => GetInfo(enumType).EnumInfo.GetEnumMember(name);

        /// <summary>
        /// Gets the enum member info, which consists of the name, value, and attributes, with the given <paramref name="name"/>.
        /// If no enum member exists with the given <paramref name="name"/> null is returned.
        /// The parameter <paramref name="ignoreCase"/> indicates if the name comparison should ignore the casing.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="name"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="name"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        [Pure]
        public static EnumMember GetEnumMember(Type enumType, string name, bool ignoreCase) => GetInfo(enumType).EnumInfo.GetEnumMember(name, ignoreCase);

        /// <summary>
        /// Retrieves the name of the constant in <paramref name="enumType"/> that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined null is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Name of the constant in <paramref name="enumType"/> that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined null is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        [Pure]
        public static string GetName(Type enumType, object value)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.GetName(value);
        }

        /// <summary>
        /// Retrieves the description of the constant in the enumeration that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined or no associated <see cref="DescriptionAttribute"/> is found then null is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Description of the constant in the enumeration that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined or no associated <see cref="DescriptionAttribute"/> is found then null is returned.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        [Pure]
        public static string GetDescription(Type enumType, object value)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.GetDescription(value);
        }

        /// <summary>
        /// Retrieves the description if not null else the name of the specified <paramref name="value"/> if defined.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        [Pure]
        public static string GetDescriptionOrName(Type enumType, object value)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.GetDescriptionOrName(value);
        }

        /// <summary>
        /// Retrieves the description if not null else the name formatted with <paramref name="nameFormatter"/> of the specified <paramref name="value"/> if defined.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="nameFormatter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        [Pure]
        public static string GetDescriptionOrName(Type enumType, object value, Func<string, string> nameFormatter)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.GetDescriptionOrName(value, nameFormatter);
        }
        #endregion

        #region Attributes
        /// <summary>
        /// Retrieves an array of all the <see cref="Attribute"/>'s of the constant in the enumeration that has the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns><see cref="Attribute"/> array if value is defined, else null</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid</exception>
        [Pure]
        public static IEnumerable<Attribute> GetAttributes(Type enumType, object value)
        {
            var info = GetInfo(enumType);

            if (value == null && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.GetAttributes(value);
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
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/></exception>
        [Pure]
        public static object Parse(Type enumType, string value) => Parse(enumType, value, false, null);

        /// <summary>
        /// Converts the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/></exception>
        [Pure]
        public static object Parse(Type enumType, string value, params EnumFormat[] parseFormatOrder) => Parse(enumType, value, false, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/></exception>
        [Pure]
        public static object Parse(Type enumType, string value, bool ignoreCase) => Parse(enumType, value, ignoreCase, null);

        /// <summary>
        /// Converts the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/></exception>
        [Pure]
        public static object Parse(Type enumType, string value, bool ignoreCase, params EnumFormat[] parseFormatOrder)
        {
            var info = GetInfo(enumType);

            if (string.IsNullOrEmpty(value) && info.IsNullable)
            {
                return null;
            }

            return info.EnumInfo.Parse(value, ignoreCase, parseFormatOrder);
        }

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified default enumerated value.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ParseOrDefault(Type enumType, string value, object defaultValue) => ParseOrDefault(enumType, value, false, defaultValue, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated value.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ParseOrDefault(Type enumType, string value, object defaultValue, params EnumFormat[] parseFormatOrder) => ParseOrDefault(enumType, value, false, defaultValue, parseFormatOrder);

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified default enumerated value.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, object defaultValue) => ParseOrDefault(enumType, value, ignoreCase, defaultValue, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated value. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultValue"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, object defaultValue, params EnumFormat[] parseFormatOrder)
        {
            object result;
            return TryParse(enumType, value, ignoreCase, out result, parseFormatOrder) ? result : defaultValue;
        }

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryParse(Type enumType, string value, out object result) => TryParse(enumType, value, false, out result, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryParse(Type enumType, string value, out object result, params EnumFormat[] parseFormatOrder) => TryParse(enumType, value, false, out result, parseFormatOrder);

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryParse(Type enumType, string value, bool ignoreCase, out object result) => TryParse(enumType, value, ignoreCase, out result, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// The return value indicates whether the conversion succeeded. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="result"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool TryParse(Type enumType, string value, bool ignoreCase, out object result, params EnumFormat[] parseFormatOrder)
        {
            var info = GetInfo(enumType);

            if (string.IsNullOrEmpty(value) && info.IsNullable)
            {
                result = null;
                return true;
            }

            return info.EnumInfo.TryParse(value, ignoreCase, out result, parseFormatOrder);
        }
        #endregion
    }
}
