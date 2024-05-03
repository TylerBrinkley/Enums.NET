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
using System.Globalization;

#if SPAN_PARSE
using ParseType = System.ReadOnlySpan<char>;
#else
using ParseType = System.String;
#endif

namespace EnumsNET.Numerics;

internal interface IUnderlyingOperations<T>
    where T : struct, IComparable<T>, IEquatable<T>
{
#if !NET7_0_OR_GREATER
    T One { get; }

    bool LessThan(T left, T right);
    T And(T left, T right);
    T Or(T left, T right);
    T Xor(T left, T right);
    T Not(T value);
    T LeftShift(T value, int amount);
    T Subtract(T left, T right);
#endif
    T Create(long value);
    bool IsInValueRange(long value);
    bool IsInValueRange(ulong value);
    bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out T result);
    bool TryParseNative(ParseType s, out T result);
    string ToHexadecimalString(T value);
    string ToDecimalString(T value);
#if SPAN
    bool TryFormat(T value, Span<char> destination, out int charsWritten);
    bool TryToHexadecimalString(T value, Span<char> destination, out int charsWritten);
    bool TryToDecimalString(T value, Span<char> destination, out int charsWritten);
#endif
    int BitCount(T value);
    bool InRange(T value, T minValue, T maxValue);
}

internal struct UnderlyingOperations : IUnderlyingOperations<byte>, IUnderlyingOperations<char>, IUnderlyingOperations<short>, IUnderlyingOperations<int>, IUnderlyingOperations<long>, IUnderlyingOperations<sbyte>, IUnderlyingOperations<ushort>, IUnderlyingOperations<uint>, IUnderlyingOperations<ulong>
{
#if !NET7_0_OR_GREATER
    #region One
    readonly byte IUnderlyingOperations<byte>.One => 1;

    readonly char IUnderlyingOperations<char>.One => (char)1;

    readonly short IUnderlyingOperations<short>.One => 1;

    readonly int IUnderlyingOperations<int>.One => 1;

    readonly long IUnderlyingOperations<long>.One => 1L;

    readonly sbyte IUnderlyingOperations<sbyte>.One => 1;

    readonly ushort IUnderlyingOperations<ushort>.One => 1;

    readonly uint IUnderlyingOperations<uint>.One => 1U;

    readonly ulong IUnderlyingOperations<ulong>.One => 1UL;
    #endregion

    #region And
    public readonly byte And(byte left, byte right) => (byte)(left & right);

    public readonly char And(char left, char right) => (char)(left & right);

    public readonly short And(short left, short right) => (short)(left & right);

    public readonly int And(int left, int right) => left & right;

    public readonly long And(long left, long right) => left & right;

    public readonly sbyte And(sbyte left, sbyte right) => (sbyte)(left & right);

    public readonly ushort And(ushort left, ushort right) => (ushort)(left & right);

    public readonly uint And(uint left, uint right) => left & right;

    public readonly ulong And(ulong left, ulong right) => left & right;
    #endregion

    #region Xor
    public readonly byte Xor(byte left, byte right) => (byte)(left ^ right);

    public readonly char Xor(char left, char right) => (char)(left ^ right);

    public readonly short Xor(short left, short right) => (short)(left ^ right);

    public readonly int Xor(int left, int right) => left ^ right;

    public readonly long Xor(long left, long right) => left ^ right;

    public readonly sbyte Xor(sbyte left, sbyte right) => (sbyte)(left ^ right);

    public readonly ushort Xor(ushort left, ushort right) => (ushort)(left ^ right);

    public readonly uint Xor(uint left, uint right) => left ^ right;

    public readonly ulong Xor(ulong left, ulong right) => left ^ right;
    #endregion

    #region LeftShift
    public readonly byte LeftShift(byte value, int amount) => (byte)(value << amount);

    public readonly char LeftShift(char value, int amount) => (char)(value << amount);

    public readonly short LeftShift(short value, int amount) => (short)(value << amount);

    public readonly int LeftShift(int value, int amount) => value << amount;

    public readonly long LeftShift(long value, int amount) => value << amount;

    public readonly sbyte LeftShift(sbyte value, int amount) => (sbyte)(value << amount);

    public readonly ushort LeftShift(ushort value, int amount) => (ushort)(value << amount);

    public readonly uint LeftShift(uint value, int amount) => value << amount;

    public readonly ulong LeftShift(ulong value, int amount) => value << amount;
    #endregion

    #region LessThan
    public readonly bool LessThan(byte left, byte right) => left < right;

    public readonly bool LessThan(char left, char right) => left < right;

    public readonly bool LessThan(short left, short right) => left < right;

    public readonly bool LessThan(int left, int right) => left < right;

    public readonly bool LessThan(long left, long right) => left < right;

    public readonly bool LessThan(sbyte left, sbyte right) => left < right;

    public readonly bool LessThan(ushort left, ushort right) => left < right;

    public readonly bool LessThan(uint left, uint right) => left < right;

    public readonly bool LessThan(ulong left, ulong right) => left < right;
    #endregion

    #region Not
    public readonly byte Not(byte value) => (byte)~value;

    public readonly char Not(char value) => (char)~value;

    public readonly short Not(short value) => (short)~value;

    public readonly int Not(int value) => ~value;

    public readonly long Not(long value) => ~value;

    public readonly sbyte Not(sbyte value) => (sbyte)~value;

    public readonly ushort Not(ushort value) => (ushort)~value;

    public readonly uint Not(uint value) => ~value;

    public readonly ulong Not(ulong value) => ~value;
    #endregion

    #region Or
    public readonly byte Or(byte left, byte right) => (byte)(left | right);

    public readonly char Or(char left, char right) => (char)(left | right);

    public readonly short Or(short left, short right) => (short)(left | right);

    public readonly int Or(int left, int right) => left | right;

    public readonly long Or(long left, long right) => left | right;

    public readonly sbyte Or(sbyte left, sbyte right) => (sbyte)(left | right);

    public readonly ushort Or(ushort left, ushort right) => (ushort)(left | right);

    public readonly uint Or(uint left, uint right) => left | right;

    public readonly ulong Or(ulong left, ulong right) => left | right;
    #endregion

    #region Subtract
    public readonly byte Subtract(byte left, byte right) => (byte)(left - right);

    public readonly char Subtract(char left, char right) => (char)(left - right);

    public readonly short Subtract(short left, short right) => (short)(left - right);

    public readonly int Subtract(int left, int right) => left - right;

    public readonly long Subtract(long left, long right) => left - right;

    public readonly sbyte Subtract(sbyte left, sbyte right) => (sbyte)(left - right);

    public readonly ushort Subtract(ushort left, ushort right) => (ushort)(left - right);

    public readonly uint Subtract(uint left, uint right) => left - right;

    public readonly ulong Subtract(ulong left, ulong right) => left - right;
    #endregion
#endif

    #region BitCount
    public readonly int BitCount(byte value) => Number.BitCount(value);

    public readonly int BitCount(char value) => Number.BitCount(value);

    public readonly int BitCount(short value) => Number.BitCount(value);

    public readonly int BitCount(int value) => Number.BitCount(value);

    public readonly int BitCount(long value) => Number.BitCount(value);

    public readonly int BitCount(sbyte value) => Number.BitCount(value);

    public readonly int BitCount(ushort value) => Number.BitCount(value);

    public readonly int BitCount(uint value) => Number.BitCount((int)value);

    public readonly int BitCount(ulong value) => Number.BitCount((long)value);
    #endregion

    #region Create
    readonly byte IUnderlyingOperations<byte>.Create(long value) => (byte)value;

    readonly char IUnderlyingOperations<char>.Create(long value) => (char)value;

    readonly short IUnderlyingOperations<short>.Create(long value) => (short)value;

    readonly int IUnderlyingOperations<int>.Create(long value) => (int)value;

    readonly long IUnderlyingOperations<long>.Create(long value) => value;

    readonly sbyte IUnderlyingOperations<sbyte>.Create(long value) => (sbyte)value;

    readonly ushort IUnderlyingOperations<ushort>.Create(long value) => (ushort)value;

    readonly uint IUnderlyingOperations<uint>.Create(long value) => (uint)value;

    readonly ulong IUnderlyingOperations<ulong>.Create(long value) => (ulong)value;
    #endregion

    #region InRange
    public readonly bool InRange(byte value, byte minValue, byte maxValue) => ((uint)value - minValue) <= ((uint)maxValue - minValue);

    public readonly bool InRange(char value, char minValue, char maxValue) => ((uint)value - minValue) <= ((uint)maxValue - minValue);

    public readonly bool InRange(short value, short minValue, short maxValue) => (uint)(value - minValue) <= (uint)(maxValue - minValue);

    public readonly bool InRange(int value, int minValue, int maxValue) => (uint)(value - minValue) <= (uint)(maxValue - minValue);

    public readonly bool InRange(long value, long minValue, long maxValue) => (ulong)(value - minValue) <= (ulong)(maxValue - minValue);

    public readonly bool InRange(sbyte value, sbyte minValue, sbyte maxValue) => (uint)(value - minValue) <= (uint)(maxValue - minValue);

    public readonly bool InRange(ushort value, ushort minValue, ushort maxValue) => ((uint)value - minValue) <= ((uint)maxValue - minValue);

    public readonly bool InRange(uint value, uint minValue, uint maxValue) => (value - minValue) <= (maxValue - minValue);

    public readonly bool InRange(ulong value, ulong minValue, ulong maxValue) => (value - minValue) <= (maxValue - minValue);
    #endregion

    #region IsInValueRange
    readonly bool IUnderlyingOperations<byte>.IsInValueRange(ulong value) => value <= byte.MaxValue;

    readonly bool IUnderlyingOperations<char>.IsInValueRange(ulong value) => value <= char.MaxValue;

    readonly bool IUnderlyingOperations<short>.IsInValueRange(ulong value) => value <= (ulong)short.MaxValue;

    readonly bool IUnderlyingOperations<int>.IsInValueRange(ulong value) => value <= int.MaxValue;

    readonly bool IUnderlyingOperations<long>.IsInValueRange(ulong value) => value <= long.MaxValue;

    readonly bool IUnderlyingOperations<sbyte>.IsInValueRange(ulong value) => value <= (ulong)sbyte.MaxValue;

    readonly bool IUnderlyingOperations<ushort>.IsInValueRange(ulong value) => value <= ushort.MaxValue;

    readonly bool IUnderlyingOperations<uint>.IsInValueRange(ulong value) => value <= uint.MaxValue;

    readonly bool IUnderlyingOperations<ulong>.IsInValueRange(ulong value) => true;

    readonly bool IUnderlyingOperations<byte>.IsInValueRange(long value) => value is >= byte.MinValue and <= byte.MaxValue;

    readonly bool IUnderlyingOperations<char>.IsInValueRange(long value) => value is >= 0L and <= char.MaxValue;

    readonly bool IUnderlyingOperations<short>.IsInValueRange(long value) => value is >= short.MinValue and <= short.MaxValue;

    readonly bool IUnderlyingOperations<int>.IsInValueRange(long value) => value is >= int.MinValue and <= int.MaxValue;

    readonly bool IUnderlyingOperations<long>.IsInValueRange(long value) => true;

    readonly bool IUnderlyingOperations<sbyte>.IsInValueRange(long value) => value is >= sbyte.MinValue and <= sbyte.MaxValue;

    readonly bool IUnderlyingOperations<ushort>.IsInValueRange(long value) => value is >= ushort.MinValue and <= ushort.MaxValue;

    readonly bool IUnderlyingOperations<uint>.IsInValueRange(long value) => value is >= uint.MinValue and <= uint.MaxValue;

    readonly bool IUnderlyingOperations<ulong>.IsInValueRange(long value) => value >= 0L;
    #endregion

    #region ToHexadecimalString
    public readonly string ToHexadecimalString(byte value) => value.ToString("X2");

    public readonly string ToHexadecimalString(char value) => ((ushort)value).ToString("X4");

    public readonly string ToHexadecimalString(short value) => value.ToString("X4");

    public readonly string ToHexadecimalString(int value) => value.ToString("X8");

    public readonly string ToHexadecimalString(long value) => value.ToString("X16");

    public readonly string ToHexadecimalString(sbyte value) => value.ToString("X2");

    public readonly string ToHexadecimalString(ushort value) => value.ToString("X4");

    public readonly string ToHexadecimalString(uint value) => value.ToString("X8");

    public readonly string ToHexadecimalString(ulong value) => value.ToString("X16");
    #endregion

    #region ToDecimalString
    public readonly string ToDecimalString(byte value) => value.ToString();

    public readonly string ToDecimalString(char value) => ((ushort)value).ToString();

    public readonly string ToDecimalString(short value) => value.ToString();

    public readonly string ToDecimalString(int value) => value.ToString();

    public readonly string ToDecimalString(long value) => value.ToString();

    public readonly string ToDecimalString(sbyte value) => value.ToString();

    public readonly string ToDecimalString(ushort value) => value.ToString();

    public readonly string ToDecimalString(uint value) => value.ToString();

    public readonly string ToDecimalString(ulong value) => value.ToString();
    #endregion

#if SPAN
    #region TryFormat
    public readonly bool TryFormat(byte value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryFormat(char value, Span<char> destination, out int charsWritten)
    {
        if (destination.Length >= 1)
        {
            destination[0] = value;
            charsWritten = 1;
            return true;
        }
        charsWritten = 0;
        return false;
    }

    public readonly bool TryFormat(short value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryFormat(int value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryFormat(long value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryFormat(sbyte value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryFormat(ushort value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryFormat(uint value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryFormat(ulong value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);
    #endregion

    #region TryToHexadecimalString
    public readonly bool TryToHexadecimalString(byte value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten, "X2");

    public readonly bool TryToHexadecimalString(char value, Span<char> destination, out int charsWritten) => ((ushort)value).TryFormat(destination, out charsWritten, "X4");

    public readonly bool TryToHexadecimalString(short value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten, "X4");

    public readonly bool TryToHexadecimalString(int value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten, "X8");

    public readonly bool TryToHexadecimalString(long value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten, "X16");

    public readonly bool TryToHexadecimalString(sbyte value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten, "X2");

    public readonly bool TryToHexadecimalString(ushort value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten, "X4");

    public readonly bool TryToHexadecimalString(uint value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten, "X8");

    public readonly bool TryToHexadecimalString(ulong value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten, "X16");
    #endregion

    #region TryToDecimalString
    public readonly bool TryToDecimalString(byte value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryToDecimalString(char value, Span<char> destination, out int charsWritten) => ((ushort)value).TryFormat(destination, out charsWritten);

    public readonly bool TryToDecimalString(short value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryToDecimalString(int value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryToDecimalString(long value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryToDecimalString(sbyte value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryToDecimalString(ushort value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryToDecimalString(uint value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);

    public readonly bool TryToDecimalString(ulong value, Span<char> destination, out int charsWritten) => value.TryFormat(destination, out charsWritten);
    #endregion
#endif

    #region TryParseNumber
    public readonly bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out byte result) => byte.TryParse(s, style, provider, out result);

    public readonly bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out char result)
    {
        var success = ushort.TryParse(s, style, provider, out var resultAsUShort);
        result = (char)resultAsUShort;
        return success;
    }

    public readonly bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out short result) => short.TryParse(s, style, provider, out result);

    public readonly bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out int result) => int.TryParse(s, style, provider, out result);

    public readonly bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out long result) => long.TryParse(s, style, provider, out result);

    public readonly bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out sbyte result) => sbyte.TryParse(s, style, provider, out result);

    public readonly bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out ushort result) => ushort.TryParse(s, style, provider, out result);

    public readonly bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out uint result) => uint.TryParse(s, style, provider, out result);

    public readonly bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out ulong result) => ulong.TryParse(s, style, provider, out result);
    #endregion

    #region TryParseNative
    public readonly bool TryParseNative(ParseType s, out byte result) => byte.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

    public readonly bool TryParseNative(ParseType s, out char result)
    {
#if SPAN_PARSE
        if (s.Length == 1)
        {
            result = s[0];
            return true;
        }
        result = default;
        return false;
#else
        return char.TryParse(s, out result);
#endif
    }

    public readonly bool TryParseNative(ParseType s, out short result) => short.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

    public readonly bool TryParseNative(ParseType s, out int result) => int.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

    public readonly bool TryParseNative(ParseType s, out long result) => long.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

    public readonly bool TryParseNative(ParseType s, out sbyte result) => sbyte.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

    public readonly bool TryParseNative(ParseType s, out ushort result) => ushort.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

    public readonly bool TryParseNative(ParseType s, out uint result) => uint.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

    public readonly bool TryParseNative(ParseType s, out ulong result) => ulong.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);
    #endregion
}