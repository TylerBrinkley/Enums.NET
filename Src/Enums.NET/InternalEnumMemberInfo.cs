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
    internal struct InternalEnumMemberInfo<TEnum, TInt> : IEnumMemberInfo<TEnum>
    {
        private readonly Attribute[] _attributes;

        public TEnum Value => EnumsCache<TEnum, TInt>.ToEnum(UnderlyingValue);

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

        public TInt UnderlyingValue { get; }

        public bool IsDefined => Name != null;

        public InternalEnumMemberInfo(TInt value, string name, Attribute[] attributes)
        {
            UnderlyingValue = value;
            Name = name;
            _attributes = attributes;
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

        public static implicit operator EnumMemberInfo<TEnum>(InternalEnumMemberInfo<TEnum, TInt> info) => info.IsDefined ? new EnumMemberInfo<TEnum>(info) : null;

        public static implicit operator EnumMemberInfo(InternalEnumMemberInfo<TEnum, TInt> info) => info.IsDefined ? new EnumMemberInfo(info) : null;

        public override string ToString() => Name;

        public string ToString(string format) => EnumsCache<TEnum, TInt>.InternalFormat(this, format);

        public string ToString(params EnumFormat[] formats) => EnumsCache<TEnum, TInt>.InternalFormat(this, formats);

        public string AsString() => ToString();

        public string AsString(string format) => ToString(format);

        public string AsString(params EnumFormat[] formats) => ToString(formats);

        public string Format(string format) => EnumsCache<TEnum, TInt>.InternalFormat(this, format);

        public string Format(EnumFormat format) => EnumsCache<TEnum, TInt>.InternalFormat(this, format);

        public string Format(EnumFormat format0, EnumFormat format1) => EnumsCache<TEnum, TInt>.InternalFormat(this, format0, format1);

        public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2) => EnumsCache<TEnum, TInt>.InternalFormat(this, format0, format1, format2);

        public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3) => EnumsCache<TEnum, TInt>.InternalFormat(this, format0, format1, format2, format3);

        public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4) => EnumsCache<TEnum, TInt>.InternalFormat(this, format0, format1, format2, format3, format4);

        public string Format(params EnumFormat[] formats) => EnumsCache<TEnum, TInt>.InternalFormat(this, formats);

        public sbyte ToSByte() => IntegralOperators<TInt>.ToSByte(UnderlyingValue);

        public byte ToByte() => IntegralOperators<TInt>.ToByte(UnderlyingValue);

        public short ToInt16() => IntegralOperators<TInt>.ToInt16(UnderlyingValue);

        public ushort ToUInt16() => IntegralOperators<TInt>.ToUInt16(UnderlyingValue);

        public int ToInt32() => IntegralOperators<TInt>.ToInt32(UnderlyingValue);

        public uint ToUInt32() => IntegralOperators<TInt>.ToUInt32(UnderlyingValue);

        public long ToInt64() => IntegralOperators<TInt>.ToInt64(UnderlyingValue);

        public ulong ToUInt64() => IntegralOperators<TInt>.ToUInt64(UnderlyingValue);

        #region Explicit Interface Implementation
        string IFormattable.ToString(string format, IFormatProvider formatProvider) => ToString(format);

        TypeCode IConvertible.GetTypeCode() => EnumsCache<TEnum, TInt>.TypeCodeValue;

        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(UnderlyingValue);

        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(UnderlyingValue);

        sbyte IConvertible.ToSByte(IFormatProvider provider) => ToSByte();

        byte IConvertible.ToByte(IFormatProvider provider) => ToByte();

        short IConvertible.ToInt16(IFormatProvider provider) => ToInt16();

        ushort IConvertible.ToUInt16(IFormatProvider provider) => ToUInt16();

        int IConvertible.ToInt32(IFormatProvider provider) => ToInt32();

        uint IConvertible.ToUInt32(IFormatProvider provider) => ToUInt32();

        long IConvertible.ToInt64(IFormatProvider provider) => ToInt64();

        ulong IConvertible.ToUInt64(IFormatProvider provider) => ToUInt64();

        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(UnderlyingValue);

        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(UnderlyingValue);

        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(UnderlyingValue);

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        string IConvertible.ToString(IFormatProvider provider) => ToString();

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(UnderlyingValue, conversionType, provider);

        int IComparable.CompareTo(object obj)
        {
            var objAsInfo = obj as IEnumMemberInfo;
            if (objAsInfo != null)
            {
                return ((IComparable)this).CompareTo(objAsInfo.Value);
            }
            var assigned = false;
            TEnum objValue = default(TEnum);
            if (obj is TEnum)
            {
                objValue = (TEnum)obj;
                assigned = true;
            }
            else
            {
                var info = obj as IEnumMemberInfo<TEnum>;
                if (info != null)
                {
                    objValue = info.Value;
                    assigned = true;
                }
            }
            if (assigned)
            {
                return ((IComparable<TEnum>)this).CompareTo(objValue);
            }
            return 1;
        }

        int IComparable<TEnum>.CompareTo(TEnum other) => EnumsCache<TEnum, TInt>.InternalCompare(UnderlyingValue, EnumsCache<TEnum, TInt>.ToInt(other));

        object IClsEnumMemberInfo.Value => Value;

        object IClsEnumMemberInfo.UnderlyingValue => UnderlyingValue;
        #endregion
    }
}
