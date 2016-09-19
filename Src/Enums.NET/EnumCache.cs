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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using EnumsNET.Numerics;

#if !NET20
using System.Runtime.Serialization;
#endif

#if !(NET20 || NET35 || NETSTANDARD10)
using System.Collections.Concurrent;
#else
using EnumsNET.Collections;
#endif

namespace EnumsNET
{
    internal sealed class EnumCache<TInt, TIntProvider>
        where TInt : struct, IFormattable, IConvertible, IComparable<TInt>, IEquatable<TInt>
        where TIntProvider : struct, INumericProvider<TInt>
    {
        #region Static
        private static readonly TIntProvider _provider = new TIntProvider();

        private static bool IsPowerOfTwo(TInt x) => _provider.And(x, _provider.Subtract(x, _provider.One)).Equals(_provider.Zero);

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

        internal readonly IEnumInfoInternal<TInt, TIntProvider> EnumInfo;

        private readonly string _enumTypeName;

        private readonly TInt _maxDefined;

        private readonly TInt _minDefined;

        // The main collection of values, names, and attributes with ~O(1) retrieval on name or value
        // If constant contains a DescriptionAttribute it will be the first in the attribute array
        private readonly Dictionary<TInt, EnumMemberInternal<TInt, TIntProvider>> _valueMap;

        // Duplicate values are stored here with a key of the constant's name, is null if no duplicates
        private readonly List<EnumMemberInternal<TInt, TIntProvider>> _duplicateValues;

        private ConcurrentDictionary<EnumFormat, EnumMemberParser> _enumMemberParsers;
        #endregion

        public EnumCache(Type enumType, IEnumInfoInternal<TInt, TIntProvider> enumInfo)
        {
            _enumTypeName = enumType.Name;
            EnumInfo = enumInfo;
            IsFlagEnum = enumType.IsDefined(typeof(FlagsAttribute), false);

            var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            _valueMap = new Dictionary<TInt, EnumMemberInternal<TInt, TIntProvider>>(fields.Length);
            if (fields.Length == 0)
            {
                return;
            }
            var duplicateValues = new List<EnumMemberInternal<TInt, TIntProvider>>();
            foreach (var field in fields)
            {
                var value = (TInt)field.GetValue(null);
                var name = field.Name;
                var attributes = Attribute.GetCustomAttributes(field, false);
                var isPrimary = false;
                foreach (var attribute in attributes)
                {
                    if (attribute is PrimaryEnumMemberAttribute)
                    {
                        isPrimary = true;
                        break;
                    }
                }
                var member = new EnumMemberInternal<TInt, TIntProvider>(value, name, attributes, this);
                EnumMemberInternal<TInt, TIntProvider> existing;
                if (_valueMap.TryGetValue(value, out existing))
                {
                    if (isPrimary)
                    {
                        _valueMap[value] = member;
                        member = existing;
                    }
                    duplicateValues.Add(member);
                }
                else
                {
                    _valueMap.Add(value, member);
                    if (IsPowerOfTwo(value))
                    {
                        AllFlags = _provider.Or(AllFlags, value);
                    }
                }
            }

            // Makes sure is in increasing value order, due to no removals
            var values = _valueMap.ToArray();
            Array.Sort(values, (first, second) => first.Key.CompareTo(second.Key));
            _valueMap = new Dictionary<TInt, EnumMemberInternal<TInt, TIntProvider>>(_valueMap.Count);
            foreach (var pair in values)
            {
                _valueMap.Add(pair.Key, pair.Value);
            }

            _maxDefined = values[values.Length - 1].Key;
            _minDefined = values[0].Key;
            IsContiguous = _provider.Subtract(_maxDefined, _provider.Create(_valueMap.Count - 1)).Equals(_minDefined);

            if (duplicateValues.Count > 0)
            {
                // Makes sure is in increasing order
                duplicateValues.Sort((first, second) => first.Value.CompareTo(second.Value));
                _duplicateValues = duplicateValues;
            }
        }

        #region Standard Enum Operations
        #region Type Methods
        public int GetEnumMemberCount(bool uniqueValued) => _valueMap.Count + (uniqueValued ? 0 : _duplicateValues?.Count ?? 0);

        public IEnumerable<EnumMemberInternal<TInt, TIntProvider>> GetEnumMembers(bool uniqueValued) => uniqueValued || _duplicateValues == null
            ? _valueMap.Values
            : GetEnumMembersInternal();

        public IEnumerable<string> GetNames(bool uniqueValued) => GetEnumMembers(uniqueValued).Select(member => member.Name);

        private IEnumerable<EnumMemberInternal<TInt, TIntProvider>> GetEnumMembersInternal()
        {
            using (var primaryEnumerator = _valueMap.GetEnumerator())
            {
                var primaryIsActive = primaryEnumerator.MoveNext();
                var primaryMember = primaryEnumerator.Current.Value;
                using (var duplicateEnumerator = _duplicateValues.GetEnumerator())
                {
                    var duplicateIsActive = duplicateEnumerator.MoveNext();
                    var duplicateMember = duplicateEnumerator.Current;
                    while (primaryIsActive || duplicateIsActive)
                    {
                        if (duplicateIsActive && (!primaryIsActive || _provider.LessThan(duplicateMember.Value, primaryMember.Value)))
                        {
                            yield return duplicateMember;
                            if (duplicateIsActive = duplicateEnumerator.MoveNext())
                            {
                                duplicateMember = duplicateEnumerator.Current;
                            }
                        }
                        else
                        {
                            yield return primaryMember;
                            if (primaryIsActive = primaryEnumerator.MoveNext())
                            {
                                primaryMember = primaryEnumerator.Current.Value;
                            }
                        }
                    }
                }
            }
        }
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
            if (!_provider.IsInValueRange(value))
            {
                throw new OverflowException("value is outside the underlying type's value range");
            }

            var result = _provider.Create(value);
            if (validate)
            {
                Validate(result, nameof(value));
            }
            return result;
        }

        public TInt ToObject(ulong value, bool validate)
        {
            if (!_provider.IsInValueRange(value))
            {
                throw new OverflowException("value is outside the underlying type's value range");
            }

            var result = _provider.Create(value);
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
                    return !validate || IsValid(result);
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
            result = _provider.Zero;
            return false;
        }

        public bool TryToObject(long value, out TInt result, bool validate)
        {
            if (_provider.IsInValueRange(value))
            {
                result = _provider.Create(value);
                return !validate || IsValid(result);
            }
            result = _provider.Zero;
            return false;
        }

        public bool TryToObject(ulong value, out TInt result, bool validate)
        {
            if (_provider.IsInValueRange(value))
            {
                result = _provider.Create(value);
                return !validate || IsValid(result);
            }
            result = _provider.Zero;
            return false;
        }
        #endregion

        #region All Values Main Methods
        public bool IsValid(TInt value) => EnumInfo.CustomValidate(value) ?? (IsFlagEnum && IsValidFlagCombination(value)) || IsDefined(value);

        public bool IsDefined(TInt value) => IsContiguous ? !(_provider.LessThan(value, _minDefined) || _provider.LessThan(_maxDefined, value)) : _valueMap.ContainsKey(value);

        public void Validate(TInt value, string paramName)
        {
            if (!IsValid(value))
            {
                throw new ArgumentException($"invalid value of {AsString(value)} for {_enumTypeName}", paramName);
            }
        }

        public string AsString(TInt value) => AsStringInternal(value, null);

        internal string AsStringInternal(TInt value, EnumMemberInternal<TInt, TIntProvider> member) => IsFlagEnum ? FormatFlagsInternal(value, member, null, null) : FormatInternal(value, member, EnumFormat.Name, EnumFormat.DecimalValue);

        public string AsString(TInt value, EnumFormat format)
        {
            var isInitialized = false;
            EnumMemberInternal<TInt, TIntProvider> member = null;
            return FormatInternal(value, ref isInitialized, ref member, format);
        }

        public string AsString(TInt value, EnumFormat format0, EnumFormat format1) => FormatInternal(value, null, format0, format1);

        public string AsString(TInt value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FormatInternal(value, null, format0, format1, format2);

        public string AsString(TInt value, EnumFormat[] formatOrder) => AsStringInternal(value, null, formatOrder);

        internal string AsStringInternal(TInt value, EnumMemberInternal<TInt, TIntProvider> member, EnumFormat[] formatOrder) => formatOrder?.Length > 0 ? FormatInternal(value, member, formatOrder) : AsStringInternal(value, member);

        public string AsString(TInt value, string format) => AsStringInternal(value, null, format);

        internal string AsStringInternal(TInt value, EnumMemberInternal<TInt, TIntProvider> member, string format) => string.IsNullOrEmpty(format) ? AsStringInternal(value, member) : FormatInternal(value, member, format);

        public string Format(TInt value, EnumFormat[] formatOrder)
        {
            Preconditions.NotNull(formatOrder, nameof(formatOrder));

            return FormatInternal(value, null, formatOrder);
        }

        public string Format(TInt value, string format)
        {
            Preconditions.NotNull(format, nameof(format));

            return FormatInternal(value, null, format);
        }

        internal string FormatInternal(TInt value, EnumMemberInternal<TInt, TIntProvider> member, string format)
        {
            switch (format)
            {
                case "G":
                case "g":
                    return AsStringInternal(value, member);
                case "F":
                case "f":
                    return FormatFlagsInternal(value, member, null, null);
                case "D":
                case "d":
                    return value.ToString("D", null);
                case "X":
                case "x":
                    return value.ToString(_provider.HexFormatString, null);
            }
            throw new FormatException("format string can be only \"G\", \"g\", \"X\", \"x\", \"F\", \"f\", \"D\" or \"d\".");
        }

        internal string FormatInternal(TInt value, ref bool isInitialized, ref EnumMemberInternal<TInt, TIntProvider> member, EnumFormat format)
        {
            if (format == EnumFormat.DecimalValue || format == EnumFormat.HexadecimalValue)
            {
                return value.ToString(format == EnumFormat.DecimalValue ? "D" : _provider.HexFormatString, null);
            }
            if (!isInitialized)
            {
                member = GetEnumMember(value);
                isInitialized = true;
            }
            switch (format)
            {
                case EnumFormat.Name:
                    return member?.Name;
                case EnumFormat.Description:
                    return member?.GetAttribute<DescriptionAttribute>()?.Description;
#if !NET20
                case EnumFormat.EnumMemberValue:
                    return member?.GetAttribute<EnumMemberAttribute>()?.Value;
#endif
                default:
                    return Enums.CustomEnumMemberFormat(member?.EnumMember, format);
            }
        }

        internal string FormatInternal(TInt value, EnumMemberInternal<TInt, TIntProvider> member, EnumFormat format0, EnumFormat format1)
        {
            var isInitialized = member != null;
            return FormatInternal(value, ref isInitialized, ref member, format0) ?? FormatInternal(value, ref isInitialized, ref member, format1);
        }

        internal string FormatInternal(TInt value, EnumMemberInternal<TInt, TIntProvider> member, EnumFormat format0, EnumFormat format1, EnumFormat format2)
        {
            var isInitialized = member != null;
            return FormatInternal(value, ref isInitialized, ref member, format0) ?? FormatInternal(value, ref isInitialized, ref member, format1) ?? FormatInternal(value, ref isInitialized, ref member, format2);
        }

        internal string FormatInternal(TInt value, EnumMemberInternal<TInt, TIntProvider> member, EnumFormat[] formatOrder)
        {
            var isInitialized = member != null;
            foreach (var format in formatOrder)
            {
                var formattedValue = FormatInternal(value, ref isInitialized, ref member, format);
                if (formattedValue != null)
                {
                    return formattedValue;
                }
            }
            return null;
        }
        #endregion

        #region Defined Values Main Methods
        public EnumMemberInternal<TInt, TIntProvider> GetEnumMember(TInt value)
        {
            EnumMemberInternal<TInt, TIntProvider> member;
            _valueMap.TryGetValue(value, out member);
            return member;
        }

        public EnumMemberInternal<TInt, TIntProvider> GetEnumMember(string name, bool ignoreCase)
        {
            Preconditions.NotNull(name, nameof(name));

            TInt result;
            EnumMemberInternal<TInt, TIntProvider> member;
            TryParseInternal(name, ignoreCase, out result, out member, Enums.NameFormatArray, false);
            return member;
        }

        public string GetName(TInt value) => GetEnumMember(value)?.Name;
        #endregion

        #region Attributes
        public TAttribute GetAttribute<TAttribute>(TInt value)
            where TAttribute : Attribute => GetEnumMember(value)?.GetAttribute<TAttribute>();

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(TInt value)
            where TAttribute : Attribute => GetEnumMember(value)?.GetAttributes<TAttribute>();

        public IEnumerable<Attribute> GetAttributes(TInt value) => GetEnumMember(value)?.Attributes;
        #endregion

        #region Parsing
        public TInt Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder)
        {
            if (IsFlagEnum)
            {
                return ParseFlags(value, ignoreCase, null, parseFormatOrder);
            }

            Preconditions.NotNull(value, nameof(value));

            value = value.Trim();
            
            if (!(parseFormatOrder?.Length > 0))
            {
                parseFormatOrder = Enums.DefaultFormatOrder;
            }

            TInt result;
            EnumMemberInternal<TInt, TIntProvider> member;
            if (TryParseInternal(value, ignoreCase, out result, out member, parseFormatOrder, true))
            {
                return result;
            }
            if (IsNumeric(value))
            {
                throw new OverflowException("value is outside the underlying type's value range");
            }
            throw new ArgumentException($"string was not recognized as being a member of {_enumTypeName}", nameof(value));
        }

        public EnumMemberInternal<TInt, TIntProvider> ParseMember(string value, bool ignoreCase, EnumFormat[] parseFormatOrder)
        {
            Preconditions.NotNull(value, nameof(value));

            value = value.Trim();
            
            if (!(parseFormatOrder?.Length > 0))
            {
                parseFormatOrder = Enums.DefaultFormatOrder;
            }

            TInt valueAsTInt;
            EnumMemberInternal<TInt, TIntProvider> result;
            if (TryParseInternal(value, ignoreCase, out valueAsTInt, out result, parseFormatOrder, false))
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
            if (IsFlagEnum)
            {
                return TryParseFlags(value, ignoreCase, null, out result, parseFormatOrder);
            }

            if (value != null)
            {
                value = value.Trim();

                if (!(parseFormatOrder?.Length > 0))
                {
                    parseFormatOrder = Enums.DefaultFormatOrder;
                }

                EnumMemberInternal<TInt, TIntProvider> member;
                return TryParseInternal(value, ignoreCase, out result, out member, parseFormatOrder, true);
            }
            result = _provider.Zero;
            return false;
        }

        public bool TryParseMember(string value, bool ignoreCase, out EnumMemberInternal<TInt, TIntProvider> result, EnumFormat[] parseFormatOrder)
        {
            if (value != null)
            {
                value = value.Trim();

                if (!(parseFormatOrder?.Length > 0))
                {
                    parseFormatOrder = Enums.DefaultFormatOrder;
                }

                TInt valueAsTInt;
                return TryParseInternal(value, ignoreCase, out valueAsTInt, out result, parseFormatOrder, false);
            }
            result = null;
            return false;
        }

        private bool TryParseInternal(string value, bool ignoreCase, out TInt result, out EnumMemberInternal<TInt, TIntProvider> member, EnumFormat[] parseFormatOrder, bool getValueOnly)
        {
            foreach (var format in parseFormatOrder)
            {
                if (format == EnumFormat.DecimalValue || format == EnumFormat.HexadecimalValue)
                {
                    if (_provider.TryParse(value, format == EnumFormat.DecimalValue ? NumberStyles.AllowLeadingSign : NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out result))
                    {
                        member = getValueOnly ? null : GetEnumMember(result);
                        return true;
                    }
                }
                else
                {
                    EnumMemberParser parser;
                    var enumMemberParsers = _enumMemberParsers;
                    if (enumMemberParsers == null || !enumMemberParsers.TryGetValue(format, out parser))
                    {
                        format.Validate(nameof(format));
                        parser = new EnumMemberParser(format, this);
                        enumMemberParsers = enumMemberParsers ?? Interlocked.CompareExchange(ref _enumMemberParsers, (enumMemberParsers = new ConcurrentDictionary<EnumFormat, EnumMemberParser>(EnumComparer<EnumFormat>.Instance)), null) ?? enumMemberParsers;
                        enumMemberParsers.TryAdd(format, parser);
                    }
                    if (parser.TryParse(value, ignoreCase, out member))
                    {
                        result = member.Value;
                        return true;
                    }
                }
            }
            result = default(TInt);
            member = null;
            return false;
        }
        #endregion
        #endregion

        #region Flag Enum Operations
        #region Main Methods
        public bool IsValidFlagCombination(TInt value) => _provider.And(AllFlags, value).Equals(value);

        public string FormatFlags(TInt value, string delimiter, EnumFormat[] formatOrder) => FormatFlagsInternal(value, null, delimiter, formatOrder);

        internal string FormatFlagsInternal(TInt value, EnumMemberInternal<TInt, TIntProvider> member, string delimiter, EnumFormat[] formatOrder)
        {
            if (!(formatOrder?.Length > 0))
            {
                formatOrder = Enums.DefaultFormatOrder;
            }

            if (member == null)
            {
                member = GetEnumMember(value);
            }

            if (member != null || value.Equals(_provider.Zero) || !IsValidFlagCombination(value))
            {
                return FormatInternal(value, member, formatOrder);
            }

            if (string.IsNullOrEmpty(delimiter))
            {
                delimiter = FlagEnums.DefaultDelimiter;
            }

            return string.Join(delimiter,
                GetFlags(value).Select(flag => FormatInternal(flag, null, formatOrder))
#if NET20 || NET35
                .ToArray()
#endif
                );
        }

        public IEnumerable<TInt> GetFlags(TInt value)
        {
            var validValue = _provider.And(value, AllFlags);
            var isLessThanZero = _provider.LessThan(validValue, _provider.Zero);
            for (var currentValue = _provider.One; (isLessThanZero || !_provider.LessThan(validValue, currentValue)) && !currentValue.Equals(_provider.Zero); currentValue = _provider.LeftShift(currentValue, 1))
            {
                if (HasAnyFlags(validValue, currentValue))
                {
                    yield return currentValue;
                }
            }
        }

        public IEnumerable<EnumMemberInternal<TInt, TIntProvider>> GetFlagMembers(TInt value) => GetFlags(value).Select(flag => GetEnumMember(flag));

        public bool HasAnyFlags(TInt value) => !value.Equals(_provider.Zero);

        public bool HasAnyFlags(TInt value, TInt otherFlags) => !_provider.And(value, otherFlags).Equals(_provider.Zero);

        public bool HasAllFlags(TInt value) => HasAllFlags(value, AllFlags);

        public bool HasAllFlags(TInt value, TInt otherFlags) => _provider.And(value, otherFlags).Equals(otherFlags);

        public TInt ToggleFlags(TInt value) => _provider.Xor(value, AllFlags);

        public TInt ToggleFlags(TInt value, TInt otherFlags) => _provider.Xor(value, otherFlags);

        public TInt CommonFlags(TInt value, TInt otherFlags) => _provider.And(value, otherFlags);

        public TInt CombineFlags(TInt value, TInt otherFlags) => _provider.Or(value, otherFlags);

        public TInt CombineFlags(TInt flag0, TInt flag1, TInt flag2) => _provider.Or(_provider.Or(flag0, flag1), flag2);

        public TInt CombineFlags(TInt flag0, TInt flag1, TInt flag2, TInt flag3) => _provider.Or(_provider.Or(_provider.Or(flag0, flag1), flag2), flag3);

        public TInt CombineFlags(TInt flag0, TInt flag1, TInt flag2, TInt flag3, TInt flag4) => _provider.Or(_provider.Or(_provider.Or(_provider.Or(flag0, flag1), flag2), flag3), flag4);

        public TInt CombineFlags(IEnumerable<TInt> flags)
        {
            Preconditions.NotNull(flags, nameof(flags));

            var combinedFlags = _provider.Zero;
            foreach (var flag in flags)
            {
                combinedFlags = _provider.Or(combinedFlags, flag);
            }
            return combinedFlags;
        }

        public TInt ExcludeFlags(TInt value, TInt otherFlags) => _provider.And(value, _provider.Not(otherFlags));
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

            var result = _provider.Zero;
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
                TInt valueAsTInt;
                EnumMemberInternal<TInt, TIntProvider> member;
                if (TryParseInternal(indValue, ignoreCase, out valueAsTInt, out member, parseFormatOrder, true))
                {
                    result = _provider.Or(result, valueAsTInt);
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
                result = _provider.Zero;
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

            var resultAsInt = _provider.Zero;
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
                TInt valueAsTInt;
                EnumMemberInternal<TInt, TIntProvider> member;
                if (!TryParseInternal(indValue, ignoreCase, out valueAsTInt, out member, parseFormatOrder, true))
                {
                    result = _provider.Zero;
                    return false;
                }
                resultAsInt = _provider.Or(resultAsInt, valueAsTInt);
                startIndex = newStartIndex;
            }
            result = resultAsInt;
            return true;
        }
        #endregion
        #endregion

        #region Nested Types
        internal sealed class EnumMemberParser
        {
            private readonly Dictionary<string, EnumMemberInternal<TInt, TIntProvider>> _formatValueMap;
            private Dictionary<string, EnumMemberInternal<TInt, TIntProvider>> _formatIgnoreCase;

            private Dictionary<string, EnumMemberInternal<TInt, TIntProvider>> FormatIgnoreCase
            {
                get
                {
                    var formatIgnoreCase = _formatIgnoreCase;
                    if (formatIgnoreCase == null)
                    {
                        formatIgnoreCase = new Dictionary<string, EnumMemberInternal<TInt, TIntProvider>>(_formatValueMap.Count, StringComparer.OrdinalIgnoreCase);
                        foreach (var pair in _formatValueMap)
                        {
                            formatIgnoreCase[pair.Key] = pair.Value;
                        }
                        
                        _formatIgnoreCase = formatIgnoreCase;
                    }
                    return formatIgnoreCase;
                }
            }

            public EnumMemberParser(EnumFormat format, EnumCache<TInt, TIntProvider> enumCache)
            {
                _formatValueMap = new Dictionary<string, EnumMemberInternal<TInt, TIntProvider>>(enumCache.GetEnumMemberCount(false), StringComparer.Ordinal);
                foreach (var member in enumCache.GetEnumMembers(false))
                {
                    var formattedValue = member.AsString(format);
                    if (formattedValue != null)
                    {
                        _formatValueMap[formattedValue] = member;
                    }
                }
            }

            internal bool TryParse(string formattedValue, bool ignoreCase, out EnumMemberInternal<TInt, TIntProvider> result) => _formatValueMap.TryGetValue(formattedValue, out result) || (ignoreCase && FormatIgnoreCase.TryGetValue(formattedValue, out result));
        }
        #endregion
    }
}