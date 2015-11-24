// Enums.NET
// Copyright 2015 Tyler Brinkley. All rights reserved.
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

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace EnumsNET
{
    /// <summary>
    /// Class that provides efficient defined enum value operations
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public class EnumMemberInfo<TEnum> : IFormattable, IConvertible, IComparable, IComparable<TEnum>, IComparable<EnumMemberInfo<TEnum>>, IEnumMemberInfo
    {
        /// <summary>
        /// The defined enum member's value
        /// </summary>
        public TEnum Value { get; }

        /// <summary>
        /// The defined enum member's name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The defined enum member's attributes
        /// </summary>
        public Attribute[] Attributes { get; }

        /// <summary>
        /// The defined enum member's <see cref="DescriptionAttribute.Description"/> if applied else null.
        /// </summary>
        public string Description => Enums.GetDescription(Attributes);

        /// <summary>
        /// The defined enum member's <see cref="EnumMemberAttribute.Value"/> if applied else null.
        /// </summary>
        public string EnumMemberValue => Enums.GetEnumMemberValue(Attributes);

        public object UnderlyingValue => EnumsCache<TEnum>.GetUnderlyingValue(Value);

        internal EnumMemberInfo(TEnum value, string name, Attribute[] attributes)
        {
            Value = value;
            Name = name;
            Attributes = attributes;
        }

        /// <summary>
        /// Retrieves the <see cref="Description"/> if not null else the <see cref="Name"/>.
        /// </summary>
        /// <returns></returns>
        public string GetDescriptionOrName() => Description ?? Name;

        /// <summary>
        /// Retrieves the <see cref="Description"/> if not null else the <see cref="Name"/> that's been formatted with <paramref name="nameFormatter"/>.
        /// </summary>
        /// <param name="nameFormatter"></param>
        /// <returns></returns>
        public string GetDescriptionOrName(Func<string, string> nameFormatter)
        {
            return Description ?? (nameFormatter != null ? nameFormatter(Name) : Name);
        }

        public bool HasAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            return GetAttribute<TAttribute>() != null;
        }

        /// <summary>
        /// Retrieves the first <typeparamref name="TAttribute"/> in <see cref="Attributes"/> if defined else null.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public TAttribute GetAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            return Enums.GetAttribute<TAttribute>(Attributes);
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

        /// <summary>
        /// Retrieves all <typeparamref name="TAttribute"/>'s in <see cref="Attributes"/>.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public TAttribute[] GetAttributes<TAttribute>()
            where TAttribute : Attribute
        {
            return Enums.GetAttributes<TAttribute>(Attributes);
        }

        public override string ToString() => Name;

        public string ToString(string format) => EnumsCache<TEnum>.InternalFormat(ToInternalEnumMemberInfo(), format);

        public string ToString(params EnumFormat[] formats) => EnumsCache<TEnum>.InternalFormat(Value, ToInternalEnumMemberInfo(), formats);

        public string AsString() => ToString();

        public string AsString(string format) => ToString(format);

        public string AsString(params EnumFormat[] formats) => ToString(formats);

        public string Format(string format) => EnumsCache<TEnum>.InternalFormat(ToInternalEnumMemberInfo(), format);

        public string Format(params EnumFormat[] formats) => EnumsCache<TEnum>.InternalFormat(Value, ToInternalEnumMemberInfo(), formats);

        [CLSCompliant(false)]
        public sbyte ToSByte() => EnumsCache<TEnum>.ToSByte(Value);

        public byte ToByte() => EnumsCache<TEnum>.ToByte(Value);

        public short ToInt16() => EnumsCache<TEnum>.ToInt16(Value);

        [CLSCompliant(false)]
        public ushort ToUInt16() => EnumsCache<TEnum>.ToUInt16(Value);

        public int ToInt32() => EnumsCache<TEnum>.ToInt32(Value);

        [CLSCompliant(false)]
        public uint ToUInt32() => EnumsCache<TEnum>.ToUInt32(Value);

        public long ToInt64() => EnumsCache<TEnum>.ToInt64(Value);

        [CLSCompliant(false)]
        public ulong ToUInt64() => EnumsCache<TEnum>.ToUInt64(Value);

        private InternalEnumMemberInfo<TEnum> ToInternalEnumMemberInfo() => new InternalEnumMemberInfo<TEnum>(Value, Name, Attributes);

        #region Explicit Interface Implementation
        string IFormattable.ToString(string format, IFormatProvider formatProvider) => ToString(format);

        TypeCode IConvertible.GetTypeCode() => EnumsCache<TEnum>.UnderlyingTypeCode;

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
            var assigned = false;
            TEnum objValue = default(TEnum);
            if (obj is TEnum)
            {
                objValue = (TEnum)obj;
                assigned = true;
            }
            else
            {
                var info = obj as EnumMemberInfo<TEnum>;
                if (info != null)
                {
                    objValue = info.Value;
                    assigned = true;
                }
            }
            if (assigned)
            {
                return EnumsCache<TEnum>.Compare(Value, objValue);
            }
            return 1;
        }

        int IComparable<TEnum>.CompareTo(TEnum other) => EnumsCache<TEnum>.Compare(Value, other);

        int IComparable<EnumMemberInfo<TEnum>>.CompareTo(EnumMemberInfo<TEnum> other) => other != null ? EnumsCache<TEnum>.Compare(Value, other.Value) : 1;

        object IEnumMemberInfo.Value => Value;
        #endregion
    }
}
