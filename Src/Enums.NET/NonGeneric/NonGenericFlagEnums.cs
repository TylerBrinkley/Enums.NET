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
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return true;
            }

            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = (EnumsCache<int>)cache;
                    return int32Cache.IsValidFlagCombination(int32Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    var uint32Cache = (EnumsCache<uint>)cache;
                    return uint32Cache.IsValidFlagCombination(uint32Cache.ToObject(value, false));
                case TypeCode.Int64:
                    var int64Cache = (EnumsCache<long>)cache;
                    return int64Cache.IsValidFlagCombination(int64Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    var uint64Cache = (EnumsCache<ulong>)cache;
                    return uint64Cache.IsValidFlagCombination(uint64Cache.ToObject(value, false));
                case TypeCode.SByte:
                    var sbyteCache = (EnumsCache<sbyte>)cache;
                    return sbyteCache.IsValidFlagCombination(sbyteCache.ToObject(value, false));
                case TypeCode.Byte:
                    var byteCache = (EnumsCache<byte>)cache;
                    return byteCache.IsValidFlagCombination(byteCache.ToObject(value, false));
                case TypeCode.Int16:
                    var int16Cache = (EnumsCache<short>)cache;
                    return int16Cache.IsValidFlagCombination(int16Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    var uint16Cache = (EnumsCache<ushort>)cache;
                    return uint16Cache.IsValidFlagCombination(uint16Cache.ToObject(value, false));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }

            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = (EnumsCache<int>)cache;
                    return int32Cache.FormatAsFlags(int32Cache.ToObject(value, false), delimiter, formats);
                case TypeCode.UInt32:
                    var uint32Cache = (EnumsCache<uint>)cache;
                    return uint32Cache.FormatAsFlags(uint32Cache.ToObject(value, false), delimiter, formats);
                case TypeCode.Int64:
                    var int64Cache = (EnumsCache<long>)cache;
                    return int64Cache.FormatAsFlags(int64Cache.ToObject(value, false), delimiter, formats);
                case TypeCode.UInt64:
                    var uint64Cache = (EnumsCache<ulong>)cache;
                    return uint64Cache.FormatAsFlags(uint64Cache.ToObject(value, false), delimiter, formats);
                case TypeCode.SByte:
                    var sbyteCache = (EnumsCache<sbyte>)cache;
                    return sbyteCache.FormatAsFlags(sbyteCache.ToObject(value, false), delimiter, formats);
                case TypeCode.Byte:
                    var byteCache = (EnumsCache<byte>)cache;
                    return byteCache.FormatAsFlags(byteCache.ToObject(value, false), delimiter, formats);
                case TypeCode.Int16:
                    var int16Cache = (EnumsCache<short>)cache;
                    return int16Cache.FormatAsFlags(int16Cache.ToObject(value, false), delimiter, formats);
                case TypeCode.UInt16:
                    var uint16Cache = (EnumsCache<ushort>)cache;
                    return uint16Cache.FormatAsFlags(uint16Cache.ToObject(value, false), delimiter, formats);
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return new object[0];
            }

            var cache = enumsCache.Cache;
            var toEnum = enumsCache.ToEnum;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32ToEnum = (Func<int, object>)toEnum;
                    var int32Cache = (EnumsCache<int>)cache;
                    return int32Cache.GetFlags(int32Cache.ToObject(value, false))?.Select(flag => int32ToEnum(flag)).ToArray();
                case TypeCode.UInt32:
                    var uint32ToEnum = (Func<uint, object>)toEnum;
                    var uint32Cache = (EnumsCache<uint>)cache;
                    return uint32Cache.GetFlags(uint32Cache.ToObject(value, false))?.Select(flag => uint32ToEnum(flag)).ToArray();
                case TypeCode.Int64:
                    var int64ToEnum = (Func<long, object>)toEnum;
                    var int64Cache = (EnumsCache<long>)cache;
                    return int64Cache.GetFlags(int64Cache.ToObject(value, false))?.Select(flag => int64ToEnum(flag)).ToArray();
                case TypeCode.UInt64:
                    var uint64ToEnum = (Func<ulong, object>)toEnum;
                    var uint64Cache = (EnumsCache<ulong>)cache;
                    return uint64Cache.GetFlags(uint64Cache.ToObject(value, false))?.Select(flag => uint64ToEnum(flag)).ToArray();
                case TypeCode.SByte:
                    var sbyteToEnum = (Func<sbyte, object>)toEnum;
                    var sbyteCache = (EnumsCache<sbyte>)cache;
                    return sbyteCache.GetFlags(sbyteCache.ToObject(value, false))?.Select(flag => sbyteToEnum(flag)).ToArray();
                case TypeCode.Byte:
                    var byteToEnum = (Func<byte, object>)toEnum;
                    var byteCache = (EnumsCache<byte>)cache;
                    return byteCache.GetFlags(byteCache.ToObject(value, false))?.Select(flag => byteToEnum(flag)).ToArray();
                case TypeCode.Int16:
                    var int16ToEnum = (Func<short, object>)toEnum;
                    var int16Cache = (EnumsCache<short>)cache;
                    return int16Cache.GetFlags(int16Cache.ToObject(value, false))?.Select(flag => int16ToEnum(flag)).ToArray();
                case TypeCode.UInt16:
                    var uint16ToEnum = (Func<ushort, object>)toEnum;
                    var uint16Cache = (EnumsCache<ushort>)cache;
                    return uint16Cache.GetFlags(uint16Cache.ToObject(value, false))?.Select(flag => uint16ToEnum(flag)).ToArray();
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return false;
            }

            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = (EnumsCache<int>)cache;
                    return int32Cache.HasAnyFlags(int32Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    var uint32Cache = (EnumsCache<uint>)cache;
                    return uint32Cache.HasAnyFlags(uint32Cache.ToObject(value, false));
                case TypeCode.Int64:
                    var int64Cache = (EnumsCache<long>)cache;
                    return int64Cache.HasAnyFlags(int64Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    var uint64Cache = (EnumsCache<ulong>)cache;
                    return uint64Cache.HasAnyFlags(uint64Cache.ToObject(value, false));
                case TypeCode.SByte:
                    var sbyteCache = (EnumsCache<sbyte>)cache;
                    return sbyteCache.HasAnyFlags(sbyteCache.ToObject(value, false));
                case TypeCode.Byte:
                    var byteCache = (EnumsCache<byte>)cache;
                    return byteCache.HasAnyFlags(byteCache.ToObject(value, false));
                case TypeCode.Int16:
                    var int16Cache = (EnumsCache<short>)cache;
                    return int16Cache.HasAnyFlags(int16Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    var uint16Cache = (EnumsCache<ushort>)cache;
                    return uint16Cache.HasAnyFlags(uint16Cache.ToObject(value, false));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (isNullable)
            {
                if (value == null)
                {
                    return !HasAnyFlags(enumType, flagMask);
                }
                if (flagMask == null)
                {
                    NonGenericEnums.ToObject(enumType, value, false);
                    return true;
                }
            }

            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = (EnumsCache<int>)cache;
                    return int32Cache.HasAnyFlags(int32Cache.ToObject(value, false), int32Cache.ToObject(flagMask, false));
                case TypeCode.UInt32:
                    var uint32Cache = (EnumsCache<uint>)cache;
                    return uint32Cache.HasAnyFlags(uint32Cache.ToObject(value, false), uint32Cache.ToObject(flagMask, false));
                case TypeCode.Int64:
                    var int64Cache = (EnumsCache<long>)cache;
                    return int64Cache.HasAnyFlags(int64Cache.ToObject(value, false), int64Cache.ToObject(flagMask, false));
                case TypeCode.UInt64:
                    var uint64Cache = (EnumsCache<ulong>)cache;
                    return uint64Cache.HasAnyFlags(uint64Cache.ToObject(value, false), uint64Cache.ToObject(flagMask, false));
                case TypeCode.SByte:
                    var sbyteCache = (EnumsCache<sbyte>)cache;
                    return sbyteCache.HasAnyFlags(sbyteCache.ToObject(value, false), sbyteCache.ToObject(flagMask, false));
                case TypeCode.Byte:
                    var byteCache = (EnumsCache<byte>)cache;
                    return byteCache.HasAnyFlags(byteCache.ToObject(value, false), byteCache.ToObject(flagMask, false));
                case TypeCode.Int16:
                    var int16Cache = (EnumsCache<short>)cache;
                    return int16Cache.HasAnyFlags(int16Cache.ToObject(value, false), int16Cache.ToObject(flagMask, false));
                case TypeCode.UInt16:
                    var uint16Cache = (EnumsCache<ushort>)cache;
                    return uint16Cache.HasAnyFlags(uint16Cache.ToObject(value, false), uint16Cache.ToObject(flagMask, false));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return false;
            }

            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = (EnumsCache<int>)cache;
                    return int32Cache.HasAllFlags(int32Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    var uint32Cache = (EnumsCache<uint>)cache;
                    return uint32Cache.HasAllFlags(uint32Cache.ToObject(value, false));
                case TypeCode.Int64:
                    var int64Cache = (EnumsCache<long>)cache;
                    return int64Cache.HasAllFlags(int64Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    var uint64Cache = (EnumsCache<ulong>)cache;
                    return uint64Cache.HasAllFlags(uint64Cache.ToObject(value, false));
                case TypeCode.SByte:
                    var sbyteCache = (EnumsCache<sbyte>)cache;
                    return sbyteCache.HasAllFlags(sbyteCache.ToObject(value, false));
                case TypeCode.Byte:
                    var byteCache = (EnumsCache<byte>)cache;
                    return byteCache.HasAllFlags(byteCache.ToObject(value, false));
                case TypeCode.Int16:
                    var int16Cache = (EnumsCache<short>)cache;
                    return int16Cache.HasAllFlags(int16Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    var uint16Cache = (EnumsCache<ushort>)cache;
                    return uint16Cache.HasAllFlags(uint16Cache.ToObject(value, false));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (isNullable)
            {
                if (value == null)
                {
                    return !HasAnyFlags(enumType, flagMask);
                }
                if (flagMask == null)
                {
                    NonGenericEnums.ToObject(enumType, value, false);
                    return true;
                }
            }

            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = (EnumsCache<int>)cache;
                    return int32Cache.HasAllFlags(int32Cache.ToObject(value, false), int32Cache.ToObject(flagMask, false));
                case TypeCode.UInt32:
                    var uint32Cache = (EnumsCache<uint>)cache;
                    return uint32Cache.HasAllFlags(uint32Cache.ToObject(value, false), uint32Cache.ToObject(flagMask, false));
                case TypeCode.Int64:
                    var int64Cache = (EnumsCache<long>)cache;
                    return int64Cache.HasAllFlags(int64Cache.ToObject(value, false), int64Cache.ToObject(flagMask, false));
                case TypeCode.UInt64:
                    var uint64Cache = (EnumsCache<ulong>)cache;
                    return uint64Cache.HasAllFlags(uint64Cache.ToObject(value, false), uint64Cache.ToObject(flagMask, false));
                case TypeCode.SByte:
                    var sbyteCache = (EnumsCache<sbyte>)cache;
                    return sbyteCache.HasAllFlags(sbyteCache.ToObject(value, false), sbyteCache.ToObject(flagMask, false));
                case TypeCode.Byte:
                    var byteCache = (EnumsCache<byte>)cache;
                    return byteCache.HasAllFlags(byteCache.ToObject(value, false), byteCache.ToObject(flagMask, false));
                case TypeCode.Int16:
                    var int16Cache = (EnumsCache<short>)cache;
                    return int16Cache.HasAllFlags(int16Cache.ToObject(value, false), int16Cache.ToObject(flagMask, false));
                case TypeCode.UInt16:
                    var uint16Cache = (EnumsCache<ushort>)cache;
                    return uint16Cache.HasAllFlags(uint16Cache.ToObject(value, false), uint16Cache.ToObject(flagMask, false));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return false;
        }

        /// <summary>
        /// Returns <paramref name="value"/> with all of it's flags toggled. Equivalent to the bitwise "xor" operator with <see cref="GetAllFlags(Type)"/>.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <returns><paramref name="value"/> with all of it's flags toggled.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type
        /// -or-
        /// <paramref name="value"/> is not a valid flag combination.</exception>
        [Pure]
        public static object ToggleFlags(Type enumType, object value)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return GetAllFlags(enumType);
            }

            var toEnum = enumsCache.ToEnum;
            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = (EnumsCache<int>)cache;
                    return ((Func<int, object>)toEnum)(int32Cache.ToggleFlags(int32Cache.ToObject(value, false)));
                case TypeCode.UInt32:
                    var uint32Cache = (EnumsCache<uint>)cache;
                    return ((Func<uint, object>)toEnum)(uint32Cache.ToggleFlags(uint32Cache.ToObject(value, false)));
                case TypeCode.Int64:
                    var int64Cache = (EnumsCache<long>)cache;
                    return ((Func<long, object>)toEnum)(int64Cache.ToggleFlags(int64Cache.ToObject(value, false)));
                case TypeCode.UInt64:
                    var uint64Cache = (EnumsCache<ulong>)cache;
                    return ((Func<ulong, object>)toEnum)(uint64Cache.ToggleFlags(uint64Cache.ToObject(value, false)));
                case TypeCode.SByte:
                    var sbyteCache = (EnumsCache<sbyte>)cache;
                    return ((Func<sbyte, object>)toEnum)(sbyteCache.ToggleFlags(sbyteCache.ToObject(value, false)));
                case TypeCode.Byte:
                    var byteCache = (EnumsCache<byte>)cache;
                    return ((Func<byte, object>)toEnum)(byteCache.ToggleFlags(byteCache.ToObject(value, false)));
                case TypeCode.Int16:
                    var int16Cache = (EnumsCache<short>)cache;
                    return ((Func<short, object>)toEnum)(int16Cache.ToggleFlags(int16Cache.ToObject(value, false)));
                case TypeCode.UInt16:
                    var uint16Cache = (EnumsCache<ushort>)cache;
                    return ((Func<ushort, object>)toEnum)(uint16Cache.ToggleFlags(uint16Cache.ToObject(value, false)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Returns <paramref name="value"/> while toggling the flags that are set in <paramref name="flagMask"/>. Equivalent to the bitwise "xor" operator.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">Must be a valid flag combination.</param>
        /// <param name="flagMask">Must be a valid flag combination.</param>
        /// <returns><paramref name="value"/> while toggling the flags that are set in <paramref name="flagMask"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is an invalid type
        /// -or-
        /// <paramref name="value"/> or <paramref name="flagMask"/> is not a valid flag combination.</exception>
        [Pure]
        public static object ToggleFlags(Type enumType, object value, object flagMask)
        {
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (isNullable)
            {
                if (value == null)
                {
                    return NonGenericEnums.ToObject(enumType, flagMask, false);
                }
                if (flagMask == null)
                {
                    return NonGenericEnums.ToObject(enumType, value, false);
                }
            }

            var toEnum = enumsCache.ToEnum;
            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = (EnumsCache<int>)cache;
                    return ((Func<int, object>)toEnum)(int32Cache.ToggleFlags(int32Cache.ToObject(value, false), int32Cache.ToObject(flagMask, false)));
                case TypeCode.UInt32:
                    var uint32Cache = (EnumsCache<uint>)cache;
                    return ((Func<uint, object>)toEnum)(uint32Cache.ToggleFlags(uint32Cache.ToObject(value, false), uint32Cache.ToObject(flagMask, false)));
                case TypeCode.Int64:
                    var int64Cache = (EnumsCache<long>)cache;
                    return ((Func<long, object>)toEnum)(int64Cache.ToggleFlags(int64Cache.ToObject(value, false), int64Cache.ToObject(flagMask, false)));
                case TypeCode.UInt64:
                    var uint64Cache = (EnumsCache<ulong>)cache;
                    return ((Func<ulong, object>)toEnum)(uint64Cache.ToggleFlags(uint64Cache.ToObject(value, false), uint64Cache.ToObject(flagMask, false)));
                case TypeCode.SByte:
                    var sbyteCache = (EnumsCache<sbyte>)cache;
                    return ((Func<sbyte, object>)toEnum)(sbyteCache.ToggleFlags(sbyteCache.ToObject(value, false), sbyteCache.ToObject(flagMask, false)));
                case TypeCode.Byte:
                    var byteCache = (EnumsCache<byte>)cache;
                    return ((Func<byte, object>)toEnum)(byteCache.ToggleFlags(byteCache.ToObject(value, false), byteCache.ToObject(flagMask, false)));
                case TypeCode.Int16:
                    var int16Cache = (EnumsCache<short>)cache;
                    return ((Func<short, object>)toEnum)(int16Cache.ToggleFlags(int16Cache.ToObject(value, false), int16Cache.ToObject(flagMask, false)));
                case TypeCode.UInt16:
                    var uint16Cache = (EnumsCache<ushort>)cache;
                    return ((Func<ushort, object>)toEnum)(uint16Cache.ToggleFlags(uint16Cache.ToObject(value, false), uint16Cache.ToObject(flagMask, false)));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (isNullable)
            {
                if (value == null)
                {
                    if (flagMask != null)
                    {
                        NonGenericEnums.ToObject(enumType, flagMask, false);
                    }
                    return null;
                }
                if (flagMask == null)
                {
                    NonGenericEnums.ToObject(enumType, value, false);
                    return null;
                }
            }

            var toEnum = enumsCache.ToEnum;
            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = (EnumsCache<int>)cache;
                    return ((Func<int, object>)toEnum)(int32Cache.CommonFlags(int32Cache.ToObject(value, false), int32Cache.ToObject(flagMask, false)));
                case TypeCode.UInt32:
                    var uint32Cache = (EnumsCache<uint>)cache;
                    return ((Func<uint, object>)toEnum)(uint32Cache.CommonFlags(uint32Cache.ToObject(value, false), uint32Cache.ToObject(flagMask, false)));
                case TypeCode.Int64:
                    var int64Cache = (EnumsCache<long>)cache;
                    return ((Func<long, object>)toEnum)(int64Cache.CommonFlags(int64Cache.ToObject(value, false), int64Cache.ToObject(flagMask, false)));
                case TypeCode.UInt64:
                    var uint64Cache = (EnumsCache<ulong>)cache;
                    return ((Func<ulong, object>)toEnum)(uint64Cache.CommonFlags(uint64Cache.ToObject(value, false), uint64Cache.ToObject(flagMask, false)));
                case TypeCode.SByte:
                    var sbyteCache = (EnumsCache<sbyte>)cache;
                    return ((Func<sbyte, object>)toEnum)(sbyteCache.CommonFlags(sbyteCache.ToObject(value, false), sbyteCache.ToObject(flagMask, false)));
                case TypeCode.Byte:
                    var byteCache = (EnumsCache<byte>)cache;
                    return ((Func<byte, object>)toEnum)(byteCache.CommonFlags(byteCache.ToObject(value, false), byteCache.ToObject(flagMask, false)));
                case TypeCode.Int16:
                    var int16Cache = (EnumsCache<short>)cache;
                    return ((Func<short, object>)toEnum)(int16Cache.CommonFlags(int16Cache.ToObject(value, false), int16Cache.ToObject(flagMask, false)));
                case TypeCode.UInt16:
                    var uint16Cache = (EnumsCache<ushort>)cache;
                    return ((Func<ushort, object>)toEnum)(uint16Cache.CommonFlags(uint16Cache.ToObject(value, false), uint16Cache.ToObject(flagMask, false)));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (isNullable)
            {
                if (flag0 == null)
                {
                    return NonGenericEnums.ToObject(enumType, flag1, false);
                }
                if (flag1 == null)
                {
                    return NonGenericEnums.ToObject(enumType, flag0, false);
                }
            }

            var toEnum = enumsCache.ToEnum;
            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = (EnumsCache<int>)cache;
                    return ((Func<int, object>)toEnum)(int32Cache.SetFlags(int32Cache.ToObject(flag0, false), int32Cache.ToObject(flag1, false)));
                case TypeCode.UInt32:
                    var uint32Cache = (EnumsCache<uint>)cache;
                    return ((Func<uint, object>)toEnum)(uint32Cache.SetFlags(uint32Cache.ToObject(flag0, false), uint32Cache.ToObject(flag1, false)));
                case TypeCode.Int64:
                    var int64Cache = (EnumsCache<long>)cache;
                    return ((Func<long, object>)toEnum)(int64Cache.SetFlags(int64Cache.ToObject(flag0, false), int64Cache.ToObject(flag1, false)));
                case TypeCode.UInt64:
                    var uint64Cache = (EnumsCache<ulong>)cache;
                    return ((Func<ulong, object>)toEnum)(uint64Cache.SetFlags(uint64Cache.ToObject(flag0, false), uint64Cache.ToObject(flag1, false)));
                case TypeCode.SByte:
                    var sbyteCache = (EnumsCache<sbyte>)cache;
                    return ((Func<sbyte, object>)toEnum)(sbyteCache.SetFlags(sbyteCache.ToObject(flag0, false), sbyteCache.ToObject(flag1, false)));
                case TypeCode.Byte:
                    var byteCache = (EnumsCache<byte>)cache;
                    return ((Func<byte, object>)toEnum)(byteCache.SetFlags(byteCache.ToObject(flag0, false), byteCache.ToObject(flag1, false)));
                case TypeCode.Int16:
                    var int16Cache = (EnumsCache<short>)cache;
                    return ((Func<short, object>)toEnum)(int16Cache.SetFlags(int16Cache.ToObject(flag0, false), int16Cache.ToObject(flag1, false)));
                case TypeCode.UInt16:
                    var uint16Cache = (EnumsCache<ushort>)cache;
                    return ((Func<ushort, object>)toEnum)(uint16Cache.SetFlags(uint16Cache.ToObject(flag0, false), uint16Cache.ToObject(flag1, false)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static object SetFlags(Type enumType, params object[] flags)
        {
            Preconditions.NotNull(flags, nameof(flags));

            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            var allFlags = isNullable ? flags.Where(flag => flag != null) : flags;
            var toEnum = enumsCache.ToEnum;
            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = (EnumsCache<int>)cache;
                    return ((Func<int, object>)toEnum)(int32Cache.SetFlags(allFlags.Select(flag => int32Cache.ToObject(flag, false))));
                case TypeCode.UInt32:
                    var uint32Cache = (EnumsCache<uint>)cache;
                    return ((Func<uint, object>)toEnum)(uint32Cache.SetFlags(allFlags.Select(flag => uint32Cache.ToObject(flag, false))));
                case TypeCode.Int64:
                    var int64Cache = (EnumsCache<long>)cache;
                    return ((Func<long, object>)toEnum)(int64Cache.SetFlags(allFlags.Select(flag => int64Cache.ToObject(flag, false))));
                case TypeCode.UInt64:
                    var uint64Cache = (EnumsCache<ulong>)cache;
                    return ((Func<ulong, object>)toEnum)(uint64Cache.SetFlags(allFlags.Select(flag => uint64Cache.ToObject(flag, false))));
                case TypeCode.SByte:
                    var sbyteCache = (EnumsCache<sbyte>)cache;
                    return ((Func<sbyte, object>)toEnum)(sbyteCache.SetFlags(allFlags.Select(flag => sbyteCache.ToObject(flag, false))));
                case TypeCode.Byte:
                    var byteCache = (EnumsCache<byte>)cache;
                    return ((Func<byte, object>)toEnum)(byteCache.SetFlags(allFlags.Select(flag => byteCache.ToObject(flag, false))));
                case TypeCode.Int16:
                    var int16Cache = (EnumsCache<short>)cache;
                    return ((Func<short, object>)toEnum)(int16Cache.SetFlags(allFlags.Select(flag => int16Cache.ToObject(flag, false))));
                case TypeCode.UInt16:
                    var uint16Cache = (EnumsCache<ushort>)cache;
                    return ((Func<ushort, object>)toEnum)(uint16Cache.SetFlags(allFlags.Select(flag => uint16Cache.ToObject(flag, false))));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (isNullable)
            {
                if (value == null)
                {
                    if (flagMask != null)
                    {
                        NonGenericEnums.ToObject(enumType, flagMask, false);
                    }
                    return null;
                }
                if (flagMask == null)
                {
                    return NonGenericEnums.ToObject(enumType, value, false);
                }
            }

            var toEnum = enumsCache.ToEnum;
            var cache = enumsCache.Cache;
            switch (enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = (EnumsCache<int>)cache;
                    return ((Func<int, object>)toEnum)(int32Cache.ClearFlags(int32Cache.ToObject(value, false), int32Cache.ToObject(flagMask, false)));
                case TypeCode.UInt32:
                    var uint32Cache = (EnumsCache<uint>)cache;
                    return ((Func<uint, object>)toEnum)(uint32Cache.ClearFlags(uint32Cache.ToObject(value, false), uint32Cache.ToObject(flagMask, false)));
                case TypeCode.Int64:
                    var int64Cache = (EnumsCache<long>)cache;
                    return ((Func<long, object>)toEnum)(int64Cache.ClearFlags(int64Cache.ToObject(value, false), int64Cache.ToObject(flagMask, false)));
                case TypeCode.UInt64:
                    var uint64Cache = (EnumsCache<ulong>)cache;
                    return ((Func<ulong, object>)toEnum)(uint64Cache.ClearFlags(uint64Cache.ToObject(value, false), uint64Cache.ToObject(flagMask, false)));
                case TypeCode.SByte:
                    var sbyteCache = (EnumsCache<sbyte>)cache;
                    return ((Func<sbyte, object>)toEnum)(sbyteCache.ClearFlags(sbyteCache.ToObject(value, false), sbyteCache.ToObject(flagMask, false)));
                case TypeCode.Byte:
                    var byteCache = (EnumsCache<byte>)cache;
                    return ((Func<byte, object>)toEnum)(byteCache.ClearFlags(byteCache.ToObject(value, false), byteCache.ToObject(flagMask, false)));
                case TypeCode.Int16:
                    var int16Cache = (EnumsCache<short>)cache;
                    return ((Func<short, object>)toEnum)(int16Cache.ClearFlags(int16Cache.ToObject(value, false), int16Cache.ToObject(flagMask, false)));
                case TypeCode.UInt16:
                    var uint16Cache = (EnumsCache<ushort>)cache;
                    return ((Func<ushort, object>)toEnum)(uint16Cache.ClearFlags(uint16Cache.ToObject(value, false), uint16Cache.ToObject(flagMask, false)));
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
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (string.IsNullOrEmpty(value) && isNullable)
            {
                return null;
            }

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
            var isNullable = new OptionalOutParameter<bool>();
            var enumsCache = NonGenericEnumsCache.Get(enumType, isNullable);

            if (string.IsNullOrEmpty(value) && isNullable)
            {
                result = null;
                return true;
            }

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
