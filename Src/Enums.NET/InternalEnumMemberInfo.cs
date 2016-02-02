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

namespace EnumsNET
{
    internal struct InternalEnumMemberInfo<TInt> : IEnumMemberInfo, IComparable<TInt>
    {
        private readonly Attribute[] _attributes;
        private readonly EnumsCache<TInt> _enumsCache;

        public TInt Value { get; }

        public string Name { get; }

        public Attribute[] Attributes
        {
            get
            {
                if (!IsDefined)
                {
                    return null;
                }
                if (_attributes == null)
                {
                    return Enums.ZeroLengthAttributes;
                }
                return _attributes.Copy();
            }
        }

        public string Description => _attributes != null ? Enums.GetDescription(_attributes) : null;

        public bool IsDefined => Name != null;

        public InternalEnumMemberInfo(TInt value, string name, Attribute[] attributes, EnumsCache<TInt> enumsCache)
        {
            Value = value;
            Name = name;
            _attributes = attributes;
            _enumsCache = enumsCache;
        }

        public string GetDescriptionOrName() => Description ?? Name;

        public string GetDescriptionOrName(Func<string, string> nameFormatter) => IsDefined ? (Description ?? (nameFormatter != null ? nameFormatter(Name) : Name)) : null;

        public bool HasAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            return GetAttribute<TAttribute>() != null;
        }

        public TAttribute GetAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            return _attributes != null ? Enums.GetAttribute<TAttribute>(_attributes) : null;
        }

        public IEnumerable<TAttribute> GetAttributes<TAttribute>()
            where TAttribute : Attribute
        {
            return IsDefined ? (_attributes != null ? Enums.GetAttributes<TAttribute>(_attributes) : new TAttribute[0]) : null;
        }

        public TResult GetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, TResult defaultValue = default(TResult))
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

        public override string ToString() => Name;

        public string ToString(string format) => _enumsCache.InternalFormat(this, format);

        public string ToString(params EnumFormat[] formats) => _enumsCache.InternalFormat(this, formats);

        public string AsString() => ToString();

        public string AsString(string format) => ToString(format);

        public string AsString(params EnumFormat[] formats) => ToString(formats);

        public string Format(string format) => _enumsCache.InternalFormat(this, format);

        public string Format(EnumFormat format) => _enumsCache.InternalFormat(this, format);

        public string Format(EnumFormat format0, EnumFormat format1) => _enumsCache.InternalFormat(this, format0, format1);

        public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2) => _enumsCache.InternalFormat(this, format0, format1, format2);

        public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3) => _enumsCache.InternalFormat(this, format0, format1, format2, format3);

        public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4) => _enumsCache.InternalFormat(this, format0, format1, format2, format3, format4);

        public string Format(params EnumFormat[] formats) => _enumsCache.InternalFormat(this, formats);

        public sbyte ToSByte() => EnumsCache<TInt>.ToSByte(Value);

        public byte ToByte() => EnumsCache<TInt>.ToByte(Value);

        public short ToInt16() => EnumsCache<TInt>.ToInt16(Value);

        public ushort ToUInt16() => EnumsCache<TInt>.ToUInt16(Value);

        public int ToInt32() => EnumsCache<TInt>.ToInt32(Value);

        public uint ToUInt32() => EnumsCache<TInt>.ToUInt32(Value);

        public long ToInt64() => EnumsCache<TInt>.ToInt64(Value);

        public ulong ToUInt64() => EnumsCache<TInt>.ToUInt64(Value);

        #region Explicit Interface Implementation
        string IFormattable.ToString(string format, IFormatProvider formatProvider) => ToString(format);

        TypeCode IConvertible.GetTypeCode() => EnumsCache<TInt>.TypeCode;

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
            var objAsInfo = obj as IEnumMemberInfo;
            if (objAsInfo != null)
            {
                return ((IComparable)this).CompareTo(objAsInfo.Value);
            }
            var assigned = false;
            TInt objValue = default(TInt);
            if (obj is TInt)
            {
                objValue = (TInt)obj;
                assigned = true;
            }
            else
            {
                var info = obj as IEnumMemberInfo<TInt>;
                if (info != null)
                {
                    objValue = info.Value;
                    assigned = true;
                }
            }
            if (assigned)
            {
                return ((IComparable<TInt>)this).CompareTo(objValue);
            }
            return 1;
        }

        int IComparable<TInt>.CompareTo(TInt other) => EnumsCache<TInt>.Compare(Value, other);

        object IClsEnumMemberInfo.Value => Value;

        object IClsEnumMemberInfo.UnderlyingValue => Value;
        #endregion
    }
}
