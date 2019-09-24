﻿#region License
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
#if DISPLAY_ATTRIBUTE
using System.ComponentModel.DataAnnotations;
#endif
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;
using EnumsNET.Numerics;
using EnumsNET.Utilities;
using UnsafeUtility = System.Runtime.CompilerServices.Unsafe;

#if SPAN
using ParseType = System.ReadOnlySpan<char>;
#else
using ParseType = System.String;
#endif

namespace EnumsNET
{
    internal abstract class EnumCache
    {
        protected static bool IsNumeric(ParseType value)
        {
            char firstChar;
            return value.Length > 0 && (char.IsDigit((firstChar = value[0])) || firstChar == '-' || firstChar == '+');
        }

        public readonly Type UnderlyingType;
        public readonly TypeCode TypeCode;
        public readonly bool IsFlagEnum;
        private protected bool _hasDuplicateValues;
        private IReadOnlyList<string>? _names;
        private IValuesContainer? _values;
        private IReadOnlyList<EnumMember>? _members;

        private protected EnumCache(Type enumType, Type underlyingType)
        {
            UnderlyingType = underlyingType;
            TypeCode = underlyingType.GetTypeCode();
            IsFlagEnum = enumType.IsDefined(typeof(FlagsAttribute), false);
        }

        public IReadOnlyList<EnumMember> GetMembers(EnumMemberSelection selection)
        {
            var cached = selection == EnumMemberSelection.All || (selection == EnumMemberSelection.Distinct && !_hasDuplicateValues);
            IReadOnlyList<EnumMember>? members;
            if (!cached || (members = _members) == null)
            {
                members = GetMembersInternal(selection, cached);
                if (cached)
                {
                    members = Interlocked.CompareExchange(ref _members, members, null) ?? members;
                }
            }
            return members;
        }

        public IReadOnlyList<string> GetNames(EnumMemberSelection selection)
        {
            var cached = selection == EnumMemberSelection.All || (selection == EnumMemberSelection.Distinct && !_hasDuplicateValues);
            IReadOnlyList<string>? names;
            if (!cached || (names = _names) == null)
            {
                names = GetNamesInternal(selection, cached);
                if (cached)
                {
                    names = Interlocked.CompareExchange(ref _names, names, null) ?? names;
                }
            }
            return names;
        }

        public IValuesContainer GetValues(EnumMemberSelection selection)
        {
            var cached = selection == EnumMemberSelection.All || (selection == EnumMemberSelection.Distinct && !_hasDuplicateValues);
            IValuesContainer? values;
            if (!cached || (values = _values) == null)
            {
                values = GetValuesInternal(selection, cached);
                if (cached)
                {
                    values = Interlocked.CompareExchange(ref _values, values, null) ?? values;
                }
            }
            return values;
        }

        public abstract string AsString(ref byte value);
        public abstract string AsString(ref byte value, string? format);
        public abstract string? AsString(ref byte value, EnumFormat format);
        public abstract string? AsString(ref byte value, ValueCollection<EnumFormat> formats);
        public abstract string AsString(object value);
        public abstract string AsString(object value, string? format);
        public abstract string? AsString(object value, EnumFormat format);
        public abstract string? AsString(object value, ValueCollection<EnumFormat> formats);
        public abstract void CombineFlags(ref byte value, ref byte otherFlags, ref byte result);
        public abstract void CombineFlags(ref byte flag0, ref byte flag1, ref byte flag2, ref byte result);
        public abstract void CombineFlags(ref byte flag0, ref byte flag1, ref byte flag2, ref byte flag3, ref byte result);
        public abstract void CombineFlags(ref byte flag0, ref byte flag1, ref byte flag2, ref byte flag3, ref byte flag4, ref byte result);
        public abstract object CombineFlags(IEnumerable<object?>? flags, bool isNullable);
        public abstract object CombineFlags(object value, object otherFlags);
        public abstract void CommonFlags(ref byte value, ref byte otherFlags, ref byte result);
        public abstract object CommonFlags(object value, object otherFlags);
        public abstract int CompareTo(ref byte value, ref byte other);
        public abstract int CompareTo(object value, object other);
        public abstract bool Equals(ref byte value, ref byte other);
        public new abstract bool Equals(object value, object other);
        public abstract string Format(ref byte value, string format);
        public abstract string Format(object value, string format);
        public abstract string? FormatFlags(ref byte value, string? delimiter, ValueCollection<EnumFormat> formats);
        public abstract string? FormatFlags(object value, string? delimiter, ValueCollection<EnumFormat> formats);
        public abstract void GetAllFlags(ref byte result);
        public abstract object GetAllFlags();
        public abstract int GetFlagCount();
        public abstract int GetFlagCount(ref byte value);
        public abstract int GetFlagCount(object value);
        public abstract int GetFlagCount(ref byte value, ref byte otherFlags);
        public abstract int GetFlagCount(object value, object otherFlags);
        public abstract IReadOnlyList<EnumMember> GetFlagMembers(ref byte value);
        public abstract IReadOnlyList<EnumMember> GetFlagMembers(object value);
        public abstract IValuesContainer GetFlags(ref byte value);
        public abstract IReadOnlyList<object> GetFlags(object value);
        public abstract int GetHashCode(ref byte value);
        public abstract EnumMemberInternal? GetMember(ref byte value);
        public abstract EnumMemberInternal? GetMember(object value);
        public abstract EnumMember? GetMember(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats);
        protected abstract IReadOnlyList<EnumMember> GetMembersInternal(EnumMemberSelection selection, bool cached);
        public abstract int GetMemberCount(EnumMemberSelection selection);
        protected abstract IReadOnlyList<string> GetNamesInternal(EnumMemberSelection selection, bool cached);
        public abstract object GetUnderlyingValue(ref byte value);
        public abstract object GetUnderlyingValue(object value);
        protected abstract IValuesContainer GetValuesInternal(EnumMemberSelection selection, bool cached);
        public abstract bool HasAllFlags(ref byte value);
        public abstract bool HasAllFlags(object value);
        public abstract bool HasAllFlags(ref byte value, ref byte otherFlags);
        public abstract bool HasAllFlags(object value, object otherFlags);
        public abstract bool HasAnyFlags(ref byte value);
        public abstract bool HasAnyFlags(object value);
        public abstract bool HasAnyFlags(ref byte value, ref byte otherFlags);
        public abstract bool HasAnyFlags(object value, object otherFlags);
        public abstract bool IsDefined(ref byte value);
        public abstract bool IsDefined(object value);
        public abstract bool IsValid(ref byte value, EnumValidation validation);
        public abstract bool IsValid(object value, EnumValidation validation);
        public abstract bool IsValidFlagCombination(ref byte value);
        public abstract bool IsValidFlagCombination(object value);
        public abstract void Parse(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats, ref byte result);
        public abstract object Parse(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats);
        public abstract void ParseFlags(ParseType value, bool ignoreCase, string? delimiter, ValueCollection<EnumFormat> formats, ref byte result);
        public abstract object ParseFlags(ParseType value, bool ignoreCase, string? delimiter, ValueCollection<EnumFormat> formats);
        public abstract void RemoveFlags(ref byte value, ref byte otherFlags, ref byte result);
        public abstract object RemoveFlags(object value, object otherFlags);
        public abstract byte ToByte(ref byte value);
        public abstract byte ToByte(object value);
        public abstract void ToggleFlags(ref byte value, ref byte result);
        public abstract object ToggleFlags(object value);
        public abstract void ToggleFlags(ref byte value, ref byte otherFlags, ref byte result);
        public abstract object ToggleFlags(object value, object otherFlags);
        public abstract short ToInt16(ref byte value);
        public abstract short ToInt16(object value);
        public abstract int ToInt32(ref byte value);
        public abstract int ToInt32(object value);
        public abstract long ToInt64(ref byte value);
        public abstract long ToInt64(object value);
        public abstract void ToObject(ulong value, EnumValidation validation, ref byte result);
        public abstract object ToObject(ulong value, EnumValidation validation);
        public abstract void ToObject(object value, EnumValidation validation, ref byte result);
        public abstract object ToObject(object value, EnumValidation validation);
        public abstract void ToObject(long value, EnumValidation validation, ref byte result);
        public abstract object ToObject(long value, EnumValidation validation);
        public abstract sbyte ToSByte(ref byte value);
        public abstract sbyte ToSByte(object value);
        public abstract ushort ToUInt16(ref byte value);
        public abstract ushort ToUInt16(object value);
        public abstract uint ToUInt32(ref byte value);
        public abstract uint ToUInt32(object value);
        public abstract ulong ToUInt64(ref byte value);
        public abstract ulong ToUInt64(object value);
        public abstract bool TryParse(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, ref byte result, ValueCollection<EnumFormat> formats);
        public abstract bool TryParse(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, out object? result, ValueCollection<EnumFormat> formats);
        public abstract bool TryParseFlags(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, string? delimiter, ref byte result, ValueCollection<EnumFormat> formats);
        public abstract bool TryParseFlags(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, string? delimiter, out object? result, ValueCollection<EnumFormat> formats);
        public abstract bool TryToObject(ulong value, ref byte result, EnumValidation validation);
        public abstract bool TryToObject(ulong value, out object? result, EnumValidation validation);
        public abstract bool TryToObject(object? value, ref byte result, EnumValidation validation);
        public abstract bool TryToObject(object? value, out object? result, EnumValidation validation);
        public abstract bool TryToObject(long value, ref byte result, EnumValidation validation);
        public abstract bool TryToObject(long value, out object? result, EnumValidation validation);
        public abstract void Validate(ref byte value, string paramName, EnumValidation validation);
        public abstract object Validate(object value, string paramName, EnumValidation validation);
    }

    internal sealed class EnumCache<TUnderlying, TUnderlyingOperations> : EnumCache
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        internal readonly IEnumBridge<TUnderlying, TUnderlyingOperations> EnumBridge;
        private readonly bool _isContiguous;
        private readonly bool _hasCustomValidator;
        private readonly string _enumTypeName;
        private readonly TUnderlying _allFlags;
        private readonly TUnderlying _maxDefined;
        private readonly TUnderlying _minDefined;
        private readonly DictionarySlim<TUnderlying, EnumMemberInternal<TUnderlying, TUnderlyingOperations>> _valueMap;
        private readonly List<EnumMemberInternal<TUnderlying, TUnderlyingOperations>>? _duplicateValues;
        private EnumMemberParser[]? _enumMemberParsers;

        private EnumMemberParser GetEnumMemberParser(EnumFormat format)
        {
            var index = format - EnumFormat.Name;
            var parsers = _enumMemberParsers;
            EnumMemberParser parser;
            if (index < 0 || parsers == null || index >= parsers.Length || (parser = parsers[index]) == null)
            {
                format.Validate(nameof(format));

                parser = new EnumMemberParser(format, this);
                EnumMemberParser[]? oldParsers;
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

        public EnumCache(Type enumType, IEnumBridge<TUnderlying, TUnderlyingOperations> enumBridge)
            : base(enumType, typeof(TUnderlying))
        {
            _enumTypeName = enumType.Name;
            EnumBridge = enumBridge;
            _hasCustomValidator = enumBridge.HasCustomValidator;

            var fields =
#if TYPE_REFLECTION
                enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
#else
                enumType.GetTypeInfo().DeclaredFields.Where(fieldInfo => (fieldInfo.Attributes & (FieldAttributes.Static | FieldAttributes.Public)) == (FieldAttributes.Static | FieldAttributes.Public)).ToArray();
#endif
            var valueMap = new Dictionary<TUnderlying, EnumMemberInternal<TUnderlying, TUnderlyingOperations>>(fields.Length);
            if (fields.Length == 0)
            {
                _valueMap = new DictionarySlim<TUnderlying, EnumMemberInternal<TUnderlying, TUnderlyingOperations>>(valueMap, 0);
                return;
            }
            List<EnumMemberInternal<TUnderlying, TUnderlyingOperations>>? duplicateValues = null;

            // This is necessary due to a .NET reflection bug with retrieving Boolean Enums values
            Dictionary<string, TUnderlying>? fieldDictionary = null;
            if (typeof(TUnderlying) == typeof(bool))
            {
                fieldDictionary = new Dictionary<string, TUnderlying>();
                var values = (TUnderlying[])Enum.GetValues(enumType);
                var names = Enum.GetNames(enumType);
                for (var i = 0; i < names.Length; ++i)
                {
                    fieldDictionary.Add(names[i], values[i]);
                }
            }

            TUnderlyingOperations operations = default;
            foreach (var field in fields)
            {
                var name = field.Name;
                var value = fieldDictionary != null ? fieldDictionary[name] : (TUnderlying)field.GetValue(null)!;
                var attributes = new AttributeCollection(
#if TYPE_REFLECTION
                    Attribute.GetCustomAttributes(field, false));
#else
                    field.GetCustomAttributes(false).ToArray());
#endif
                var member = new EnumMemberInternal<TUnderlying, TUnderlyingOperations>(value, name, attributes, this);
                if (valueMap.TryGetValue(value, out var existing))
                {
                    if (attributes.Has<PrimaryEnumMemberAttribute>())
                    {
                        valueMap[value] = member;
                        member = existing;
                    }
                    (duplicateValues ??= new List<EnumMemberInternal<TUnderlying, TUnderlyingOperations>>()).Add(member);
                }
                else
                {
                    valueMap.Add(value, member);
                    // Is Power of Two
                    if (operations.BitCount(value) == 1)
                    {
                        _allFlags = operations.Or(_allFlags, value);
                    }
                }
            }
            
            // Makes sure is in increasing value order, due to no removals
            _valueMap = new DictionarySlim<TUnderlying, EnumMemberInternal<TUnderlying, TUnderlyingOperations>>(valueMap.OrderBy(p => p.Key), valueMap.Count);

            _maxDefined = _valueMap._entries[_valueMap.Count - 1].Key;
            _minDefined = _valueMap._entries[0].Key;
            
            _isContiguous = operations.Subtract(_maxDefined, operations.Create(_valueMap.Count - 1)).Equals(_minDefined);

            if (duplicateValues != null)
            {
                duplicateValues.TrimExcess();
                // Makes sure is in increasing order
                duplicateValues.Sort((first, second) => first.Value.CompareTo(second.Value));
                _duplicateValues = duplicateValues;
                _duplicateValues.Capacity = _duplicateValues.Count;
                _hasDuplicateValues = true;
            }
        }

        #region Standard Enum Operations
        #region Type Methods
        public override int GetMemberCount(EnumMemberSelection selection)
        {
            switch (selection)
            {
                case EnumMemberSelection.All:
#if DISPLAY_ATTRIBUTE
                case EnumMemberSelection.DisplayOrder:
#endif
                    return _valueMap.Count + (_duplicateValues?.Count ?? 0);
                default:
                    selection.Validate(nameof(selection));
                    if (selection.HasAnyFlags(EnumMemberSelection.Flags))
                    {
                        return GetFlagCount();
                    }
                    if (selection.HasAnyFlags(EnumMemberSelection.Distinct))
                    {
                        return _valueMap.Count;
                    }
                    return 0;
            }
        }

        protected override IReadOnlyList<EnumMember> GetMembersInternal(EnumMemberSelection selection, bool cached) => EnumBridge.CreateMembersContainer(GetInternalMembers(selection), GetMemberCount(selection), cached);

        protected override IReadOnlyList<string> GetNamesInternal(EnumMemberSelection selection, bool cached) => new NamesContainer(GetInternalMembers(selection).Select(m => m.Name), GetMemberCount(selection), cached);

        protected override IValuesContainer GetValuesInternal(EnumMemberSelection selection, bool cached) => EnumBridge.CreateValuesContainer(GetInternalMembers(selection).Select(m => m.Value), GetMemberCount(selection), cached);

        private IEnumerable<EnumMemberInternal<TUnderlying, TUnderlyingOperations>> GetInternalMembers(EnumMemberSelection selection)
        {
            IEnumerable<EnumMemberInternal<TUnderlying, TUnderlyingOperations>> members;
            switch (selection)
            {
                case EnumMemberSelection.All:
#if DISPLAY_ATTRIBUTE
                case EnumMemberSelection.DisplayOrder:
#endif
                    members = _duplicateValues == null ? _valueMap.Values : GetMembersInternal();
                    break;
                default:
                    selection.Validate(nameof(selection));
                    if (selection.HasAnyFlags(EnumMemberSelection.Flags))
                    {
                        members = EnumerateFlags(_allFlags).Select(flag => GetMember(flag)!);
                    }
                    else if (selection.HasAnyFlags(EnumMemberSelection.Distinct))
                    {
                        members = _valueMap.Values;
                    }
                    else
                    {
                        return null!; // should never get here
                    }
                    break;
            }

#if DISPLAY_ATTRIBUTE
            return selection.HasAnyFlags(EnumMemberSelection.DisplayOrder)
                ? members.OrderBy(m => m.Attributes.Get<DisplayAttribute>()?.GetOrder() ?? int.MaxValue)
                : members;
#else
            return members;
#endif
        }

        private IEnumerable<EnumMemberInternal<TUnderlying, TUnderlyingOperations>> GetMembersInternal()
        {
            var primaryEnumerator = _valueMap.GetEnumerator();
            var primaryIsActive = primaryEnumerator.MoveNext();
            var primaryMember = primaryEnumerator.Current;
            var duplicateEnumerator = _duplicateValues!.GetEnumerator();
            TUnderlyingOperations operations = default;
            var duplicateIsActive = duplicateEnumerator.MoveNext();
            var duplicateMember = duplicateEnumerator.Current;
            while (primaryIsActive || duplicateIsActive)
            {
                if (duplicateIsActive && (!primaryIsActive || operations.LessThan(duplicateMember.Value, primaryMember.Key)))
                {
                    yield return duplicateMember;
                    if (duplicateIsActive = duplicateEnumerator.MoveNext())
                    {
                        duplicateMember = duplicateEnumerator.Current;
                    }
                }
                else
                {
                    yield return primaryMember.Value;
                    if (primaryIsActive = primaryEnumerator.MoveNext())
                    {
                        primaryMember = primaryEnumerator.Current;
                    }
                }
            }
        }
        #endregion

        #region ToObject
        public override void ToObject(ulong value, EnumValidation validation, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ToObjectInternal(value, validation);
        }

        public override object ToObject(ulong value, EnumValidation validation) => EnumBridge.ToObjectUnchecked(ToObjectInternal(value, validation));

        public override void ToObject(object value, EnumValidation validation, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ToObjectInternal(value, validation);
        }

        public override object ToObject(object value, EnumValidation validation) => EnumBridge.ToObjectUnchecked(ToObjectInternal(value, validation));

        public override void ToObject(long value, EnumValidation validation, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ToObjectInternal(value, validation);
        }

        public override object ToObject(long value, EnumValidation validation) => EnumBridge.ToObjectUnchecked(ToObjectInternal(value, validation));

        public TUnderlying ToObject(object value)
        {
            Preconditions.NotNull(value, nameof(value));

            if (EnumBridge.IsEnum(value))
            {
                return UnsafeUtility.As<byte, TUnderlying>(ref value.GetRawData());
            }

            if (value is TUnderlying || value is TUnderlying?)
            {
                return (TUnderlying)value;
            }

            var type = value.GetType();

            return ((Nullable.GetUnderlyingType(type) ?? type).GetTypeCode()) switch
            {
                TypeCode.SByte => ToObject((sbyte)value),
                TypeCode.Byte => ToObject((byte)value),
                TypeCode.Int16 => ToObject((short)value),
                TypeCode.UInt16 => ToObject((ushort)value),
                TypeCode.Int32 => ToObject((int)value),
                TypeCode.UInt32 => ToObject((uint)value),
                TypeCode.Int64 => ToObject((long)value),
                TypeCode.UInt64 => ToObject((ulong)value),
                TypeCode.String => ParseInternal((string)value, false, Enums.DefaultFormats),
                TypeCode.Boolean => ToObject(Convert.ToByte((bool)value)),
                TypeCode.Char => ToObject((char)value),
                _ => throw new ArgumentException($"value is not type {_enumTypeName}, SByte, Int16, Int32, Int64, Byte, UInt16, UInt32, UInt64, or String."),
            };
        }

        public TUnderlying ToObjectInternal(object value, EnumValidation validation)
        {
            Preconditions.NotNull(value, nameof(value));

            if (EnumBridge.IsEnum(value))
            {
                var result = UnsafeUtility.As<byte, TUnderlying>(ref value.GetRawData());
                Validate(result, nameof(value), validation);
                return result;
            }

            if (value is TUnderlying || value is TUnderlying?)
            {
                var result = (TUnderlying)value;
                Validate(result, nameof(value), validation);
                return result;
            }

            var type = value.GetType();

            switch ((Nullable.GetUnderlyingType(type) ?? type).GetTypeCode())
            {
                case TypeCode.SByte:
                    return ToObjectInternal((sbyte)value, validation);
                case TypeCode.Byte:
                    return ToObjectInternal((byte)value, validation);
                case TypeCode.Int16:
                    return ToObjectInternal((short)value, validation);
                case TypeCode.UInt16:
                    return ToObjectInternal((ushort)value, validation);
                case TypeCode.Int32:
                    return ToObjectInternal((int)value, validation);
                case TypeCode.UInt32:
                    return ToObjectInternal((uint)value, validation);
                case TypeCode.Int64:
                    return ToObjectInternal((long)value, validation);
                case TypeCode.UInt64:
                    return ToObjectInternal((ulong)value, validation);
                case TypeCode.String:
                    var result = ParseInternal((string)value, false, Enums.DefaultFormats);
                    Validate(result, nameof(value), validation);
                    return result;
                case TypeCode.Boolean:
                    return ToObjectInternal(Convert.ToByte((bool)value), validation);
                case TypeCode.Char:
                    return ToObjectInternal((char)value, validation);
            }
            throw new ArgumentException($"value is not type {_enumTypeName}, SByte, Int16, Int32, Int64, Byte, UInt16, UInt32, UInt64, or String.");
        }

        public TUnderlying ToObject(long value)
        {
            TUnderlyingOperations operations = default;
            if (!operations.IsInValueRange(value))
            {
                throw new OverflowException("value is outside the underlying type's value range");
            }

            return operations.Create(value);
        }

        public TUnderlying ToObjectInternal(long value, EnumValidation validation)
        {
            TUnderlyingOperations operations = default;
            if (!operations.IsInValueRange(value))
            {
                throw new OverflowException("value is outside the underlying type's value range");
            }

            var result = operations.Create(value);
            Validate(result, nameof(value), validation);
            return result;
        }

        public TUnderlying ToObject(ulong value)
        {
            TUnderlyingOperations operations = default;
            if (!operations.IsInValueRange(value))
            {
                throw new OverflowException("value is outside the underlying type's value range");
            }

            return operations.Create((long)value);
        }

        public TUnderlying ToObjectInternal(ulong value, EnumValidation validation)
        {
            TUnderlyingOperations operations = default;
            if (!operations.IsInValueRange(value))
            {
                throw new OverflowException("value is outside the underlying type's value range");
            }

            var result = operations.Create((long)value);
            Validate(result, nameof(value), validation);
            return result;
        }

        public override bool TryToObject(object? value, ref byte result, EnumValidation validation)
        {
            if (TryToObject(value, out var r, validation))
            {
                ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
                underlying = r;
                return true;
            }
            return false;
        }

        public override bool TryToObject(object? value, out object? result, EnumValidation validation)
        {
            if (TryToObject(value, out var r, validation))
            {
                result = EnumBridge.ToObjectUnchecked(r);
                return true;
            }
            result = default;
            return false;
        }

        public override bool TryToObject(long value, ref byte result, EnumValidation validation)
        {
            if (TryToObject(value, out var r, validation))
            {
                ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
                underlying = r;
                return true;
            }
            return false;
        }

        public override bool TryToObject(long value, out object? result, EnumValidation validation)
        {
            if (TryToObject(value, out var r, validation))
            {
                result = EnumBridge.ToObjectUnchecked(r);
                return true;
            }
            result = default;
            return false;
        }

        public override bool TryToObject(ulong value, ref byte result, EnumValidation validation)
        {
            if (TryToObject(value, out var r, validation))
            {
                ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
                underlying = r;
                return true;
            }
            return false;
        }

        public override bool TryToObject(ulong value, out object? result, EnumValidation validation)
        {
            if (TryToObject(value, out var r, validation))
            {
                result = EnumBridge.ToObjectUnchecked(r);
                return true;
            }
            result = default;
            return false;
        }

        public bool TryToObject(object? value, out TUnderlying result, EnumValidation validation)
        {
            if (value != null)
            {
                if (EnumBridge.IsEnum(value))
                {
                    result = UnsafeUtility.As<byte, TUnderlying>(ref value.GetRawData());
                    return IsValid(result, validation);
                }

                if (value is TUnderlying || value is TUnderlying?)
                {
                    result = (TUnderlying)value;
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
                        if (TryParse((string)value, false, out result, Enums.DefaultFormats))
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
            result = default;
            return false;
        }

        public bool TryToObject(long value, out TUnderlying result, EnumValidation validation)
        {
            TUnderlyingOperations operations = default;
            if (operations.IsInValueRange(value))
            {
                result = operations.Create(value);
                return IsValid(result, validation);
            }
            result = default;
            return false;
        }

        public bool TryToObject(ulong value, out TUnderlying result, EnumValidation validation)
        {
            TUnderlyingOperations operations = default;
            if (operations.IsInValueRange(value))
            {
                result = operations.Create((long)value);
                return IsValid(result, validation);
            }
            result = default;
            return false;
        }
        #endregion

        #region All Values Main Methods
        public override bool IsValid(ref byte value, EnumValidation validation) => IsValid(UnsafeUtility.As<byte, TUnderlying>(ref value), validation);

        public override bool IsValid(object value, EnumValidation validation) => IsValid(ToObject(value), validation);

        public bool IsValid(TUnderlying value, EnumValidation validation)
        {
            switch (validation)
            {
                case EnumValidation.Default:
                    return _hasCustomValidator ? EnumBridge.CustomValidate(value) : (IsFlagEnum && IsValidFlagCombination(value)) || IsDefined(value);
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

        public override bool IsDefined(ref byte value) => IsDefined(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override bool IsDefined(object value) => IsDefined(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsDefined(TUnderlying value)
        {
            TUnderlyingOperations operations = default;
            return _isContiguous ? !(operations.LessThan(value, _minDefined) || operations.LessThan(_maxDefined, value)) : _valueMap.TryGetValue(value, out _);
        }

        public override void Validate(ref byte value, string paramName, EnumValidation validation) => Validate(UnsafeUtility.As<byte, TUnderlying>(ref value), paramName, validation);

        public override object Validate(object value, string paramName, EnumValidation validation)
        {
            var underlying = ToObject(value);
            Validate(underlying, paramName, validation);
            return EnumBridge.ToObjectUnchecked(underlying);
        }

        public void Validate(TUnderlying value, string paramName, EnumValidation validation)
        {
            if (!IsValid(value, validation))
            {
                throw new ArgumentException($"invalid value of {AsStringInternal(value, null)} for {_enumTypeName}", paramName);
            }
        }

        public override string AsString(ref byte value) => AsStringInternal(UnsafeUtility.As<byte, TUnderlying>(ref value), null);

        public override string AsString(ref byte value, string? format) => AsStringInternal(UnsafeUtility.As<byte, TUnderlying>(ref value), null, format);

        public override string? AsString(ref byte value, EnumFormat format) => AsString(UnsafeUtility.As<byte, TUnderlying>(ref value), format);

        public override string? AsString(ref byte value, ValueCollection<EnumFormat> formats) => AsStringInternal(UnsafeUtility.As<byte, TUnderlying>(ref value), null, formats);

        public override string AsString(object value) => AsStringInternal(ToObject(value), null);

        public override string AsString(object value, string? format) => AsStringInternal(ToObject(value), null, format);

        public override string? AsString(object value, EnumFormat format) => AsString(ToObject(value), format);

        public override string? AsString(object value, ValueCollection<EnumFormat> formats) => AsStringInternal(ToObject(value), null, formats);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal string AsStringInternal(TUnderlying value, EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member) => IsFlagEnum ? FormatFlagsInternal(value, member, null, Enums.DefaultFormats)! : FormatInternal(value, member, Enums.DefaultFormats)!;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string? AsString(TUnderlying value, EnumFormat format)
        {
            var isInitialized = false;
            EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member = null;
            return FormatInternal(value, ref isInitialized, ref member, format);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal string? AsStringInternal(TUnderlying value, EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member, ValueCollection<EnumFormat> formats) => formats.Any() ? FormatInternal(value, member, formats) : AsStringInternal(value, member);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal string AsStringInternal(TUnderlying value, EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member, string? format) => string.IsNullOrEmpty(format) ? AsStringInternal(value, member) : FormatInternal(value, member, format!);

        public override string Format(ref byte value, string format) => FormatInternal(UnsafeUtility.As<byte, TUnderlying>(ref value), null, format);

        public override string Format(object value, string format) => FormatInternal(ToObject(value), null, format);

        internal string FormatInternal(TUnderlying value, EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member, string format)
        {
            Preconditions.NotNull(format, nameof(format));

            if (format.Length == 1)
            {
                switch (format[0])
                {
                    case 'G':
                    case 'g':
                        return AsStringInternal(value, member);
                    case 'F':
                    case 'f':
                        return FormatFlagsInternal(value, member, null, Enums.DefaultFormats)!;
                    case 'D':
                    case 'd':
                        return value.ToString()!;
                    case 'X':
                    case 'x':
                        return default(TUnderlyingOperations).ToHexadecimalString(value);
                }
            }
            throw new FormatException("format string can be only \"G\", \"g\", \"X\", \"x\", \"F\", \"f\", \"D\" or \"d\".");
        }

        internal string? FormatInternal(TUnderlying value, ref bool isInitialized, ref EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member, EnumFormat format)
        {
            if (format == EnumFormat.UnderlyingValue)
            {
                return value.ToString();
            }
            TUnderlyingOperations operations = default;
            if (format == EnumFormat.DecimalValue)
            {
                return operations.ToDecimalString(value);
            }
            if (format == EnumFormat.HexadecimalValue)
            {
                return operations.ToHexadecimalString(value);
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
                case EnumFormat.EnumMemberValue:
                    return member?.Attributes.Get<EnumMemberAttribute>()?.Value;
#if DISPLAY_ATTRIBUTE
                case EnumFormat.DisplayName:
                    return member?.Attributes.Get<DisplayAttribute>()?.GetName();
#endif
                default:
                    format.Validate(nameof(format));
                    return member != null ? Enums.CustomEnumMemberFormat(member.EnumMember, format) : null;
            }
        }

        internal string? FormatInternal(TUnderlying value, EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member, ValueCollection<EnumFormat> formats)
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

        public override object GetUnderlyingValue(object value) => ToObject(value);

        public override object GetUnderlyingValue(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value);

        public override int GetHashCode(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).GetHashCode();

        public override bool Equals(ref byte value, ref byte other) => UnsafeUtility.As<byte, TUnderlying>(ref value).Equals(UnsafeUtility.As<byte, TUnderlying>(ref other));

        public override bool Equals(object value, object other) => ToObject(value).Equals(ToObject(other));

        public override int CompareTo(ref byte value, ref byte other) => UnsafeUtility.As<byte, TUnderlying>(ref value).CompareTo(UnsafeUtility.As<byte, TUnderlying>(ref other));

        public override int CompareTo(object value, object other) => ToObject(value).CompareTo(ToObject(other));

#if ICONVERTIBLE
        public override sbyte ToSByte(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToSByte(null);

        public override sbyte ToSByte(object value) => ToObject(value).ToSByte(null);

        public override byte ToByte(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToByte(null);

        public override byte ToByte(object value) => ToObject(value).ToByte(null);

        public override short ToInt16(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToInt16(null);

        public override short ToInt16(object value) => ToObject(value).ToInt16(null);

        public override ushort ToUInt16(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToUInt16(null);

        public override ushort ToUInt16(object value) => ToObject(value).ToUInt16(null);

        public override int ToInt32(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToInt32(null);

        public override int ToInt32(object value) => ToObject(value).ToInt32(null);

        public override uint ToUInt32(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToUInt32(null);

        public override uint ToUInt32(object value) => ToObject(value).ToUInt32(null);

        public override long ToInt64(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToInt64(null);

        public override long ToInt64(object value) => ToObject(value).ToInt64(null);

        public override ulong ToUInt64(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToUInt64(null);

        public override ulong ToUInt64(object value) => ToObject(value).ToUInt64(null);
#else
        public override sbyte ToSByte(ref byte value) => default(TUnderlyingOperations).ToSByte(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override sbyte ToSByte(object value) => default(TUnderlyingOperations).ToSByte(ToObject(value));

        public override byte ToByte(ref byte value) => default(TUnderlyingOperations).ToByte(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override byte ToByte(object value) => default(TUnderlyingOperations).ToByte(ToObject(value));

        public override short ToInt16(ref byte value) => default(TUnderlyingOperations).ToInt16(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override short ToInt16(object value) => default(TUnderlyingOperations).ToInt16(ToObject(value));

        public override ushort ToUInt16(ref byte value) => default(TUnderlyingOperations).ToUInt16(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override ushort ToUInt16(object value) => default(TUnderlyingOperations).ToUInt16(ToObject(value));

        public override int ToInt32(ref byte value) => default(TUnderlyingOperations).ToInt32(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override int ToInt32(object value) => default(TUnderlyingOperations).ToInt32(ToObject(value));

        public override uint ToUInt32(ref byte value) => default(TUnderlyingOperations).ToUInt32(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override uint ToUInt32(object value) => default(TUnderlyingOperations).ToUInt32(ToObject(value));

        public override long ToInt64(ref byte value) => default(TUnderlyingOperations).ToInt64(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override long ToInt64(object value) => default(TUnderlyingOperations).ToInt64(ToObject(value));

        public override ulong ToUInt64(ref byte value) => default(TUnderlyingOperations).ToUInt64(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override ulong ToUInt64(object value) => default(TUnderlyingOperations).ToUInt64(ToObject(value));
#endif
        #endregion

        #region Defined Values Main Methods
        public override EnumMemberInternal? GetMember(ref byte value) => GetMember(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override EnumMemberInternal? GetMember(object value) => GetMember(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EnumMemberInternal<TUnderlying, TUnderlyingOperations>? GetMember(TUnderlying value)
        {
            _valueMap.TryGetValue(value, out var member);
            return member;
        }

        public override EnumMember? GetMember(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats)
        {
            value = value.Trim();

            if (!formats.Any())
            {
                formats = Enums.NameFormat;
            }

            TryParseInternal(value, ignoreCase, out _, out var member, formats, false);
            return member?.EnumMember;
        }
        #endregion

        #region Parsing
        public override void Parse(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ParseInternal(value, ignoreCase, formats);
        }

        public override object Parse(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats) => EnumBridge.ToObjectUnchecked(ParseInternal(value, ignoreCase, formats));

        public TUnderlying ParseInternal(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats)
        {
            if (IsFlagEnum)
            {
                return ParseFlagsInternal(value, ignoreCase, null, formats);
            }

            value = value.Trim();
            
            if (!formats.Any())
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

        public override bool TryParse(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, ref byte result, ValueCollection<EnumFormat> formats)
        {
            if (TryParse(value, ignoreCase, out var r, formats))
            {
                ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
                underlying = r;
                return true;
            }
            return false;
        }

        public override bool TryParse(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, out object? result, ValueCollection<EnumFormat> formats)
        {
            if (TryParse(value, ignoreCase, out var r, formats))
            {
                result = EnumBridge.ToObjectUnchecked(r);
                return true;
            }
            result = default;
            return false;
        }

        public bool TryParse(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, out TUnderlying result, ValueCollection<EnumFormat> formats)
        {
            if (IsFlagEnum)
            {
                return TryParseFlags(value, ignoreCase, null, out result, formats);
            }

            if (value != null)
            {
                value = value.Trim();

                if (!formats.Any())
                {
                    formats = Enums.DefaultFormats;
                }

                return TryParseInternal(value, ignoreCase, out result, out _, formats, true);
            }
            result = default;
            return false;
        }

        private bool TryParseInternal(ParseType value, bool ignoreCase, out TUnderlying result, out EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member, ValueCollection<EnumFormat> formats, bool getValueOnly)
        {
            TUnderlyingOperations operations = default;
            foreach (var format in formats)
            {
                if (format == EnumFormat.UnderlyingValue)
                {
                    if (operations.TryParseNative(value, out result))
                    {
                        member = getValueOnly ? null : GetMember(result);
                        return true;
                    }
                }
                else if (format == EnumFormat.DecimalValue || format == EnumFormat.HexadecimalValue)
                {
                    if (operations.TryParseNumber(value, format == EnumFormat.DecimalValue ? NumberStyles.AllowLeadingSign : NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out result))
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
            result = default;
            member = null;
            return false;
        }
        #endregion
        #endregion

        #region Flag Enum Operations
        public override void GetAllFlags(ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = _allFlags;
        }

        public override object GetAllFlags() => EnumBridge.ToObjectUnchecked(_allFlags);

        #region Main Methods
        public override bool IsValidFlagCombination(ref byte value) => IsValidFlagCombination(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override bool IsValidFlagCombination(object value) => IsValidFlagCombination(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidFlagCombination(TUnderlying value) => default(TUnderlyingOperations).And(_allFlags, value).Equals(value);

        public override string? FormatFlags(ref byte value, string? delimiter, ValueCollection<EnumFormat> formats) => FormatFlagsInternal(UnsafeUtility.As<byte, TUnderlying>(ref value), null, delimiter, formats);

        public override string? FormatFlags(object value, string? delimiter, ValueCollection<EnumFormat> formats) => FormatFlagsInternal(ToObject(value), null, delimiter, formats);

        internal string? FormatFlagsInternal(TUnderlying value, EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member, string? delimiter, ValueCollection<EnumFormat> formats)
        {
            if (!formats.Any())
            {
                formats = Enums.DefaultFormats;
            }

            if (member == null)
            {
                member = GetMember(value);
            }

            if (member != null || value.Equals(default) || !IsValidFlagCombination(value))
            {
                return FormatInternal(value, member, formats);
            }

            if (string.IsNullOrEmpty(delimiter))
            {
                delimiter = FlagEnums.DefaultDelimiter;
            }

            return string.Join(delimiter, EnumerateFlags(value).Select(flag => FormatInternal(flag, null, formats)));
        }

        public override IReadOnlyList<object> GetFlags(object value) => GetFlags(ToObject(value)).GetNonGenericContainer();

        public override IValuesContainer GetFlags(ref byte value) => GetFlags(UnsafeUtility.As<byte, TUnderlying>(ref value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IValuesContainer GetFlags(TUnderlying value) => EnumBridge.CreateValuesContainer(EnumerateFlags(value), GetFlagCount(value), false);

        private IEnumerable<TUnderlying> EnumerateFlags(TUnderlying value)
        {
            TUnderlyingOperations operations = default;
            var validValue = operations.And(value, _allFlags);
            var isLessThanZero = operations.LessThan(validValue, default);
            for (var currentValue = operations.One; (isLessThanZero || !operations.LessThan(validValue, currentValue)) && !currentValue.Equals(default); currentValue = operations.LeftShift(currentValue, 1))
            {
                if (HasAnyFlags(validValue, currentValue))
                {
                    yield return currentValue;
                }
            }
        }

        public override IReadOnlyList<EnumMember> GetFlagMembers(ref byte value) => GetFlagMembers(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override IReadOnlyList<EnumMember> GetFlagMembers(object value) => GetFlagMembers(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<EnumMember> GetFlagMembers(TUnderlying value) => EnumBridge.CreateMembersContainer(EnumerateFlags(value).Select(flag => GetMember(flag)!), GetFlagCount(value), false);

        public override int GetFlagCount() => default(TUnderlyingOperations).BitCount(_allFlags);

        public override int GetFlagCount(ref byte value) => GetFlagCount(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override int GetFlagCount(object value) => GetFlagCount(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetFlagCount(TUnderlying value)
        {
            TUnderlyingOperations operations = default;
            return operations.BitCount(operations.And(value, _allFlags));
        }

        public override int GetFlagCount(ref byte value, ref byte otherFlags) => GetFlagCount(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));

        public override int GetFlagCount(object value, object otherFlags) => GetFlagCount(ToObject(value), ToObject(otherFlags));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetFlagCount(TUnderlying value, TUnderlying otherFlags)
        {
            TUnderlyingOperations operations = default;
            return operations.BitCount(operations.And(operations.And(value, otherFlags), _allFlags));
        }

        public override bool HasAnyFlags(ref byte value) => HasAnyFlags(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override bool HasAnyFlags(object value) => HasAnyFlags(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyFlags(TUnderlying value) => !value.Equals(default);

        public override bool HasAnyFlags(ref byte value, ref byte otherFlags) => HasAnyFlags(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));

        public override bool HasAnyFlags(object value, object otherFlags) => HasAnyFlags(ToObject(value), ToObject(otherFlags));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyFlags(TUnderlying value, TUnderlying otherFlags) => !default(TUnderlyingOperations).And(value, otherFlags).Equals(default);

        public override bool HasAllFlags(ref byte value) => HasAllFlags(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override bool HasAllFlags(object value) => HasAllFlags(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAllFlags(TUnderlying value) => HasAllFlags(value, _allFlags);

        public override bool HasAllFlags(ref byte value, ref byte otherFlags) => HasAllFlags(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));

        public override bool HasAllFlags(object value, object otherFlags) => HasAllFlags(ToObject(value), ToObject(otherFlags));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAllFlags(TUnderlying value, TUnderlying otherFlags) => default(TUnderlyingOperations).And(value, otherFlags).Equals(otherFlags);

        public override void ToggleFlags(ref byte value, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ToggleFlags(UnsafeUtility.As<byte, TUnderlying>(ref value));
        }

        public override object ToggleFlags(object value) => EnumBridge.ToObjectUnchecked(ToggleFlags(ToObject(value)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TUnderlying ToggleFlags(TUnderlying value) => default(TUnderlyingOperations).Xor(value, _allFlags);

        public override void ToggleFlags(ref byte value, ref byte otherFlags, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ToggleFlags(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));
        }

        public override object ToggleFlags(object value, object otherFlags) => EnumBridge.ToObjectUnchecked(ToggleFlags(ToObject(value), ToObject(otherFlags)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TUnderlying ToggleFlags(TUnderlying value, TUnderlying otherFlags) => default(TUnderlyingOperations).Xor(value, otherFlags);

        public override void CommonFlags(ref byte value, ref byte otherFlags, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = CommonFlags(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));
        }

        public override object CommonFlags(object value, object otherFlags) => EnumBridge.ToObjectUnchecked(CommonFlags(ToObject(value), ToObject(otherFlags)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TUnderlying CommonFlags(TUnderlying value, TUnderlying otherFlags) => default(TUnderlyingOperations).And(value, otherFlags);

        public override void CombineFlags(ref byte value, ref byte otherFlags, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = default(TUnderlyingOperations).Or(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));
        }

        public override object CombineFlags(object value, object otherFlags) => EnumBridge.ToObjectUnchecked(default(TUnderlyingOperations).Or(ToObject(value), ToObject(otherFlags)));

        public override void CombineFlags(ref byte flag0, ref byte flag1, ref byte flag2, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            TUnderlyingOperations operations = default;
            underlying = operations.Or(operations.Or(UnsafeUtility.As<byte, TUnderlying>(ref flag0), UnsafeUtility.As<byte, TUnderlying>(ref flag1)), UnsafeUtility.As<byte, TUnderlying>(ref flag2));
        }

        public override void CombineFlags(ref byte flag0, ref byte flag1, ref byte flag2, ref byte flag3, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            TUnderlyingOperations operations = default;
            underlying = operations.Or(operations.Or(operations.Or(UnsafeUtility.As<byte, TUnderlying>(ref flag0), UnsafeUtility.As<byte, TUnderlying>(ref flag1)), UnsafeUtility.As<byte, TUnderlying>(ref flag2)), UnsafeUtility.As<byte, TUnderlying>(ref flag3));
        }

        public override void CombineFlags(ref byte flag0, ref byte flag1, ref byte flag2, ref byte flag3, ref byte flag4, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            TUnderlyingOperations operations = default;
            underlying = operations.Or(operations.Or(operations.Or(operations.Or(UnsafeUtility.As<byte, TUnderlying>(ref flag0), UnsafeUtility.As<byte, TUnderlying>(ref flag1)), UnsafeUtility.As<byte, TUnderlying>(ref flag2)), UnsafeUtility.As<byte, TUnderlying>(ref flag3)), UnsafeUtility.As<byte, TUnderlying>(ref flag4));
        }

        public override object CombineFlags(IEnumerable<object?>? flags, bool isNullable)
        {
            TUnderlyingOperations operations = default;
            TUnderlying result = default;
            if (flags != null)
            {
                foreach (var flag in flags)
                {
                    if (!isNullable || flag != null)
                    {
                        result = operations.Or(result, ToObject(flag!));
                    }
                }
            }
            return EnumBridge.ToObjectUnchecked(result);
        }

        public override void RemoveFlags(ref byte value, ref byte otherFlags, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = RemoveFlags(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));
        }

        public override object RemoveFlags(object value, object otherFlags) => EnumBridge.ToObjectUnchecked(RemoveFlags(ToObject(value), ToObject(otherFlags)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TUnderlying RemoveFlags(TUnderlying value, TUnderlying otherFlags)
        {
            TUnderlyingOperations operations = default;
            return operations.And(value, operations.Not(otherFlags));
        }
        #endregion

        #region Parsing
        public override void ParseFlags(ParseType value, bool ignoreCase, string? delimiter, ValueCollection<EnumFormat> formats, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ParseFlagsInternal(value, ignoreCase, delimiter, formats);
        }

        public override object ParseFlags(ParseType value, bool ignoreCase, string? delimiter, ValueCollection<EnumFormat> formats) => EnumBridge.ToObjectUnchecked(ParseFlagsInternal(value, ignoreCase, delimiter, formats));

        public TUnderlying ParseFlagsInternal(ParseType value, bool ignoreCase, string? delimiter, ValueCollection<EnumFormat> formats)
        {
            if (string.IsNullOrEmpty(delimiter))
            {
                delimiter = FlagEnums.DefaultDelimiter;
            }

#if SPAN
            var effectiveDelimiter = delimiter.AsSpan().Trim();
            if (effectiveDelimiter.Length == 0)
            {
                effectiveDelimiter = delimiter;
            }
#else
            var effectiveDelimiter = delimiter!.Trim();
            if (effectiveDelimiter.Length == 0)
            {
                effectiveDelimiter = delimiter;
            }
#endif

            if (!formats.Any())
            {
                formats = Enums.DefaultFormats;
            }

            TUnderlyingOperations operations = default;
            TUnderlying result = default;
            var startIndex = 0;
            var valueLength = value.Length;
            while (startIndex < valueLength)
            {
                while (startIndex < valueLength && char.IsWhiteSpace(value[startIndex]))
                {
                    ++startIndex;
                }
#if SPAN
                var delimiterIndex = value.Slice(startIndex).IndexOf(effectiveDelimiter, StringComparison.Ordinal);
#else
                var delimiterIndex = value.IndexOf(effectiveDelimiter, startIndex, StringComparison.Ordinal);
#endif
                if (delimiterIndex < 0)
                {
                    delimiterIndex = valueLength;
                }
                var newStartIndex = delimiterIndex + effectiveDelimiter.Length;
                while (delimiterIndex > startIndex && char.IsWhiteSpace(value[delimiterIndex - 1]))
                {
                    --delimiterIndex;
                }
#if SPAN
                var indValue = value.Slice(startIndex, delimiterIndex - startIndex);
#else
                var indValue = value.Substring(startIndex, delimiterIndex - startIndex);
#endif
                if (TryParseInternal(indValue, ignoreCase, out var underlying, out _, formats, true))
                {
                    result = operations.Or(result, underlying);
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

        public override bool TryParseFlags(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, string? delimiter, ref byte result, ValueCollection<EnumFormat> formats)
        {
            if (TryParseFlags(value, ignoreCase, delimiter, out var r, formats))
            {
                ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
                underlying = r;
                return true;
            }
            return false;
        }

        public override bool TryParseFlags(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, string? delimiter, out object? result, ValueCollection<EnumFormat> formats)
        {
            if (TryParseFlags(value, ignoreCase, delimiter, out var r, formats))
            {
                result = EnumBridge.ToObjectUnchecked(r);
                return true;
            }
            result = default;
            return false;
        }

        public bool TryParseFlags(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, string? delimiter, out TUnderlying result, ValueCollection<EnumFormat> formats)
        {
            if (value == null)
            {
                result = default;
                return false;
            }

            if (string.IsNullOrEmpty(delimiter))
            {
                delimiter = FlagEnums.DefaultDelimiter;
            }

#if SPAN
            var effectiveDelimiter = delimiter.AsSpan().Trim();
            if (effectiveDelimiter.Length == 0)
            {
                effectiveDelimiter = delimiter;
            }
#else
            var effectiveDelimiter = delimiter!.Trim();
            if (effectiveDelimiter.Length == 0)
            {
                effectiveDelimiter = delimiter;
            }
#endif

            if (!formats.Any())
            {
                formats = Enums.DefaultFormats;
            }

            TUnderlying resultAsInt = default;
            var startIndex = 0;
            var valueLength = value.Length;
            TUnderlyingOperations operations = default;
            while (startIndex < valueLength)
            {
                while (startIndex < valueLength && char.IsWhiteSpace(value[startIndex]))
                {
                    ++startIndex;
                }
#if SPAN
                var delimiterIndex = value.Slice(startIndex).IndexOf(effectiveDelimiter, StringComparison.Ordinal);
#else
                var delimiterIndex = value.IndexOf(effectiveDelimiter, startIndex, StringComparison.Ordinal);
#endif
                if (delimiterIndex < 0)
                {
                    delimiterIndex = valueLength;
                }
                var newStartIndex = delimiterIndex + effectiveDelimiter.Length;
                while (delimiterIndex > startIndex && char.IsWhiteSpace(value[delimiterIndex - 1]))
                {
                    --delimiterIndex;
                }
#if SPAN
                var indValue = value.Slice(startIndex, delimiterIndex - startIndex);
#else
                var indValue = value.Substring(startIndex, delimiterIndex - startIndex);
#endif
                if (!TryParseInternal(indValue, ignoreCase, out var underlying, out _, formats, true))
                {
                    result = default;
                    return false;
                }
                resultAsInt = operations.Or(resultAsInt, underlying);
                startIndex = newStartIndex;
            }
            result = resultAsInt;
            return true;
        }
        #endregion
        #endregion

        internal sealed class EnumMemberParser
        {
            private readonly struct Entry
            {
                public readonly int OrdinalHashCode;
                public readonly int OrdinalNext;
                public readonly int OrdinalIgnoreCaseHashCode;
                public readonly int OrdinalIgnoreCaseNext;
                public readonly string FormattedValue;
                public readonly EnumMemberInternal<TUnderlying, TUnderlyingOperations> Member;

                public Entry(int ordinalHashCode, int ordinalNext, int ordinalIgnoreCaseHashCode, int ordinalIgnoreCaseNext, string formattedValue, EnumMemberInternal<TUnderlying, TUnderlyingOperations> member)
                {
                    OrdinalHashCode = ordinalHashCode;
                    OrdinalNext = ordinalNext;
                    OrdinalIgnoreCaseHashCode = ordinalIgnoreCaseHashCode;
                    OrdinalIgnoreCaseNext = ordinalIgnoreCaseNext;
                    FormattedValue = formattedValue;
                    Member = member;
                }
            }

            private readonly int[] _ordinalBuckets;
            private readonly int[] _ordinalIgnoreCaseBuckets;
            private readonly Entry[] _entries;

            public EnumMemberParser(EnumFormat format, EnumCache<TUnderlying, TUnderlyingOperations> enumCache)
            {
                var size = HashHelpers.PowerOf2(enumCache.GetMemberCount(EnumMemberSelection.All));
                var ordinalBuckets = new int[size];
                var ordinalIgnoreCaseBuckets = new int[size];
                var entries = new Entry[size];
                var i = 0;
                foreach (var member in enumCache.GetInternalMembers(EnumMemberSelection.All))
                {
                    var formattedValue = member.AsString(format);
                    if (formattedValue != null)
                    {
                        var ordinalHashCode = formattedValue.GetHashCode();
                        ref int ordinalBucket = ref ordinalBuckets[ordinalHashCode & (size - 1)];
                        var ordinalIgnoreCaseHashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(formattedValue);
                        ref int ordinalIgnoreCaseBucket = ref ordinalIgnoreCaseBuckets[ordinalIgnoreCaseHashCode & (size - 1)];
                        entries[i] = new Entry(ordinalHashCode, ordinalBucket - 1, ordinalIgnoreCaseHashCode, ordinalIgnoreCaseBucket - 1, formattedValue, member);
                        ordinalBucket = i + 1;
                        ordinalIgnoreCaseBucket = i + 1;
                    }
                    ++i;
                }
                _ordinalBuckets = ordinalBuckets;
                _ordinalIgnoreCaseBuckets = ordinalIgnoreCaseBuckets;
                _entries = entries;
            }

            internal bool TryParse(ParseType formattedValue, bool ignoreCase, [NotNullWhen(true)] out EnumMemberInternal<TUnderlying, TUnderlyingOperations>? result)
            {
                var entries = _entries;
                if (ignoreCase)
                {
#if SPAN
                    var hashCode = string.GetHashCode(formattedValue, StringComparison.OrdinalIgnoreCase);
#else
                    var hashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(formattedValue);
#endif
                    for (var i = _ordinalIgnoreCaseBuckets[hashCode & (_ordinalIgnoreCaseBuckets.Length - 1)] - 1; i >= 0; i = entries[i].OrdinalIgnoreCaseNext)
                    {
                        if (entries[i].OrdinalIgnoreCaseHashCode == hashCode &&
#if SPAN
                            entries[i].FormattedValue.AsSpan().Equals(formattedValue, StringComparison.OrdinalIgnoreCase))
#else
                            string.Equals(entries[i].FormattedValue, formattedValue, StringComparison.OrdinalIgnoreCase))
#endif
                        {
                            result = entries[i].Member;
                            return true;
                        }
                    }
                }
                else
                {
#if SPAN
                    var hashCode = string.GetHashCode(formattedValue, StringComparison.Ordinal);
#else
                    var hashCode = formattedValue.GetHashCode();
#endif
                    for (var i = _ordinalBuckets[hashCode & (_ordinalBuckets.Length - 1)] - 1; i >= 0; i = entries[i].OrdinalNext)
                    {
                        if (entries[i].OrdinalHashCode == hashCode &&
#if SPAN
                            entries[i].FormattedValue.AsSpan().Equals(formattedValue, StringComparison.Ordinal))
#else
                            string.Equals(entries[i].FormattedValue, formattedValue, StringComparison.Ordinal))
#endif
                        {
                            result = entries[i].Member;
                            return true;
                        }
                    }
                }
                result = default;
                return false;
            }
        }

    }
}