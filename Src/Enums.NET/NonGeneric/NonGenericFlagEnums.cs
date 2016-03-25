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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return ((NonGenericEnumInfo<int>)enumInfo).Cache.IsFlagEnum;
                case TypeCode.UInt32:
                    return ((NonGenericEnumInfo<uint>)enumInfo).Cache.IsFlagEnum;
                case TypeCode.Int64:
                    return ((NonGenericEnumInfo<long>)enumInfo).Cache.IsFlagEnum;
                case TypeCode.UInt64:
                    return ((NonGenericEnumInfo<ulong>)enumInfo).Cache.IsFlagEnum;
                case TypeCode.SByte:
                    return ((NonGenericEnumInfo<sbyte>)enumInfo).Cache.IsFlagEnum;
                case TypeCode.Byte:
                    return ((NonGenericEnumInfo<byte>)enumInfo).Cache.IsFlagEnum;
                case TypeCode.Int16:
                    return ((NonGenericEnumInfo<short>)enumInfo).Cache.IsFlagEnum;
                case TypeCode.UInt16:
                    return ((NonGenericEnumInfo<ushort>)enumInfo).Cache.IsFlagEnum;
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
            var enumInfo = NonGenericEnumInfo.Get(enumType);
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.ToEnum(int32EnumInfo.Cache.AllFlags);
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.ToEnum(uint32EnumInfo.Cache.AllFlags);
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return int64EnumInfo.ToEnum(int64EnumInfo.Cache.AllFlags);
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return uint64EnumInfo.ToEnum(uint64EnumInfo.Cache.AllFlags);
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return sbyteEnumInfo.ToEnum(sbyteEnumInfo.Cache.AllFlags);
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return byteEnumInfo.ToEnum(byteEnumInfo.Cache.AllFlags);
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return int16EnumInfo.ToEnum(int16EnumInfo.Cache.AllFlags);
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return uint16EnumInfo.ToEnum(uint16EnumInfo.Cache.AllFlags);
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return true;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.IsValidFlagCombination(int32Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.IsValidFlagCombination(uint32Cache.ToObject(value, false));
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.IsValidFlagCombination(int64Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.IsValidFlagCombination(uint64Cache.ToObject(value, false));
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.IsValidFlagCombination(sbyteCache.ToObject(value, false));
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.IsValidFlagCombination(byteCache.ToObject(value, false));
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.IsValidFlagCombination(int16Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.FormatAsFlags(int32Cache.ToObject(value, false), delimiter, formats);
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.FormatAsFlags(uint32Cache.ToObject(value, false), delimiter, formats);
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.FormatAsFlags(int64Cache.ToObject(value, false), delimiter, formats);
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.FormatAsFlags(uint64Cache.ToObject(value, false), delimiter, formats);
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.FormatAsFlags(sbyteCache.ToObject(value, false), delimiter, formats);
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.FormatAsFlags(byteCache.ToObject(value, false), delimiter, formats);
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.FormatAsFlags(int16Cache.ToObject(value, false), delimiter, formats);
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return new object[0];
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.Cache.GetFlags(int32EnumInfo.Cache.ToObject(value, false))?.Select(flag => int32EnumInfo.ToEnum(flag)).ToArray();
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.Cache.GetFlags(uint32EnumInfo.Cache.ToObject(value, false))?.Select(flag => uint32EnumInfo.ToEnum(flag)).ToArray();
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return int64EnumInfo.Cache.GetFlags(int64EnumInfo.Cache.ToObject(value, false))?.Select(flag => int64EnumInfo.ToEnum(flag)).ToArray();
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return uint64EnumInfo.Cache.GetFlags(uint64EnumInfo.Cache.ToObject(value, false))?.Select(flag => uint64EnumInfo.ToEnum(flag)).ToArray();
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return sbyteEnumInfo.Cache.GetFlags(sbyteEnumInfo.Cache.ToObject(value, false))?.Select(flag => sbyteEnumInfo.ToEnum(flag)).ToArray();
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return byteEnumInfo.Cache.GetFlags(byteEnumInfo.Cache.ToObject(value, false))?.Select(flag => byteEnumInfo.ToEnum(flag)).ToArray();
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return int16EnumInfo.Cache.GetFlags(int16EnumInfo.Cache.ToObject(value, false))?.Select(flag => int16EnumInfo.ToEnum(flag)).ToArray();
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return uint16EnumInfo.Cache.GetFlags(uint16EnumInfo.Cache.ToObject(value, false))?.Select(flag => uint16EnumInfo.ToEnum(flag)).ToArray();
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return false;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.HasAnyFlags(int32Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.HasAnyFlags(uint32Cache.ToObject(value, false));
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.HasAnyFlags(int64Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.HasAnyFlags(uint64Cache.ToObject(value, false));
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.HasAnyFlags(sbyteCache.ToObject(value, false));
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.HasAnyFlags(byteCache.ToObject(value, false));
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.HasAnyFlags(int16Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

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
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.HasAnyFlags(int32Cache.ToObject(value, false), int32Cache.ToObject(flagMask, false));
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.HasAnyFlags(uint32Cache.ToObject(value, false), uint32Cache.ToObject(flagMask, false));
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.HasAnyFlags(int64Cache.ToObject(value, false), int64Cache.ToObject(flagMask, false));
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.HasAnyFlags(uint64Cache.ToObject(value, false), uint64Cache.ToObject(flagMask, false));
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.HasAnyFlags(sbyteCache.ToObject(value, false), sbyteCache.ToObject(flagMask, false));
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.HasAnyFlags(byteCache.ToObject(value, false), byteCache.ToObject(flagMask, false));
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.HasAnyFlags(int16Cache.ToObject(value, false), int16Cache.ToObject(flagMask, false));
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return false;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.HasAllFlags(int32Cache.ToObject(value, false));
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.HasAllFlags(uint32Cache.ToObject(value, false));
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.HasAllFlags(int64Cache.ToObject(value, false));
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.HasAllFlags(uint64Cache.ToObject(value, false));
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.HasAllFlags(sbyteCache.ToObject(value, false));
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.HasAllFlags(byteCache.ToObject(value, false));
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.HasAllFlags(int16Cache.ToObject(value, false));
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

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
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32Cache = ((NonGenericEnumInfo<int>)enumInfo).Cache;
                    return int32Cache.HasAllFlags(int32Cache.ToObject(value, false), int32Cache.ToObject(flagMask, false));
                case TypeCode.UInt32:
                    var uint32Cache = ((NonGenericEnumInfo<uint>)enumInfo).Cache;
                    return uint32Cache.HasAllFlags(uint32Cache.ToObject(value, false), uint32Cache.ToObject(flagMask, false));
                case TypeCode.Int64:
                    var int64Cache = ((NonGenericEnumInfo<long>)enumInfo).Cache;
                    return int64Cache.HasAllFlags(int64Cache.ToObject(value, false), int64Cache.ToObject(flagMask, false));
                case TypeCode.UInt64:
                    var uint64Cache = ((NonGenericEnumInfo<ulong>)enumInfo).Cache;
                    return uint64Cache.HasAllFlags(uint64Cache.ToObject(value, false), uint64Cache.ToObject(flagMask, false));
                case TypeCode.SByte:
                    var sbyteCache = ((NonGenericEnumInfo<sbyte>)enumInfo).Cache;
                    return sbyteCache.HasAllFlags(sbyteCache.ToObject(value, false), sbyteCache.ToObject(flagMask, false));
                case TypeCode.Byte:
                    var byteCache = ((NonGenericEnumInfo<byte>)enumInfo).Cache;
                    return byteCache.HasAllFlags(byteCache.ToObject(value, false), byteCache.ToObject(flagMask, false));
                case TypeCode.Int16:
                    var int16Cache = ((NonGenericEnumInfo<short>)enumInfo).Cache;
                    return int16Cache.HasAllFlags(int16Cache.ToObject(value, false), int16Cache.ToObject(flagMask, false));
                case TypeCode.UInt16:
                    var uint16Cache = ((NonGenericEnumInfo<ushort>)enumInfo).Cache;
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (value == null && isNullable)
            {
                return GetAllFlags(enumType);
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.ToEnum(int32EnumInfo.Cache.ToggleFlags(int32EnumInfo.Cache.ToObject(value, false)));
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.ToEnum(uint32EnumInfo.Cache.ToggleFlags(uint32EnumInfo.Cache.ToObject(value, false)));
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return int64EnumInfo.ToEnum(int64EnumInfo.Cache.ToggleFlags(int64EnumInfo.Cache.ToObject(value, false)));
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return uint64EnumInfo.ToEnum(uint64EnumInfo.Cache.ToggleFlags(uint64EnumInfo.Cache.ToObject(value, false)));
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return sbyteEnumInfo.ToEnum(sbyteEnumInfo.Cache.ToggleFlags(sbyteEnumInfo.Cache.ToObject(value, false)));
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return byteEnumInfo.ToEnum(byteEnumInfo.Cache.ToggleFlags(byteEnumInfo.Cache.ToObject(value, false)));
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return int16EnumInfo.ToEnum(int16EnumInfo.Cache.ToggleFlags(int16EnumInfo.Cache.ToObject(value, false)));
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return uint16EnumInfo.ToEnum(uint16EnumInfo.Cache.ToggleFlags(uint16EnumInfo.Cache.ToObject(value, false)));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

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
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.ToEnum(int32EnumInfo.Cache.ToggleFlags(int32EnumInfo.Cache.ToObject(value, false), int32EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.ToEnum(uint32EnumInfo.Cache.ToggleFlags(uint32EnumInfo.Cache.ToObject(value, false), uint32EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return int64EnumInfo.ToEnum(int64EnumInfo.Cache.ToggleFlags(int64EnumInfo.Cache.ToObject(value, false), int64EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return uint64EnumInfo.ToEnum(uint64EnumInfo.Cache.ToggleFlags(uint64EnumInfo.Cache.ToObject(value, false), uint64EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return sbyteEnumInfo.ToEnum(sbyteEnumInfo.Cache.ToggleFlags(sbyteEnumInfo.Cache.ToObject(value, false), sbyteEnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return byteEnumInfo.ToEnum(byteEnumInfo.Cache.ToggleFlags(byteEnumInfo.Cache.ToObject(value, false), byteEnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return int16EnumInfo.ToEnum(int16EnumInfo.Cache.ToggleFlags(int16EnumInfo.Cache.ToObject(value, false), int16EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return uint16EnumInfo.ToEnum(uint16EnumInfo.Cache.ToggleFlags(uint16EnumInfo.Cache.ToObject(value, false), uint16EnumInfo.Cache.ToObject(flagMask, false)));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

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
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.ToEnum(int32EnumInfo.Cache.CommonFlags(int32EnumInfo.Cache.ToObject(value, false), int32EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.ToEnum(uint32EnumInfo.Cache.CommonFlags(uint32EnumInfo.Cache.ToObject(value, false), uint32EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return int64EnumInfo.ToEnum(int64EnumInfo.Cache.CommonFlags(int64EnumInfo.Cache.ToObject(value, false), int64EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return uint64EnumInfo.ToEnum(uint64EnumInfo.Cache.CommonFlags(uint64EnumInfo.Cache.ToObject(value, false), uint64EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return sbyteEnumInfo.ToEnum(sbyteEnumInfo.Cache.CommonFlags(sbyteEnumInfo.Cache.ToObject(value, false), sbyteEnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return byteEnumInfo.ToEnum(byteEnumInfo.Cache.CommonFlags(byteEnumInfo.Cache.ToObject(value, false), byteEnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return int16EnumInfo.ToEnum(int16EnumInfo.Cache.CommonFlags(int16EnumInfo.Cache.ToObject(value, false), int16EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return uint16EnumInfo.ToEnum(uint16EnumInfo.Cache.CommonFlags(uint16EnumInfo.Cache.ToObject(value, false), uint16EnumInfo.Cache.ToObject(flagMask, false)));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

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
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.ToEnum(int32EnumInfo.Cache.SetFlags(int32EnumInfo.Cache.ToObject(flag0, false), int32EnumInfo.Cache.ToObject(flag1, false)));
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.ToEnum(uint32EnumInfo.Cache.SetFlags(uint32EnumInfo.Cache.ToObject(flag0, false), uint32EnumInfo.Cache.ToObject(flag1, false)));
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return int64EnumInfo.ToEnum(int64EnumInfo.Cache.SetFlags(int64EnumInfo.Cache.ToObject(flag0, false), int64EnumInfo.Cache.ToObject(flag1, false)));
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return uint64EnumInfo.ToEnum(uint64EnumInfo.Cache.SetFlags(uint64EnumInfo.Cache.ToObject(flag0, false), uint64EnumInfo.Cache.ToObject(flag1, false)));
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return sbyteEnumInfo.ToEnum(sbyteEnumInfo.Cache.SetFlags(sbyteEnumInfo.Cache.ToObject(flag0, false), sbyteEnumInfo.Cache.ToObject(flag1, false)));
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return byteEnumInfo.ToEnum(byteEnumInfo.Cache.SetFlags(byteEnumInfo.Cache.ToObject(flag0, false), byteEnumInfo.Cache.ToObject(flag1, false)));
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return int16EnumInfo.ToEnum(int16EnumInfo.Cache.SetFlags(int16EnumInfo.Cache.ToObject(flag0, false), int16EnumInfo.Cache.ToObject(flag1, false)));
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return uint16EnumInfo.ToEnum(uint16EnumInfo.Cache.SetFlags(uint16EnumInfo.Cache.ToObject(flag0, false), uint16EnumInfo.Cache.ToObject(flag1, false)));
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        [Pure]
        public static object SetFlags(Type enumType, params object[] flags)
        {
            Preconditions.NotNull(flags, nameof(flags));

            var isNullable = new OptionalOutParameter<bool>();
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            var allFlags = isNullable ? flags.Where(flag => flag != null) : flags;
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.ToEnum(int32EnumInfo.Cache.SetFlags(allFlags.Select(flag => int32EnumInfo.Cache.ToObject(flag, false))));
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.ToEnum(uint32EnumInfo.Cache.SetFlags(allFlags.Select(flag => uint32EnumInfo.Cache.ToObject(flag, false))));
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return int64EnumInfo.ToEnum(int64EnumInfo.Cache.SetFlags(allFlags.Select(flag => int64EnumInfo.Cache.ToObject(flag, false))));
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return uint64EnumInfo.ToEnum(uint64EnumInfo.Cache.SetFlags(allFlags.Select(flag => uint64EnumInfo.Cache.ToObject(flag, false))));
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return sbyteEnumInfo.ToEnum(sbyteEnumInfo.Cache.SetFlags(allFlags.Select(flag => sbyteEnumInfo.Cache.ToObject(flag, false))));
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return byteEnumInfo.ToEnum(byteEnumInfo.Cache.SetFlags(allFlags.Select(flag => byteEnumInfo.Cache.ToObject(flag, false))));
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return int16EnumInfo.ToEnum(int16EnumInfo.Cache.SetFlags(allFlags.Select(flag => int16EnumInfo.Cache.ToObject(flag, false))));
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return uint16EnumInfo.ToEnum(uint16EnumInfo.Cache.SetFlags(allFlags.Select(flag => uint16EnumInfo.Cache.ToObject(flag, false))));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

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

            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.ToEnum(int32EnumInfo.Cache.ClearFlags(int32EnumInfo.Cache.ToObject(value, false), int32EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.ToEnum(uint32EnumInfo.Cache.ClearFlags(uint32EnumInfo.Cache.ToObject(value, false), uint32EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return int64EnumInfo.ToEnum(int64EnumInfo.Cache.ClearFlags(int64EnumInfo.Cache.ToObject(value, false), int64EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return uint64EnumInfo.ToEnum(uint64EnumInfo.Cache.ClearFlags(uint64EnumInfo.Cache.ToObject(value, false), uint64EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return sbyteEnumInfo.ToEnum(sbyteEnumInfo.Cache.ClearFlags(sbyteEnumInfo.Cache.ToObject(value, false), sbyteEnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return byteEnumInfo.ToEnum(byteEnumInfo.Cache.ClearFlags(byteEnumInfo.Cache.ToObject(value, false), byteEnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return int16EnumInfo.ToEnum(int16EnumInfo.Cache.ClearFlags(int16EnumInfo.Cache.ToObject(value, false), int16EnumInfo.Cache.ToObject(flagMask, false)));
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return uint16EnumInfo.ToEnum(uint16EnumInfo.Cache.ClearFlags(uint16EnumInfo.Cache.ToObject(value, false), uint16EnumInfo.Cache.ToObject(flagMask, false)));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (string.IsNullOrEmpty(value) && isNullable)
            {
                return null;
            }
            
            switch (enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    var int32EnumInfo = (NonGenericEnumInfo<int>)enumInfo;
                    return int32EnumInfo.ToEnum(int32EnumInfo.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    return uint32EnumInfo.ToEnum(uint32EnumInfo.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    return int64EnumInfo.ToEnum(int64EnumInfo.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    return uint64EnumInfo.ToEnum(uint64EnumInfo.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    return sbyteEnumInfo.ToEnum(sbyteEnumInfo.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    return byteEnumInfo.ToEnum(byteEnumInfo.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    return int16EnumInfo.ToEnum(int16EnumInfo.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    return uint16EnumInfo.ToEnum(uint16EnumInfo.Cache.ParseFlags(value, ignoreCase, delimiter, parseFormatOrder));
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
            var enumInfo = NonGenericEnumInfo.Get(enumType, isNullable);

            if (string.IsNullOrEmpty(value) && isNullable)
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
                    success = int32EnumInfo.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsInt32, parseFormatOrder);
                    result = int32EnumInfo.ToEnum(resultAsInt32);
                    return success;
                case TypeCode.UInt32:
                    var uint32EnumInfo = (NonGenericEnumInfo<uint>)enumInfo;
                    uint resultAsUInt32;
                    success = uint32EnumInfo.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsUInt32, parseFormatOrder);
                    result = uint32EnumInfo.ToEnum(resultAsUInt32);
                    return success;
                case TypeCode.Int64:
                    var int64EnumInfo = (NonGenericEnumInfo<long>)enumInfo;
                    long resultAsInt64;
                    success = int64EnumInfo.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsInt64, parseFormatOrder);
                    result = int64EnumInfo.ToEnum(resultAsInt64);
                    return success;
                case TypeCode.UInt64:
                    var uint64EnumInfo = (NonGenericEnumInfo<ulong>)enumInfo;
                    ulong resultAsUInt64;
                    success = uint64EnumInfo.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsUInt64, parseFormatOrder);
                    result = uint64EnumInfo.ToEnum(resultAsUInt64);
                    return success;
                case TypeCode.SByte:
                    var sbyteEnumInfo = (NonGenericEnumInfo<sbyte>)enumInfo;
                    sbyte resultAsSByte;
                    success = sbyteEnumInfo.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsSByte, parseFormatOrder);
                    result = sbyteEnumInfo.ToEnum(resultAsSByte);
                    return success;
                case TypeCode.Byte:
                    var byteEnumInfo = (NonGenericEnumInfo<byte>)enumInfo;
                    byte resultAsByte;
                    success = byteEnumInfo.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsByte, parseFormatOrder);
                    result = byteEnumInfo.ToEnum(resultAsByte);
                    return success;
                case TypeCode.Int16:
                    var int16EnumInfo = (NonGenericEnumInfo<short>)enumInfo;
                    short resultAsInt16;
                    success = int16EnumInfo.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsInt16, parseFormatOrder);
                    result = int16EnumInfo.ToEnum(resultAsInt16);
                    return success;
                case TypeCode.UInt16:
                    var uint16EnumInfo = (NonGenericEnumInfo<ushort>)enumInfo;
                    ushort resultAsUInt16;
                    success = uint16EnumInfo.Cache.TryParseFlags(value, ignoreCase, delimiter, out resultAsUInt16, parseFormatOrder);
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
