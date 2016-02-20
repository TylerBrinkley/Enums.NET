// Enums.NET
// Copyright 2016 Tyler Brinkley. All rights reserved.
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

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
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
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
        /// Retrieves the underlying type of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns>The underlying type of <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static Type GetUnderlyingType<TEnum>()
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
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
        public static TypeCode GetTypeCode<TEnum>()
        {
            VerifyTypeIsEnum(typeof(TEnum));
            return Enums<TEnum>.TypeCode;
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
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
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
        public static IEnumerable<EnumMemberInfo<TEnum>> GetEnumMemberInfos<TEnum>(bool uniqueValued = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            IEnumerable<IEnumMemberInfo> infos;
            switch (Enums<TEnum>.TypeCode)
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
            return infos.Select(info => new EnumMemberInfo<TEnum>(info));
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
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
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
            var cache = Enums<TEnum>.Cache;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var int32ToEnum = (Func<int, TEnum>)toEnum;
                    return ((EnumsCache<int>)cache).GetValues(uniqueValued).Select(value => int32ToEnum(value));
                case TypeCode.UInt32:
                    var uint32ToEnum = (Func<uint, TEnum>)toEnum;
                    return ((EnumsCache<uint>)cache).GetValues(uniqueValued).Select(value => uint32ToEnum(value));
                case TypeCode.Int64:
                    var int64ToEnum = (Func<long, TEnum>)toEnum;
                    return ((EnumsCache<long>)cache).GetValues(uniqueValued).Select(value => int64ToEnum(value));
                case TypeCode.UInt64:
                    var uint64ToEnum = (Func<ulong, TEnum>)toEnum;
                    return ((EnumsCache<ulong>)cache).GetValues(uniqueValued).Select(value => uint64ToEnum(value));
                case TypeCode.SByte:
                    var sbyteToEnum = (Func<sbyte, TEnum>)toEnum;
                    return ((EnumsCache<sbyte>)cache).GetValues(uniqueValued).Select(value => sbyteToEnum(value));
                case TypeCode.Byte:
                    var byteToEnum = (Func<byte, TEnum>)toEnum;
                    return ((EnumsCache<byte>)cache).GetValues(uniqueValued).Select(value => byteToEnum(value));
                case TypeCode.Int16:
                    var int16ToEnum = (Func<short, TEnum>)toEnum;
                    return ((EnumsCache<short>)cache).GetValues(uniqueValued).Select(value => int16ToEnum(value));
                case TypeCode.UInt16:
                    var uint16ToEnum = (Func<ushort, TEnum>)toEnum;
                    return ((EnumsCache<ushort>)cache).GetValues(uniqueValued).Select(value => uint16ToEnum(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Retrieves an array of <typeparamref name="TEnum"/> defined constants' descriptions in value order. Descriptions are taken from the <see cref="DescriptionAttribute"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <typeparamref name="TEnum"/> defined constants' descriptions in value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<string> GetDescriptions<TEnum>(bool uniqueValued = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetDescriptions(uniqueValued);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetDescriptions(uniqueValued);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetDescriptions(uniqueValued);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetDescriptions(uniqueValued);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetDescriptions(uniqueValued);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetDescriptions(uniqueValued);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetDescriptions(uniqueValued);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetDescriptions(uniqueValued);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static IEnumerable<string> GetDescriptionsOrNames<TEnum>(bool uniqueValued = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetDescriptionsOrNames(uniqueValued);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetDescriptionsOrNames(uniqueValued);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetDescriptionsOrNames(uniqueValued);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetDescriptionsOrNames(uniqueValued);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetDescriptionsOrNames(uniqueValued);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetDescriptionsOrNames(uniqueValued);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetDescriptionsOrNames(uniqueValued);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetDescriptionsOrNames(uniqueValued);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        public static IEnumerable<string> GetDescriptionsOrNames<TEnum>(Func<string, string> nameFormatter, bool uniqueValued = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetDescriptionsOrNames(nameFormatter, uniqueValued);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Retrieves an array of all of <typeparamref name="TEnum"/>'s defined constants' attributes in value order.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of all of <typeparamref name="TEnum"/>'s defined constants' attributes in value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<Attribute[]> GetAllAttributes<TEnum>(bool uniqueValued = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetAllAttributes(uniqueValued);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetAllAttributes(uniqueValued);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetAllAttributes(uniqueValued);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetAllAttributes(uniqueValued);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetAllAttributes(uniqueValued);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetAllAttributes(uniqueValued);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetAllAttributes(uniqueValued);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetAllAttributes(uniqueValued);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Retrieves an array of <typeparamref name="TEnum"/>'s defined constants' <typeparamref name="TAttribute"/> in value order.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <typeparamref name="TEnum"/>'s defined constants' <typeparamref name="TAttribute"/> in value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static IEnumerable<TAttribute> GetAttributes<TEnum, TAttribute>(bool uniqueValued = false)
            where TAttribute : Attribute
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetAttributes<TAttribute>(uniqueValued);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<TEnum, int>)toInt;
                    return EnumsCache<int>.Compare(enumToInt32(x), enumToInt32(y));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<TEnum, uint>)toInt;
                    return EnumsCache<uint>.Compare(enumToUInt32(x), enumToUInt32(y));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<TEnum, long>)toInt;
                    return EnumsCache<long>.Compare(enumToInt64(x), enumToInt64(y));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<TEnum, ulong>)toInt;
                    return EnumsCache<ulong>.Compare(enumToUInt64(x), enumToUInt64(y));
                case TypeCode.SByte:
                    var enumToSByte = (Func<TEnum, sbyte>)toInt;
                    return EnumsCache<sbyte>.Compare(enumToSByte(x), enumToSByte(y));
                case TypeCode.Byte:
                    var enumToByte = (Func<TEnum, byte>)toInt;
                    return EnumsCache<byte>.Compare(enumToByte(x), enumToByte(y));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<TEnum, short>)toInt;
                    return EnumsCache<short>.Compare(enumToInt16(x), enumToInt16(y));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<TEnum, ushort>)toInt;
                    return EnumsCache<ushort>.Compare(enumToUInt16(x), enumToUInt16(y));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

        [Pure]
        public static bool Equals<TEnum>(TEnum x, TEnum y)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<TEnum, int>)toInt;
                    return EnumsCache<int>.Equal(enumToInt32(x), enumToInt32(y));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<TEnum, uint>)toInt;
                    return EnumsCache<uint>.Equal(enumToUInt32(x), enumToUInt32(y));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<TEnum, long>)toInt;
                    return EnumsCache<long>.Equal(enumToInt64(x), enumToInt64(y));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<TEnum, ulong>)toInt;
                    return EnumsCache<ulong>.Equal(enumToUInt64(x), enumToUInt64(y));
                case TypeCode.SByte:
                    var enumToSByte = (Func<TEnum, sbyte>)toInt;
                    return EnumsCache<sbyte>.Equal(enumToSByte(x), enumToSByte(y));
                case TypeCode.Byte:
                    var enumToByte = (Func<TEnum, byte>)toInt;
                    return EnumsCache<byte>.Equal(enumToByte(x), enumToByte(y));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<TEnum, short>)toInt;
                    return EnumsCache<short>.Equal(enumToInt16(x), enumToInt16(y));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<TEnum, ushort>)toInt;
                    return EnumsCache<ushort>.Equal(enumToUInt16(x), enumToUInt16(y));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

        [Pure]
        public static EnumFormat RegisterCustomEnumFormat<TEnum>(Func<EnumMemberInfo<TEnum>, string> formatter)
        {
            VerifyTypeIsEnum(typeof(TEnum));

            return Enums.InternalRegisterCustomEnumFormat(formatter);
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
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
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
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).IsValid(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).IsValid(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).IsValid(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).IsValid(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).IsValid(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).IsValid(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).IsValid(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).IsValid(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
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
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
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
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
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
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
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
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
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
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
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
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
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
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsDefined<TEnum>(ulong value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
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
            switch (Enums<TEnum>.TypeCode)
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
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsInValueRange<TEnum>(ulong value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
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
        public static TEnum ToObject<TEnum>(object value, bool validate = true)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).ToObject(value, validate));
                case TypeCode.UInt32:
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).ToObject(value, validate));
                case TypeCode.Int64:
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).ToObject(value, validate));
                case TypeCode.UInt64:
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).ToObject(value, validate));
                case TypeCode.SByte:
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).ToObject(value, validate));
                case TypeCode.Byte:
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).ToObject(value, validate));
                case TypeCode.Int16:
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).ToObject(value, validate));
                case TypeCode.UInt16:
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).ToObject(value, validate));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
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
        public static TEnum ToObject<TEnum>(sbyte value, bool validate = true) => ToObject<TEnum>((long)value, validate);

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
        public static TEnum ToObject<TEnum>(byte value, bool validate = true) => ToObject<TEnum>((ulong)value, validate);

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
        public static TEnum ToObject<TEnum>(short value, bool validate = true) => ToObject<TEnum>((long)value, validate);

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
        public static TEnum ToObject<TEnum>(ushort value, bool validate = true) => ToObject<TEnum>((ulong)value, validate);

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
        public static TEnum ToObject<TEnum>(int value, bool validate = true) => ToObject<TEnum>((long)value, validate);

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
        public static TEnum ToObject<TEnum>(uint value, bool validate = true) => ToObject<TEnum>((ulong)value, validate);

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
        public static TEnum ToObject<TEnum>(long value, bool validate = true)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).ToObject(value, validate));
                case TypeCode.UInt32:
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).ToObject(value, validate));
                case TypeCode.Int64:
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).ToObject(value, validate));
                case TypeCode.UInt64:
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).ToObject(value, validate));
                case TypeCode.SByte:
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).ToObject(value, validate));
                case TypeCode.Byte:
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).ToObject(value, validate));
                case TypeCode.Int16:
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).ToObject(value, validate));
                case TypeCode.UInt16:
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).ToObject(value, validate));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
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
        public static TEnum ToObject<TEnum>(ulong value, bool validate = true)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).ToObject(value, validate));
                case TypeCode.UInt32:
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).ToObject(value, validate));
                case TypeCode.Int64:
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).ToObject(value, validate));
                case TypeCode.UInt64:
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).ToObject(value, validate));
                case TypeCode.SByte:
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).ToObject(value, validate));
                case TypeCode.Byte:
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).ToObject(value, validate));
                case TypeCode.Int16:
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).ToObject(value, validate));
                case TypeCode.UInt16:
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).ToObject(value, validate));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ToObjectOrDefault<TEnum>(object value, TEnum defaultEnum, bool validate = true)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            TEnum result;
            return TryToObject(value, out result, validate) ? result : defaultEnum;
        }

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<TEnum>(sbyte value, TEnum defaultEnum, bool validate = true) => ToObjectOrDefault((long)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ToObjectOrDefault<TEnum>(byte value, TEnum defaultEnum, bool validate = true) => ToObjectOrDefault((ulong)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ToObjectOrDefault<TEnum>(short value, TEnum defaultEnum, bool validate = true) => ToObjectOrDefault((long)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<TEnum>(ushort value, TEnum defaultEnum, bool validate = true) => ToObjectOrDefault((ulong)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ToObjectOrDefault<TEnum>(int value, TEnum defaultEnum, bool validate = true) => ToObjectOrDefault((long)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<TEnum>(uint value, TEnum defaultEnum, bool validate = true) => ToObjectOrDefault((ulong)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ToObjectOrDefault<TEnum>(long value, TEnum defaultEnum, bool validate = true)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            TEnum result;
            return TryToObject(value, out result, validate) ? result : defaultEnum;
        }

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<TEnum>(ulong value, TEnum defaultEnum, bool validate = true)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            TEnum result;
            return TryToObject(value, out result, validate) ? result : defaultEnum;
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
        public static bool TryToObject<TEnum>(object value, out TEnum result, bool validate = true)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toEnum = Enums<TEnum>.ToEnum;
            bool success;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = ((EnumsCache<int>)cache).TryToObject(value, out resultAsInt32, validate);
                    result = ((Func<int, TEnum>)toEnum)(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = ((EnumsCache<uint>)cache).TryToObject(value, out resultAsUInt32, validate);
                    result = ((Func<uint, TEnum>)toEnum)(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = ((EnumsCache<long>)cache).TryToObject(value, out resultAsInt64, validate);
                    result = ((Func<long, TEnum>)toEnum)(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = ((EnumsCache<ulong>)cache).TryToObject(value, out resultAsUInt64, validate);
                    result = ((Func<ulong, TEnum>)toEnum)(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = ((EnumsCache<sbyte>)cache).TryToObject(value, out resultAsSByte, validate);
                    result = ((Func<sbyte, TEnum>)toEnum)(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = ((EnumsCache<byte>)cache).TryToObject(value, out resultAsByte, validate);
                    result = ((Func<byte, TEnum>)toEnum)(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = ((EnumsCache<short>)cache).TryToObject(value, out resultAsInt16, validate);
                    result = ((Func<short, TEnum>)toEnum)(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = ((EnumsCache<ushort>)cache).TryToObject(value, out resultAsUInt16, validate);
                    result = ((Func<ushort, TEnum>)toEnum)(resultAsUInt16);
                    return success;
            }
            Debug.Fail("Unknown Enum TypeCode");
            result = default(TEnum);
            return false;
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
        public static bool TryToObject<TEnum>(sbyte value, out TEnum result, bool validate = true) => TryToObject((long)value, out result, validate);

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
        public static bool TryToObject<TEnum>(byte value, out TEnum result, bool validate = true) => TryToObject((ulong)value, out result, validate);

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
        public static bool TryToObject<TEnum>(short value, out TEnum result, bool validate = true) => TryToObject((long)value, out result, validate);

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
        public static bool TryToObject<TEnum>(ushort value, out TEnum result, bool validate = true) => TryToObject((ulong)value, out result, validate);

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
        public static bool TryToObject<TEnum>(int value, out TEnum result, bool validate = true) => TryToObject((long)value, out result, validate);

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
        public static bool TryToObject<TEnum>(uint value, out TEnum result, bool validate = true) => TryToObject((ulong)value, out result, validate);

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
        public static bool TryToObject<TEnum>(long value, out TEnum result, bool validate = true)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toEnum = Enums<TEnum>.ToEnum;
            bool success;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = ((EnumsCache<int>)cache).TryToObject(value, out resultAsInt32, validate);
                    result = ((Func<int, TEnum>)toEnum)(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = ((EnumsCache<uint>)cache).TryToObject(value, out resultAsUInt32, validate);
                    result = ((Func<uint, TEnum>)toEnum)(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = ((EnumsCache<long>)cache).TryToObject(value, out resultAsInt64, validate);
                    result = ((Func<long, TEnum>)toEnum)(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = ((EnumsCache<ulong>)cache).TryToObject(value, out resultAsUInt64, validate);
                    result = ((Func<ulong, TEnum>)toEnum)(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = ((EnumsCache<sbyte>)cache).TryToObject(value, out resultAsSByte, validate);
                    result = ((Func<sbyte, TEnum>)toEnum)(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = ((EnumsCache<byte>)cache).TryToObject(value, out resultAsByte, validate);
                    result = ((Func<byte, TEnum>)toEnum)(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = ((EnumsCache<short>)cache).TryToObject(value, out resultAsInt16, validate);
                    result = ((Func<short, TEnum>)toEnum)(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = ((EnumsCache<ushort>)cache).TryToObject(value, out resultAsUInt16, validate);
                    result = ((Func<ushort, TEnum>)toEnum)(resultAsUInt16);
                    return success;
            }
            Debug.Fail("Unknown Enum TypeCode");
            result = default(TEnum);
            return false;
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
        public static bool TryToObject<TEnum>(ulong value, out TEnum result, bool validate = true)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toEnum = Enums<TEnum>.ToEnum;
            bool success;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = ((EnumsCache<int>)cache).TryToObject(value, out resultAsInt32, validate);
                    result = ((Func<int, TEnum>)toEnum)(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = ((EnumsCache<uint>)cache).TryToObject(value, out resultAsUInt32, validate);
                    result = ((Func<uint, TEnum>)toEnum)(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = ((EnumsCache<long>)cache).TryToObject(value, out resultAsInt64, validate);
                    result = ((Func<long, TEnum>)toEnum)(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = ((EnumsCache<ulong>)cache).TryToObject(value, out resultAsUInt64, validate);
                    result = ((Func<ulong, TEnum>)toEnum)(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = ((EnumsCache<sbyte>)cache).TryToObject(value, out resultAsSByte, validate);
                    result = ((Func<sbyte, TEnum>)toEnum)(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = ((EnumsCache<byte>)cache).TryToObject(value, out resultAsByte, validate);
                    result = ((Func<byte, TEnum>)toEnum)(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = ((EnumsCache<short>)cache).TryToObject(value, out resultAsInt16, validate);
                    result = ((Func<short, TEnum>)toEnum)(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = ((EnumsCache<ushort>)cache).TryToObject(value, out resultAsUInt16, validate);
                    result = ((Func<ushort, TEnum>)toEnum)(resultAsUInt16);
                    return success;
            }
            Debug.Fail("Unknown Enum TypeCode");
            result = default(TEnum);
            return false;
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
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    ((EnumsCache<int>)cache).Validate(((Func<TEnum, int>)toInt)(value), paramName);
                    return value;
                case TypeCode.UInt32:
                    ((EnumsCache<uint>)cache).Validate(((Func<TEnum, uint>)toInt)(value), paramName);
                    return value;
                case TypeCode.Int64:
                    ((EnumsCache<long>)cache).Validate(((Func<TEnum, long>)toInt)(value), paramName);
                    return value;
                case TypeCode.UInt64:
                    ((EnumsCache<ulong>)cache).Validate(((Func<TEnum, ulong>)toInt)(value), paramName);
                    return value;
                case TypeCode.SByte:
                    ((EnumsCache<sbyte>)cache).Validate(((Func<TEnum, sbyte>)toInt)(value), paramName);
                    return value;
                case TypeCode.Byte:
                    ((EnumsCache<byte>)cache).Validate(((Func<TEnum, byte>)toInt)(value), paramName);
                    return value;
                case TypeCode.Int16:
                    ((EnumsCache<short>)cache).Validate(((Func<TEnum, short>)toInt)(value), paramName);
                    return value;
                case TypeCode.UInt16:
                    ((EnumsCache<ushort>)cache).Validate(((Func<TEnum, ushort>)toInt)(value), paramName);
                    return value;
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
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
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).AsString(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).AsString(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).AsString(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).AsString(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).AsString(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).AsString(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).AsString(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).AsString(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).AsString(((Func<TEnum, int>)toInt)(value), format);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).AsString(((Func<TEnum, uint>)toInt)(value), format);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).AsString(((Func<TEnum, long>)toInt)(value), format);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).AsString(((Func<TEnum, ulong>)toInt)(value), format);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).AsString(((Func<TEnum, sbyte>)toInt)(value), format);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).AsString(((Func<TEnum, byte>)toInt)(value), format);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).AsString(((Func<TEnum, short>)toInt)(value), format);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).AsString(((Func<TEnum, ushort>)toInt)(value), format);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string AsString<TEnum>(TEnum value, params EnumFormat[] formats)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).AsString(((Func<TEnum, int>)toInt)(value), formats);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).AsString(((Func<TEnum, uint>)toInt)(value), formats);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).AsString(((Func<TEnum, long>)toInt)(value), formats);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).AsString(((Func<TEnum, ulong>)toInt)(value), formats);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).AsString(((Func<TEnum, sbyte>)toInt)(value), formats);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).AsString(((Func<TEnum, byte>)toInt)(value), formats);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).AsString(((Func<TEnum, short>)toInt)(value), formats);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).AsString(((Func<TEnum, ushort>)toInt)(value), formats);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<TEnum, int>)toInt)(value), format);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<TEnum, uint>)toInt)(value), format);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<TEnum, long>)toInt)(value), format);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<TEnum, ulong>)toInt)(value), format);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<TEnum, sbyte>)toInt)(value), format);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<TEnum, byte>)toInt)(value), format);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<TEnum, short>)toInt)(value), format);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<TEnum, ushort>)toInt)(value), format);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format<TEnum>(TEnum value, EnumFormat format)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<TEnum, int>)toInt)(value), format);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<TEnum, uint>)toInt)(value), format);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<TEnum, long>)toInt)(value), format);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<TEnum, ulong>)toInt)(value), format);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<TEnum, sbyte>)toInt)(value), format);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<TEnum, byte>)toInt)(value), format);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<TEnum, short>)toInt)(value), format);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<TEnum, ushort>)toInt)(value), format);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<TEnum, int>)toInt)(value), format0, format1);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<TEnum, uint>)toInt)(value), format0, format1);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<TEnum, long>)toInt)(value), format0, format1);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<TEnum, ulong>)toInt)(value), format0, format1);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<TEnum, sbyte>)toInt)(value), format0, format1);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<TEnum, byte>)toInt)(value), format0, format1);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<TEnum, short>)toInt)(value), format0, format1);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<TEnum, ushort>)toInt)(value), format0, format1);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<TEnum, int>)toInt)(value), format0, format1, format2);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<TEnum, uint>)toInt)(value), format0, format1, format2);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<TEnum, long>)toInt)(value), format0, format1, format2);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<TEnum, ulong>)toInt)(value), format0, format1, format2);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<TEnum, sbyte>)toInt)(value), format0, format1, format2);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<TEnum, byte>)toInt)(value), format0, format1, format2);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<TEnum, short>)toInt)(value), format0, format1, format2);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<TEnum, ushort>)toInt)(value), format0, format1, format2);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<TEnum, int>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<TEnum, uint>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<TEnum, long>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<TEnum, ulong>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<TEnum, sbyte>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<TEnum, byte>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<TEnum, short>)toInt)(value), format0, format1, format2, format3);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<TEnum, ushort>)toInt)(value), format0, format1, format2, format3);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<TEnum, int>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<TEnum, uint>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<TEnum, long>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<TEnum, ulong>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<TEnum, sbyte>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<TEnum, byte>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<TEnum, short>)toInt)(value), format0, format1, format2, format3, format4);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<TEnum, ushort>)toInt)(value), format0, format1, format2, format3, format4);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format<TEnum>(TEnum value, params EnumFormat[] formats)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).Format(((Func<TEnum, int>)toInt)(value), formats);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).Format(((Func<TEnum, uint>)toInt)(value), formats);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).Format(((Func<TEnum, long>)toInt)(value), formats);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).Format(((Func<TEnum, ulong>)toInt)(value), formats);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).Format(((Func<TEnum, sbyte>)toInt)(value), formats);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).Format(((Func<TEnum, byte>)toInt)(value), formats);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).Format(((Func<TEnum, short>)toInt)(value), formats);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).Format(((Func<TEnum, ushort>)toInt)(value), formats);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<TEnum, int>)toInt)(value);
                case TypeCode.UInt32:
                    return ((Func<TEnum, uint>)toInt)(value);
                case TypeCode.Int64:
                    return ((Func<TEnum, long>)toInt)(value);
                case TypeCode.UInt64:
                    return ((Func<TEnum, ulong>)toInt)(value);
                case TypeCode.SByte:
                    return ((Func<TEnum, sbyte>)toInt)(value);
                case TypeCode.Byte:
                    return ((Func<TEnum, byte>)toInt)(value);
                case TypeCode.Int16:
                    return ((Func<TEnum, short>)toInt)(value);
                case TypeCode.UInt16:
                    return ((Func<TEnum, ushort>)toInt)(value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToSByte(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToSByte(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToSByte(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToSByte(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToSByte(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToSByte(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToSByte(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToSByte(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
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
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToByte(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToByte(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToByte(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToByte(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToByte(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToByte(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToByte(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToByte(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
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
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToInt16(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToInt16(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToInt16(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToInt16(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToInt16(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToInt16(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToInt16(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToInt16(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
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
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToUInt16(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToUInt16(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToUInt16(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToUInt16(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToUInt16(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToUInt16(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToUInt16(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToUInt16(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
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
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToInt32(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToInt32(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToInt32(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToInt32(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToInt32(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToInt32(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToInt32(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToInt32(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
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
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToUInt32(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToUInt32(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToUInt32(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToUInt32(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToUInt32(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToUInt32(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToUInt32(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToUInt32(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
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
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToInt64(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToInt64(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToInt64(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToInt64(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToInt64(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToInt64(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToInt64(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToInt64(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
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
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToUInt64(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToUInt64(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToUInt64(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToUInt64(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToUInt64(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToUInt64(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToUInt64(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToUInt64(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

        [Pure]
        public static int GetHashCode<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<TEnum, int>)toInt)(value);
                case TypeCode.UInt32:
                    return ((Func<TEnum, uint>)toInt)(value).GetHashCode();
                case TypeCode.Int64:
                    return ((Func<TEnum, long>)toInt)(value).GetHashCode();
                case TypeCode.UInt64:
                    return ((Func<TEnum, ulong>)toInt)(value).GetHashCode();
                case TypeCode.SByte:
                    return ((Func<TEnum, sbyte>)toInt)(value);
                case TypeCode.Byte:
                    return ((Func<TEnum, byte>)toInt)(value);
                case TypeCode.Int16:
                    return ((Func<TEnum, short>)toInt)(value);
                case TypeCode.UInt16:
                    return ((Func<TEnum, ushort>)toInt)(value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }
        #endregion

        #region Defined Values Main Methods
        [Pure]
        public static EnumMemberInfo<TEnum> GetEnumMemberInfo<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            IEnumMemberInfo info = null;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    info = ((EnumsCache<int>)cache).GetEnumMemberInfo(((Func<TEnum, int>)toInt)(value));
                    break;
                case TypeCode.UInt32:
                    info = ((EnumsCache<uint>)cache).GetEnumMemberInfo(((Func<TEnum, uint>)toInt)(value));
                    break;
                case TypeCode.Int64:
                    info = ((EnumsCache<long>)cache).GetEnumMemberInfo(((Func<TEnum, long>)toInt)(value));
                    break;
                case TypeCode.UInt64:
                    info = ((EnumsCache<ulong>)cache).GetEnumMemberInfo(((Func<TEnum, ulong>)toInt)(value));
                    break;
                case TypeCode.SByte:
                    info = ((EnumsCache<sbyte>)cache).GetEnumMemberInfo(((Func<TEnum, sbyte>)toInt)(value));
                    break;
                case TypeCode.Byte:
                    info = ((EnumsCache<byte>)cache).GetEnumMemberInfo(((Func<TEnum, byte>)toInt)(value));
                    break;
                case TypeCode.Int16:
                    info = ((EnumsCache<short>)cache).GetEnumMemberInfo(((Func<TEnum, short>)toInt)(value));
                    break;
                case TypeCode.UInt16:
                    info = ((EnumsCache<ushort>)cache).GetEnumMemberInfo(((Func<TEnum, ushort>)toInt)(value));
                    break;
                default:
                    Debug.Fail("Unknown Enum TypeCode");
                    return null;
            }
            return info.IsDefined ? new EnumMemberInfo<TEnum>(info) : null;
        }

        [Pure]
        public static EnumMemberInfo<TEnum> GetEnumMemberInfo<TEnum>(string name, bool ignoreCase = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            IEnumMemberInfo info = null;
            switch (Enums<TEnum>.TypeCode)
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
            return info.IsDefined ? new EnumMemberInfo<TEnum>(info) : null;
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
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetName(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetName(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetName(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetName(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetName(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetName(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetName(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetName(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetDescription(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetDescription(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetDescription(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetDescription(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetDescription(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetDescription(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetDescription(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetDescription(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string GetDescriptionOrName<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetDescriptionOrName(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetDescriptionOrName(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetDescriptionOrName(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetDescriptionOrName(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetDescriptionOrName(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetDescriptionOrName(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetDescriptionOrName(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetDescriptionOrName(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        public static string GetDescriptionOrName<TEnum>(TEnum value, Func<string, string> nameFormatter)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetDescriptionOrName(((Func<TEnum, int>)toInt)(value), nameFormatter);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetDescriptionOrName(((Func<TEnum, uint>)toInt)(value), nameFormatter);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetDescriptionOrName(((Func<TEnum, long>)toInt)(value), nameFormatter);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetDescriptionOrName(((Func<TEnum, ulong>)toInt)(value), nameFormatter);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetDescriptionOrName(((Func<TEnum, sbyte>)toInt)(value), nameFormatter);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetDescriptionOrName(((Func<TEnum, byte>)toInt)(value), nameFormatter);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetDescriptionOrName(((Func<TEnum, short>)toInt)(value), nameFormatter);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetDescriptionOrName(((Func<TEnum, ushort>)toInt)(value), nameFormatter);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetAttribute<TAttribute>(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetAttribute<TAttribute>(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetAttribute<TAttribute>(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetAttribute<TAttribute>(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetAttribute<TAttribute>(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetAttribute<TAttribute>(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetAttribute<TAttribute>(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetAttribute<TAttribute>(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
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
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetAttributeSelect(((Func<TEnum, int>)toInt)(value), selector, defaultValue);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetAttributeSelect(((Func<TEnum, uint>)toInt)(value), selector, defaultValue);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetAttributeSelect(((Func<TEnum, long>)toInt)(value), selector, defaultValue);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetAttributeSelect(((Func<TEnum, ulong>)toInt)(value), selector, defaultValue);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetAttributeSelect(((Func<TEnum, sbyte>)toInt)(value), selector, defaultValue);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetAttributeSelect(((Func<TEnum, byte>)toInt)(value), selector, defaultValue);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetAttributeSelect(((Func<TEnum, short>)toInt)(value), selector, defaultValue);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetAttributeSelect(((Func<TEnum, ushort>)toInt)(value), selector, defaultValue);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TResult);
        }

        [Pure]
        public static bool TryGetAttributeSelect<TEnum, TAttribute, TResult>(this TEnum value, Func<TAttribute, TResult> selector, out TResult result)
            where TAttribute : Attribute
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).TryGetAttributeSelect(((Func<TEnum, int>)toInt)(value), selector, out result);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).TryGetAttributeSelect(((Func<TEnum, uint>)toInt)(value), selector, out result);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).TryGetAttributeSelect(((Func<TEnum, long>)toInt)(value), selector, out result);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).TryGetAttributeSelect(((Func<TEnum, ulong>)toInt)(value), selector, out result);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).TryGetAttributeSelect(((Func<TEnum, sbyte>)toInt)(value), selector, out result);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).TryGetAttributeSelect(((Func<TEnum, byte>)toInt)(value), selector, out result);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).TryGetAttributeSelect(((Func<TEnum, short>)toInt)(value), selector, out result);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).TryGetAttributeSelect(((Func<TEnum, ushort>)toInt)(value), selector, out result);
            }
            Debug.Fail("Unknown Enum TypeCode");
            result = default(TResult);
            return false;
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
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetAttributes<TAttribute>(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetAttributes<TAttribute>(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetAttributes<TAttribute>(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetAttributes<TAttribute>(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetAttributes<TAttribute>(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetAttributes<TAttribute>(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetAttributes<TAttribute>(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetAttributes<TAttribute>(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Retrieves an array of all the <see cref="Attribute"/>'s of the constant in the enumeration that has the specified <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns><see cref="Attribute"/> array if value is defined, else null</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static Attribute[] GetAllAttributes<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).GetAllAttributes(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).GetAllAttributes(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).GetAllAttributes(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).GetAllAttributes(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).GetAllAttributes(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).GetAllAttributes(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).GetAllAttributes(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).GetAllAttributes(((Func<TEnum, ushort>)toInt)(value));
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
            var cache = Enums<TEnum>.Cache;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.UInt32:
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.Int64:
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.UInt64:
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.SByte:
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.Byte:
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.Int16:
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.UInt16:
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).Parse(value, ignoreCase, parseFormatOrder));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified default enumerated value.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, TEnum defaultEnum) => ParseOrDefault(value, false, defaultEnum, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated value.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, TEnum defaultEnum, params EnumFormat[] parseFormatOrder) => ParseOrDefault(value, false, defaultEnum, parseFormatOrder);

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified default enumerated value.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, bool ignoreCase, TEnum defaultEnum) => ParseOrDefault(value, ignoreCase, defaultEnum, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated value. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, bool ignoreCase, TEnum defaultEnum, params EnumFormat[] parseFormatOrder)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            TEnum result;
            return TryParse(value, ignoreCase, out result, parseFormatOrder) ? result : defaultEnum;
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
            var cache = Enums<TEnum>.Cache;
            var toEnum = Enums<TEnum>.ToEnum;
            var success = false;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = ((EnumsCache<int>)cache).TryParse(value, ignoreCase, out resultAsInt32, parseFormatOrder);
                    result = ((Func<int, TEnum>)toEnum)(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = ((EnumsCache<uint>)cache).TryParse(value, ignoreCase, out resultAsUInt32, parseFormatOrder);
                    result = ((Func<uint, TEnum>)toEnum)(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = ((EnumsCache<long>)cache).TryParse(value, ignoreCase, out resultAsInt64, parseFormatOrder);
                    result = ((Func<long, TEnum>)toEnum)(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = ((EnumsCache<ulong>)cache).TryParse(value, ignoreCase, out resultAsUInt64, parseFormatOrder);
                    result = ((Func<ulong, TEnum>)toEnum)(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = ((EnumsCache<sbyte>)cache).TryParse(value, ignoreCase, out resultAsSByte, parseFormatOrder);
                    result = ((Func<sbyte, TEnum>)toEnum)(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = ((EnumsCache<byte>)cache).TryParse(value, ignoreCase, out resultAsByte, parseFormatOrder);
                    result = ((Func<byte, TEnum>)toEnum)(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = ((EnumsCache<short>)cache).TryParse(value, ignoreCase, out resultAsInt16, parseFormatOrder);
                    result = ((Func<short, TEnum>)toEnum)(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = ((EnumsCache<ushort>)cache).TryParse(value, ignoreCase, out resultAsUInt16, parseFormatOrder);
                    result = ((Func<ushort, TEnum>)toEnum)(resultAsUInt16);
                    return success;
            }
            Debug.Fail("Unknown Enum TypeCode");
            result = default(TEnum);
            return false;
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
