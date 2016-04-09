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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EnumsNET.Numerics;

namespace EnumsNET
{
    internal struct InternalEnumMemberInfo<TInt, TIntProvider> : IEnumMemberInfo
        where TInt : struct, IFormattable, IConvertible, IComparable<TInt>, IEquatable<TInt>
        where TIntProvider : struct, INumericProvider<TInt>
    {
        private readonly Attribute[] _attributes;
        private readonly EnumCache<TInt, TIntProvider> _enumCache;

        public TInt Value { get; }

        public string Name { get; }

        public IEnumerable<Attribute> Attributes => IsDefined ? new ReadOnlyCollection<Attribute>(_attributes ?? Enums.EmptyAttributes) : null;

        public string Description => _attributes != null ? Enums.GetDescription(_attributes) : null;

        public bool IsDefined => Name != null;

        public InternalEnumMemberInfo(TInt value, string name, Attribute[] attributes, EnumCache<TInt, TIntProvider> enumCache)
        {
            Value = value;
            Name = name;
            _attributes = attributes;
            _enumCache = enumCache;
        }

        public bool HasAttribute<TAttribute>()
            where TAttribute : Attribute => GetAttribute<TAttribute>() != null;

        public TAttribute GetAttribute<TAttribute>()
            where TAttribute : Attribute => _attributes != null ? Enums.GetAttribute<TAttribute>(_attributes) : null;

        public IEnumerable<TAttribute> GetAttributes<TAttribute>()
            where TAttribute : Attribute => IsDefined ? (_attributes != null ? Enums.GetAttributes<TAttribute>(_attributes) : new TAttribute[0]) : null;

        public TResult GetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, TResult defaultValue)
            where TAttribute : Attribute
        {
            TResult result;
            if (!TryGetAttributeSelect(selector, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        public bool TryGetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, out TResult result)
            where TAttribute : Attribute
        {
            Preconditions.NotNull(selector, nameof(selector));

            var attr = GetAttribute<TAttribute>();
            if (attr != null)
            {
                result = selector(attr);
                return true;
            }
            result = default(TResult);
            return false;
        }

        public override string ToString() => _enumCache.InternalAsString(this);

        public string ToString(string format) => _enumCache.InternalAsString(this, format);

        public string ToString(params EnumFormat[] formats) => _enumCache.InternalAsString(this, formats);

        public string AsString() => ToString();

        public string AsString(string format) => ToString(format);

        public string AsString(params EnumFormat[] formats) => ToString(formats);

        public string Format(string format)
        {
            Preconditions.NotNull(format, nameof(format));

            return _enumCache.InternalFormat(this, format);
        }

        public string Format(EnumFormat format) => _enumCache.InternalFormat(this, format);

        public string Format(EnumFormat format0, EnumFormat format1) => _enumCache.InternalFormat(this, format0, format1);

        public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2) => _enumCache.InternalFormat(this, format0, format1, format2);

        public string Format(params EnumFormat[] formats)
        {
            Preconditions.NotNull(formats, nameof(formats));

            return _enumCache.InternalFormat(this, formats);
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

        internal int CompareTo(InternalEnumMemberInfo<TInt, TIntProvider> obj) => Value.CompareTo(obj.Value);

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

        int IComparable.CompareTo(object obj) => obj is InternalEnumMemberInfo<TInt, TIntProvider> ? CompareTo((InternalEnumMemberInfo<TInt, TIntProvider>)obj) : 1;

        object IEnumMemberInfo.Value => Value;

        object IEnumMemberInfo.UnderlyingValue => Value;
        #endregion
    }
}
