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

namespace EnumsNET
{
    internal struct InternalEnumMemberInfo<TInt> : IEnumMemberInfo, IComparable<InternalEnumMemberInfo<TInt>>
        where TInt : struct
    {
        private readonly Attribute[] _attributes;
        private readonly EnumCache<TInt> _enumsCache;

        public TInt Value { get; }

        public string Name { get; }

        public IEnumerable<Attribute> Attributes => IsDefined ? new ReadOnlyCollection<Attribute>(_attributes ?? Enums.EmptyAttributes) : null;

        public string Description => _attributes != null ? Enums.GetDescription(_attributes) : null;

        public bool IsDefined => Name != null;

        public InternalEnumMemberInfo(TInt value, string name, Attribute[] attributes, EnumCache<TInt> enumsCache)
        {
            Value = value;
            Name = name;
            _attributes = attributes;
            _enumsCache = enumsCache;
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

        public override string ToString() => _enumsCache.InternalAsString(this);

        public string ToString(string format) => _enumsCache.InternalAsString(this, format);

        public string ToString(params EnumFormat[] formats) => _enumsCache.InternalAsString(this, formats);

        public string AsString() => ToString();

        public string AsString(string format) => ToString(format);

        public string AsString(params EnumFormat[] formats) => ToString(formats);

        public string Format(string format)
        {
            Preconditions.NotNull(format, nameof(format));

            return _enumsCache.InternalFormat(this, format);
        }

        public string Format(EnumFormat format) => _enumsCache.InternalFormat(this, format);

        public string Format(EnumFormat format0, EnumFormat format1) => _enumsCache.InternalFormat(this, format0, format1);

        public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2) => _enumsCache.InternalFormat(this, format0, format1, format2);

        public string Format(params EnumFormat[] formats)
        {
            Preconditions.NotNull(formats, nameof(formats));

            return _enumsCache.InternalFormat(this, formats);
        }

        public sbyte ToSByte() => EnumCache<TInt>.ToSByte(Value);

        public byte ToByte() => EnumCache<TInt>.ToByte(Value);

        public short ToInt16() => EnumCache<TInt>.ToInt16(Value);

        public ushort ToUInt16() => EnumCache<TInt>.ToUInt16(Value);

        public int ToInt32() => EnumCache<TInt>.ToInt32(Value);

        public uint ToUInt32() => EnumCache<TInt>.ToUInt32(Value);

        public long ToInt64() => EnumCache<TInt>.ToInt64(Value);

        public ulong ToUInt64() => EnumCache<TInt>.ToUInt64(Value);

        public override int GetHashCode() => Value.GetHashCode();

        public int CompareTo(InternalEnumMemberInfo<TInt> obj) => EnumCache<TInt>.Compare(Value, obj.Value);

        #region Explicit Interface Implementation
        string IFormattable.ToString(string format, IFormatProvider formatProvider) => ToString(format);

        TypeCode IConvertible.GetTypeCode() => EnumCache<TInt>.TypeCode;

        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Value);

        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(Value);

        sbyte IConvertible.ToSByte(IFormatProvider provider) => ToSByte();

        byte IConvertible.ToByte(IFormatProvider provider) => ToByte();

        short IConvertible.ToInt16(IFormatProvider provider) => ToInt16();

        ushort IConvertible.ToUInt16(IFormatProvider provider) => ToUInt16();

        int IConvertible.ToInt32(IFormatProvider provider) => ToInt32();

        uint IConvertible.ToUInt32(IFormatProvider provider) => ToUInt32();

        long IConvertible.ToInt64(IFormatProvider provider) => ToInt64();

        ulong IConvertible.ToUInt64(IFormatProvider provider) => ToUInt64();

        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(Value);

        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(Value);

        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(Value);

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        string IConvertible.ToString(IFormatProvider provider) => ToString();

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Value, conversionType, provider);

        int IComparable.CompareTo(object obj)
        {
            if (obj is InternalEnumMemberInfo<TInt>)
            {
                return CompareTo((InternalEnumMemberInfo<TInt>)obj);
            }
            return 1;
        }

        object IEnumMemberInfo.Value => Value;

        object IEnumMemberInfo.UnderlyingValue => Value;
        #endregion
    }
}
