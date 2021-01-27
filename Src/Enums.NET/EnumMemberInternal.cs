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
using System.Runtime.CompilerServices;
using System.Threading;
using EnumsNET.Numerics;

namespace EnumsNET
{
    internal abstract class EnumMemberInternal : IComparable<EnumMemberInternal>
    {
        public readonly string Name;
        public readonly AttributeCollection Attributes;
        private EnumMember? _enumMember;

        public EnumMember EnumMember
        {
            get
            {
                var enumMember = _enumMember;
                return enumMember ?? Interlocked.CompareExchange(ref _enumMember, (enumMember = CreateEnumMember()), null) ?? enumMember;
            }
        }

        private protected abstract EnumMember CreateEnumMember();

        protected EnumMemberInternal(string name, AttributeCollection attributes)
        {
            Name = name;
            Attributes = attributes;
        }

        public abstract void GetValue(ref byte result);
        public abstract object GetValue();
        public abstract string AsString(string format);
        public abstract string? AsString(EnumFormat format);
        public abstract string? AsString(ValueCollection<EnumFormat> formats);
#if SPAN_PARSE
        public abstract bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format);
        public abstract bool TryFormat(Span<char> destination, out int charsWritten, ValueCollection<EnumFormat> formats);
#endif
        public abstract byte ToByte();
        public abstract short ToInt16();
        public abstract int ToInt32();
        public abstract long ToInt64();
        public abstract sbyte ToSByte();
        public abstract ushort ToUInt16();
        public abstract uint ToUInt32();
        public abstract ulong ToUInt64();
        public abstract bool IsValidFlagCombination();
        public abstract bool HasAnyFlags();
        public abstract bool HasAllFlags();
        public abstract int GetFlagCount();
        public abstract IValuesContainer GetFlags();
        public abstract IReadOnlyList<EnumMember> GetFlagMembers();
        public abstract int CompareTo(EnumMemberInternal? other);
#if ICONVERTIBLE
        public abstract TypeCode GetTypeCode();
        public abstract bool ToBoolean(IFormatProvider? provider);
        public abstract char ToChar(IFormatProvider? provider);
        public abstract float ToSingle(IFormatProvider? provider);
        public abstract double ToDouble(IFormatProvider? provider);
        public abstract decimal ToDecimal(IFormatProvider? provider);
        public abstract DateTime ToDateTime(IFormatProvider? provider);
        public abstract object ToType(Type conversionType, IFormatProvider? provider);
#endif
    }

    // Putting the logic here as opposed to directly in EnumMember<TEnum, TUnderlying, TUnderlyingOperations>
    // reduces memory usage because it doesn't have the enum type as a generic type parameter.
    internal sealed class EnumMemberInternal<TUnderlying, TUnderlyingOperations> : EnumMemberInternal, IEquatable<EnumMemberInternal<TUnderlying, TUnderlyingOperations>>
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        internal EnumCache<TUnderlying, TUnderlyingOperations> EnumCache;

        public readonly TUnderlying Value;

        // Used for lookups by value from EnumCache
        internal EnumMemberInternal<TUnderlying, TUnderlyingOperations>? Next;

        private protected override EnumMember CreateEnumMember() => EnumCache.EnumBridge.CreateEnumMember(this);

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable. Gets set soon after creation but cannot be set at creation as EnumCache is not created yet
        public EnumMemberInternal(TUnderlying value, string name, AttributeCollection attributes)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(name, attributes)
        {
            Value = value;
        }

        public override void GetValue(ref byte result)
        {
            ref var v = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            v = Value;
        }

        public override object GetValue() => Value;

        public override string AsString(string format) => EnumCache.AsStringInternal(Value, this, format);

        public override string? AsString(EnumFormat format)
        {
            var isInitialized = true;
            EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member = this;
            return EnumCache.AsStringInternal(Value, ref isInitialized, ref member, format);
        }

        public override string? AsString(ValueCollection<EnumFormat> formats) => EnumCache.AsStringInternal(Value, this, formats);

#if SPAN_PARSE
        public override bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format) => EnumCache.TryFormatInternal(Value, this, destination, out charsWritten, format);

        public override bool TryFormat(Span<char> destination, out int charsWritten, ValueCollection<EnumFormat> formats) => EnumCache.TryFormatInternal(Value, this, destination, out charsWritten, formats);
#endif

        public override int GetHashCode() => Value.GetHashCode();

        public override bool IsValidFlagCombination() => EnumCache.IsValidFlagCombination(Value);

        public override int GetFlagCount() => EnumCache.GetFlagCount(Value);

        public override bool HasAnyFlags() => EnumCache.HasAnyFlags(Value);

        public override bool HasAllFlags() => EnumCache.HasAllFlags(Value);

        public override IValuesContainer GetFlags() => EnumCache.GetFlags(Value);

        public override IReadOnlyList<EnumMember> GetFlagMembers() => EnumCache.GetFlagMembers(Value);

        public override int CompareTo(EnumMemberInternal? other) => other != null ? Value.CompareTo(UnsafeUtility.As<EnumMemberInternal<TUnderlying, TUnderlyingOperations>>(other).Value) : 1;

        // Implemented so that Distinct will work
        public bool Equals(EnumMemberInternal<TUnderlying, TUnderlyingOperations>? other) => other != null && Value.Equals(other.Value);

#if ICONVERTIBLE
        public override sbyte ToSByte() => Value.ToSByte(null);

        public override byte ToByte() => Value.ToByte(null);

        public override short ToInt16() => Value.ToInt16(null);

        public override ushort ToUInt16() => Value.ToUInt16(null);

        public override int ToInt32() => Value.ToInt32(null);

        public override uint ToUInt32() => Value.ToUInt32(null);

        public override long ToInt64() => Value.ToInt64(null);

        public override ulong ToUInt64() => Value.ToUInt64(null);

        public override TypeCode GetTypeCode() => Value.GetTypeCode();

        public override bool ToBoolean(IFormatProvider? provider) => Value.ToBoolean(provider);

        public override char ToChar(IFormatProvider? provider) => Value.ToChar(provider);

        public override float ToSingle(IFormatProvider? provider) => Value.ToSingle(provider);

        public override double ToDouble(IFormatProvider? provider) => Value.ToDouble(provider);

        public override decimal ToDecimal(IFormatProvider? provider) => Value.ToDecimal(provider);

        public override DateTime ToDateTime(IFormatProvider? provider) => Value.ToDateTime(provider);

        public override object ToType(Type conversionType, IFormatProvider? provider) => Value.ToType(conversionType, provider);
#else
        public override sbyte ToSByte() => default(TUnderlyingOperations).ToSByte(Value);

        public override byte ToByte() => default(TUnderlyingOperations).ToByte(Value);

        public override short ToInt16() => default(TUnderlyingOperations).ToInt16(Value);

        public override ushort ToUInt16() => default(TUnderlyingOperations).ToUInt16(Value);

        public override int ToInt32() => default(TUnderlyingOperations).ToInt32(Value);

        public override uint ToUInt32() => default(TUnderlyingOperations).ToUInt32(Value);

        public override long ToInt64() => default(TUnderlyingOperations).ToInt64(Value);

        public override ulong ToUInt64() => default(TUnderlyingOperations).ToUInt64(Value);
#endif
    }
}