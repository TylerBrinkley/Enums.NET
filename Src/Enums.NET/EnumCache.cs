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
using System.Runtime.CompilerServices;

#if ENUM_MEMBER_ATTRIBUTE
using System.Runtime.Serialization;
#endif

#if DISPLAY_ATTRIBUTE
using System.ComponentModel.DataAnnotations;
#endif

namespace EnumsNET
{
    internal sealed class EnumCache<TInt, TIntProvider>
        where TInt : struct, IComparable<TInt>, IEquatable<TInt>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TIntProvider : struct, INumericProvider<TInt>
    {
        #region Static
        internal static readonly TIntProvider Provider = new TIntProvider();

        private static bool IsNumeric(string value)
        {
            char firstChar;
            return value.Length > 0 && (char.IsDigit((firstChar = value[0])) || firstChar == '-' || firstChar == '+');
        }
        #endregion

        #region Fields
        internal readonly TInt AllFlags;

        internal readonly bool IsFlagEnum;

        internal readonly IEnumInfoInternal<TInt, TIntProvider> EnumInfo;

        private readonly bool _isContiguous;

        private readonly bool _hasCustomValidator;

        private readonly string _enumTypeName;

        private readonly TInt _maxDefined;

        private readonly TInt _minDefined;
        
        private readonly Dictionary<TInt, EnumMemberInternal<TInt, TIntProvider>> _valueMap;

        private readonly List<EnumMemberInternal<TInt, TIntProvider>> _duplicateValues;
        
        private EnumMemberParser[] _enumMemberParsers;

        private EnumMemberParser GetEnumMemberParser(EnumFormat format)
        {
            var index = format - EnumFormat.Name;
            var parsers = _enumMemberParsers;
            EnumMemberParser parser;
            if (index < 0 || parsers == null || index >= parsers.Length || (parser = parsers[index]) == null)
            {
                format.Validate(nameof(format));

                parser = new EnumMemberParser(format, this);
                EnumMemberParser[] oldParsers;
                do
                {
                    oldParsers = parsers;
                    parsers = new EnumMemberParser[Math.Max(oldParsers?.Length ?? 0, index + 1)];
                    oldParsers?.CopyTo(parsers, 0);
                    parsers[index] = parser;
                } while ((parsers = Interlocked.CompareExchange(ref _enumMemberParsers, parsers, oldParsers)) != oldParsers);
            }
            return parser;
        }
        #endregion

        public EnumCache(Type enumType, IEnumInfoInternal<TInt, TIntProvider> enumInfo)
        {
            _enumTypeName = enumType.Name;
            EnumInfo = enumInfo;
            _hasCustomValidator = enumInfo.HasCustomValidator;

            IsFlagEnum = enumType.IsDefined(typeof(FlagsAttribute), false);

            var fields =
#if TYPE_REFLECTION
                enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
#else
                enumType.GetTypeInfo().DeclaredFields.Where(fieldInfo => (fieldInfo.Attributes & (FieldAttributes.Static | FieldAttributes.Public)) == (FieldAttributes.Static | FieldAttributes.Public)).ToArray();
#endif
            _valueMap = new Dictionary<TInt, EnumMemberInternal<TInt, TIntProvider>>(fields.Length);
            if (fields.Length == 0)
            {
                return;
            }
            List<EnumMemberInternal<TInt, TIntProvider>> duplicateValues = null;

            // This is necessary due to a .NET reflection bug with retrieving Boolean Enums values
            Dictionary<string, TInt> fieldDictionary = null;
            var isBoolean = typeof(TInt) == typeof(bool);
            if (isBoolean)
            {
                fieldDictionary = new Dictionary<string, TInt>();
                var values = (TInt[])Enum.GetValues(enumType);
                var names = Enum.GetNames(enumType);
                for (var i = 0; i < names.Length; ++i)
                {
                    fieldDictionary.Add(names[i], values[i]);
                }
            }

            foreach (var field in fields)
            {
                var name = field.Name;
                var value = isBoolean ? fieldDictionary[name] : (TInt)field.GetValue(null);
                var attributes = new AttributeCollection(
#if TYPE_REFLECTION
                    Attribute.GetCustomAttributes(field, false));
#else
                    field.GetCustomAttributes(false).ToArray());
#endif
                var member = new EnumMemberInternal<TInt, TIntProvider>(value, name, attributes, this);
                if (_valueMap.TryGetValue(value, out var existing))
                {
                    if (attributes.Has<PrimaryEnumMemberAttribute>())
                    {
                        _valueMap[value] = member;
                        member = existing;
                    }
                    (duplicateValues ?? (duplicateValues = new List<EnumMemberInternal<TInt, TIntProvider>>())).Add(member);
                }
                else
                {
                    _valueMap.Add(value, member);
                    // Is Power of Two
                    if (Provider.And(value, Provider.Subtract(value, Provider.One)).Equals(Provider.Zero))
                    {
                        AllFlags = Provider.Or(AllFlags, value);
                    }
                }
            }
            
            var isInOrder = true;
            var previous = default(TInt);
            var isFirst = true;
            foreach (var pair in _valueMap)
            {
                var key = pair.Key;
                if (isFirst)
                {
                    _minDefined = key;
                    isFirst = false;
                }
                else if (previous.CompareTo(key) > 0)
                {
                    isInOrder = false;
                    break;
                }
                previous = key;
            }
            if (isInOrder)
            {
                _maxDefined = previous;
            }
            else
            {
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
            }
            
            _isContiguous = Provider.Subtract(_maxDefined, Provider.Create(_valueMap.Count - 1)).Equals(_minDefined);

            if (duplicateValues != null)
            {
                duplicateValues.TrimExcess();
                // Makes sure is in increasing order
                duplicateValues.Sort((first, second) => first.Value.CompareTo(second.Value));
                _duplicateValues = duplicateValues;
                _duplicateValues.Capacity = _duplicateValues.Count;
            }
        }

        #region Standard Enum Operations
        #region Type Methods
        public int GetMemberCount(EnumMemberSelection selection)
        {
            switch (selection)
            {
                case EnumMemberSelection.All:
#if DISPLAY_ATTRIBUTE
                case EnumMemberSelection.DisplayOrder:
#endif
                    return _valueMap.Count + (_duplicateValues?.Count ?? 0);
                default:
                    if (selection.HasAnyFlags(EnumMemberSelection.Flags))
                    {
                        return GetFlags(AllFlags).Count();
                    }
                    if (selection.HasAnyFlags(EnumMemberSelection.Distinct))
                    {
                        return _valueMap.Count;
                    }
                    selection.Validate(nameof(selection));
                    return 0;
            }
        }

        public IEnumerable<EnumMemberInternal<TInt, TIntProvider>> GetMembers(EnumMemberSelection selection)
        {
            IEnumerable<EnumMemberInternal<TInt, TIntProvider>> members;
            switch (selection)
            {
                case EnumMemberSelection.All:
#if DISPLAY_ATTRIBUTE
                case EnumMemberSelection.DisplayOrder:
#endif
                    members = _duplicateValues == null ? _valueMap.Values : GetMembersInternal();
                    break;
                default:
                    if (selection.HasAnyFlags(EnumMemberSelection.Flags))
                    {
                        members = GetFlagMembers(AllFlags);
                    }
                    else if (selection.HasAnyFlags(EnumMemberSelection.Distinct))
                    {
                        members = _valueMap.Values;
                    }
                    else
                    {
                        selection.Validate(nameof(selection));
                        return null;
                    }
                    break;
            }

#if DISPLAY_ATTRIBUTE
            return selection.HasAnyFlags(EnumMemberSelection.DisplayOrder)
                ? members.OrderBy(member => member.Attributes.Get<DisplayAttribute>()?.GetOrder() ?? int.MaxValue)
                : members;
#else
            return members;
#endif
        }

        public IEnumerable<string> GetNames(EnumMemberSelection selection) => GetMembers(selection).Select(member => member.Name);

        public IEnumerable<TInt> GetValues(EnumMemberSelection selection) => GetMembers(selection).Select(member => member.Value);

        private IEnumerable<EnumMemberInternal<TInt, TIntProvider>> GetMembersInternal()
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
                        if (duplicateIsActive && (!primaryIsActive || Provider.LessThan(duplicateMember.Value, primaryMember.Value)))
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
        public TInt ToObject(object value, EnumValidation validation)
        {
            Preconditions.NotNull(value, nameof(value));

            if (value is TInt || value is TInt?)
            {
                var result = (TInt)value;
                Validate(result, nameof(value), validation);
                return result;
            }

            var type = value.GetType();

            switch ((Nullable.GetUnderlyingType(type) ?? type).GetTypeCode())
            {
                case TypeCode.SByte:
                    return ToObject((sbyte)value, validation);
                case TypeCode.Byte:
                    return ToObject((byte)value, validation);
                case TypeCode.Int16:
                    return ToObject((short)value, validation);
                case TypeCode.UInt16:
                    return ToObject((ushort)value, validation);
                case TypeCode.Int32:
                    return ToObject((int)value, validation);
                case TypeCode.UInt32:
                    return ToObject((uint)value, validation);
                case TypeCode.Int64:
                    return ToObject((long)value, validation);
                case TypeCode.UInt64:
                    return ToObject((ulong)value, validation);
                case TypeCode.String:
                    var result = Parse((string)value, false, null);
                    Validate(result, nameof(value), validation);
                    return result;
                case TypeCode.Boolean:
                    return ToObject(Convert.ToByte((bool)value), validation);
                case TypeCode.Char:
                    return ToObject((char)value, validation);
            }
            throw new ArgumentException($"value is not type {_enumTypeName}, SByte, Int16, Int32, Int64, Byte, UInt16, UInt32, UInt64, or String.");
        }

        public TInt ToObject(long value, EnumValidation validation)
        {
            if (!Provider.IsInValueRange(value))
            {
                throw new OverflowException("value is outside the underlying type's value range");
            }

            var result = Provider.Create(value);
            Validate(result, nameof(value), validation);
            return result;
        }

        public TInt ToObject(ulong value, EnumValidation validation)
        {
            if (!Provider.IsInValueRange(value))
            {
                throw new OverflowException("value is outside the underlying type's value range");
            }

            var result = Provider.Create(value);
            Validate(result, nameof(value), validation);
            return result;
        }

        public bool TryToObject(object value, out TInt result, EnumValidation validation)
        {
            if (value != null)
            {
                if (value is TInt || value is TInt?)
                {
                    result = (TInt)value;
                    return IsValid(result, validation);
                }

                var type = value.GetType();

                switch ((Nullable.GetUnderlyingType(type) ?? type).GetTypeCode())
                {
                    case TypeCode.SByte:
                        return TryToObject((sbyte)value, out result, validation);
                    case TypeCode.Byte:
                        return TryToObject((byte)value, out result, validation);
                    case TypeCode.Int16:
                        return TryToObject((short)value, out result, validation);
                    case TypeCode.UInt16:
                        return TryToObject((ushort)value, out result, validation);
                    case TypeCode.Int32:
                        return TryToObject((int)value, out result, validation);
                    case TypeCode.UInt32:
                        return TryToObject((uint)value, out result, validation);
                    case TypeCode.Int64:
                        return TryToObject((long)value, out result, validation);
                    case TypeCode.UInt64:
                        return TryToObject((ulong)value, out result, validation);
                    case TypeCode.String:
                        if (TryParse((string)value, false, out result, null))
                        {
                            return IsValid(result, validation);
                        }
                        break;
                    case TypeCode.Boolean:
                        return TryToObject(Convert.ToByte((bool)value), out result, validation);
                    case TypeCode.Char:
                        return TryToObject((char)value, out result, validation);
                }
            }
            result = Provider.Zero;
            return false;
        }

        public bool TryToObject(long value, out TInt result, EnumValidation validation)
        {
            if (Provider.IsInValueRange(value))
            {
                result = Provider.Create(value);
                return IsValid(result, validation);
            }
            result = Provider.Zero;
            return false;
        }

        public bool TryToObject(ulong value, out TInt result, EnumValidation validation)
        {
            if (Provider.IsInValueRange(value))
            {
                result = Provider.Create(value);
                return IsValid(result, validation);
            }
            result = Provider.Zero;
            return false;
        }
        #endregion

        #region All Values Main Methods
        public bool IsValid(TInt value, EnumValidation validation)
        {
            switch (validation)
            {
                case EnumValidation.Default:
                    return _hasCustomValidator ? EnumInfo.CustomValidate(value) : IsValidSimple(value);
                case EnumValidation.IsDefined:
                    return IsDefined(value);
                case EnumValidation.IsValidFlagCombination:
                    return IsValidFlagCombination(value);
                case EnumValidation.None:
                    return true;
                default:
                    validation.Validate(nameof(validation));
                    return false;
            }
        }

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool IsValidSimple(TInt value) => (IsFlagEnum && IsValidFlagCombination(value)) || IsDefined(value);

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool IsDefined(TInt value) => _isContiguous ? !(Provider.LessThan(value, _minDefined) || Provider.LessThan(_maxDefined, value)) : _valueMap.ContainsKey(value);

        public void Validate(TInt value, string paramName, EnumValidation validation)
        {
            if (!IsValid(value, validation))
            {
                throw new ArgumentException($"invalid value of {AsString(value)} for {_enumTypeName}", paramName);
            }
        }

        public string AsString(TInt value) => AsStringInternal(value, null);

        internal string AsStringInternal(TInt value, EnumMemberInternal<TInt, TIntProvider> member) => IsFlagEnum ? FormatFlagsInternal(value, member, null, null) : FormatInternal(value, member, EnumFormat.Name, EnumFormat.UnderlyingValue);

        public string AsString(TInt value, EnumFormat format)
        {
            var isInitialized = false;
            EnumMemberInternal<TInt, TIntProvider> member = null;
            return FormatInternal(value, ref isInitialized, ref member, format);
        }

        public string AsString(TInt value, EnumFormat format0, EnumFormat format1) => FormatInternal(value, null, format0, format1);

        public string AsString(TInt value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => FormatInternal(value, null, format0, format1, format2);

        public string AsString(TInt value, EnumFormat[] formats) => AsStringInternal(value, null, formats);

        internal string AsStringInternal(TInt value, EnumMemberInternal<TInt, TIntProvider> member, EnumFormat[] formats) => formats?.Length > 0 ? FormatInternal(value, member, formats) : AsStringInternal(value, member);

        public string AsString(TInt value, string format) => AsStringInternal(value, null, format);

        internal string AsStringInternal(TInt value, EnumMemberInternal<TInt, TIntProvider> member, string format) => string.IsNullOrEmpty(format) ? AsStringInternal(value, member) : FormatInternal(value, member, format);

        public string Format(TInt value, EnumFormat[] formats)
        {
            Preconditions.NotNull(formats, nameof(formats));

            return FormatInternal(value, null, formats);
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
                    return value.ToString();
                case "X":
                case "x":
                    return Provider.ToHexidecimalString(value);
            }
            throw new FormatException("format string can be only \"G\", \"g\", \"X\", \"x\", \"F\", \"f\", \"D\" or \"d\".");
        }

        internal string FormatInternal(TInt value, ref bool isInitialized, ref EnumMemberInternal<TInt, TIntProvider> member, EnumFormat format)
        {
            if (format == EnumFormat.UnderlyingValue)
            {
                return value.ToString();
            }
            if (format == EnumFormat.DecimalValue)
            {
                return Provider.ToDecimalString(value);
            }
            if (format == EnumFormat.HexadecimalValue)
            {
                return Provider.ToHexidecimalString(value);
            }
            if (!isInitialized)
            {
                member = GetMember(value);
                isInitialized = true;
            }
            switch (format)
            {
                case EnumFormat.Name:
                    return member?.Name;
                case EnumFormat.Description:
                    return member?.Attributes.Get<DescriptionAttribute>()?.Description;
#if ENUM_MEMBER_ATTRIBUTE
                case EnumFormat.EnumMemberValue:
                    return member?.Attributes.Get<EnumMemberAttribute>()?.Value;
#endif
#if DISPLAY_ATTRIBUTE
                case EnumFormat.DisplayName:
                    return member?.Attributes.Get<DisplayAttribute>()?.GetName();
#endif
                default:
                    format.Validate(nameof(format));
                    return member != null ? Enums.CustomEnumMemberFormat(member.EnumMember, format) : null;
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

        internal string FormatInternal(TInt value, EnumMemberInternal<TInt, TIntProvider> member, EnumFormat[] formats)
        {
            var isInitialized = member != null;
            foreach (var format in formats)
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
        public EnumMemberInternal<TInt, TIntProvider> GetMember(TInt value)
        {
            _valueMap.TryGetValue(value, out var member);
            return member;
        }

        public EnumMemberInternal<TInt, TIntProvider> GetMember(string value, bool ignoreCase, EnumFormat[] formats)
        {
            Preconditions.NotNull(value, nameof(value));

            value = value.Trim();

            if (!(formats?.Length > 0))
            {
                formats = Enums.NameFormatArray;
            }

            TryParseInternal(value, ignoreCase, out _, out var member, formats, false);
            return member;
        }
        #endregion

        #region Parsing
        public TInt Parse(string value, bool ignoreCase, EnumFormat[] formats)
        {
            if (IsFlagEnum)
            {
                return ParseFlags(value, ignoreCase, null, formats);
            }

            Preconditions.NotNull(value, nameof(value));

            value = value.Trim();
            
            if (!(formats?.Length > 0))
            {
                formats = Enums.DefaultFormats;
            }

            if (TryParseInternal(value, ignoreCase, out var result, out _, formats, true))
            {
                return result;
            }
            if (IsNumeric(value))
            {
                throw new OverflowException("value is outside the underlying type's value range");
            }
            throw new ArgumentException($"string was not recognized as being a member of {_enumTypeName}", nameof(value));
        }

        public bool TryParse(string value, bool ignoreCase, out TInt result, EnumFormat[] formats)
        {
            if (IsFlagEnum)
            {
                return TryParseFlags(value, ignoreCase, null, out result, formats);
            }

            if (value != null)
            {
                value = value.Trim();

                if (!(formats?.Length > 0))
                {
                    formats = Enums.DefaultFormats;
                }

                return TryParseInternal(value, ignoreCase, out result, out _, formats, true);
            }
            result = Provider.Zero;
            return false;
        }

        private bool TryParseInternal(string value, bool ignoreCase, out TInt result, out EnumMemberInternal<TInt, TIntProvider> member, EnumFormat[] formats, bool getValueOnly)
        {
            foreach (var format in formats)
            {
                if (format == EnumFormat.UnderlyingValue)
                {
                    if (Provider.TryParseNative(value, out result))
                    {
                        member = getValueOnly ? null : GetMember(result);
                        return true;
                    }
                }
                else if (format == EnumFormat.DecimalValue || format == EnumFormat.HexadecimalValue)
                {
                    if (Provider.TryParseNumber(value, format == EnumFormat.DecimalValue ? NumberStyles.AllowLeadingSign : NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out result))
                    {
                        member = getValueOnly ? null : GetMember(result);
                        return true;
                    }
                }
                else
                {
                    var parser = GetEnumMemberParser(format);
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
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool IsValidFlagCombination(TInt value) => Provider.And(AllFlags, value).Equals(value);

        public string FormatFlags(TInt value, string delimiter, EnumFormat[] formats) => FormatFlagsInternal(value, null, delimiter, formats);

        internal string FormatFlagsInternal(TInt value, EnumMemberInternal<TInt, TIntProvider> member, string delimiter, EnumFormat[] formats)
        {
            if (!(formats?.Length > 0))
            {
                formats = Enums.DefaultFormats;
            }

            if (member == null)
            {
                member = GetMember(value);
            }

            if (member != null || value.Equals(Provider.Zero) || !IsValidFlagCombination(value))
            {
                return FormatInternal(value, member, formats);
            }

            if (string.IsNullOrEmpty(delimiter))
            {
                delimiter = FlagEnums.DefaultDelimiter;
            }

            return string.Join(delimiter,
                GetFlags(value).Select(flag => FormatInternal(flag, null, formats))
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

        public IEnumerable<EnumMemberInternal<TInt, TIntProvider>> GetFlagMembers(TInt value) => GetFlags(value).Select(flag => GetMember(flag));

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool HasAnyFlags(TInt value) => !value.Equals(Provider.Zero);

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool HasAnyFlags(TInt value, TInt otherFlags) => !Provider.And(value, otherFlags).Equals(Provider.Zero);

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool HasAllFlags(TInt value) => HasAllFlags(value, AllFlags);

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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

        public TInt RemoveFlags(TInt value, TInt otherFlags) => Provider.And(value, Provider.Not(otherFlags));
        #endregion

        #region Parsing
        public TInt ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] formats)
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

            if (!(formats?.Length > 0))
            {
                formats = Enums.DefaultFormats;
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
                if (TryParseInternal(indValue, ignoreCase, out var valueAsTInt, out _, formats, true))
                {
                    result = Provider.Or(result, valueAsTInt);
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

        public bool TryParseFlags(string value, bool ignoreCase, string delimiter, out TInt result, EnumFormat[] formats)
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

            if (!(formats?.Length > 0))
            {
                formats = Enums.DefaultFormats;
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
                if (!TryParseInternal(indValue, ignoreCase, out var valueAsTInt, out _, formats, true))
                {
                    result = Provider.Zero;
                    return false;
                }
                resultAsInt = Provider.Or(resultAsInt, valueAsTInt);
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
                _formatValueMap = new Dictionary<string, EnumMemberInternal<TInt, TIntProvider>>(enumCache.GetMemberCount(EnumMemberSelection.All), StringComparer.Ordinal);
                foreach (var member in enumCache.GetMembers(EnumMemberSelection.All))
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