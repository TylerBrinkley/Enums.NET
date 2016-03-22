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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return ((NonGenericEnumInfo<int>)enumInfo).Cache.IsContiguous;
                case TypeCode.UInt32:
                    return ((NonGenericEnumInfo<uint>)enumInfo).Cache.IsContiguous;
                case TypeCode.Int64:
                    return ((NonGenericEnumInfo<long>)enumInfo).Cache.IsContiguous;
                case TypeCode.UInt64:
                    return ((NonGenericEnumInfo<ulong>)enumInfo).Cache.IsContiguous;
                case TypeCode.SByte:
                    return ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.IsContiguous;
                case TypeCode.Byte:
                    return ((NonGenericEnumInfo<byte>)enumInfo).Cache.IsContiguous;
                case TypeCode.Int16:
                    return ((NonGenericEnumInfo<short>)enumInfo).Cache.IsContiguous;
                case TypeCode.UInt16:
                    return ((NonGenericEnumInfo<ushort>)enumInfo).Cache.IsContiguous;
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);

            switch (enumInfo.TypeCode)
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
        public static TypeCode GetTypeCode(Type enumType) => NonGenericEnumInfo.Get(enumType).TypeCode;
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return ((NonGenericEnumInfo<int>)enumInfo).Cache.GetDefinedCount(uniqueValued);
                case TypeCode.UInt32:
                    return ((NonGenericEnumInfo<uint>)enumInfo).Cache.GetDefinedCount(uniqueValued);
                case TypeCode.Int64:
                    return ((NonGenericEnumInfo<long>)enumInfo).Cache.GetDefinedCount(uniqueValued);
                case TypeCode.UInt64:
                    return ((NonGenericEnumInfo<ulong>)enumInfo).Cache.GetDefinedCount(uniqueValued);
                case TypeCode.SByte:
                    return ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.GetDefinedCount(uniqueValued);
                case TypeCode.Byte:
                    return ((NonGenericEnumInfo<byte>)enumInfo).Cache.GetDefinedCount(uniqueValued);
                case TypeCode.Int16:
                    return ((NonGenericEnumInfo<short>)enumInfo).Cache.GetDefinedCount(uniqueValued);
                case TypeCode.UInt16:
                    return ((NonGenericEnumInfo<ushort>)enumInfo).Cache.GetDefinedCount(uniqueValued);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

        [Pure]
        public static IEnumerable<EnumMemberInfo> GetEnumMemberInfos(Type enumType, bool uniqueValued = false)
        {
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            IEnumerable<IEnumMemberInfo> infos;
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    infos = ((NonGenericEnumInfo<int>)enumInfo).Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.UInt32:
                    infos = ((NonGenericEnumInfo<uint>)enumInfo).Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.Int64:
                    infos = ((NonGenericEnumInfo<long>)enumInfo).Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.UInt64:
                    infos = ((NonGenericEnumInfo<ulong>)enumInfo).Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.SByte:
                    infos = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.Byte:
                    infos = ((NonGenericEnumInfo<byte>)enumInfo).Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.Int16:
                    infos = ((NonGenericEnumInfo<short>)enumInfo).Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.UInt16:
                    infos = ((NonGenericEnumInfo<ushort>)enumInfo).Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                default:
                    Debug.Fail("Unknown Enum TypeCode");
                    return null;
            }
            return infos.Select(info => new NonGenericEnumMemberInfo(info, enumInfo));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return ((NonGenericEnumInfo<int>)enumInfo).Cache.GetNames(uniqueValued);
                case TypeCode.UInt32:
                    return ((NonGenericEnumInfo<uint>)enumInfo).Cache.GetNames(uniqueValued);
                case TypeCode.Int64:
                    return ((NonGenericEnumInfo<long>)enumInfo).Cache.GetNames(uniqueValued);
                case TypeCode.UInt64:
                    return ((NonGenericEnumInfo<ulong>)enumInfo).Cache.GetNames(uniqueValued);
                case TypeCode.SByte:
                    return ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.GetNames(uniqueValued);
                case TypeCode.Byte:
                    return ((NonGenericEnumInfo<byte>)enumInfo).Cache.GetNames(uniqueValued);
                case TypeCode.Int16:
                    return ((NonGenericEnumInfo<short>)enumInfo).Cache.GetNames(uniqueValued);
                case TypeCode.UInt16:
                    return ((NonGenericEnumInfo<ushort>)enumInfo).Cache.GetNames(uniqueValued);
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.Cache.GetValues(uniqueValued).Select(value => int32EnumInfo.ToEnum(value));
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.Cache.GetValues(uniqueValued).Select(value => uint32EnumInfo.ToEnum(value));
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return ((NonGenericEnumInfo<long>)enumInfo).Cache.GetValues(uniqueValued).Select(value => int64EnumInfo.ToEnum(value));
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return ((NonGenericEnumInfo<ulong>)enumInfo).Cache.GetValues(uniqueValued).Select(value => uint64EnumInfo.ToEnum(value));
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.GetValues(uniqueValued).Select(value => sbyteEnumInfo.ToEnum(value));
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return ((NonGenericEnumInfo<byte>)enumInfo).Cache.GetValues(uniqueValued).Select(value => byteEnumInfo.ToEnum(value));
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return ((NonGenericEnumInfo<short>)enumInfo).Cache.GetValues(uniqueValued).Select(value => int16EnumInfo.ToEnum(value));
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return ((NonGenericEnumInfo<ushort>)enumInfo).Cache.GetValues(uniqueValued).Select(value => uint16EnumInfo.ToEnum(value));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (isNullable)
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        return 0;
                    }
                    ToObject(enumType, y, false);
                    return -1;
                }
                if (y == null)
                {
                    ToObject(enumType, x, false);
                    return 1;
                }
            }

            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return EnumsCache<int>.Compare(int32Cache.ToObject(x, false), int32Cache.ToObject(y, false));
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return EnumsCache<uint>.Compare(uint32Cache.ToObject(x, false), uint32Cache.ToObject(y, false));
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return EnumsCache<long>.Compare(int64Cache.ToObject(x, false), int64Cache.ToObject(y, false));
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return EnumsCache<ulong>.Compare(uint64Cache.ToObject(x, false), uint64Cache.ToObject(y, false));
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return EnumsCache<sbyte>.Compare(sbyteCache.ToObject(x, false), sbyteCache.ToObject(y, false));
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return EnumsCache<byte>.Compare(byteCache.ToObject(x, false), byteCache.ToObject(y, false));
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return EnumsCache<short>.Compare(int16Cache.ToObject(x, false), int16Cache.ToObject(y, false));
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return EnumsCache<ushort>.Compare(uint16Cache.ToObject(x, false), uint16Cache.ToObject(y, false));
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
        public static EnumFormat RegisterCustomEnumFormat(Type enumType, Func<EnumMemberInfo, string> formatter) => NonGenericEnumInfo.Get(enumType).RegisterCustomEnumFormat(formatter);
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return true;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return ((NonGenericEnumInfo<int>)enumInfo).Cache.IsValid(value);
                case TypeCode.UInt32:
                    return ((NonGenericEnumInfo<uint>)enumInfo).Cache.IsValid(value);
                case TypeCode.Int64:
                    return ((NonGenericEnumInfo<long>)enumInfo).Cache.IsValid(value);
                case TypeCode.UInt64:
                    return ((NonGenericEnumInfo<ulong>)enumInfo).Cache.IsValid(value);
                case TypeCode.SByte:
                    return ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.IsValid(value);
                case TypeCode.Byte:
                    return ((NonGenericEnumInfo<byte>)enumInfo).Cache.IsValid(value);
                case TypeCode.Int16:
                    return ((NonGenericEnumInfo<short>)enumInfo).Cache.IsValid(value);
                case TypeCode.UInt16:
                    return ((NonGenericEnumInfo<ushort>)enumInfo).Cache.IsValid(value);
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return ((NonGenericEnumInfo<int>)enumInfo).Cache.IsValid(value);
                case TypeCode.UInt32:
                    return ((NonGenericEnumInfo<uint>)enumInfo).Cache.IsValid(value);
                case TypeCode.Int64:
                    return ((NonGenericEnumInfo<long>)enumInfo).Cache.IsValid(value);
                case TypeCode.UInt64:
                    return ((NonGenericEnumInfo<ulong>)enumInfo).Cache.IsValid(value);
                case TypeCode.SByte:
                    return ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.IsValid(value);
                case TypeCode.Byte:
                    return ((NonGenericEnumInfo<byte>)enumInfo).Cache.IsValid(value);
                case TypeCode.Int16:
                    return ((NonGenericEnumInfo<short>)enumInfo).Cache.IsValid(value);
                case TypeCode.UInt16:
                    return ((NonGenericEnumInfo<ushort>)enumInfo).Cache.IsValid(value);
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return ((NonGenericEnumInfo<int>)enumInfo).Cache.IsValid(value);
                case TypeCode.UInt32:
                    return ((NonGenericEnumInfo<uint>)enumInfo).Cache.IsValid(value);
                case TypeCode.Int64:
                    return ((NonGenericEnumInfo<long>)enumInfo).Cache.IsValid(value);
                case TypeCode.UInt64:
                    return ((NonGenericEnumInfo<ulong>)enumInfo).Cache.IsValid(value);
                case TypeCode.SByte:
                    return ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.IsValid(value);
                case TypeCode.Byte:
                    return ((NonGenericEnumInfo<byte>)enumInfo).Cache.IsValid(value);
                case TypeCode.Int16:
                    return ((NonGenericEnumInfo<short>)enumInfo).Cache.IsValid(value);
                case TypeCode.UInt16:
                    return ((NonGenericEnumInfo<ushort>)enumInfo).Cache.IsValid(value);
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return false;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return ((NonGenericEnumInfo<int>)enumInfo).Cache.IsDefined(value);
                case TypeCode.UInt32:
                    return ((NonGenericEnumInfo<uint>)enumInfo).Cache.IsDefined(value);
                case TypeCode.Int64:
                    return ((NonGenericEnumInfo<long>)enumInfo).Cache.IsDefined(value);
                case TypeCode.UInt64:
                    return ((NonGenericEnumInfo<ulong>)enumInfo).Cache.IsDefined(value);
                case TypeCode.SByte:
                    return ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.IsDefined(value);
                case TypeCode.Byte:
                    return ((NonGenericEnumInfo<byte>)enumInfo).Cache.IsDefined(value);
                case TypeCode.Int16:
                    return ((NonGenericEnumInfo<short>)enumInfo).Cache.IsDefined(value);
                case TypeCode.UInt16:
                    return ((NonGenericEnumInfo<ushort>)enumInfo).Cache.IsDefined(value);
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return ((NonGenericEnumInfo<int>)enumInfo).Cache.IsDefined(name, ignoreCase);
                case TypeCode.UInt32:
                    return ((NonGenericEnumInfo<uint>)enumInfo).Cache.IsDefined(name, ignoreCase);
                case TypeCode.Int64:
                    return ((NonGenericEnumInfo<long>)enumInfo).Cache.IsDefined(name, ignoreCase);
                case TypeCode.UInt64:
                    return ((NonGenericEnumInfo<ulong>)enumInfo).Cache.IsDefined(name, ignoreCase);
                case TypeCode.SByte:
                    return ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.IsDefined(name, ignoreCase);
                case TypeCode.Byte:
                    return ((NonGenericEnumInfo<byte>)enumInfo).Cache.IsDefined(name, ignoreCase);
                case TypeCode.Int16:
                    return ((NonGenericEnumInfo<short>)enumInfo).Cache.IsDefined(name, ignoreCase);
                case TypeCode.UInt16:
                    return ((NonGenericEnumInfo<ushort>)enumInfo).Cache.IsDefined(name, ignoreCase);
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return ((NonGenericEnumInfo<int>)enumInfo).Cache.IsDefined(value);
                case TypeCode.UInt32:
                    return ((NonGenericEnumInfo<uint>)enumInfo).Cache.IsDefined(value);
                case TypeCode.Int64:
                    return ((NonGenericEnumInfo<long>)enumInfo).Cache.IsDefined(value);
                case TypeCode.UInt64:
                    return ((NonGenericEnumInfo<ulong>)enumInfo).Cache.IsDefined(value);
                case TypeCode.SByte:
                    return ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.IsDefined(value);
                case TypeCode.Byte:
                    return ((NonGenericEnumInfo<byte>)enumInfo).Cache.IsDefined(value);
                case TypeCode.Int16:
                    return ((NonGenericEnumInfo<short>)enumInfo).Cache.IsDefined(value);
                case TypeCode.UInt16:
                    return ((NonGenericEnumInfo<ushort>)enumInfo).Cache.IsDefined(value);
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return ((NonGenericEnumInfo<int>)enumInfo).Cache.IsDefined(value);
                case TypeCode.UInt32:
                    return ((NonGenericEnumInfo<uint>)enumInfo).Cache.IsDefined(value);
                case TypeCode.Int64:
                    return ((NonGenericEnumInfo<long>)enumInfo).Cache.IsDefined(value);
                case TypeCode.UInt64:
                    return ((NonGenericEnumInfo<ulong>)enumInfo).Cache.IsDefined(value);
                case TypeCode.SByte:
                    return ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.IsDefined(value);
                case TypeCode.Byte:
                    return ((NonGenericEnumInfo<byte>)enumInfo).Cache.IsDefined(value);
                case TypeCode.Int16:
                    return ((NonGenericEnumInfo<short>)enumInfo).Cache.IsDefined(value);
                case TypeCode.UInt16:
                    return ((NonGenericEnumInfo<ushort>)enumInfo).Cache.IsDefined(value);
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.ToEnum(int32EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.ToEnum(uint32EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return int64EnumInfo.ToEnum(int64EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return uint64EnumInfo.ToEnum(uint64EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return sbyteEnumInfo.ToEnum(sbyteEnumInfo.Cache.ToObject(value, validate));
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return byteEnumInfo.ToEnum(byteEnumInfo.Cache.ToObject(value, validate));
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return int16EnumInfo.ToEnum(int16EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return uint16EnumInfo.ToEnum(uint16EnumInfo.Cache.ToObject(value, validate));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.ToEnum(int32EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.ToEnum(uint32EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return int64EnumInfo.ToEnum(int64EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return uint64EnumInfo.ToEnum(uint64EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return sbyteEnumInfo.ToEnum(sbyteEnumInfo.Cache.ToObject(value, validate));
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return byteEnumInfo.ToEnum(byteEnumInfo.Cache.ToObject(value, validate));
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return int16EnumInfo.ToEnum(int16EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return uint16EnumInfo.ToEnum(uint16EnumInfo.Cache.ToObject(value, validate));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.ToEnum(int32EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.ToEnum(uint32EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return int64EnumInfo.ToEnum(int64EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return uint64EnumInfo.ToEnum(uint64EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return sbyteEnumInfo.ToEnum(sbyteEnumInfo.Cache.ToObject(value, validate));
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return byteEnumInfo.ToEnum(byteEnumInfo.Cache.ToObject(value, validate));
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return int16EnumInfo.ToEnum(int16EnumInfo.Cache.ToObject(value, validate));
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return uint16EnumInfo.ToEnum(uint16EnumInfo.Cache.ToObject(value, validate));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                result = null;
                return true;
            }
            
            bool success;
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    int resultAsInt32;
                    success = int32EnumInfo.Cache.TryToObject(value, out resultAsInt32, validate);
                    result = int32EnumInfo.ToEnum(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    uint resultAsUInt32;
                    success = uint32EnumInfo.Cache.TryToObject(value, out resultAsUInt32, validate);
                    result = uint32EnumInfo.ToEnum(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    long resultAsInt64;
                    success = int64EnumInfo.Cache.TryToObject(value, out resultAsInt64, validate);
                    result = int64EnumInfo.ToEnum(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    ulong resultAsUInt64;
                    success = uint64EnumInfo.Cache.TryToObject(value, out resultAsUInt64, validate);
                    result = uint64EnumInfo.ToEnum(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    sbyte resultAsSByte;
                    success = sbyteEnumInfo.Cache.TryToObject(value, out resultAsSByte, validate);
                    result = sbyteEnumInfo.ToEnum(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    byte resultAsByte;
                    success = byteEnumInfo.Cache.TryToObject(value, out resultAsByte, validate);
                    result = byteEnumInfo.ToEnum(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    short resultAsInt16;
                    success = int16EnumInfo.Cache.TryToObject(value, out resultAsInt16, validate);
                    result = int16EnumInfo.ToEnum(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    ushort resultAsUInt16;
                    success = uint16EnumInfo.Cache.TryToObject(value, out resultAsUInt16, validate);
                    result = uint16EnumInfo.ToEnum(resultAsUInt16);
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            bool success;
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    int resultAsInt32;
                    success = int32EnumInfo.Cache.TryToObject(value, out resultAsInt32, validate);
                    result = int32EnumInfo.ToEnum(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    uint resultAsUInt32;
                    success = uint32EnumInfo.Cache.TryToObject(value, out resultAsUInt32, validate);
                    result = uint32EnumInfo.ToEnum(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    long resultAsInt64;
                    success = int64EnumInfo.Cache.TryToObject(value, out resultAsInt64, validate);
                    result = int64EnumInfo.ToEnum(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    ulong resultAsUInt64;
                    success = uint64EnumInfo.Cache.TryToObject(value, out resultAsUInt64, validate);
                    result = uint64EnumInfo.ToEnum(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    sbyte resultAsSByte;
                    success = sbyteEnumInfo.Cache.TryToObject(value, out resultAsSByte, validate);
                    result = sbyteEnumInfo.ToEnum(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    byte resultAsByte;
                    success = byteEnumInfo.Cache.TryToObject(value, out resultAsByte, validate);
                    result = byteEnumInfo.ToEnum(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    short resultAsInt16;
                    success = int16EnumInfo.Cache.TryToObject(value, out resultAsInt16, validate);
                    result = int16EnumInfo.ToEnum(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    ushort resultAsUInt16;
                    success = uint16EnumInfo.Cache.TryToObject(value, out resultAsUInt16, validate);
                    result = uint16EnumInfo.ToEnum(resultAsUInt16);
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            bool success;
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    int resultAsInt32;
                    success = int32EnumInfo.Cache.TryToObject(value, out resultAsInt32, validate);
                    result = int32EnumInfo.ToEnum(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    uint resultAsUInt32;
                    success = uint32EnumInfo.Cache.TryToObject(value, out resultAsUInt32, validate);
                    result = uint32EnumInfo.ToEnum(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    long resultAsInt64;
                    success = int64EnumInfo.Cache.TryToObject(value, out resultAsInt64, validate);
                    result = int64EnumInfo.ToEnum(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    ulong resultAsUInt64;
                    success = uint64EnumInfo.Cache.TryToObject(value, out resultAsUInt64, validate);
                    result = uint64EnumInfo.ToEnum(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    sbyte resultAsSByte;
                    success = sbyteEnumInfo.Cache.TryToObject(value, out resultAsSByte, validate);
                    result = sbyteEnumInfo.ToEnum(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    byte resultAsByte;
                    success = byteEnumInfo.Cache.TryToObject(value, out resultAsByte, validate);
                    result = byteEnumInfo.ToEnum(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    short resultAsInt16;
                    success = int16EnumInfo.Cache.TryToObject(value, out resultAsInt16, validate);
                    result = int16EnumInfo.ToEnum(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    ushort resultAsUInt16;
                    success = uint16EnumInfo.Cache.TryToObject(value, out resultAsUInt16, validate);
                    result = uint16EnumInfo.ToEnum(resultAsUInt16);
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return value;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    int32Cache.Validate(int32Cache.ToObject(value, false), paramName);
                    return value;
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    uint32Cache.Validate(uint32Cache.ToObject(value, false), paramName);
                    return value;
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    int64Cache.Validate(int64Cache.ToObject(value, false), paramName);
                    return value;
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    uint64Cache.Validate(uint64Cache.ToObject(value, false), paramName);
                    return value;
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    sbyteCache.Validate(sbyteCache.ToObject(value, false), paramName);
                    return value;
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    byteCache.Validate(byteCache.ToObject(value, false), paramName);
                    return value;
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    int16Cache.Validate(int16Cache.ToObject(value, false), paramName);
                    return value;
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    uint16Cache.Validate(uint16Cache.ToObject(value, false), paramName);
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.AsString(int32Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.AsString(uint32Cache.ToObject(value, false));
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.AsString(int64Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.AsString(uint64Cache.ToObject(value, false));
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.AsString(sbyteCache.ToObject(value, false));
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.AsString(byteCache.ToObject(value, false));
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.AsString(int16Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.AsString(uint16Cache.ToObject(value, false));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.AsString(int32Cache.ToObject(value, false), format);
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.AsString(uint32Cache.ToObject(value, false), format);
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.AsString(int64Cache.ToObject(value, false), format);
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.AsString(uint64Cache.ToObject(value, false), format);
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.AsString(sbyteCache.ToObject(value, false), format);
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.AsString(byteCache.ToObject(value, false), format);
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.AsString(int16Cache.ToObject(value, false), format);
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.AsString(uint16Cache.ToObject(value, false), format);
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.AsString(int32Cache.ToObject(value, false), formats);
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.AsString(uint32Cache.ToObject(value, false), formats);
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.AsString(int64Cache.ToObject(value, false), formats);
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.AsString(uint64Cache.ToObject(value, false), formats);
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.AsString(sbyteCache.ToObject(value, false), formats);
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.AsString(byteCache.ToObject(value, false), formats);
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.AsString(int16Cache.ToObject(value, false), formats);
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.AsString(uint16Cache.ToObject(value, false), formats);
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.Format(int32Cache.ToObject(value, false), format);
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.Format(uint32Cache.ToObject(value, false), format);
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.Format(int64Cache.ToObject(value, false), format);
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.Format(uint64Cache.ToObject(value, false), format);
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.Format(sbyteCache.ToObject(value, false), format);
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.Format(byteCache.ToObject(value, false), format);
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.Format(int16Cache.ToObject(value, false), format);
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.Format(uint16Cache.ToObject(value, false), format);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format(Type enumType, object value, EnumFormat format)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.Format(int32Cache.ToObject(value, false), format);
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.Format(uint32Cache.ToObject(value, false), format);
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.Format(int64Cache.ToObject(value, false), format);
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.Format(uint64Cache.ToObject(value, false), format);
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.Format(sbyteCache.ToObject(value, false), format);
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.Format(byteCache.ToObject(value, false), format);
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.Format(int16Cache.ToObject(value, false), format);
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.Format(uint16Cache.ToObject(value, false), format);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format(Type enumType, object value, EnumFormat format0, EnumFormat format1)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.Format(int32Cache.ToObject(value, false), format0, format1);
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.Format(uint32Cache.ToObject(value, false), format0, format1);
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.Format(int64Cache.ToObject(value, false), format0, format1);
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.Format(uint64Cache.ToObject(value, false), format0, format1);
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.Format(sbyteCache.ToObject(value, false), format0, format1);
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.Format(byteCache.ToObject(value, false), format0, format1);
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.Format(int16Cache.ToObject(value, false), format0, format1);
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.Format(uint16Cache.ToObject(value, false), format0, format1);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format(Type enumType, object value, EnumFormat format0, EnumFormat format1, EnumFormat format2)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.Format(int32Cache.ToObject(value, false), format0, format1, format2);
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.Format(uint32Cache.ToObject(value, false), format0, format1, format2);
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.Format(int64Cache.ToObject(value, false), format0, format1, format2);
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.Format(uint64Cache.ToObject(value, false), format0, format1, format2);
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.Format(sbyteCache.ToObject(value, false), format0, format1, format2);
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.Format(byteCache.ToObject(value, false), format0, format1, format2);
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.Format(int16Cache.ToObject(value, false), format0, format1, format2);
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.Format(uint16Cache.ToObject(value, false), format0, format1, format2);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format(Type enumType, object value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.Format(int32Cache.ToObject(value, false), format0, format1, format2, format3);
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.Format(uint32Cache.ToObject(value, false), format0, format1, format2, format3);
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.Format(int64Cache.ToObject(value, false), format0, format1, format2, format3);
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.Format(uint64Cache.ToObject(value, false), format0, format1, format2, format3);
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.Format(sbyteCache.ToObject(value, false), format0, format1, format2, format3);
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.Format(byteCache.ToObject(value, false), format0, format1, format2, format3);
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.Format(int16Cache.ToObject(value, false), format0, format1, format2, format3);
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.Format(uint16Cache.ToObject(value, false), format0, format1, format2, format3);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format(Type enumType, object value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.Format(int32Cache.ToObject(value, false), format0, format1, format2, format3, format4);
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.Format(uint32Cache.ToObject(value, false), format0, format1, format2, format3, format4);
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.Format(int64Cache.ToObject(value, false), format0, format1, format2, format3, format4);
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.Format(uint64Cache.ToObject(value, false), format0, format1, format2, format3, format4);
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.Format(sbyteCache.ToObject(value, false), format0, format1, format2, format3, format4);
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.Format(byteCache.ToObject(value, false), format0, format1, format2, format3, format4);
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.Format(int16Cache.ToObject(value, false), format0, format1, format2, format3, format4);
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.Format(uint16Cache.ToObject(value, false), format0, format1, format2, format3, format4);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format(Type enumType, object value, params EnumFormat[] formats)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.Format(int32Cache.ToObject(value, false), formats);
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.Format(uint32Cache.ToObject(value, false), formats);
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.Format(int64Cache.ToObject(value, false), formats);
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.Format(uint64Cache.ToObject(value, false), formats);
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.Format(sbyteCache.ToObject(value, false), formats);
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.Format(byteCache.ToObject(value, false), formats);
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.Format(int16Cache.ToObject(value, false), formats);
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.Format(uint16Cache.ToObject(value, false), formats);
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return ((NonGenericEnumInfo<int>)enumInfo).Cache.ToObject(value, false);
                case TypeCode.UInt32:
                    return ((NonGenericEnumInfo<uint>)enumInfo).Cache.ToObject(value, false);
                case TypeCode.Int64:
                    return ((NonGenericEnumInfo<long>)enumInfo).Cache.ToObject(value, false);
                case TypeCode.UInt64:
                    return ((NonGenericEnumInfo<ulong>)enumInfo).Cache.ToObject(value, false);
                case TypeCode.SByte:
                    return ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.ToObject(value, false);
                case TypeCode.Byte:
                    return ((NonGenericEnumInfo<byte>)enumInfo).Cache.ToObject(value, false);
                case TypeCode.Int16:
                    return ((NonGenericEnumInfo<short>)enumInfo).Cache.ToObject(value, false);
                case TypeCode.UInt16:
                    return ((NonGenericEnumInfo<ushort>)enumInfo).Cache.ToObject(value, false);
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToSByte(((NonGenericEnumInfo<int>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToSByte(((NonGenericEnumInfo<uint>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToSByte(((NonGenericEnumInfo<long>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToSByte(((NonGenericEnumInfo<ulong>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToSByte(((NonGenericEnumInfo<sbyte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToSByte(((NonGenericEnumInfo<byte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToSByte(((NonGenericEnumInfo<short>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToSByte(((NonGenericEnumInfo<ushort>)enumInfo).Cache.ToObject(value, false));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToByte(((NonGenericEnumInfo<int>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToByte(((NonGenericEnumInfo<uint>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToByte(((NonGenericEnumInfo<long>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToByte(((NonGenericEnumInfo<ulong>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToByte(((NonGenericEnumInfo<sbyte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToByte(((NonGenericEnumInfo<byte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToByte(((NonGenericEnumInfo<short>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToByte(((NonGenericEnumInfo<ushort>)enumInfo).Cache.ToObject(value, false));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToInt16(((NonGenericEnumInfo<int>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToInt16(((NonGenericEnumInfo<uint>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToInt16(((NonGenericEnumInfo<long>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToInt16(((NonGenericEnumInfo<ulong>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToInt16(((NonGenericEnumInfo<sbyte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToInt16(((NonGenericEnumInfo<byte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToInt16(((NonGenericEnumInfo<short>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToInt16(((NonGenericEnumInfo<ushort>)enumInfo).Cache.ToObject(value, false));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToUInt16(((NonGenericEnumInfo<int>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToUInt16(((NonGenericEnumInfo<uint>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToUInt16(((NonGenericEnumInfo<long>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToUInt16(((NonGenericEnumInfo<ulong>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToUInt16(((NonGenericEnumInfo<sbyte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToUInt16(((NonGenericEnumInfo<byte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToUInt16(((NonGenericEnumInfo<short>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToUInt16(((NonGenericEnumInfo<ushort>)enumInfo).Cache.ToObject(value, false));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToInt32(((NonGenericEnumInfo<int>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToInt32(((NonGenericEnumInfo<uint>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToInt32(((NonGenericEnumInfo<long>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToInt32(((NonGenericEnumInfo<ulong>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToInt32(((NonGenericEnumInfo<sbyte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToInt32(((NonGenericEnumInfo<byte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToInt32(((NonGenericEnumInfo<short>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToInt32(((NonGenericEnumInfo<ushort>)enumInfo).Cache.ToObject(value, false));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToUInt32(((NonGenericEnumInfo<int>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToUInt32(((NonGenericEnumInfo<uint>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToUInt32(((NonGenericEnumInfo<long>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToUInt32(((NonGenericEnumInfo<ulong>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToUInt32(((NonGenericEnumInfo<sbyte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToUInt32(((NonGenericEnumInfo<byte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToUInt32(((NonGenericEnumInfo<short>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToUInt32(((NonGenericEnumInfo<ushort>)enumInfo).Cache.ToObject(value, false));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToInt64(((NonGenericEnumInfo<int>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToInt64(((NonGenericEnumInfo<uint>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToInt64(((NonGenericEnumInfo<long>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToInt64(((NonGenericEnumInfo<ulong>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToInt64(((NonGenericEnumInfo<sbyte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToInt64(((NonGenericEnumInfo<byte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToInt64(((NonGenericEnumInfo<short>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToInt64(((NonGenericEnumInfo<ushort>)enumInfo).Cache.ToObject(value, false));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToUInt64(((NonGenericEnumInfo<int>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToUInt64(((NonGenericEnumInfo<uint>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToUInt64(((NonGenericEnumInfo<long>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToUInt64(((NonGenericEnumInfo<ulong>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToUInt64(((NonGenericEnumInfo<sbyte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToUInt64(((NonGenericEnumInfo<byte>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToUInt64(((NonGenericEnumInfo<short>)enumInfo).Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToUInt64(((NonGenericEnumInfo<ushort>)enumInfo).Cache.ToObject(value, false));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

        [Pure]
        public static bool Equals(Type enumType, object x, object y)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (isNullable)
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        return true;
                    }
                    ToObject(enumType, y, false);
                    return false;
                }
                if (y == null)
                {
                    ToObject(enumType, x, false);
                    return false;
                }
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.ToObject(x, false) == int32Cache.ToObject(y, false);
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.ToObject(x, false) == uint32Cache.ToObject(y, false);
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.ToObject(x, false) == int64Cache.ToObject(y, false);
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.ToObject(x, false) == uint64Cache.ToObject(y, false);
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.ToObject(x, false) == sbyteCache.ToObject(y, false);
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.ToObject(x, false) == byteCache.ToObject(y, false);
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.ToObject(x, false) == int16Cache.ToObject(y, false);
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.ToObject(x, false) == uint16Cache.ToObject(y, false);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }
        #endregion

        #region Defined Values Main Methods
        [Pure]
        public static EnumMemberInfo GetEnumMemberInfo(Type enumType, object value)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            IEnumMemberInfo info = null;
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    info = int32Cache.GetEnumMemberInfo(int32Cache.ToObject(value, false));
                    break;
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    info = uint32Cache.GetEnumMemberInfo(uint32Cache.ToObject(value, false));
                    break;
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    info = int64Cache.GetEnumMemberInfo(int64Cache.ToObject(value, false));
                    break;
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    info = uint64Cache.GetEnumMemberInfo(uint64Cache.ToObject(value, false));
                    break;
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    info = sbyteCache.GetEnumMemberInfo(sbyteCache.ToObject(value, false));
                    break;
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    info = byteCache.GetEnumMemberInfo(byteCache.ToObject(value, false));
                    break;
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    info = int16Cache.GetEnumMemberInfo(int16Cache.ToObject(value, false));
                    break;
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    info = uint16Cache.GetEnumMemberInfo(uint16Cache.ToObject(value, false));
                    break;
                default:
                    Debug.Fail("Unknown Enum TypeCode");
                    return null;
            }
            return info.IsDefined ? new NonGenericEnumMemberInfo(info, enumInfo) : null;
        }

        [Pure]
        public static EnumMemberInfo GetEnumMemberInfo(Type enumType, string name, bool ignoreCase = false)
        {
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            IEnumMemberInfo info = null;
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    info = ((NonGenericEnumInfo<int>)enumInfo).Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.UInt32:
                    info = ((NonGenericEnumInfo<uint>)enumInfo).Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.Int64:
                    info = ((NonGenericEnumInfo<long>)enumInfo).Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.UInt64:
                    info = ((NonGenericEnumInfo<ulong>)enumInfo).Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.SByte:
                    info = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.Byte:
                    info = ((NonGenericEnumInfo<byte>)enumInfo).Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.Int16:
                    info = ((NonGenericEnumInfo<short>)enumInfo).Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.UInt16:
                    info = ((NonGenericEnumInfo<ushort>)enumInfo).Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                default:
                    Debug.Fail("Unknown Enum TypeCode");
                    return null;
            }
            return info.IsDefined ? new NonGenericEnumMemberInfo(info, enumInfo) : null;
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.GetName(int32Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.GetName(uint32Cache.ToObject(value, false));
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.GetName(int64Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.GetName(uint64Cache.ToObject(value, false));
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.GetName(sbyteCache.ToObject(value, false));
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.GetName(byteCache.ToObject(value, false));
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.GetName(int16Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.GetName(uint16Cache.ToObject(value, false));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.GetDescription(int32Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.GetDescription(uint32Cache.ToObject(value, false));
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.GetDescription(int64Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.GetDescription(uint64Cache.ToObject(value, false));
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.GetDescription(sbyteCache.ToObject(value, false));
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.GetDescription(byteCache.ToObject(value, false));
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.GetDescription(int16Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.GetDescription(uint16Cache.ToObject(value, false));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.GetAttributes(int32Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.GetAttributes(uint32Cache.ToObject(value, false));
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.GetAttributes(int64Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.GetAttributes(uint64Cache.ToObject(value, false));
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.GetAttributes(sbyteCache.ToObject(value, false));
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.GetAttributes(byteCache.ToObject(value, false));
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.GetAttributes(int16Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
                    return uint16Cache.GetAttributes(uint16Cache.ToObject(value, false));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (string.IsNullOrEmpty(value) && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.ToEnum(int32EnumInfo.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.ToEnum(uint32EnumInfo.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return int64EnumInfo.ToEnum(int64EnumInfo.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return uint64EnumInfo.ToEnum(uint64EnumInfo.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return sbyteEnumInfo.ToEnum(sbyteEnumInfo.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return byteEnumInfo.ToEnum(byteEnumInfo.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return int16EnumInfo.ToEnum(int16EnumInfo.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return uint16EnumInfo.ToEnum(uint16EnumInfo.Cache.Parse(value, ignoreCase, parseFormatOrder));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (string.IsNullOrEmpty(value) && isNullable)
            {
                result = null;
                return true;
            }

            var success = false;
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    int resultAsInt32;
                    success = int32EnumInfo.Cache.TryParse(value, ignoreCase, out resultAsInt32, parseFormatOrder);
                    result = int32EnumInfo.ToEnum(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    uint resultAsUInt32;
                    success = uint32EnumInfo.Cache.TryParse(value, ignoreCase, out resultAsUInt32, parseFormatOrder);
                    result = uint32EnumInfo.ToEnum(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    long resultAsInt64;
                    success = int64EnumInfo.Cache.TryParse(value, ignoreCase, out resultAsInt64, parseFormatOrder);
                    result = int64EnumInfo.ToEnum(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    ulong resultAsUInt64;
                    success = uint64EnumInfo.Cache.TryParse(value, ignoreCase, out resultAsUInt64, parseFormatOrder);
                    result = uint64EnumInfo.ToEnum(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    sbyte resultAsSByte;
                    success = sbyteEnumInfo.Cache.TryParse(value, ignoreCase, out resultAsSByte, parseFormatOrder);
                    result = sbyteEnumInfo.ToEnum(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    byte resultAsByte;
                    success = byteEnumInfo.Cache.TryParse(value, ignoreCase, out resultAsByte, parseFormatOrder);
                    result = byteEnumInfo.ToEnum(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    short resultAsInt16;
                    success = int16EnumInfo.Cache.TryParse(value, ignoreCase, out resultAsInt16, parseFormatOrder);
                    result = int16EnumInfo.ToEnum(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    ushort resultAsUInt16;
                    success = uint16EnumInfo.Cache.TryParse(value, ignoreCase, out resultAsUInt16, parseFormatOrder);
                    result = uint16EnumInfo.ToEnum(resultAsUInt16);
                    return success;
            }
            Debug.Fail("Unknown Enum TypeCode");
            result = null;
            return false;
        }
        #endregion
    }
}
