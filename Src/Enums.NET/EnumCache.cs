#region License
// Copyright (c) 2016 Tyler Brinkley
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using EnumsNET.Collections;
using EnumsNET.Numerics;

namespace EnumsNET
{
    internal sealed class EnumCache<TInt, TIntProvider>
        where TInt : struct, IFormattable, IConvertible, IComparable<TInt>, IEquatable<TInt>
        where TIntProvider : struct, INumericProvider<TInt>
    {
        #region Static
        internal static readonly TIntProvider Provider = new TIntProvider();

        private static bool IsPowerOfTwo(TInt x) => Provider.And(x, Provider.Subtract(x, Provider.One)).Equals(Provider.Zero);

        private static bool IsNumeric(string value)
        {
            char firstChar;
            return value.Length > 0 && (char.IsDigit((firstChar = value[0])) || firstChar == '-' || firstChar == '+');
        }
        #endregion

        #region Fields
        internal readonly TInt AllFlags;

        internal readonly bool IsFlagEnum;

        internal readonly bool IsContiguous;

        private readonly string _enumTypeName;

        private readonly TInt _maxDefined;

        private readonly TInt _minDefined;

        private readonly Func<EnumFormat, Func<InternalEnumMember<TInt, TIntProvider>, string>> _getCustomEnumFormatter;

        private readonly Func<TInt, bool> _customValidator;

        // The main collection of values, names, and attributes with ~O(1) retrieval on name or value
        // If constant contains a DescriptionAttribute it will be the first in the attribute array
        private readonly OrderedBiDirectionalDictionary<TInt, NameAndAttributes> _valueMap;

        // Duplicate values are stored here with a key of the constant's name, is null if no duplicates
        private readonly Dictionary<string, ValueAndAttributes<TInt>> _duplicateValues;

        private Dictionary<string, string> _ignoreCaseSet;

        private ThreadSafeDictionary<EnumFormat, EnumParser> _customEnumParsers;
        #endregion

        #region Properties
        // Enables case insensitive parsing, lazily instantiated to reduce memory usage if not going to use this feature, is thread-safe as it's only used for retrieval
        private Dictionary<string, string> IgnoreCaseSet
        {
            get
            {
                if (_ignoreCaseSet == null)
                {
                    var ignoreCaseSet = new Dictionary<string, string>(GetEnumMemberCount(false), StringComparer.OrdinalIgnoreCase);
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

        public EnumCache(Type enumType, Func<EnumFormat, Func<InternalEnumMember<TInt, TIntProvider>, string>> getCustomEnumFormatter, Func<TInt, bool> customValidator)
        {
            Debug.Assert(enumType != null);
            Debug.Assert(enumType.IsEnum);
            _enumTypeName = enumType.Name;
            Debug.Assert(getCustomEnumFormatter != null);
            _getCustomEnumFormatter = getCustomEnumFormatter;
            _customValidator = customValidator;
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
                var isPrimaryDupe = false;
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
                                    if (isPrimaryDupe)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        if (!isPrimaryDupe && (attr as PrimaryEnumMemberAttribute) != null)
                        {
                            isPrimaryDupe = true;
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
                        if (!Provider.LessThan(value, mapValue))
                        {
                            break;
                        }
                    }
                    _valueMap.Insert(index, value, new NameAndAttributes(name, attributes));
                    if (IsPowerOfTwo(value))
                    {
                        AllFlags = Provider.Or(AllFlags, value);
                    }
                }
                else
                {
                    if (isPrimaryDupe)
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
            IsContiguous = Provider.Subtract(_maxDefined, Provider.Create(_valueMap.Count - 1)).Equals(_minDefined);

            _valueMap.TrimExcess();
            if (duplicateValues.Count > 0)
            {
                // Makes sure is in increasing order, due to no removals
#if NET20
                var dupes = duplicateValues.ToArray();
                Array.Sort(dupes, (first, second) => first.Value.Value.CompareTo(second.Value.Value));
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
        public int GetEnumMemberCount(bool uniqueValued) => _valueMap.Count + (uniqueValued ? 0 : _duplicateValues?.Count ?? 0);

        public IEnumerable<InternalEnumMember<TInt, TIntProvider>> GetEnumMembers(bool uniqueValued)
        {
            if (uniqueValued || _duplicateValues == null)
            {
                return _valueMap.Select(pair => new InternalEnumMember<TInt, TIntProvider>(pair.First, pair.Second.Name, pair.Second.Attributes, this));
            }
            else
            {
                return GetAllEnumMembersInValueOrder();
            }
        }

        public IEnumerable<string> GetNames(bool uniqueValued) => GetEnumMembers(uniqueValued).Select(member => member.Name);

        public IEnumerable<TInt> GetValues(bool uniqueValued) => GetEnumMembers(uniqueValued).Select(member => member.Value);

        private IEnumerable<InternalEnumMember<TInt, TIntProvider>> GetAllEnumMembersInValueOrder()
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
                    var count = GetEnumMemberCount(false);
                    for (var i = 0; i < count; ++i)
                    {
                        TInt value;
                        string name;
                        Attribute[] attributes;
                        if (dupeIsActive && (!mainIsActive || Provider.LessThan(dupePair.Value.Value, mainPair.First)))
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
                        yield return new InternalEnumMember<TInt, TIntProvider>(value, name, attributes, this);
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

        public bool IsValid(TInt value) => _customValidator?.Invoke(value) ?? (IsFlagEnum ? IsValidFlagCombination(value) || IsDefined(value) : IsDefined(value));

        public bool IsValid(long value) => Provider.IsInValueRange(value) && IsValid(Provider.Create(value));

        public bool IsValid(ulong value) => Provider.IsInValueRange(value) && IsValid(Provider.Create(value));
        #endregion

        #region IsDefined
        public bool IsDefined(object value)
        {
            Preconditions.NotNull(value, nameof(value));

            TInt result;
            return TryToObject(value, out result, false) && IsDefined(result);
        }

        public bool IsDefined(TInt value) => IsContiguous ? (!(Provider.LessThan(value, _minDefined) || Provider.LessThan(_maxDefined, value))) : _valueMap.ContainsFirst(value);

        public bool IsDefined(string name, bool ignoreCase)
        {
            Preconditions.NotNull(name, nameof(name));

            return _valueMap.ContainsSecond(new NameAndAttributes(name)) || (_duplicateValues?.ContainsKey(name) ?? false) || (ignoreCase && IgnoreCaseSet.ContainsKey(name));
        }

        public bool IsDefined(long value) => Provider.IsInValueRange(value) && IsDefined(Provider.Create(value));

        public bool IsDefined(ulong value) => Provider.IsInValueRange(value) && IsDefined(Provider.Create(value));
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
            if (!Provider.IsInValueRange(value))
            {
                throw new OverflowException("value is outside the underlying type's value range");
            }

            var result = Provider.Create(value);
            if (validate)
            {
                Validate(result, nameof(value));
            }
            return result;
        }

        public TInt ToObject(ulong value, bool validate)
        {
            if (!Provider.IsInValueRange(value))
            {
                throw new OverflowException("value is outside the underlying type's value range");
            }

            var result = Provider.Create(value);
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
            result = Provider.Zero;
            return false;
        }

        public bool TryToObject(long value, out TInt result, bool validate)
        {
            if (Provider.IsInValueRange(value))
            {
                result = Provider.Create(value);
                return !validate || IsValid(result);
            }
            result = Provider.Zero;
            return false;
        }

        public bool TryToObject(ulong value, out TInt result, bool validate)
        {
            if (Provider.IsInValueRange(value))
            {
                result = Provider.Create(value);
                return !validate || IsValid(result);
            }
            result = Provider.Zero;
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

        public string AsString(TInt value) => InternalAsString(GetEnumMember(value));

        internal string InternalAsString(InternalEnumMember<TInt, TIntProvider> member) => IsFlagEnum ? InternalFormatFlags(member, null, null) : InternalFormat(member, EnumFormat.Name, EnumFormat.DecimalValue);

        public string AsString(TInt value, EnumFormat format) => InternalFormat(GetEnumMember(value), format);

        public string AsString(TInt value, EnumFormat format0, EnumFormat format1) => InternalFormat(GetEnumMember(value), format0, format1);

        public string AsString(TInt value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => InternalFormat(GetEnumMember(value), format0, format1, format2);

        public string AsString(TInt value, EnumFormat[] formatOrder) => InternalAsString(GetEnumMember(value), formatOrder);

        internal string InternalAsString(InternalEnumMember<TInt, TIntProvider> member, EnumFormat[] formatOrder) => formatOrder?.Length > 0 ? InternalFormat(member, formatOrder) : InternalAsString(member);

        public string AsString(TInt value, string format) => InternalAsString(GetEnumMember(value), format);

        internal string InternalAsString(InternalEnumMember<TInt, TIntProvider> member, string format) => string.IsNullOrEmpty(format) ? InternalAsString(member) : InternalFormat(member, format);

        public string Format(TInt value, EnumFormat[] formatOrder)
        {
            Preconditions.NotNull(formatOrder, nameof(formatOrder));

            return InternalFormat(GetEnumMember(value), formatOrder);
        }

        public string Format(TInt value, string format)
        {
            Preconditions.NotNull(format, nameof(format));

            return InternalFormat(GetEnumMember(value), format);
        }

        internal string InternalFormat(InternalEnumMember<TInt, TIntProvider> member, string format)
        {
            switch (format)
            {
                case "G":
                case "g":
                    return InternalAsString(member);
                case "F":
                case "f":
                    return InternalFormatFlags(member, null, null);
                case "D":
                case "d":
                    return member.Value.ToString("D", null);
                case "X":
                case "x":
                    return member.Value.ToString(Provider.HexFormatString, null);
            }
            throw new FormatException("format string can be only \"G\", \"g\", \"X\", \"x\", \"F\", \"f\", \"D\" or \"d\".");
        }

        internal string InternalFormat(InternalEnumMember<TInt, TIntProvider> member, EnumFormat format)
        {
            switch (format)
            {
                case EnumFormat.DecimalValue:
                    return member.Value.ToString("D", null);
                case EnumFormat.HexadecimalValue:
                    return member.Value.ToString(Provider.HexFormatString, null);
                case EnumFormat.Name:
                    return member.Name;
                case EnumFormat.Description:
                    return member.Description;
                default:
                    return _getCustomEnumFormatter(format)?.Invoke(member);
            }
        }

        internal string InternalFormat(InternalEnumMember<TInt, TIntProvider> member, EnumFormat format0, EnumFormat format1)
        {
            return InternalFormat(member, format0) ?? InternalFormat(member, format1);
        }

        internal string InternalFormat(InternalEnumMember<TInt, TIntProvider> member, EnumFormat format0, EnumFormat format1, EnumFormat format2)
        {
            return InternalFormat(member, format0) ?? InternalFormat(member, format1) ?? InternalFormat(member, format2);
        }

        internal string InternalFormat(InternalEnumMember<TInt, TIntProvider> member, EnumFormat[] formatOrder)
        {
            foreach (var format in formatOrder)
            {
                var formattedValue = InternalFormat(member, format);
                if (formattedValue != null)
                {
                    return formattedValue;
                }
            }
            return null;
        }
        #endregion

        #region Defined Values Main Methods
        public InternalEnumMember<TInt, TIntProvider> GetEnumMember(TInt value)
        {
            var index = _valueMap.IndexOfFirst(value);
            if (index >= 0)
            {
                var nameAndAttributes = _valueMap.GetSecondAt(index);
                return new InternalEnumMember<TInt, TIntProvider>(value, nameAndAttributes.Name, nameAndAttributes.Attributes, this);
            }
            return new InternalEnumMember<TInt, TIntProvider>(value, null, null, this);
        }

        public InternalEnumMember<TInt, TIntProvider> GetEnumMember(string name, bool ignoreCase)
        {
            Preconditions.NotNull(name, nameof(name));

            return InternalGetEnumMember(name, ignoreCase);
        }

        private InternalEnumMember<TInt, TIntProvider> InternalGetEnumMember(string name, bool ignoreCase)
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
                        return new InternalEnumMember<TInt, TIntProvider>();
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
                    return new InternalEnumMember<TInt, TIntProvider>(valueAndAttributes.Value, name, valueAndAttributes.Attributes, this);
                }
            }
            var pair = _valueMap.GetAt(index);
            return new InternalEnumMember<TInt, TIntProvider>(pair.First, name, pair.Second.Attributes, this);
        }

        public string GetName(TInt value) => GetEnumMember(value).Name;

        public string GetDescription(TInt value) => GetEnumMember(value).Description;

        public string GetDescriptionOrName(TInt value) => GetEnumMember(value).GetDescriptionOrName();

        public string GetDescriptionOrName(TInt value, Func<string, string> nameFormatter) => GetEnumMember(value).GetDescriptionOrName(nameFormatter);
        #endregion

        #region Attributes
        public bool HasAttribute<TAttribute>(TInt value)
            where TAttribute : Attribute => GetEnumMember(value).HasAttribute<TAttribute>();

        public TAttribute GetAttribute<TAttribute>(TInt value)
            where TAttribute : Attribute => GetEnumMember(value).GetAttribute<TAttribute>();

        public TResult GetAttributeSelect<TAttribute, TResult>(TInt value, Func<TAttribute, TResult> selector, TResult defaultValue)
            where TAttribute : Attribute => GetEnumMember(value).GetAttributeSelect(selector, defaultValue);

        public bool TryGetAttributeSelect<TAttribute, TResult>(TInt value, Func<TAttribute, TResult> selector, out TResult result)
            where TAttribute : Attribute => GetEnumMember(value).TryGetAttributeSelect(selector, out result);

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(TInt value)
            where TAttribute : Attribute => GetEnumMember(value).GetAttributes<TAttribute>();

        public IEnumerable<Attribute> GetAttributes(TInt value) => GetEnumMember(value).Attributes;
        #endregion

        #region Parsing
        public TInt Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder)
        {
            Preconditions.NotNull(value, nameof(value));

            value = value.Trim();
            TInt result;
            if (IsFlagEnum)
            {
                return ParseFlags(value, ignoreCase, null, parseFormatOrder);
            }

            if (!(parseFormatOrder?.Length > 0))
            {
                parseFormatOrder = Enums.DefaultFormatOrder;
            }

            if (InternalTryParse(value, ignoreCase, out result, parseFormatOrder))
            {
                return result;
            }
            if (IsNumeric(value))
            {
                throw new OverflowException("value is outside the underlying type's value range");
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
                    return TryParseFlags(value, ignoreCase, null, out result, parseFormatOrder);
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
            result = Provider.Zero;
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
                        success = Provider.TryParse(value, NumberStyles.AllowLeadingSign, null, out result);
                        break;
                    case EnumFormat.HexadecimalValue:
                        success = Provider.TryParse(value, NumberStyles.AllowHexSpecifier, null, out result);
                        break;
                    case EnumFormat.Name:
                        var member = InternalGetEnumMember(value, ignoreCase);
                        if (member.IsDefined)
                        {
                            result = member.Value;
                            success = true;
                        }
                        break;
                    default:
                        EnumParser parser = null;
                        if (_customEnumParsers?.TryGetValue(format, out parser) != true)
                        {
                            if (format == EnumFormat.Description)
                            {
                                parser = new EnumParser(internalMember => internalMember.Description, this);
                            }
                            else
                            {
                                var formatter = _getCustomEnumFormatter(format);
                                if (formatter != null)
                                {
                                    parser = new EnumParser(formatter, this);
                                }
                            }
                            if (parser != null)
                            {
                                var customEnumParsers = _customEnumParsers;
                                if (customEnumParsers == null)
                                {
                                    customEnumParsers = new ThreadSafeDictionary<EnumFormat, EnumParser>(new EnumComparer<EnumFormat>());
                                    customEnumParsers = Interlocked.CompareExchange(ref _customEnumParsers, customEnumParsers, null) ?? customEnumParsers;
                                }
                                customEnumParsers.TryAdd(format, parser);
                            }
                        }
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
        public bool IsValidFlagCombination(TInt value) => Provider.And(AllFlags, value).Equals(value);

        public string FormatFlags(TInt value, string delimiter, EnumFormat[] formatOrder) => InternalFormatFlags(GetEnumMember(value), delimiter, formatOrder);

        internal string InternalFormatFlags(InternalEnumMember<TInt, TIntProvider> member, string delimiter, EnumFormat[] formatOrder)
        {
            if (!(formatOrder?.Length > 0))
            {
                formatOrder = Enums.DefaultFormatOrder;
            }

            if (member.IsDefined || member.Value.Equals(Provider.Zero) || !IsValidFlagCombination(member.Value))
            {
                return InternalFormat(member, formatOrder);
            }

            if (string.IsNullOrEmpty(delimiter))
            {
                delimiter = FlagEnums.DefaultDelimiter;
            }

            return string.Join(delimiter,
                GetFlags(member.Value).Select(flag => InternalFormat(GetEnumMember(flag), formatOrder))
#if NET20 || NET35
                .ToArray()
#endif
                );
        }

        public IEnumerable<TInt> GetFlags(TInt value)
        {
            var validValue = Provider.And(value, AllFlags);
            var isLessThanZero = Provider.LessThan(validValue, Provider.Zero);
            for (var currentValue = Provider.One; (isLessThanZero || !Provider.LessThan(validValue, currentValue)) && !currentValue.Equals(Provider.Zero); currentValue = Provider.LeftShift(currentValue, 1))
            {
                if (HasAnyFlags(validValue, currentValue))
                {
                    yield return currentValue;
                }
            }
        }

        public IEnumerable<InternalEnumMember<TInt, TIntProvider>> GetFlagMembers(TInt value) => GetFlags(value).Select(flag => GetEnumMember(flag));

        public bool HasAnyFlags(TInt value) => !value.Equals(Provider.Zero);

        public bool HasAnyFlags(TInt value, TInt otherFlags) => !Provider.And(value, otherFlags).Equals(Provider.Zero);

        public bool HasAllFlags(TInt value) => HasAllFlags(value, AllFlags);

        public bool HasAllFlags(TInt value, TInt otherFlags) => Provider.And(value, otherFlags).Equals(otherFlags);

        public TInt ToggleFlags(TInt value) => Provider.Xor(value, AllFlags);

        public TInt ToggleFlags(TInt value, TInt otherFlags) => Provider.Xor(value, otherFlags);

        public TInt CommonFlags(TInt value, TInt otherFlags) => Provider.And(value, otherFlags);

        public TInt CombineFlags(TInt value, TInt otherFlags) => Provider.Or(value, otherFlags);

        public TInt CombineFlags(TInt flag0, TInt flag1, TInt flag2) => Provider.Or(Provider.Or(flag0, flag1), flag2);

        public TInt CombineFlags(TInt flag0, TInt flag1, TInt flag2, TInt flag3) => Provider.Or(Provider.Or(Provider.Or(flag0, flag1), flag2), flag3);

        public TInt CombineFlags(TInt flag0, TInt flag1, TInt flag2, TInt flag3, TInt flag4) => Provider.Or(Provider.Or(Provider.Or(Provider.Or(flag0, flag1), flag2), flag3), flag4);

        public TInt CombineFlags(IEnumerable<TInt> flags)
        {
            var combinedFlags = Provider.Zero;
            if (flags != null)
            {
                foreach (var flag in flags)
                {
                    combinedFlags = Provider.Or(combinedFlags, flag);
                }
            }
            return combinedFlags;
        }

        public TInt ExcludeFlags(TInt value, TInt otherFlags) => Provider.And(value, Provider.Not(otherFlags));
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

            if (!(parseFormatOrder?.Length > 0))
            {
                parseFormatOrder = Enums.DefaultFormatOrder;
            }

            var result = Provider.Zero;
            var startIndex = 0;
            var valueLength = value.Length;
            while (startIndex < valueLength)
            {
                while (startIndex < valueLength && char.IsWhiteSpace(value[startIndex]))
                {
                    ++startIndex;
                }
                var delimiterIndex = value.IndexOf(effectiveDelimiter, startIndex, StringComparison.Ordinal);
                if (delimiterIndex < 0)
                {
                    delimiterIndex = valueLength;
                }
                var newStartIndex = delimiterIndex + effectiveDelimiter.Length;
                while (delimiterIndex > startIndex && char.IsWhiteSpace(value[delimiterIndex - 1]))
                {
                    --delimiterIndex;
                }
                var indValue = value.Substring(startIndex, delimiterIndex - startIndex);
                TInt indValueAsInt;
                if (InternalTryParse(indValue, ignoreCase, out indValueAsInt, parseFormatOrder))
                {
                    result = Provider.Or(result, indValueAsInt);
                }
                else
                {
                    if (IsNumeric(indValue))
                    {
                        throw new OverflowException("value is outside the underlying type's value range");
                    }
                    throw new ArgumentException("value is not a valid combination of flag enum values");
                }
                startIndex = newStartIndex;
            }
            return result;
        }

        public bool TryParseFlags(string value, bool ignoreCase, string delimiter, out TInt result, EnumFormat[] parseFormatOrder)
        {
            if (value == null)
            {
                result = Provider.Zero;
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

            if (!(parseFormatOrder?.Length > 0))
            {
                parseFormatOrder = Enums.DefaultFormatOrder;
            }

            var resultAsInt = Provider.Zero;
            var startIndex = 0;
            var valueLength = value.Length;
            while (startIndex < valueLength)
            {
                while (startIndex < valueLength && char.IsWhiteSpace(value[startIndex]))
                {
                    ++startIndex;
                }
                var delimiterIndex = value.IndexOf(effectiveDelimiter, startIndex, StringComparison.Ordinal);
                if (delimiterIndex < 0)
                {
                    delimiterIndex = valueLength;
                }
                var newStartIndex = delimiterIndex + effectiveDelimiter.Length;
                while (delimiterIndex > startIndex && char.IsWhiteSpace(value[delimiterIndex - 1]))
                {
                    --delimiterIndex;
                }
                var indValue = value.Substring(startIndex, delimiterIndex - startIndex);
                TInt indValueAsInt;
                if (!InternalTryParse(indValue, ignoreCase, out indValueAsInt, parseFormatOrder))
                {
                    result = Provider.Zero;
                    return false;
                }
                resultAsInt = Provider.Or(resultAsInt, indValueAsInt);
                startIndex = newStartIndex;
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
                            formatIgnoreCase[pair.Key] = pair.Value;
                        }
                        
                        _formatIgnoreCase = formatIgnoreCase;
                    }
                    return _formatIgnoreCase;
                }
            }

            public EnumParser(Func<InternalEnumMember<TInt, TIntProvider>, string> formatter, EnumCache<TInt, TIntProvider> enumCache)
            {
                _formatValueMap = new Dictionary<string, TInt>(enumCache.GetEnumMemberCount(false));
                foreach (var member in enumCache.GetEnumMembers(false))
                {
                    var formattedValue = formatter(member);
                    if (formattedValue != null)
                    {
                        _formatValueMap[formattedValue] = member.Value;
                    }
                }
            }

            internal bool TryParse(string formattedValue, bool ignoreCase, out TInt result) => _formatValueMap.TryGetValue(formattedValue, out result) || (ignoreCase && FormatIgnoreCase.TryGetValue(formattedValue, out result));
        }
        #endregion
    }
}