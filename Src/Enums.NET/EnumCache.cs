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
#if DISPLAY_ATTRIBUTE
using System.ComponentModel.DataAnnotations;
#endif
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using EnumsNET.Numerics;
using EnumsNET.Utilities;

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
        private protected readonly bool _hasDuplicateValues;
        private IReadOnlyList<string>? _names;
        private IValuesContainer? _values;
        private IReadOnlyList<EnumMember>? _members;
        private EnumComparer? _enumComparer;

        protected EnumCache(Type underlyingType, bool isFlagEnum, bool hasDuplicateValues)
        {
            UnderlyingType = underlyingType;
            TypeCode = underlyingType.GetTypeCode();
            IsFlagEnum = isFlagEnum;
            _hasDuplicateValues = hasDuplicateValues;
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

        public EnumComparer EnumComparer
        {
            get
            {
                var enumComparer = _enumComparer;
                return enumComparer ?? Interlocked.CompareExchange(ref _enumComparer, (enumComparer = CreateEnumComparer()), null) ?? enumComparer;
            }
        }

        protected abstract EnumComparer CreateEnumComparer();
        public abstract string AsString(ref byte value);
        public abstract string AsString(ref byte value, string format);
        public abstract string? AsString(ref byte value, EnumFormat format);
        public abstract string? AsString(ref byte value, ValueCollection<EnumFormat> formats);
        public abstract string AsString(object value);
        public abstract string AsString(object value, string format);
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
        public abstract int GetHashCode(object value);
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

    internal abstract class EnumCache<TUnderlying, TUnderlyingOperations> : EnumCache
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        internal readonly IEnumBridge<TUnderlying, TUnderlyingOperations> EnumBridge;
        private readonly bool _isContiguous;
        private readonly object? _customValidator;
        private protected readonly string _enumTypeName;
        private readonly TUnderlying _allFlags;
        private protected readonly TUnderlying _maxDefined;
        private protected readonly TUnderlying _minDefined;
        private readonly EnumMemberInternal<TUnderlying, TUnderlyingOperations>?[] _buckets; // A hash bucket for lookups by value
        private readonly EnumMemberInternal<TUnderlying, TUnderlyingOperations>[] _members;
        private readonly int _distinctCount;
        private EnumMemberParser?[] _enumMemberParsers = ArrayHelper.Empty<EnumMemberParser>();

        protected EnumCache(Type enumType, IEnumBridge<TUnderlying, TUnderlyingOperations> enumBridge, bool isFlagEnum, EnumMemberInternal<TUnderlying, TUnderlyingOperations>[] members, EnumMemberInternal<TUnderlying, TUnderlyingOperations>?[] buckets, TUnderlying allFlags, int distinctCount, bool isContiguous, object? customValidator)
            : base(typeof(TUnderlying), isFlagEnum, distinctCount != members.Length)
        {
            _enumTypeName = enumType.Name;
            EnumBridge = enumBridge;
            _customValidator = customValidator;
            _members = members;
            _buckets = buckets;
            _allFlags = allFlags;
            _distinctCount = distinctCount;
            _isContiguous = isContiguous;
            if (members.Length > 0)
            {
                _maxDefined = members[members.Length - 1].Value;
                _minDefined = members[0].Value;
            }
            foreach (var member in members)
            {
                member.EnumCache = this;
            }
        }

        #region Standard Enum Operations
        #region Type Methods
        protected sealed override EnumComparer CreateEnumComparer() => EnumBridge.CreateEnumComparer(this);

        public sealed override int GetMemberCount(EnumMemberSelection selection)
        {
            switch (selection)
            {
                case EnumMemberSelection.All:
#if DISPLAY_ATTRIBUTE
                case EnumMemberSelection.DisplayOrder:
#endif
                    return _members.Length;
                case EnumMemberSelection.Flags:
                case EnumMemberSelection.Flags | EnumMemberSelection.Distinct:
#if DISPLAY_ATTRIBUTE
                case EnumMemberSelection.Flags | EnumMemberSelection.DisplayOrder:
                case EnumMemberSelection.Flags | EnumMemberSelection.DisplayOrder | EnumMemberSelection.Distinct:
#endif
                    return GetFlagCount();
                case EnumMemberSelection.Distinct:
#if DISPLAY_ATTRIBUTE
                case EnumMemberSelection.Distinct | EnumMemberSelection.DisplayOrder:
#endif
                    return _distinctCount;
                default:
                    throw new ArgumentException($"invalid value of {selection.AsString()} for EnumMemberSelection", nameof(selection));
            }
        }

        protected sealed override IReadOnlyList<EnumMember> GetMembersInternal(EnumMemberSelection selection, bool cached) => EnumBridge.CreateMembersContainer(GetMembersInternal(selection), GetMemberCount(selection), cached);

        protected sealed override IReadOnlyList<string> GetNamesInternal(EnumMemberSelection selection, bool cached) => new NamesContainer(GetMembersInternal(selection), GetMemberCount(selection), cached);

        protected sealed override IValuesContainer GetValuesInternal(EnumMemberSelection selection, bool cached) => EnumBridge.CreateValuesContainer(GetMembersInternal(selection), GetMemberCount(selection), cached);

        private IEnumerable<EnumMemberInternal<TUnderlying, TUnderlyingOperations>> GetMembersInternal(EnumMemberSelection selection)
        {
            IEnumerable<EnumMemberInternal<TUnderlying, TUnderlyingOperations>> members;
            switch (selection)
            {
                case EnumMemberSelection.All:
#if DISPLAY_ATTRIBUTE
                case EnumMemberSelection.DisplayOrder:
#endif
                    members = _members;
                    break;
                case EnumMemberSelection.Flags:
                case EnumMemberSelection.Flags | EnumMemberSelection.Distinct:
#if DISPLAY_ATTRIBUTE
                case EnumMemberSelection.Flags | EnumMemberSelection.DisplayOrder:
                case EnumMemberSelection.Flags | EnumMemberSelection.DisplayOrder | EnumMemberSelection.Distinct:
#endif
                    members = EnumerateFlagMembers(_allFlags);
                    break;
                case EnumMemberSelection.Distinct:
#if DISPLAY_ATTRIBUTE
                case EnumMemberSelection.Distinct | EnumMemberSelection.DisplayOrder:
#endif
                    members = _hasDuplicateValues ? _members.Distinct() : _members;
                    break;
                default:
                    throw new ArgumentException($"invalid value of {selection.AsString()} for EnumMemberSelection", nameof(selection));
            }

#if DISPLAY_ATTRIBUTE
            return selection.HasAnyFlags(EnumMemberSelection.DisplayOrder)
                ? members.OrderBy(m => m.Attributes.Get<DisplayAttribute>()?.GetOrder() ?? int.MaxValue)
                : members;
#else
            return members;
#endif
        }
        #endregion

        #region ToObject
        public sealed override void ToObject(ulong value, EnumValidation validation, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ToObjectInternal(value, validation);
        }

        public sealed override object ToObject(ulong value, EnumValidation validation) => EnumBridge.ToObjectUnchecked(ToObjectInternal(value, validation));

        public sealed override void ToObject(object value, EnumValidation validation, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ToObjectInternal(value, validation);
        }

        public sealed override object ToObject(object value, EnumValidation validation) => EnumBridge.ToObjectUnchecked(ToObjectInternal(value, validation));

        public sealed override void ToObject(long value, EnumValidation validation, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ToObjectInternal(value, validation);
        }

        public sealed override object ToObject(long value, EnumValidation validation) => EnumBridge.ToObjectUnchecked(ToObjectInternal(value, validation));

        public TUnderlying ToObject(object value)
        {
            var v = EnumBridge.IsEnum(value);
            if (v.HasValue)
            {
                return v.GetValueOrDefault();
            }

            if (value is TUnderlying u)
            {
                return u;
            }

            Preconditions.NotNull(value, nameof(value));

            var type = value.GetType();

            return ((Nullable.GetUnderlyingType(type) ?? type).GetTypeCode()) switch
            {
                TypeCode.Boolean => ToObject(Convert.ToByte((bool)value)),
                TypeCode.Char => ToObject((char)value),
                TypeCode.SByte => ToObject((sbyte)value),
                TypeCode.Byte => ToObject((byte)value),
                TypeCode.Int16 => ToObject((short)value),
                TypeCode.UInt16 => ToObject((ushort)value),
                TypeCode.Int32 => ToObject((int)value),
                TypeCode.UInt32 => ToObject((uint)value),
                TypeCode.Int64 => ToObject((long)value),
                TypeCode.UInt64 => ToObject((ulong)value),
                TypeCode.String => ParseInternal((string)value, false, Enums.DefaultFormats),
                _ => throw new ArgumentException($"value is not type {_enumTypeName}, SByte, Int16, Int32, Int64, Byte, UInt16, UInt32, UInt64, Boolean, Char, or String."),
            };
        }

        public TUnderlying ToObjectInternal(object value, EnumValidation validation)
        {
            var v = EnumBridge.IsEnum(value);
            if (v.HasValue)
            {
                Validate(v.GetValueOrDefault(), nameof(value), validation);
                return v.GetValueOrDefault();
            }

            if (value is TUnderlying u)
            {
                Validate(u, nameof(value), validation);
                return u;
            }

            Preconditions.NotNull(value, nameof(value));

            var type = value.GetType();

            switch ((Nullable.GetUnderlyingType(type) ?? type).GetTypeCode())
            {
                case TypeCode.Boolean:
                    return ToObjectInternal(Convert.ToByte((bool)value), validation);
                case TypeCode.Char:
                    return ToObjectInternal((char)value, validation);
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
            }
            throw new ArgumentException($"value is not type {_enumTypeName}, SByte, Int16, Int32, Int64, Byte, UInt16, UInt32, UInt64, Boolean, Char, or String.");
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

        public sealed override bool TryToObject(object? value, ref byte result, EnumValidation validation)
        {
            if (TryToObject(value, out var r, validation))
            {
                ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
                underlying = r;
                return true;
            }
            return false;
        }

        public sealed override bool TryToObject(object? value, out object? result, EnumValidation validation)
        {
            if (TryToObject(value, out var r, validation))
            {
                result = EnumBridge.ToObjectUnchecked(r);
                return true;
            }
            result = default;
            return false;
        }

        public sealed override bool TryToObject(long value, ref byte result, EnumValidation validation)
        {
            if (TryToObject(value, out var r, validation))
            {
                ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
                underlying = r;
                return true;
            }
            return false;
        }

        public sealed override bool TryToObject(long value, out object? result, EnumValidation validation)
        {
            if (TryToObject(value, out var r, validation))
            {
                result = EnumBridge.ToObjectUnchecked(r);
                return true;
            }
            result = default;
            return false;
        }

        public sealed override bool TryToObject(ulong value, ref byte result, EnumValidation validation)
        {
            if (TryToObject(value, out var r, validation))
            {
                ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
                underlying = r;
                return true;
            }
            return false;
        }

        public sealed override bool TryToObject(ulong value, out object? result, EnumValidation validation)
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
                var v = EnumBridge.IsEnum(value);
                if (v.HasValue)
                {
                    result = v.GetValueOrDefault();
                    return IsValid(result, validation);
                }

                if (value is TUnderlying u)
                {
                    result = u;
                    return IsValid(u, validation);
                }

                var type = value.GetType();

                switch ((Nullable.GetUnderlyingType(type) ?? type).GetTypeCode())
                {
                    case TypeCode.Boolean:
                        return TryToObject(Convert.ToByte((bool)value), out result, validation);
                    case TypeCode.Char:
                        return TryToObject((char)value, out result, validation);
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
        public sealed override bool IsValid(ref byte value, EnumValidation validation) => IsValid(UnsafeUtility.As<byte, TUnderlying>(ref value), validation);

        public sealed override bool IsValid(object value, EnumValidation validation) => IsValid(ToObject(value), validation);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValid(TUnderlying value, EnumValidation validation) => validation switch
        {
            EnumValidation.None => true,
            EnumValidation.Default => _customValidator != null ? EnumBridge.CustomValidate(_customValidator, value) : (IsFlagEnum && IsValidFlagCombination(value)) || IsDefined(value),
            EnumValidation.IsDefined => IsDefined(value),
            EnumValidation.IsValidFlagCombination => IsValidFlagCombination(value),
            _ => validation.Validate(nameof(validation)) != validation,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsDefined(TUnderlying value) => _isContiguous ? default(TUnderlyingOperations).InRange(value, _minDefined, _maxDefined) : GetMember(value) != null;

        public sealed override void Validate(ref byte value, string paramName, EnumValidation validation) => Validate(UnsafeUtility.As<byte, TUnderlying>(ref value), paramName, validation);

        public sealed override object Validate(object value, string paramName, EnumValidation validation)
        {
            var underlying = ToObject(value);
            Validate(underlying, paramName, validation);
            return EnumBridge.ToObjectUnchecked(underlying);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Validate(TUnderlying value, string paramName, EnumValidation validation)
        {
            if (!IsValid(value, validation))
            {
                throw new ArgumentException($"invalid value of {AsString(value)} for {_enumTypeName}", paramName);
            }
        }

        public sealed override string AsString(ref byte value, string format) => AsStringInternal(UnsafeUtility.As<byte, TUnderlying>(ref value), null, format);

        public sealed override string? AsString(ref byte value, EnumFormat format) => AsString(UnsafeUtility.As<byte, TUnderlying>(ref value), format);

        public sealed override string? AsString(ref byte value, ValueCollection<EnumFormat> formats) => AsStringInternal(UnsafeUtility.As<byte, TUnderlying>(ref value), null, formats);

        public sealed override string AsString(object value, string format) => AsStringInternal(ToObject(value), null, format);

        public sealed override string? AsString(object value, EnumFormat format) => AsString(ToObject(value), format);

        public sealed override string? AsString(object value, ValueCollection<EnumFormat> formats) => AsStringInternal(ToObject(value), null, formats);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string AsString(TUnderlying value)
        {
            var member = GetMember(value);
            return IsFlagEnum ? FormatFlagsInternal(value, member, null, Enums.DefaultFormats)! : member?.Name ?? value.ToString()!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string? AsString(TUnderlying value, EnumFormat format) => format switch
        {
            EnumFormat.DecimalValue => default(TUnderlyingOperations).ToDecimalString(value),
            EnumFormat.HexadecimalValue => default(TUnderlyingOperations).ToHexadecimalString(value),
            EnumFormat.UnderlyingValue => value.ToString(),
            EnumFormat.Name => GetMember(value)?.Name,
            EnumFormat.Description => GetMember(value)?.Attributes.Get<DescriptionAttribute>()?.Description,
            EnumFormat.EnumMemberValue => GetMember(value)?.Attributes.Get<EnumMemberAttribute>()?.Value,
#if DISPLAY_ATTRIBUTE
            EnumFormat.DisplayName => GetMember(value)?.Attributes.Get<DisplayAttribute>()?.GetName(),
#endif
            _ => Enums.CustomEnumMemberFormat(GetMember(value)?.EnumMember, format.Validate(nameof(format)))
        };

        internal string AsStringInternal(TUnderlying value, EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member, string format)
        {
            if (format.Length == 1)
            {
                switch (format[0])
                {
                    case 'G':
                    case 'g':
                        return IsFlagEnum ? FormatFlagsInternal(value, member ?? GetMember(value), null, Enums.DefaultFormats)! : (member ?? GetMember(value))?.Name ?? value.ToString()!;
                    case 'F':
                    case 'f':
                        return FormatFlagsInternal(value, member ?? GetMember(value), null, Enums.DefaultFormats)!;
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

        internal string? AsStringInternal(TUnderlying value, ref bool isInitialized, ref EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member, EnumFormat format) => format switch
        {
            EnumFormat.DecimalValue => default(TUnderlyingOperations).ToDecimalString(value),
            EnumFormat.HexadecimalValue => default(TUnderlyingOperations).ToHexadecimalString(value),
            EnumFormat.UnderlyingValue => value.ToString(),
            EnumFormat.Name => TryInitializeMember(value, ref isInitialized, ref member)?.Name,
            EnumFormat.Description => TryInitializeMember(value, ref isInitialized, ref member)?.Attributes.Get<DescriptionAttribute>()?.Description,
            EnumFormat.EnumMemberValue => TryInitializeMember(value, ref isInitialized, ref member)?.Attributes.Get<EnumMemberAttribute>()?.Value,
#if DISPLAY_ATTRIBUTE
            EnumFormat.DisplayName => TryInitializeMember(value, ref isInitialized, ref member)?.Attributes.Get<DisplayAttribute>()?.GetName(),
#endif
            _ => Enums.CustomEnumMemberFormat(TryInitializeMember(value, ref isInitialized, ref member)?.EnumMember, format.Validate(nameof(format)))
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private EnumMemberInternal<TUnderlying, TUnderlyingOperations>? TryInitializeMember(TUnderlying value, ref bool isInitialized, ref EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member)
        {
            if (!isInitialized)
            {
                member = GetMember(value);
                isInitialized = true;
            }
            return member;
        }

        internal string? AsStringInternal(TUnderlying value, EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member, ValueCollection<EnumFormat> formats)
        {
            var isInitialized = member != null;
            foreach (var format in formats)
            {
                var formattedValue = AsStringInternal(value, ref isInitialized, ref member, format);
                if (formattedValue != null)
                {
                    return formattedValue;
                }
            }
            return null;
        }

        public sealed override object GetUnderlyingValue(object value) => ToObject(value);

        public sealed override object GetUnderlyingValue(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value);

        public sealed override int GetHashCode(object value) => ToObject(value).GetHashCode();

        public sealed override int GetHashCode(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).GetHashCode();

        public sealed override bool Equals(ref byte value, ref byte other) => UnsafeUtility.As<byte, TUnderlying>(ref value).Equals(UnsafeUtility.As<byte, TUnderlying>(ref other));

        public sealed override bool Equals(object value, object other) => ToObject(value).Equals(ToObject(other));

        public sealed override int CompareTo(ref byte value, ref byte other) => UnsafeUtility.As<byte, TUnderlying>(ref value).CompareTo(UnsafeUtility.As<byte, TUnderlying>(ref other));

        public sealed override int CompareTo(object value, object other) => ToObject(value).CompareTo(ToObject(other));

#if ICONVERTIBLE
        public sealed override sbyte ToSByte(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToSByte(null);

        public sealed override sbyte ToSByte(object value) => ToObject(value).ToSByte(null);

        public sealed override byte ToByte(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToByte(null);

        public sealed override byte ToByte(object value) => ToObject(value).ToByte(null);

        public sealed override short ToInt16(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToInt16(null);

        public sealed override short ToInt16(object value) => ToObject(value).ToInt16(null);

        public sealed override ushort ToUInt16(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToUInt16(null);

        public sealed override ushort ToUInt16(object value) => ToObject(value).ToUInt16(null);

        public sealed override int ToInt32(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToInt32(null);

        public sealed override int ToInt32(object value) => ToObject(value).ToInt32(null);

        public sealed override uint ToUInt32(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToUInt32(null);

        public sealed override uint ToUInt32(object value) => ToObject(value).ToUInt32(null);

        public sealed override long ToInt64(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToInt64(null);

        public sealed override long ToInt64(object value) => ToObject(value).ToInt64(null);

        public sealed override ulong ToUInt64(ref byte value) => UnsafeUtility.As<byte, TUnderlying>(ref value).ToUInt64(null);

        public sealed override ulong ToUInt64(object value) => ToObject(value).ToUInt64(null);
#else
        public sealed override sbyte ToSByte(ref byte value) => default(TUnderlyingOperations).ToSByte(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override sbyte ToSByte(object value) => default(TUnderlyingOperations).ToSByte(ToObject(value));

        public sealed override byte ToByte(ref byte value) => default(TUnderlyingOperations).ToByte(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override byte ToByte(object value) => default(TUnderlyingOperations).ToByte(ToObject(value));

        public sealed override short ToInt16(ref byte value) => default(TUnderlyingOperations).ToInt16(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override short ToInt16(object value) => default(TUnderlyingOperations).ToInt16(ToObject(value));

        public sealed override ushort ToUInt16(ref byte value) => default(TUnderlyingOperations).ToUInt16(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override ushort ToUInt16(object value) => default(TUnderlyingOperations).ToUInt16(ToObject(value));

        public sealed override int ToInt32(ref byte value) => default(TUnderlyingOperations).ToInt32(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override int ToInt32(object value) => default(TUnderlyingOperations).ToInt32(ToObject(value));

        public sealed override uint ToUInt32(ref byte value) => default(TUnderlyingOperations).ToUInt32(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override uint ToUInt32(object value) => default(TUnderlyingOperations).ToUInt32(ToObject(value));

        public sealed override long ToInt64(ref byte value) => default(TUnderlyingOperations).ToInt64(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override long ToInt64(object value) => default(TUnderlyingOperations).ToInt64(ToObject(value));

        public sealed override ulong ToUInt64(ref byte value) => default(TUnderlyingOperations).ToUInt64(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override ulong ToUInt64(object value) => default(TUnderlyingOperations).ToUInt64(ToObject(value));
#endif
        #endregion

        #region Defined Values Main Methods
        public override EnumMemberInternal? GetMember(ref byte value) => GetMember(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override EnumMemberInternal? GetMember(object value) => GetMember(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EnumMemberInternal<TUnderlying, TUnderlyingOperations>? GetMember(TUnderlying value)
        {
            var next = _buckets[value.GetHashCode() & (_buckets.Length - 1)];
            while (next?.Value.Equals(value) == false)
            {
                next = next.Next;
            }
            return next;
        }

        public sealed override EnumMember? GetMember(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats)
        {
            TryParseInternal(value.Trim(), ignoreCase, out _, out var member, formats, false);
            return member?.EnumMember;
        }
        #endregion

        #region Parsing
        public TUnderlying ParseInternal(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats)
        {
            if (IsFlagEnum)
            {
                return ParseFlagsInternal(value, ignoreCase, null, formats);
            }

            value = value.Trim();

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

                return TryParseInternal(value, ignoreCase, out result, out _, formats, true);
            }
            result = default;
            return false;
        }

        protected bool TryParseInternal(ParseType value, bool ignoreCase, out TUnderlying result, out EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member, ValueCollection<EnumFormat> formats, bool getValueOnly)
        {
            Debug.Assert(formats.Count > 0);

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
                    var index = format - EnumFormat.Name;
                    var parsers = _enumMemberParsers;
                    EnumMemberParser? parser;
                    if ((uint)index >= (uint)parsers.Length || (parser = parsers[index]) == null)
                    {
                        format.Validate(nameof(format));

                        parser = new EnumMemberParser(format, this);
                        EnumMemberParser?[] oldParsers;
                        do
                        {
                            oldParsers = parsers;
                            parsers = new EnumMemberParser?[Math.Max(oldParsers.Length, index + 1)];
                            oldParsers.CopyTo(parsers, 0);
                            parsers[index] = parser;
                        } while ((parsers = Interlocked.CompareExchange(ref _enumMemberParsers, parsers, oldParsers)) != oldParsers);
                    }
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
        public sealed override void GetAllFlags(ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = _allFlags;
        }

        public sealed override object GetAllFlags() => EnumBridge.ToObjectUnchecked(_allFlags);

        #region Main Methods
        public sealed override bool IsValidFlagCombination(ref byte value) => IsValidFlagCombination(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override bool IsValidFlagCombination(object value) => IsValidFlagCombination(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidFlagCombination(TUnderlying value) => default(TUnderlyingOperations).And(_allFlags, value).Equals(value);

        public sealed override string? FormatFlags(ref byte value, string? delimiter, ValueCollection<EnumFormat> formats) => FormatFlagsInternal(UnsafeUtility.As<byte, TUnderlying>(ref value), delimiter, formats);

        public sealed override string? FormatFlags(object value, string? delimiter, ValueCollection<EnumFormat> formats) => FormatFlagsInternal(ToObject(value), delimiter, formats);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string? FormatFlagsInternal(TUnderlying value, string? delimiter, ValueCollection<EnumFormat> formats) => FormatFlagsInternal(value, GetMember(value), delimiter, formats);

        internal string? FormatFlagsInternal(TUnderlying value, EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member, string? delimiter, ValueCollection<EnumFormat> formats)
        {
            if (member != null || value.Equals(default) || !IsValidFlagCombination(value))
            {
                return AsStringInternal(value, member, formats);
            }

            if (string.IsNullOrEmpty(delimiter))
            {
                delimiter = FlagEnums.DefaultDelimiter;
            }

            var sb = new StringBuilder();
            TUnderlyingOperations operations = default;
            var isLessThanZero = operations.LessThan(value, default);
            for (var currentValue = operations.One; isLessThanZero ? !currentValue.Equals(default) : !operations.LessThan(value, currentValue); currentValue = operations.LeftShift(currentValue, 1))
            {
                if (HasAnyFlags(value, currentValue))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(delimiter);
                    }
                    sb.Append(AsStringInternal(currentValue, null, formats));
                }
            }

            return sb.ToString();
        }

        public sealed override IReadOnlyList<object> GetFlags(object value) => GetFlags(ToObject(value)).GetNonGenericContainer();

        public sealed override IValuesContainer GetFlags(ref byte value) => GetFlags(UnsafeUtility.As<byte, TUnderlying>(ref value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IValuesContainer GetFlags(TUnderlying value) => EnumBridge.CreateValuesContainer(EnumerateFlagMembers(value), GetFlagCount(value), false);

        public sealed override IReadOnlyList<EnumMember> GetFlagMembers(ref byte value) => GetFlagMembers(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override IReadOnlyList<EnumMember> GetFlagMembers(object value) => GetFlagMembers(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<EnumMember> GetFlagMembers(TUnderlying value) => EnumBridge.CreateMembersContainer(EnumerateFlagMembers(value), GetFlagCount(value), false);

        private IEnumerable<EnumMemberInternal<TUnderlying, TUnderlyingOperations>> EnumerateFlagMembers(TUnderlying value)
        {
            TUnderlyingOperations operations = default;
            var validValue = operations.And(value, _allFlags);
            var isLessThanZero = operations.LessThan(validValue, default);
            for (var currentValue = operations.One; isLessThanZero ? !currentValue.Equals(default) : !operations.LessThan(value, currentValue); currentValue = operations.LeftShift(currentValue, 1))
            {
                if (HasAnyFlags(validValue, currentValue))
                {
                    yield return GetMember(currentValue)!;
                }
            }
        }

        public sealed override int GetFlagCount() => default(TUnderlyingOperations).BitCount(_allFlags);

        public sealed override int GetFlagCount(ref byte value) => GetFlagCount(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override int GetFlagCount(object value) => GetFlagCount(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetFlagCount(TUnderlying value)
        {
            TUnderlyingOperations operations = default;
            return operations.BitCount(operations.And(value, _allFlags));
        }

        public sealed override int GetFlagCount(ref byte value, ref byte otherFlags) => GetFlagCount(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));

        public sealed override int GetFlagCount(object value, object otherFlags) => GetFlagCount(ToObject(value), ToObject(otherFlags));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetFlagCount(TUnderlying value, TUnderlying otherFlags)
        {
            TUnderlyingOperations operations = default;
            return operations.BitCount(operations.And(operations.And(value, otherFlags), _allFlags));
        }

        public sealed override bool HasAnyFlags(ref byte value) => HasAnyFlags(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override bool HasAnyFlags(object value) => HasAnyFlags(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyFlags(TUnderlying value) => !value.Equals(default);

        public sealed override bool HasAnyFlags(ref byte value, ref byte otherFlags) => HasAnyFlags(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));

        public sealed override bool HasAnyFlags(object value, object otherFlags) => HasAnyFlags(ToObject(value), ToObject(otherFlags));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyFlags(TUnderlying value, TUnderlying otherFlags) => !default(TUnderlyingOperations).And(value, otherFlags).Equals(default);

        public sealed override bool HasAllFlags(ref byte value) => HasAllFlags(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override bool HasAllFlags(object value) => HasAllFlags(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAllFlags(TUnderlying value) => HasAllFlags(value, _allFlags);

        public sealed override bool HasAllFlags(ref byte value, ref byte otherFlags) => HasAllFlags(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));

        public sealed override bool HasAllFlags(object value, object otherFlags) => HasAllFlags(ToObject(value), ToObject(otherFlags));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAllFlags(TUnderlying value, TUnderlying otherFlags) => default(TUnderlyingOperations).And(value, otherFlags).Equals(otherFlags);

        public sealed override void ToggleFlags(ref byte value, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ToggleFlags(UnsafeUtility.As<byte, TUnderlying>(ref value));
        }

        public sealed override object ToggleFlags(object value) => EnumBridge.ToObjectUnchecked(ToggleFlags(ToObject(value)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TUnderlying ToggleFlags(TUnderlying value) => default(TUnderlyingOperations).Xor(value, _allFlags);

        public sealed override void ToggleFlags(ref byte value, ref byte otherFlags, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ToggleFlags(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));
        }

        public sealed override object ToggleFlags(object value, object otherFlags) => EnumBridge.ToObjectUnchecked(ToggleFlags(ToObject(value), ToObject(otherFlags)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TUnderlying ToggleFlags(TUnderlying value, TUnderlying otherFlags) => default(TUnderlyingOperations).Xor(value, otherFlags);

        public sealed override void CommonFlags(ref byte value, ref byte otherFlags, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = CommonFlags(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));
        }

        public sealed override object CommonFlags(object value, object otherFlags) => EnumBridge.ToObjectUnchecked(CommonFlags(ToObject(value), ToObject(otherFlags)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TUnderlying CommonFlags(TUnderlying value, TUnderlying otherFlags) => default(TUnderlyingOperations).And(value, otherFlags);

        public sealed override void CombineFlags(ref byte value, ref byte otherFlags, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = default(TUnderlyingOperations).Or(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));
        }

        public sealed override object CombineFlags(object value, object otherFlags) => EnumBridge.ToObjectUnchecked(default(TUnderlyingOperations).Or(ToObject(value), ToObject(otherFlags)));

        public sealed override void CombineFlags(ref byte flag0, ref byte flag1, ref byte flag2, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            TUnderlyingOperations operations = default;
            underlying = operations.Or(operations.Or(UnsafeUtility.As<byte, TUnderlying>(ref flag0), UnsafeUtility.As<byte, TUnderlying>(ref flag1)), UnsafeUtility.As<byte, TUnderlying>(ref flag2));
        }

        public sealed override void CombineFlags(ref byte flag0, ref byte flag1, ref byte flag2, ref byte flag3, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            TUnderlyingOperations operations = default;
            underlying = operations.Or(operations.Or(operations.Or(UnsafeUtility.As<byte, TUnderlying>(ref flag0), UnsafeUtility.As<byte, TUnderlying>(ref flag1)), UnsafeUtility.As<byte, TUnderlying>(ref flag2)), UnsafeUtility.As<byte, TUnderlying>(ref flag3));
        }

        public sealed override void CombineFlags(ref byte flag0, ref byte flag1, ref byte flag2, ref byte flag3, ref byte flag4, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            TUnderlyingOperations operations = default;
            underlying = operations.Or(operations.Or(operations.Or(operations.Or(UnsafeUtility.As<byte, TUnderlying>(ref flag0), UnsafeUtility.As<byte, TUnderlying>(ref flag1)), UnsafeUtility.As<byte, TUnderlying>(ref flag2)), UnsafeUtility.As<byte, TUnderlying>(ref flag3)), UnsafeUtility.As<byte, TUnderlying>(ref flag4));
        }

        public sealed override object CombineFlags(IEnumerable<object?>? flags, bool isNullable)
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

        public sealed override void RemoveFlags(ref byte value, ref byte otherFlags, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = RemoveFlags(UnsafeUtility.As<byte, TUnderlying>(ref value), UnsafeUtility.As<byte, TUnderlying>(ref otherFlags));
        }

        public sealed override object RemoveFlags(object value, object otherFlags) => EnumBridge.ToObjectUnchecked(RemoveFlags(ToObject(value), ToObject(otherFlags)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TUnderlying RemoveFlags(TUnderlying value, TUnderlying otherFlags)
        {
            TUnderlyingOperations operations = default;
            return operations.And(value, operations.Not(otherFlags));
        }
        #endregion

        #region Parsing
        public sealed override void ParseFlags(ParseType value, bool ignoreCase, string? delimiter, ValueCollection<EnumFormat> formats, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ParseFlagsInternal(value, ignoreCase, delimiter, formats);
        }

        public sealed override object ParseFlags(ParseType value, bool ignoreCase, string? delimiter, ValueCollection<EnumFormat> formats) => EnumBridge.ToObjectUnchecked(ParseFlagsInternal(value, ignoreCase, delimiter, formats));

        public TUnderlying ParseFlagsInternal(ParseType value, bool ignoreCase, string? delimiter, ValueCollection<EnumFormat> formats)
        {
            if (string.IsNullOrEmpty(delimiter))
            {
                delimiter = FlagEnums.DefaultDelimiter;
            }

#if SPAN
            var effectiveDelimiter = delimiter.AsSpan().Trim();
#else
            var effectiveDelimiter = delimiter!.Trim();
#endif
            if (effectiveDelimiter.Length == 0)
            {
                effectiveDelimiter = delimiter;
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
                delimiterIndex = delimiterIndex < 0 ? valueLength : delimiterIndex + startIndex;
#else
                var delimiterIndex = value.IndexOf(effectiveDelimiter, startIndex, StringComparison.Ordinal);
                if (delimiterIndex < 0)
                {
                    delimiterIndex = valueLength;
                }
#endif
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

        public sealed override bool TryParseFlags(
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

        public sealed override bool TryParseFlags(
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
#else
            var effectiveDelimiter = delimiter!.Trim();
#endif
            if (effectiveDelimiter.Length == 0)
            {
                effectiveDelimiter = delimiter;
            }

            TUnderlying resultAsUnderlying = default;
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
                delimiterIndex = delimiterIndex < 0 ? valueLength : delimiterIndex + startIndex;
#else
                var delimiterIndex = value.IndexOf(effectiveDelimiter, startIndex, StringComparison.Ordinal);
                if (delimiterIndex < 0)
                {
                    delimiterIndex = valueLength;
                }
#endif
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
                resultAsUnderlying = operations.Or(resultAsUnderlying, underlying);
                startIndex = newStartIndex;
            }
            result = resultAsUnderlying;
            return true;
        }
        #endregion
        #endregion

        internal sealed class EnumMemberParser
        {
            private readonly struct Entry
            {
                public readonly int OrdinalNext;
                public readonly int OrdinalIgnoreCaseNext;
                public readonly string FormattedValue;
                public readonly EnumMemberInternal<TUnderlying, TUnderlyingOperations> Member;

                public Entry(int ordinalNext, int ordinalIgnoreCaseNext, string formattedValue, EnumMemberInternal<TUnderlying, TUnderlyingOperations> member)
                {
                    OrdinalNext = ordinalNext;
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
                var size = enumCache._buckets.Length;
                var ordinalBuckets = new int[size];
                var ordinalIgnoreCaseBuckets = new int[size];
                var members = enumCache._members;
                var entries = new Entry[members.Length];
                for (var i = 0; i < members.Length; ++i)
                {
                    var member = members[i];
                    var formattedValue = member.AsString(format);
                    if (formattedValue != null)
                    {
                        var ordinalHashCode = formattedValue.GetHashCode();
                        ref int ordinalBucket = ref ordinalBuckets[ordinalHashCode & (size - 1)];
                        var ordinalIgnoreCaseHashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(formattedValue);
                        ref int ordinalIgnoreCaseBucket = ref ordinalIgnoreCaseBuckets[ordinalIgnoreCaseHashCode & (size - 1)];
                        entries[i] = new Entry(ordinalBucket - 1, ordinalIgnoreCaseBucket - 1, formattedValue, member);
                        ordinalBucket = i + 1;
                        ordinalIgnoreCaseBucket = i + 1;
                    }
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
#if SPAN
                        if (formattedValue.Equals(entries[i].FormattedValue, StringComparison.OrdinalIgnoreCase))
#else
                        if (string.Equals(entries[i].FormattedValue, formattedValue, StringComparison.OrdinalIgnoreCase))
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
#if SPAN
                        if (formattedValue.Equals(entries[i].FormattedValue, StringComparison.Ordinal))
#else
                        if (entries[i].FormattedValue == formattedValue)
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

    internal abstract class StandardEnumCache<TUnderlying, TUnderlyingOperations> : EnumCache<TUnderlying, TUnderlyingOperations>
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        public StandardEnumCache(Type enumType, IEnumBridge<TUnderlying, TUnderlyingOperations> enumBridge, EnumMemberInternal<TUnderlying, TUnderlyingOperations>[] members, EnumMemberInternal<TUnderlying, TUnderlyingOperations>?[] buckets, TUnderlying allFlags, int distinctCount, bool isContiguous, object? customValidator)
            : base(enumType, enumBridge, isFlagEnum: false, members, buckets, allFlags, distinctCount, isContiguous, customValidator)
        {
        }

        public sealed override void Parse(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ParseInternal(value, ignoreCase, formats);
        }

        public sealed override object Parse(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats) => EnumBridge.ToObjectUnchecked(ParseInternal(value, ignoreCase, formats));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new TUnderlying ParseInternal(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats)
        {
            value = value.Trim();

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

        public sealed override bool TryParse(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, ref byte result, ValueCollection<EnumFormat> formats)
        {
            if (value != null && TryParseInternal(value.Trim(), ignoreCase, out var r, out _, formats, true))
            {
                ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
                underlying = r;
                return true;
            }
            return false;
        }

        public sealed override bool TryParse(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, out object? result, ValueCollection<EnumFormat> formats)
        {
            if (value != null && TryParseInternal(value.Trim(), ignoreCase, out var r, out _, formats, true))
            {
                result = EnumBridge.ToObjectUnchecked(r);
                return true;
            }
            result = null;
            return false;
        }
    }

    internal sealed class ContiguousStandardEnumCache<TUnderlying, TUnderlyingOperations> : StandardEnumCache<TUnderlying, TUnderlyingOperations>
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        private readonly EnumMemberInternal<TUnderlying, TUnderlyingOperations>[] _distinctMembers;

        public ContiguousStandardEnumCache(Type enumType, IEnumBridge<TUnderlying, TUnderlyingOperations> enumBridge, EnumMemberInternal<TUnderlying, TUnderlyingOperations>[] members, EnumMemberInternal<TUnderlying, TUnderlyingOperations>?[] buckets, TUnderlying allFlags, int distinctCount, object? customValidator)
            : base(enumType, enumBridge, members, buckets, allFlags, distinctCount, isContiguous: true, customValidator)
        {
            if (distinctCount == members.Length)
            {
                _distinctMembers = members;
            }
            else
            {
                var distinctMembers = new EnumMemberInternal<TUnderlying, TUnderlyingOperations>[distinctCount];
                var previous = members[0];
                distinctMembers[0] = previous;
                var i = 1;
                for (var j = 1; j < members.Length; ++j)
                {
                    var next = members[j];
                    if (!next.Value.Equals(previous.Value))
                    {
                        distinctMembers[i++] = next;
                        previous = next;
                    }
                }
                _distinctMembers = distinctMembers;
            }
        }

        public sealed override string AsString(ref byte value) => AsString(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override string AsString(object value) => AsString(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new string AsString(TUnderlying value)
        {
            var member = GetMember(value);
            return member != null ? member.Name : value.ToString()!;
        }

        public override bool IsDefined(ref byte value) => IsDefined(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override bool IsDefined(object value) => IsDefined(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new bool IsDefined(TUnderlying value) => default(TUnderlyingOperations).InRange(value, _minDefined, _maxDefined);

        public override EnumMemberInternal? GetMember(ref byte value) => GetMember(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override EnumMemberInternal? GetMember(object value) => GetMember(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new EnumMemberInternal<TUnderlying, TUnderlyingOperations>? GetMember(TUnderlying value)
        {
            TUnderlyingOperations operations = default;
            if (operations.InRange(value, _minDefined, _maxDefined))
            {
                var index = operations.Subtract(value, _minDefined);
                return _distinctMembers
#if ICONVERTIBLE
                    [index.ToInt32(null)];
#else
                    [operations.ToInt32(index)];
#endif
            }
            return null;
        }
    }

    internal sealed class NonContiguousStandardEnumCache<TUnderlying, TUnderlyingOperations> : StandardEnumCache<TUnderlying, TUnderlyingOperations>
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        public NonContiguousStandardEnumCache(Type enumType, IEnumBridge<TUnderlying, TUnderlyingOperations> enumBridge, EnumMemberInternal<TUnderlying, TUnderlyingOperations>[] members, EnumMemberInternal<TUnderlying, TUnderlyingOperations>?[] buckets, TUnderlying allFlags, int distinctCount, object? customValidator)
            : base(enumType, enumBridge, members, buckets, allFlags, distinctCount, isContiguous: false, customValidator)
        {
        }

        public sealed override string AsString(ref byte value) => AsString(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public sealed override string AsString(object value) => AsString(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new string AsString(TUnderlying value)
        {
            var member = GetMember(value);
            return member != null ? member.Name : value.ToString()!;
        }

        public override bool IsDefined(ref byte value) => IsDefined(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override bool IsDefined(object value) => IsDefined(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new bool IsDefined(TUnderlying value) => GetMember(value) != null;
    }

    internal sealed class FlagEnumCache<TUnderlying, TUnderlyingOperations> : EnumCache<TUnderlying, TUnderlyingOperations>
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        public FlagEnumCache(Type enumType, IEnumBridge<TUnderlying, TUnderlyingOperations> enumBridge, EnumMemberInternal<TUnderlying, TUnderlyingOperations>[] members, EnumMemberInternal<TUnderlying, TUnderlyingOperations>?[] buckets, TUnderlying allFlags, int distinctCount, bool isContiguous, object? customValidator)
            : base(enumType, enumBridge, isFlagEnum: true, members, buckets, allFlags, distinctCount, isContiguous, customValidator)
        {
        }

        public override string AsString(ref byte value) => AsString(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override string AsString(object value) => AsString(ToObject(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new string AsString(TUnderlying value) => FormatFlagsInternal(value, GetMember(value), null, Enums.DefaultFormats)!;

        public override bool IsDefined(ref byte value) => IsDefined(UnsafeUtility.As<byte, TUnderlying>(ref value));

        public override bool IsDefined(object value) => IsDefined(ToObject(value));

        public override void Parse(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats, ref byte result)
        {
            ref TUnderlying underlying = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            underlying = ParseFlagsInternal(value, ignoreCase, null, formats);
        }

        public override object Parse(ParseType value, bool ignoreCase, ValueCollection<EnumFormat> formats) => EnumBridge.ToObjectUnchecked(ParseFlagsInternal(value, ignoreCase, null, formats));

        public override bool TryParse(
#if SPAN
            ReadOnlySpan<char>
#else
            string?
#endif
            value, bool ignoreCase, ref byte result, ValueCollection<EnumFormat> formats)
        {
            if (TryParseFlags(value, ignoreCase, null, out var r, formats))
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
            if (TryParseFlags(value, ignoreCase, null, out var r, formats))
            {
                result = EnumBridge.ToObjectUnchecked(r);
                return true;
            }
            result = null;
            return false;
        }
    }
}