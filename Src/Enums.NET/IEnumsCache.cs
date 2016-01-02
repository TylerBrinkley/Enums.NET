using System;
using System.Collections.Generic;

namespace EnumsNET
{
	internal interface IEnumsCache
	{
		#region Properties
		bool IsContiguous { get; }

		Type UnderlyingType { get; }

		TypeCode UnderlyingTypeCode { get; }

		bool IsFlagEnum { get; }

		object AllFlags { get; }
		#endregion

		#region Standard Enum Operations
		#region Type Methods
		int GetDefinedCount(bool uniqueValued);

		IEnumerable<string> GetNames(bool uniqueValued);

		IEnumerable<object> GetValues(bool uniqueValued);

		IEnumerable<string> GetDescriptions(bool uniqueValued);

		IEnumerable<string> GetEnumMemberValues(bool uniqueValued);

		IEnumerable<Attribute[]> GetAllAttributes(bool uniqueValued);

		IEnumerable<string> GetDescriptionsOrNames(bool uniqueValued);

		IEnumerable<string> GetDescriptionsOrNames(Func<string, string> nameFormatter, bool uniqueValued);

		IEnumerable<EnumMemberInfo> GetEnumMemberInfos(bool uniqueValued);

		int Compare(object x, object y);
		#endregion

		#region IsValid
		bool IsValid(object value);

		bool IsValid(long value);

		bool IsValid(ulong value);
		#endregion

		#region IsDefined
		bool IsDefined(object value);

		bool IsDefined(string name);

		bool IsDefined(string name, bool ignoreCase);

		bool IsDefined(long value);

		bool IsDefined(ulong value);
		#endregion

		#region IsWithinUnderlyingTypesValueRange
		bool IsWithinUnderlyingTypesValueRange(long value);

		bool IsWithinUnderlyingTypesValueRange(ulong value);
		#endregion

		#region ToEnum
		object ToObject(object value, bool validate);

		object ToObject(long value, bool validate);

		object ToObject(ulong value, bool validate);

		object ToObjectOrDefault(object value, object defaultEnum, bool validate);

		object ToObjectOrDefault(long value, object defaultEnum, bool validate);

		object ToObjectOrDefault(ulong value, object defaultEnum, bool validate);

		bool TryToObject(object value, out object result, bool validate);

		bool TryToObject(long value, out object result, bool validate);

		bool TryToObject(ulong value, out object result, bool validate);
		#endregion

		#region All Values Main Methods
		object Validate(object value, string paramName);

		string AsString(object value);

		string AsString(object value, string format);

		string AsString(object value, params EnumFormat[] formats);

		string Format(object value, string format);

		string Format(object value, params EnumFormat[] formats);

		object GetUnderlyingValue(object value);

		sbyte ToSByte(object value);

		byte ToByte(object value);

		short ToInt16(object value);

		ushort ToUInt16(object value);

		int ToInt32(object value);

		uint ToUInt32(object value);

		long ToInt64(object value);

		ulong ToUInt64(object value);
		#endregion

		#region Defined Values Main Methods
		EnumMemberInfo GetEnumMemberInfo(object value);

		EnumMemberInfo GetEnumMemberInfo(string name);

		EnumMemberInfo GetEnumMemberInfo(string name, bool ignoreCase);

		string GetName(object value);

		string GetDescription(object value);

		string GetDescriptionOrName(object value);

		string GetDescriptionOrName(object value, Func<string, string> nameFormatter);

		string GetEnumMemberValue(object value);
		#endregion

		#region Attributes
		Attribute[] GetAllAttributes(object value);
		#endregion

		#region Parsing
		object Parse(string value);

		object Parse(string value, EnumFormat[] parseFormatOrder);

		object Parse(string value, bool ignoreCase);

		object Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder);

		object ParseOrDefault(string value, object defaultEnum);

		object ParseOrDefault(string value, object defaultEnum, EnumFormat[] parseFormatOrder);

		object ParseOrDefault(string value, bool ignoreCase, object defaultEnum);

		object ParseOrDefault(string value, bool ignoreCase, object defaultEnum, EnumFormat[] parseFormatOrder);

		bool TryParse(string value, out object result);

		bool TryParse(string value, out object result, EnumFormat[] parseFormatOrder);

		bool TryParse(string value, bool ignoreCase, out object result);

		bool TryParse(string value, bool ignoreCase, out object result, EnumFormat[] parseFormatOrder);
		#endregion
		#endregion

		#region Flag Enum Operations
		#region Main Methods
		bool IsValidFlagCombination(object value);

		string FormatAsFlags(object value);

		string FormatAsFlags(object value, string delimiter);

		string FormatAsFlags(object value, EnumFormat[] formats);

		string FormatAsFlags(object value, string delimiter, EnumFormat[] formats);

		object[] GetFlags(object value);

		bool HasAnyFlags(object value);

		bool HasAnyFlags(object value, object flagMask);

		bool HasAllFlags(object value);

		bool HasAllFlags(object value, object flagMask);

		object InvertFlags(object value);

		object InvertFlags(object value, object flagMask);

		object CommonFlags(object value, object flagMask);

		object SetFlags(object flag0, object flag1);

		object SetFlags(object flag0, object flag1, object flag2);

		object SetFlags(object flag0, object flag1, object flag2, object flag3);

		object SetFlags(object flag0, object flag1, object flag2, object flag3, object flag4);

		object SetFlags(object[] flags);

		object ClearFlags(object value, object flagMask);
		#endregion

		#region Parsing
		object ParseFlags(string value);

		object ParseFlags(string value, EnumFormat[] parseFormatOrder);

		object ParseFlags(string value, bool ignoreCase);

		object ParseFlags(string value, bool ignoreCase, EnumFormat[] parseFormatOrder);

		object ParseFlags(string value, string delimiter);

		object ParseFlags(string value, string delimiter, EnumFormat[] parseFormatOrder);

		object ParseFlags(string value, bool ignoreCase, string delimiter);

		object ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder);

		object ParseFlagsOrDefault(string value, object defaultEnum);

		object ParseFlagsOrDefault(string value, object defaultEnum, EnumFormat[] parseFormatOrder);

		object ParseFlagsOrDefault(string value, bool ignoreCase, object defaultEnum);

		object ParseFlagsOrDefault(string value, bool ignoreCase, object defaultEnum, EnumFormat[] parseFormatOrder);

		object ParseFlagsOrDefault(string value, string delimiter, object defaultEnum);

		object ParseFlagsOrDefault(string value, string delimiter, object defaultEnum, EnumFormat[] parseFormatOrder);

		object ParseFlagsOrDefault(string value, bool ignoreCase, string delimiter, object defaultEnum);

		object ParseFlagsOrDefault(string value, bool ignoreCase, string delimiter, object defaultEnum, EnumFormat[] parseFormatOrder);

		bool TryParseFlags(string value, out object result);

		bool TryParseFlags(string value, out object result, EnumFormat[] parseFormatOrder);

		bool TryParseFlags(string value, bool ignoreCase, out object result);

		bool TryParseFlags(string value, bool ignoreCase, out object result, EnumFormat[] parseFormatOrder);

		bool TryParseFlags(string value, string delimiter, out object result);

		bool TryParseFlags(string value, string delimiter, out object result, EnumFormat[] parseFormatOrder);

		bool TryParseFlags(string value, bool ignoreCase, string delimiter, out object result);

		bool TryParseFlags(string value, bool ignoreCase, string delimiter, out object result, EnumFormat[] parseFormatOrder);
		#endregion
		#endregion
	}
}
