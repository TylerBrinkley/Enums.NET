using System;

namespace EnumsNET
{
	internal interface IEnumsCache
	{
		#region Properties
		bool IsContiguous { get; }

		Type UnderlyingType { get; }

		bool IsFlagEnum { get; }

		object AllFlags { get; }
		#endregion

		#region Standard Enum Operations
		#region Type Methods
		int GetDefinedCount(bool uniqueDefined);

		string[] GetNames(bool uniqueDefined);

		object[] GetValues(bool uniqueDefined);

		string[] GetDescriptions(bool uniqueDefined);

		string[] GetEnumMemberValues(bool uniqueDefined);

		Attribute[][] GetAllAttributes(bool uniqueDefined);

		string[] GetDescriptionsOrNames(bool uniqueDefined);

		string[] GetDescriptionsOrNames(Func<string, string> nameFormatter, bool uniqueDefined);

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
		object ToEnum(object value, bool validate);

		object ToEnum(long value, bool validate);

		object ToEnum(ulong value, bool validate);

		object ToEnumOrDefault(object value, object defaultEnum, bool validate);

		object ToEnumOrDefault(long value, object defaultEnum, bool validate);

		object ToEnumOrDefault(ulong value, object defaultEnum, bool validate);

		bool TryToEnum(object value, out object result, bool validate);

		bool TryToEnum(long value, out object result, bool validate);

		bool TryToEnum(ulong value, out object result, bool validate);
		#endregion

		#region All Values Main Methods
		object Validate(object value, string paramName);

		string AsString(object value);

		string AsString(object value, string format);

		string AsString(object value, params EnumFormat[] formats);

		string AsString(string name);

		string AsString(string name, string format);

		string AsString(string name, params EnumFormat[] formats);

		string Format(object value, string format);

		string Format(object value, params EnumFormat[] formats);

		string Format(string name, string format);

		string Format(string name, params EnumFormat[] formats);

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
		string GetName(object value);

		string GetDescription(object value);

		string GetDescription(string name);

		string GetDescriptionOrName(object value);

		string GetDescriptionOrName(object value, Func<string, string> nameFormatter);

		string GetDescriptionOrName(string name);

		string GetDescriptionOrName(string name, Func<string, string> nameFormatter);

		string GetEnumMemberValue(object value);

		string GetEnumMemberValue(string name);
		#endregion

		#region Attributes
		Attribute[] GetAllAttributes(object value);

		Attribute[] GetAllAttributes(string name);
		#endregion

		#region Parsing
		object Parse(string value);

		object Parse(string value, bool ignoreCase);

		object Parse(string value, params EnumFormat[] parseOrder);

		object Parse(string value, bool ignoreCase, params EnumFormat[] parseOrder);

		object ParseOrDefault(string value, object defaultEnum);

		object ParseOrDefault(string value, bool ignoreCase, object defaultEnum);

		object ParseOrDefault(string value, object defaultEnum, params EnumFormat[] parseOrder);

		object ParseOrDefault(string value, bool ignoreCase, object defaultEnum, params EnumFormat[] parseOrder);

		bool TryParse(string value, out object result);

		bool TryParse(string value, bool ignoreCase, out object result);

		bool TryParse(string value, out object result, params EnumFormat[] parseOrder);

		bool TryParse(string value, bool ignoreCase, out object result, params EnumFormat[] parseOrder);
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

		object CommonFlags(object[] flags);

		object SetFlags(object value, object flagMask);

		object SetFlags(object[] flags);

		object ClearFlags(object value, object flagMask);
		#endregion

		#region Parsing
		object ParseFlags(string value);

		object ParseFlags(string value, bool ignoreCase);

		object ParseFlags(string value, string delimiter);

		object ParseFlags(string value, bool ignoreCase, string delimiter);

		object ParseFlagsOrDefault(string value, object defaultEnum);

		object ParseFlagsOrDefault(string value, bool ignoreCase, object defaultEnum);

		object ParseFlagsOrDefault(string value, string delimiter, object defaultEnum);

		object ParseFlagsOrDefault(string value, bool ignoreCase, string delimiter, object defaultEnum);

		bool TryParseFlags(string value, out object result);

		bool TryParseFlags(string value, bool ignoreCase, out object result);

		bool TryParseFlags(string value, string delimiter, out object result);

		bool TryParseFlags(string value, bool ignoreCase, string delimiter, out object result);
		#endregion
		#endregion
	}
}
