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
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace EnumsNET
{
	public sealed class EnumMemberInfo : IEnumMemberInfo, IComparable<EnumMemberInfo>
	{
		private readonly IEnumMemberInfo _enumMemberInfo;

		public object Value => _enumMemberInfo.Value;

		public string Name => _enumMemberInfo.Name;

		public Attribute[] Attributes => _enumMemberInfo.Attributes;

		public string Description => _enumMemberInfo.Description;

		public object UnderlyingValue => _enumMemberInfo.UnderlyingValue;

		internal EnumMemberInfo(IEnumMemberInfo enumMemberInfo)
		{
			_enumMemberInfo = enumMemberInfo;
		}

		public string GetDescriptionOrName() => _enumMemberInfo.GetDescriptionOrName();

		public string GetDescriptionOrName(Func<string, string> nameFormatter) => _enumMemberInfo.GetDescriptionOrName(nameFormatter);

		public override string ToString() => _enumMemberInfo.ToString();

		public string ToString(string format) => _enumMemberInfo.ToString(format);

		public string ToString(params EnumFormat[] formats) => _enumMemberInfo.ToString(formats);

		public string AsString() => _enumMemberInfo.AsString();

		public string AsString(string format) => _enumMemberInfo.AsString(format);

		public string AsString(params EnumFormat[] formats) => _enumMemberInfo.AsString(formats);

		public string Format(string format) => _enumMemberInfo.Format(format);

		public string Format(EnumFormat format) => _enumMemberInfo.Format(format);

		public string Format(EnumFormat format0, EnumFormat format1) => _enumMemberInfo.Format(format0, format1);

		public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2) => _enumMemberInfo.Format(format0, format1, format2);

		public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3) => _enumMemberInfo.Format(format0, format1, format2, format3);

		public string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4) => _enumMemberInfo.Format(format0, format1, format2, format3, format4);

		public string Format(params EnumFormat[] formats) => _enumMemberInfo.Format(formats);

		public bool HasAttribute<TAttribute>() where TAttribute : Attribute => _enumMemberInfo.HasAttribute<TAttribute>();

		public TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute => _enumMemberInfo.GetAttribute<TAttribute>();

		public TResult GetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, TResult defaultValue = default(TResult)) where TAttribute : Attribute => _enumMemberInfo.GetAttributeSelect(selector, defaultValue);

		public bool TryGetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, out TResult result) where TAttribute : Attribute => _enumMemberInfo.TryGetAttributeSelect(selector, out result);

		public IEnumerable<TAttribute> GetAttributes<TAttribute>() where TAttribute : Attribute => _enumMemberInfo.GetAttributes<TAttribute>();

		[CLSCompliant(false)]
		public sbyte ToSByte() => _enumMemberInfo.ToSByte();

		public byte ToByte() => _enumMemberInfo.ToByte();

		public short ToInt16() => _enumMemberInfo.ToInt16();

		[CLSCompliant(false)]
		public ushort ToUInt16() => _enumMemberInfo.ToUInt16();

		public int ToInt32() => _enumMemberInfo.ToInt32();

		[CLSCompliant(false)]
		public uint ToUInt32() => _enumMemberInfo.ToUInt32();

		public long ToInt64() => _enumMemberInfo.ToInt64();
		
		[CLSCompliant(false)]
		public ulong ToUInt64() => _enumMemberInfo.ToUInt64();

		#region Explicit Interface Implementation
		string IFormattable.ToString(string format, IFormatProvider formatProvider) => _enumMemberInfo.ToString(format, formatProvider);

		TypeCode IConvertible.GetTypeCode() => _enumMemberInfo.GetTypeCode();

		bool IConvertible.ToBoolean(IFormatProvider provider) => _enumMemberInfo.ToBoolean(provider);

		char IConvertible.ToChar(IFormatProvider provider) => _enumMemberInfo.ToChar(provider);

		sbyte IConvertible.ToSByte(IFormatProvider provider) => _enumMemberInfo.ToSByte(provider);

		byte IConvertible.ToByte(IFormatProvider provider) => _enumMemberInfo.ToByte(provider);

		short IConvertible.ToInt16(IFormatProvider provider) => _enumMemberInfo.ToInt16(provider);

		ushort IConvertible.ToUInt16(IFormatProvider provider) => _enumMemberInfo.ToUInt16(provider);

		int IConvertible.ToInt32(IFormatProvider provider) => _enumMemberInfo.ToInt32(provider);

		uint IConvertible.ToUInt32(IFormatProvider provider) => _enumMemberInfo.ToUInt32(provider);

		long IConvertible.ToInt64(IFormatProvider provider) => _enumMemberInfo.ToInt64(provider);

		ulong IConvertible.ToUInt64(IFormatProvider provider) => _enumMemberInfo.ToUInt64(provider);

		float IConvertible.ToSingle(IFormatProvider provider) => _enumMemberInfo.ToSingle(provider);

		double IConvertible.ToDouble(IFormatProvider provider) => _enumMemberInfo.ToDouble(provider);

		decimal IConvertible.ToDecimal(IFormatProvider provider) => _enumMemberInfo.ToDecimal(provider);

		DateTime IConvertible.ToDateTime(IFormatProvider provider) => _enumMemberInfo.ToDateTime(provider);

		string IConvertible.ToString(IFormatProvider provider) => _enumMemberInfo.ToString(provider);

		object IConvertible.ToType(Type conversionType, IFormatProvider provider) => _enumMemberInfo.ToType(conversionType, provider);

		int IComparable.CompareTo(object obj) => _enumMemberInfo.CompareTo((obj as EnumMemberInfo)?.Value ?? obj);

		int IComparable<EnumMemberInfo>.CompareTo(EnumMemberInfo other) => _enumMemberInfo.CompareTo(other?.Value);

		bool IClsEnumMemberInfo.IsDefined => _enumMemberInfo.IsDefined;
		#endregion
	}

	/// <summary>
	/// Class that provides efficient defined enum member operations
	/// </summary>
	/// <typeparam name="TEnum"></typeparam>
	public sealed class EnumMemberInfo<TEnum> : IEnumMemberInfo<TEnum>, IComparable<EnumMemberInfo<TEnum>>
	{
		private Attribute[] _attributes;

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
		public Attribute[] Attributes => _attributes.Copy();

		/// <summary>
		/// The defined enum member's <see cref="DescriptionAttribute.Description"/> if applied else null.
		/// </summary>
		public string Description => Enums.GetDescription(_attributes);

		/// <summary>
		/// The defined enum member's underlying integer value
		/// </summary>
		public object UnderlyingValue => Enums<TEnum>.Cache.GetUnderlyingValue(Value);

		internal EnumMemberInfo(TEnum value, string name, Attribute[] attributes)
		{
			Value = value;
			Name = name;
			_attributes = attributes ?? Enums.ZeroLengthAttributes;
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

		/// <summary>
		/// Indicates if <see cref="Attributes"/> contains a <typeparamref name="TAttribute"/>.
		/// </summary>
		/// <typeparam name="TAttribute"></typeparam>
		/// <returns>Indication if <see cref="Attributes"/> contains a <typeparamref name="TAttribute"/>.</returns>
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
			return Enums.GetAttribute<TAttribute>(_attributes);
		}

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

		/// <summary>
		/// Tries to retrieve the first <typeparamref name="TAttribute"/> in <see cref="Attributes"/> if defined and sets <paramref name="result"/>
		/// to applying the <paramref name="selector"/> to the <typeparamref name="TAttribute"/>. Returns true if a <typeparamref name="TAttribute"/>
		/// is found else false.
		/// </summary>
		/// <typeparam name="TAttribute"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="selector"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="selector"/> is null.</exception>
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
		public IEnumerable<TAttribute> GetAttributes<TAttribute>()
			where TAttribute : Attribute
		{
			return Enums.GetAttributes<TAttribute>(_attributes);
		}

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

		[CLSCompliant(false)]
		public sbyte ToSByte() => Enums<TEnum>.Cache.ToSByte(Value);

		public byte ToByte() => Enums<TEnum>.Cache.ToByte(Value);

		public short ToInt16() => Enums<TEnum>.Cache.ToInt16(Value);

		[CLSCompliant(false)]
		public ushort ToUInt16() => Enums<TEnum>.Cache.ToUInt16(Value);

		public int ToInt32() => Enums<TEnum>.Cache.ToInt32(Value);

		[CLSCompliant(false)]
		public uint ToUInt32() => Enums<TEnum>.Cache.ToUInt32(Value);

		public long ToInt64() => Enums<TEnum>.Cache.ToInt64(Value);

		[CLSCompliant(false)]
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
				var info = obj as EnumMemberInfo<TEnum>;
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

		int IComparable<EnumMemberInfo<TEnum>>.CompareTo(EnumMemberInfo<TEnum> other) => other != null ? Enums<TEnum>.Cache.Compare(Value, other.Value) : 1;

		bool IClsEnumMemberInfo.IsDefined => true;

		object IClsEnumMemberInfo.Value => Value;
		#endregion
	}
}
