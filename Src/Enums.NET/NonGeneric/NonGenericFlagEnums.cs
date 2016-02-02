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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

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
        public static bool IsFlagEnum(Type enumType)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).IsFlagEnum;
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).IsFlagEnum;
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).IsFlagEnum;
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).IsFlagEnum;
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).IsFlagEnum;
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).IsFlagEnum;
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).IsFlagEnum;
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).IsFlagEnum;
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

        /// <summary>
        /// Retrieves all the flags defined by <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns>All the flags defined by <paramref name="enumType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type</exception>
        [Pure]
        public static object GetAllFlags(Type enumType)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).AllFlags);
                case TypeCode.UInt32:
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).AllFlags);
                case TypeCode.Int64:
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).AllFlags);
                case TypeCode.UInt64:
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).AllFlags);
                case TypeCode.SByte:
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).AllFlags);
                case TypeCode.Byte:
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).AllFlags);
                case TypeCode.Int16:
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).AllFlags);
                case TypeCode.UInt16:
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).AllFlags);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }
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
        public static bool IsValidFlagCombination(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).IsValidFlagCombination(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).IsValidFlagCombination(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).IsValidFlagCombination(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).IsValidFlagCombination(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).IsValidFlagCombination(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).IsValidFlagCombination(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).IsValidFlagCombination(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).IsValidFlagCombination(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

        [Pure]
        public static string FormatAsFlags(Type enumType, object value) => FormatAsFlags(enumType, value, null, null);

        [Pure]
        public static string FormatAsFlags(Type enumType, object value, params EnumFormat[] formats) => FormatAsFlags(enumType, value, null, formats);

        [Pure]
        public static string FormatAsFlags(Type enumType, object value, string delimiter) => FormatAsFlags(enumType, value, delimiter, null);

        [Pure]
        public static string FormatAsFlags(Type enumType, object value, string delimiter, params EnumFormat[] formats)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).FormatAsFlags(((Func<object, int>)toInt)(value), delimiter, formats);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).FormatAsFlags(((Func<object, uint>)toInt)(value), delimiter, formats);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).FormatAsFlags(((Func<object, long>)toInt)(value), delimiter, formats);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).FormatAsFlags(((Func<object, ulong>)toInt)(value), delimiter, formats);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).FormatAsFlags(((Func<object, sbyte>)toInt)(value), delimiter, formats);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).FormatAsFlags(((Func<object, byte>)toInt)(value), delimiter, formats);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).FormatAsFlags(((Func<object, short>)toInt)(value), delimiter, formats);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).FormatAsFlags(((Func<object, ushort>)toInt)(value), delimiter, formats);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

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
        public static object[] GetFlags(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32ToEnum = (Func<int, object>)toEnum;
                    return ((EnumsCache<int>)cache).GetFlags(((Func<object, int>)toInt)(value))?.Select(flag => int32ToEnum(flag)).ToArray();
                case TypeCode.UInt32:
                    var uint32ToEnum = (Func<uint, object>)toEnum;
                    return ((EnumsCache<uint>)cache).GetFlags(((Func<object, uint>)toInt)(value))?.Select(flag => uint32ToEnum(flag)).ToArray();
                case TypeCode.Int64:
                    var int64ToEnum = (Func<long, object>)toEnum;
                    return ((EnumsCache<long>)cache).GetFlags(((Func<object, long>)toInt)(value))?.Select(flag => int64ToEnum(flag)).ToArray();
                case TypeCode.UInt64:
                    var uint64ToEnum = (Func<ulong, object>)toEnum;
                    return ((EnumsCache<ulong>)cache).GetFlags(((Func<object, ulong>)toInt)(value))?.Select(flag => uint64ToEnum(flag)).ToArray();
                case TypeCode.SByte:
                    var sbyteToEnum = (Func<sbyte, object>)toEnum;
                    return ((EnumsCache<sbyte>)cache).GetFlags(((Func<object, sbyte>)toInt)(value))?.Select(flag => sbyteToEnum(flag)).ToArray();
                case TypeCode.Byte:
                    var byteToEnum = (Func<byte, object>)toEnum;
                    return ((EnumsCache<byte>)cache).GetFlags(((Func<object, byte>)toInt)(value))?.Select(flag => byteToEnum(flag)).ToArray();
                case TypeCode.Int16:
                    var int16ToEnum = (Func<short, object>)toEnum;
                    return ((EnumsCache<short>)cache).GetFlags(((Func<object, short>)toInt)(value))?.Select(flag => int16ToEnum(flag)).ToArray();
                case TypeCode.UInt16:
                    var uint16ToEnum = (Func<ushort, object>)toEnum;
                    return ((EnumsCache<ushort>)cache).GetFlags(((Func<object, ushort>)toInt)(value))?.Select(flag => uint16ToEnum(flag)).ToArray();
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

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
        public static bool HasAnyFlags(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).HasAnyFlags(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).HasAnyFlags(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).HasAnyFlags(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).HasAnyFlags(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).HasAnyFlags(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).HasAnyFlags(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).HasAnyFlags(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).HasAnyFlags(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

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
        public static bool HasAnyFlags(Type enumType, object value, object flagMask)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<object, int>)toInt;
                    return ((EnumsCache<int>)cache).HasAnyFlags(enumToInt32(value), enumToInt32(flagMask));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<object, uint>)toInt;
                    return ((EnumsCache<uint>)cache).HasAnyFlags(enumToUInt32(value), enumToUInt32(flagMask));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<object, long>)toInt;
                    return ((EnumsCache<long>)cache).HasAnyFlags(enumToInt64(value), enumToInt64(flagMask));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<object, ulong>)toInt;
                    return ((EnumsCache<ulong>)cache).HasAnyFlags(enumToUInt64(value), enumToUInt64(flagMask));
                case TypeCode.SByte:
                    var enumToSByte = (Func<object, sbyte>)toInt;
                    return ((EnumsCache<sbyte>)cache).HasAnyFlags(enumToSByte(value), enumToSByte(flagMask));
                case TypeCode.Byte:
                    var enumToByte = (Func<object, byte>)toInt;
                    return ((EnumsCache<byte>)cache).HasAnyFlags(enumToByte(value), enumToByte(flagMask));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<object, short>)toInt;
                    return ((EnumsCache<short>)cache).HasAnyFlags(enumToInt16(value), enumToInt16(flagMask));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<object, ushort>)toInt;
                    return ((EnumsCache<ushort>)cache).HasAnyFlags(enumToUInt16(value), enumToUInt16(flagMask));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

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
        public static bool HasAllFlags(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).HasAllFlags(((Func<object, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).HasAllFlags(((Func<object, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).HasAllFlags(((Func<object, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).HasAllFlags(((Func<object, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).HasAllFlags(((Func<object, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).HasAllFlags(((Func<object, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).HasAllFlags(((Func<object, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).HasAllFlags(((Func<object, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

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
        public static bool HasAllFlags(Type enumType, object value, object flagMask)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<object, int>)toInt;
                    return ((EnumsCache<int>)cache).HasAllFlags(enumToInt32(value), enumToInt32(flagMask));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<object, uint>)toInt;
                    return ((EnumsCache<uint>)cache).HasAllFlags(enumToUInt32(value), enumToUInt32(flagMask));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<object, long>)toInt;
                    return ((EnumsCache<long>)cache).HasAllFlags(enumToInt64(value), enumToInt64(flagMask));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<object, ulong>)toInt;
                    return ((EnumsCache<ulong>)cache).HasAllFlags(enumToUInt64(value), enumToUInt64(flagMask));
                case TypeCode.SByte:
                    var enumToSByte = (Func<object, sbyte>)toInt;
                    return ((EnumsCache<sbyte>)cache).HasAllFlags(enumToSByte(value), enumToSByte(flagMask));
                case TypeCode.Byte:
                    var enumToByte = (Func<object, byte>)toInt;
                    return ((EnumsCache<byte>)cache).HasAllFlags(enumToByte(value), enumToByte(flagMask));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<object, short>)toInt;
                    return ((EnumsCache<short>)cache).HasAllFlags(enumToInt16(value), enumToInt16(flagMask));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<object, ushort>)toInt;
                    return ((EnumsCache<ushort>)cache).HasAllFlags(enumToUInt16(value), enumToUInt16(flagMask));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

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
        public static object InvertFlags(Type enumType, object value)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).InvertFlags(((Func<object, int>)toInt)(value)));
                case TypeCode.UInt32:
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).InvertFlags(((Func<object, uint>)toInt)(value)));
                case TypeCode.Int64:
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).InvertFlags(((Func<object, long>)toInt)(value)));
                case TypeCode.UInt64:
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).InvertFlags(((Func<object, ulong>)toInt)(value)));
                case TypeCode.SByte:
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).InvertFlags(((Func<object, sbyte>)toInt)(value)));
                case TypeCode.Byte:
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).InvertFlags(((Func<object, byte>)toInt)(value)));
                case TypeCode.Int16:
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).InvertFlags(((Func<object, short>)toInt)(value)));
                case TypeCode.UInt16:
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).InvertFlags(((Func<object, ushort>)toInt)(value)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

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
        public static object InvertFlags(Type enumType, object value, object flagMask)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<object, int>)toInt;
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).InvertFlags(enumToInt32(value), enumToInt32(flagMask)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<object, uint>)toInt;
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).InvertFlags(enumToUInt32(value), enumToUInt32(flagMask)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<object, long>)toInt;
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).InvertFlags(enumToInt64(value), enumToInt64(flagMask)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<object, ulong>)toInt;
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).InvertFlags(enumToUInt64(value), enumToUInt64(flagMask)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<object, sbyte>)toInt;
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).InvertFlags(enumToSByte(value), enumToSByte(flagMask)));
                case TypeCode.Byte:
                    var enumToByte = (Func<object, byte>)toInt;
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).InvertFlags(enumToByte(value), enumToByte(flagMask)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<object, short>)toInt;
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).InvertFlags(enumToInt16(value), enumToInt16(flagMask)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<object, ushort>)toInt;
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).InvertFlags(enumToUInt16(value), enumToUInt16(flagMask)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

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
        public static object CommonFlags(Type enumType, object value, object flagMask)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<object, int>)toInt;
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).CommonFlags(enumToInt32(value), enumToInt32(flagMask)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<object, uint>)toInt;
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).CommonFlags(enumToUInt32(value), enumToUInt32(flagMask)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<object, long>)toInt;
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).CommonFlags(enumToInt64(value), enumToInt64(flagMask)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<object, ulong>)toInt;
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).CommonFlags(enumToUInt64(value), enumToUInt64(flagMask)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<object, sbyte>)toInt;
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).CommonFlags(enumToSByte(value), enumToSByte(flagMask)));
                case TypeCode.Byte:
                    var enumToByte = (Func<object, byte>)toInt;
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).CommonFlags(enumToByte(value), enumToByte(flagMask)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<object, short>)toInt;
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).CommonFlags(enumToInt16(value), enumToInt16(flagMask)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<object, ushort>)toInt;
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).CommonFlags(enumToUInt16(value), enumToUInt16(flagMask)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

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
        public static object SetFlags(Type enumType, object flag0, object flag1)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<object, int>)toInt;
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).SetFlags(enumToInt32(flag0), enumToInt32(flag1)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<object, uint>)toInt;
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).SetFlags(enumToUInt32(flag0), enumToUInt32(flag1)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<object, long>)toInt;
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).SetFlags(enumToInt64(flag0), enumToInt64(flag1)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<object, ulong>)toInt;
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).SetFlags(enumToUInt64(flag0), enumToUInt64(flag1)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<object, sbyte>)toInt;
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).SetFlags(enumToSByte(flag0), enumToSByte(flag1)));
                case TypeCode.Byte:
                    var enumToByte = (Func<object, byte>)toInt;
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).SetFlags(enumToByte(flag0), enumToByte(flag1)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<object, short>)toInt;
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).SetFlags(enumToInt16(flag0), enumToInt16(flag1)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<object, ushort>)toInt;
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).SetFlags(enumToUInt16(flag0), enumToUInt16(flag1)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static object SetFlags(Type enumType, object flag0, object flag1, object flag2)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<object, int>)toInt;
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).SetFlags(enumToInt32(flag0), enumToInt32(flag1), enumToInt32(flag2)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<object, uint>)toInt;
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).SetFlags(enumToUInt32(flag0), enumToUInt32(flag1), enumToUInt32(flag2)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<object, long>)toInt;
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).SetFlags(enumToInt64(flag0), enumToInt64(flag1), enumToInt64(flag2)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<object, ulong>)toInt;
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).SetFlags(enumToUInt64(flag0), enumToUInt64(flag1), enumToUInt64(flag2)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<object, sbyte>)toInt;
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).SetFlags(enumToSByte(flag0), enumToSByte(flag1), enumToSByte(flag2)));
                case TypeCode.Byte:
                    var enumToByte = (Func<object, byte>)toInt;
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).SetFlags(enumToByte(flag0), enumToByte(flag1), enumToByte(flag2)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<object, short>)toInt;
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).SetFlags(enumToInt16(flag0), enumToInt16(flag1), enumToInt16(flag2)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<object, ushort>)toInt;
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).SetFlags(enumToUInt16(flag0), enumToUInt16(flag1), enumToUInt16(flag2)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static object SetFlags(Type enumType, object flag0, object flag1, object flag2, object flag3)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<object, int>)toInt;
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).SetFlags(enumToInt32(flag0), enumToInt32(flag1), enumToInt32(flag2), enumToInt32(flag3)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<object, uint>)toInt;
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).SetFlags(enumToUInt32(flag0), enumToUInt32(flag1), enumToUInt32(flag2), enumToUInt32(flag3)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<object, long>)toInt;
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).SetFlags(enumToInt64(flag0), enumToInt64(flag1), enumToInt64(flag2), enumToInt64(flag3)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<object, ulong>)toInt;
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).SetFlags(enumToUInt64(flag0), enumToUInt64(flag1), enumToUInt64(flag2), enumToUInt64(flag3)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<object, sbyte>)toInt;
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).SetFlags(enumToSByte(flag0), enumToSByte(flag1), enumToSByte(flag2), enumToSByte(flag3)));
                case TypeCode.Byte:
                    var enumToByte = (Func<object, byte>)toInt;
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).SetFlags(enumToByte(flag0), enumToByte(flag1), enumToByte(flag2), enumToByte(flag3)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<object, short>)toInt;
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).SetFlags(enumToInt16(flag0), enumToInt16(flag1), enumToInt16(flag2), enumToInt16(flag3)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<object, ushort>)toInt;
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).SetFlags(enumToUInt16(flag0), enumToUInt16(flag1), enumToUInt16(flag2), enumToUInt16(flag3)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static object SetFlags(Type enumType, object flag0, object flag1, object flag2, object flag3, object flag4)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<object, int>)toInt;
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).SetFlags(enumToInt32(flag0), enumToInt32(flag1), enumToInt32(flag2), enumToInt32(flag3), enumToInt32(flag4)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<object, uint>)toInt;
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).SetFlags(enumToUInt32(flag0), enumToUInt32(flag1), enumToUInt32(flag2), enumToUInt32(flag3), enumToUInt32(flag4)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<object, long>)toInt;
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).SetFlags(enumToInt64(flag0), enumToInt64(flag1), enumToInt64(flag2), enumToInt64(flag3), enumToInt64(flag4)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<object, ulong>)toInt;
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).SetFlags(enumToUInt64(flag0), enumToUInt64(flag1), enumToUInt64(flag2), enumToUInt64(flag3), enumToUInt64(flag4)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<object, sbyte>)toInt;
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).SetFlags(enumToSByte(flag0), enumToSByte(flag1), enumToSByte(flag2), enumToSByte(flag3), enumToSByte(flag4)));
                case TypeCode.Byte:
                    var enumToByte = (Func<object, byte>)toInt;
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).SetFlags(enumToByte(flag0), enumToByte(flag1), enumToByte(flag2), enumToByte(flag3), enumToByte(flag4)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<object, short>)toInt;
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).SetFlags(enumToInt16(flag0), enumToInt16(flag1), enumToInt16(flag2), enumToInt16(flag3), enumToInt16(flag4)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<object, ushort>)toInt;
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).SetFlags(enumToUInt16(flag0), enumToUInt16(flag1), enumToUInt16(flag2), enumToUInt16(flag3), enumToUInt16(flag4)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static object SetFlags(Type enumType, params object[] flags)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<object, int>)toInt;
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).SetFlags(flags.Select(flag => enumToInt32(flag))));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<object, uint>)toInt;
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).SetFlags(flags.Select(flag => enumToUInt32(flag))));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<object, long>)toInt;
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).SetFlags(flags.Select(flag => enumToInt64(flag))));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<object, ulong>)toInt;
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).SetFlags(flags.Select(flag => enumToUInt64(flag))));
                case TypeCode.SByte:
                    var enumToSByte = (Func<object, sbyte>)toInt;
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).SetFlags(flags.Select(flag => enumToSByte(flag))));
                case TypeCode.Byte:
                    var enumToByte = (Func<object, byte>)toInt;
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).SetFlags(flags.Select(flag => enumToByte(flag))));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<object, short>)toInt;
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).SetFlags(flags.Select(flag => enumToInt16(flag))));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<object, ushort>)toInt;
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).SetFlags(flags.Select(flag => enumToUInt16(flag))));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

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
        public static object ClearFlags(Type enumType, object value, object flagMask)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toInt = enumsCache.ToInt;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<object, int>)toInt;
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).ClearFlags(enumToInt32(value), enumToInt32(flagMask)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<object, uint>)toInt;
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).ClearFlags(enumToUInt32(value), enumToUInt32(flagMask)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<object, long>)toInt;
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).ClearFlags(enumToInt64(value), enumToInt64(flagMask)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<object, ulong>)toInt;
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).ClearFlags(enumToUInt64(value), enumToUInt64(flagMask)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<object, sbyte>)toInt;
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).ClearFlags(enumToSByte(value), enumToSByte(flagMask)));
                case TypeCode.Byte:
                    var enumToByte = (Func<object, byte>)toInt;
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).ClearFlags(enumToByte(value), enumToByte(flagMask)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<object, short>)toInt;
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).ClearFlags(enumToInt16(value), enumToInt16(flagMask)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<object, ushort>)toInt;
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).ClearFlags(enumToUInt16(value), enumToUInt16(flagMask)));
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
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> or <paramref name="value"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <paramref name="enumType"/>.</exception>
        [Pure]
        public static object Parse(Type enumType, string value) => Parse(enumType, value, false, null, null);

        [Pure]
        public static object Parse(Type enumType, string value, params EnumFormat[] parseFormatOrder) => Parse(enumType, value, false, null, parseFormatOrder);

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
        public static object Parse(Type enumType, string value, bool ignoreCase) => Parse(enumType, value, ignoreCase, null, null);

        [Pure]
        public static object Parse(Type enumType, string value, bool ignoreCase, params EnumFormat[] parseFormatOrder) => Parse(enumType, value, ignoreCase, null, parseFormatOrder);

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
        public static object Parse(Type enumType, string value, string delimiter) => Parse(enumType, value, false, delimiter, null);

        [Pure]
        public static object Parse(Type enumType, string value, string delimiter, params EnumFormat[] parseFormatOrder) => Parse(enumType, value, false, delimiter, parseFormatOrder);

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
        public static object Parse(Type enumType, string value, bool ignoreCase, string delimiter) => Parse(enumType, value, ignoreCase, delimiter, null);

        [Pure]
        public static object Parse(Type enumType, string value, bool ignoreCase, string delimiter, params EnumFormat[] parseFormatOrder)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, object>)toEnum)(((EnumsCache<int>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.UInt32:
                    return ((Func<uint, object>)toEnum)(((EnumsCache<uint>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.Int64:
                    return ((Func<long, object>)toEnum)(((EnumsCache<long>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.UInt64:
                    return ((Func<ulong, object>)toEnum)(((EnumsCache<ulong>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.SByte:
                    return ((Func<sbyte, object>)toEnum)(((EnumsCache<sbyte>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.Byte:
                    return ((Func<byte, object>)toEnum)(((EnumsCache<byte>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.Int16:
                    return ((Func<short, object>)toEnum)(((EnumsCache<short>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.UInt16:
                    return ((Func<ushort, object>)toEnum)(((EnumsCache<ushort>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

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
        public static object ParseOrDefault(Type enumType, string value, object defaultEnum = null) => ParseOrDefault(enumType, value, false, null, defaultEnum, null);

        [Pure]
        public static object ParseOrDefault(Type enumType, string value, object defaultEnum, params EnumFormat[] parseFormatOrder) => ParseOrDefault(enumType, value, false, null, defaultEnum, parseFormatOrder);

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
        public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, object defaultEnum = null) => ParseOrDefault(enumType, value, ignoreCase, null, defaultEnum, null);

        [Pure]
        public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, object defaultEnum, params EnumFormat[] parseFormatOrder) => ParseOrDefault(enumType, value, ignoreCase, null, defaultEnum, parseFormatOrder);

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
        public static object ParseOrDefault(Type enumType, string value, string delimiter, object defaultEnum = null) => ParseOrDefault(enumType, value, false, delimiter, defaultEnum, null);

        [Pure]
        public static object ParseOrDefault(Type enumType, string value, string delimiter, object defaultEnum, params EnumFormat[] parseFormatOrder) => ParseOrDefault(enumType, value, false, delimiter, defaultEnum, parseFormatOrder);

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
        public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, string delimiter, object defaultEnum = null) => ParseOrDefault(enumType, value, ignoreCase, delimiter, defaultEnum, null);

        [Pure]
        public static object ParseOrDefault(Type enumType, string value, bool ignoreCase, string delimiter, object defaultEnum, params EnumFormat[] parseFormatOrder)
        {
            object result;
            return TryParse(enumType, value, ignoreCase, delimiter, out result, parseFormatOrder) ? result : defaultEnum;
        }

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
        public static bool TryParse(Type enumType, string value, out object result) => TryParse(enumType, value, false, null, out result, null);

        [Pure]
        public static bool TryParse(Type enumType, string value, out object result, params EnumFormat[] parseFormatOrder) => TryParse(enumType, value, false, null, out result, parseFormatOrder);

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
        public static bool TryParse(Type enumType, string value, bool ignoreCase, out object result) => TryParse(enumType, value, ignoreCase, null, out result, null);

        [Pure]
        public static bool TryParse(Type enumType, string value, bool ignoreCase, out object result, params EnumFormat[] parseFormatOrder) => TryParse(enumType, value, ignoreCase, null, out result, parseFormatOrder);

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
        public static bool TryParse(Type enumType, string value, string delimiter, out object result) => TryParse(enumType, value, false, delimiter, out result, null);

        [Pure]
        public static bool TryParse(Type enumType, string value, string delimiter, out object result, params EnumFormat[] parseFormatOrder) => TryParse(enumType, value, false, delimiter, out result, parseFormatOrder);

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
        public static bool TryParse(Type enumType, string value, bool ignoreCase, string delimiter, out object result) => TryParse(enumType, value, ignoreCase, delimiter, out result, null);

        [Pure]
        public static bool TryParse(Type enumType, string value, bool ignoreCase, string delimiter, out object result, params EnumFormat[] parseFormatOrder)
        {
            var enumsCache = NonGenericEnumsCache.Get(enumType);
            var cache = enumsCache.Cache;
            var toEnum = enumsCache.ToEnum;
            var success = false;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = ((EnumsCache<int>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsInt32, parseFormatOrder);
                    result = ((Func<int, object>)toEnum)(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = ((EnumsCache<uint>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsUInt32, parseFormatOrder);
                    result = ((Func<uint, object>)toEnum)(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = ((EnumsCache<long>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsInt64, parseFormatOrder);
                    result = ((Func<long, object>)toEnum)(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = ((EnumsCache<ulong>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsUInt64, parseFormatOrder);
                    result = ((Func<ulong, object>)toEnum)(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = ((EnumsCache<sbyte>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsSByte, parseFormatOrder);
                    result = ((Func<sbyte, object>)toEnum)(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = ((EnumsCache<byte>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsByte, parseFormatOrder);
                    result = ((Func<byte, object>)toEnum)(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = ((EnumsCache<short>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsInt16, parseFormatOrder);
                    result = ((Func<short, object>)toEnum)(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = ((EnumsCache<ushort>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsUInt16, parseFormatOrder);
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
