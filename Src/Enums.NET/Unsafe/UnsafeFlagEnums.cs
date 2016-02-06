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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace EnumsNET.Unsafe
{
    /// <summary>
    /// Identical to <see cref="FlagEnums"/> but is not type safe which is useful when dealing with generics and instead throws an <see cref="ArgumentException"/> if TEnum is not an enum/>
    /// </summary>
    public static class UnsafeFlagEnums
    {
        #region "Properties"
        /// <summary>
        /// Indicates if <typeparamref name="TEnum"/> is marked with the <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns>Indication if <typeparamref name="TEnum"/> is marked with the <see cref="FlagsAttribute"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsFlagEnum<TEnum>()
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            switch (Enums<TEnum>.TypeCode)
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
        /// Retrieves all the flags defined by <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns>All the flags defined by <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum GetAllFlags<TEnum>()
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).AllFlags);
                case TypeCode.UInt32:
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).AllFlags);
                case TypeCode.Int64:
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).AllFlags);
                case TypeCode.UInt64:
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).AllFlags);
                case TypeCode.SByte:
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).AllFlags);
                case TypeCode.Byte:
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).AllFlags);
                case TypeCode.Int16:
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).AllFlags);
                case TypeCode.UInt16:
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).AllFlags);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// Indicates whether <paramref name="value"/> is a valid flag combination of the defined enum values.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns>Indication of whether <paramref name="value"/> is a valid flag combination of the defined enum values.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool IsValidFlagCombination<TEnum>(TEnum value)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).IsValidFlagCombination(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).IsValidFlagCombination(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).IsValidFlagCombination(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).IsValidFlagCombination(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).IsValidFlagCombination(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).IsValidFlagCombination(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).IsValidFlagCombination(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).IsValidFlagCombination(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

        /// <summary>
        /// Returns the names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination null is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Should be a valid flag combination.</param>
        /// <returns>The names of <paramref name="value"/>'s flags delimited with commas or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination null is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static string FormatAsFlags<TEnum>(TEnum value) => FormatAsFlags(value, null, null);

        [Pure]
        public static string FormatAsFlags<TEnum>(TEnum value, params EnumFormat[] formats) => FormatAsFlags(value, null, formats);

        /// <summary>
        /// Returns the names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Should be a valid flag combination.</param>
        /// <param name="delimiter">The delimiter to use to separate individual flag names. Cannot be null or empty.</param>
        /// <returns>The names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination null is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="delimiter"/> is empty</exception>
        /// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
        [Pure]
        public static string FormatAsFlags<TEnum>(TEnum value, string delimiter) => FormatAsFlags(value, delimiter, null);

        [Pure]
        public static string FormatAsFlags<TEnum>(TEnum value, string delimiter, params EnumFormat[] formats)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).FormatAsFlags(((Func<TEnum, int>)toInt)(value), delimiter, formats);
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).FormatAsFlags(((Func<TEnum, uint>)toInt)(value), delimiter, formats);
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).FormatAsFlags(((Func<TEnum, long>)toInt)(value), delimiter, formats);
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).FormatAsFlags(((Func<TEnum, ulong>)toInt)(value), delimiter, formats);
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).FormatAsFlags(((Func<TEnum, sbyte>)toInt)(value), delimiter, formats);
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).FormatAsFlags(((Func<TEnum, byte>)toInt)(value), delimiter, formats);
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).FormatAsFlags(((Func<TEnum, short>)toInt)(value), delimiter, formats);
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).FormatAsFlags(((Func<TEnum, ushort>)toInt)(value), delimiter, formats);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Returns an array of the flags that compose <paramref name="value"/>.
        /// If <paramref name="value"/> is not a valid flag combination null is returned.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Should be a valid flag combination.</param>
        /// <returns>Array of the flags that compose <paramref name="value"/>.
        /// If <paramref name="value"/> is not a valid flag combination null is returned.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum[] GetFlags<TEnum>(TEnum value)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var int32ToEnum = (Func<int, TEnum>)toEnum;
                    return ((EnumsCache<int>)cache).GetFlags(((Func<TEnum, int>)toInt)(value))?.Select(flag => int32ToEnum(flag)).ToArray();
                case TypeCode.UInt32:
                    var uint32ToEnum = (Func<uint, TEnum>)toEnum;
                    return ((EnumsCache<uint>)cache).GetFlags(((Func<TEnum, uint>)toInt)(value))?.Select(flag => uint32ToEnum(flag)).ToArray();
                case TypeCode.Int64:
                    var int64ToEnum = (Func<long, TEnum>)toEnum;
                    return ((EnumsCache<long>)cache).GetFlags(((Func<TEnum, long>)toInt)(value))?.Select(flag => int64ToEnum(flag)).ToArray();
                case TypeCode.UInt64:
                    var uint64ToEnum = (Func<ulong, TEnum>)toEnum;
                    return ((EnumsCache<ulong>)cache).GetFlags(((Func<TEnum, ulong>)toInt)(value))?.Select(flag => uint64ToEnum(flag)).ToArray();
                case TypeCode.SByte:
                    var sbyteToEnum = (Func<sbyte, TEnum>)toEnum;
                    return ((EnumsCache<sbyte>)cache).GetFlags(((Func<TEnum, sbyte>)toInt)(value))?.Select(flag => sbyteToEnum(flag)).ToArray();
                case TypeCode.Byte:
                    var byteToEnum = (Func<byte, TEnum>)toEnum;
                    return ((EnumsCache<byte>)cache).GetFlags(((Func<TEnum, byte>)toInt)(value))?.Select(flag => byteToEnum(flag)).ToArray();
                case TypeCode.Int16:
                    var int16ToEnum = (Func<short, TEnum>)toEnum;
                    return ((EnumsCache<short>)cache).GetFlags(((Func<TEnum, short>)toInt)(value))?.Select(flag => int16ToEnum(flag)).ToArray();
                case TypeCode.UInt16:
                    var uint16ToEnum = (Func<ushort, TEnum>)toEnum;
                    return ((EnumsCache<ushort>)cache).GetFlags(((Func<TEnum, ushort>)toInt)(value))?.Select(flag => uint16ToEnum(flag)).ToArray();
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Indicates if <paramref name="value"/> has any flags set.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <returns>Indication if <paramref name="value"/> has any flags set.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not a valid flag combination.</exception>
        [Pure]
        public static bool HasAnyFlags<TEnum>(TEnum value)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).HasAnyFlags(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).HasAnyFlags(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).HasAnyFlags(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).HasAnyFlags(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).HasAnyFlags(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).HasAnyFlags(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).HasAnyFlags(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).HasAnyFlags(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

        /// <summary>
        /// Indicates if <paramref name="value"/> has any flags set that are also set in <paramref name="flagMask"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <param name="flagMask">Must be a valid flag combination.</param>
        /// <returns>Indication if <paramref name="value"/> has any flags set that are also set in <paramref name="flagMask"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static bool HasAnyFlags<TEnum>(TEnum value, TEnum flagMask)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<TEnum, int>)toInt;
                    return ((EnumsCache<int>)cache).HasAnyFlags(enumToInt32(value), enumToInt32(flagMask));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<TEnum, uint>)toInt;
                    return ((EnumsCache<uint>)cache).HasAnyFlags(enumToUInt32(value), enumToUInt32(flagMask));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<TEnum, long>)toInt;
                    return ((EnumsCache<long>)cache).HasAnyFlags(enumToInt64(value), enumToInt64(flagMask));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<TEnum, ulong>)toInt;
                    return ((EnumsCache<ulong>)cache).HasAnyFlags(enumToUInt64(value), enumToUInt64(flagMask));
                case TypeCode.SByte:
                    var enumToSByte = (Func<TEnum, sbyte>)toInt;
                    return ((EnumsCache<sbyte>)cache).HasAnyFlags(enumToSByte(value), enumToSByte(flagMask));
                case TypeCode.Byte:
                    var enumToByte = (Func<TEnum, byte>)toInt;
                    return ((EnumsCache<byte>)cache).HasAnyFlags(enumToByte(value), enumToByte(flagMask));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<TEnum, short>)toInt;
                    return ((EnumsCache<short>)cache).HasAnyFlags(enumToInt16(value), enumToInt16(flagMask));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<TEnum, ushort>)toInt;
                    return ((EnumsCache<ushort>)cache).HasAnyFlags(enumToUInt16(value), enumToUInt16(flagMask));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

        /// <summary>
        /// Indicates if <paramref name="value"/> has all flags set that are defined in <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <returns>Indication if <paramref name="value"/> has all flags set that are defined in <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not a valid flag combination.</exception>
        [Pure]
        public static bool HasAllFlags<TEnum>(TEnum value)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((EnumsCache<int>)cache).HasAllFlags(((Func<TEnum, int>)toInt)(value));
                case TypeCode.UInt32:
                    return ((EnumsCache<uint>)cache).HasAllFlags(((Func<TEnum, uint>)toInt)(value));
                case TypeCode.Int64:
                    return ((EnumsCache<long>)cache).HasAllFlags(((Func<TEnum, long>)toInt)(value));
                case TypeCode.UInt64:
                    return ((EnumsCache<ulong>)cache).HasAllFlags(((Func<TEnum, ulong>)toInt)(value));
                case TypeCode.SByte:
                    return ((EnumsCache<sbyte>)cache).HasAllFlags(((Func<TEnum, sbyte>)toInt)(value));
                case TypeCode.Byte:
                    return ((EnumsCache<byte>)cache).HasAllFlags(((Func<TEnum, byte>)toInt)(value));
                case TypeCode.Int16:
                    return ((EnumsCache<short>)cache).HasAllFlags(((Func<TEnum, short>)toInt)(value));
                case TypeCode.UInt16:
                    return ((EnumsCache<ushort>)cache).HasAllFlags(((Func<TEnum, ushort>)toInt)(value));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

        /// <summary>
        /// Indicates if <paramref name="value"/> has all of the flags set that are also set in <paramref name="flagMask"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <param name="flagMask">Must be a valid flag combination.</param>
        /// <returns>Indication if <paramref name="value"/> has all of the flags set that are also set in <paramref name="flagMask"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static bool HasAllFlags<TEnum>(TEnum value, TEnum flagMask)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<TEnum, int>)toInt;
                    return ((EnumsCache<int>)cache).HasAllFlags(enumToInt32(value), enumToInt32(flagMask));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<TEnum, uint>)toInt;
                    return ((EnumsCache<uint>)cache).HasAllFlags(enumToUInt32(value), enumToUInt32(flagMask));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<TEnum, long>)toInt;
                    return ((EnumsCache<long>)cache).HasAllFlags(enumToInt64(value), enumToInt64(flagMask));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<TEnum, ulong>)toInt;
                    return ((EnumsCache<ulong>)cache).HasAllFlags(enumToUInt64(value), enumToUInt64(flagMask));
                case TypeCode.SByte:
                    var enumToSByte = (Func<TEnum, sbyte>)toInt;
                    return ((EnumsCache<sbyte>)cache).HasAllFlags(enumToSByte(value), enumToSByte(flagMask));
                case TypeCode.Byte:
                    var enumToByte = (Func<TEnum, byte>)toInt;
                    return ((EnumsCache<byte>)cache).HasAllFlags(enumToByte(value), enumToByte(flagMask));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<TEnum, short>)toInt;
                    return ((EnumsCache<short>)cache).HasAllFlags(enumToInt16(value), enumToInt16(flagMask));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<TEnum, ushort>)toInt;
                    return ((EnumsCache<ushort>)cache).HasAllFlags(enumToUInt16(value), enumToUInt16(flagMask));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

        /// <summary>
        /// Returns <paramref name="value"/> with all of it's flags inverted. Equivalent to the bitwise "xor" operator with <see cref="GetAllFlags{TEnum}()"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <returns><paramref name="value"/> with all of it's flags inverted.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not a valid flag combination.</exception>
        [Pure]
        public static TEnum InvertFlags<TEnum>(TEnum value)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).InvertFlags(((Func<TEnum, int>)toInt)(value)));
                case TypeCode.UInt32:
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).InvertFlags(((Func<TEnum, uint>)toInt)(value)));
                case TypeCode.Int64:
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).InvertFlags(((Func<TEnum, long>)toInt)(value)));
                case TypeCode.UInt64:
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).InvertFlags(((Func<TEnum, ulong>)toInt)(value)));
                case TypeCode.SByte:
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).InvertFlags(((Func<TEnum, sbyte>)toInt)(value)));
                case TypeCode.Byte:
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).InvertFlags(((Func<TEnum, byte>)toInt)(value)));
                case TypeCode.Int16:
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).InvertFlags(((Func<TEnum, short>)toInt)(value)));
                case TypeCode.UInt16:
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).InvertFlags(((Func<TEnum, ushort>)toInt)(value)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        /// <summary>
        /// Returns <paramref name="value"/> while inverting the flags that are set in <paramref name="flagMask"/>. Equivalent to the bitwise "xor" operator.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <param name="flagMask">Must be a valid flag combination.</param>
        /// <returns><paramref name="value"/> while inverting the flags that are set in <paramref name="flagMask"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static TEnum InvertFlags<TEnum>(TEnum value, TEnum flagMask)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<TEnum, int>)toInt;
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).InvertFlags(enumToInt32(value), enumToInt32(flagMask)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<TEnum, uint>)toInt;
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).InvertFlags(enumToUInt32(value), enumToUInt32(flagMask)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<TEnum, long>)toInt;
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).InvertFlags(enumToInt64(value), enumToInt64(flagMask)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<TEnum, ulong>)toInt;
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).InvertFlags(enumToUInt64(value), enumToUInt64(flagMask)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<TEnum, sbyte>)toInt;
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).InvertFlags(enumToSByte(value), enumToSByte(flagMask)));
                case TypeCode.Byte:
                    var enumToByte = (Func<TEnum, byte>)toInt;
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).InvertFlags(enumToByte(value), enumToByte(flagMask)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<TEnum, short>)toInt;
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).InvertFlags(enumToInt16(value), enumToInt16(flagMask)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<TEnum, ushort>)toInt;
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).InvertFlags(enumToUInt16(value), enumToUInt16(flagMask)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        /// <summary>
        /// Returns <paramref name="value"/> with only the flags that are also set in <paramref name="flagMask"/>. Equivalent to the bitwise "and" operation.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <param name="flagMask">Must be a valid flag combination.</param>
        /// <returns><paramref name="value"/> with only the flags that are also set in <paramref name="flagMask"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static TEnum CommonFlags<TEnum>(TEnum value, TEnum flagMask)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<TEnum, int>)toInt;
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).CommonFlags(enumToInt32(value), enumToInt32(flagMask)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<TEnum, uint>)toInt;
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).CommonFlags(enumToUInt32(value), enumToUInt32(flagMask)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<TEnum, long>)toInt;
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).CommonFlags(enumToInt64(value), enumToInt64(flagMask)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<TEnum, ulong>)toInt;
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).CommonFlags(enumToUInt64(value), enumToUInt64(flagMask)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<TEnum, sbyte>)toInt;
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).CommonFlags(enumToSByte(value), enumToSByte(flagMask)));
                case TypeCode.Byte:
                    var enumToByte = (Func<TEnum, byte>)toInt;
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).CommonFlags(enumToByte(value), enumToByte(flagMask)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<TEnum, short>)toInt;
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).CommonFlags(enumToInt16(value), enumToInt16(flagMask)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<TEnum, ushort>)toInt;
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).CommonFlags(enumToUInt16(value), enumToUInt16(flagMask)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        /// <summary>
        /// Returns <paramref name="flag0"/> with the flags specified in <paramref name="flag1"/> set. Equivalent to the bitwise "or" operation.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="flag0">Must be a valid flag combination.</param>
        /// <param name="flag1">Must be a valid flag combination.</param>
        /// <returns><paramref name="flag0"/> with the flags specified in <paramref name="flag1"/> set.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="flag0"/> or <paramref name="flag1"/> is not a valid flag combination.</exception>
        [Pure]
        public static TEnum SetFlags<TEnum>(TEnum flag0, TEnum flag1)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<TEnum, int>)toInt;
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).SetFlags(enumToInt32(flag0), enumToInt32(flag1)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<TEnum, uint>)toInt;
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).SetFlags(enumToUInt32(flag0), enumToUInt32(flag1)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<TEnum, long>)toInt;
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).SetFlags(enumToInt64(flag0), enumToInt64(flag1)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<TEnum, ulong>)toInt;
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).SetFlags(enumToUInt64(flag0), enumToUInt64(flag1)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<TEnum, sbyte>)toInt;
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).SetFlags(enumToSByte(flag0), enumToSByte(flag1)));
                case TypeCode.Byte:
                    var enumToByte = (Func<TEnum, byte>)toInt;
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).SetFlags(enumToByte(flag0), enumToByte(flag1)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<TEnum, short>)toInt;
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).SetFlags(enumToInt16(flag0), enumToInt16(flag1)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<TEnum, ushort>)toInt;
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).SetFlags(enumToUInt16(flag0), enumToUInt16(flag1)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        [Pure]
        public static TEnum SetFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<TEnum, int>)toInt;
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).SetFlags(enumToInt32(flag0), enumToInt32(flag1), enumToInt32(flag2)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<TEnum, uint>)toInt;
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).SetFlags(enumToUInt32(flag0), enumToUInt32(flag1), enumToUInt32(flag2)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<TEnum, long>)toInt;
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).SetFlags(enumToInt64(flag0), enumToInt64(flag1), enumToInt64(flag2)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<TEnum, ulong>)toInt;
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).SetFlags(enumToUInt64(flag0), enumToUInt64(flag1), enumToUInt64(flag2)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<TEnum, sbyte>)toInt;
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).SetFlags(enumToSByte(flag0), enumToSByte(flag1), enumToSByte(flag2)));
                case TypeCode.Byte:
                    var enumToByte = (Func<TEnum, byte>)toInt;
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).SetFlags(enumToByte(flag0), enumToByte(flag1), enumToByte(flag2)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<TEnum, short>)toInt;
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).SetFlags(enumToInt16(flag0), enumToInt16(flag1), enumToInt16(flag2)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<TEnum, ushort>)toInt;
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).SetFlags(enumToUInt16(flag0), enumToUInt16(flag1), enumToUInt16(flag2)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        [Pure]
        public static TEnum SetFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<TEnum, int>)toInt;
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).SetFlags(enumToInt32(flag0), enumToInt32(flag1), enumToInt32(flag2)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<TEnum, uint>)toInt;
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).SetFlags(enumToUInt32(flag0), enumToUInt32(flag1), enumToUInt32(flag2)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<TEnum, long>)toInt;
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).SetFlags(enumToInt64(flag0), enumToInt64(flag1), enumToInt64(flag2)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<TEnum, ulong>)toInt;
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).SetFlags(enumToUInt64(flag0), enumToUInt64(flag1), enumToUInt64(flag2)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<TEnum, sbyte>)toInt;
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).SetFlags(enumToSByte(flag0), enumToSByte(flag1), enumToSByte(flag2)));
                case TypeCode.Byte:
                    var enumToByte = (Func<TEnum, byte>)toInt;
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).SetFlags(enumToByte(flag0), enumToByte(flag1), enumToByte(flag2)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<TEnum, short>)toInt;
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).SetFlags(enumToInt16(flag0), enumToInt16(flag1), enumToInt16(flag2)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<TEnum, ushort>)toInt;
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).SetFlags(enumToUInt16(flag0), enumToUInt16(flag1), enumToUInt16(flag2)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        [Pure]
        public static TEnum SetFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4)
        {
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<TEnum, int>)toInt;
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).SetFlags(enumToInt32(flag0), enumToInt32(flag1), enumToInt32(flag2), enumToInt32(flag3), enumToInt32(flag4)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<TEnum, uint>)toInt;
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).SetFlags(enumToUInt32(flag0), enumToUInt32(flag1), enumToUInt32(flag2), enumToUInt32(flag3), enumToUInt32(flag4)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<TEnum, long>)toInt;
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).SetFlags(enumToInt64(flag0), enumToInt64(flag1), enumToInt64(flag2), enumToInt64(flag3), enumToInt64(flag4)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<TEnum, ulong>)toInt;
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).SetFlags(enumToUInt64(flag0), enumToUInt64(flag1), enumToUInt64(flag2), enumToUInt64(flag3), enumToUInt64(flag4)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<TEnum, sbyte>)toInt;
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).SetFlags(enumToSByte(flag0), enumToSByte(flag1), enumToSByte(flag2), enumToSByte(flag3), enumToSByte(flag4)));
                case TypeCode.Byte:
                    var enumToByte = (Func<TEnum, byte>)toInt;
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).SetFlags(enumToByte(flag0), enumToByte(flag1), enumToByte(flag2), enumToByte(flag3), enumToByte(flag4)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<TEnum, short>)toInt;
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).SetFlags(enumToInt16(flag0), enumToInt16(flag1), enumToInt16(flag2), enumToInt16(flag3), enumToInt16(flag4)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<TEnum, ushort>)toInt;
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).SetFlags(enumToUInt16(flag0), enumToUInt16(flag1), enumToUInt16(flag2), enumToUInt16(flag3), enumToUInt16(flag4)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        [Pure]
        public static TEnum SetFlags<TEnum>(params TEnum[] flags)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<TEnum, int>)toInt;
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).SetFlags(flags.Select(flag => enumToInt32(flag))));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<TEnum, uint>)toInt;
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).SetFlags(flags.Select(flag => enumToUInt32(flag))));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<TEnum, long>)toInt;
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).SetFlags(flags.Select(flag => enumToInt64(flag))));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<TEnum, ulong>)toInt;
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).SetFlags(flags.Select(flag => enumToUInt64(flag))));
                case TypeCode.SByte:
                    var enumToSByte = (Func<TEnum, sbyte>)toInt;
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).SetFlags(flags.Select(flag => enumToSByte(flag))));
                case TypeCode.Byte:
                    var enumToByte = (Func<TEnum, byte>)toInt;
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).SetFlags(flags.Select(flag => enumToByte(flag))));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<TEnum, short>)toInt;
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).SetFlags(flags.Select(flag => enumToInt16(flag))));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<TEnum, ushort>)toInt;
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).SetFlags(flags.Select(flag => enumToUInt16(flag))));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        /// <summary>
        /// Returns <paramref name="value"/> with the flags specified in <paramref name="flagMask"/> cleared.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <param name="flagMask">Must be a valid flag combination.</param>
        /// <returns><paramref name="value"/> with the flags specified in <paramref name="flagMask"/> cleared.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static TEnum ClearFlags<TEnum>(TEnum value, TEnum flagMask)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toInt = Enums<TEnum>.ToInt;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = (Func<TEnum, int>)toInt;
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).ClearFlags(enumToInt32(value), enumToInt32(flagMask)));
                case TypeCode.UInt32:
                    var enumToUInt32 = (Func<TEnum, uint>)toInt;
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).ClearFlags(enumToUInt32(value), enumToUInt32(flagMask)));
                case TypeCode.Int64:
                    var enumToInt64 = (Func<TEnum, long>)toInt;
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).ClearFlags(enumToInt64(value), enumToInt64(flagMask)));
                case TypeCode.UInt64:
                    var enumToUInt64 = (Func<TEnum, ulong>)toInt;
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).ClearFlags(enumToUInt64(value), enumToUInt64(flagMask)));
                case TypeCode.SByte:
                    var enumToSByte = (Func<TEnum, sbyte>)toInt;
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).ClearFlags(enumToSByte(value), enumToSByte(flagMask)));
                case TypeCode.Byte:
                    var enumToByte = (Func<TEnum, byte>)toInt;
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).ClearFlags(enumToByte(value), enumToByte(flagMask)));
                case TypeCode.Int16:
                    var enumToInt16 = (Func<TEnum, short>)toInt;
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).ClearFlags(enumToInt16(value), enumToInt16(flagMask)));
                case TypeCode.UInt16:
                    var enumToUInt16 = (Func<TEnum, ushort>)toInt;
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).ClearFlags(enumToUInt16(value), enumToUInt16(flagMask)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
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
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static TEnum Parse<TEnum>(string value) => Parse<TEnum>(value, false, null, null);

        [Pure]
        public static TEnum Parse<TEnum>(string value, params EnumFormat[] parseFormatOrder) => Parse<TEnum>(value, false, null, parseFormatOrder);

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
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static TEnum Parse<TEnum>(string value, bool ignoreCase) => Parse<TEnum>(value, ignoreCase, null, null);

        [Pure]
        public static TEnum Parse<TEnum>(string value, bool ignoreCase, params EnumFormat[] parseFormatOrder) => Parse<TEnum>(value, ignoreCase, null, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// delimited with the specified delimiter to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="delimiter">The effective delimiter will be the <paramref name="delimiter"/> trimmed if it's not all whitespace.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static TEnum Parse<TEnum>(string value, string delimiter) => Parse<TEnum>(value, false, delimiter, null);

        [Pure]
        public static TEnum Parse<TEnum>(string value, string delimiter, params EnumFormat[] parseFormatOrder) => Parse<TEnum>(value, false, delimiter, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// delimited with the specified delimiter to an equivalent enumerated object.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="delimiter">The effective delimiter will be the <paramref name="delimiter"/> trimmed if it's not all whitespace.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static TEnum Parse<TEnum>(string value, bool ignoreCase, string delimiter) => Parse<TEnum>(value, ignoreCase, delimiter, null);

        [Pure]
        public static TEnum Parse<TEnum>(string value, bool ignoreCase, string delimiter, params EnumFormat[] parseFormatOrder)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toEnum = Enums<TEnum>.ToEnum;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, TEnum>)toEnum)(((EnumsCache<int>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.UInt32:
                    return ((Func<uint, TEnum>)toEnum)(((EnumsCache<uint>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.Int64:
                    return ((Func<long, TEnum>)toEnum)(((EnumsCache<long>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.UInt64:
                    return ((Func<ulong, TEnum>)toEnum)(((EnumsCache<ulong>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.SByte:
                    return ((Func<sbyte, TEnum>)toEnum)(((EnumsCache<sbyte>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.Byte:
                    return ((Func<byte, TEnum>)toEnum)(((EnumsCache<byte>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.Int16:
                    return ((Func<short, TEnum>)toEnum)(((EnumsCache<short>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.UInt16:
                    return ((Func<ushort, TEnum>)toEnum)(((EnumsCache<ushort>)cache).ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified enumerated value.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, TEnum defaultEnum) => ParseOrDefault(value, false, null, defaultEnum, null);

        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, TEnum defaultEnum, params EnumFormat[] parseFormatOrder) => ParseOrDefault(value, false, null, defaultEnum, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified enumerated value.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, bool ignoreCase, TEnum defaultEnum) => ParseOrDefault(value, ignoreCase, null, defaultEnum, null);

        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, bool ignoreCase, TEnum defaultEnum, params EnumFormat[] parseFormatOrder) => ParseOrDefault(value, ignoreCase, null, defaultEnum, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants delimited with the specified delimiter to an equivalent enumerated object but if it fails
        /// returns the specified enumerated value.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="delimiter"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="delimiter"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, string delimiter, TEnum defaultEnum) => ParseOrDefault(value, false, delimiter, defaultEnum, null);

        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, string delimiter, TEnum defaultEnum, params EnumFormat[] parseFormatOrder) => ParseOrDefault(value, false, delimiter, defaultEnum, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants delimited with the specified delimiter to an equivalent enumerated object but if it fails
        /// returns the specified enumerated value. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="delimiter"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="delimiter"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, bool ignoreCase, string delimiter, TEnum defaultEnum) => ParseOrDefault(value, ignoreCase, delimiter, defaultEnum, null);

        [Pure]
        public static TEnum ParseOrDefault<TEnum>(string value, bool ignoreCase, string delimiter, TEnum defaultEnum, params EnumFormat[] parseFormatOrder)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            TEnum result;
            return TryParse(value, ignoreCase, delimiter, out result, parseFormatOrder) ? result : defaultEnum;
        }

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type</exception>
        [Pure]
        public static bool TryParse<TEnum>(string value, out TEnum result) => TryParse(value, false, null, out result, null);

        [Pure]
        public static bool TryParse<TEnum>(string value, out TEnum result, params EnumFormat[] parseFormatOrder) => TryParse(value, false, null, out result, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
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
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) => TryParse(value, ignoreCase, null, out result, null);

        [Pure]
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result, params EnumFormat[] parseFormatOrder) => TryParse(value, ignoreCase, null, out result, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants delimited with the specified delimiter to an equivalent enumerated object. The return value
        /// indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="delimiter"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="delimiter"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
        [Pure]
        public static bool TryParse<TEnum>(string value, string delimiter, out TEnum result) => TryParse(value, false, delimiter, out result, null);

        [Pure]
        public static bool TryParse<TEnum>(string value, string delimiter, out TEnum result, params EnumFormat[] parseFormatOrder) => TryParse(value, false, delimiter, out result, parseFormatOrder);

        /// <summary>
        /// Tries to convert the specified string representation of the name or numeric value of one or more enumerated
        /// constants delimited with the specified delimiter to an equivalent enumerated object. The return value
        /// indicates whether the conversion succeeded. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="delimiter"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="delimiter"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
        [Pure]
        public static bool TryParse<TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result) => TryParse(value, ignoreCase, delimiter, out result, null);

        [Pure]
        public static bool TryParse<TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result, params EnumFormat[] parseFormatOrder)
        {
            UnsafeEnums.VerifyTypeIsEnum(typeof(TEnum));
            var cache = Enums<TEnum>.Cache;
            var toEnum = Enums<TEnum>.ToEnum;
            var success = false;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = ((EnumsCache<int>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsInt32, parseFormatOrder);
                    result = ((Func<int, TEnum>)toEnum)(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = ((EnumsCache<uint>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsUInt32, parseFormatOrder);
                    result = ((Func<uint, TEnum>)toEnum)(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = ((EnumsCache<long>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsInt64, parseFormatOrder);
                    result = ((Func<long, TEnum>)toEnum)(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = ((EnumsCache<ulong>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsUInt64, parseFormatOrder);
                    result = ((Func<ulong, TEnum>)toEnum)(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = ((EnumsCache<sbyte>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsSByte, parseFormatOrder);
                    result = ((Func<sbyte, TEnum>)toEnum)(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = ((EnumsCache<byte>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsByte, parseFormatOrder);
                    result = ((Func<byte, TEnum>)toEnum)(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = ((EnumsCache<short>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsInt16, parseFormatOrder);
                    result = ((Func<short, TEnum>)toEnum)(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = ((EnumsCache<ushort>)cache).TryParseFlags(value, ignoreCase, delimiter, out resultAsUInt16, parseFormatOrder);
                    result = ((Func<ushort, TEnum>)toEnum)(resultAsUInt16);
                    return success;
            }
            Debug.Fail("Unknown Enum TypeCode");
            result = default(TEnum);
            return false;
        }
        #endregion
    }
}
