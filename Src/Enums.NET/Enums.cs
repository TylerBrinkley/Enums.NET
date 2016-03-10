// Enums.NET
// Copyright 2016 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//	  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#define USE_EMIT

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using ExtraConstraints;
using System.Linq;

#if NET20 || USE_EMIT
using System.Reflection.Emit;
#else
using System.Linq.Expressions;
#endif

namespace EnumsNET
{
    /// <summary>
    /// Static class that provides efficient type-safe enum operations through the use of cached enum names, values, and attributes.
    /// Many operations are exposed as extension methods for convenience.
    /// </summary>
    public static class Enums
    {
        internal static readonly EnumFormat[] DefaultFormatOrder = { EnumFormat.Name, EnumFormat.DecimalValue };

        internal static readonly Attribute[] EmptyAttributes = { };

        private const int _startingCustomEnumFormatValue = 100;

        private const int _startingGenericCustomEnumFormatValue = 200;

        private static int _lastCustomEnumFormatIndex = -1;

        private static List<Func<EnumMemberInfo, string>> _customEnumFormatters;
        
        /// <summary>
        /// Registers a universal custom enum format
        /// </summary>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public static EnumFormat RegisterCustomEnumFormat(Func<EnumMemberInfo, string> formatter)
        {
            Preconditions.NotNull(formatter, nameof(formatter));

            var index = Interlocked.Increment(ref _lastCustomEnumFormatIndex);
            if (index == 0)
            {
                _customEnumFormatters = new List<Func<EnumMemberInfo, string>>();
            }
            else
            {
                while (_customEnumFormatters?.Count != index)
                {
                }
            }
            _customEnumFormatters.Add(formatter);
            return (EnumFormat)(index + _startingCustomEnumFormatValue);
        }

        #region "Properties"
        /// <summary>
        /// Indicates if <typeparamref name="TEnum"/>'s defined values are contiguous.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>Indication if <typeparamref name="TEnum"/>'s defined values are contiguous.</returns>
        [Pure]
        public static bool IsContiguous<[EnumConstraint] TEnum>()
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>The underlying type of <typeparamref name="TEnum"/>.</returns>
        [Pure]
        public static Type GetUnderlyingType<[EnumConstraint] TEnum>()
            where TEnum : struct
        {
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
        public static TypeCode GetTypeCode<[EnumConstraint] TEnum>() where TEnum : struct => Enums<TEnum>.TypeCode;
        #endregion

        #region Type Methods
        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members count.
        /// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns><typeparamref name="TEnum"/>'s members count.</returns>
        [Pure]
        public static int GetDefinedCount<[EnumConstraint] TEnum>(bool uniqueValued = false)
            where TEnum : struct
        {
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

        /// <summary>
        /// Retrieves in value order an array of info on <typeparamref name="TEnum"/>'s members.
        /// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns></returns>
        [Pure]
        public static IEnumerable<EnumMemberInfo<TEnum>> GetEnumMemberInfos<[EnumConstraint] TEnum>(bool uniqueValued = false)
            where TEnum : struct
        {
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
        /// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' names.
        /// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <typeparamref name="TEnum"/>'s members' names in value order.</returns>
        [Pure]
        public static IEnumerable<string> GetNames<[EnumConstraint] TEnum>(bool uniqueValued = false)
            where TEnum : struct
        {
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
        /// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' values.
        /// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <typeparamref name="TEnum"/>'s members' values in value order.</returns>
        [Pure]
        public static IEnumerable<TEnum> GetValues<[EnumConstraint] TEnum>(bool uniqueValued = false)
            where TEnum : struct
        {
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
        /// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' <see cref="DescriptionAttribute.Description"/>s.
        /// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <typeparamref name="TEnum"/>'s members' <see cref="DescriptionAttribute.Description"/>s in value order.</returns>
        [Pure]
        public static IEnumerable<string> GetDescriptions<[EnumConstraint] TEnum>(bool uniqueValued = false)
            where TEnum : struct
        {
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

        public static IEnumerable<string> GetFormattedValues<[EnumConstraint] TEnum>(EnumFormat[] formats, bool uniqueValued = false)
            where TEnum : struct
        {
            switch (Enums<TEnum>.TypeCode)
            {
                case TypeCode.Int32:
                    return Enums<TEnum, int>.Cache.GetFormattedValues(formats, uniqueValued);
                case TypeCode.UInt32:
                    return Enums<TEnum, uint>.Cache.GetFormattedValues(formats, uniqueValued);
                case TypeCode.Int64:
                    return Enums<TEnum, long>.Cache.GetFormattedValues(formats, uniqueValued);
                case TypeCode.UInt64:
                    return Enums<TEnum, ulong>.Cache.GetFormattedValues(formats, uniqueValued);
                case TypeCode.SByte:
                    return Enums<TEnum, sbyte>.Cache.GetFormattedValues(formats, uniqueValued);
                case TypeCode.Byte:
                    return Enums<TEnum, byte>.Cache.GetFormattedValues(formats, uniqueValued);
                case TypeCode.Int16:
                    return Enums<TEnum, short>.Cache.GetFormattedValues(formats, uniqueValued);
                case TypeCode.UInt16:
                    return Enums<TEnum, ushort>.Cache.GetFormattedValues(formats, uniqueValued);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }

        /// <summary>
        /// Retrieves in value order an array of all of <typeparamref name="TEnum"/>'s members' attributes.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of all of <typeparamref name="TEnum"/>'s members' attributes in value order.</returns>
        [Pure]
        public static IEnumerable<Attribute[]> GetAttributes<[EnumConstraint] TEnum>(bool uniqueValued = false)
            where TEnum : struct
        {
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
        /// Retrieves in value order an array of <typeparamref name="TEnum"/>'s members' <typeparamref name="TAttribute"/>s.
        /// The optional parameter <paramref name="uniqueValued"/> indicates whether to exclude duplicate values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="uniqueValued"></param>
        /// <returns>Array of <typeparamref name="TEnum"/>'s members' <typeparamref name="TAttribute"/> in value order.</returns>
        [Pure]
        public static IEnumerable<TAttribute> GetAttributes<[EnumConstraint] TEnum, TAttribute>(bool uniqueValued = false)
            where TAttribute : Attribute
            where TEnum : struct
        {
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
        /// Compares two <typeparamref name="TEnum"/>'s for ordering.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>1 if <paramref name="x"/> is greater than <paramref name="y"/>, 0 if <paramref name="x"/> equals <paramref name="y"/>,
        /// and -1 if <paramref name="x"/> is less than <paramref name="y"/>.</returns>
        [Pure]
        public static int Compare<[EnumConstraint] TEnum>(TEnum x, TEnum y)
            where TEnum : struct
        {
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
        public static EnumFormat RegisterCustomEnumFormat<[EnumConstraint] TEnum>(Func<EnumMemberInfo<TEnum>, string> formatter)
            where TEnum : struct
        {
            return InternalRegisterCustomEnumFormat(formatter);
        }
        #endregion

        #region IsValid
        /// <summary>
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
        [Pure]
        public static bool IsValid<[EnumConstraint] TEnum>(object value)
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Indication whether <paramref name="value"/> is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
        [Pure]
        public static bool IsValid<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsValid<[EnumConstraint] TEnum>(sbyte value)
            where TEnum : struct => IsValid<TEnum>((long)value);

        /// <summary>
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
        [Pure]
        public static bool IsValid<[EnumConstraint] TEnum>(byte value)
            where TEnum : struct => IsValid<TEnum>((ulong)value);

        /// <summary>
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
        [Pure]
        public static bool IsValid<[EnumConstraint] TEnum>(short value)
            where TEnum : struct => IsValid<TEnum>((long)value);

        /// <summary>
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsValid<[EnumConstraint] TEnum>(ushort value)
            where TEnum : struct => IsValid<TEnum>((ulong)value);

        /// <summary>
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
        [Pure]
        public static bool IsValid<[EnumConstraint] TEnum>(int value)
            where TEnum : struct => IsValid<TEnum>((long)value);

        /// <summary>
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsValid<[EnumConstraint] TEnum>(uint value)
            where TEnum : struct => IsValid<TEnum>((ulong)value);

        /// <summary>
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
        [Pure]
        public static bool IsValid<[EnumConstraint] TEnum>(long value)
            where TEnum : struct
        {
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
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined or if <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/>
        /// whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsValid<[EnumConstraint] TEnum>(ulong value)
            where TEnum : struct
        {
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
        /// Indicates whether <paramref name="value"/> can be converted to <typeparamref name="TEnum"/> and is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">must be <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to <typeparamref name="TEnum"/> and is defined.</returns>
        [Pure]
        public static bool IsDefined<[EnumConstraint] TEnum>(object value)
            where TEnum : struct
        {
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
        /// Indicates whether <paramref name="value"/> is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Indication whether <paramref name="value"/> is defined.</returns>
        [Pure]
        public static bool IsDefined<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="name"></param>
        /// <param name="ignoreCase">Specifies whether the operation is case-insensitive.</param>
        /// <returns>Indication whether a constant with the specified <paramref name="name"/> exists in <typeparamref name="TEnum"/>.
        /// <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null</exception>
        [Pure]
        public static bool IsDefined<[EnumConstraint] TEnum>(string name, bool ignoreCase = false)
            where TEnum : struct
        {
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
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsDefined<[EnumConstraint] TEnum>(sbyte value)
            where TEnum : struct => IsDefined<TEnum>((long)value);

        /// <summary>
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.</returns>
        [Pure]
        public static bool IsDefined<[EnumConstraint] TEnum>(byte value)
            where TEnum : struct => IsDefined<TEnum>((ulong)value);

        /// <summary>
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.</returns>
        [Pure]
        public static bool IsDefined<[EnumConstraint] TEnum>(short value)
            where TEnum : struct => IsDefined<TEnum>((long)value);

        /// <summary>
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsDefined<[EnumConstraint] TEnum>(ushort value)
            where TEnum : struct => IsDefined<TEnum>((ulong)value);

        /// <summary>
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.</returns>
        [Pure]
        public static bool IsDefined<[EnumConstraint] TEnum>(int value)
            where TEnum : struct => IsDefined<TEnum>((long)value);

        /// <summary>
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsDefined<[EnumConstraint] TEnum>(uint value)
            where TEnum : struct => IsDefined<TEnum>((ulong)value);

        /// <summary>
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.</returns>
        [Pure]
        public static bool IsDefined<[EnumConstraint] TEnum>(long value)
            where TEnum : struct
        {
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
        /// Indicates whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether <paramref name="value"/> can be converted to a <typeparamref name="TEnum"/>
        /// and that that value is defined.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsDefined<[EnumConstraint] TEnum>(ulong value)
            where TEnum : struct
        {
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
        /// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsInValueRange<[EnumConstraint] TEnum>(sbyte value)
            where TEnum : struct => IsInValueRange<TEnum>((long)value);

        /// <summary>
        /// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
        [Pure]
        public static bool IsInValueRange<[EnumConstraint] TEnum>(byte value)
            where TEnum : struct => IsInValueRange<TEnum>((ulong)value);

        /// <summary>
        /// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
        [Pure]
        public static bool IsInValueRange<[EnumConstraint] TEnum>(short value)
            where TEnum : struct => IsInValueRange<TEnum>((long)value);

        /// <summary>
        /// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsInValueRange<[EnumConstraint] TEnum>(ushort value)
            where TEnum : struct => IsInValueRange<TEnum>((ulong)value);

        /// <summary>
        /// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
        [Pure]
        public static bool IsInValueRange<[EnumConstraint] TEnum>(int value)
            where TEnum : struct => IsInValueRange<TEnum>((long)value);

        /// <summary>
        /// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsInValueRange<[EnumConstraint] TEnum>(uint value)
            where TEnum : struct => IsInValueRange<TEnum>((ulong)value);

        /// <summary>
        /// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
        [Pure]
        public static bool IsInValueRange<[EnumConstraint] TEnum>(long value)
            where TEnum : struct
        {
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
        /// Indicates whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Indication whether the specified <paramref name="value"/> is within <typeparamref name="TEnum"/>'s underlying type's value range.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool IsInValueRange<[EnumConstraint] TEnum>(ulong value)
            where TEnum : struct
        {
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
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that the result is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/>.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not a valid type
        /// -or-
        /// <paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(object value, bool validate = true)
            where TEnum : struct
        {
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
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<[EnumConstraint] TEnum>(sbyte value, bool validate = true)
            where TEnum : struct => ToObject<TEnum>((long)value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(byte value, bool validate = true)
            where TEnum : struct => ToObject<TEnum>((ulong)value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(short value, bool validate = true)
            where TEnum : struct => ToObject<TEnum>((long)value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<[EnumConstraint] TEnum>(ushort value, bool validate = true)
            where TEnum : struct => ToObject<TEnum>((ulong)value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(int value, bool validate = true)
            where TEnum : struct => ToObject<TEnum>((long)value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<[EnumConstraint] TEnum>(uint value, bool validate = true)
            where TEnum : struct => ToObject<TEnum>((ulong)value, validate);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        public static TEnum ToObject<[EnumConstraint] TEnum>(long value, bool validate = true)
            where TEnum : struct
        {
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
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="validate"/> is true and <paramref name="value"/> is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObject<[EnumConstraint] TEnum>(ulong value, bool validate = true)
            where TEnum : struct
        {
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
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="defaultEnum">The fallback value to return.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(object value, TEnum defaultEnum, bool validate = true)
            where TEnum : struct
        {
            TEnum result;
            return TryToObject(value, out result, validate) ? result : defaultEnum;
        }

        /// <summary>
        /// Tries to converts the specified 8-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultEnum">The fallback value to return.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(sbyte value, TEnum defaultEnum, bool validate = true)
            where TEnum : struct => ToObjectOrDefault((long)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultEnum">The fallback value to return.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(byte value, TEnum defaultEnum, bool validate = true)
            where TEnum : struct => ToObjectOrDefault((ulong)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultEnum">The fallback value to return.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(short value, TEnum defaultEnum, bool validate = true)
            where TEnum : struct => ToObjectOrDefault((long)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultEnum">The fallback value to return.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(ushort value, TEnum defaultEnum, bool validate = true)
            where TEnum : struct => ToObjectOrDefault((ulong)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultEnum">The fallback value to return.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(int value, TEnum defaultEnum, bool validate = true)
            where TEnum : struct => ToObjectOrDefault((long)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultEnum">The fallback value to return.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(uint value, TEnum defaultEnum, bool validate = true)
            where TEnum : struct => ToObjectOrDefault((ulong)value, defaultEnum, validate);

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultEnum">The fallback value to return.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
        [Pure]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(long value, TEnum defaultEnum, bool validate = true)
            where TEnum : struct
        {
            TEnum result;
            return TryToObject(value, out result, validate) ? result : defaultEnum;
        }

        /// <summary>
        /// Tries to converts the specified 64-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. If it fails
        /// <paramref name="defaultEnum"/> is returned.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="defaultEnum">The fallback value to return.</param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns>If succeeded <paramref name="value"/> converted to a <typeparamref name="TEnum"/> otherwise <paramref name="defaultEnum"/>.</returns>
        [Pure]
        [CLSCompliant(false)]
        public static TEnum ToObjectOrDefault<[EnumConstraint] TEnum>(ulong value, TEnum defaultEnum, bool validate = true)
            where TEnum : struct
        {
            TEnum result;
            return TryToObject(value, out result, validate) ? result : defaultEnum;
        }

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it's within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is returned in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, or <see cref="string"/></param>
        /// <param name="result"></param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(object value, out TEnum result, bool validate = true)
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<[EnumConstraint] TEnum>(sbyte value, out TEnum result, bool validate = true)
            where TEnum : struct => TryToObject((long)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 8-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(byte value, out TEnum result, bool validate = true)
            where TEnum : struct => TryToObject((ulong)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(short value, out TEnum result, bool validate = true)
            where TEnum : struct => TryToObject((long)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 16-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<[EnumConstraint] TEnum>(ushort value, out TEnum result, bool validate = true)
            where TEnum : struct => TryToObject((ulong)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(int value, out TEnum result, bool validate = true)
            where TEnum : struct => TryToObject((long)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 32-bit unsigned integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<[EnumConstraint] TEnum>(uint value, out TEnum result, bool validate = true)
            where TEnum : struct => TryToObject((ulong)value, out result, validate);

        /// <summary>
        /// Tries to converts the specified 64-bit signed integer <paramref name="value"/> to an enumeration member while checking that the <paramref name="value"/> is within the
        /// underlying types value range. The optional parameter <paramref name="validate"/> indicates whether to check that the result is valid. An indication
        /// if the operation succeeded is returned and the result of the operation or if it fails the default enumeration value is stored in the output parameter <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        public static bool TryToObject<[EnumConstraint] TEnum>(long value, out TEnum result, bool validate = true)
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result"></param>
        /// <param name="validate">(Optional) Indicates whether to check that the result is valid.</param>
        /// <returns></returns>
        [Pure]
        [CLSCompliant(false)]
        public static bool TryToObject<[EnumConstraint] TEnum>(ulong value, out TEnum result, bool validate = true)
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <returns><paramref name="value"/> for use in constructor initializers and fluent API's</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is invalid</exception>
        [Pure]
        public static TEnum Validate<[EnumConstraint] TEnum>(this TEnum value, string paramName)
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        public static string AsString<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value</exception>
        [Pure]
        public static string AsString<[EnumConstraint] TEnum>(this TEnum value, string format)
            where TEnum : struct
        {
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

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="formats"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        [Pure]
        public static string AsString<[EnumConstraint] TEnum>(this TEnum value, params EnumFormat[] formats)
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        [Pure]
        public static string Format<[EnumConstraint] TEnum>(this TEnum value, string format)
            where TEnum : struct
        {
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
        public static string Format<[EnumConstraint] TEnum>(this TEnum value, EnumFormat format)
            where TEnum : struct
        {
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
        public static string Format<[EnumConstraint] TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1)
            where TEnum : struct
        {
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
        public static string Format<[EnumConstraint] TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2)
            where TEnum : struct
        {
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
        public static string Format<[EnumConstraint] TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3)
            where TEnum : struct
        {
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
        public static string Format<[EnumConstraint] TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4)
            where TEnum : struct
        {
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

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent string representation according to the specified <paramref name="formats"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="formats"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="formats"/> is empty.</exception>
        [Pure]
        public static string Format<[EnumConstraint] TEnum>(this TEnum value, params EnumFormat[] formats)
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        public static object GetUnderlyingValue<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        /// Converts <paramref name="value"/> to an <see cref="sbyte"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="sbyte"/>'s value range without overflowing</exception>
        [Pure]
        [CLSCompliant(false)]
        public static sbyte ToSByte<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        /// Converts <paramref name="value"/> to a <see cref="byte"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="byte"/>'s value range without overflowing</exception>
        [Pure]
        public static byte ToByte<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        /// Converts <paramref name="value"/> to an <see cref="short"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="short"/>'s value range without overflowing</exception>
        [Pure]
        public static short ToInt16<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        /// Converts <paramref name="value"/> to a <see cref="ushort"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ushort"/>'s value range without overflowing</exception>
        [Pure]
        [CLSCompliant(false)]
        public static ushort ToUInt16<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        /// Converts <paramref name="value"/> to an <see cref="int"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="int"/>'s value range without overflowing</exception>
        [Pure]
        public static int ToInt32<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        /// Converts <paramref name="value"/> to a <see cref="uint"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="uint"/>'s value range without overflowing</exception>
        [Pure]
        [CLSCompliant(false)]
        public static uint ToUInt32<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        /// Converts <paramref name="value"/> to an <see cref="long"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="long"/>'s value range without overflowing</exception>
        [Pure]
        public static long ToInt64<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        /// Converts <paramref name="value"/> to a <see cref="ulong"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ulong"/>'s value range without overflowing</exception>
        [Pure]
        [CLSCompliant(false)]
        public static ulong ToUInt64<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        public static int GetHashCode<[EnumConstraint] TEnum>(TEnum value)
            where TEnum : struct
        {
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
        public static bool Equals<[EnumConstraint] TEnum>(this TEnum value, TEnum other)
            where TEnum : struct
        {
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
        public static EnumMemberInfo<TEnum> GetEnumMemberInfo<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        public static EnumMemberInfo<TEnum> GetEnumMemberInfo<[EnumConstraint] TEnum>(string name, bool ignoreCase = false)
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Name of the constant in <typeparamref name="TEnum"/> that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined null is returned.</returns>
        [Pure]
        public static string GetName<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns>Description of the constant in the enumeration that has the specified <paramref name="value"/>. If <paramref name="value"/>
        /// is not defined or no associated <see cref="DescriptionAttribute"/> is found then null is returned.</returns>
        [Pure]
        public static string GetDescription<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        #endregion

        #region Attributes
        /// <summary>
        /// Indicates if the enumerated constant with the specified <paramref name="value"/> has a <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="value"></param>
        /// <returns>Indication if the enumerated constant with the specified <paramref name="value"/> has a <typeparamref name="TAttribute"/>.</returns>
        [Pure]
        public static bool HasAttribute<[EnumConstraint] TEnum, TAttribute>(this TEnum value)
            where TAttribute : Attribute
            where TEnum : struct => GetAttribute<TEnum, TAttribute>(value) != null;

        /// <summary>
        /// Retrieves the <typeparamref name="TAttribute"/> if it exists of the enumerated constant with the specified <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="value"></param>
        /// <returns><typeparamref name="TAttribute"/> of the enumerated constant with the specified <paramref name="value"/> if defined and has attribute, else null</returns>
        [Pure]
        public static TAttribute GetAttribute<[EnumConstraint] TEnum, TAttribute>(this TEnum value)
            where TAttribute : Attribute
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <param name="selector"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is null</exception>
        [Pure]
        public static TResult GetAttributeSelect<[EnumConstraint] TEnum, TAttribute, TResult>(this TEnum value, Func<TAttribute, TResult> selector, TResult defaultValue = default(TResult))
            where TAttribute : Attribute
            where TEnum : struct
        {
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

        /// <summary>
        /// Tries to retrieve the first <typeparamref name="TAttribute"/> if it exists of the enumeration constant with the specified <paramref name="value"/>
        /// and sets <paramref name="result"/> to the result of applying the <paramref name="selector"/> to the <typeparamref name="TAttribute"/>.
        /// Returns true if a <typeparamref name="TAttribute"/> is found else false.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <param name="selector"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        public static bool TryGetAttributeSelect<[EnumConstraint] TEnum, TAttribute, TResult>(this TEnum value, Func<TAttribute, TResult> selector, out TResult result)
            where TAttribute : Attribute
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="value"></param>
        /// <returns><typeparamref name="TAttribute"/> array</returns>
        [Pure]
        public static IEnumerable<TAttribute> GetAttributes<[EnumConstraint] TEnum, TAttribute>(this TEnum value)
            where TAttribute : Attribute
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns><see cref="Attribute"/> array if value is defined, else null</returns>
        [Pure]
        public static Attribute[] GetAttributes<[EnumConstraint] TEnum>(this TEnum value)
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/></exception>
        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value)
            where TEnum : struct => Parse<TEnum>(value, false, null);

        /// <summary>
        /// Converts the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/></exception>
        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => Parse<TEnum>(value, false, parseFormatOrder);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/></exception>
        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value, bool ignoreCase)
            where TEnum : struct => Parse<TEnum>(value, ignoreCase);

        /// <summary>
        /// Converts the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is either an empty string or only contains white space.
        /// -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/></exception>
        [Pure]
        public static TEnum Parse<[EnumConstraint] TEnum>(string value, bool ignoreCase, params EnumFormat[] parseFormatOrder)
            where TEnum : struct
        {
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
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, TEnum defaultEnum)
            where TEnum : struct => ParseOrDefault(value, false, defaultEnum, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, TEnum defaultEnum, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => ParseOrDefault(value, false, defaultEnum, parseFormatOrder);

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object but if it fails returns the specified default enumerated value.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, TEnum defaultEnum)
            where TEnum : struct => ParseOrDefault(value, ignoreCase, defaultEnum, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>
        /// but if it fails returns the specified default enumerated value. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="defaultEnum"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        [Pure]
        public static TEnum ParseOrDefault<[EnumConstraint] TEnum>(string value, bool ignoreCase, TEnum defaultEnum, params EnumFormat[] parseFormatOrder)
            where TEnum : struct
        {
            TEnum result;
            return TryParse(value, ignoreCase, out result, parseFormatOrder) ? result : defaultEnum;
        }

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, out TEnum result)
            where TEnum : struct => TryParse(value, false, out result, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, out TEnum result, params EnumFormat[] parseFormatOrder)
            where TEnum : struct => TryParse(value, false, out result, parseFormatOrder);

        /// <summary>
        /// Tries to convert the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, bool ignoreCase, out TEnum result)
            where TEnum : struct => TryParse(value, ignoreCase, out result, null);

        /// <summary>
        /// Tries to convert the string representation of an enumerated constant using the given <paramref name="parseFormatOrder"/>.
        /// The return value indicates whether the conversion succeeded. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="result"></param>
        /// <param name="parseFormatOrder"></param>
        /// <returns></returns>
        [Pure]
        public static bool TryParse<[EnumConstraint] TEnum>(string value, bool ignoreCase, out TEnum result, params EnumFormat[] parseFormatOrder)
            where TEnum : struct
        {
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
        internal static void InitializeCache(Type enumType, Func<EnumFormat, Func<IEnumMemberInfo, string>> getCustomEnumFormatter, out object cache, out Delegate toEnum, out Delegate toInt)
        {
            var underlyingType = Enum.GetUnderlyingType(enumType);
            var typeCode = Type.GetTypeCode(underlyingType);

#if NET20 || USE_EMIT
            var toIntMethod = new DynamicMethod(enumType.Name + "_ToInt",
                                       underlyingType,
                                       new[] { enumType },
                                       enumType, true);
            var toIntGenerator = toIntMethod.GetILGenerator();
            toIntGenerator.DeclareLocal(underlyingType);
            toIntGenerator.Emit(OpCodes.Ldarg_0);
            toIntGenerator.Emit(OpCodes.Stloc_0);
            toIntGenerator.Emit(OpCodes.Ldloc_0);
            toIntGenerator.Emit(OpCodes.Ret);
            toInt = toIntMethod.CreateDelegate(typeof(Func<,>).MakeGenericType(enumType, underlyingType));

            var toEnumMethod = new DynamicMethod(underlyingType.Name + "_ToEnum",
                                       enumType,
                                       new[] { underlyingType },
                                       underlyingType, true);
            var toEnumGenerator = toEnumMethod.GetILGenerator();
            toEnumGenerator.DeclareLocal(enumType);
            toEnumGenerator.Emit(OpCodes.Ldarg_0);
            toEnumGenerator.Emit(OpCodes.Stloc_0);
            toEnumGenerator.Emit(OpCodes.Ldloc_0);
            toEnumGenerator.Emit(OpCodes.Ret);
            toEnum = toEnumMethod.CreateDelegate(typeof(Func<,>).MakeGenericType(underlyingType, enumType));
#else
            var enumParam = Expression.Parameter(enumType, "x");
            var enumParamConvert = Expression.Convert(enumParam, underlyingType);
            toInt = Expression.Lambda(enumParamConvert, enumParam).Compile();
            var intParam = Expression.Parameter(underlyingType, "y");
            var intParamConvert = Expression.Convert(intParam, enumType);
            toEnum = Expression.Lambda(intParamConvert, intParam).Compile();
#endif

            switch (typeCode)
            {
                case TypeCode.Int32:
                    cache = new EnumsCache<int>(enumType, getCustomEnumFormatter);
                    break;
                case TypeCode.UInt32:
                    cache = new EnumsCache<uint>(enumType, getCustomEnumFormatter);
                    break;
                case TypeCode.Int64:
                    cache = new EnumsCache<long>(enumType, getCustomEnumFormatter);
                    break;
                case TypeCode.UInt64:
                    cache = new EnumsCache<ulong>(enumType, getCustomEnumFormatter);
                    break;
                case TypeCode.SByte:
                    cache = new EnumsCache<sbyte>(enumType, getCustomEnumFormatter);
                    break;
                case TypeCode.Byte:
                    cache = new EnumsCache<byte>(enumType, getCustomEnumFormatter);
                    break;
                case TypeCode.Int16:
                    cache = new EnumsCache<short>(enumType, getCustomEnumFormatter);
                    break;
                case TypeCode.UInt16:
                    cache = new EnumsCache<ushort>(enumType, getCustomEnumFormatter);
                    break;
                default:
                    Debug.Fail("Unknown Enum TypeCode");
                    cache = null;
                    break;
            }
        }

        internal static EnumFormat InternalRegisterCustomEnumFormat<TEnum>(Func<EnumMemberInfo<TEnum>, string> formatter)
        {
            Preconditions.NotNull(formatter, nameof(formatter));

            var index = Interlocked.Increment(ref Enums<TEnum>.LastCustomEnumFormatIndex);
            if (index == 0)
            {
                Enums<TEnum>.CustomEnumFormatters = new List<Func<EnumMemberInfo<TEnum>, string>>();
            }
            else
            {
                while (Enums<TEnum>.CustomEnumFormatters?.Count != index)
                {
                }
            }
            Enums<TEnum>.CustomEnumFormatters.Add(formatter);
            return (EnumFormat)(index + _startingGenericCustomEnumFormatValue);
        }

        internal static string DescriptionEnumFormatter(IEnumMemberInfo info) => info.Description;

        internal static string GetDescription(Attribute[] attributes)
        {
            return attributes.Length > 0 ? (attributes[0] as DescriptionAttribute)?.Description : null;
        }

        internal static TAttribute GetAttribute<TAttribute>(Attribute[] attributes)
            where TAttribute : Attribute
        {
            foreach (var attribute in attributes)
            {
                var castedAttr = attribute as TAttribute;
                if (castedAttr != null)
                {
                    return castedAttr;
                }
            }
            return null;
        }

        internal static IEnumerable<TAttribute> GetAttributes<TAttribute>(Attribute[] attributes)
            where TAttribute : Attribute
        {
            foreach (var attribute in attributes)
            {
                var castedAttr = attribute as TAttribute;
                if (castedAttr != null)
                {
                    yield return castedAttr;
                }
            }
        }

        internal static Func<IEnumMemberInfo, string> InternalGetCustomEnumFormatter<TEnum>(EnumFormat format)
        {
            var formatter = GetCustomEnumFormatter(format) ?? GetGenericCustomEnumFormatter<TEnum>(format);
            return formatter != null ? info => formatter(new EnumMemberInfo<TEnum>(info)) : (Func<IEnumMemberInfo, string>)null;
        }

        private static Func<EnumMemberInfo, string> GetCustomEnumFormatter(EnumFormat format)
        {
            var index = (int)format - _startingCustomEnumFormatValue;
            if (index >= 0 && index < _customEnumFormatters?.Count)
            {
                return _customEnumFormatters[index];
            }
            return null;
        }

        private static Func<EnumMemberInfo<TEnum>, string> GetGenericCustomEnumFormatter<TEnum>(EnumFormat format)
        {
            var index = (int)format - _startingGenericCustomEnumFormatValue;
            if (index >= 0 && index < Enums<TEnum>.CustomEnumFormatters?.Count)
            {
                return Enums<TEnum>.CustomEnumFormatters[index];
            }
            return null;
        }

        internal static bool IsNumeric(string value)
        {
            var firstChar = value[0];
            return char.IsDigit(firstChar) || firstChar == '-' || firstChar == '+';
        }

        internal static OverflowException GetOverflowException() => new OverflowException("value is outside the underlying type's value range");
        #endregion
    }

    internal static class Enums<TEnum>
    {
        internal static readonly TypeCode TypeCode;

        internal static int LastCustomEnumFormatIndex = -1;

        internal static List<Func<EnumMemberInfo<TEnum>, string>> CustomEnumFormatters;

        static Enums()
        {
            Debug.Assert(typeof(TEnum).IsEnum);
            TypeCode = Type.GetTypeCode(Enum.GetUnderlyingType(typeof(TEnum)));
        }
    }

    internal static class Enums<TEnum, TInt>
    {
        internal static readonly EnumsCache<TInt> Cache;

        internal static readonly Func<TInt, TEnum> ToEnum;

        internal static readonly Func<TEnum, TInt> ToInt;

        static Enums()
        {
            Debug.Assert(typeof(TEnum).IsEnum);
            object cache;
            Delegate toEnum;
            Delegate toInt;
            Enums.InitializeCache(typeof(TEnum), Enums.InternalGetCustomEnumFormatter<TEnum>, out cache, out toEnum, out toInt);
            Cache = (EnumsCache<TInt>)cache;
            ToEnum = (Func<TInt, TEnum>)toEnum;
            ToInt = (Func<TEnum, TInt>)toInt;
        }
    }
}