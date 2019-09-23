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
using System.Threading;
using EnumsNET.Numerics;
using UnsafeUtility = System.Runtime.CompilerServices.Unsafe;

namespace EnumsNET
{
    internal abstract class EnumMemberInternal : IFormattable, IComparable<EnumMemberInternal>
#if ICONVERTIBLE
        , IConvertible
#endif
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

        internal abstract EnumMember CreateEnumMember();

        protected EnumMemberInternal(string name, AttributeCollection attributes)
        {
            Name = name;
            Attributes = attributes;
        }

        public abstract void GetValue(ref byte result);
        public abstract object GetUnderlyingValue();
        public abstract string AsString(string? format);
        public abstract string? AsString(EnumFormat format);
        public abstract string? AsString(ValueCollection<EnumFormat> formats);
        public abstract string Format(string format);
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
        public abstract object GetFlags();
        public abstract IReadOnlyList<EnumMember> GetFlagMembers();
        public abstract string ToString(string? format, IFormatProvider? formatProvider);
        public abstract int CompareTo(EnumMemberInternal? other);
#if ICONVERTIBLE
        public abstract TypeCode GetTypeCode();
        public abstract bool ToBoolean(IFormatProvider? provider);
        public abstract char ToChar(IFormatProvider? provider);
        public abstract sbyte ToSByte(IFormatProvider? provider);
        public abstract byte ToByte(IFormatProvider? provider);
        public abstract short ToInt16(IFormatProvider? provider);
        public abstract ushort ToUInt16(IFormatProvider? provider);
        public abstract int ToInt32(IFormatProvider? provider);
        public abstract uint ToUInt32(IFormatProvider? provider);
        public abstract long ToInt64(IFormatProvider? provider);
        public abstract ulong ToUInt64(IFormatProvider? provider);
        public abstract float ToSingle(IFormatProvider? provider);
        public abstract double ToDouble(IFormatProvider? provider);
        public abstract decimal ToDecimal(IFormatProvider? provider);
        public abstract DateTime ToDateTime(IFormatProvider? provider);
        public abstract string ToString(IFormatProvider? provider);
        public abstract object ToType(Type conversionType, IFormatProvider? provider);
#endif
    }

    // Putting the logic here as opposed to directly in EnumMember<TEnum, TUnderlying, TUnderlyingOperations>
    // reduces memory usage because it doesn't have the enum type as a generic type parameter.
    internal sealed class EnumMemberInternal<TUnderlying, TUnderlyingOperations> : EnumMemberInternal
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        private readonly EnumCache<TUnderlying, TUnderlyingOperations> _enumCache;

        public readonly TUnderlying Value;

        internal override EnumMember CreateEnumMember() => _enumCache.EnumBridge.CreateEnumMember(this);

        public EnumMemberInternal(TUnderlying value, string name, AttributeCollection attributes, EnumCache<TUnderlying, TUnderlyingOperations> enumCache)
            : base(name, attributes)
        {
            Value = value;
            _enumCache = enumCache;
        }

        public override void GetValue(ref byte result)
        {
            ref var v = ref UnsafeUtility.As<byte, TUnderlying>(ref result);
            v = Value;
        }

        public override object GetUnderlyingValue() => Value;

        public override string AsString(string? format) => _enumCache.AsStringInternal(Value, this, format);

        public override string? AsString(EnumFormat format)
        {
            var isInitialized = true;
            EnumMemberInternal<TUnderlying, TUnderlyingOperations>? member = this;
            return _enumCache.FormatInternal(Value, ref isInitialized, ref member, format);
        }

        public override string? AsString(ValueCollection<EnumFormat> formats) => _enumCache.FormatInternal(Value, this, formats);

        public override string Format(string format) => _enumCache.FormatInternal(Value, this, format);

        public override int GetHashCode() => Value.GetHashCode();

        public override bool IsValidFlagCombination() => _enumCache.IsValidFlagCombination(Value);

        public override int GetFlagCount() => _enumCache.GetFlagCount(Value);

        public override bool HasAnyFlags() => _enumCache.HasAnyFlags(Value);

        public override bool HasAllFlags() => _enumCache.HasAllFlags(Value);

        public override object GetFlags() => _enumCache.GetFlags(Value);

        public override IReadOnlyList<EnumMember> GetFlagMembers() => _enumCache.GetFlagMembers(Value);

        public override string ToString(string? format, IFormatProvider? formatProvider) => AsString(format);

        public override int CompareTo(EnumMemberInternal? other) => other != null ? Value.CompareTo(UnsafeUtility.As<EnumMemberInternal<TUnderlying, TUnderlyingOperations>>(other).Value) : 1;

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

        public override sbyte ToSByte(IFormatProvider? provider) => Value.ToSByte(provider);

        public override byte ToByte(IFormatProvider? provider) => Value.ToByte(provider);

        public override short ToInt16(IFormatProvider? provider) => Value.ToInt16(provider);

        public override ushort ToUInt16(IFormatProvider? provider) => Value.ToUInt16(provider);

        public override int ToInt32(IFormatProvider? provider) => Value.ToInt32(provider);

        public override uint ToUInt32(IFormatProvider? provider) => Value.ToUInt32(provider);

        public override long ToInt64(IFormatProvider? provider) => Value.ToInt64(provider);

        public override ulong ToUInt64(IFormatProvider? provider) => Value.ToUInt64(provider);

        public override float ToSingle(IFormatProvider? provider) => Value.ToSingle(provider);

        public override double ToDouble(IFormatProvider? provider) => Value.ToDouble(provider);

        public override decimal ToDecimal(IFormatProvider? provider) => Value.ToDecimal(provider);

        public override DateTime ToDateTime(IFormatProvider? provider) => Value.ToDateTime(provider);

        public override string ToString(IFormatProvider? provider) => Name;

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
