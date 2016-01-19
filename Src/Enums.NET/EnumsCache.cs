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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace EnumsNET
{
	internal sealed class EnumsCache<TEnum, TInt> : IInternalEnumsCache<TEnum>
	{
		#region Fields
		// The main collection of values, names, and attributes with ~O(1) retrieval on name or value
		// If constant contains a DescriptionAttribute it will be the first in the attribute array
		private static OrderedBiDirectionalDictionary<TInt, NameAndAttributes> _valueMap;

		// Duplicate values are stored here with a key of the constant's name, is null if no duplicates
		private static Dictionary<string, ValueAndAttributes<TInt>> _duplicateValues;

		private static Func<TEnum, TInt> _toInt;

		private static Func<TInt, TEnum> _toEnum;

		private readonly TInt _allFlags;

		private Dictionary<string, string> _ignoreCaseSet;

		private int _lastCustomEnumFormatIndex = -1;

		private List<Func<IEnumMemberInfo<TEnum>, string>> _customEnumFormatters;

		private ConcurrentDictionary<EnumFormat, EnumParser> _customEnumFormatParsers;
		#endregion

		#region Properties
		public TEnum AllFlags => _toEnum(_allFlags);

		public bool IsFlagEnum { get; }

		public bool IsContiguous { get; }

		public TypeCode TypeCode { get; }

		public Type UnderlyingType => typeof(TInt);

		private TInt MaxDefined => _valueMap.GetFirstAt(_valueMap.Count - 1);

		private TInt MinDefined => _valueMap.GetFirstAt(0);

		// Enables case insensitive parsing, lazily instantiated to reduce memory usage if not going to use this feature, is thread-safe
		private Dictionary<string, string> IgnoreCaseSet
		{
			get
			{
				if (_ignoreCaseSet == null)
				{
					var ignoreCaseSet = new Dictionary<string, string>(GetDefinedCount(false), StringComparer.OrdinalIgnoreCase);
					foreach (var nameAndAttributes in _valueMap.SecondItems)
					{
						ignoreCaseSet.Add(nameAndAttributes.Name, nameAndAttributes.Name);
					}
					if (_duplicateValues != null)
					{
						foreach (var name in _duplicateValues.Keys)
						{
							ignoreCaseSet.Add(name, name);
						}
					}
					_ignoreCaseSet = ignoreCaseSet;
				}
				return _ignoreCaseSet;
			}
		}
		#endregion

		public EnumsCache(Func<TEnum, TInt> toInt, Func<TInt, TEnum> toEnum)
		{
			_toInt = toInt;
			_toEnum = toEnum;

			var type = typeof(TEnum);
			Debug.Assert(type.IsEnum);
			TypeCode = Type.GetTypeCode(UnderlyingType);
			IsFlagEnum = type.IsDefined(typeof(FlagsAttribute), false);

			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
			_valueMap = new OrderedBiDirectionalDictionary<TInt, NameAndAttributes>(fields.Length);
			if (fields.Length == 0)
			{
				return;
			}
			var duplicateValues = new Dictionary<string, ValueAndAttributes<TInt>>();
			foreach (var field in fields)
			{
				var value = _toInt((TEnum)field.GetValue(null));
				var name = field.Name;
				var attributes = Attribute.GetCustomAttributes(field, false);
				var isMainDupe = false;
				if (attributes.Length > 0)
				{
					var descriptionFound = false;
					for (var i = 0; i < attributes.Length; ++i)
					{
						if (!descriptionFound)
						{
							var descAttr = attributes[i] as DescriptionAttribute;
							if (descAttr != null)
							{
								for (var j = i; j > 0; --j)
								{
									attributes[j] = attributes[j - 1];
								}
								attributes[0] = descAttr;
								descriptionFound = true;
								if (isMainDupe)
								{
									break;
								}
							}
						}
						if (!isMainDupe && (attributes[i] as MainDuplicateAttribute) != null)
						{
							isMainDupe = true;
							if (descriptionFound)
							{
								break;
							}
						}
					}
				}
				var index = _valueMap.IndexOfFirst(value);
				if (index < 0)
				{
					for (index = _valueMap.Count; index > 0; --index)
					{
						var mapValue = _valueMap.GetFirstAt(index - 1);
						if (!IntegralOperators<TInt>.GreaterThan(mapValue, value))
						{
							break;
						}
					}
					_valueMap.Insert(index, value, new NameAndAttributes(name, attributes));
					if (IntegralOperators<TInt>.IsPowerOfTwo(value))
					{
						_allFlags = IntegralOperators<TInt>.Or(_allFlags, value);
					}
				}
				else
				{
					if (isMainDupe)
					{
						var nameAndAttributes = _valueMap.GetSecondAt(index);
						_valueMap.ReplaceSecondAt(index, new NameAndAttributes(name, attributes));
						name = nameAndAttributes.Name;
						attributes = nameAndAttributes.Attributes;
					}
					duplicateValues.Add(name, new ValueAndAttributes<TInt>(value, attributes));
				}
			}
			var maxDefined = _toEnum(MaxDefined);
			var minDefined = _toEnum(MinDefined);
			if (TypeCode == TypeCode.UInt64)
			{
				IsContiguous = ToUInt64(maxDefined) - ToUInt64(minDefined) + 1UL == (ulong)_valueMap.Count;
			}
			else
			{
				IsContiguous = ToInt64(maxDefined) - ToInt64(minDefined) + 1L == _valueMap.Count;
			}

			_valueMap.TrimExcess();
			if (duplicateValues.Count > 0)
			{
				// Makes sure is in increasing order, due to no removals
				var dupes = duplicateValues.OrderBy(pair => pair.Value.Value).ToList();
				_duplicateValues = new Dictionary<string, ValueAndAttributes<TInt>>(duplicateValues.Count);
				foreach (var pair in dupes)
				{
					_duplicateValues.Add(pair.Key, pair.Value);
				}
			}
		}

		#region Standard Enum Operations
		#region Type Methods
		public int GetDefinedCount(bool uniqueValued) => _valueMap.Count + (uniqueValued ? 0 : _duplicateValues?.Count ?? 0);

		public IEnumerable<EnumMemberInfo<TEnum>> GetEnumMemberInfos(bool uniqueValued) => GetEnumMembersInValueOrder(uniqueValued).Select(info => info.ToEnumMemberInfo());

		public IEnumerable<string> GetNames(bool uniqueValued) => GetEnumMembersInValueOrder(uniqueValued).Select(info => info.Name);

		public IEnumerable<TEnum> GetValues(bool uniqueValued) => GetEnumMembersInValueOrder(uniqueValued).Select(info => info.Value);

		public IEnumerable<string> GetDescriptions(bool uniqueValued) => GetEnumMembersInValueOrder(uniqueValued).Select(info => info.Description);

		public IEnumerable<string> GetDescriptionsOrNames(bool uniqueValued) => GetEnumMembersInValueOrder(uniqueValued).Select(info => info.GetDescriptionOrName());

		public IEnumerable<string> GetDescriptionsOrNames(Func<string, string> nameFormatter, bool uniqueValued) => GetEnumMembersInValueOrder(uniqueValued).Select(info => info.GetDescriptionOrName(nameFormatter));

		public IEnumerable<string> GetFormattedValues(EnumFormat[] formats, bool uniqueValued) => GetEnumMembersInValueOrder(uniqueValued).Select(info => info.Format(formats));

		public IEnumerable<Attribute[]> GetAllAttributes(bool uniqueValued) => GetEnumMembersInValueOrder(uniqueValued).Select(info => info.Attributes);

		public IEnumerable<TAttribute> GetAttributes<TAttribute>(bool uniqueValued)
			where TAttribute : Attribute
		{
			return GetEnumMembersInValueOrder(uniqueValued).Select(info => info.GetAttribute<TAttribute>());
		}

		public int Compare(TEnum x, TEnum y)
		{
			var xAsInt = _toInt(x);
			var yAsInt = _toInt(y);
			if (IntegralOperators<TInt>.GreaterThan(xAsInt, yAsInt))
			{
				return 1;
			}
			if (IntegralOperators<TInt>.GreaterThan(yAsInt, xAsInt))
			{
				return -1;
			}
			return 0;
		}

		public bool Equals(TEnum x, TEnum y) => IntegralOperators<TInt>.Equals(_toInt(x), _toInt(y));

		public int GetHashCode(TEnum value) => _toInt(value).GetHashCode();

		public EnumFormat RegisterCustomEnumFormat(Func<IEnumMemberInfo<TEnum>, string> formatter)
		{
			var index = Interlocked.Increment(ref _lastCustomEnumFormatIndex);
			if (index == 0)
			{
				_customEnumFormatters = new List<Func<IEnumMemberInfo<TEnum>, string>>();
			}
			else
			{
				while (_customEnumFormatters == null || _customEnumFormatters.Count < index)
				{
				}
			}
			_customEnumFormatters.Insert(index, formatter);
			return Enums.ToObject<EnumFormat>(index + Enums.StartingGenericCustomEnumFormatValue, false);
		}

		private IEnumerable<InternalEnumMemberInfo<TEnum>> GetEnumMembersInValueOrder(bool uniqueValued)
		{
			if (uniqueValued)
			{
				return _valueMap.Select(pair =>
				{
					var second = pair.Second;
					return new InternalEnumMemberInfo<TEnum>(_toEnum(pair.First), second.Name, second.Attributes);
				});
			}
			else
			{
				return GetAllEnumMembersInValueOrder();
			}
		}

		private IEnumerable<InternalEnumMemberInfo<TEnum>> GetAllEnumMembersInValueOrder()
		{
			using (var mainEnumerator = _valueMap.GetEnumerator())
			{
				var mainIsActive = mainEnumerator.MoveNext();
				var mainPair = mainIsActive ? mainEnumerator.Current : new Pair<TInt, NameAndAttributes>();
				using (IEnumerator<KeyValuePair<string, ValueAndAttributes<TInt>>> dupeEnumerator = _duplicateValues?.GetEnumerator())
				{
					var dupeIsActive = dupeEnumerator?.MoveNext() ?? false;
					var dupePair = dupeIsActive ? dupeEnumerator.Current : new KeyValuePair<string, ValueAndAttributes<TInt>>();
					var count = GetDefinedCount(false);
					for (var i = 0; i < count; ++i)
					{
						TInt value;
						string name;
						Attribute[] attributes;
						if (dupeIsActive && (!mainIsActive || IntegralOperators<TInt>.GreaterThan(mainPair.First, dupePair.Value.Value)))
						{
							value = dupePair.Value.Value;
							name = dupePair.Key;
							attributes = dupePair.Value.Attributes;
							if (dupeEnumerator.MoveNext())
							{
								dupePair = dupeEnumerator.Current;
							}
							else
							{
								dupeIsActive = false;
							}
						}
						else
						{
							value = mainPair.First;
							name = mainPair.Second.Name;
							attributes = mainPair.Second.Attributes;
							if (mainEnumerator.MoveNext())
							{
								mainPair = mainEnumerator.Current;
							}
							else
							{
								mainIsActive = false;
							}
						}
						yield return new InternalEnumMemberInfo<TEnum>(_toEnum(value), name, attributes);
					}
				}
			}
		}
		#endregion

		#region IsValid
		public bool IsValid(object value)
		{
			TEnum result;
			return TryToObject(value, out result);
		}

		public bool IsValid(TEnum value) => IsFlagEnum ? IsValidFlagCombination(value) : IsDefined(value);

		public bool IsValid(long value) => IsInValueRange(value) && IsValid(InternalToObject(value));

		public bool IsValid(ulong value) => IsInValueRange(value) && IsValid(InternalToObject(value));
		#endregion

		#region IsDefined
		public bool IsDefined(object value)
		{
			TEnum result;
			return TryToObject(value, out result, false) && IsDefined(result);
		}

		public bool IsDefined(TEnum value)
		{
			var valueAsInt = _toInt(value);
			return IsContiguous ? !(IntegralOperators<TInt>.GreaterThan(MinDefined, valueAsInt) || IntegralOperators<TInt>.GreaterThan(valueAsInt, MaxDefined)) : _valueMap.ContainsFirst(valueAsInt);
		}

		public bool IsDefined(string name, bool ignoreCase = false)
		{
			Preconditions.NotNull(name, nameof(name));

			return _valueMap.ContainsSecond(new NameAndAttributes(name)) || (_duplicateValues?.ContainsKey(name) ?? false) || (ignoreCase && IgnoreCaseSet.ContainsKey(name));
		}

		public bool IsDefined(long value) => IsInValueRange(value) && IsDefined(InternalToObject(value));

		public bool IsDefined(ulong value) => IsInValueRange(value) && IsDefined(InternalToObject(value));
		#endregion

		#region IsInValueRange
		public bool IsInValueRange(long value) => IntegralOperators<TInt>.Int64IsInValueRange(value);

		public bool IsInValueRange(ulong value) => IntegralOperators<TInt>.UInt64IsInValueRange(value);
		#endregion

		#region ToObject
		public TEnum ToObject(object value, bool validate = true)
		{
			Preconditions.NotNull(value, nameof(value));

			if (value is TEnum)
			{
				var result = (TEnum)value;
				if (validate)
				{
					Validate(result, nameof(value));
				}
				return result;
			}

			switch (Type.GetTypeCode(value.GetType()))
			{
				case TypeCode.SByte:
					return ToObject((sbyte)value, validate);
				case TypeCode.Byte:
					return ToObject((byte)value, validate);
				case TypeCode.Int16:
					return ToObject((short)value, validate);
				case TypeCode.UInt16:
					return ToObject((ushort)value, validate);
				case TypeCode.Int32:
					return ToObject((int)value, validate);
				case TypeCode.UInt32:
					return ToObject((uint)value, validate);
				case TypeCode.Int64:
					return ToObject((long)value, validate);
				case TypeCode.UInt64:
					return ToObject((ulong)value, validate);
				case TypeCode.String:
					var result = Parse((string)value);
					if (validate)
					{
						Validate(result, nameof(value));
					}
					return result;
			}
			throw new ArgumentException($"value is not type {typeof(TEnum).Name}, SByte, Int16, Int32, Int64, Byte, UInt16, UInt32, UInt64, or String.");
		}

		public TEnum ToObject(long value, bool validate = true)
		{
			if (!IsInValueRange(value))
			{
				throw Enums.GetOverflowException();
			}

			var result = InternalToObject(value);
			if (validate)
			{
				Validate(result, nameof(value));
			}
			return result;
		}

		public TEnum ToObject(ulong value, bool validate = true)
		{
			if (!IsInValueRange(value))
			{
				throw Enums.GetOverflowException();
			}

			var result = InternalToObject(value);
			if (validate)
			{
				Validate(result, nameof(value));
			}
			return result;
		}

		public TEnum ToObjectOrDefault(object value, TEnum defaultEnum, bool validate = true)
		{
			TEnum result;
			if (!TryToObject(value, out result, validate))
			{
				result = defaultEnum;
			}
			return result;
		}

		public TEnum ToObjectOrDefault(long value, TEnum defaultEnum, bool validate = true)
		{
			TEnum result;
			if (!TryToObject(value, out result, validate))
			{
				result = defaultEnum;
			}
			return result;
		}

		public TEnum ToObjectOrDefault(ulong value, TEnum defaultEnum, bool validate = true)
		{
			TEnum result;
			if (!TryToObject(value, out result, validate))
			{
				result = defaultEnum;
			}
			return result;
		}

		public bool TryToObject(object value, out TEnum result, bool validate = true)
		{
			Preconditions.NotNull(value, nameof(value));

			if (value is TEnum)
			{
				result = (TEnum)value;
				return true;
			}

			switch (Type.GetTypeCode(value.GetType()))
			{
				case TypeCode.SByte:
					return TryToObject((sbyte)value, out result, validate);
				case TypeCode.Byte:
					return TryToObject((byte)value, out result, validate);
				case TypeCode.Int16:
					return TryToObject((short)value, out result, validate);
				case TypeCode.UInt16:
					return TryToObject((ushort)value, out result, validate);
				case TypeCode.Int32:
					return TryToObject((int)value, out result, validate);
				case TypeCode.UInt32:
					return TryToObject((uint)value, out result, validate);
				case TypeCode.Int64:
					return TryToObject((long)value, out result, validate);
				case TypeCode.UInt64:
					return TryToObject((ulong)value, out result, validate);
				case TypeCode.String:
					if (TryParse((string)value, out result))
					{
						if (!validate || IsValid(result))
						{
							return true;
						}
					}
					break;
			}
			result = default(TEnum);
			return false;
		}

		public bool TryToObject(long value, out TEnum result, bool validate = true)
		{
			if (IsInValueRange(value))
			{
				result = InternalToObject(value);
				if (!validate || IsValid(result))
				{
					return true;
				}
			}
			result = default(TEnum);
			return false;
		}

		public bool TryToObject(ulong value, out TEnum result, bool validate = true)
		{
			if (IsInValueRange(value))
			{
				result = InternalToObject(value);
				if (!validate || IsValid(result))
				{
					return true;
				}
			}
			result = default(TEnum);
			return false;
		}

		private TEnum InternalToObject(long value) => _toEnum(IntegralOperators<TInt>.FromInt64(value));

		private TEnum InternalToObject(ulong value) => _toEnum(IntegralOperators<TInt>.FromUInt64(value));
		#endregion

		#region All Values Main Methods
		public TEnum Validate(TEnum value, string paramName)
		{
			if (!IsValid(value))
			{
				throw new ArgumentException($"invalid value of {AsString(value)} for {typeof(TEnum).Name}", paramName);
			}
			return value;
		}

		public string AsString(TEnum value)
		{
			if (IsFlagEnum)
			{
				var str = FormatAsFlags(value);
				if (str != null)
				{
					return str;
				}
			}
			return Format(value, Enums.DefaultParseFormatOrder);
		}

		public string AsString(TEnum value, EnumFormat[] formats) => formats?.Length > 0 ? InternalFormat(value, formats) : AsString(value);

		public string AsString(TEnum value, string format) => string.IsNullOrEmpty(format) ? AsString(value) : Format(value, format);

		public string Format(TEnum value, EnumFormat format) => InternalFormat(value, GetInternalEnumMemberInfo(value), format);

		public string Format(TEnum value, EnumFormat format0, EnumFormat format1) => InternalFormat(value, GetInternalEnumMemberInfo(value), format0, format1);

		public string Format(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => InternalFormat(value, GetInternalEnumMemberInfo(value), format0, format1, format2);

		public string Format(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3) => InternalFormat(value, GetInternalEnumMemberInfo(value), format0, format1, format2, format3);

		public string Format(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4) => InternalFormat(value, GetInternalEnumMemberInfo(value), format0, format1, format2, format3, format4);

		public string Format(TEnum value, EnumFormat[] formats)
		{
			Preconditions.NotNullOrEmpty(formats, nameof(formats));

			return InternalFormat(value, GetInternalEnumMemberInfo(value), formats);
		}

		public string Format(TEnum value, string format)
		{
			Preconditions.NotNull(format, nameof(format));

			switch (format)
			{
				case "G":
				case "g":
					return AsString(value);
				case "F":
				case "f":
					return FormatAsFlags(value) ?? AsString(value);
				case "D":
				case "d":
					return ToDecimalString(value);
				case "X":
				case "x":
					return ToHexadecimalString(value);
			}
			throw new FormatException("format string can be only \"G\", \"g\", \"X\", \"x\", \"F\", \"f\", \"D\" or \"d\".");
		}

		public string InternalFormat(IEnumMemberInfo<TEnum> info, string format)
		{
			switch (format)
			{
				case "G":
				case "g":
				case "F":
				case "f":
					return InternalFormat(info.Value, info, Enums.DefaultParseFormatOrder);
				case "D":
				case "d":
					return ToDecimalString(info.Value);
				case "X":
				case "x":
					return ToHexadecimalString(info.Value);
			}
			throw new FormatException("format string can be only \"G\", \"g\", \"X\", \"x\", \"F\", \"f\", \"D\" or \"d\".");
		}

		private string InternalFormat(TEnum value, EnumFormat[] formats)
		{
			return InternalFormat(value, GetInternalEnumMemberInfo(value), formats);
		}

		public string InternalFormat(TEnum value, IEnumMemberInfo<TEnum> info, EnumFormat format)
		{
			switch (format)
			{
				case EnumFormat.DecimalValue:
					return ToDecimalString(value);
				case EnumFormat.HexadecimalValue:
					return ToHexadecimalString(value);
				case EnumFormat.Name:
					return info.Name;
				case EnumFormat.Description:
					return info.Description;
				default:
					var index = Enums.ToInt32(format) - Enums.StartingCustomEnumFormatValue;
					if (index >= 0 && index < Enums.CustomEnumFormatters?.Count)
					{
						return Enums.CustomEnumFormatters[index](info);
					}
					else
					{
						index -= Enums.StartingGenericCustomEnumFormatValue - Enums.StartingCustomEnumFormatValue;
						if (index >= 0 && index < _customEnumFormatters?.Count)
						{
							return _customEnumFormatters[index](info);
						}
					}
					return null;
			}
		}

		public string InternalFormat(TEnum value, IEnumMemberInfo<TEnum> info, EnumFormat format0, EnumFormat format1)
		{
			return InternalFormat(value, info, format0) ?? InternalFormat(value, info, format1);
		}

		public string InternalFormat(TEnum value, IEnumMemberInfo<TEnum> info, EnumFormat format0, EnumFormat format1, EnumFormat format2)
		{
			return InternalFormat(value, info, format0) ?? InternalFormat(value, info, format1) ?? InternalFormat(value, info, format2);
		}

		public string InternalFormat(TEnum value, IEnumMemberInfo<TEnum> info, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3)
		{
			return InternalFormat(value, info, format0) ?? InternalFormat(value, info, format1) ?? InternalFormat(value, info, format2) ?? InternalFormat(value, info, format3);
		}

		public string InternalFormat(TEnum value, IEnumMemberInfo<TEnum> info, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4)
		{
			return InternalFormat(value, info, format0) ?? InternalFormat(value, info, format1) ?? InternalFormat(value, info, format2) ?? InternalFormat(value, info, format3) ?? InternalFormat(value, info, format4);
		}

		public string InternalFormat(TEnum value, IEnumMemberInfo<TEnum> info, EnumFormat[] formats)
		{
			foreach (var format in formats)
			{
				var formattedValue = InternalFormat(value, info, format);
				if (formattedValue != null)
				{
					return formattedValue;
				}
			}
			return null;
		}

		private string ToDecimalString(TEnum value) => IntegralOperators<TInt>.ToString(_toInt(value), "D");

		private string ToHexadecimalString(TEnum value) => IntegralOperators<TInt>.ToString(_toInt(value), IntegralOperators<TInt>.HexFormatString);

		public object GetUnderlyingValue(TEnum value) => _toInt(value);

		public sbyte ToSByte(TEnum value) => IntegralOperators<TInt>.ToSByte(_toInt(value));

		public byte ToByte(TEnum value) => IntegralOperators<TInt>.ToByte(_toInt(value));

		public short ToInt16(TEnum value) => IntegralOperators<TInt>.ToInt16(_toInt(value));

		public ushort ToUInt16(TEnum value) => IntegralOperators<TInt>.ToUInt16(_toInt(value));

		public int ToInt32(TEnum value) => IntegralOperators<TInt>.ToInt32(_toInt(value));

		public uint ToUInt32(TEnum value) => IntegralOperators<TInt>.ToUInt32(_toInt(value));

		public long ToInt64(TEnum value) => IntegralOperators<TInt>.ToInt64(_toInt(value));

		public ulong ToUInt64(TEnum value) => IntegralOperators<TInt>.ToUInt64(_toInt(value));
		#endregion

		#region Defined Values Main Methods
		public EnumMemberInfo<TEnum> GetEnumMemberInfo(TEnum value)
		{
			return GetInternalEnumMemberInfo(value).ToEnumMemberInfo();
		}

		public EnumMemberInfo<TEnum> GetEnumMemberInfo(string name, bool ignoreCase = false)
		{
			return GetInternalEnumMemberInfo(name, ignoreCase).ToEnumMemberInfo();
		}

		public string GetName(TEnum value)
		{
			return GetInternalEnumMemberInfo(value).Name;
		}

		public string GetDescription(TEnum value)
		{
			return GetInternalEnumMemberInfo(value).Description;
		}

		public string GetDescriptionOrName(TEnum value)
		{
			return GetInternalEnumMemberInfo(value).GetDescriptionOrName();
		}

		public string GetDescriptionOrName(TEnum value, Func<string, string> nameFormatter)
		{
			return GetInternalEnumMemberInfo(value).GetDescriptionOrName(nameFormatter);
		}

		private InternalEnumMemberInfo<TEnum> GetInternalEnumMemberInfo(TEnum value)
		{
			var index = _valueMap.IndexOfFirst(_toInt(value));
			if (index >= 0)
			{
				var nameAndAttributes = _valueMap.GetSecondAt(index);
				return new InternalEnumMemberInfo<TEnum>(value, nameAndAttributes.Name, nameAndAttributes.Attributes);
			}
			else
			{
				return new InternalEnumMemberInfo<TEnum>(value, null, null);
			}
		}

		private InternalEnumMemberInfo<TEnum> GetInternalEnumMemberInfo(string name, bool ignoreCase)
		{
			Preconditions.NotNull(name, nameof(name));

			var index = _valueMap.IndexOfSecond(new NameAndAttributes(name));
			if (index >= 0)
			{
				var pair = _valueMap.GetAt(index);
				return new InternalEnumMemberInfo<TEnum>(_toEnum(pair.First), pair.Second.Name, pair.Second.Attributes);
			}
			ValueAndAttributes<TInt> valueAndAttributes;
			if (_duplicateValues != null && _duplicateValues.TryGetValue(name, out valueAndAttributes))
			{
				return new InternalEnumMemberInfo<TEnum>(_toEnum(valueAndAttributes.Value), name, valueAndAttributes.Attributes);
			}
			if (ignoreCase)
			{
				string actualName;
				if (IgnoreCaseSet.TryGetValue(name, out actualName))
				{
					index = _valueMap.IndexOfSecond(new NameAndAttributes(actualName));
					if (index >= 0)
					{
						var pair = _valueMap.GetAt(index);
						return new InternalEnumMemberInfo<TEnum>(_toEnum(pair.First), pair.Second.Name, pair.Second.Attributes);
					}
					valueAndAttributes = _duplicateValues[actualName];
					return new InternalEnumMemberInfo<TEnum>(_toEnum(valueAndAttributes.Value), actualName, valueAndAttributes.Attributes);
				}
			}
			return new InternalEnumMemberInfo<TEnum>();
		}
		#endregion

		#region Attributes
		public bool HasAttribute<TAttribute>(TEnum value)
			where TAttribute : Attribute
		{
			return GetInternalEnumMemberInfo(value).HasAttribute<TAttribute>();
		}

		public TAttribute GetAttribute<TAttribute>(TEnum value)
			where TAttribute : Attribute
		{
			return GetInternalEnumMemberInfo(value).GetAttribute<TAttribute>();
		}

		public TResult GetAttributeSelect<TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, TResult defaultValue)
			where TAttribute : Attribute
		{
			return GetInternalEnumMemberInfo(value).GetAttributeSelect(selector, defaultValue);
		}

		public bool TryGetAttributeSelect<TAttribute, TResult>(TEnum value, Func<TAttribute, TResult> selector, out TResult result)
			where TAttribute : Attribute
		{
			return GetInternalEnumMemberInfo(value).TryGetAttributeSelect(selector, out result);
		}

		public IEnumerable<TAttribute> GetAttributes<TAttribute>(TEnum value)
			where TAttribute : Attribute
		{
			return GetInternalEnumMemberInfo(value).GetAttributes<TAttribute>();
		}

		public Attribute[] GetAllAttributes(TEnum value)
		{
			return GetInternalEnumMemberInfo(value).Attributes;
		}
		#endregion

		#region Parsing
		public TEnum Parse(string value) => Parse(value, false, null);

		public TEnum Parse(string value, EnumFormat[] parseFormatOrder) => Parse(value, false, parseFormatOrder);

		public TEnum Parse(string value, bool ignoreCase) => Parse(value, ignoreCase, null);

		public TEnum Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder)
		{
			Preconditions.NotNull(value, nameof(value));

			value = value.Trim();
			TInt result;
			if (IsFlagEnum)
			{
				return IntegralOperators<TInt>.TryParse(value, NumberStyles.AllowLeadingSign, null, out result) ? _toEnum(result) : ParseFlags(value, ignoreCase, FlagEnums.DefaultDelimiter, parseFormatOrder);
			}

			if (!(parseFormatOrder?.Length > 0))
			{
				parseFormatOrder = Enums.DefaultParseFormatOrder;
			}

			if (InternalTryParse(value, ignoreCase, out result, parseFormatOrder))
			{
				return _toEnum(result);
			}
			if (Enums.IsNumeric(value))
			{
				throw Enums.GetOverflowException();
			}
			throw new ArgumentException($"string was not recognized as being a member of {typeof(TEnum).Name}", nameof(value));
		}

		public TEnum ParseOrDefault(string value, TEnum defaultEnum) => ParseOrDefault(value, false, defaultEnum, null);

		public TEnum ParseOrDefault(string value, TEnum defaultEnum, EnumFormat[] parseFormatOrder) => ParseOrDefault(value, false, defaultEnum, parseFormatOrder);

		public TEnum ParseOrDefault(string value, bool ignoreCase, TEnum defaultEnum) => ParseOrDefault(value, ignoreCase, defaultEnum, null);

		public TEnum ParseOrDefault(string value, bool ignoreCase, TEnum defaultEnum, EnumFormat[] parseFormatOrder)
		{
			TEnum result;
			if (!TryParse(value, ignoreCase, out result, parseFormatOrder))
			{
				result = defaultEnum;
			}
			return result;
		}

		public bool TryParse(string value, out TEnum result) => TryParse(value, false, out result, null);

		public bool TryParse(string value, out TEnum result, EnumFormat[] parseFormatOrder) => TryParse(value, false, out result, parseFormatOrder);

		public bool TryParse(string value, bool ignoreCase, out TEnum result) => TryParse(value, ignoreCase, out result, null);

		public bool TryParse(string value, bool ignoreCase, out TEnum result, EnumFormat[] parseFormatOrder)
		{
			if (value != null)
			{
				TInt resultAsInt;
				value = value.Trim();
				if (IsFlagEnum)
				{
					if (IntegralOperators<TInt>.TryParse(value, NumberStyles.AllowLeadingSign, null, out resultAsInt) || TryParseFlags(value, ignoreCase, FlagEnums.DefaultDelimiter, out result, parseFormatOrder))
					{
						result = _toEnum(resultAsInt);
						return true;
					}
					return false;
				}

				if (!(parseFormatOrder?.Length > 0))
				{
					parseFormatOrder = Enums.DefaultParseFormatOrder;
				}

				if (InternalTryParse(value, ignoreCase, out resultAsInt, parseFormatOrder))
				{
					result = _toEnum(resultAsInt);
					return true;
				}
			}
			result = default(TEnum);
			return false;
		}

		private bool InternalTryParse(string value, bool ignoreCase, out TInt result, EnumFormat[] parseFormatOrder)
		{
			foreach (var format in parseFormatOrder)
			{
				switch (format)
				{
					case EnumFormat.DecimalValue:
					case EnumFormat.HexadecimalValue:
						if (IntegralOperators<TInt>.TryParse(value, format == EnumFormat.DecimalValue ? NumberStyles.AllowLeadingSign : NumberStyles.AllowHexSpecifier, null, out result))
						{
							return true;
						}
						break;
					case EnumFormat.Name:
						if (TryParseName(value, ignoreCase, out result))
						{
							return true;
						}
						break;
					default:
						EnumParser parser = null;
						if (_customEnumFormatParsers?.TryGetValue(format, out parser) != true)
						{
							switch (format)
							{
								case EnumFormat.Description:
									parser = new EnumParser(Enums.DescriptionEnumFormatter);
									break;
								default:
									var index = Enums.ToInt32(format) - Enums.StartingCustomEnumFormatValue;
									if (index >= 0 && index < Enums.CustomEnumFormatters?.Count)
									{
										parser = new EnumParser(Enums.CustomEnumFormatters[index]);
									}
									else
									{
										index -= Enums.StartingGenericCustomEnumFormatValue - Enums.StartingCustomEnumFormatValue;
										if (index >= 0 && index < _customEnumFormatters?.Count)
										{
											parser = new EnumParser(_customEnumFormatters[index]);
										}
									}
									break;
							}
							if (parser != null)
							{
								if (_customEnumFormatParsers == null)
								{
									Interlocked.CompareExchange(ref _customEnumFormatParsers, new ConcurrentDictionary<EnumFormat, EnumParser>(), null);
								}
								_customEnumFormatParsers.TryAdd(format, parser);
							}
						}
						if (parser != null && parser.TryParse(value, ignoreCase, out result))
						{
							return true;
						}
						break;
				}
			}
			result = default(TInt);
			return false;
		}

		private bool TryParseName(string value, bool ignoreCase, out TInt result)
		{
			if (_valueMap.TryGetFirst(new NameAndAttributes(value), out result))
			{
				return true;
			}
			ValueAndAttributes<TInt> valueAndAttributes;
			if (_duplicateValues != null && _duplicateValues.TryGetValue(value, out valueAndAttributes))
			{
				result = valueAndAttributes.Value;
				return true;
			}
			if (ignoreCase)
			{
				string name;
				if (IgnoreCaseSet.TryGetValue(value, out name))
				{
					if (!_valueMap.TryGetFirst(new NameAndAttributes(name), out result))
					{
						result = _duplicateValues[name].Value;
					}
					return true;
				}
			}
			result = default(TInt);
			return false;
		}
		#endregion
		#endregion

		#region Flag Enum Operations
		#region Main Methods
		public bool IsValidFlagCombination(TEnum value) => IsValidFlagCombination(_toInt(value));

		private bool IsValidFlagCombination(TInt value)
		{
			return IntegralOperators<TInt>.Equals(IntegralOperators<TInt>.And(_allFlags, value), value);
		}

		public string FormatAsFlags(TEnum value) => FormatAsFlags(value, FlagEnums.DefaultDelimiter, Enums.DefaultParseFormatOrder);

		public string FormatAsFlags(TEnum value, string delimiter) => FormatAsFlags(value, delimiter, Enums.DefaultParseFormatOrder);

		public string FormatAsFlags(TEnum value, EnumFormat[] formats) => FormatAsFlags(value, FlagEnums.DefaultDelimiter, formats);

		public string FormatAsFlags(TEnum value, string delimiter = FlagEnums.DefaultDelimiter, EnumFormat[] formats = null)
		{
			Preconditions.NotNullOrEmpty(delimiter, nameof(delimiter));

			var valueAsInt = _toInt(value);

			if (!IsValidFlagCombination(valueAsInt))
			{
				return null;
			}

			if (!(formats?.Length > 0))
			{
				formats = Enums.DefaultParseFormatOrder;
			}

			List<TInt> flags;
			var info = GetInternalEnumMemberInfo(value);
			if (info.IsDefined || (flags = InternalGetFlags(valueAsInt)).Count == 0)
			{
				return InternalFormat(value, info, formats);
			}

			return string.Join(delimiter, flags.Select(flag => InternalFormat(_toEnum(flag), formats)));
		}

		public TEnum[] GetFlags(TEnum value)
		{
			var valueAsInt = _toInt(value);
			return IsValidFlagCombination(valueAsInt) ? InternalGetFlags(valueAsInt).Select(v => _toEnum(v)).ToArray() : null;
		}

		public bool HasAnyFlags(TEnum value)
		{
			var valueAsInt = ValidateIsValidFlagCombination(value, nameof(value));
			return !IntegralOperators<TInt>.Equals(valueAsInt, default(TInt));
		}

		public bool HasAnyFlags(TEnum value, TEnum flagMask)
		{
			var valueAsInt = ValidateIsValidFlagCombination(value, nameof(value));
			var flagMaskAsInt = ValidateIsValidFlagCombination(flagMask, nameof(flagMask));
			return InternalHasAnyFlags(valueAsInt, flagMaskAsInt);
		}

		public bool HasAllFlags(TEnum value)
		{
			var valueAsInt = ValidateIsValidFlagCombination(value, nameof(value));
			return IntegralOperators<TInt>.Equals(valueAsInt, _allFlags);
		}

		public bool HasAllFlags(TEnum value, TEnum flagMask)
		{
			var valueAsInt = ValidateIsValidFlagCombination(value, nameof(value));
			var flagMaskAsInt = ValidateIsValidFlagCombination(flagMask, nameof(flagMask));
			return IntegralOperators<TInt>.Equals(IntegralOperators<TInt>.And(valueAsInt, flagMaskAsInt), flagMaskAsInt);
		}

		public TEnum InvertFlags(TEnum value)
		{
			var valueAsInt = ValidateIsValidFlagCombination(value, nameof(value));
			return _toEnum(IntegralOperators<TInt>.Xor(valueAsInt, _allFlags));
		}

		public TEnum InvertFlags(TEnum value, TEnum flagMask)
		{
			var valueAsInt = ValidateIsValidFlagCombination(value, nameof(value));
			var flagMaskAsInt = ValidateIsValidFlagCombination(flagMask, nameof(flagMask));
			return _toEnum(IntegralOperators<TInt>.Xor(valueAsInt, flagMaskAsInt));
		}

		public TEnum CommonFlags(TEnum value, TEnum flagMask)
		{
			var valueAsInt = ValidateIsValidFlagCombination(value, nameof(value));
			var flagMaskAsInt = ValidateIsValidFlagCombination(flagMask, nameof(flagMask));
			return _toEnum(IntegralOperators<TInt>.And(valueAsInt, flagMaskAsInt));
		}

		public TEnum SetFlags(TEnum flag0, TEnum flag1)
		{
			var flag0AsInt = ValidateIsValidFlagCombination(flag0, nameof(flag0));
			var flag1AsInt = ValidateIsValidFlagCombination(flag1, nameof(flag1));
			return _toEnum(IntegralOperators<TInt>.Or(flag0AsInt, flag1AsInt));
		}

		public TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2)
		{
			var flag0AsInt = ValidateIsValidFlagCombination(flag0, nameof(flag0));
			var flag1AsInt = ValidateIsValidFlagCombination(flag1, nameof(flag1));
			var flag2AsInt = ValidateIsValidFlagCombination(flag2, nameof(flag2));

			var or = IntegralOperators<TInt>.Or;
			return _toEnum(or(or(flag0AsInt, flag1AsInt), flag2AsInt));
		}

		public TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3)
		{
			var flag0AsInt = ValidateIsValidFlagCombination(flag0, nameof(flag0));
			var flag1AsInt = ValidateIsValidFlagCombination(flag1, nameof(flag1));
			var flag2AsInt = ValidateIsValidFlagCombination(flag2, nameof(flag2));
			var flag3AsInt = ValidateIsValidFlagCombination(flag3, nameof(flag3));

			var or = IntegralOperators<TInt>.Or;
			return _toEnum(or(or(or(flag0AsInt, flag1AsInt), flag2AsInt), flag3AsInt));
		}

		public TEnum SetFlags(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4)
		{
			var flag0AsInt = ValidateIsValidFlagCombination(flag0, nameof(flag0));
			var flag1AsInt = ValidateIsValidFlagCombination(flag1, nameof(flag1));
			var flag2AsInt = ValidateIsValidFlagCombination(flag2, nameof(flag2));
			var flag3AsInt = ValidateIsValidFlagCombination(flag3, nameof(flag3));
			var flag4AsInt = ValidateIsValidFlagCombination(flag4, nameof(flag4));

			var or = IntegralOperators<TInt>.Or;
			return _toEnum(or(or(or(or(flag0AsInt, flag1AsInt), flag2AsInt), flag3AsInt), flag4AsInt));
		}

		public TEnum SetFlags(TEnum[] flags)
		{
			var flag = default(TInt);
			var flagsLength = flags?.Length ?? 0;
			var or = IntegralOperators<TInt>.Or;
			for (var i = 0; i < flagsLength; ++i)
			{
				var nextFlag = ValidateIsValidFlagCombination(flags[i], nameof(flags) + " must contain all valid flag combinations");
				flag = or(flag, nextFlag);
			}
			return _toEnum(flag);
		}

		public TEnum ClearFlags(TEnum value, TEnum flagMask)
		{
			var valueAsInt = ValidateIsValidFlagCombination(value, nameof(value));
			var flagMaskAsInt = ValidateIsValidFlagCombination(flagMask, nameof(flagMask));
			return _toEnum(IntegralOperators<TInt>.And(valueAsInt, IntegralOperators<TInt>.Xor(flagMaskAsInt, _allFlags)));
		}
		#endregion

		#region Parsing
		public TEnum ParseFlags(string value) => ParseFlags(value, false, FlagEnums.DefaultDelimiter, null);

		public TEnum ParseFlags(string value, EnumFormat[] parseFormatOrder) => ParseFlags(value, false, FlagEnums.DefaultDelimiter, parseFormatOrder);

		public TEnum ParseFlags(string value, bool ignoreCase) => ParseFlags(value, ignoreCase, FlagEnums.DefaultDelimiter, null);

		public TEnum ParseFlags(string value, bool ignoreCase, EnumFormat[] parseFormatOrder) => ParseFlags(value, ignoreCase, FlagEnums.DefaultDelimiter, parseFormatOrder);

		public TEnum ParseFlags(string value, string delimiter) => ParseFlags(value, false, delimiter, null);

		public TEnum ParseFlags(string value, string delimiter, EnumFormat[] parseFormatOrder) => ParseFlags(value, false, delimiter, parseFormatOrder);

		public TEnum ParseFlags(string value, bool ignoreCase, string delimiter) => ParseFlags(value, ignoreCase, delimiter, null);

		public TEnum ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder)
		{
			Preconditions.NotNull(value, nameof(value));
			Preconditions.NotNullOrEmpty(delimiter, nameof(delimiter));

			var effectiveDelimiter = delimiter.Trim();
			if (effectiveDelimiter.Length == 0)
			{
				effectiveDelimiter = delimiter;
			}
			var split = value.Split(new[] { effectiveDelimiter }, StringSplitOptions.None);

			if (!(parseFormatOrder?.Length > 0))
			{
				parseFormatOrder = Enums.DefaultParseFormatOrder;
			}

			var result = default(TInt);
			var or = IntegralOperators<TInt>.Or;
			foreach (var indValue in split)
			{
				var trimmedIndValue = indValue.Trim();
				TInt indValueAsTEnum;
				if (InternalTryParse(trimmedIndValue, ignoreCase, out indValueAsTEnum, parseFormatOrder))
				{
					if (!IsValidFlagCombination(indValueAsTEnum))
					{
						throw new ArgumentException("All individual enum values within value must be valid");
					}
					result = or(result, indValueAsTEnum);
				}
				else
				{
					if (Enums.IsNumeric(indValue))
					{
						throw Enums.GetOverflowException();
					}
					throw new ArgumentException("value is not a valid combination of flag enum values");
				}
			}
			return _toEnum(result);
		}

		public TEnum ParseFlagsOrDefault(string value, TEnum defaultEnum) => ParseFlagsOrDefault(value, false, FlagEnums.DefaultDelimiter, defaultEnum, null);

		public TEnum ParseFlagsOrDefault(string value, TEnum defaultEnum, EnumFormat[] parseFormatOrder) => ParseFlagsOrDefault(value, false, FlagEnums.DefaultDelimiter, defaultEnum, parseFormatOrder);

		public TEnum ParseFlagsOrDefault(string value, bool ignoreCase, TEnum defaultEnum) => ParseFlagsOrDefault(value, ignoreCase, FlagEnums.DefaultDelimiter, defaultEnum, null);

		public TEnum ParseFlagsOrDefault(string value, bool ignoreCase, TEnum defaultEnum, EnumFormat[] parseFormatOrder) => ParseFlagsOrDefault(value, ignoreCase, FlagEnums.DefaultDelimiter, defaultEnum, parseFormatOrder);

		public TEnum ParseFlagsOrDefault(string value, string delimiter, TEnum defaultEnum) => ParseFlagsOrDefault(value, false, delimiter, defaultEnum, null);

		public TEnum ParseFlagsOrDefault(string value, string delimiter, TEnum defaultEnum, EnumFormat[] parseFormatOrder) => ParseFlagsOrDefault(value, false, delimiter, defaultEnum, parseFormatOrder);

		public TEnum ParseFlagsOrDefault(string value, bool ignoreCase, string delimiter, TEnum defaultEnum) => ParseFlagsOrDefault(value, ignoreCase, delimiter, defaultEnum, null);

		public TEnum ParseFlagsOrDefault(string value, bool ignoreCase, string delimiter, TEnum defaultEnum, EnumFormat[] parseFormatOrder)
		{
			ValidateIsValidFlagCombination(defaultEnum, nameof(defaultEnum));

			TEnum enumValue;
			if (!TryParseFlags(value, ignoreCase, delimiter, out enumValue, parseFormatOrder))
			{
				enumValue = defaultEnum;
			}
			return enumValue;
		}

		public bool TryParseFlags(string value, out TEnum result) => TryParseFlags(value, false, FlagEnums.DefaultDelimiter, out result, null);

		public bool TryParseFlags(string value, out TEnum result, EnumFormat[] parseFormatOrder) => TryParseFlags(value, false, FlagEnums.DefaultDelimiter, out result, parseFormatOrder);

		public bool TryParseFlags(string value, bool ignoreCase, out TEnum result) => TryParseFlags(value, ignoreCase, FlagEnums.DefaultDelimiter, out result, null);

		public bool TryParseFlags(string value, bool ignoreCase, out TEnum result, EnumFormat[] parseFormatOrder) => TryParseFlags(value, ignoreCase, FlagEnums.DefaultDelimiter, out result, parseFormatOrder);

		public bool TryParseFlags(string value, string delimiter, out TEnum result) => TryParseFlags(value, false, delimiter, out result, null);

		public bool TryParseFlags(string value, string delimiter, out TEnum result, EnumFormat[] parseFormatOrder) => TryParseFlags(value, false, delimiter, out result, parseFormatOrder);

		public bool TryParseFlags(string value, bool ignoreCase, string delimiter, out TEnum result) => TryParseFlags(value, ignoreCase, delimiter, out result, null);

		public bool TryParseFlags(string value, bool ignoreCase, string delimiter, out TEnum result, EnumFormat[] parseFormatOrder)
		{
			Preconditions.NotNullOrEmpty(delimiter, nameof(delimiter));

			if (value == null)
			{
				result = default(TEnum);
				return false;
			}

			var effectiveDelimiter = delimiter.Trim();
			if (effectiveDelimiter.Length == 0)
			{
				effectiveDelimiter = delimiter;
			}
			var split = value.Split(new[] { effectiveDelimiter }, StringSplitOptions.None);

			if (!(parseFormatOrder?.Length > 0))
			{
				parseFormatOrder = Enums.DefaultParseFormatOrder;
			}

			var resultAsInt = default(TInt);
			var or = IntegralOperators<TInt>.Or;
			foreach (var indValue in split)
			{
				var trimmedIndValue = indValue.Trim();
				TInt indValueAsTEnum;
				if (!InternalTryParse(trimmedIndValue, ignoreCase, out indValueAsTEnum, parseFormatOrder) || !IsValidFlagCombination(indValueAsTEnum))
				{
					result = default(TEnum);
					return false;
				}
				resultAsInt = or(resultAsInt, indValueAsTEnum);
			}
			result = _toEnum(resultAsInt);
			return true;
		}
		#endregion

		#region Private Methods
		private List<TInt> InternalGetFlags(TInt value)
		{
			var valueAsULong = TypeCode == TypeCode.UInt64 ? IntegralOperators<TInt>.ToUInt64(value) : (ulong)IntegralOperators<TInt>.ToInt64(value);
			var values = new List<TInt>();
			for (var currentValue = 1UL; currentValue <= valueAsULong && currentValue != 0UL; currentValue <<= 1)
			{
				var currentValueAsTEnum = IntegralOperators<TInt>.FromUInt64(currentValue);
				if (IsValidFlagCombination(currentValueAsTEnum) && InternalHasAnyFlags(value, currentValueAsTEnum))
				{
					values.Add(currentValueAsTEnum);
				}
			}
			return values;
		}

		private bool InternalHasAnyFlags(TInt value, TInt flagMask)
		{
			return !IntegralOperators<TInt>.Equals(IntegralOperators<TInt>.And(value, flagMask), default(TInt));
		}

		private TInt ValidateIsValidFlagCombination(TEnum value, string paramName)
		{
			var valueAsInt = _toInt(value);
			if (!IsValidFlagCombination(valueAsInt))
			{
				throw new ArgumentException("must be valid flag combination", paramName);
			}
			return valueAsInt;
		}
		#endregion
		#endregion

		#region Nested Types
		internal class EnumParser
		{
			private readonly Dictionary<string, string> _formatNameMap;
			private Dictionary<string, string> _formatIgnoreCase;

			private Dictionary<string, string> FormatIgnoreCase
			{
				get
				{
					if (_formatIgnoreCase == null)
					{
						var formatIgnoreCase = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
						foreach (var pair in _formatNameMap)
						{
							if (!formatIgnoreCase.ContainsKey(pair.Key))
							{
								formatIgnoreCase.Add(pair.Key, pair.Value);
							}
						}

						// Reduces memory usage
						_formatIgnoreCase = new Dictionary<string, string>(formatIgnoreCase, StringComparer.OrdinalIgnoreCase);
					}
					return _formatIgnoreCase;
				}
			}

			public EnumParser(Func<IEnumMemberInfo<TEnum>, string> enumFormatter)
			{
				var formatNameMap = new Dictionary<string, string>();
				foreach (var pair in _valueMap)
				{
					var format = enumFormatter(new InternalEnumMemberInfo<TEnum>(_toEnum(pair.First), pair.Second.Name, pair.Second.Attributes));
					if (format != null && !formatNameMap.ContainsKey(format))
					{
						formatNameMap.Add(format, pair.Second.Name);
					}
				}
				if (_duplicateValues != null)
				{
					foreach (var pair in _duplicateValues)
					{
						var format = enumFormatter(new InternalEnumMemberInfo<TEnum>(_toEnum(pair.Value.Value), pair.Key, pair.Value.Attributes));
						if (format != null && !formatNameMap.ContainsKey(format))
						{
							formatNameMap.Add(format, pair.Key);
						}
					}
				}

				// Reduces memory usage
				_formatNameMap = new Dictionary<string, string>(formatNameMap);
			}

			internal bool TryParse(string format, bool ignoreCase, out TInt result)
			{
				string name;
				if (_formatNameMap.TryGetValue(format, out name) || (ignoreCase && FormatIgnoreCase.TryGetValue(format, out name)))
				{
					if (!_valueMap.TryGetFirst(new NameAndAttributes(name), out result))
					{
						result = _duplicateValues[name].Value;
					}
					return true;
				}
				result = default(TInt);
				return false;
			}
		}
		#endregion

		#region IEnumsCache Implementation
		#region Properties
		object IEnumsCache.AllFlags => AllFlags;
		#endregion

		#region Standard Enum Operations
		#region Type Methods
		IEnumerable<EnumMemberInfo> IEnumsCache.GetEnumMemberInfos(bool uniqueValued) => GetEnumMemberInfos(uniqueValued).Select(info => new EnumMemberInfo(info));

		IEnumerable<object> IEnumsCache.GetValues(bool uniqueValued) => GetValues(uniqueValued).Select(value => (object)value);

		int IEnumsCache.Compare(object x, object y) => Compare(ConvertToEnum(x, nameof(x)), ConvertToEnum(y, nameof(y)));

		bool IEnumsCache.Equals(object x, object y) => Equals(ConvertToEnum(x, nameof(x)), ConvertToEnum(y, nameof(y)));

		EnumFormat IEnumsCache.RegisterCustomEnumFormat(Func<IEnumMemberInfo, string> formatter) => RegisterCustomEnumFormat(formatter);
		#endregion

		#region ToObject
		object IEnumsCache.ToObject(object value, bool validate) => ToObject(value, validate);

		object IEnumsCache.ToObject(long value, bool validate) => ToObject(value, validate);

		object IEnumsCache.ToObject(ulong value, bool validate) => ToObject(value, validate);

		object IEnumsCache.ToObjectOrDefault(object value, object defaultEnum, bool validate)
		{
			object result;
			if (!((IEnumsCache)this).TryToObject(value, out result, validate))
			{
				result = defaultEnum;
			}
			return result;
		}

		object IEnumsCache.ToObjectOrDefault(long value, object defaultEnum, bool validate)
		{
			object result;
			if (!((IEnumsCache)this).TryToObject(value, out result, validate))
			{
				result = defaultEnum;
			}
			return result;
		}

		object IEnumsCache.ToObjectOrDefault(ulong value, object defaultEnum, bool validate)
		{
			object result;
			if (!((IEnumsCache)this).TryToObject(value, out result, validate))
			{
				result = defaultEnum;
			}
			return result;
		}

		bool IEnumsCache.TryToObject(object value, out object result, bool validate)
		{
			TEnum resultAsTEnum;
			var success = TryToObject(value, out resultAsTEnum, validate);
			result = resultAsTEnum;
			return success;
		}

		bool IEnumsCache.TryToObject(long value, out object result, bool validate)
		{
			TEnum resultAsTEnum;
			var success = TryToObject(value, out resultAsTEnum, validate);
			result = resultAsTEnum;
			return success;
		}

		bool IEnumsCache.TryToObject(ulong value, out object result, bool validate)
		{
			TEnum resultAsTEnum;
			var success = TryToObject(value, out resultAsTEnum, validate);
			result = resultAsTEnum;
			return success;
		}
		#endregion

		#region All Values Main Methods
		object IEnumsCache.Validate(object value, string paramName) => Validate(ConvertToEnum(value, nameof(value)), paramName);

		string IEnumsCache.AsString(object value) => AsString(ConvertToEnum(value, nameof(value)));

		string IEnumsCache.AsString(object value, string format) => AsString(ConvertToEnum(value, nameof(value)), format);

		string IEnumsCache.AsString(object value, EnumFormat[] formats) => AsString(ConvertToEnum(value, nameof(value)), formats);

		string IEnumsCache.Format(object value, string format) => Format(ConvertToEnum(value, nameof(value)), format);

		string IEnumsCache.Format(object value, EnumFormat format) => Format(ConvertToEnum(value, nameof(value)), format);

		string IEnumsCache.Format(object value, EnumFormat format0, EnumFormat format1) => Format(ConvertToEnum(value, nameof(value)), format0, format1);

		string IEnumsCache.Format(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Format(ConvertToEnum(value, nameof(value)), format0, format1, format2);

		string IEnumsCache.Format(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3) => Format(ConvertToEnum(value, nameof(value)), format0, format1, format2, format3);

		string IEnumsCache.Format(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4) => Format(ConvertToEnum(value, nameof(value)), format0, format1, format2, format3, format4);

		string IEnumsCache.Format(object value, EnumFormat[] formats) => Format(ConvertToEnum(value, nameof(value)), formats);

		object IEnumsCache.GetUnderlyingValue(object value) => GetUnderlyingValue(ConvertToEnum(value, nameof(value)));

		sbyte IEnumsCache.ToSByte(object value) => ToSByte(ConvertToEnum(value, nameof(value)));

		byte IEnumsCache.ToByte(object value) => ToByte(ConvertToEnum(value, nameof(value)));

		short IEnumsCache.ToInt16(object value) => ToInt16(ConvertToEnum(value, nameof(value)));

		ushort IEnumsCache.ToUInt16(object value) => ToUInt16(ConvertToEnum(value, nameof(value)));

		int IEnumsCache.ToInt32(object value) => ToInt32(ConvertToEnum(value, nameof(value)));

		uint IEnumsCache.ToUInt32(object value) => ToUInt32(ConvertToEnum(value, nameof(value)));

		long IEnumsCache.ToInt64(object value) => ToInt64(ConvertToEnum(value, nameof(value)));

		ulong IEnumsCache.ToUInt64(object value) => ToUInt64(ConvertToEnum(value, nameof(value)));
		#endregion

		#region Defined Values Main Methods
		EnumMemberInfo IEnumsCache.GetEnumMemberInfo(object value)
		{
			var info = GetEnumMemberInfo(ConvertToEnum(value, nameof(value)));
			return info != null ? new EnumMemberInfo(info) : null;
		}

		EnumMemberInfo IEnumsCache.GetEnumMemberInfo(string name, bool ignoreCase)
		{
			var info = GetEnumMemberInfo(name, ignoreCase);
			return info != null ? new EnumMemberInfo(info) : null;
		}

		string IEnumsCache.GetName(object value) => GetName(ConvertToEnum(value, nameof(value)));

		string IEnumsCache.GetDescription(object value) => GetDescription(ConvertToEnum(value, nameof(value)));

		string IEnumsCache.GetDescriptionOrName(object value) => GetDescriptionOrName(ConvertToEnum(value, nameof(value)));

		string IEnumsCache.GetDescriptionOrName(object value, Func<string, string> nameFormatter) => GetDescriptionOrName(ConvertToEnum(value, nameof(value)), nameFormatter);
		#endregion

		#region Attributes
		Attribute[] IEnumsCache.GetAllAttributes(object value) => GetAllAttributes(ConvertToEnum(value, nameof(value)));
		#endregion

		#region Parsing
		object IEnumsCache.Parse(string value) => Parse(value);

		object IEnumsCache.Parse(string value, EnumFormat[] parseFormatOrder) => Parse(value, parseFormatOrder);

		object IEnumsCache.Parse(string value, bool ignoreCase) => Parse(value, ignoreCase);

		object IEnumsCache.Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder) => Parse(value, ignoreCase, parseFormatOrder);

		object IEnumsCache.ParseOrDefault(string value, object defaultEnum)
		{
			object result;
			if (!((IEnumsCache)this).TryParse(value, out result))
			{
				result = defaultEnum;
			}
			return result;
		}

		object IEnumsCache.ParseOrDefault(string value, object defaultEnum, EnumFormat[] parseFormatOrder)
		{
			object result;
			if (!((IEnumsCache)this).TryParse(value, out result, parseFormatOrder))
			{
				result = defaultEnum;
			}
			return result;
		}

		object IEnumsCache.ParseOrDefault(string value, bool ignoreCase, object defaultEnum)
		{
			object result;
			if (!((IEnumsCache)this).TryParse(value, ignoreCase, out result))
			{
				result = defaultEnum;
			}
			return result;
		}

		object IEnumsCache.ParseOrDefault(string value, bool ignoreCase, object defaultEnum, EnumFormat[] parseFormatOrder)
		{
			object result;
			if (!((IEnumsCache)this).TryParse(value, ignoreCase, out result, parseFormatOrder))
			{
				result = defaultEnum;
			}
			return result;
		}

		bool IEnumsCache.TryParse(string value, out object result)
		{
			TEnum resultAsTEnum;
			var success = TryParse(value, out resultAsTEnum);
			result = resultAsTEnum;
			return success;
		}

		bool IEnumsCache.TryParse(string value, out object result, EnumFormat[] parseFormatOrder)
		{
			TEnum resultAsTEnum;
			var success = TryParse(value, out resultAsTEnum, parseFormatOrder);
			result = resultAsTEnum;
			return success;
		}

		bool IEnumsCache.TryParse(string value, bool ignoreCase, out object result)
		{
			TEnum resultAsTEnum;
			var success = TryParse(value, ignoreCase, out resultAsTEnum);
			result = resultAsTEnum;
			return success;
		}

		bool IEnumsCache.TryParse(string value, bool ignoreCase, out object result, EnumFormat[] parseFormatOrder)
		{
			TEnum resultAsTEnum;
			var success = TryParse(value, ignoreCase, out resultAsTEnum, parseFormatOrder);
			result = resultAsTEnum;
			return success;
		}
		#endregion
		#endregion

		#region Flag Enum Operations
		#region Main Methods
		bool IEnumsCache.IsValidFlagCombination(object value) => IsValidFlagCombination(ConvertToEnum(value, nameof(value)));

		string IEnumsCache.FormatAsFlags(object value) => FormatAsFlags(ConvertToEnum(value, nameof(value)));

		string IEnumsCache.FormatAsFlags(object value, string delimiter) => FormatAsFlags(ConvertToEnum(value, nameof(value)), delimiter);

		string IEnumsCache.FormatAsFlags(object value, EnumFormat[] formats) => FormatAsFlags(ConvertToEnum(value, nameof(value)), formats);

		string IEnumsCache.FormatAsFlags(object value, string delimiter, EnumFormat[] formats) => FormatAsFlags(ConvertToEnum(value, nameof(value)), delimiter, formats);

		object[] IEnumsCache.GetFlags(object value) => GetFlags(ConvertToEnum(value, nameof(value))).Select(flag => (object)flag).ToArray();

		bool IEnumsCache.HasAnyFlags(object value) => HasAnyFlags(ConvertToEnum(value, nameof(value)));

		bool IEnumsCache.HasAnyFlags(object value, object flagMask) => HasAnyFlags(ConvertToEnum(value, nameof(value)), ConvertToEnum(flagMask, nameof(flagMask)));

		bool IEnumsCache.HasAllFlags(object value) => HasAllFlags(ConvertToEnum(value, nameof(value)));

		bool IEnumsCache.HasAllFlags(object value, object flagMask) => HasAllFlags(ConvertToEnum(value, nameof(value)), ConvertToEnum(flagMask, nameof(flagMask)));

		object IEnumsCache.InvertFlags(object value) => InvertFlags(ConvertToEnum(value, nameof(value)));

		object IEnumsCache.InvertFlags(object value, object flagMask) => InvertFlags(ConvertToEnum(value, nameof(value)), ConvertToEnum(flagMask, nameof(flagMask)));

		object IEnumsCache.CommonFlags(object value, object flagMask) => CommonFlags(ConvertToEnum(value, nameof(value)), ConvertToEnum(flagMask, nameof(flagMask)));

		object IEnumsCache.SetFlags(object flag0, object flag1) => SetFlags(ConvertToEnum(flag0, nameof(flag0)), ConvertToEnum(flag1, nameof(flag1)));

		object IEnumsCache.SetFlags(object flag0, object flag1, object flag2) => SetFlags(ConvertToEnum(flag0, nameof(flag0)), ConvertToEnum(flag1, nameof(flag1)), ConvertToEnum(flag2, nameof(flag2)));

		object IEnumsCache.SetFlags(object flag0, object flag1, object flag2, object flag3) => SetFlags(ConvertToEnum(flag0, nameof(flag0)), ConvertToEnum(flag1, nameof(flag1)), ConvertToEnum(flag2, nameof(flag2)), ConvertToEnum(flag3, nameof(flag3)));

		object IEnumsCache.SetFlags(object flag0, object flag1, object flag2, object flag3, object flag4) => SetFlags(ConvertToEnum(flag0, nameof(flag0)), ConvertToEnum(flag1, nameof(flag1)), ConvertToEnum(flag2, nameof(flag2)), ConvertToEnum(flag3, nameof(flag3)), ConvertToEnum(flag4, nameof(flag4)));

		object IEnumsCache.SetFlags(object[] flags) => SetFlags(ConvertToEnumArray(flags, nameof(flags)));

		object IEnumsCache.ClearFlags(object value, object flagMask) => ClearFlags(ConvertToEnum(value, nameof(value)), ConvertToEnum(flagMask, nameof(flagMask)));
		#endregion

		#region Parsing
		object IEnumsCache.ParseFlags(string value) => ParseFlags(value);

		object IEnumsCache.ParseFlags(string value, EnumFormat[] parseFormatOrder) => ParseFlags(value, parseFormatOrder);

		object IEnumsCache.ParseFlags(string value, bool ignoreCase) => ParseFlags(value, ignoreCase);

		object IEnumsCache.ParseFlags(string value, bool ignoreCase, EnumFormat[] parseFormatOrder) => ParseFlags(value, ignoreCase, parseFormatOrder);

		object IEnumsCache.ParseFlags(string value, string delimiter) => ParseFlags(value, delimiter);

		object IEnumsCache.ParseFlags(string value, string delimiter, EnumFormat[] parseFormatOrder) => ParseFlags(value, delimiter, parseFormatOrder);

		object IEnumsCache.ParseFlags(string value, bool ignoreCase, string delimiter) => ParseFlags(value, ignoreCase, delimiter);

		object IEnumsCache.ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder) => ParseFlags(value, ignoreCase, delimiter, parseFormatOrder);

		object IEnumsCache.ParseFlagsOrDefault(string value, object defaultEnum)
		{
			object result;
			if (!((IEnumsCache)this).TryParseFlags(value, out result))
			{
				result = defaultEnum;
			}
			return result;
		}

		object IEnumsCache.ParseFlagsOrDefault(string value, object defaultEnum, EnumFormat[] parseFormatOrder)
		{
			object result;
			if (!((IEnumsCache)this).TryParseFlags(value, out result, parseFormatOrder))
			{
				result = defaultEnum;
			}
			return result;
		}

		object IEnumsCache.ParseFlagsOrDefault(string value, bool ignoreCase, object defaultEnum)
		{
			object result;
			if (!((IEnumsCache)this).TryParseFlags(value, ignoreCase, out result))
			{
				result = defaultEnum;
			}
			return result;
		}

		object IEnumsCache.ParseFlagsOrDefault(string value, bool ignoreCase, object defaultEnum, EnumFormat[] parseFormatOrder)
		{
			object result;
			if (!((IEnumsCache)this).TryParseFlags(value, ignoreCase, out result, parseFormatOrder))
			{
				result = defaultEnum;
			}
			return result;
		}

		object IEnumsCache.ParseFlagsOrDefault(string value, string delimiter, object defaultEnum)
		{
			object result;
			if (!((IEnumsCache)this).TryParseFlags(value, delimiter, out result))
			{
				result = defaultEnum;
			}
			return result;
		}

		object IEnumsCache.ParseFlagsOrDefault(string value, string delimiter, object defaultEnum, EnumFormat[] parseFormatOrder)
		{
			object result;
			if (!((IEnumsCache)this).TryParseFlags(value, delimiter, out result, parseFormatOrder))
			{
				result = defaultEnum;
			}
			return result;
		}

		object IEnumsCache.ParseFlagsOrDefault(string value, bool ignoreCase, string delimiter, object defaultEnum)
		{
			object result;
			if (!((IEnumsCache)this).TryParseFlags(value, ignoreCase, delimiter, out result))
			{
				result = defaultEnum;
			}
			return result;
		}

		object IEnumsCache.ParseFlagsOrDefault(string value, bool ignoreCase, string delimiter, object defaultEnum, EnumFormat[] parseFormatOrder)
		{
			object result;
			if (!((IEnumsCache)this).TryParseFlags(value, ignoreCase, delimiter, out result, parseFormatOrder))
			{
				result = defaultEnum;
			}
			return result;
		}

		bool IEnumsCache.TryParseFlags(string value, out object result)
		{
			TEnum resultAtTEnum;
			var success = TryParseFlags(value, out resultAtTEnum);
			result = resultAtTEnum;
			return success;
		}

		bool IEnumsCache.TryParseFlags(string value, out object result, EnumFormat[] parseFormatOrder)
		{
			TEnum resultAtTEnum;
			var success = TryParseFlags(value, out resultAtTEnum, parseFormatOrder);
			result = resultAtTEnum;
			return success;
		}

		bool IEnumsCache.TryParseFlags(string value, bool ignoreCase, out object result)
		{
			TEnum resultAtTEnum;
			var success = TryParseFlags(value, ignoreCase, out resultAtTEnum);
			result = resultAtTEnum;
			return success;
		}

		bool IEnumsCache.TryParseFlags(string value, bool ignoreCase, out object result, EnumFormat[] parseFormatOrder)
		{
			TEnum resultAtTEnum;
			var success = TryParseFlags(value, ignoreCase, out resultAtTEnum, parseFormatOrder);
			result = resultAtTEnum;
			return success;
		}

		bool IEnumsCache.TryParseFlags(string value, string delimiter, out object result)
		{
			TEnum resultAtTEnum;
			var success = TryParseFlags(value, delimiter, out resultAtTEnum);
			result = resultAtTEnum;
			return success;
		}

		bool IEnumsCache.TryParseFlags(string value, string delimiter, out object result, EnumFormat[] parseFormatOrder)
		{
			TEnum resultAtTEnum;
			var success = TryParseFlags(value, delimiter, out resultAtTEnum, parseFormatOrder);
			result = resultAtTEnum;
			return success;
		}

		bool IEnumsCache.TryParseFlags(string value, bool ignoreCase, string delimiter, out object result)
		{
			TEnum resultAtTEnum;
			var success = TryParseFlags(value, ignoreCase, delimiter, out resultAtTEnum);
			result = resultAtTEnum;
			return success;
		}

		bool IEnumsCache.TryParseFlags(string value, bool ignoreCase, string delimiter, out object result, EnumFormat[] parseFormatOrder)
		{
			TEnum resultAtTEnum;
			var success = TryParseFlags(value, ignoreCase, delimiter, out resultAtTEnum, parseFormatOrder);
			result = resultAtTEnum;
			return success;
		}
		#endregion
		#endregion

		#region Private Methods
		private TEnum ConvertToEnum(object value, string paramName)
		{
			TEnum result;
			if (TryToObject(value, out result, false))
			{
				return result;
			}
			throw new ArgumentException($"value is not of type {typeof(TEnum).Name}", paramName);
		}

		private TEnum[] ConvertToEnumArray(object[] values, string paramName)
		{
			TEnum[] enumValues = null;
			if (values != null)
			{
				enumValues = new TEnum[values.Length];
				for (var i = 0; i < values.Length; ++i)
				{
					enumValues[i] = ConvertToEnum(values[i], $"{paramName}[{i}]");
				}
			}
			return enumValues;
		}
		#endregion
		#endregion
	}
}