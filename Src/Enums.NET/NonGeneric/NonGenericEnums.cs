// Enums.NET
// Copyright 2016 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
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
using System.Diagnostics;
using System.Linq;

namespace EnumsNET.NonGeneric
{
    /// <summary>
    /// A non-generic implementation of the static class <see cref="Enums"/>, sort of a superset of .NET's built-in <see cref="Enum"/> class.
    /// When the type is known at compile-time the <see cref="Enums"/> class should be used instead, to provide type safety and to avoid boxing.
    /// </summary>
    public static class NonGenericEnums
    {
        #region "Properties"
        /// <summary>
        /// Indicates if <paramref name="enumType"/> is contiguous.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns>Indication if <paramref name="enumType"/> is contiguous.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsContiguous(Type enumType)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);

            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).IsContiguous;
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).IsContiguous;
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).IsContiguous;
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).IsContiguous;
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).IsContiguous;
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).IsContiguous;
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).IsContiguous;
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).IsContiguous;
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

        /// <summary>
        /// Retrieves the underlying type of <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns>The underlying type of <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static Type GetUnderlyingType(Type enumType)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);

            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return typeof(int);
                case TypeCode.UInt32:
                    return typeof(uint);
                case TypeCode.Int64:
                    return typeof(long);
                case TypeCode.UInt64:
                    return typeof(ulong);
                case TypeCode.SByte:
                    return typeof(sbyte);
                case TypeCode.Byte:
                    return typeof(byte);
                case TypeCode.Int16:
                    return typeof(short);
                case TypeCode.UInt16:
                    return typeof(ushort);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static TypeCode GetTypeCode(Type enumType) => NonGenericEnumsCache.Get(enumType).TypeCode;
        #endregion

        #region Type Methods
        /// <summary>
        /// Retrieves <paramref name="enumType"/>'s count of defined constants.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="uniqueValued"></param>
        /// <returns><paramref name="enumType"/>'s count of defined constants.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static int GetDefinedCount(Type enumType, bool uniqueValued = false)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);

            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetDefinedCount(uniqueValued);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetDefinedCount(uniqueValued);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetDefinedCount(uniqueValued);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetDefinedCount(uniqueValued);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetDefinedCount(uniqueValued);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetDefinedCount(uniqueValued);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetDefinedCount(uniqueValued);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetDefinedCount(uniqueValued);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

        [Pure]
        public static IEnumerable<EnumMemberInfo> GetEnumMemberInfos(Type enumType, bool uniqueValued = false)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            IEnumerable<IEnumMemberInfo> infos;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    infos = ((EnumsCache<int>)cache).GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.UInt32:
                    infos = ((EnumsCache<uint>)cache).GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.Int64:
                    infos = ((EnumsCache<long>)cache).GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.UInt64:
                    infos = ((EnumsCache<ulong>)cache).GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.SByte:
                    infos = ((EnumsCache<sbyte>)cache).GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.Byte:
                    infos = ((EnumsCache<byte>)cache).GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.Int16:
                    infos = ((EnumsCache<short>)cache).GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.UInt16:
                    infos = ((EnumsCache<ushort>)cache).GetEnumMemberInfos(uniqueValued);
                    break;
                default:
                    Debug.Fail("Unknown Enum TypeCode");
                    return null;
            }
            return infos.Select(info => new NonGenericEnumMemberInfo(info, enumsCache));
        }

        /// <summary>
        /// Retrieves an array of <paramref name="enumType"/>'s defined constants' names in value order.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <paramref name="enumType"/>'s defined constants' names in value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<string> GetNames(Type enumType, bool uniqueValued = false)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);

            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetNames(uniqueValued);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetNames(uniqueValued);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetNames(uniqueValued);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetNames(uniqueValued);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetNames(uniqueValued);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetNames(uniqueValued);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetNames(uniqueValued);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetNames(uniqueValued);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Retrieves an array of <paramref name="enumType"/> defined constants' values in value order.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <paramref name="enumType"/> defined constants' values in value order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<object> GetValues(Type enumType, bool uniqueValued = false)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);

            var cache = enumsCache.Cache;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32ToEnum = (Func<int, object>)toEnum;
                    return ((EnumsCache<int>)cache).GetValues(uniqueValued).Select(value => int32ToEnum(value));
                case TypeCode.UInt32:
                    var uint32ToEnum = (Func<uint, object>)toEnum;
                    return ((EnumsCache<uint>)cache).GetValues(uniqueValued).Select(value => uint32ToEnum(value));
                case TypeCode.Int64:
                    var int64ToEnum = (Func<long, object>)toEnum;
                    return ((EnumsCache<long>)cache).GetValues(uniqueValued).Select(value => int64ToEnum(value));
                case TypeCode.UInt64:
                    var uint64ToEnum = (Func<ulong, object>)toEnum;
                    return ((EnumsCache<ulong>)cache).GetValues(uniqueValued).Select(value => uint64ToEnum(value));
                case TypeCode.SByte:
                    var sbyteToEnum = (Func<sbyte, object>)toEnum;
                    return ((EnumsCache<sbyte>)cache).GetValues(uniqueValued).Select(value => sbyteToEnum(value));
                case TypeCode.Byte:
                    var byteToEnum = (Func<byte, object>)toEnum;
                    return ((EnumsCache<byte>)cache).GetValues(uniqueValued).Select(value => byteToEnum(value));
                case TypeCode.Int16:
                    var int16ToEnum = (Func<short, object>)toEnum;
                    return ((EnumsCache<short>)cache).GetValues(uniqueValued).Select(value => int16ToEnum(value));
                case TypeCode.UInt16:
                    var uint16ToEnum = (Func<ushort, object>)toEnum;
                    return ((EnumsCache<ushort>)cache).GetValues(uniqueValued).Select(value => uint16ToEnum(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

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
            var enumsCache = NonGenericEnumsCache.Get(enumType);

            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<object, int>)toInt;
                    return EnumsCache<int>.Compare(enumToInt32(x), enumToInt32(y));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<object, uint>)toInt;
                    return EnumsCache<uint>.Compare(enumToUInt32(x), enumToUInt32(y));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<object, long>)toInt;
                    return EnumsCache<long>.Compare(enumToInt64(x), enumToInt64(y));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<object, ulong>)toInt;
                    return EnumsCache<ulong>.Compare(enumToUInt64(x), enumToUInt64(y));
                case TypeCode.SByte:
                    var enumToSByte = (Func<object, sbyte>)toInt;
                    return EnumsCache<sbyte>.Compare(enumToSByte(x), enumToSByte(y));
                case TypeCode.Byte:
                    var enumToByte = (Func<object, byte>)toInt;
                    return EnumsCache<byte>.Compare(enumToByte(x), enumToByte(y));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<object, short>)toInt;
                    return EnumsCache<short>.Compare(enumToInt16(x), enumToInt16(y));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<object, ushort>)toInt;
                    return EnumsCache<ushort>.Compare(enumToUInt16(x), enumToUInt16(y));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
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
        public static EnumFormat RegisterCustomEnumFormat(Type enumType, Func<EnumMemberInfo, string> formatter)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            return enumsCache.RegisterCustomEnumFormat(formatter);
        }
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
            var enumsCache = NonGenericEnumsCache.Get(enumType);

            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).IsValid(value);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).IsValid(value);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).IsValid(value);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).IsValid(value);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).IsValid(value);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).IsValid(value);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).IsValid(value);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).IsValid(value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
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
        public static bool IsValid(Type enumType, sbyte value) => IsValid(enumType, (long)value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid(Type enumType, byte value) => IsValid(enumType, (ulong)value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid(Type enumType, short value) => IsValid(enumType, (long)value);

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
        public static bool IsValid(Type enumType, ushort value) => IsValid(enumType, (ulong)value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid(Type enumType, int value) => IsValid(enumType, (long)value);

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
        public static bool IsValid(Type enumType, uint value) => IsValid(enumType, (ulong)value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid for <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns>Indication if value is valid for <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsValid(Type enumType, long value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);

            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).IsValid(value);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).IsValid(value);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).IsValid(value);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).IsValid(value);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).IsValid(value);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).IsValid(value);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).IsValid(value);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).IsValid(value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
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
        public static bool IsValid(Type enumType, ulong value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);

            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).IsValid(value);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).IsValid(value);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).IsValid(value);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).IsValid(value);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).IsValid(value);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).IsValid(value);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).IsValid(value);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).IsValid(value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }
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
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).IsDefined(value);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).IsDefined(value);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).IsDefined(value);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).IsDefined(value);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).IsDefined(value);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).IsDefined(value);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).IsDefined(value);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).IsDefined(value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

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
        public static bool IsDefined(Type enumType, string name, bool ignoreCase = false)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).IsDefined(name, ignoreCase);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).IsDefined(name, ignoreCase);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).IsDefined(name, ignoreCase);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).IsDefined(name, ignoreCase);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).IsDefined(name, ignoreCase);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).IsDefined(name, ignoreCase);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).IsDefined(name, ignoreCase);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).IsDefined(name, ignoreCase);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

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
        public static bool IsDefined(Type enumType, sbyte value) => IsDefined(enumType, (long)value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined(Type enumType, byte value) => IsDefined(enumType, (ulong)value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined(Type enumType, short value) => IsDefined(enumType, (long)value);

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
        public static bool IsDefined(Type enumType, ushort value) => IsDefined(enumType, (ulong)value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined(Type enumType, int value) => IsDefined(enumType, (long)value);

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
        public static bool IsDefined(Type enumType, uint value) => IsDefined(enumType, (ulong)value);

        /// <summary>
        /// Returns an indication whether a constant with the specified <paramref name="value"/> exists in the enumeration.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsDefined(Type enumType, long value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).IsDefined(value);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).IsDefined(value);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).IsDefined(value);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).IsDefined(value);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).IsDefined(value);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).IsDefined(value);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).IsDefined(value);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).IsDefined(value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

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
        public static bool IsDefined(Type enumType, ulong value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).IsDefined(value);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).IsDefined(value);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).IsDefined(value);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).IsDefined(value);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).IsDefined(value);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).IsDefined(value);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).IsDefined(value);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).IsDefined(value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }
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
        public static bool IsInValueRange(Type enumType, sbyte value) => IsInValueRange(enumType, (long)value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsInValueRange(Type enumType, byte value) => IsInValueRange(enumType, (ulong)value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsInValueRange(Type enumType, short value) => IsInValueRange(enumType, (long)value);

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
        public static bool IsInValueRange(Type enumType, ushort value) => IsInValueRange(enumType, (ulong)value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsInValueRange(Type enumType, int value) => IsInValueRange(enumType, (long)value);

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
        public static bool IsInValueRange(Type enumType, uint value) => IsInValueRange(enumType, (ulong)value);

        /// <summary>
        /// Returns an indication whether the specified <paramref name="value"/> is within the underlying types value range.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static bool IsInValueRange(Type enumType, long value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.Int64IsInValueRange(value);
                case TypeCode.UInt32:
                    return EnumsCache<uint>.Int64IsInValueRange(value);
                case TypeCode.Int64:
                    return EnumsCache<long>.Int64IsInValueRange(value);
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.Int64IsInValueRange(value);
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.Int64IsInValueRange(value);
                case TypeCode.Byte:
                    return EnumsCache<byte>.Int64IsInValueRange(value);
                case TypeCode.Int16:
                    return EnumsCache<short>.Int64IsInValueRange(value);
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.Int64IsInValueRange(value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

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
        public static bool IsInValueRange(Type enumType, ulong value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.UInt64IsInValueRange(value);
                case TypeCode.UInt32:
                    return EnumsCache<uint>.UInt64IsInValueRange(value);
                case TypeCode.Int64:
                    return EnumsCache<long>.UInt64IsInValueRange(value);
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.UInt64IsInValueRange(value);
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.UInt64IsInValueRange(value);
                case TypeCode.Byte:
                    return EnumsCache<byte>.UInt64IsInValueRange(value);
                case TypeCode.Int16:
                    return EnumsCache<short>.UInt64IsInValueRange(value);
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.UInt64IsInValueRange(value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }
        #endregion

        #region ToObject
        /// <summary>
        /// Converts the specified <paramref name="value"/> to an enumeration member while checking that the result is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
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
        public static object ToObject(Type enumType, object value, bool validate = true)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).ToObject(value, validate));
                case TypeCode.UInt32:
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).ToObject(value, validate));
                case TypeCode.Int64:
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).ToObject(value, validate));
                case TypeCode.UInt64:
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).ToObject(value, validate));
                case TypeCode.SByte:
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).ToObject(value, validate));
                case TypeCode.Byte:
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).ToObject(value, validate));
                case TypeCode.Int16:
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).ToObject(value, validate));
                case TypeCode.UInt16:
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).ToObject(value, validate));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
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
        public static object ToObject(Type enumType, sbyte value, bool validate = true) => ToObject(enumType, (long)value, validate);

        /// <summary>
        /// Converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
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
        public static object ToObject(Type enumType, byte value, bool validate = true) => ToObject(enumType, (ulong)value, validate);

        /// <summary>
        /// Converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
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
        public static object ToObject(Type enumType, short value, bool validate = true) => ToObject(enumType, (long)value, validate);

        /// <summary>
        /// Converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
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
        public static object ToObject(Type enumType, ushort value, bool validate = true) => ToObject(enumType, (ulong)value, validate);

        /// <summary>
        /// Converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
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
        public static object ToObject(Type enumType, int value, bool validate = true) => ToObject(enumType, (long)value, validate);

        /// <summary>
        /// Converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
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
        public static object ToObject(Type enumType, uint value, bool validate = true) => ToObject(enumType, (ulong)value, validate);

        /// <summary>
        /// Converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
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
        public static object ToObject(Type enumType, long value, bool validate = true)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).ToObject(value, validate));
                case TypeCode.UInt32:
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).ToObject(value, validate));
                case TypeCode.Int64:
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).ToObject(value, validate));
                case TypeCode.UInt64:
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).ToObject(value, validate));
                case TypeCode.SByte:
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).ToObject(value, validate));
                case TypeCode.Byte:
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).ToObject(value, validate));
                case TypeCode.Int16:
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).ToObject(value, validate));
                case TypeCode.UInt16:
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).ToObject(value, validate));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
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
        public static object ToObject(Type enumType, ulong value, bool validate = true)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).ToObject(value, validate));
                case TypeCode.UInt32:
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).ToObject(value, validate));
                case TypeCode.Int64:
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).ToObject(value, validate));
                case TypeCode.UInt64:
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).ToObject(value, validate));
                case TypeCode.SByte:
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).ToObject(value, validate));
                case TypeCode.Byte:
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).ToObject(value, validate));
                case TypeCode.Int16:
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).ToObject(value, validate));
                case TypeCode.UInt16:
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).ToObject(value, validate));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, object value, object defaultEnum = null, bool validate = true)
        {
            object result;
            return TryToObject(enumType, value, out result, validate) ? result : defaultEnum;
        }

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObjectOrDefault(Type enumType, sbyte value, object defaultEnum = null, bool validate = true) => ToObjectOrDefault(enumType, (long)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, byte value, object defaultEnum = null, bool validate = true) => ToObjectOrDefault(enumType, (ulong)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, short value, object defaultEnum = null, bool validate = true) => ToObjectOrDefault(enumType, (long)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObjectOrDefault(Type enumType, ushort value, object defaultEnum = null, bool validate = true) => ToObjectOrDefault(enumType, (ulong)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, int value, object defaultEnum = null, bool validate = true) => ToObjectOrDefault(enumType, (long)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObjectOrDefault(Type enumType, uint value, object defaultEnum = null, bool validate = true) => ToObjectOrDefault(enumType, (ulong)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ToObjectOrDefault(Type enumType, long value, object defaultEnum = null, bool validate = true)
        {
            object result;
            return TryToObject(enumType, value, out result, validate) ? result : defaultEnum;
        }

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static object ToObjectOrDefault(Type enumType, ulong value, object defaultEnum = null, bool validate = true)
        {
            object result;
            return TryToObject(enumType, value, out result, validate) ? result : defaultEnum;
        }

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to an enumeration member while checking that the result is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
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
        public static bool TryToObject(Type enumType, object value, out object result, bool validate = true)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toEnum = enumsCache.ToEnum;
            bool success;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = ((EnumsCache<int>)cache).TryToObject(value, out resultAsInt32, validate);
                    result = ((Func<int, object>)toEnum)(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = ((EnumsCache<uint>)cache).TryToObject(value, out resultAsUInt32, validate);
                    result = ((Func<uint, object>)toEnum)(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = ((EnumsCache<long>)cache).TryToObject(value, out resultAsInt64, validate);
                    result = ((Func<long, object>)toEnum)(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = ((EnumsCache<ulong>)cache).TryToObject(value, out resultAsUInt64, validate);
                    result = ((Func<ulong, object>)toEnum)(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = ((EnumsCache<sbyte>)cache).TryToObject(value, out resultAsSByte, validate);
                    result = ((Func<sbyte, object>)toEnum)(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = ((EnumsCache<byte>)cache).TryToObject(value, out resultAsByte, validate);
                    result = ((Func<byte, object>)toEnum)(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = ((EnumsCache<short>)cache).TryToObject(value, out resultAsInt16, validate);
                    result = ((Func<short, object>)toEnum)(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = ((EnumsCache<ushort>)cache).TryToObject(value, out resultAsUInt16, validate);
                    result = ((Func<ushort, object>)toEnum)(resultAsUInt16);
                    return success;
            }
            Debug.Fail("Unknown Enum TypeCode");
            result = null;
            return false;
        }

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
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
        public static bool TryToObject(Type enumType, sbyte value, out object result, bool validate = true) => TryToObject(enumType, (long)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
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
        public static bool TryToObject(Type enumType, byte value, out object result, bool validate = true) => TryToObject(enumType, (ulong)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
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
        public static bool TryToObject(Type enumType, short value, out object result, bool validate = true) => TryToObject(enumType, (long)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
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
        public static bool TryToObject(Type enumType, ushort value, out object result, bool validate = true) => TryToObject(enumType, (ulong)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
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
        public static bool TryToObject(Type enumType, int value, out object result, bool validate = true) => TryToObject(enumType, (long)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
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
        public static bool TryToObject(Type enumType, uint value, out object result, bool validate = true) => TryToObject(enumType, (ulong)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
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
        public static bool TryToObject(Type enumType, long value, out object result, bool validate = true)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toEnum = enumsCache.ToEnum;
            bool success;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = ((EnumsCache<int>)cache).TryToObject(value, out resultAsInt32, validate);
                    result = ((Func<int, object>)toEnum)(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = ((EnumsCache<uint>)cache).TryToObject(value, out resultAsUInt32, validate);
                    result = ((Func<uint, object>)toEnum)(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = ((EnumsCache<long>)cache).TryToObject(value, out resultAsInt64, validate);
                    result = ((Func<long, object>)toEnum)(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = ((EnumsCache<ulong>)cache).TryToObject(value, out resultAsUInt64, validate);
                    result = ((Func<ulong, object>)toEnum)(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = ((EnumsCache<sbyte>)cache).TryToObject(value, out resultAsSByte, validate);
                    result = ((Func<sbyte, object>)toEnum)(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = ((EnumsCache<byte>)cache).TryToObject(value, out resultAsByte, validate);
                    result = ((Func<byte, object>)toEnum)(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = ((EnumsCache<short>)cache).TryToObject(value, out resultAsInt16, validate);
                    result = ((Func<short, object>)toEnum)(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = ((EnumsCache<ushort>)cache).TryToObject(value, out resultAsUInt16, validate);
                    result = ((Func<ushort, object>)toEnum)(resultAsUInt16);
                    return success;
            }
            Debug.Fail("Unknown Enum TypeCode");
            result = null;
            return false;
        }

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
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
        public static bool TryToObject(Type enumType, ulong value, out object result, bool validate = true)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toEnum = enumsCache.ToEnum;
            bool success;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = ((EnumsCache<int>)cache).TryToObject(value, out resultAsInt32, validate);
                    result = ((Func<int, object>)toEnum)(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = ((EnumsCache<uint>)cache).TryToObject(value, out resultAsUInt32, validate);
                    result = ((Func<uint, object>)toEnum)(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = ((EnumsCache<long>)cache).TryToObject(value, out resultAsInt64, validate);
                    result = ((Func<long, object>)toEnum)(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = ((EnumsCache<ulong>)cache).TryToObject(value, out resultAsUInt64, validate);
                    result = ((Func<ulong, object>)toEnum)(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = ((EnumsCache<sbyte>)cache).TryToObject(value, out resultAsSByte, validate);
                    result = ((Func<sbyte, object>)toEnum)(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = ((EnumsCache<byte>)cache).TryToObject(value, out resultAsByte, validate);
                    result = ((Func<byte, object>)toEnum)(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = ((EnumsCache<short>)cache).TryToObject(value, out resultAsInt16, validate);
                    result = ((Func<short, object>)toEnum)(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = ((EnumsCache<ushort>)cache).TryToObject(value, out resultAsUInt16, validate);
                    result = ((Func<ushort, object>)toEnum)(resultAsUInt16);
                    return success;
            }
            Debug.Fail("Unknown Enum TypeCode");
            result = null;
            return false;
        }
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
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    ((EnumsCache<int>)cache).Validate(((Func<object, int>)toInt)(value), paramName);
                    return value;
                case TypeCode.UInt32:
                    ((EnumsCache<uint>)cache).Validate(((Func<object, uint>)toInt)(value), paramName);
                    return value;
                case TypeCode.Int64:
                    ((EnumsCache<long>)cache).Validate(((Func<object, long>)toInt)(value), paramName);
                    return value;
                case TypeCode.UInt64:
                    ((EnumsCache<ulong>)cache).Validate(((Func<object, ulong>)toInt)(value), paramName);
                    return value;
                case TypeCode.SByte:
                    ((EnumsCache<sbyte>)cache).Validate(((Func<object, sbyte>)toInt)(value), paramName);
                    return value;
                case TypeCode.Byte:
                    ((EnumsCache<byte>)cache).Validate(((Func<object, byte>)toInt)(value), paramName);
                    return value;
                case TypeCode.Int16:
                    ((EnumsCache<short>)cache).Validate(((Func<object, short>)toInt)(value), paramName);
                    return value;
                case TypeCode.UInt16:
                    ((EnumsCache<ushort>)cache).Validate(((Func<object, ushort>)toInt)(value), paramName);
                    return value;
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).AsString(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).AsString(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).AsString(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).AsString(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).AsString(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).AsString(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).AsString(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).AsString(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).AsString(((Func<object, int>)toInt)(value), format);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).AsString(((Func<object, uint>)toInt)(value), format);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).AsString(((Func<object, long>)toInt)(value), format);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).AsString(((Func<object, ulong>)toInt)(value), format);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).AsString(((Func<object, sbyte>)toInt)(value), format);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).AsString(((Func<object, byte>)toInt)(value), format);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).AsString(((Func<object, short>)toInt)(value), format);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).AsString(((Func<object, ushort>)toInt)(value), format);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="formats"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static string AsString(Type enumType, object value, params EnumFormat[] formats)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).AsString(((Func<object, int>)toInt)(value), formats);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).AsString(((Func<object, uint>)toInt)(value), formats);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).AsString(((Func<object, long>)toInt)(value), formats);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).AsString(((Func<object, ulong>)toInt)(value), formats);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).AsString(((Func<object, sbyte>)toInt)(value), formats);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).AsString(((Func<object, byte>)toInt)(value), formats);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).AsString(((Func<object, short>)toInt)(value), formats);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).AsString(((Func<object, ushort>)toInt)(value), formats);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<object, int>)toInt)(value), format);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<object, uint>)toInt)(value), format);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<object, long>)toInt)(value), format);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<object, ulong>)toInt)(value), format);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<object, sbyte>)toInt)(value), format);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<object, byte>)toInt)(value), format);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<object, short>)toInt)(value), format);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<object, ushort>)toInt)(value), format);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format(Type enumType, object value, EnumFormat format)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<object, int>)toInt)(value), format);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<object, uint>)toInt)(value), format);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<object, long>)toInt)(value), format);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<object, ulong>)toInt)(value), format);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<object, sbyte>)toInt)(value), format);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<object, byte>)toInt)(value), format);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<object, short>)toInt)(value), format);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<object, ushort>)toInt)(value), format);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format(Type enumType, object value, EnumFormat format0, EnumFormat format1)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<object, int>)toInt)(value), format0, format1);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<object, uint>)toInt)(value), format0, format1);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<object, long>)toInt)(value), format0, format1);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<object, ulong>)toInt)(value), format0, format1);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<object, sbyte>)toInt)(value), format0, format1);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<object, byte>)toInt)(value), format0, format1);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<object, short>)toInt)(value), format0, format1);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<object, ushort>)toInt)(value), format0, format1);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format(Type enumType, object value, EnumFormat format0, EnumFormat format1, EnumFormat format2)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<object, int>)toInt)(value), format0, format1, format2);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<object, uint>)toInt)(value), format0, format1, format2);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<object, long>)toInt)(value), format0, format1, format2);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<object, ulong>)toInt)(value), format0, format1, format2);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<object, sbyte>)toInt)(value), format0, format1, format2);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<object, byte>)toInt)(value), format0, format1, format2);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<object, short>)toInt)(value), format0, format1, format2);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<object, ushort>)toInt)(value), format0, format1, format2);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format(Type enumType, object value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<object, int>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<object, uint>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<object, long>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<object, ulong>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<object, sbyte>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<object, byte>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<object, short>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<object, ushort>)toInt)(value), format0, format1, format2, format3);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format(Type enumType, object value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<object, int>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<object, uint>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<object, long>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<object, ulong>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<object, sbyte>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<object, byte>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<object, short>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<object, ushort>)toInt)(value), format0, format1, format2, format3, format4);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format(Type enumType, object value, params EnumFormat[] formats)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<object, int>)toInt)(value), formats);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<object, uint>)toInt)(value), formats);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<object, long>)toInt)(value), formats);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<object, ulong>)toInt)(value), formats);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<object, sbyte>)toInt)(value), formats);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<object, byte>)toInt)(value), formats);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<object, short>)toInt)(value), formats);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<object, ushort>)toInt)(value), formats);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<object, int>)toInt)(value);
                case TypeCode.UInt32:
                    return ((Func<object, uint>)toInt)(value);
                case TypeCode.Int64:
                    return ((Func<object, long>)toInt)(value);
                case TypeCode.UInt64:
                    return ((Func<object, ulong>)toInt)(value);
                case TypeCode.SByte:
                    return ((Func<object, sbyte>)toInt)(value);
                case TypeCode.Byte:
                    return ((Func<object, byte>)toInt)(value);
                case TypeCode.Int16:
                    return ((Func<object, short>)toInt)(value);
                case TypeCode.UInt16:
                    return ((Func<object, ushort>)toInt)(value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
        public static sbyte ToSByte(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToSByte(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToSByte(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToSByte(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToSByte(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToSByte(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToSByte(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToSByte(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToSByte(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

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
        public static byte ToByte(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToByte(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToByte(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToByte(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToByte(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToByte(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToByte(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToByte(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToByte(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

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
        public static short ToInt16(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToInt16(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToInt16(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToInt16(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToInt16(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToInt16(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToInt16(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToInt16(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToInt16(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

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
        public static ushort ToUInt16(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToUInt16(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToUInt16(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToUInt16(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToUInt16(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToUInt16(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToUInt16(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToUInt16(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToUInt16(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

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
        public static int ToInt32(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToInt32(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToInt32(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToInt32(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToInt32(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToInt32(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToInt32(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToInt32(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToInt32(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

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
        public static uint ToUInt32(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToUInt32(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToUInt32(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToUInt32(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToUInt32(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToUInt32(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToUInt32(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToUInt32(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToUInt32(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

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
        public static long ToInt64(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToInt64(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToInt64(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToInt64(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToInt64(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToInt64(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToInt64(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToInt64(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToInt64(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

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
        public static ulong ToUInt64(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToUInt64(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToUInt64(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToUInt64(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToUInt64(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToUInt64(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToUInt64(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToUInt64(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToUInt64(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

        [Pure]
        public static bool Equals(Type enumType, object x, object y)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);

            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<object, int>)toInt;
                    return enumToInt32(x) == enumToInt32(y);
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<object, uint>)toInt;
                    return enumToUInt32(x) == enumToUInt32(y);
                case TypeCode.Int64:
                    var enumToInt64 = (Func<object, long>)toInt;
                    return enumToInt64(x) == enumToInt64(y);
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<object, ulong>)toInt;
                    return enumToUInt64(x) == enumToUInt64(y);
                case TypeCode.SByte:
                    var enumToSByte = (Func<object, sbyte>)toInt;
                    return enumToSByte(x) == enumToSByte(y);
                case TypeCode.Byte:
                    var enumToByte = (Func<object, byte>)toInt;
                    return enumToByte(x) == enumToByte(y);
                case TypeCode.Int16:
                    var enumToInt16 = (Func<object, short>)toInt;
                    return enumToInt16(x) == enumToInt16(y);
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<object, ushort>)toInt;
                    return enumToUInt16(x) == enumToUInt16(y);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }
        #endregion

        #region Defined Values Main Methods
        [Pure]
        public static EnumMemberInfo GetEnumMemberInfo(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            IEnumMemberInfo info = null;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    info = ((EnumsCache<int>)cache).GetEnumMemberInfo(((Func<object, int>)toInt)(value));
                    break;
                case TypeCode.UInt32:
                    info = ((EnumsCache<uint>)cache).GetEnumMemberInfo(((Func<object, uint>)toInt)(value));
                    break;
                case TypeCode.Int64:
                    info = ((EnumsCache<long>)cache).GetEnumMemberInfo(((Func<object, long>)toInt)(value));
                    break;
                case TypeCode.UInt64:
                    info = ((EnumsCache<ulong>)cache).GetEnumMemberInfo(((Func<object, ulong>)toInt)(value));
                    break;
                case TypeCode.SByte:
                    info = ((EnumsCache<sbyte>)cache).GetEnumMemberInfo(((Func<object, sbyte>)toInt)(value));
                    break;
                case TypeCode.Byte:
                    info = ((EnumsCache<byte>)cache).GetEnumMemberInfo(((Func<object, byte>)toInt)(value));
                    break;
                case TypeCode.Int16:
                    info = ((EnumsCache<short>)cache).GetEnumMemberInfo(((Func<object, short>)toInt)(value));
                    break;
                case TypeCode.UInt16:
                    info = ((EnumsCache<ushort>)cache).GetEnumMemberInfo(((Func<object, ushort>)toInt)(value));
                    break;
                default:
                    Debug.Fail("Unknown Enum TypeCode");
                    return null;
            }
            return info.IsDefined ? new NonGenericEnumMemberInfo(info, enumsCache) : null;
        }

        [Pure]
        public static EnumMemberInfo GetEnumMemberInfo(Type enumType, string name, bool ignoreCase = false)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            IEnumMemberInfo info = null;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    info = ((EnumsCache<int>)cache).GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.UInt32:
                    info = ((EnumsCache<uint>)cache).GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.Int64:
                    info = ((EnumsCache<long>)cache).GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.UInt64:
                    info = ((EnumsCache<ulong>)cache).GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.SByte:
                    info = ((EnumsCache<sbyte>)cache).GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.Byte:
                    info = ((EnumsCache<byte>)cache).GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.Int16:
                    info = ((EnumsCache<short>)cache).GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.UInt16:
                    info = ((EnumsCache<ushort>)cache).GetEnumMemberInfo(name, ignoreCase);
                    break;
                default:
                    Debug.Fail("Unknown Enum TypeCode");
                    return null;
            }
            return info.IsDefined ? new NonGenericEnumMemberInfo(info, enumsCache) : null;
        }

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
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetName(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetName(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetName(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetName(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetName(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetName(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetName(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetName(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetDescription(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetDescription(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetDescription(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetDescription(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetDescription(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetDescription(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetDescription(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetDescription(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
        public static Attribute[] GetAttributes(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetAttributes(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetAttributes(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetAttributes(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetAttributes(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetAttributes(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetAttributes(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetAttributes(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetAttributes(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.UInt32:
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.Int64:
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.UInt64:
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.SByte:
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.Byte:
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.Int16:
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.UInt16:
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).Parse(value, ignoreCase, parseFormatOrder));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified default enumerated value.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ParseOrDefault(Type enumType, string value, object defaultEnum = null) => ParseOrDefault(enumType, value, false, defaultEnum, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated value.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ParseOrDefault(Type enumType, string value, object defaultEnum, params EnumFormat[] parseFormatOrder) => ParseOrDefault(enumType, value, false, defaultEnum, parseFormatOrder);

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified default enumerated value.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, object defaultEnum = null) => ParseOrDefault(enumType, value, ignoreCase, defaultEnum, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated value. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, object defaultEnum, params EnumFormat[] parseFormatOrder)
        {
            object result;
            return TryParse(enumType, value, ignoreCase, out result, parseFormatOrder) ? result : defaultEnum;
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
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toEnum = enumsCache.ToEnum;
            var success = false;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = ((EnumsCache<int>)cache).TryParse(value, ignoreCase, out resultAsInt32, parseFormatOrder);
                    result = ((Func<int, object>)toEnum)(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = ((EnumsCache<uint>)cache).TryParse(value, ignoreCase, out resultAsUInt32, parseFormatOrder);
                    result = ((Func<uint, object>)toEnum)(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = ((EnumsCache<long>)cache).TryParse(value, ignoreCase, out resultAsInt64, parseFormatOrder);
                    result = ((Func<long, object>)toEnum)(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = ((EnumsCache<ulong>)cache).TryParse(value, ignoreCase, out resultAsUInt64, parseFormatOrder);
                    result = ((Func<ulong, object>)toEnum)(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = ((EnumsCache<sbyte>)cache).TryParse(value, ignoreCase, out resultAsSByte, parseFormatOrder);
                    result = ((Func<sbyte, object>)toEnum)(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = ((EnumsCache<byte>)cache).TryParse(value, ignoreCase, out resultAsByte, parseFormatOrder);
                    result = ((Func<byte, object>)toEnum)(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = ((EnumsCache<short>)cache).TryParse(value, ignoreCase, out resultAsInt16, parseFormatOrder);
                    result = ((Func<short, object>)toEnum)(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = ((EnumsCache<ushort>)cache).TryParse(value, ignoreCase, out resultAsUInt16, parseFormatOrder);
                    result = ((Func<ushort, object>)toEnum)(resultAsUInt16);
                    return success;
            }
            Debug.Fail("Unknown Enum TypeCode");
            result = null;
            return false;
        }
        #endregion
    }
}
