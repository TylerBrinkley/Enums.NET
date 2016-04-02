// Enums.NET
// Copyright 2016 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

#if !(NET20 || NET35)
using System.Collections.Concurrent;
#endif

namespace EnumsNET
{
    internal sealed class EnumCache<TInt>
        where TInt : struct
    {
        #region Static
        internal new static readonly Func<TInt, TInt, bool> Equals = (Func<TInt, TInt, bool>)EnumCache.Equals(typeof(TInt));

        internal static readonly Func<TInt, TInt, bool> GreaterThan = (Func<TInt, TInt, bool>)EnumCache.GreaterThan(typeof(TInt));

        internal static readonly Func<TInt, TInt, TInt> And = (Func<TInt, TInt, TInt>)EnumCache.And(typeof(TInt));

        internal static readonly Func<TInt, TInt, TInt> Or = (Func<TInt, TInt, TInt>)EnumCache.Or(typeof(TInt));

        internal static readonly Func<TInt, TInt, TInt> Xor = (Func<TInt, TInt, TInt>)EnumCache.Xor(typeof(TInt));

        internal static readonly Func<TInt, int, TInt> LeftShift = (Func<TInt, int, TInt>)EnumCache.LeftShift(typeof(TInt));

        internal static readonly Func<TInt, TInt, TInt> Add = (Func<TInt, TInt, TInt>)EnumCache.Add(typeof(TInt));

        internal static readonly Func<TInt, TInt, TInt> Subtract = (Func<TInt, TInt, TInt>)EnumCache.Subtract(typeof(TInt));

        internal static bool IsPowerOfTwo(TInt x) => Equals(And(x, Subtract(x, One)), Zero);

        internal static readonly Func<long, TInt> FromInt64 = (Func<long, TInt>)EnumCache.FromInt64(typeof(TInt));

        internal static readonly Func<ulong, TInt> FromUInt64 = (Func<ulong, TInt>)EnumCache.FromUInt64(typeof(TInt));

        internal static readonly Func<long, bool> Int64IsInValueRange = EnumCache.Int64IsInValueRange(typeof(TInt));

        internal static readonly Func<ulong, bool> UInt64IsInValueRange = EnumCache.UInt64IsInValueRange(typeof(TInt));

        internal static readonly Func<TInt, sbyte> ToSByte = (Func<TInt, sbyte>)EnumCache.ToSByte(typeof(TInt));

        internal static readonly Func<TInt, byte> ToByte = (Func<TInt, byte>)EnumCache.ToByte(typeof(TInt));

        internal static readonly Func<TInt, short> ToInt16 = (Func<TInt, short>)EnumCache.ToInt16(typeof(TInt));

        internal static readonly Func<TInt, ushort> ToUInt16 = (Func<TInt, ushort>)EnumCache.ToUInt16(typeof(TInt));

        internal static readonly Func<TInt, int> ToInt32 = (Func<TInt, int>)EnumCache.ToInt32(typeof(TInt));

        internal static readonly Func<TInt, uint> ToUInt32 = (Func<TInt, uint>)EnumCache.ToUInt32(typeof(TInt));

        internal static readonly Func<TInt, long> ToInt64 = (Func<TInt, long>)EnumCache.ToInt64(typeof(TInt));

        internal static readonly Func<TInt, ulong> ToUInt64 = (Func<TInt, ulong>)EnumCache.ToUInt64(typeof(TInt));

        internal static readonly Func<TInt, string, string> ToStringFormat = (Func<TInt, string, string>)EnumCache.ToStringFormat(typeof(TInt));

        internal static readonly IntegralTryParse<TInt> TryParseMethod = (IntegralTryParse<TInt>)EnumCache.TryParseMethod(typeof(TInt));

        internal static readonly string HexFormatString = EnumCache.HexFormatString(typeof(TInt));

        internal static readonly TInt Zero = (TInt)EnumCache.Zero(typeof(TInt));

        internal static readonly TInt One = (TInt)EnumCache.One(typeof(TInt));

        internal static readonly TypeCode TypeCode = Type.GetTypeCode(typeof(TInt));

        internal static int Compare(TInt x, TInt y) => GreaterThan(x, y) ? 1 : (GreaterThan(y, x) ? -1 : 0);
        #endregion

        #region Fields
        internal readonly TInt AllFlags;

        internal readonly bool IsFlagEnum;

        internal readonly bool IsContiguous;

        private readonly string _enumTypeName;

        private readonly TInt _maxDefined;

        private readonly TInt _minDefined;

        private readonly Func<EnumFormat, Func<InternalEnumMemberInfo<TInt>, string>> _getCustomFormatter;

        // The main collection of values, names, and attributes with ~O(1) retrieval on name or value
        // If constant contains a DescriptionAttribute it will be the first in the attribute array
        private readonly OrderedBiDirectionalDictionary<TInt, NameAndAttributes> _valueMap;

        // Duplicate values are stored here with a key of the constant's name, is null if no duplicates
        private readonly Dictionary<string, ValueAndAttributes<TInt>> _duplicateValues;

        private Dictionary<string, string> _ignoreCaseSet;

#if NET20 || NET35
        private Dictionary<EnumFormat, EnumParser> _customEnumFormatParsers;
#else
        private ConcurrentDictionary<EnumFormat, EnumParser> _customEnumFormatParsers;
#endif
        #endregion

        #region Properties
        // Enables case insensitive parsing, lazily instantiated to reduce memory usage if not going to use this feature, is thread-safe as it's only used for retrieval
        private Dictionary<string, string> IgnoreCaseSet
        {
            get
            {
                if (_ignoreCaseSet == null)
                {
                    var ignoreCaseSet = new Dictionary<string, string>(GetDefinedCount(false), StringComparer.OrdinalIgnoreCase);
                    foreach (var nameAndAttributes in _valueMap.SecondItems)
                    {
                        ignoreCaseSet[nameAndAttributes.Name] = nameAndAttributes.Name;
                    }
                    if (_duplicateValues != null)
                    {
                        foreach (var name in _duplicateValues.Keys)
                        {
                            ignoreCaseSet[name] = name;
                        }
                    }
                    _ignoreCaseSet = ignoreCaseSet;
                }
                return _ignoreCaseSet;
            }
        }
        #endregion

        public EnumCache(Type enumType, Func<EnumFormat, Func<InternalEnumMemberInfo<TInt>, string>> getCustomFormatter)
        {
            Debug.Assert(enumType != null);
            Debug.Assert(enumType.IsEnum);
            _enumTypeName = enumType.Name;
            Debug.Assert(getCustomFormatter != null);
            _getCustomFormatter = getCustomFormatter;
            IsFlagEnum = enumType.IsDefined(typeof(FlagsAttribute), false);

            var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            _valueMap = new OrderedBiDirectionalDictionary<TInt, NameAndAttributes>(fields.Length);
            if (fields.Length == 0)
            {
                return;
            }
            var duplicateValues = new Dictionary<string, ValueAndAttributes<TInt>>();
            foreach (var field in fields)
            {
                var value = (TInt)field.GetValue(null);
                var name = field.Name;
                var attributes = Attribute.GetCustomAttributes(field, false);
                var isMainDupe = false;
                if (attributes.Length > 0)
                {
                    var descriptionFound = false;
                    for (var i = 0; i < attributes.Length; ++i)
                    {
                        var attr = attributes[i];
                        if (!descriptionFound)
                        {
                            var descAttr = attr as DescriptionAttribute;
                            if (descAttr != null)
                            {
                                for (var j = i; j > 0; --j)
                                {
                                    attributes[j] = attributes[j - 1];
                                }
                                attributes[0] = descAttr;
                                if (descAttr.GetType() == typeof(DescriptionAttribute))
                                {
                                    descriptionFound = true;
                                    if (isMainDupe)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        if (!isMainDupe && (attr as MainDuplicateAttribute) != null)
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
                        if (!GreaterThan(mapValue, value))
                        {
                            break;
                        }
                    }
                    _valueMap.Insert(index, value, new NameAndAttributes(name, attributes));
                    if (IsPowerOfTwo(value))
                    {
                        AllFlags = Or(AllFlags, value);
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
            _maxDefined = _valueMap.GetFirstAt(_valueMap.Count - 1);
            _minDefined = _valueMap.GetFirstAt(0);
            IsContiguous = Equals(Subtract(_maxDefined, FromInt64(_valueMap.Count - 1)), _minDefined);

            _valueMap.TrimExcess();
            if (duplicateValues.Count > 0)
            {
                // Makes sure is in increasing order, due to no removals
#if NET20
                var dupes = duplicateValues.ToArray();
                Array.Sort(dupes, (first, second) => InternalCompare(first.Value.Value, second.Value.Value));
#else
                var dupes = duplicateValues.OrderBy(pair => pair.Value.Value).ToList();
#endif
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

        public IEnumerable<InternalEnumMemberInfo<TInt>> GetEnumMemberInfos(bool uniqueValued)
        {
            if (uniqueValued || _duplicateValues == null)
            {
                return _valueMap.Select(pair => new InternalEnumMemberInfo<TInt>(pair.First, pair.Second.Name, pair.Second.Attributes, this));
            }
            else
            {
                return GetAllEnumMembersInValueOrder();
            }
        }

        public IEnumerable<string> GetNames(bool uniqueValued) => GetEnumMemberInfos(uniqueValued).Select(info => info.Name);

        public IEnumerable<TInt> GetValues(bool uniqueValued) => GetEnumMemberInfos(uniqueValued).Select(info => info.Value);

        private IEnumerable<InternalEnumMemberInfo<TInt>> GetAllEnumMembersInValueOrder()
        {
            using (var mainEnumerator = _valueMap.GetEnumerator())
            {
                var mainIsActive = true;
                mainEnumerator.MoveNext();
                var mainPair = mainEnumerator.Current;
                using (IEnumerator<KeyValuePair<string, ValueAndAttributes<TInt>>> dupeEnumerator = _duplicateValues.GetEnumerator())
                {
                    var dupeIsActive = true;
                    dupeEnumerator.MoveNext();
                    var dupePair = dupeEnumerator.Current;
                    var count = GetDefinedCount(false);
                    for (var i = 0; i < count; ++i)
                    {
                        TInt value;
                        string name;
                        Attribute[] attributes;
                        if (dupeIsActive && (!mainIsActive || GreaterThan(mainPair.First, dupePair.Value.Value)))
                        {
                            value = dupePair.Value.Value;
                            name = dupePair.Key;
                            attributes = dupePair.Value.Attributes;
                            if (dupeIsActive = dupeEnumerator.MoveNext())
                            {
                                dupePair = dupeEnumerator.Current;
                            }
                        }
                        else
                        {
                            value = mainPair.First;
                            name = mainPair.Second.Name;
                            attributes = mainPair.Second.Attributes;
                            if (mainIsActive = mainEnumerator.MoveNext())
                            {
                                mainPair = mainEnumerator.Current;
                            }
                        }
                        yield return new InternalEnumMemberInfo<TInt>(value, name, attributes, this);
                    }
                }
            }
        }
        #endregion

        #region IsValid
        public bool IsValid(object value)
        {
            Preconditions.NotNull(value, nameof(value));

            TInt result;
            return TryToObject(value, out result, true);
        }

        public bool IsValid(TInt value) => IsFlagEnum ? IsValidFlagCombination(value) : IsDefined(value);

        public bool IsValid(long value) => Int64IsInValueRange(value) && IsValid(FromInt64(value));

        public bool IsValid(ulong value) => UInt64IsInValueRange(value) && IsValid(FromUInt64(value));
        #endregion

        #region IsDefined
        public bool IsDefined(object value)
        {
            Preconditions.NotNull(value, nameof(value));

            TInt result;
            return TryToObject(value, out result, false) && IsDefined(result);
        }

        public bool IsDefined(TInt value) => IsContiguous ? (!(GreaterThan(_minDefined, value) || GreaterThan(value, _maxDefined))) : _valueMap.ContainsFirst(value);

        public bool IsDefined(string name, bool ignoreCase)
        {
            Preconditions.NotNull(name, nameof(name));

            return _valueMap.ContainsSecond(new NameAndAttributes(name)) || (_duplicateValues?.ContainsKey(name) ?? false) || (ignoreCase && IgnoreCaseSet.ContainsKey(name));
        }

        public bool IsDefined(long value) => Int64IsInValueRange(value) && IsDefined(FromInt64(value));

        public bool IsDefined(ulong value) => UInt64IsInValueRange(value) && IsDefined(FromUInt64(value));
        #endregion

        #region ToObject
        public TInt ToObject(object value, bool validate)
        {
            Preconditions.NotNull(value, nameof(value));

            if (value is TInt || value is TInt?)
            {
                var result = (TInt)value;
                if (validate)
                {
                    Validate(result, nameof(value));
                }
                return result;
            }

            var type = value.GetType();

            switch (Type.GetTypeCode(Nullable.GetUnderlyingType(type) ?? type))
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
                    var result = Parse((string)value, false, null);
                    if (validate)
                    {
                        Validate(result, nameof(value));
                    }
                    return result;
            }
            throw new ArgumentException($"value is not type {_enumTypeName}, SByte, Int16, Int32, Int64, Byte, UInt16, UInt32, UInt64, or String.");
        }

        public TInt ToObject(long value, bool validate)
        {
            if (!Int64IsInValueRange(value))
            {
                throw Enums.GetOverflowException();
            }

            var result = FromInt64(value);
            if (validate)
            {
                Validate(result, nameof(value));
            }
            return result;
        }

        public TInt ToObject(ulong value, bool validate)
        {
            if (!UInt64IsInValueRange(value))
            {
                throw Enums.GetOverflowException();
            }

            var result = FromUInt64(value);
            if (validate)
            {
                Validate(result, nameof(value));
            }
            return result;
        }

        public bool TryToObject(object value, out TInt result, bool validate)
        {
            if (value != null)
            {
                if (value is TInt || value is TInt?)
                {
                    result = (TInt)value;
                    return true;
                }

                var type = value.GetType();

                switch (Type.GetTypeCode(Nullable.GetUnderlyingType(type) ?? type))
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
                        if (TryParse((string)value, false, out result, null))
                        {
                            return !validate || IsValid(result);
                        }
                        break;
                }
            }
            result = Zero;
            return false;
        }

        public bool TryToObject(long value, out TInt result, bool validate)
        {
            if (Int64IsInValueRange(value))
            {
                result = FromInt64(value);
                return !validate || IsValid(result);
            }
            result = Zero;
            return false;
        }

        public bool TryToObject(ulong value, out TInt result, bool validate)
        {
            if (UInt64IsInValueRange(value))
            {
                result = FromUInt64(value);
                return !validate || IsValid(result);
            }
            result = Zero;
            return false;
        }
        #endregion

        #region All Values Main Methods
        public void Validate(TInt value, string paramName)
        {
            if (!IsValid(value))
            {
                throw new ArgumentException($"invalid value of {AsString(value)} for {_enumTypeName}", paramName);
            }
        }

        public string AsString(TInt value) => InternalAsString(GetEnumMemberInfo(value));

        internal string InternalAsString(InternalEnumMemberInfo<TInt> info)
        {
            if (IsFlagEnum)
            {
                var str = InternalFormatAsFlags(info, null, null);
                if (str != null)
                {
                    return str;
                }
            }
            return InternalFormat(info, EnumFormat.Name, EnumFormat.DecimalValue);
        }

        public string AsString(TInt value, EnumFormat[] formats) => InternalAsString(GetEnumMemberInfo(value), formats);

        internal string InternalAsString(InternalEnumMemberInfo<TInt> info, EnumFormat[] formats) => formats?.Length > 0 ? InternalFormat(info, formats) : InternalAsString(info);

        public string AsString(TInt value, string format) => InternalAsString(GetEnumMemberInfo(value), format);

        internal string InternalAsString(InternalEnumMemberInfo<TInt> info, string format) => string.IsNullOrEmpty(format) ? InternalAsString(info) : InternalFormat(info, format);

        public string Format(TInt value, EnumFormat format) => InternalFormat(GetEnumMemberInfo(value), format);

        public string Format(TInt value, EnumFormat format0, EnumFormat format1) => InternalFormat(GetEnumMemberInfo(value), format0, format1);

        public string Format(TInt value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => InternalFormat(GetEnumMemberInfo(value), format0, format1, format2);

        public string Format(TInt value, EnumFormat[] formats)
        {
            Preconditions.NotNull(formats, nameof(formats));

            return InternalFormat(GetEnumMemberInfo(value), formats);
        }

        public string Format(TInt value, string format)
        {
            Preconditions.NotNull(format, nameof(format));

            return InternalFormat(GetEnumMemberInfo(value), format);
        }

        internal string InternalFormat(InternalEnumMemberInfo<TInt> info, string format)
        {
            switch (format)
            {
                case "G":
                case "g":
                    return InternalAsString(info);
                case "F":
                case "f":
                    return InternalFormatAsFlags(info, null, null) ?? InternalFormat(info, EnumFormat.Name, EnumFormat.DecimalValue);
                case "D":
                case "d":
                    return ToStringFormat(info.Value, "D");
                case "X":
                case "x":
                    return ToStringFormat(info.Value, HexFormatString);
            }
            throw new FormatException("format string can be only \"G\", \"g\", \"X\", \"x\", \"F\", \"f\", \"D\" or \"d\".");
        }

        internal string InternalFormat(InternalEnumMemberInfo<TInt> info, EnumFormat format)
        {
            switch (format)
            {
                case EnumFormat.DecimalValue:
                    return ToStringFormat(info.Value, "D");
                case EnumFormat.HexadecimalValue:
                    return ToStringFormat(info.Value, HexFormatString);
                case EnumFormat.Name:
                    return info.Name;
                case EnumFormat.Description:
                    return info.Description;
                default:
                    return _getCustomFormatter(format)?.Invoke(info);
            }
        }

        internal string InternalFormat(InternalEnumMemberInfo<TInt> info, EnumFormat format0, EnumFormat format1)
        {
            return InternalFormat(info, format0) ?? InternalFormat(info, format1);
        }

        internal string InternalFormat(InternalEnumMemberInfo<TInt> info, EnumFormat format0, EnumFormat format1, EnumFormat format2)
        {
            return InternalFormat(info, format0) ?? InternalFormat(info, format1) ?? InternalFormat(info, format2);
        }

        internal string InternalFormat(InternalEnumMemberInfo<TInt> info, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3)
        {
            return InternalFormat(info, format0) ?? InternalFormat(info, format1) ?? InternalFormat(info, format2) ?? InternalFormat(info, format3);
        }

        internal string InternalFormat(InternalEnumMemberInfo<TInt> info, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4)
        {
            return InternalFormat(info, format0) ?? InternalFormat(info, format1) ?? InternalFormat(info, format2) ?? InternalFormat(info, format3) ?? InternalFormat(info, format4);
        }

        internal string InternalFormat(InternalEnumMemberInfo<TInt> info, EnumFormat[] formats)
        {
            foreach (var format in formats)
            {
                var formattedValue = InternalFormat(info, format);
                if (formattedValue != null)
                {
                    return formattedValue;
                }
            }
            return null;
        }
        #endregion

        #region Defined Values Main Methods
        public InternalEnumMemberInfo<TInt> GetEnumMemberInfo(TInt value)
        {
            var index = _valueMap.IndexOfFirst(value);
            if (index >= 0)
            {
                var nameAndAttributes = _valueMap.GetSecondAt(index);
                return new InternalEnumMemberInfo<TInt>(value, nameAndAttributes.Name, nameAndAttributes.Attributes, this);
            }
            return new InternalEnumMemberInfo<TInt>(value, null, null, this);
        }

        public InternalEnumMemberInfo<TInt> GetEnumMemberInfo(string name, bool ignoreCase)
        {
            Preconditions.NotNull(name, nameof(name));

            return InternalGetEnumMemberInfo(name, ignoreCase);
        }

        private InternalEnumMemberInfo<TInt> InternalGetEnumMemberInfo(string name, bool ignoreCase)
        {
            var index = _valueMap.IndexOfSecond(new NameAndAttributes(name));
            if (index < 0)
            {
                var valueAndAttributes = default(ValueAndAttributes<TInt>);
                bool foundInDuplicates;
                if (!(foundInDuplicates = (_duplicateValues?.TryGetValue(name, out valueAndAttributes)).GetValueOrDefault()))
                {
                    if (!(ignoreCase && IgnoreCaseSet.TryGetValue(name, out name)))
                    {
                        return new InternalEnumMemberInfo<TInt>();
                    }
                    index = _valueMap.IndexOfSecond(new NameAndAttributes(name));
                    if (index < 0)
                    {
                        valueAndAttributes = _duplicateValues[name];
                        foundInDuplicates = true;
                    }
                }
                if (foundInDuplicates)
                {
                    return new InternalEnumMemberInfo<TInt>(valueAndAttributes.Value, name, valueAndAttributes.Attributes, this);
                }
            }
            var pair = _valueMap.GetAt(index);
            return new InternalEnumMemberInfo<TInt>(pair.First, name, pair.Second.Attributes, this);
        }

        public string GetName(TInt value) => GetEnumMemberInfo(value).Name;

        public string GetDescription(TInt value) => GetEnumMemberInfo(value).Description;
        #endregion

        #region Attributes
        public bool HasAttribute<TAttribute>(TInt value)
            where TAttribute : Attribute => GetEnumMemberInfo(value).HasAttribute<TAttribute>();

        public TAttribute GetAttribute<TAttribute>(TInt value)
            where TAttribute : Attribute => GetEnumMemberInfo(value).GetAttribute<TAttribute>();

        public TResult GetAttributeSelect<TAttribute, TResult>(TInt value, Func<TAttribute, TResult> selector, TResult defaultValue)
            where TAttribute : Attribute => GetEnumMemberInfo(value).GetAttributeSelect(selector, defaultValue);

        public bool TryGetAttributeSelect<TAttribute, TResult>(TInt value, Func<TAttribute, TResult> selector, out TResult result)
            where TAttribute : Attribute => GetEnumMemberInfo(value).TryGetAttributeSelect(selector, out result);

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(TInt value)
            where TAttribute : Attribute => GetEnumMemberInfo(value).GetAttributes<TAttribute>();

        public Attribute[] GetAttributes(TInt value) => GetEnumMemberInfo(value).Attributes;
        #endregion

        #region Parsing
        public TInt Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder)
        {
            Preconditions.NotNull(value, nameof(value));

            value = value.Trim();
            TInt result;
            if (IsFlagEnum)
            {
                return TryParseMethod(value, NumberStyles.AllowLeadingSign, null, out result) ? result : ParseFlags(value, ignoreCase, null, parseFormatOrder);
            }

            if (!(parseFormatOrder?.Length > 0))
            {
                parseFormatOrder = Enums.DefaultFormatOrder;
            }

            if (InternalTryParse(value, ignoreCase, out result, parseFormatOrder))
            {
                return result;
            }
            if (Enums.IsNumeric(value))
            {
                throw Enums.GetOverflowException();
            }
            throw new ArgumentException($"string was not recognized as being a member of {_enumTypeName}", nameof(value));
        }

        public bool TryParse(string value, bool ignoreCase, out TInt result, EnumFormat[] parseFormatOrder)
        {
            if (value != null)
            {
                value = value.Trim();
                if (IsFlagEnum)
                {
                    return TryParseMethod(value, NumberStyles.AllowLeadingSign, null, out result) || TryParseFlags(value, ignoreCase, null, out result, parseFormatOrder);
                }

                if (!(parseFormatOrder?.Length > 0))
                {
                    parseFormatOrder = Enums.DefaultFormatOrder;
                }

                if (InternalTryParse(value, ignoreCase, out result, parseFormatOrder))
                {
                    return true;
                }
            }
            result = Zero;
            return false;
        }

        private bool InternalTryParse(string value, bool ignoreCase, out TInt result, EnumFormat[] parseFormatOrder)
        {
            result = default(TInt);
            foreach (var format in parseFormatOrder)
            {
                var success = false;
                switch (format)
                {
                    case EnumFormat.DecimalValue:
                        success = TryParseMethod(value, NumberStyles.AllowLeadingSign, null, out result);
                        break;
                    case EnumFormat.HexadecimalValue:
                        success = TryParseMethod(value, NumberStyles.AllowHexSpecifier, null, out result);
                        break;
                    case EnumFormat.Name:
                        var info = InternalGetEnumMemberInfo(value, ignoreCase);
                        if (info.IsDefined)
                        {
                            result = info.Value;
                            success = true;
                        }
                        break;
                    default:
                        EnumParser parser = null;
#if NET20
                        lock (_valueMap)
                        {
#endif
                            if (_customEnumFormatParsers?.TryGetValue(format, out parser) != true)
                            {
                                switch (format)
                                {
                                    case EnumFormat.Description:
                                        parser = new EnumParser(internalInfo => internalInfo.Description, this);
                                        break;
                                    default:
                                        var customEnumFormatter = _getCustomFormatter(format);
                                        if (customEnumFormatter != null)
                                        {
                                            parser = new EnumParser(customEnumFormatter, this);
                                        }
                                        break;
                                }
                                if (parser != null)
                                {
                                    if (_customEnumFormatParsers == null)
                                    {
                                        Interlocked.CompareExchange(ref _customEnumFormatParsers,
#if NET20
                                            new Dictionary<EnumFormat, EnumParser>(new EnumComparer<EnumFormat>()),
#else
                                            new ConcurrentDictionary<EnumFormat, EnumParser>(new EnumComparer<EnumFormat>()),
#endif
                                            null);
                                    }
#if NET20
                                    _customEnumFormatParsers.Add(format, parser);
#else
                                    _customEnumFormatParsers.TryAdd(format, parser);
#endif
                                }
                            }
#if NET20
                        }
#endif
                        success = parser?.TryParse(value, ignoreCase, out result) ?? false;
                        break;
                }
                if (success)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
        #endregion

        #region Flag Enum Operations
        #region Main Methods
        public bool IsValidFlagCombination(TInt value) => Equals(And(AllFlags, value), value);

        public string FormatAsFlags(TInt value, string delimiter, EnumFormat[] formats) => InternalFormatAsFlags(GetEnumMemberInfo(value), delimiter, formats);

        private string InternalFormatAsFlags(InternalEnumMemberInfo<TInt> info, string delimiter, EnumFormat[] formats)
        {
            if (!IsValidFlagCombination(info.Value))
            {
                return null;
            }

            if (string.IsNullOrEmpty(delimiter))
            {
                delimiter = FlagEnums.DefaultDelimiter;
            }

            if (!(formats?.Length > 0))
            {
                formats = Enums.DefaultFormatOrder;
            }
            
            if (info.IsDefined || Equals(info.Value, Zero))
            {
                return InternalFormat(info, formats);
            }

            return string.Join(delimiter,
                InternalGetFlags(info.Value).Select(flag => InternalFormat(GetEnumMemberInfo(flag), formats))
#if NET20 || NET35
                .ToArray()
#endif
                );
        }

        public IEnumerable<TInt> GetFlags(TInt value) => IsValidFlagCombination(value) ? InternalGetFlags(value) : null;

        private IEnumerable<TInt> InternalGetFlags(TInt value)
        {
            var isLessThanZero = GreaterThan(Zero, value);
            for (var currentValue = One; (isLessThanZero || !GreaterThan(currentValue, value)) && !Equals(currentValue, Zero); currentValue = LeftShift(currentValue, 1))
            {
                if (IsValidFlagCombination(currentValue) && InternalHasAnyFlags(value, currentValue))
                {
                    yield return currentValue;
                }
            }
        }

        public bool HasAnyFlags(TInt value)
        {
            ValidateFlagCombination(value, nameof(value));
            return !Equals(value, Zero);
        }

        private void ValidateFlagCombination(TInt value, string paramName)
        {
            if (!IsValidFlagCombination(value))
            {
                throw new ArgumentException("must be valid flag combination", paramName);
            }
        }

        public bool HasAnyFlags(TInt value, TInt flagMask)
        {
            ValidateFlagCombination(value, nameof(value));
            ValidateFlagCombination(flagMask, nameof(flagMask));
            return InternalHasAnyFlags(value, flagMask);
        }

        private bool InternalHasAnyFlags(TInt value, TInt flagMask) => !Equals(And(value, flagMask), Zero);

        public bool HasAllFlags(TInt value)
        {
            ValidateFlagCombination(value, nameof(value));
            return Equals(value, AllFlags);
        }

        public bool HasAllFlags(TInt value, TInt flagMask)
        {
            ValidateFlagCombination(value, nameof(value));
            ValidateFlagCombination(flagMask, nameof(flagMask));
            return Equals(And(value, flagMask), flagMask);
        }

        public TInt ToggleFlags(TInt value)
        {
            ValidateFlagCombination(value, nameof(value));
            return Xor(value, AllFlags);
        }

        public TInt ToggleFlags(TInt value, TInt flagMask)
        {
            ValidateFlagCombination(value, nameof(value));
            ValidateFlagCombination(flagMask, nameof(flagMask));
            return Xor(value, flagMask);
        }

        public TInt CommonFlags(TInt value, TInt flagMask)
        {
            ValidateFlagCombination(value, nameof(value));
            ValidateFlagCombination(flagMask, nameof(flagMask));
            return And(value, flagMask);
        }

        public TInt SetFlags(TInt flag0, TInt flag1)
        {
            ValidateFlagCombination(flag0, nameof(flag0));
            ValidateFlagCombination(flag1, nameof(flag1));
            return Or(flag0, flag1);
        }

        public TInt SetFlags(TInt flag0, TInt flag1, TInt flag2)
        {
            ValidateFlagCombination(flag0, nameof(flag0));
            ValidateFlagCombination(flag1, nameof(flag1));
            ValidateFlagCombination(flag2, nameof(flag2));
            return Or(Or(flag0, flag1), flag2);
        }

        public TInt SetFlags(TInt flag0, TInt flag1, TInt flag2, TInt flag3)
        {
            ValidateFlagCombination(flag0, nameof(flag0));
            ValidateFlagCombination(flag1, nameof(flag1));
            ValidateFlagCombination(flag2, nameof(flag2));
            ValidateFlagCombination(flag3, nameof(flag3));
            return Or(Or(Or(flag0, flag1), flag2), flag3);
        }

        public TInt SetFlags(TInt flag0, TInt flag1, TInt flag2, TInt flag3, TInt flag4)
        {
            ValidateFlagCombination(flag0, nameof(flag0));
            ValidateFlagCombination(flag1, nameof(flag1));
            ValidateFlagCombination(flag2, nameof(flag2));
            ValidateFlagCombination(flag3, nameof(flag3));
            ValidateFlagCombination(flag4, nameof(flag4));
            return Or(Or(Or(Or(flag0, flag1), flag2), flag3), flag4);
        }

        public TInt SetFlags(IEnumerable<TInt> flags)
        {
            var flag = Zero;
            if (flags != null)
            {
                foreach (var nextFlag in flags)
                {
                    ValidateFlagCombination(nextFlag, nameof(flags) + " must contain all valid flag combinations");
                    flag = Or(flag, nextFlag);
                }
            }
            return flag;
        }

        public TInt ClearFlags(TInt value, TInt flagMask)
        {
            ValidateFlagCombination(value, nameof(value));
            ValidateFlagCombination(flagMask, nameof(flagMask));
            return And(value, Xor(flagMask, AllFlags));
        }
        #endregion

        #region Parsing
        public TInt ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder)
        {
            Preconditions.NotNull(value, nameof(value));

            if (string.IsNullOrEmpty(delimiter))
            {
                delimiter = FlagEnums.DefaultDelimiter;
            }

            var effectiveDelimiter = delimiter.Trim();
            if (effectiveDelimiter.Length == 0)
            {
                effectiveDelimiter = delimiter;
            }
            var split = value.Split(new[] { effectiveDelimiter }, StringSplitOptions.None);

            if (!(parseFormatOrder?.Length > 0))
            {
                parseFormatOrder = Enums.DefaultFormatOrder;
            }

            var result = Zero;
            foreach (var indValue in split)
            {
                var trimmedIndValue = indValue.Trim();
                TInt indValueAsInt;
                if (InternalTryParse(trimmedIndValue, ignoreCase, out indValueAsInt, parseFormatOrder))
                {
                    if (!IsValidFlagCombination(indValueAsInt))
                    {
                        throw new ArgumentException("All individual enum values within value must be valid");
                    }
                    result = Or(result, indValueAsInt);
                }
                else
                {
                    if (Enums.IsNumeric(trimmedIndValue))
                    {
                        throw Enums.GetOverflowException();
                    }
                    throw new ArgumentException("value is not a valid combination of flag enum values");
                }
            }
            return result;
        }

        public bool TryParseFlags(string value, bool ignoreCase, string delimiter, out TInt result, EnumFormat[] parseFormatOrder)
        {
            if (value == null)
            {
                result = Zero;
                return false;
            }

            if (string.IsNullOrEmpty(delimiter))
            {
                delimiter = FlagEnums.DefaultDelimiter;
            }

            var effectiveDelimiter = delimiter.Trim();
            if (effectiveDelimiter.Length == 0)
            {
                effectiveDelimiter = delimiter;
            }
            var split = value.Split(new[] { effectiveDelimiter }, StringSplitOptions.None);

            if (!(parseFormatOrder?.Length > 0))
            {
                parseFormatOrder = Enums.DefaultFormatOrder;
            }

            var resultAsInt = Zero;
            foreach (var indValue in split)
            {
                var trimmedIndValue = indValue.Trim();
                TInt indValueAsInt;
                if (!InternalTryParse(trimmedIndValue, ignoreCase, out indValueAsInt, parseFormatOrder) || !IsValidFlagCombination(indValueAsInt))
                {
                    result = Zero;
                    return false;
                }
                resultAsInt = Or(resultAsInt, indValueAsInt);
            }
            result = resultAsInt;
            return true;
        }
        #endregion
        #endregion

        #region Nested Types
        internal sealed class EnumParser
        {
            private readonly Dictionary<string, TInt> _formatValueMap;
            private Dictionary<string, TInt> _formatIgnoreCase;

            private Dictionary<string, TInt> FormatIgnoreCase
            {
                get
                {
                    if (_formatIgnoreCase == null)
                    {
                        var formatIgnoreCase = new Dictionary<string, TInt>(_formatValueMap.Count, StringComparer.OrdinalIgnoreCase);
                        foreach (var pair in _formatValueMap)
                        {
                            if (!formatIgnoreCase.ContainsKey(pair.Key))
                            {
                                formatIgnoreCase.Add(pair.Key, pair.Value);
                            }
                        }
                        
                        _formatIgnoreCase = formatIgnoreCase;
                    }
                    return _formatIgnoreCase;
                }
            }

            public EnumParser(Func<InternalEnumMemberInfo<TInt>, string> formatter, EnumCache<TInt> enumsCache)
            {
                _formatValueMap = new Dictionary<string, TInt>(enumsCache.GetDefinedCount(false));
                foreach (var info in enumsCache.GetEnumMemberInfos(false))
                {
                    var format = formatter(info);
                    if (format != null)
                    {
                        _formatValueMap[format] = info.Value;
                    }
                }
            }

            internal bool TryParse(string format, bool ignoreCase, out TInt result) => _formatValueMap.TryGetValue(format, out result) || (ignoreCase && FormatIgnoreCase.TryGetValue(format, out result));
        }
        #endregion
    }

    internal delegate bool IntegralTryParse<TInt>(string value, NumberStyles styles, IFormatProvider provider, out TInt result);

    internal static class EnumCache
    {
        internal static Delegate Equals(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, sbyte, bool>)((a, b) => a == b);
                case TypeCode.Byte:
                    return (Func<byte, byte, bool>)((a, b) => a == b);
                case TypeCode.Int16:
                    return (Func<short, short, bool>)((a, b) => a == b);
                case TypeCode.UInt16:
                    return (Func<ushort, ushort, bool>)((a, b) => a == b);
                case TypeCode.Int32:
                    return (Func<int, int, bool>)((a, b) => a == b);
                case TypeCode.UInt32:
                    return (Func<uint, uint, bool>)((a, b) => a == b);
                case TypeCode.Int64:
                    return (Func<long, long, bool>)((a, b) => a == b);
                case TypeCode.UInt64:
                    return (Func<ulong, ulong, bool>)((a, b) => a == b);
            }
            return null;
        }

        internal static Delegate GreaterThan(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, sbyte, bool>)((a, b) => a > b);
                case TypeCode.Byte:
                    return (Func<byte, byte, bool>)((a, b) => a > b);
                case TypeCode.Int16:
                    return (Func<short, short, bool>)((a, b) => a > b);
                case TypeCode.UInt16:
                    return (Func<ushort, ushort, bool>)((a, b) => a > b);
                case TypeCode.Int32:
                    return (Func<int, int, bool>)((a, b) => a > b);
                case TypeCode.UInt32:
                    return (Func<uint, uint, bool>)((a, b) => a > b);
                case TypeCode.Int64:
                    return (Func<long, long, bool>)((a, b) => a > b);
                case TypeCode.UInt64:
                    return (Func<ulong, ulong, bool>)((a, b) => a > b);
            }
            return null;
        }

        internal static Delegate And(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, sbyte, sbyte>)((a, b) => (sbyte)(a & b));
                case TypeCode.Byte:
                    return (Func<byte, byte, byte>)((a, b) => (byte)(a & b));
                case TypeCode.Int16:
                    return (Func<short, short, short>)((a, b) => (short)(a & b));
                case TypeCode.UInt16:
                    return (Func<ushort, ushort, ushort>)((a, b) => (ushort)(a & b));
                case TypeCode.Int32:
                    return (Func<int, int, int>)((a, b) => a & b);
                case TypeCode.UInt32:
                    return (Func<uint, uint, uint>)((a, b) => a & b);
                case TypeCode.Int64:
                    return (Func<long, long, long>)((a, b) => a & b);
                case TypeCode.UInt64:
                    return (Func<ulong, ulong, ulong>)((a, b) => a & b);
            }
            return null;
        }

        internal static Delegate Or(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, sbyte, sbyte>)((a, b) => (sbyte)(a | b));
                case TypeCode.Byte:
                    return (Func<byte, byte, byte>)((a, b) => (byte)(a | b));
                case TypeCode.Int16:
                    return (Func<short, short, short>)((a, b) => (short)(a | b));
                case TypeCode.UInt16:
                    return (Func<ushort, ushort, ushort>)((a, b) => (ushort)(a | b));
                case TypeCode.Int32:
                    return (Func<int, int, int>)((a, b) => a | b);
                case TypeCode.UInt32:
                    return (Func<uint, uint, uint>)((a, b) => a | b);
                case TypeCode.Int64:
                    return (Func<long, long, long>)((a, b) => a | b);
                case TypeCode.UInt64:
                    return (Func<ulong, ulong, ulong>)((a, b) => a | b);
            }
            return null;
        }

        internal static Delegate Xor(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, sbyte, sbyte>)((a, b) => (sbyte)(a ^ b));
                case TypeCode.Byte:
                    return (Func<byte, byte, byte>)((a, b) => (byte)(a ^ b));
                case TypeCode.Int16:
                    return (Func<short, short, short>)((a, b) => (short)(a ^ b));
                case TypeCode.UInt16:
                    return (Func<ushort, ushort, ushort>)((a, b) => (ushort)(a ^ b));
                case TypeCode.Int32:
                    return (Func<int, int, int>)((a, b) => a ^ b);
                case TypeCode.UInt32:
                    return (Func<uint, uint, uint>)((a, b) => a ^ b);
                case TypeCode.Int64:
                    return (Func<long, long, long>)((a, b) => a ^ b);
                case TypeCode.UInt64:
                    return (Func<ulong, ulong, ulong>)((a, b) => a ^ b);
            }
            return null;
        }

        internal static Delegate LeftShift(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, int, sbyte>)((a, b) => (sbyte)(a << b));
                case TypeCode.Byte:
                    return (Func<byte, int, byte>)((a, b) => (byte)(a << b));
                case TypeCode.Int16:
                    return (Func<short, int, short>)((a, b) => (short)(a << b));
                case TypeCode.UInt16:
                    return (Func<ushort, int, ushort>)((a, b) => (ushort)(a << b));
                case TypeCode.Int32:
                    return (Func<int, int, int>)((a, b) => a << b);
                case TypeCode.UInt32:
                    return (Func<uint, int, uint>)((a, b) => a << b);
                case TypeCode.Int64:
                    return (Func<long, int, long>)((a, b) => a << b);
                case TypeCode.UInt64:
                    return (Func<ulong, int, ulong>)((a, b) => a << b);
            }
            return null;
        }

        internal static Delegate Add(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, sbyte, sbyte>)((a, b) => (sbyte)(a + b));
                case TypeCode.Byte:
                    return (Func<byte, byte, byte>)((a, b) => (byte)(a + b));
                case TypeCode.Int16:
                    return (Func<short, short, short>)((a, b) => (short)(a + b));
                case TypeCode.UInt16:
                    return (Func<ushort, ushort, ushort>)((a, b) => (ushort)(a + b));
                case TypeCode.Int32:
                    return (Func<int, int, int>)((a, b) => a + b);
                case TypeCode.UInt32:
                    return (Func<uint, uint, uint>)((a, b) => a + b);
                case TypeCode.Int64:
                    return (Func<long, long, long>)((a, b) => a + b);
                case TypeCode.UInt64:
                    return (Func<ulong, ulong, ulong>)((a, b) => a + b);
            }
            return null;
        }

        internal static Delegate Subtract(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, sbyte, sbyte>)((a, b) => (sbyte)(a - b));
                case TypeCode.Byte:
                    return (Func<byte, byte, byte>)((a, b) => (byte)(a - b));
                case TypeCode.Int16:
                    return (Func<short, short, short>)((a, b) => (short)(a - b));
                case TypeCode.UInt16:
                    return (Func<ushort, ushort, ushort>)((a, b) => (ushort)(a - b));
                case TypeCode.Int32:
                    return (Func<int, int, int>)((a, b) => a - b);
                case TypeCode.UInt32:
                    return (Func<uint, uint, uint>)((a, b) => a - b);
                case TypeCode.Int64:
                    return (Func<long, long, long>)((a, b) => a - b);
                case TypeCode.UInt64:
                    return (Func<ulong, ulong, ulong>)((a, b) => a - b);
            }
            return null;
        }

        internal static Delegate FromInt64(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<long, sbyte>)(a => (sbyte)a);
                case TypeCode.Byte:
                    return (Func<long, byte>)(a => (byte)a);
                case TypeCode.Int16:
                    return (Func<long, short>)(a => (short)a);
                case TypeCode.UInt16:
                    return (Func<long, ushort>)(a => (ushort)a);
                case TypeCode.Int32:
                    return (Func<long, int>)(a => (int)a);
                case TypeCode.UInt32:
                    return (Func<long, uint>)(a => (uint)a);
                case TypeCode.Int64:
                    return (Func<long, long>)(a => a);
                case TypeCode.UInt64:
                    return (Func<long, ulong>)(a => (ulong)a);
            }
            return null;
        }

        internal static Delegate FromUInt64(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<ulong, sbyte>)(a => (sbyte)a);
                case TypeCode.Byte:
                    return (Func<ulong, byte>)(a => (byte)a);
                case TypeCode.Int16:
                    return (Func<ulong, short>)(a => (short)a);
                case TypeCode.UInt16:
                    return (Func<ulong, ushort>)(a => (ushort)a);
                case TypeCode.Int32:
                    return (Func<ulong, int>)(a => (int)a);
                case TypeCode.UInt32:
                    return (Func<ulong, uint>)(a => (uint)a);
                case TypeCode.Int64:
                    return (Func<ulong, long>)(a => (long)a);
                case TypeCode.UInt64:
                    return (Func<ulong, ulong>)(a => a);
            }
            return null;
        }

        internal static Func<long, bool> Int64IsInValueRange(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return a => a >= sbyte.MinValue && a <= sbyte.MaxValue;
                case TypeCode.Byte:
                    return a => a >= byte.MinValue && a <= byte.MaxValue;
                case TypeCode.Int16:
                    return a => a >= short.MinValue && a <= short.MaxValue;
                case TypeCode.UInt16:
                    return a => a >= ushort.MinValue && a <= ushort.MaxValue;
                case TypeCode.Int32:
                    return a => a >= int.MinValue && a <= int.MaxValue;
                case TypeCode.UInt32:
                    return a => a >= uint.MinValue && a <= uint.MaxValue;
                case TypeCode.Int64:
                    return a => true;
                case TypeCode.UInt64:
                    return a => a >= 0L;
            }
            return null;
        }

        internal static Func<ulong, bool> UInt64IsInValueRange(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return a => a <= (ulong)sbyte.MaxValue;
                case TypeCode.Byte:
                    return a => a <= byte.MaxValue;
                case TypeCode.Int16:
                    return a => a <= (ulong)short.MaxValue;
                case TypeCode.UInt16:
                    return a => a <= ushort.MaxValue;
                case TypeCode.Int32:
                    return a => a <= int.MaxValue;
                case TypeCode.UInt32:
                    return a => a <= uint.MaxValue;
                case TypeCode.Int64:
                    return a => a <= long.MaxValue;
                case TypeCode.UInt64:
                    return a => true;
            }
            return null;
        }

        internal static Delegate ToSByte(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, sbyte>)Convert.ToSByte;
                case TypeCode.Byte:
                    return (Func<byte, sbyte>)Convert.ToSByte;
                case TypeCode.Int16:
                    return (Func<short, sbyte>)Convert.ToSByte;
                case TypeCode.UInt16:
                    return (Func<ushort, sbyte>)Convert.ToSByte;
                case TypeCode.Int32:
                    return (Func<int, sbyte>)Convert.ToSByte;
                case TypeCode.UInt32:
                    return (Func<uint, sbyte>)Convert.ToSByte;
                case TypeCode.Int64:
                    return (Func<long, sbyte>)Convert.ToSByte;
                case TypeCode.UInt64:
                    return (Func<ulong, sbyte>)Convert.ToSByte;
            }
            return null;
        }

        internal static Delegate ToByte(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, byte>)Convert.ToByte;
                case TypeCode.Byte:
                    return (Func<byte, byte>)Convert.ToByte;
                case TypeCode.Int16:
                    return (Func<short, byte>)Convert.ToByte;
                case TypeCode.UInt16:
                    return (Func<ushort, byte>)Convert.ToByte;
                case TypeCode.Int32:
                    return (Func<int, byte>)Convert.ToByte;
                case TypeCode.UInt32:
                    return (Func<uint, byte>)Convert.ToByte;
                case TypeCode.Int64:
                    return (Func<long, byte>)Convert.ToByte;
                case TypeCode.UInt64:
                    return (Func<ulong, byte>)Convert.ToByte;
            }
            return null;
        }

        internal static Delegate ToInt16(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, short>)Convert.ToInt16;
                case TypeCode.Byte:
                    return (Func<byte, short>)Convert.ToInt16;
                case TypeCode.Int16:
                    return (Func<short, short>)Convert.ToInt16;
                case TypeCode.UInt16:
                    return (Func<ushort, short>)Convert.ToInt16;
                case TypeCode.Int32:
                    return (Func<int, short>)Convert.ToInt16;
                case TypeCode.UInt32:
                    return (Func<uint, short>)Convert.ToInt16;
                case TypeCode.Int64:
                    return (Func<long, short>)Convert.ToInt16;
                case TypeCode.UInt64:
                    return (Func<ulong, short>)Convert.ToInt16;
            }
            return null;
        }

        internal static Delegate ToUInt16(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, ushort>)Convert.ToUInt16;
                case TypeCode.Byte:
                    return (Func<byte, ushort>)Convert.ToUInt16;
                case TypeCode.Int16:
                    return (Func<short, ushort>)Convert.ToUInt16;
                case TypeCode.UInt16:
                    return (Func<ushort, ushort>)Convert.ToUInt16;
                case TypeCode.Int32:
                    return (Func<int, ushort>)Convert.ToUInt16;
                case TypeCode.UInt32:
                    return (Func<uint, ushort>)Convert.ToUInt16;
                case TypeCode.Int64:
                    return (Func<long, ushort>)Convert.ToUInt16;
                case TypeCode.UInt64:
                    return (Func<ulong, ushort>)Convert.ToUInt16;
            }
            return null;
        }

        internal static Delegate ToInt32(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, int>)Convert.ToInt32;
                case TypeCode.Byte:
                    return (Func<byte, int>)Convert.ToInt32;
                case TypeCode.Int16:
                    return (Func<short, int>)Convert.ToInt32;
                case TypeCode.UInt16:
                    return (Func<ushort, int>)Convert.ToInt32;
                case TypeCode.Int32:
                    return (Func<int, int>)Convert.ToInt32;
                case TypeCode.UInt32:
                    return (Func<uint, int>)Convert.ToInt32;
                case TypeCode.Int64:
                    return (Func<long, int>)Convert.ToInt32;
                case TypeCode.UInt64:
                    return (Func<ulong, int>)Convert.ToInt32;
            }
            return null;
        }

        internal static Delegate ToUInt32(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, uint>)Convert.ToUInt32;
                case TypeCode.Byte:
                    return (Func<byte, uint>)Convert.ToUInt32;
                case TypeCode.Int16:
                    return (Func<short, uint>)Convert.ToUInt32;
                case TypeCode.UInt16:
                    return (Func<ushort, uint>)Convert.ToUInt32;
                case TypeCode.Int32:
                    return (Func<int, uint>)Convert.ToUInt32;
                case TypeCode.UInt32:
                    return (Func<uint, uint>)Convert.ToUInt32;
                case TypeCode.Int64:
                    return (Func<long, uint>)Convert.ToUInt32;
                case TypeCode.UInt64:
                    return (Func<ulong, uint>)Convert.ToUInt32;
            }
            return null;
        }

        internal static Delegate ToInt64(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, long>)Convert.ToInt64;
                case TypeCode.Byte:
                    return (Func<byte, long>)Convert.ToInt64;
                case TypeCode.Int16:
                    return (Func<short, long>)Convert.ToInt64;
                case TypeCode.UInt16:
                    return (Func<ushort, long>)Convert.ToInt64;
                case TypeCode.Int32:
                    return (Func<int, long>)Convert.ToInt64;
                case TypeCode.UInt32:
                    return (Func<uint, long>)Convert.ToInt64;
                case TypeCode.Int64:
                    return (Func<long, long>)Convert.ToInt64;
                case TypeCode.UInt64:
                    return (Func<ulong, long>)Convert.ToInt64;
            }
            return null;
        }

        internal static Delegate ToUInt64(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, ulong>)Convert.ToUInt64;
                case TypeCode.Byte:
                    return (Func<byte, ulong>)Convert.ToUInt64;
                case TypeCode.Int16:
                    return (Func<short, ulong>)Convert.ToUInt64;
                case TypeCode.UInt16:
                    return (Func<ushort, ulong>)Convert.ToUInt64;
                case TypeCode.Int32:
                    return (Func<int, ulong>)Convert.ToUInt64;
                case TypeCode.UInt32:
                    return (Func<uint, ulong>)Convert.ToUInt64;
                case TypeCode.Int64:
                    return (Func<long, ulong>)Convert.ToUInt64;
                case TypeCode.UInt64:
                    return (Func<ulong, ulong>)Convert.ToUInt64;
            }
            return null;
        }

        internal static Delegate ToStringFormat(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (Func<sbyte, string, string>)((a, b) => a.ToString(b));
                case TypeCode.Byte:
                    return (Func<byte, string, string>)((a, b) => a.ToString(b));
                case TypeCode.Int16:
                    return (Func<short, string, string>)((a, b) => a.ToString(b));
                case TypeCode.UInt16:
                    return (Func<ushort, string, string>)((a, b) => a.ToString(b));
                case TypeCode.Int32:
                    return (Func<int, string, string>)((a, b) => a.ToString(b));
                case TypeCode.UInt32:
                    return (Func<uint, string, string>)((a, b) => a.ToString(b));
                case TypeCode.Int64:
                    return (Func<long, string, string>)((a, b) => a.ToString(b));
                case TypeCode.UInt64:
                    return (Func<ulong, string, string>)((a, b) => a.ToString(b));
            }
            return null;
        }

        internal static Delegate TryParseMethod(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (IntegralTryParse<sbyte>)sbyte.TryParse;
                case TypeCode.Byte:
                    return (IntegralTryParse<byte>)byte.TryParse;
                case TypeCode.Int16:
                    return (IntegralTryParse<short>)short.TryParse;
                case TypeCode.UInt16:
                    return (IntegralTryParse<ushort>)ushort.TryParse;
                case TypeCode.Int32:
                    return (IntegralTryParse<int>)int.TryParse;
                case TypeCode.UInt32:
                    return (IntegralTryParse<uint>)uint.TryParse;
                case TypeCode.Int64:
                    return (IntegralTryParse<long>)long.TryParse;
                case TypeCode.UInt64:
                    return (IntegralTryParse<ulong>)ulong.TryParse;
            }
            return null;
        }

        internal static string HexFormatString(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return "X2";
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    return "X4";
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return "X8";
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return "X16";
            }
            return null;
        }

        internal static object Zero(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (sbyte)0;
                case TypeCode.Byte:
                    return (byte)0;
                case TypeCode.Int16:
                    return (short)0;
                case TypeCode.UInt16:
                    return (ushort)0;
                case TypeCode.Int32:
                    return 0;
                case TypeCode.UInt32:
                    return 0U;
                case TypeCode.Int64:
                    return 0L;
                case TypeCode.UInt64:
                    return 0UL;
            }
            return null;
        }

        internal static object One(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return (sbyte)1;
                case TypeCode.Byte:
                    return (byte)1;
                case TypeCode.Int16:
                    return (short)1;
                case TypeCode.UInt16:
                    return (ushort)1;
                case TypeCode.Int32:
                    return 1;
                case TypeCode.UInt32:
                    return 1U;
                case TypeCode.Int64:
                    return 1L;
                case TypeCode.UInt64:
                    return 1UL;
            }
            return null;
        }
    }
}