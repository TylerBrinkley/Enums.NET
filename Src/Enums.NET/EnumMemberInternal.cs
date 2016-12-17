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
using System.Threading;
using EnumsNET.Numerics;

namespace EnumsNET
{
    // Putting the logic here as opposed to directly in EnumMember<TEnum, TInt, TIntProvider>
    // reduces memory usage because it doesn't have the enum type as a generic type parameter.
    internal sealed class EnumMemberInternal<TInt, TIntProvider> : IEnumMember
        where TInt : struct, IFormattable, IComparable<TInt>, IEquatable<TInt>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TIntProvider : struct, INumericProvider<TInt>
    {
        private readonly EnumCache<TInt, TIntProvider> _enumCache;
        private EnumMember _enumMember;

        public TInt Value { get; }

        public string Name { get; }

        public AttributeCollection Attributes { get; }

        internal EnumMember EnumMember
        {
            get
            {
                EnumMember enumMember;
                return _enumMember ?? Interlocked.CompareExchange(ref _enumMember, (enumMember = _enumCache.EnumInfo.CreateEnumMember(this)), null) ?? enumMember;
            }
        }

        public EnumMemberInternal(TInt value, string name, Attribute[] attributes, EnumCache<TInt, TIntProvider> enumCache)
        {
            Value = value;
            Name = name;
            Attributes = new AttributeCollection(attributes);
            _enumCache = enumCache;
        }

        public string AsString(string format) => _enumCache.AsStringInternal(Value, this, format);

        public string AsString(EnumFormat format)
        {
            var isInitialized = true;
            var member = this;
            return _enumCache.FormatInternal(Value, ref isInitialized, ref member, format);
        }

        public string AsString(EnumFormat format0, EnumFormat format1) => _enumCache.FormatInternal(Value, this, format0, format1);

        public string AsString(EnumFormat format0, EnumFormat format1, EnumFormat format2) => _enumCache.FormatInternal(Value, this, format0, format1, format2);

        public string AsString(params EnumFormat[] formats) => _enumCache.FormatInternal(Value, this, formats);

        public string Format(string format)
        {
            Preconditions.NotNull(format, nameof(format));

            return _enumCache.FormatInternal(Value, this, format);
        }

        public string Format(params EnumFormat[] formats)
        {
            Preconditions.NotNull(formats, nameof(formats));

            return _enumCache.FormatInternal(Value, this, formats);
        }

#if ICONVERTIBLE
        public sbyte ToSByte() => Value.ToSByte(null);

        public byte ToByte() => Value.ToByte(null);

        public short ToInt16() => Value.ToInt16(null);

        public ushort ToUInt16() => Value.ToUInt16(null);

        public int ToInt32() => Value.ToInt32(null);

        public uint ToUInt32() => Value.ToUInt32(null);

        public long ToInt64() => Value.ToInt64(null);

        public ulong ToUInt64() => Value.ToUInt64(null);
#else
        public sbyte ToSByte() => EnumCache<TInt, TIntProvider>.Provider.ToSByte(Value);

        public byte ToByte() => EnumCache<TInt, TIntProvider>.Provider.ToByte(Value);

        public short ToInt16() => EnumCache<TInt, TIntProvider>.Provider.ToInt16(Value);

        public ushort ToUInt16() => EnumCache<TInt, TIntProvider>.Provider.ToUInt16(Value);

        public int ToInt32() => EnumCache<TInt, TIntProvider>.Provider.ToInt32(Value);

        public uint ToUInt32() => EnumCache<TInt, TIntProvider>.Provider.ToUInt32(Value);

        public long ToInt64() => EnumCache<TInt, TIntProvider>.Provider.ToInt64(Value);

        public ulong ToUInt64() => EnumCache<TInt, TIntProvider>.Provider.ToUInt64(Value);
#endif

        public override int GetHashCode() => Value.GetHashCode();

        internal int CompareTo(EnumMemberInternal<TInt, TIntProvider> other) => Value.CompareTo(other.Value);

        public bool IsValidFlagCombination() => _enumCache.IsValidFlagCombination(Value);

        public bool HasAnyFlags() => _enumCache.HasAnyFlags(Value);

        public bool HasAllFlags() => _enumCache.HasAllFlags(Value);

        public IEnumerable<TInt> GetFlags() => _enumCache.GetFlags(Value);

        public IEnumerable<EnumMemberInternal<TInt, TIntProvider>> GetFlagMembers() => _enumCache.GetFlagMembers(Value);

        #region Explicit Interface Implementation
        string IFormattable.ToString(string format, IFormatProvider formatProvider) => AsString(format);

#if ICONVERTIBLE
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
#endif

        object IEnumMember.GetUnderlyingValue() => Value;
        #endregion
    }
}
