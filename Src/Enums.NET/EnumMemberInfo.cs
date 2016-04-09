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
using EnumsNET.Numerics;

namespace EnumsNET
{
    /// <summary>
    /// Represents an enum member's Name, Value, and Attributes
    /// </summary>
    public abstract class EnumMemberInfo : IEnumMemberInfo, IComparable<EnumMemberInfo>, IEquatable<EnumMemberInfo>
    {
        internal readonly IEnumMemberInfo _info;

        /// <summary>
        /// The defined enum member's value
        /// </summary>
        public object Value => GetValue();

        /// <summary>
        /// The defined enum member's name
        /// </summary>
        public string Name => _info.Name;

        /// <summary>
        /// The defined enum member's attributes
        /// </summary>
        public IEnumerable<Attribute> Attributes => _info.Attributes;

        /// <summary>
        /// The defined enum member's <see cref="DescriptionAttribute.Description"/> if applied else null.
        /// </summary>
        public string Description => _info.Description;

        /// <summary>
        /// The defined enum member's underlying integer value
        /// </summary>
        public object UnderlyingValue => _info.UnderlyingValue;

        /// <summary>
        /// Specifies if the given EnumMemberInfo is defined, the only time it may not be defined is within
        /// a custom enum formatter
        /// </summary>
        public bool IsDefined => _info.IsDefined;

        internal EnumMemberInfo(IEnumMemberInfo info)
        {
            _info = info;
        }

        /// <summary>
        /// Converts <see cref="Value"/> to its equivalent string representation.
        /// </summary>
        /// <returns></returns>
        public sealed override string ToString() => _info.ToString();

        /// <summary>
        /// Converts <see cref="Value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value</exception>
        public string ToString(string format) => _info.ToString(format);

        /// <summary>
        /// Converts <see cref="Value"/> to its equivalent string representation according to the specified <paramref name="formats"/>.
        /// </summary>
        /// <param name="formats"></param>
        /// <returns></returns>
        public string ToString(params EnumFormat[] formats) => _info.ToString(formats);

        /// <summary>
        /// Converts the specified <see cref="Value"/> to its equivalent string representation.
        /// </summary>
        /// <returns></returns>
        public string AsString() => _info.AsString();

        /// <summary>
        /// Converts <see cref="Value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value</exception>
        public string AsString(string format) => _info.AsString(format);

        /// <summary>
        /// Converts <see cref="Value"/> to its equivalent string representation according to the specified <paramref name="formats"/>.
        /// </summary>
        /// <param name="formats"></param>
        /// <returns></returns>
        public string AsString(params EnumFormat[] formats) => _info.AsString(formats);

        /// <summary>
        /// Converts <see cref="Value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        public string Format(string format) => _info.Format(format);

        /// <summary>
        /// Converts <see cref="Value"/> to its equivalent string representation according to the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string Format(EnumFormat format) => _info.Format(format);

        /// <summary>
        /// Converts <see cref="Value"/> to its equivalent string representation in the specified format order.
        /// </summary>
        /// <param name="format0"></param>
        /// <param name="format1"></param>
        /// <returns></returns>
        public string Format(EnumFormat format0, EnumFormat format1) => _info.Format(format0, format1);

        /// <summary>
        /// Converts <see cref="Value"/> to its equivalent string representation in the specified format order.
        /// </summary>
        /// <param name="format0"></param>
        /// <param name="format1"></param>
        /// <param name="format2"></param>
        /// <returns></returns>
        public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2) => _info.Format(format0, format1, format2);

        /// <summary>
        /// Converts <see cref="Value"/> to its equivalent string representation according to the specified <paramref name="formats"/>.
        /// </summary>
        /// <param name="formats"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="formats"/> is null.</exception>
        public string Format(params EnumFormat[] formats) => _info.Format(formats);

        /// <summary>
        /// Indicates if <see cref="Attributes"/> contains a <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns>Indication if <see cref="Attributes"/> contains a <typeparamref name="TAttribute"/>.</returns>
        public bool HasAttribute<TAttribute>() where TAttribute : Attribute => _info.HasAttribute<TAttribute>();

        /// <summary>
        /// Retrieves the first <typeparamref name="TAttribute"/> in <see cref="Attributes"/> if defined else null.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute => _info.GetAttribute<TAttribute>();

        /// <summary>
        /// Retrieves the first <typeparamref name="TAttribute"/> in <see cref="Attributes"/> if defined and returns a <typeparamref name="TResult"/>
        /// using the <paramref name="selector"/> else returns <paramref name="defaultValue"/>.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is null.</exception>
        public TResult GetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, TResult defaultValue = default(TResult)) where TAttribute : Attribute => _info.GetAttributeSelect(selector, defaultValue);

        /// <summary>
        /// Tries to retrieve the first <typeparamref name="TAttribute"/> in <see cref="Attributes"/> if defined and sets <paramref name="result"/>
        /// to the result of applying the <paramref name="selector"/> to the <typeparamref name="TAttribute"/>.
        /// Returns true if a <typeparamref name="TAttribute"/> is found else false.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> is null.</exception>
        public bool TryGetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, out TResult result) where TAttribute : Attribute => _info.TryGetAttributeSelect(selector, out result);

        /// <summary>
        /// Retrieves all <typeparamref name="TAttribute"/>'s in <see cref="Attributes"/>.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public IEnumerable<TAttribute> GetAttributes<TAttribute>() where TAttribute : Attribute => _info.GetAttributes<TAttribute>();

        /// <summary>
        /// Converts <see cref="Value"/> to an <see cref="sbyte"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="sbyte"/>'s value range without overflowing</exception>
        [CLSCompliant(false)]
        public sbyte ToSByte() => _info.ToSByte();

        /// <summary>
        /// Converts <see cref="Value"/> to a <see cref="byte"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="byte"/>'s value range without overflowing</exception>
        public byte ToByte() => _info.ToByte();

        /// <summary>
        /// Converts <see cref="Value"/> to an <see cref="short"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="short"/>'s value range without overflowing</exception>
        public short ToInt16() => _info.ToInt16();

        /// <summary>
        /// Converts <see cref="Value"/> to a <see cref="ushort"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="ushort"/>'s value range without overflowing</exception>
        [CLSCompliant(false)]
        public ushort ToUInt16() => _info.ToUInt16();

        /// <summary>
        /// Converts <see cref="Value"/> to an <see cref="int"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="int"/>'s value range without overflowing</exception>
        public int ToInt32() => _info.ToInt32();

        /// <summary>
        /// Converts <see cref="Value"/> to a <see cref="uint"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="uint"/>'s value range without overflowing</exception>
        [CLSCompliant(false)]
        public uint ToUInt32() => _info.ToUInt32();

        /// <summary>
        /// Converts <see cref="Value"/> to an <see cref="long"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="long"/>'s value range without overflowing</exception>
        public long ToInt64() => _info.ToInt64();

        /// <summary>
        /// Converts <see cref="Value"/> to a <see cref="ulong"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="ulong"/>'s value range without overflowing</exception>
        [CLSCompliant(false)]
        public ulong ToUInt64() => _info.ToUInt64();

        /// <summary>
        /// A more efficient GetHashCode method as it doesn't require boxing and unboxing of the value
        /// </summary>
        /// <returns></returns>
        public sealed override int GetHashCode() => _info.GetHashCode();

        /// <summary>
        /// Determines whether the specified <see cref="EnumMemberInfo"/> is equal to the current <see cref="EnumMemberInfo"/>.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Equals(EnumMemberInfo info) => EqualsMethod(info);

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="object"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public sealed override bool Equals(object obj) => EqualsMethod(obj as EnumMemberInfo);

        internal abstract object GetValue();

        internal abstract bool EqualsMethod(EnumMemberInfo info);

        #region Explicit Interface Implementation
        string IFormattable.ToString(string format, IFormatProvider formatProvider) => _info.ToString(format, formatProvider);

        TypeCode IConvertible.GetTypeCode() => _info.GetTypeCode();

        bool IConvertible.ToBoolean(IFormatProvider provider) => _info.ToBoolean(provider);

        char IConvertible.ToChar(IFormatProvider provider) => _info.ToChar(provider);

        sbyte IConvertible.ToSByte(IFormatProvider provider) => _info.ToSByte(provider);

        byte IConvertible.ToByte(IFormatProvider provider) => _info.ToByte(provider);

        short IConvertible.ToInt16(IFormatProvider provider) => _info.ToInt16(provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => _info.ToUInt16(provider);

        int IConvertible.ToInt32(IFormatProvider provider) => _info.ToInt32(provider);

        uint IConvertible.ToUInt32(IFormatProvider provider) => _info.ToUInt32(provider);

        long IConvertible.ToInt64(IFormatProvider provider) => _info.ToInt64(provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider) => _info.ToUInt64(provider);

        float IConvertible.ToSingle(IFormatProvider provider) => _info.ToSingle(provider);

        double IConvertible.ToDouble(IFormatProvider provider) => _info.ToDouble(provider);

        decimal IConvertible.ToDecimal(IFormatProvider provider) => _info.ToDecimal(provider);

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => _info.ToDateTime(provider);

        string IConvertible.ToString(IFormatProvider provider) => _info.ToString(provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => _info.ToType(conversionType, provider);

        int IComparable.CompareTo(object obj) => _info.CompareTo((obj as EnumMemberInfo)?._info);

        // implemented in derived class
        int IComparable<EnumMemberInfo>.CompareTo(EnumMemberInfo other) => 0;
        #endregion
    }

    /// <summary>
    /// Represents a <typeparamref name="TEnum"/> member's Name, Value, and Attributes
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public abstract class EnumMemberInfo<TEnum> : EnumMemberInfo, IComparable<EnumMemberInfo<TEnum>>, IEquatable<EnumMemberInfo<TEnum>>
    {
        /// <summary>
        /// The defined enum member's value
        /// </summary>
        public new TEnum Value => GetGenericValue();

        internal EnumMemberInfo(IEnumMemberInfo info)
            : base(info)
        {
        }

        /// <summary>
        /// Determines whether the specified <see cref="EnumMemberInfo{TEnum}"/> is equal to the current <see cref="EnumMemberInfo{TEnum}"/>
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Equals(EnumMemberInfo<TEnum> info) => GenericEqualsMethod(info);

        internal abstract TEnum GetGenericValue();

        internal abstract bool GenericEqualsMethod(EnumMemberInfo<TEnum> info);

        internal sealed override object GetValue() => GetGenericValue();

        internal sealed override bool EqualsMethod(EnumMemberInfo info) => GenericEqualsMethod(info as EnumMemberInfo<TEnum>);

        #region Explicit Interface Implementation
        // Implemented in derived class
        int IComparable<EnumMemberInfo<TEnum>>.CompareTo(EnumMemberInfo<TEnum> other) => 0;
        #endregion
    }

    internal sealed class EnumMemberInfo<TEnum, TInt, TIntProvider> : EnumMemberInfo<TEnum>, IComparable<EnumMemberInfo<TEnum>>, IComparable<EnumMemberInfo>
        where TInt : struct, IFormattable, IConvertible, IComparable<TInt>, IEquatable<TInt>
        where TIntProvider : struct, INumericProvider<TInt>
    {
        private new InternalEnumMemberInfo<TInt, TIntProvider> _info;

        internal EnumMemberInfo(InternalEnumMemberInfo<TInt, TIntProvider> info)
            : base(info)
        {
            _info = info;
        }

        internal override bool GenericEqualsMethod(EnumMemberInfo<TEnum> info) => info != null && _info.Value.Equals(((EnumMemberInfo<TEnum, TInt, TIntProvider>)info)._info.Value) && Name == info.Name;

        internal override TEnum GetGenericValue() => EnumInfo<TEnum, TInt, TIntProvider>.ToEnum(_info.Value);

        #region Explicit Interface Implementation
        int IComparable<EnumMemberInfo>.CompareTo(EnumMemberInfo other)
        {
            var otherInfo = other as EnumMemberInfo<TEnum, TInt, TIntProvider>;
            return otherInfo != null ? _info.CompareTo(otherInfo._info) : 1;
        }

        int IComparable<EnumMemberInfo<TEnum>>.CompareTo(EnumMemberInfo<TEnum> other) => other != null ? _info.CompareTo(((EnumMemberInfo<TEnum, TInt, TIntProvider>)other)._info) : 1;
        #endregion
    }
}