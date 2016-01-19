// Enums.NET
// Copyright 2015 Tyler Brinkley. All rights reserved.
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
	internal struct InternalEnumMemberInfo<TEnum> : IEnumMemberInfo<TEnum>
	{
		private readonly Attribute[] _attributes;

		public TEnum Value { get; }

		public string Name { get; }

		public Attribute[] Attributes
		{
			get
			{
				if (Name == null)
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

		public object UnderlyingValue => Enums<TEnum>.Cache.GetUnderlyingValue(Value);

		public bool IsDefined => Name != null;

		public InternalEnumMemberInfo(TEnum value, string name, Attribute[] attributes)
		{
			Value = value;
			Name = name;
			_attributes = attributes;
		}

		public string GetDescriptionOrName() => Description ?? Name;

		public string GetDescriptionOrName(Func<string, string> nameFormatter) => Name != null ? (Description ?? (nameFormatter != null ? nameFormatter(Name) : Name)) : null;

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
			return Name != null ? (_attributes != null ? Enums.GetAttributes<TAttribute>(_attributes) : new TAttribute[0]) : null;
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

		public EnumMemberInfo<TEnum> ToEnumMemberInfo() => Name != null ? new EnumMemberInfo<TEnum>(Value, Name, _attributes) : null;

		public override string ToString() => Name;

		public string ToString(string format) => Enums<TEnum>.Cache.InternalFormat(this, format);

		public string ToString(params EnumFormat[] formats) => Enums<TEnum>.Cache.InternalFormat(Value, this, formats);

		public string AsString() => ToString();

		public string AsString(string format) => ToString(format);

		public string AsString(params EnumFormat[] formats) => ToString(formats);

		public string Format(string format) => Enums<TEnum>.Cache.InternalFormat(this, format);

		public string Format(EnumFormat format) => Enums<TEnum>.Cache.InternalFormat(Value, this, format);

		public string Format(EnumFormat format0, EnumFormat format1) => Enums<TEnum>.Cache.InternalFormat(Value, this, format0, format1);

		public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2) => Enums<TEnum>.Cache.InternalFormat(Value, this, format0, format1, format2);

		public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3) => Enums<TEnum>.Cache.InternalFormat(Value, this, format0, format1, format2, format3);

		public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4) => Enums<TEnum>.Cache.InternalFormat(Value, this, format0, format1, format2, format3, format4);

		public string Format(params EnumFormat[] formats) => Enums<TEnum>.Cache.InternalFormat(Value, this, formats);

		public sbyte ToSByte() => Enums<TEnum>.Cache.ToSByte(Value);

		public byte ToByte() => Enums<TEnum>.Cache.ToByte(Value);

		public short ToInt16() => Enums<TEnum>.Cache.ToInt16(Value);

		public ushort ToUInt16() => Enums<TEnum>.Cache.ToUInt16(Value);

		public int ToInt32() => Enums<TEnum>.Cache.ToInt32(Value);

		public uint ToUInt32() => Enums<TEnum>.Cache.ToUInt32(Value);

		public long ToInt64() => Enums<TEnum>.Cache.ToInt64(Value);

		public ulong ToUInt64() => Enums<TEnum>.Cache.ToUInt64(Value);

		#region Explicit Interface Implementation
		string IFormattable.ToString(string format, IFormatProvider formatProvider) => ToString(format);

		TypeCode IConvertible.GetTypeCode() => Enums<TEnum>.Cache.TypeCode;

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
				var info = obj as IEnumMemberInfo<TEnum>;
				if (info != null)
				{
					objValue = info.Value;
					assigned = true;
				}
			}
			if (assigned)
			{
				return Enums<TEnum>.Cache.Compare(Value, objValue);
			}
			return 1;
		}

		int IComparable<TEnum>.CompareTo(TEnum other) => Enums<TEnum>.Cache.Compare(Value, other);

		object IClsEnumMemberInfo.Value => Value;
		#endregion
	}
}
