using System;
using System.Collections.Generic;

namespace EnumsNET
{
	public interface IClsEnumMemberInfo : IFormattable, IComparable
	{
		bool IsDefined { get; }
		object Value { get; }
		string Name { get; }
		Attribute[] Attributes { get; }
		string Description { get; }
		object UnderlyingValue { get; }

		string GetDescriptionOrName();
		string GetDescriptionOrName(Func<string, string> nameFormatter);
		string ToString();
		string ToString(string format);
		string ToString(params EnumFormat[] formats);
		string AsString();
		string AsString(string format);
		string AsString(params EnumFormat[] formats);
		string Format(string format);
		string Format(EnumFormat format);
		string Format(EnumFormat format0, EnumFormat format1);
		string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2);
		string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3);
		string Format(EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4);
		string Format(params EnumFormat[] formats);
		bool HasAttribute<TAttribute>() where TAttribute : Attribute;
		TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute;
		TResult GetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, TResult defaultValue = default(TResult)) where TAttribute : Attribute;
		bool TryGetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, out TResult result) where TAttribute : Attribute;
		IEnumerable<TAttribute> GetAttributes<TAttribute>() where TAttribute : Attribute;
		byte ToByte();
		short ToInt16();
		int ToInt32();
		long ToInt64();
	}

	public interface IClsEnumMemberInfo<TEnum> : IClsEnumMemberInfo, IComparable<TEnum>
	{
		new TEnum Value { get; }
	}
}
