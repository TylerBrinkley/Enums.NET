#region License
// Copyright (c) 2016 Tyler Brinkley
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using EnumsNET.Numerics;
using EnumsNET.Utilities;

namespace EnumsNET
{
    /// <summary>
    /// An enum member which is composed of its name, value, and attributes.
    /// </summary>
    public abstract class EnumMember : IComparable<EnumMember>, IEquatable<EnumMember>, IComparable, IFormattable
#if ICONVERTIBLE
        , IConvertible
#endif
    {
        internal readonly EnumMemberInternal Member;

        /// <summary>
        /// The enum member's value.
        /// </summary>
        public object Value => GetValue();

        /// <summary>
        /// The enum member's name.
        /// </summary>
        public string Name => Member.Name;

        /// <summary>
        /// The enum member's attributes.
        /// </summary>
        public AttributeCollection Attributes => Member.Attributes;

        internal EnumMember(EnumMemberInternal member)
        {
            Member = member;
        }

        /// <summary>
        /// Retrieves the enum member's name.
        /// </summary>
        /// <returns>The enum member's name.</returns>
        public sealed override string ToString() => Member.Name;

        /// <summary>
        /// Retrieves the enum member's name.
        /// </summary>
        /// <returns>The enum member's name.</returns>
        public string AsString() => Member.Name;

        /// <summary>
        /// Converts the enum member to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of the enum member.</returns>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        public string AsString(string format) => Member.AsString(format);

        /// <summary>
        /// Converts the enum member to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of the enum member.</returns>
        /// <exception cref="ArgumentException"><paramref name="format"/> is an invalid value.</exception>
        public string AsString(EnumFormat format) => Member.AsString(format);

        /// <summary>
        /// Converts the enum member to its string representation using the specified formats.
        /// </summary>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use if using the first resolves to <c>null</c>.</param>
        /// <returns>A string representation of the enum member.</returns>
        /// <exception cref="ArgumentException"><paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public string AsString(EnumFormat format0, EnumFormat format1) => Member.AsString(new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Converts the enum member to its string representation using the specified formats.
        /// </summary>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use if using the first resolves to <c>null</c>.</param>
        /// <param name="format2">The third output format to use if using the first and second both resolve to <c>null</c>.</param>
        /// <returns>A string representation of the enum member.</returns>
        /// <exception cref="ArgumentException"><paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public string AsString(EnumFormat format0, EnumFormat format1, EnumFormat format2) => Member.AsString(new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Converts the enum member to its string representation using the specified <paramref name="formats"/>.
        /// </summary>
        /// <param name="formats">The output formats to use.</param>
        /// <returns>A string representation of the enum member.</returns>
        /// <exception cref="ArgumentException"><paramref name="formats"/> contains an invalid value.</exception>
        public string AsString(params EnumFormat[] formats) => Member.AsString(new ValueCollection<EnumFormat>(formats));

        /// <summary>
        /// Converts the enum member to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of the enum member.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        public string Format(string format) => Member.Format(format);

        /// <summary>
        /// Converts the enum member to its string representation using the specified <paramref name="formats"/>.
        /// </summary>
        /// <param name="formats">The output formats to use.</param>
        /// <returns>A string representation of the enum member.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="formats"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="formats"/> contains an invalid value.</exception>
        public string Format(params EnumFormat[] formats)
        {
            Preconditions.NotNull(formats, nameof(formats));

            return AsString(formats);
        }

        /// <summary>
        /// Indicates if <see cref="Attributes"/> contains a <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type.</typeparam>
        /// <returns>Indication if <see cref="Attributes"/> contains a <typeparamref name="TAttribute"/>.</returns>
        [Obsolete("Use Attributes.Has instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool HasAttribute<TAttribute>()
            where TAttribute : Attribute => Attributes.Has<TAttribute>();

        /// <summary>
        /// Retrieves the first <typeparamref name="TAttribute"/> in <see cref="Attributes"/> if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type.</typeparam>
        /// <returns>The first <typeparamref name="TAttribute"/> in <see cref="Attributes"/> if defined otherwise <c>null</c>.</returns>
        [Obsolete("Use Attributes.Get instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TAttribute GetAttribute<TAttribute>()
            where TAttribute : Attribute => Attributes.Get<TAttribute>();

        /// <summary>
        /// Retrieves all <typeparamref name="TAttribute"/>'s in <see cref="Attributes"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type.</typeparam>
        /// <returns>All <typeparamref name="TAttribute"/>'s in <see cref="Attributes"/>.</returns>
        [Obsolete("Use Attributes.GetAll instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable<TAttribute> GetAttributes<TAttribute>()
            where TAttribute : Attribute => Attributes.GetAll<TAttribute>();

        /// <summary>
        /// Retrieves the enum member's underlying integral value.
        /// </summary>
        /// <returns>The enum member's underlying integral value.</returns>
        public object GetUnderlyingValue() => Member.GetUnderlyingValue();

        /// <summary>
        /// Converts <see cref="Value"/> to an <see cref="sbyte"/>.
        /// </summary>
        /// <returns><see cref="Value"/> converted to an <see cref="sbyte"/>.</returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="sbyte"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public sbyte ToSByte() => Member.ToSByte();

        /// <summary>
        /// Converts <see cref="Value"/> to a <see cref="byte"/>.
        /// </summary>
        /// <returns><see cref="Value"/> converted to a <see cref="byte"/>.</returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="byte"/>'s value range without overflowing.</exception>
        public byte ToByte() => Member.ToByte();

        /// <summary>
        /// Converts <see cref="Value"/> to an <see cref="short"/>.
        /// </summary>
        /// <returns><see cref="Value"/> converted to an <see cref="short"/>.</returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="short"/>'s value range without overflowing.</exception>
        public short ToInt16() => Member.ToInt16();

        /// <summary>
        /// Converts <see cref="Value"/> to a <see cref="ushort"/>.
        /// </summary>
        /// <returns><see cref="Value"/> converted to a <see cref="ushort"/>.</returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="ushort"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public ushort ToUInt16() => Member.ToUInt16();

        /// <summary>
        /// Converts <see cref="Value"/> to an <see cref="int"/>.
        /// </summary>
        /// <returns><see cref="Value"/> converted to an <see cref="int"/>.</returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="int"/>'s value range without overflowing.</exception>
        public int ToInt32() => Member.ToInt32();

        /// <summary>
        /// Converts <see cref="Value"/> to a <see cref="uint"/>.
        /// </summary>
        /// <returns><see cref="Value"/> converted to a <see cref="uint"/>.</returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="uint"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public uint ToUInt32() => Member.ToUInt32();

        /// <summary>
        /// Converts <see cref="Value"/> to an <see cref="long"/>.
        /// </summary>
        /// <returns><see cref="Value"/> converted to an <see cref="long"/>.</returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="long"/>'s value range without overflowing.</exception>
        public long ToInt64() => Member.ToInt64();

        /// <summary>
        /// Converts <see cref="Value"/> to a <see cref="ulong"/>.
        /// </summary>
        /// <returns><see cref="Value"/> converted to a <see cref="ulong"/>.</returns>
        /// <exception cref="OverflowException"><see cref="Value"/> cannot fit within <see cref="ulong"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public ulong ToUInt64() => Member.ToUInt64();

        /// <summary>
        /// Retrieves the hash code of <see cref="Value"/>.
        /// </summary>
        /// <returns>The hash code of <see cref="Value"/>.</returns>
        public sealed override int GetHashCode() => Member.GetHashCode();

        /// <summary>
        /// Indicates whether the specified <see cref="EnumMember"/> is equal to the current <see cref="EnumMember"/>.
        /// </summary>
        /// <param name="other">The other <see cref="EnumMember"/>.</param>
        /// <returns>Indication whether the specified <see cref="EnumMember"/> is equal to the current <see cref="EnumMember"/>.</returns>
        public bool Equals(EnumMember other) => ReferenceEquals(this, other);

        /// <summary>
        /// Indicates whether the specified <see cref="object"/> is equal to the current <see cref="object"/>.
        /// </summary>
        /// <param name="other">The other <see cref="object"/>.</param>
        /// <returns>Indication whether the specified <see cref="object"/> is equal to the current <see cref="object"/>.</returns>
        public sealed override bool Equals(object other) => ReferenceEquals(this, other);

        internal abstract object GetValue();

        internal abstract IEnumerable<object> GetFlags();

        internal abstract IEnumerable<EnumMember> GetFlagMembers();

        internal bool IsValidFlagCombination() => Member.IsValidFlagCombination();

        internal int GetFlagCount() => Member.GetFlagCount();

        internal bool HasAnyFlags() => Member.HasAnyFlags();

        internal bool HasAllFlags() => Member.HasAllFlags();

        #region Explicit Interface Implementation
        string IFormattable.ToString(string format, IFormatProvider formatProvider) => Member.ToString(format, formatProvider);

#if ICONVERTIBLE
        TypeCode IConvertible.GetTypeCode() => Member.GetTypeCode();

        bool IConvertible.ToBoolean(IFormatProvider provider) => Member.ToBoolean(provider);

        char IConvertible.ToChar(IFormatProvider provider) => Member.ToChar(provider);

        sbyte IConvertible.ToSByte(IFormatProvider provider) => Member.ToSByte(provider);

        byte IConvertible.ToByte(IFormatProvider provider) => Member.ToByte(provider);

        short IConvertible.ToInt16(IFormatProvider provider) => Member.ToInt16(provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => Member.ToUInt16(provider);

        int IConvertible.ToInt32(IFormatProvider provider) => Member.ToInt32(provider);

        uint IConvertible.ToUInt32(IFormatProvider provider) => Member.ToUInt32(provider);

        long IConvertible.ToInt64(IFormatProvider provider) => Member.ToInt64(provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider) => Member.ToUInt64(provider);

        float IConvertible.ToSingle(IFormatProvider provider) => Member.ToSingle(provider);

        double IConvertible.ToDouble(IFormatProvider provider) => Member.ToDouble(provider);

        decimal IConvertible.ToDecimal(IFormatProvider provider) => Member.ToDecimal(provider);

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Member.ToDateTime(provider);

        string IConvertible.ToString(IFormatProvider provider) => Member.ToString(provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Member.ToType(conversionType, provider);
#endif

        // implemented in derived class
        int IComparable.CompareTo(object obj) => 0;

        // implemented in derived class
        int IComparable<EnumMember>.CompareTo(EnumMember other) => 0;
        #endregion
    }

    /// <summary>
    /// An enum member which is composed of its name, value, and attributes.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    public abstract class EnumMember<TEnum> : EnumMember, IComparable<EnumMember<TEnum>>, IEquatable<EnumMember<TEnum>>
    {
        /// <summary>
        /// The enum member's value.
        /// </summary>
        public new TEnum Value => GetGenericValue();

        internal EnumMember(EnumMemberInternal member)
            : base(member)
        {
        }

        /// <summary>
        /// Indicates whether the specified <see cref="EnumMember{TEnum}"/> is equal to the current <see cref="EnumMember{TEnum}"/>.
        /// </summary>
        /// <param name="other">The other <see cref="EnumMember{TEnum}"/>.</param>
        /// <returns>Indication whether the specified <see cref="EnumMember{TEnum}"/> is equal to the current <see cref="EnumMember{TEnum}"/>.</returns>
        public bool Equals(EnumMember<TEnum> other) => ReferenceEquals(this, other);

        internal abstract TEnum GetGenericValue();

        internal abstract IEnumerable<TEnum> GetGenericFlags();

        internal abstract IEnumerable<EnumMember<TEnum>> GetGenericFlagMembers();

        internal sealed override object GetValue() => GetGenericValue();

        internal sealed override IEnumerable<object> GetFlags() => GetGenericFlags().Select(flag => (object)flag);

        internal sealed override IEnumerable<EnumMember> GetFlagMembers() => GetGenericFlagMembers()
#if !COVARIANCE
            .Select(flag => (EnumMember)flag)
#endif
            ;

        #region Explicit Interface Implementation
        // Implemented in derived class
        int IComparable<EnumMember<TEnum>>.CompareTo(EnumMember<TEnum> other) => 0;
        #endregion
    }

    internal sealed class EnumMember<TEnum, TUnderlying, TUnderlyingOperations> : EnumMember<TEnum>, IComparable<EnumMember<TEnum>>, IComparable<EnumMember>, IComparable
        where TEnum : struct, Enum
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        internal new EnumMemberInternal<TUnderlying, TUnderlyingOperations> Member => UnsafeUtility.As<EnumMemberInternal<TUnderlying, TUnderlyingOperations>>(base.Member);

        internal EnumMember(EnumMemberInternal<TUnderlying, TUnderlyingOperations> member)
            : base(member)
        {
        }

        internal override TEnum GetGenericValue()
        {
            var underlying = Member.Value;
            return UnsafeUtility.As<TUnderlying, TEnum>(ref underlying);
        }

        internal override IEnumerable<TEnum> GetGenericFlags() => Member.GetFlags().Select(flag => UnsafeUtility.As<TUnderlying, TEnum>(ref flag));

        internal override IEnumerable<EnumMember<TEnum>> GetGenericFlagMembers() => Member.GetFlagMembers().Select(m => UnsafeUtility.As<EnumMember<TEnum>>(m));

        #region Interface Implementation
        public int CompareTo(object other) => CompareTo(other as EnumMember<TEnum>);

        public int CompareTo(EnumMember other) => CompareTo(other as EnumMember<TEnum>);

        public int CompareTo(EnumMember<TEnum> other) => other != null ? Member.CompareTo(UnsafeUtility.As<EnumMember<TEnum, TUnderlying, TUnderlyingOperations>>(other).Member) : 1;
        #endregion
    }
}