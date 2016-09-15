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
using System.Collections.ObjectModel;
using EnumsNET.Numerics;

namespace EnumsNET
{
    internal sealed class InternalEnumMember<TInt, TIntProvider> : IEnumMember
        where TInt : struct, IFormattable, IConvertible, IComparable<TInt>, IEquatable<TInt>
        where TIntProvider : struct, INumericProvider<TInt>
    {
        private readonly ReadOnlyCollection<Attribute> _attributes;
        private readonly EnumCache<TInt, TIntProvider> _enumCache;

        public TInt Value { get; }

        public string Name { get; }

        public IEnumerable<Attribute> Attributes => _attributes;

        internal EnumMember EnumMember { get; }

        public InternalEnumMember(TInt value, string name, Attribute[] attributes, EnumCache<TInt, TIntProvider> enumCache)
        {
            Value = value;
            Name = name;
            _attributes = attributes != null ? new ReadOnlyCollection<Attribute>(attributes) : null;
            _enumCache = enumCache;
            EnumMember = enumCache?.EnumInfo.CreateEnumMember(this);
        }

        public bool HasAttribute<TAttribute>()
            where TAttribute : Attribute => GetAttribute<TAttribute>() != null;

        public TAttribute GetAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            foreach (var attribute in _attributes)
            {
                var castedAttr = attribute as TAttribute;
                if (castedAttr != null)
                {
                    return castedAttr;
                }
            }
            return null;
        }

        public IEnumerable<TAttribute> GetAttributes<TAttribute>()
            where TAttribute : Attribute
        {
            var attributes = new List<TAttribute>();
            foreach (var attribute in _attributes)
            {
                var castedAttr = attribute as TAttribute;
                if (castedAttr != null)
                {
                    attributes.Add(castedAttr);
                }
            }
            return attributes;
        }

        public override string ToString() => _enumCache.InternalAsString(Value, this);

        public string ToString(string format) => _enumCache.InternalAsString(Value, this, format);

        public string ToString(params EnumFormat[] formatOrder) => _enumCache.InternalAsString(Value, this, formatOrder);

        public string AsString() => ToString();

        public string AsString(string format) => ToString(format);

        public string AsString(EnumFormat format)
        {
            var isInitialized = true;
            var member = this;
            return _enumCache.InternalFormat(Value, ref isInitialized, ref member, format);
        }

        public string AsString(EnumFormat format0, EnumFormat format1) => _enumCache.InternalFormat(Value, this, format0, format1);

        public string AsString(EnumFormat format0, EnumFormat format1, EnumFormat format2) => _enumCache.InternalFormat(Value, this, format0, format1, format2);

        public string AsString(params EnumFormat[] formatOrder) => ToString(formatOrder);

        public string Format(string format)
        {
            Preconditions.NotNull(format, nameof(format));

            return _enumCache.InternalFormat(Value, this, format);
        }

        public string Format(params EnumFormat[] formatOrder)
        {
            Preconditions.NotNull(formatOrder, nameof(formatOrder));

            return _enumCache.InternalFormat(Value, this, formatOrder);
        }

        public sbyte ToSByte() => Value.ToSByte(null);

        public byte ToByte() => Value.ToByte(null);

        public short ToInt16() => Value.ToInt16(null);

        public ushort ToUInt16() => Value.ToUInt16(null);

        public int ToInt32() => Value.ToInt32(null);

        public uint ToUInt32() => Value.ToUInt32(null);

        public long ToInt64() => Value.ToInt64(null);

        public ulong ToUInt64() => Value.ToUInt64(null);

        public override int GetHashCode() => Value.GetHashCode();

        internal int CompareTo(InternalEnumMember<TInt, TIntProvider> other) => Value.CompareTo(other.Value);

        public bool IsValidFlagCombination() => _enumCache.IsValidFlagCombination(Value);

        public bool HasAnyFlags() => _enumCache.HasAnyFlags(Value);

        public bool HasAllFlags() => _enumCache.HasAllFlags(Value);

        public IEnumerable<TInt> GetFlags() => _enumCache.GetFlags(Value);

        public IEnumerable<InternalEnumMember<TInt, TIntProvider>> GetFlagMembers() => _enumCache.GetFlagMembers(Value);

        #region Explicit Interface Implementation
        string IFormattable.ToString(string format, IFormatProvider formatProvider) => ToString(format);

        TypeCode IConvertible.GetTypeCode() => Value.GetTypeCode();

        bool IConvertible.ToBoolean(IFormatProvider provider) => Value.ToBoolean(provider);

        char IConvertible.ToChar(IFormatProvider provider) => Value.ToChar(provider);

        sbyte IConvertible.ToSByte(IFormatProvider provider) => Value.ToSByte(provider);

        byte IConvertible.ToByte(IFormatProvider provider) => Value.ToByte(provider);

        short IConvertible.ToInt16(IFormatProvider provider) => Value.ToInt16(provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => Value.ToUInt16(provider);

        int IConvertible.ToInt32(IFormatProvider provider) => Value.ToInt32(provider);

        uint IConvertible.ToUInt32(IFormatProvider provider) => Value.ToUInt32(provider);

        long IConvertible.ToInt64(IFormatProvider provider) => Value.ToInt64(provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider) => Value.ToUInt64(provider);

        float IConvertible.ToSingle(IFormatProvider provider) => Value.ToSingle(provider);

        double IConvertible.ToDouble(IFormatProvider provider) => Value.ToDouble(provider);

        decimal IConvertible.ToDecimal(IFormatProvider provider) => Value.ToDecimal(provider);

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Value.ToDateTime(provider);

        string IConvertible.ToString(IFormatProvider provider) => ToString();

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Value.ToType(conversionType, provider);

        object IEnumMember.GetUnderlyingValue() => Value;
        #endregion
    }
}
