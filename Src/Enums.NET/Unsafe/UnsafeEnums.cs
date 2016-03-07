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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.IsContiguous;
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.IsContiguous;
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.IsContiguous;
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.IsContiguous;
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.IsContiguous;
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.IsContiguous;
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.IsContiguous;
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.IsContiguous;
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetDefinedCount(uniqueValued);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetDefinedCount(uniqueValued);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetDefinedCount(uniqueValued);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetDefinedCount(uniqueValued);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetDefinedCount(uniqueValued);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetDefinedCount(uniqueValued);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetDefinedCount(uniqueValued);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetDefinedCount(uniqueValued);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

        [Pure]
        public static IEnumerable<EnumMemberInfo<TEnum>> GetEnumMemberInfos<TEnum>(bool uniqueValued = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            IEnumerable<IEnumMemberInfo> infos;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    infos = Enums<TEnum, int>.Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.UInt32:
                    infos = Enums<TEnum, uint>.Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.Int64:
                    infos = Enums<TEnum, long>.Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.UInt64:
                    infos = Enums<TEnum, ulong>.Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.SByte:
                    infos = Enums<TEnum, sbyte>.Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.Byte:
                    infos = Enums<TEnum, byte>.Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.Int16:
                    infos = Enums<TEnum, short>.Cache.GetEnumMemberInfos(uniqueValued);
                    break;
                case TypeCode.UInt16:
                    infos = Enums<TEnum, ushort>.Cache.GetEnumMemberInfos(uniqueValued);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetNames(uniqueValued);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetNames(uniqueValued);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetNames(uniqueValued);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetNames(uniqueValued);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetNames(uniqueValued);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetNames(uniqueValued);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetNames(uniqueValued);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetNames(uniqueValued);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetValues(uniqueValued).Select(value => Enums<TEnum, int>.ToEnum(value));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetValues(uniqueValued).Select(value => Enums<TEnum, uint>.ToEnum(value));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetValues(uniqueValued).Select(value => Enums<TEnum, long>.ToEnum(value));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetValues(uniqueValued).Select(value => Enums<TEnum, ulong>.ToEnum(value));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetValues(uniqueValued).Select(value => Enums<TEnum, sbyte>.ToEnum(value));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetValues(uniqueValued).Select(value => Enums<TEnum, byte>.ToEnum(value));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetValues(uniqueValued).Select(value => Enums<TEnum, short>.ToEnum(value));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetValues(uniqueValued).Select(value => Enums<TEnum, ushort>.ToEnum(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetDescriptions(uniqueValued);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetDescriptions(uniqueValued);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetDescriptions(uniqueValued);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetDescriptions(uniqueValued);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetDescriptions(uniqueValued);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetDescriptions(uniqueValued);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetDescriptions(uniqueValued);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetDescriptions(uniqueValued);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static IEnumerable<string> GetDescriptionsOrNames<TEnum>(bool uniqueValued = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetDescriptionsOrNames(uniqueValued);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetDescriptionsOrNames(uniqueValued);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetDescriptionsOrNames(uniqueValued);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetDescriptionsOrNames(uniqueValued);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetDescriptionsOrNames(uniqueValued);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetDescriptionsOrNames(uniqueValued);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetDescriptionsOrNames(uniqueValued);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetDescriptionsOrNames(uniqueValued);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        public static IEnumerable<string> GetDescriptionsOrNames<TEnum>(Func<string, string> nameFormatter, bool uniqueValued = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetDescriptionsOrNames(nameFormatter, uniqueValued);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetDescriptionsOrNames(nameFormatter, uniqueValued);
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
        public static IEnumerable<Attribute[]> GetAttributes<TEnum>(bool uniqueValued = false)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetAttributes(uniqueValued);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetAttributes(uniqueValued);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetAttributes(uniqueValued);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetAttributes(uniqueValued);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetAttributes(uniqueValued);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetAttributes(uniqueValued);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetAttributes(uniqueValued);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetAttributes(uniqueValued);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetAttributes<TAttribute>(uniqueValued);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetAttributes<TAttribute>(uniqueValued);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = Enums<TEnum, int>.ToInt;
                    return EnumsCache<int>.Compare(enumToInt32(x), enumToInt32(y));
                case TypeCode.UInt32:
                    var enumToUInt32 = Enums<TEnum, uint>.ToInt;
                    return EnumsCache<uint>.Compare(enumToUInt32(x), enumToUInt32(y));
                case TypeCode.Int64:
                    var enumToInt64 = Enums<TEnum, long>.ToInt;
                    return EnumsCache<long>.Compare(enumToInt64(x), enumToInt64(y));
                case TypeCode.UInt64:
                    var enumToUInt64 = Enums<TEnum, ulong>.ToInt;
                    return EnumsCache<ulong>.Compare(enumToUInt64(x), enumToUInt64(y));
                case TypeCode.SByte:
                    var enumToSByte = Enums<TEnum, sbyte>.ToInt;
                    return EnumsCache<sbyte>.Compare(enumToSByte(x), enumToSByte(y));
                case TypeCode.Byte:
                    var enumToByte = Enums<TEnum, byte>.ToInt;
                    return EnumsCache<byte>.Compare(enumToByte(x), enumToByte(y));
                case TypeCode.Int16:
                    var enumToInt16 = Enums<TEnum, short>.ToInt;
                    return EnumsCache<short>.Compare(enumToInt16(x), enumToInt16(y));
                case TypeCode.UInt16:
                    var enumToUInt16 = Enums<TEnum, ushort>.ToInt;
                    return EnumsCache<ushort>.Compare(enumToUInt16(x), enumToUInt16(y));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

        /// <summary>
        /// Registers a custom enum format for <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="formatter"></param>
        /// <returns></returns>
        /// /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.IsValid(value);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.IsValid(value);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.IsValid(value);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.IsValid(value);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.IsValid(value);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.IsValid(value);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.IsValid(value);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.IsValid(value);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.IsValid(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.IsValid(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.IsValid(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.IsValid(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.IsValid(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.IsValid(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.IsValid(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.IsValid(Enums<TEnum, ushort>.ToInt(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.IsValid(value);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.IsValid(value);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.IsValid(value);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.IsValid(value);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.IsValid(value);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.IsValid(value);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.IsValid(value);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.IsValid(value);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.IsValid(value);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.IsValid(value);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.IsValid(value);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.IsValid(value);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.IsValid(value);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.IsValid(value);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.IsValid(value);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.IsValid(value);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.IsDefined(value);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.IsDefined(value);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.IsDefined(value);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.IsDefined(value);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.IsDefined(value);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.IsDefined(value);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.IsDefined(value);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.IsDefined(value);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.IsDefined(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.IsDefined(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.IsDefined(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.IsDefined(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.IsDefined(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.IsDefined(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.IsDefined(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.IsDefined(Enums<TEnum, ushort>.ToInt(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.IsDefined(name, ignoreCase);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.IsDefined(name, ignoreCase);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.IsDefined(name, ignoreCase);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.IsDefined(name, ignoreCase);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.IsDefined(name, ignoreCase);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.IsDefined(name, ignoreCase);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.IsDefined(name, ignoreCase);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.IsDefined(name, ignoreCase);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.IsDefined(value);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.IsDefined(value);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.IsDefined(value);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.IsDefined(value);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.IsDefined(value);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.IsDefined(value);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.IsDefined(value);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.IsDefined(value);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.IsDefined(value);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.IsDefined(value);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.IsDefined(value);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.IsDefined(value);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.IsDefined(value);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.IsDefined(value);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.IsDefined(value);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.IsDefined(value);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.ToObject(value, validate));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.ToObject(value, validate));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.ToObject(value, validate));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.ToObject(value, validate));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.ToObject(value, validate));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.ToObject(value, validate));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.ToObject(value, validate));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.ToObject(value, validate));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.ToObject(value, validate));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.ToObject(value, validate));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.ToObject(value, validate));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.ToObject(value, validate));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.ToObject(value, validate));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.ToObject(value, validate));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.ToObject(value, validate));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.ToObject(value, validate));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.ToObject(value, validate));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.ToObject(value, validate));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.ToObject(value, validate));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.ToObject(value, validate));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.ToObject(value, validate));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.ToObject(value, validate));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.ToObject(value, validate));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.ToObject(value, validate));
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
            bool success;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = Enums<TEnum, int>.Cache.TryToObject(value, out resultAsInt32, validate);
                    result = Enums<TEnum, int>.ToEnum(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = Enums<TEnum, uint>.Cache.TryToObject(value, out resultAsUInt32, validate);
                    result = Enums<TEnum, uint>.ToEnum(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = Enums<TEnum, long>.Cache.TryToObject(value, out resultAsInt64, validate);
                    result = Enums<TEnum, long>.ToEnum(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = Enums<TEnum, ulong>.Cache.TryToObject(value, out resultAsUInt64, validate);
                    result = Enums<TEnum, ulong>.ToEnum(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = Enums<TEnum, sbyte>.Cache.TryToObject(value, out resultAsSByte, validate);
                    result = Enums<TEnum, sbyte>.ToEnum(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = Enums<TEnum, byte>.Cache.TryToObject(value, out resultAsByte, validate);
                    result = Enums<TEnum, byte>.ToEnum(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = Enums<TEnum, short>.Cache.TryToObject(value, out resultAsInt16, validate);
                    result = Enums<TEnum, short>.ToEnum(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = Enums<TEnum, ushort>.Cache.TryToObject(value, out resultAsUInt16, validate);
                    result = Enums<TEnum, ushort>.ToEnum(resultAsUInt16);
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
            bool success;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = Enums<TEnum, int>.Cache.TryToObject(value, out resultAsInt32, validate);
                    result = Enums<TEnum, int>.ToEnum(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = Enums<TEnum, uint>.Cache.TryToObject(value, out resultAsUInt32, validate);
                    result = Enums<TEnum, uint>.ToEnum(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = Enums<TEnum, long>.Cache.TryToObject(value, out resultAsInt64, validate);
                    result = Enums<TEnum, long>.ToEnum(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = Enums<TEnum, ulong>.Cache.TryToObject(value, out resultAsUInt64, validate);
                    result = Enums<TEnum, ulong>.ToEnum(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = Enums<TEnum, sbyte>.Cache.TryToObject(value, out resultAsSByte, validate);
                    result = Enums<TEnum, sbyte>.ToEnum(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = Enums<TEnum, byte>.Cache.TryToObject(value, out resultAsByte, validate);
                    result = Enums<TEnum, byte>.ToEnum(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = Enums<TEnum, short>.Cache.TryToObject(value, out resultAsInt16, validate);
                    result = Enums<TEnum, short>.ToEnum(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = Enums<TEnum, ushort>.Cache.TryToObject(value, out resultAsUInt16, validate);
                    result = Enums<TEnum, ushort>.ToEnum(resultAsUInt16);
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
            bool success;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = Enums<TEnum, int>.Cache.TryToObject(value, out resultAsInt32, validate);
                    result = Enums<TEnum, int>.ToEnum(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = Enums<TEnum, uint>.Cache.TryToObject(value, out resultAsUInt32, validate);
                    result = Enums<TEnum, uint>.ToEnum(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = Enums<TEnum, long>.Cache.TryToObject(value, out resultAsInt64, validate);
                    result = Enums<TEnum, long>.ToEnum(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = Enums<TEnum, ulong>.Cache.TryToObject(value, out resultAsUInt64, validate);
                    result = Enums<TEnum, ulong>.ToEnum(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = Enums<TEnum, sbyte>.Cache.TryToObject(value, out resultAsSByte, validate);
                    result = Enums<TEnum, sbyte>.ToEnum(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = Enums<TEnum, byte>.Cache.TryToObject(value, out resultAsByte, validate);
                    result = Enums<TEnum, byte>.ToEnum(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = Enums<TEnum, short>.Cache.TryToObject(value, out resultAsInt16, validate);
                    result = Enums<TEnum, short>.ToEnum(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = Enums<TEnum, ushort>.Cache.TryToObject(value, out resultAsUInt16, validate);
                    result = Enums<TEnum, ushort>.ToEnum(resultAsUInt16);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    Enums<TEnum, int>.Cache.Validate(Enums<TEnum, int>.ToInt(value), paramName);
                    return value;
                case TypeCode.UInt32:
                    Enums<TEnum, uint>.Cache.Validate(Enums<TEnum, uint>.ToInt(value), paramName);
                    return value;
                case TypeCode.Int64:
                    Enums<TEnum, long>.Cache.Validate(Enums<TEnum, long>.ToInt(value), paramName);
                    return value;
                case TypeCode.UInt64:
                    Enums<TEnum, ulong>.Cache.Validate(Enums<TEnum, ulong>.ToInt(value), paramName);
                    return value;
                case TypeCode.SByte:
                    Enums<TEnum, sbyte>.Cache.Validate(Enums<TEnum, sbyte>.ToInt(value), paramName);
                    return value;
                case TypeCode.Byte:
                    Enums<TEnum, byte>.Cache.Validate(Enums<TEnum, byte>.ToInt(value), paramName);
                    return value;
                case TypeCode.Int16:
                    Enums<TEnum, short>.Cache.Validate(Enums<TEnum, short>.ToInt(value), paramName);
                    return value;
                case TypeCode.UInt16:
                    Enums<TEnum, ushort>.Cache.Validate(Enums<TEnum, ushort>.ToInt(value), paramName);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.AsString(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.AsString(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.AsString(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.AsString(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.AsString(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.AsString(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.AsString(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.AsString(Enums<TEnum, ushort>.ToInt(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.AsString(Enums<TEnum, int>.ToInt(value), format);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.AsString(Enums<TEnum, uint>.ToInt(value), format);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.AsString(Enums<TEnum, long>.ToInt(value), format);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.AsString(Enums<TEnum, ulong>.ToInt(value), format);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.AsString(Enums<TEnum, sbyte>.ToInt(value), format);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.AsString(Enums<TEnum, byte>.ToInt(value), format);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.AsString(Enums<TEnum, short>.ToInt(value), format);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.AsString(Enums<TEnum, ushort>.ToInt(value), format);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string AsString<TEnum>(TEnum value, params EnumFormat[] formats)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.AsString(Enums<TEnum, int>.ToInt(value), formats);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.AsString(Enums<TEnum, uint>.ToInt(value), formats);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.AsString(Enums<TEnum, long>.ToInt(value), formats);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.AsString(Enums<TEnum, ulong>.ToInt(value), formats);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.AsString(Enums<TEnum, sbyte>.ToInt(value), formats);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.AsString(Enums<TEnum, byte>.ToInt(value), formats);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.AsString(Enums<TEnum, short>.ToInt(value), formats);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.AsString(Enums<TEnum, ushort>.ToInt(value), formats);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.Format(Enums<TEnum, int>.ToInt(value), format);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.Format(Enums<TEnum, uint>.ToInt(value), format);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.Format(Enums<TEnum, long>.ToInt(value), format);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.Format(Enums<TEnum, ulong>.ToInt(value), format);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.Format(Enums<TEnum, sbyte>.ToInt(value), format);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.Format(Enums<TEnum, byte>.ToInt(value), format);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.Format(Enums<TEnum, short>.ToInt(value), format);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.Format(Enums<TEnum, ushort>.ToInt(value), format);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format<TEnum>(TEnum value, EnumFormat format)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch(Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.Format(Enums<TEnum, int>.ToInt(value), format);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.Format(Enums<TEnum, uint>.ToInt(value), format);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.Format(Enums<TEnum, long>.ToInt(value), format);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.Format(Enums<TEnum, ulong>.ToInt(value), format);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.Format(Enums<TEnum, sbyte>.ToInt(value), format);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.Format(Enums<TEnum, byte>.ToInt(value), format);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.Format(Enums<TEnum, short>.ToInt(value), format);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.Format(Enums<TEnum, ushort>.ToInt(value), format);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.Format(Enums<TEnum, int>.ToInt(value), format0, format1);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.Format(Enums<TEnum, uint>.ToInt(value), format0, format1);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.Format(Enums<TEnum, long>.ToInt(value), format0, format1);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.Format(Enums<TEnum, ulong>.ToInt(value), format0, format1);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.Format(Enums<TEnum, sbyte>.ToInt(value), format0, format1);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.Format(Enums<TEnum, byte>.ToInt(value), format0, format1);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.Format(Enums<TEnum, short>.ToInt(value), format0, format1);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.Format(Enums<TEnum, ushort>.ToInt(value), format0, format1);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.Format(Enums<TEnum, int>.ToInt(value), format0, format1, format2);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.Format(Enums<TEnum, uint>.ToInt(value), format0, format1, format2);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.Format(Enums<TEnum, long>.ToInt(value), format0, format1, format2);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.Format(Enums<TEnum, ulong>.ToInt(value), format0, format1, format2);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.Format(Enums<TEnum, sbyte>.ToInt(value), format0, format1, format2);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.Format(Enums<TEnum, byte>.ToInt(value), format0, format1, format2);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.Format(Enums<TEnum, short>.ToInt(value), format0, format1, format2);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.Format(Enums<TEnum, ushort>.ToInt(value), format0, format1, format2);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.Format(Enums<TEnum, int>.ToInt(value), format0, format1, format2, format3);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.Format(Enums<TEnum, uint>.ToInt(value), format0, format1, format2, format3);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.Format(Enums<TEnum, long>.ToInt(value), format0, format1, format2, format3);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.Format(Enums<TEnum, ulong>.ToInt(value), format0, format1, format2, format3);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.Format(Enums<TEnum, sbyte>.ToInt(value), format0, format1, format2, format3);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.Format(Enums<TEnum, byte>.ToInt(value), format0, format1, format2, format3);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.Format(Enums<TEnum, short>.ToInt(value), format0, format1, format2, format3);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.Format(Enums<TEnum, ushort>.ToInt(value), format0, format1, format2, format3);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.Format(Enums<TEnum, int>.ToInt(value), format0, format1, format2, format3, format4);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.Format(Enums<TEnum, uint>.ToInt(value), format0, format1, format2, format3, format4);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.Format(Enums<TEnum, long>.ToInt(value), format0, format1, format2, format3, format4);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.Format(Enums<TEnum, ulong>.ToInt(value), format0, format1, format2, format3, format4);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.Format(Enums<TEnum, sbyte>.ToInt(value), format0, format1, format2, format3, format4);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.Format(Enums<TEnum, byte>.ToInt(value), format0, format1, format2, format3, format4);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.Format(Enums<TEnum, short>.ToInt(value), format0, format1, format2, format3, format4);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.Format(Enums<TEnum, ushort>.ToInt(value), format0, format1, format2, format3, format4);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static string Format<TEnum>(TEnum value, params EnumFormat[] formats)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.Format(Enums<TEnum, int>.ToInt(value), formats);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.Format(Enums<TEnum, uint>.ToInt(value), formats);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.Format(Enums<TEnum, long>.ToInt(value), formats);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.Format(Enums<TEnum, ulong>.ToInt(value), formats);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.Format(Enums<TEnum, sbyte>.ToInt(value), formats);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.Format(Enums<TEnum, byte>.ToInt(value), formats);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.Format(Enums<TEnum, short>.ToInt(value), formats);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.Format(Enums<TEnum, ushort>.ToInt(value), formats);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.ToInt(value);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.ToInt(value);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.ToInt(value);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.ToInt(value);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.ToInt(value);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.ToInt(value);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.ToInt(value);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.ToInt(value);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToSByte(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToSByte(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToSByte(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToSByte(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToSByte(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToSByte(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToSByte(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToSByte(Enums<TEnum, ushort>.ToInt(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToByte(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToByte(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToByte(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToByte(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToByte(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToByte(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToByte(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToByte(Enums<TEnum, ushort>.ToInt(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToInt16(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToInt16(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToInt16(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToInt16(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToInt16(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToInt16(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToInt16(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToInt16(Enums<TEnum, ushort>.ToInt(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToUInt16(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToUInt16(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToUInt16(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToUInt16(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToUInt16(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToUInt16(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToUInt16(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToUInt16(Enums<TEnum, ushort>.ToInt(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToInt32(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToInt32(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToInt32(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToInt32(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToInt32(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToInt32(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToInt32(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToInt32(Enums<TEnum, ushort>.ToInt(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToUInt32(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToUInt32(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToUInt32(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToUInt32(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToUInt32(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToUInt32(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToUInt32(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToUInt32(Enums<TEnum, ushort>.ToInt(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToInt64(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToInt64(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToInt64(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToInt64(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToInt64(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToInt64(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToInt64(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToInt64(Enums<TEnum, ushort>.ToInt(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return EnumsCache<int>.ToUInt64(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return EnumsCache<uint>.ToUInt64(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return EnumsCache<long>.ToUInt64(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return EnumsCache<ulong>.ToUInt64(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return EnumsCache<sbyte>.ToUInt64(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return EnumsCache<byte>.ToUInt64(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return EnumsCache<short>.ToUInt64(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return EnumsCache<ushort>.ToUInt64(Enums<TEnum, ushort>.ToInt(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

        [Pure]
        public static int GetHashCode<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.ToInt(value);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.ToInt(value).GetHashCode();
                case TypeCode.Int64:
                    return Enums<TEnum, long>.ToInt(value).GetHashCode();
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.ToInt(value).GetHashCode();
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.ToInt(value);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.ToInt(value);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.ToInt(value);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.ToInt(value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return 0;
        }

        [Pure]
        public static bool Equals<TEnum>(TEnum value, TEnum other)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = Enums<TEnum, int>.ToInt;
                    return EnumsCache<int>.Equal(enumToInt32(value), enumToInt32(other));
                case TypeCode.UInt32:
                    var enumToUInt32 = Enums<TEnum, uint>.ToInt;
                    return EnumsCache<uint>.Equal(enumToUInt32(value), enumToUInt32(other));
                case TypeCode.Int64:
                    var enumToInt64 = Enums<TEnum, long>.ToInt;
                    return EnumsCache<long>.Equal(enumToInt64(value), enumToInt64(other));
                case TypeCode.UInt64:
                    var enumToUInt64 = Enums<TEnum, ulong>.ToInt;
                    return EnumsCache<ulong>.Equal(enumToUInt64(value), enumToUInt64(other));
                case TypeCode.SByte:
                    var enumToSByte = Enums<TEnum, sbyte>.ToInt;
                    return EnumsCache<sbyte>.Equal(enumToSByte(value), enumToSByte(other));
                case TypeCode.Byte:
                    var enumToByte = Enums<TEnum, byte>.ToInt;
                    return EnumsCache<byte>.Equal(enumToByte(value), enumToByte(other));
                case TypeCode.Int16:
                    var enumToInt16 = Enums<TEnum, short>.ToInt;
                    return EnumsCache<short>.Equal(enumToInt16(value), enumToInt16(other));
                case TypeCode.UInt16:
                    var enumToUInt16 = Enums<TEnum, ushort>.ToInt;
                    return EnumsCache<ushort>.Equal(enumToUInt16(value), enumToUInt16(other));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }
        #endregion

        #region Defined Values Main Methods
        [Pure]
        public static EnumMemberInfo<TEnum> GetEnumMemberInfo<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            IEnumMemberInfo info = null;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    info = Enums<TEnum, int>.Cache.GetEnumMemberInfo(Enums<TEnum, int>.ToInt(value));
                    break;
                case TypeCode.UInt32:
                    info = Enums<TEnum, uint>.Cache.GetEnumMemberInfo(Enums<TEnum, uint>.ToInt(value));
                    break;
                case TypeCode.Int64:
                    info = Enums<TEnum, long>.Cache.GetEnumMemberInfo(Enums<TEnum, long>.ToInt(value));
                    break;
                case TypeCode.UInt64:
                    info = Enums<TEnum, ulong>.Cache.GetEnumMemberInfo(Enums<TEnum, ulong>.ToInt(value));
                    break;
                case TypeCode.SByte:
                    info = Enums<TEnum, sbyte>.Cache.GetEnumMemberInfo(Enums<TEnum, sbyte>.ToInt(value));
                    break;
                case TypeCode.Byte:
                    info = Enums<TEnum, byte>.Cache.GetEnumMemberInfo(Enums<TEnum, byte>.ToInt(value));
                    break;
                case TypeCode.Int16:
                    info = Enums<TEnum, short>.Cache.GetEnumMemberInfo(Enums<TEnum, short>.ToInt(value));
                    break;
                case TypeCode.UInt16:
                    info = Enums<TEnum, ushort>.Cache.GetEnumMemberInfo(Enums<TEnum, ushort>.ToInt(value));
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
            IEnumMemberInfo info = null;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    info = Enums<TEnum, int>.Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.UInt32:
                    info = Enums<TEnum, uint>.Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.Int64:
                    info = Enums<TEnum, long>.Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.UInt64:
                    info = Enums<TEnum, ulong>.Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.SByte:
                    info = Enums<TEnum, sbyte>.Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.Byte:
                    info = Enums<TEnum, byte>.Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.Int16:
                    info = Enums<TEnum, short>.Cache.GetEnumMemberInfo(name, ignoreCase);
                    break;
                case TypeCode.UInt16:
                    info = Enums<TEnum, ushort>.Cache.GetEnumMemberInfo(name, ignoreCase);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetName(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetName(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetName(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetName(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetName(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetName(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetName(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetName(Enums<TEnum, ushort>.ToInt(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetDescription(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetDescription(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetDescription(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetDescription(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetDescription(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetDescription(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetDescription(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetDescription(Enums<TEnum, ushort>.ToInt(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Retrieves the description if not null else the name of the specified <paramref name="value"/> if defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static string GetDescriptionOrName<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetDescriptionOrName(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetDescriptionOrName(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetDescriptionOrName(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetDescriptionOrName(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetDescriptionOrName(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetDescriptionOrName(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetDescriptionOrName(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetDescriptionOrName(Enums<TEnum, ushort>.ToInt(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Retrieves the description if not null else the name formatted with <paramref name="nameFormatter"/> of the specified <paramref name="value"/> if defined.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="nameFormatter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        public static string GetDescriptionOrName<TEnum>(TEnum value, Func<string, string> nameFormatter)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetDescriptionOrName(Enums<TEnum, int>.ToInt(value), nameFormatter);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetDescriptionOrName(Enums<TEnum, uint>.ToInt(value), nameFormatter);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetDescriptionOrName(Enums<TEnum, long>.ToInt(value), nameFormatter);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetDescriptionOrName(Enums<TEnum, ulong>.ToInt(value), nameFormatter);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetDescriptionOrName(Enums<TEnum, sbyte>.ToInt(value), nameFormatter);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetDescriptionOrName(Enums<TEnum, byte>.ToInt(value), nameFormatter);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetDescriptionOrName(Enums<TEnum, short>.ToInt(value), nameFormatter);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetDescriptionOrName(Enums<TEnum, ushort>.ToInt(value), nameFormatter);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetAttribute<TAttribute>(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetAttribute<TAttribute>(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetAttribute<TAttribute>(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetAttribute<TAttribute>(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetAttribute<TAttribute>(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetAttribute<TAttribute>(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetAttribute<TAttribute>(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetAttribute<TAttribute>(Enums<TEnum, ushort>.ToInt(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetAttributeSelect(Enums<TEnum, int>.ToInt(value), selector, defaultValue);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetAttributeSelect(Enums<TEnum, uint>.ToInt(value), selector, defaultValue);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetAttributeSelect(Enums<TEnum, long>.ToInt(value), selector, defaultValue);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetAttributeSelect(Enums<TEnum, ulong>.ToInt(value), selector, defaultValue);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetAttributeSelect(Enums<TEnum, sbyte>.ToInt(value), selector, defaultValue);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetAttributeSelect(Enums<TEnum, byte>.ToInt(value), selector, defaultValue);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetAttributeSelect(Enums<TEnum, short>.ToInt(value), selector, defaultValue);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetAttributeSelect(Enums<TEnum, ushort>.ToInt(value), selector, defaultValue);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TResult);
        }

        [Pure]
        public static bool TryGetAttributeSelect<TEnum, TAttribute, TResult>(this TEnum value, Func<TAttribute, TResult> selector, out TResult result)
            where TAttribute : Attribute
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.TryGetAttributeSelect(Enums<TEnum, int>.ToInt(value), selector, out result);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.TryGetAttributeSelect(Enums<TEnum, uint>.ToInt(value), selector, out result);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.TryGetAttributeSelect(Enums<TEnum, long>.ToInt(value), selector, out result);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.TryGetAttributeSelect(Enums<TEnum, ulong>.ToInt(value), selector, out result);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.TryGetAttributeSelect(Enums<TEnum, sbyte>.ToInt(value), selector, out result);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.TryGetAttributeSelect(Enums<TEnum, byte>.ToInt(value), selector, out result);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.TryGetAttributeSelect(Enums<TEnum, short>.ToInt(value), selector, out result);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.TryGetAttributeSelect(Enums<TEnum, ushort>.ToInt(value), selector, out result);
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetAttributes<TAttribute>(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetAttributes<TAttribute>(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetAttributes<TAttribute>(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetAttributes<TAttribute>(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetAttributes<TAttribute>(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetAttributes<TAttribute>(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetAttributes<TAttribute>(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetAttributes<TAttribute>(Enums<TEnum, ushort>.ToInt(value));
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
        public static Attribute[] GetAttributes<TEnum>(TEnum value)
        {
            VerifyTypeIsEnum(typeof(TEnum));
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetAttributes(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetAttributes(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetAttributes(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetAttributes(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetAttributes(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetAttributes(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetAttributes(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetAttributes(Enums<TEnum, ushort>.ToInt(value));
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
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.Parse(value, ignoreCase, parseFormatOrder));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.Parse(value, ignoreCase, parseFormatOrder));
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
            bool success;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = Enums<TEnum, int>.Cache.TryParse(value, ignoreCase, out resultAsInt32, parseFormatOrder);
                    result = Enums<TEnum, int>.ToEnum(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = Enums<TEnum, uint>.Cache.TryParse(value, ignoreCase, out resultAsUInt32, parseFormatOrder);
                    result = Enums<TEnum, uint>.ToEnum(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = Enums<TEnum, long>.Cache.TryParse(value, ignoreCase, out resultAsInt64, parseFormatOrder);
                    result = Enums<TEnum, long>.ToEnum(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = Enums<TEnum, ulong>.Cache.TryParse(value, ignoreCase, out resultAsUInt64, parseFormatOrder);
                    result = Enums<TEnum, ulong>.ToEnum(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = Enums<TEnum, sbyte>.Cache.TryParse(value, ignoreCase, out resultAsSByte, parseFormatOrder);
                    result = Enums<TEnum, sbyte>.ToEnum(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = Enums<TEnum, byte>.Cache.TryParse(value, ignoreCase, out resultAsByte, parseFormatOrder);
                    result = Enums<TEnum, byte>.ToEnum(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = Enums<TEnum, short>.Cache.TryParse(value, ignoreCase, out resultAsInt16, parseFormatOrder);
                    result = Enums<TEnum, short>.ToEnum(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = Enums<TEnum, ushort>.Cache.TryParse(value, ignoreCase, out resultAsUInt16, parseFormatOrder);
                    result = Enums<TEnum, ushort>.ToEnum(resultAsUInt16);
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
