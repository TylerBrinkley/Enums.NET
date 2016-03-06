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
using ExtraConstraints;

namespace EnumsNET
{
    /// <summary>
    /// Static class that provides efficient type-safe flag enum operations through the use of cached enum names, values, and attributes.
    /// Many operations are exposed as extension methods for convenience.
    /// </summary>
    public static class FlagEnums
    {
        internal const string DefaultDelimiter = ", ";

        #region "Properties"
        /// <summary>
        /// Indicates if <typeparamref name="TEnum"/> is marked with the <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns>Indication if <typeparamref name="TEnum"/> is marked with the <see cref="FlagsAttribute"/>.</returns>
        [Pure]
        public static bool IsFlagEnum<[EnumConstraint] TEnum>()
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.IsFlagEnum;
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.IsFlagEnum;
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.IsFlagEnum;
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.IsFlagEnum;
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.IsFlagEnum;
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.IsFlagEnum;
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.IsFlagEnum;
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.IsFlagEnum;
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

        /// <summary>
        /// Retrieves all the flags defined by <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns>All the flags defined by <typeparamref name="TEnum"/>.</returns>
        [Pure]
        public static TEnum GetAllFlags<[EnumConstraint] TEnum>()
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.AllFlags);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.AllFlags);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.AllFlags);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.AllFlags);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.AllFlags);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.AllFlags);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.AllFlags);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.AllFlags);
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
        [Pure]
        public static bool IsValidFlagCombination<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.IsValidFlagCombination(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.IsValidFlagCombination(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.IsValidFlagCombination(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.IsValidFlagCombination(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.IsValidFlagCombination(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.IsValidFlagCombination(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.IsValidFlagCombination(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.IsValidFlagCombination(Enums<TEnum, ushort>.ToInt(value));
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
        [Pure]
        public static string FormatAsFlags<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct => FormatAsFlags(value, null, null);

        [Pure]
        public static string FormatAsFlags<[EnumConstraint] TEnum>(this TEnum value, params EnumFormat[] formats)
            where TEnum : struct => FormatAsFlags(value, null, formats);

        /// <summary>
        /// Returns the names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Should be a valid flag combination.</param>
        /// <param name="delimiter">The delimiter to use to separate individual flag names. Cannot be null or empty.</param>
        /// <returns>The names of <paramref name="value"/>'s flags delimited with <paramref name="delimiter"/> or if empty returns the name of the zero flag if defined otherwise "0".
        /// If <paramref name="value"/> is not a valid flag combination null is returned.</returns>
        [Pure]
        public static string FormatAsFlags<[EnumConstraint] TEnum>(this TEnum value, string delimiter)
            where TEnum : struct => FormatAsFlags(value, delimiter, null);

        [Pure]
        public static string FormatAsFlags<[EnumConstraint] TEnum>(this TEnum value, string delimiter, params EnumFormat[] formats)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.FormatAsFlags(Enums<TEnum, int>.ToInt(value), delimiter, formats);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.FormatAsFlags(Enums<TEnum, uint>.ToInt(value), delimiter, formats);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.FormatAsFlags(Enums<TEnum, long>.ToInt(value), delimiter, formats);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.FormatAsFlags(Enums<TEnum, ulong>.ToInt(value), delimiter, formats);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.FormatAsFlags(Enums<TEnum, sbyte>.ToInt(value), delimiter, formats);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.FormatAsFlags(Enums<TEnum, byte>.ToInt(value), delimiter, formats);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.FormatAsFlags(Enums<TEnum, short>.ToInt(value), delimiter, formats);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.FormatAsFlags(Enums<TEnum, ushort>.ToInt(value), delimiter, formats);
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
        [Pure]
        public static TEnum[] GetFlags<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var int32ToEnum = Enums<TEnum, int>.ToEnum;
                    return Enums<TEnum, int>.Cache.GetFlags(Enums<TEnum, int>.ToInt(value))?.Select(flag => int32ToEnum(flag)).ToArray();
                case TypeCode.UInt32:
                    var uint32ToEnum = Enums<TEnum, uint>.ToEnum;
                    return Enums<TEnum, uint>.Cache.GetFlags(Enums<TEnum, uint>.ToInt(value))?.Select(flag => uint32ToEnum(flag)).ToArray();
                case TypeCode.Int64:
                    var int64ToEnum = Enums<TEnum, long>.ToEnum;
                    return Enums<TEnum, long>.Cache.GetFlags(Enums<TEnum, long>.ToInt(value))?.Select(flag => int64ToEnum(flag)).ToArray();
                case TypeCode.UInt64:
                    var uint64ToEnum = Enums<TEnum, ulong>.ToEnum;
                    return Enums<TEnum, ulong>.Cache.GetFlags(Enums<TEnum, ulong>.ToInt(value))?.Select(flag => uint64ToEnum(flag)).ToArray();
                case TypeCode.SByte:
                    var sbyteToEnum = Enums<TEnum, sbyte>.ToEnum;
                    return Enums<TEnum, sbyte>.Cache.GetFlags(Enums<TEnum, sbyte>.ToInt(value))?.Select(flag => sbyteToEnum(flag)).ToArray();
                case TypeCode.Byte:
                    var byteToEnum = Enums<TEnum, byte>.ToEnum;
                    return Enums<TEnum, byte>.Cache.GetFlags(Enums<TEnum, byte>.ToInt(value))?.Select(flag => byteToEnum(flag)).ToArray();
                case TypeCode.Int16:
                    var int16ToEnum = Enums<TEnum, short>.ToEnum;
                    return Enums<TEnum, short>.Cache.GetFlags(Enums<TEnum, short>.ToInt(value))?.Select(flag => int16ToEnum(flag)).ToArray();
                case TypeCode.UInt16:
                    var uint16ToEnum = Enums<TEnum, ushort>.ToEnum;
                    return Enums<TEnum, ushort>.Cache.GetFlags(Enums<TEnum, ushort>.ToInt(value))?.Select(flag => uint16ToEnum(flag)).ToArray();
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
        /// <exception cref="ArgumentException"><paramref name="value"/> is not a valid flag combination.</exception>
        [Pure]
        public static bool HasAnyFlags<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.HasAnyFlags(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.HasAnyFlags(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.HasAnyFlags(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.HasAnyFlags(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.HasAnyFlags(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.HasAnyFlags(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.HasAnyFlags(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.HasAnyFlags(Enums<TEnum, ushort>.ToInt(value));
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
        /// <exception cref="ArgumentException"><paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static bool HasAnyFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum flagMask)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = Enums<TEnum, int>.ToInt;
                    return Enums<TEnum, int>.Cache.HasAnyFlags(enumToInt32(value), enumToInt32(flagMask));
                case TypeCode.UInt32:
                    var enumToUInt32 = Enums<TEnum, uint>.ToInt;
                    return Enums<TEnum, uint>.Cache.HasAnyFlags(enumToUInt32(value), enumToUInt32(flagMask));
                case TypeCode.Int64:
                    var enumToInt64 = Enums<TEnum, long>.ToInt;
                    return Enums<TEnum, long>.Cache.HasAnyFlags(enumToInt64(value), enumToInt64(flagMask));
                case TypeCode.UInt64:
                    var enumToUInt64 = Enums<TEnum, ulong>.ToInt;
                    return Enums<TEnum, ulong>.Cache.HasAnyFlags(enumToUInt64(value), enumToUInt64(flagMask));
                case TypeCode.SByte:
                    var enumToSByte = Enums<TEnum, sbyte>.ToInt;
                    return Enums<TEnum, sbyte>.Cache.HasAnyFlags(enumToSByte(value), enumToSByte(flagMask));
                case TypeCode.Byte:
                    var enumToByte = Enums<TEnum, byte>.ToInt;
                    return Enums<TEnum, byte>.Cache.HasAnyFlags(enumToByte(value), enumToByte(flagMask));
                case TypeCode.Int16:
                    var enumToInt16 = Enums<TEnum, short>.ToInt;
                    return Enums<TEnum, short>.Cache.HasAnyFlags(enumToInt16(value), enumToInt16(flagMask));
                case TypeCode.UInt16:
                    var enumToUInt16 = Enums<TEnum, ushort>.ToInt;
                    return Enums<TEnum, ushort>.Cache.HasAnyFlags(enumToUInt16(value), enumToUInt16(flagMask));
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
        /// <exception cref="ArgumentException"><paramref name="value"/> is not a valid flag combination.</exception>
        [Pure]
        public static bool HasAllFlags<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.HasAllFlags(Enums<TEnum, int>.ToInt(value));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.HasAllFlags(Enums<TEnum, uint>.ToInt(value));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.HasAllFlags(Enums<TEnum, long>.ToInt(value));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.HasAllFlags(Enums<TEnum, ulong>.ToInt(value));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.HasAllFlags(Enums<TEnum, sbyte>.ToInt(value));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.HasAllFlags(Enums<TEnum, byte>.ToInt(value));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.HasAllFlags(Enums<TEnum, short>.ToInt(value));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.HasAllFlags(Enums<TEnum, ushort>.ToInt(value));
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
        /// <exception cref="ArgumentException"><paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static bool HasAllFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum flagMask)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = Enums<TEnum, int>.ToInt;
                    return Enums<TEnum, int>.Cache.HasAllFlags(enumToInt32(value), enumToInt32(flagMask));
                case TypeCode.UInt32:
                    var enumToUInt32 = Enums<TEnum, uint>.ToInt;
                    return Enums<TEnum, uint>.Cache.HasAllFlags(enumToUInt32(value), enumToUInt32(flagMask));
                case TypeCode.Int64:
                    var enumToInt64 = Enums<TEnum, long>.ToInt;
                    return Enums<TEnum, long>.Cache.HasAllFlags(enumToInt64(value), enumToInt64(flagMask));
                case TypeCode.UInt64:
                    var enumToUInt64 = Enums<TEnum, ulong>.ToInt;
                    return Enums<TEnum, ulong>.Cache.HasAllFlags(enumToUInt64(value), enumToUInt64(flagMask));
                case TypeCode.SByte:
                    var enumToSByte = Enums<TEnum, sbyte>.ToInt;
                    return Enums<TEnum, sbyte>.Cache.HasAllFlags(enumToSByte(value), enumToSByte(flagMask));
                case TypeCode.Byte:
                    var enumToByte = Enums<TEnum, byte>.ToInt;
                    return Enums<TEnum, byte>.Cache.HasAllFlags(enumToByte(value), enumToByte(flagMask));
                case TypeCode.Int16:
                    var enumToInt16 = Enums<TEnum, short>.ToInt;
                    return Enums<TEnum, short>.Cache.HasAllFlags(enumToInt16(value), enumToInt16(flagMask));
                case TypeCode.UInt16:
                    var enumToUInt16 = Enums<TEnum, ushort>.ToInt;
                    return Enums<TEnum, ushort>.Cache.HasAllFlags(enumToUInt16(value), enumToUInt16(flagMask));
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
        /// <exception cref="ArgumentException"><paramref name="value"/> is not a valid flag combination.</exception>
        [Pure]
        public static TEnum InvertFlags<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.InvertFlags(Enums<TEnum, int>.ToInt(value)));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.InvertFlags(Enums<TEnum, uint>.ToInt(value)));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.InvertFlags(Enums<TEnum, long>.ToInt(value)));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.InvertFlags(Enums<TEnum, ulong>.ToInt(value)));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.InvertFlags(Enums<TEnum, sbyte>.ToInt(value)));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.InvertFlags(Enums<TEnum, byte>.ToInt(value)));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.InvertFlags(Enums<TEnum, short>.ToInt(value)));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.InvertFlags(Enums<TEnum, ushort>.ToInt(value)));
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
        /// <exception cref="ArgumentException"><paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static TEnum InvertFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum flagMask)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = Enums<TEnum, int>.ToInt;
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.InvertFlags(enumToInt32(value), enumToInt32(flagMask)));
                case TypeCode.UInt32:
                    var enumToUInt32 = Enums<TEnum, uint>.ToInt;
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.InvertFlags(enumToUInt32(value), enumToUInt32(flagMask)));
                case TypeCode.Int64:
                    var enumToInt64 = Enums<TEnum, long>.ToInt;
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.InvertFlags(enumToInt64(value), enumToInt64(flagMask)));
                case TypeCode.UInt64:
                    var enumToUInt64 = Enums<TEnum, ulong>.ToInt;
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.InvertFlags(enumToUInt64(value), enumToUInt64(flagMask)));
                case TypeCode.SByte:
                    var enumToSByte = Enums<TEnum, sbyte>.ToInt;
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.InvertFlags(enumToSByte(value), enumToSByte(flagMask)));
                case TypeCode.Byte:
                    var enumToByte = Enums<TEnum, byte>.ToInt;
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.InvertFlags(enumToByte(value), enumToByte(flagMask)));
                case TypeCode.Int16:
                    var enumToInt16 = Enums<TEnum, short>.ToInt;
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.InvertFlags(enumToInt16(value), enumToInt16(flagMask)));
                case TypeCode.UInt16:
                    var enumToUInt16 = Enums<TEnum, ushort>.ToInt;
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.InvertFlags(enumToUInt16(value), enumToUInt16(flagMask)));
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
        /// <exception cref="ArgumentException"><paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static TEnum CommonFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum flagMask)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = Enums<TEnum, int>.ToInt;
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.CommonFlags(enumToInt32(value), enumToInt32(flagMask)));
                case TypeCode.UInt32:
                    var enumToUInt32 = Enums<TEnum, uint>.ToInt;
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.CommonFlags(enumToUInt32(value), enumToUInt32(flagMask)));
                case TypeCode.Int64:
                    var enumToInt64 = Enums<TEnum, long>.ToInt;
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.CommonFlags(enumToInt64(value), enumToInt64(flagMask)));
                case TypeCode.UInt64:
                    var enumToUInt64 = Enums<TEnum, ulong>.ToInt;
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.CommonFlags(enumToUInt64(value), enumToUInt64(flagMask)));
                case TypeCode.SByte:
                    var enumToSByte = Enums<TEnum, sbyte>.ToInt;
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.CommonFlags(enumToSByte(value), enumToSByte(flagMask)));
                case TypeCode.Byte:
                    var enumToByte = Enums<TEnum, byte>.ToInt;
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.CommonFlags(enumToByte(value), enumToByte(flagMask)));
                case TypeCode.Int16:
                    var enumToInt16 = Enums<TEnum, short>.ToInt;
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.CommonFlags(enumToInt16(value), enumToInt16(flagMask)));
                case TypeCode.UInt16:
                    var enumToUInt16 = Enums<TEnum, ushort>.ToInt;
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.CommonFlags(enumToUInt16(value), enumToUInt16(flagMask)));
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
        /// <exception cref="ArgumentException"><paramref name="flag0"/> or <paramref name="flag1"/> is not a valid flag combination.</exception>
        [Pure]
        public static TEnum SetFlags<[EnumConstraint] TEnum>(this TEnum flag0, TEnum flag1)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = Enums<TEnum, int>.ToInt;
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.SetFlags(enumToInt32(flag0), enumToInt32(flag1)));
                case TypeCode.UInt32:
                    var enumToUInt32 = Enums<TEnum, uint>.ToInt;
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.SetFlags(enumToUInt32(flag0), enumToUInt32(flag1)));
                case TypeCode.Int64:
                    var enumToInt64 = Enums<TEnum, long>.ToInt;
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.SetFlags(enumToInt64(flag0), enumToInt64(flag1)));
                case TypeCode.UInt64:
                    var enumToUInt64 = Enums<TEnum, ulong>.ToInt;
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.SetFlags(enumToUInt64(flag0), enumToUInt64(flag1)));
                case TypeCode.SByte:
                    var enumToSByte = Enums<TEnum, sbyte>.ToInt;
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.SetFlags(enumToSByte(flag0), enumToSByte(flag1)));
                case TypeCode.Byte:
                    var enumToByte = Enums<TEnum, byte>.ToInt;
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.SetFlags(enumToByte(flag0), enumToByte(flag1)));
                case TypeCode.Int16:
                    var enumToInt16 = Enums<TEnum, short>.ToInt;
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.SetFlags(enumToInt16(flag0), enumToInt16(flag1)));
                case TypeCode.UInt16:
                    var enumToUInt16 = Enums<TEnum, ushort>.ToInt;
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.SetFlags(enumToUInt16(flag0), enumToUInt16(flag1)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        [Pure]
        public static TEnum SetFlags<[EnumConstraint] TEnum>(this TEnum flag0, TEnum flag1, TEnum flag2)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = Enums<TEnum, int>.ToInt;
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.SetFlags(enumToInt32(flag0), enumToInt32(flag1), enumToInt32(flag2)));
                case TypeCode.UInt32:
                    var enumToUInt32 = Enums<TEnum, uint>.ToInt;
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.SetFlags(enumToUInt32(flag0), enumToUInt32(flag1), enumToUInt32(flag2)));
                case TypeCode.Int64:
                    var enumToInt64 = Enums<TEnum, long>.ToInt;
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.SetFlags(enumToInt64(flag0), enumToInt64(flag1), enumToInt64(flag2)));
                case TypeCode.UInt64:
                    var enumToUInt64 = Enums<TEnum, ulong>.ToInt;
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.SetFlags(enumToUInt64(flag0), enumToUInt64(flag1), enumToUInt64(flag2)));
                case TypeCode.SByte:
                    var enumToSByte = Enums<TEnum, sbyte>.ToInt;
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.SetFlags(enumToSByte(flag0), enumToSByte(flag1), enumToSByte(flag2)));
                case TypeCode.Byte:
                    var enumToByte = Enums<TEnum, byte>.ToInt;
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.SetFlags(enumToByte(flag0), enumToByte(flag1), enumToByte(flag2)));
                case TypeCode.Int16:
                    var enumToInt16 = Enums<TEnum, short>.ToInt;
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.SetFlags(enumToInt16(flag0), enumToInt16(flag1), enumToInt16(flag2)));
                case TypeCode.UInt16:
                    var enumToUInt16 = Enums<TEnum, ushort>.ToInt;
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.SetFlags(enumToUInt16(flag0), enumToUInt16(flag1), enumToUInt16(flag2)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        [Pure]
        public static TEnum SetFlags<[EnumConstraint] TEnum>(this TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = Enums<TEnum, int>.ToInt;
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.SetFlags(enumToInt32(flag0), enumToInt32(flag1), enumToInt32(flag2), enumToInt32(flag3)));
                case TypeCode.UInt32:
                    var enumToUInt32 = Enums<TEnum, uint>.ToInt;
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.SetFlags(enumToUInt32(flag0), enumToUInt32(flag1), enumToUInt32(flag2), enumToUInt32(flag3)));
                case TypeCode.Int64:
                    var enumToInt64 = Enums<TEnum, long>.ToInt;
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.SetFlags(enumToInt64(flag0), enumToInt64(flag1), enumToInt64(flag2), enumToInt64(flag3)));
                case TypeCode.UInt64:
                    var enumToUInt64 = Enums<TEnum, ulong>.ToInt;
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.SetFlags(enumToUInt64(flag0), enumToUInt64(flag1), enumToUInt64(flag2), enumToUInt64(flag3)));
                case TypeCode.SByte:
                    var enumToSByte = Enums<TEnum, sbyte>.ToInt;
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.SetFlags(enumToSByte(flag0), enumToSByte(flag1), enumToSByte(flag2), enumToSByte(flag3)));
                case TypeCode.Byte:
                    var enumToByte = Enums<TEnum, byte>.ToInt;
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.SetFlags(enumToByte(flag0), enumToByte(flag1), enumToByte(flag2), enumToByte(flag3)));
                case TypeCode.Int16:
                    var enumToInt16 = Enums<TEnum, short>.ToInt;
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.SetFlags(enumToInt16(flag0), enumToInt16(flag1), enumToInt16(flag2), enumToInt16(flag3)));
                case TypeCode.UInt16:
                    var enumToUInt16 = Enums<TEnum, ushort>.ToInt;
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.SetFlags(enumToUInt16(flag0), enumToUInt16(flag1), enumToUInt16(flag2), enumToUInt16(flag3)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        [Pure]
        public static TEnum SetFlags<[EnumConstraint] TEnum>(this TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = Enums<TEnum, int>.ToInt;
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.SetFlags(enumToInt32(flag0), enumToInt32(flag1), enumToInt32(flag2), enumToInt32(flag3), enumToInt32(flag4)));
                case TypeCode.UInt32:
                    var enumToUInt32 = Enums<TEnum, uint>.ToInt;
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.SetFlags(enumToUInt32(flag0), enumToUInt32(flag1), enumToUInt32(flag2), enumToUInt32(flag3), enumToUInt32(flag4)));
                case TypeCode.Int64:
                    var enumToInt64 = Enums<TEnum, long>.ToInt;
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.SetFlags(enumToInt64(flag0), enumToInt64(flag1), enumToInt64(flag2), enumToInt64(flag3), enumToInt64(flag4)));
                case TypeCode.UInt64:
                    var enumToUInt64 = Enums<TEnum, ulong>.ToInt;
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.SetFlags(enumToUInt64(flag0), enumToUInt64(flag1), enumToUInt64(flag2), enumToUInt64(flag3), enumToUInt64(flag4)));
                case TypeCode.SByte:
                    var enumToSByte = Enums<TEnum, sbyte>.ToInt;
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.SetFlags(enumToSByte(flag0), enumToSByte(flag1), enumToSByte(flag2), enumToSByte(flag3), enumToSByte(flag4)));
                case TypeCode.Byte:
                    var enumToByte = Enums<TEnum, byte>.ToInt;
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.SetFlags(enumToByte(flag0), enumToByte(flag1), enumToByte(flag2), enumToByte(flag3), enumToByte(flag4)));
                case TypeCode.Int16:
                    var enumToInt16 = Enums<TEnum, short>.ToInt;
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.SetFlags(enumToInt16(flag0), enumToInt16(flag1), enumToInt16(flag2), enumToInt16(flag3), enumToInt16(flag4)));
                case TypeCode.UInt16:
                    var enumToUInt16 = Enums<TEnum, ushort>.ToInt;
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.SetFlags(enumToUInt16(flag0), enumToUInt16(flag1), enumToUInt16(flag2), enumToUInt16(flag3), enumToUInt16(flag4)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return default(TEnum);
        }

        [Pure]
        public static TEnum SetFlags<[EnumConstraint] TEnum>(params TEnum[] flags)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = Enums<TEnum, int>.ToInt;
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.SetFlags(flags.Select(flag => enumToInt32(flag))));
                case TypeCode.UInt32:
                    var enumToUInt32 = Enums<TEnum, uint>.ToInt;
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.SetFlags(flags.Select(flag => enumToUInt32(flag))));
                case TypeCode.Int64:
                    var enumToInt64 = Enums<TEnum, long>.ToInt;
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.SetFlags(flags.Select(flag => enumToInt64(flag))));
                case TypeCode.UInt64:
                    var enumToUInt64 = Enums<TEnum, ulong>.ToInt;
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.SetFlags(flags.Select(flag => enumToUInt64(flag))));
                case TypeCode.SByte:
                    var enumToSByte = Enums<TEnum, sbyte>.ToInt;
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.SetFlags(flags.Select(flag => enumToSByte(flag))));
                case TypeCode.Byte:
                    var enumToByte = Enums<TEnum, byte>.ToInt;
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.SetFlags(flags.Select(flag => enumToByte(flag))));
                case TypeCode.Int16:
                    var enumToInt16 = Enums<TEnum, short>.ToInt;
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.SetFlags(flags.Select(flag => enumToInt16(flag))));
                case TypeCode.UInt16:
                    var enumToUInt16 = Enums<TEnum, ushort>.ToInt;
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.SetFlags(flags.Select(flag => enumToUInt16(flag))));
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
        /// <exception cref="ArgumentException"><paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static TEnum ClearFlags<[EnumConstraint] TEnum>(this TEnum value, TEnum flagMask)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    var enumToInt32 = Enums<TEnum, int>.ToInt;
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.ClearFlags(enumToInt32(value), enumToInt32(flagMask)));
                case TypeCode.UInt32:
                    var enumToUInt32 = Enums<TEnum, uint>.ToInt;
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.ClearFlags(enumToUInt32(value), enumToUInt32(flagMask)));
                case TypeCode.Int64:
                    var enumToInt64 = Enums<TEnum, long>.ToInt;
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.ClearFlags(enumToInt64(value), enumToInt64(flagMask)));
                case TypeCode.UInt64:
                    var enumToUInt64 = Enums<TEnum, ulong>.ToInt;
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.ClearFlags(enumToUInt64(value), enumToUInt64(flagMask)));
                case TypeCode.SByte:
                    var enumToSByte = Enums<TEnum, sbyte>.ToInt;
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.ClearFlags(enumToSByte(value), enumToSByte(flagMask)));
                case TypeCode.Byte:
                    var enumToByte = Enums<TEnum, byte>.ToInt;
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.ClearFlags(enumToByte(value), enumToByte(flagMask)));
                case TypeCode.Int16:
                    var enumToInt16 = Enums<TEnum, short>.ToInt;
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.ClearFlags(enumToInt16(value), enumToInt16(flagMask)));
                case TypeCode.UInt16:
                    var enumToUInt16 = Enums<TEnum, ushort>.ToInt;
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.ClearFlags(enumToUInt16(value), enumToUInt16(flagMask)));
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
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value)
            where TEnum : struct => Parse<TEnum>(value, false, null, null);

        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => Parse<TEnum>(value, false, null, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value, bool ignoreCase)
            where TEnum : struct => Parse<TEnum>(value, ignoreCase, null, null);

        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value, bool ignoreCase, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => Parse<TEnum>(value, ignoreCase, null, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// delimited with the specified delimiter to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="delimiter">The effective delimiter will be the <paramref name="delimiter"/> trimmed if it's not all whitespace.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value, string delimiter)
            where TEnum : struct => Parse<TEnum>(value, false, delimiter, null);

        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value, string delimiter, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => Parse<TEnum>(value, false, delimiter, parseFormatOrder);

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
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter)
            where TEnum : struct => Parse<TEnum>(value, ignoreCase, delimiter, null);

        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter, params EnumFormat[] parseFormatOrder)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.ToEnum(Enums<TEnum, int>.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.ToEnum(Enums<TEnum, uint>.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.Int64:
                    return Enums<TEnum, long>.ToEnum(Enums<TEnum, long>.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.ToEnum(Enums<TEnum, ulong>.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.ToEnum(Enums<TEnum, sbyte>.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.ToEnum(Enums<TEnum, byte>.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.Int16:
                    return Enums<TEnum, short>.ToEnum(Enums<TEnum, short>.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.ToEnum(Enums<TEnum, ushort>.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
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
        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, TEnum defaultEnum)
            where TEnum : struct => ParseOrDefault(value, false, null, defaultEnum, null);

        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, TEnum defaultEnum, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => ParseOrDefault(value, false, null, defaultEnum, parseFormatOrder);

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
        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, TEnum defaultEnum)
            where TEnum : struct => ParseOrDefault(value, ignoreCase, null, defaultEnum, null);

        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, TEnum defaultEnum, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => ParseOrDefault(value, ignoreCase, null, defaultEnum, parseFormatOrder);

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
        /// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="delimiter"/> is an empty string.</exception>
        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, string delimiter, TEnum defaultEnum)
            where TEnum : struct => ParseOrDefault(value, false, delimiter, defaultEnum, null);

        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, string delimiter, TEnum defaultEnum, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => ParseOrDefault(value, false, delimiter, defaultEnum, parseFormatOrder);

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
        /// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="delimiter"/> is an empty string.</exception>
        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter, TEnum defaultEnum)
            where TEnum : struct => ParseOrDefault(value, ignoreCase, delimiter, defaultEnum, null);

        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter, TEnum defaultEnum, params EnumFormat[] parseFormatOrder)
            where TEnum : struct
        {
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
        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, out TEnum result)
            where TEnum : struct => TryParse(value, false, null, out result, null);

        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, out TEnum result, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => TryParse(value, false, null, out result, parseFormatOrder);

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
        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, bool ignoreCase, out TEnum result)
            where TEnum : struct => TryParse(value, ignoreCase, null, out result, null);

        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, bool ignoreCase, out TEnum result, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => TryParse(value, ignoreCase, null, out result, parseFormatOrder);

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
        /// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="delimiter"/> is an empty string.</exception>
        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, string delimiter, out TEnum result)
            where TEnum : struct => TryParse(value, false, delimiter, out result, null);

        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, string delimiter, out TEnum result, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => TryParse(value, false, delimiter, out result, parseFormatOrder);

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
        /// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="delimiter"/> is an empty string.</exception>
        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result)
            where TEnum : struct => TryParse(value, ignoreCase, delimiter, out result, null);

        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, bool ignoreCase, string delimiter, out TEnum result, params EnumFormat[] parseFormatOrder)
            where TEnum : struct
        {
            bool success;
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    int resultAsInt32;
                    success = Enums<TEnum, int>.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsInt32, parseFormatOrder);
                    result = Enums<TEnum, int>.ToEnum(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    uint resultAsUInt32;
                    success = Enums<TEnum, uint>.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsUInt32, parseFormatOrder);
                    result = Enums<TEnum, uint>.ToEnum(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    long resultAsInt64;
                    success = Enums<TEnum, long>.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsInt64, parseFormatOrder);
                    result = Enums<TEnum, long>.ToEnum(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    ulong resultAsUInt64;
                    success = Enums<TEnum, ulong>.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsUInt64, parseFormatOrder);
                    result = Enums<TEnum, ulong>.ToEnum(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    sbyte resultAsSByte;
                    success = Enums<TEnum, sbyte>.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsSByte, parseFormatOrder);
                    result = Enums<TEnum, sbyte>.ToEnum(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    byte resultAsByte;
                    success = Enums<TEnum, byte>.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsByte, parseFormatOrder);
                    result = Enums<TEnum, byte>.ToEnum(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    short resultAsInt16;
                    success = Enums<TEnum, short>.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsInt16, parseFormatOrder);
                    result = Enums<TEnum, short>.ToEnum(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    ushort resultAsUInt16;
                    success = Enums<TEnum, ushort>.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsUInt16, parseFormatOrder);
                    result = Enums<TEnum, ushort>.ToEnum(resultAsUInt16);
                    return success;
            }
            Debug.Fail("Unknown Enum TypeCode");
            result = default(TEnum);
            return false;
        }
        #endregion
    }
}
