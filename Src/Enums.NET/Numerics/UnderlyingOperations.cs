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

#if SPAN
using ParseType = System.ReadOnlySpan<char>;
#else
using ParseType = System.String;
#endif

namespace EnumsNET.Numerics
{
    internal interface IUnderlyingOperations<T>
        where T : struct, IComparable<T>, IEquatable<T>
    {
        T One { get; }

        bool LessThan(T left, T right);
        T And(T left, T right);
        T Or(T left, T right);
        T Xor(T left, T right);
        T Not(T value);
        T LeftShift(T value, int amount);
        T Subtract(T left, T right);
        T Create(long value);
        bool IsInValueRange(long value);
        bool IsInValueRange(ulong value);
        bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out T result);
        bool TryParseNative(ParseType s, out T result);
        string ToHexadecimalString(T value);
        string ToDecimalString(T value);
        int BitCount(T value);
        bool InRange(T value, T minValue, T maxValue);

#if !ICONVERTIBLE
        sbyte ToSByte(T value);
        byte ToByte(T value);
        short ToInt16(T value);
        ushort ToUInt16(T value);
        int ToInt32(T value);
        uint ToUInt32(T value);
        long ToInt64(T value);
        ulong ToUInt64(T value);
#endif
    }

    internal struct UnderlyingOperations : IUnderlyingOperations<bool>, IUnderlyingOperations<byte>, IUnderlyingOperations<char>, IUnderlyingOperations<short>, IUnderlyingOperations<int>, IUnderlyingOperations<long>, IUnderlyingOperations<sbyte>, IUnderlyingOperations<ushort>, IUnderlyingOperations<uint>, IUnderlyingOperations<ulong>
    {
        #region One
        bool IUnderlyingOperations<bool>.One => true;

        byte IUnderlyingOperations<byte>.One => 1;

        char IUnderlyingOperations<char>.One => (char)1;

        short IUnderlyingOperations<short>.One => 1;

        int IUnderlyingOperations<int>.One => 1;

        long IUnderlyingOperations<long>.One => 1L;

        sbyte IUnderlyingOperations<sbyte>.One => 1;

        ushort IUnderlyingOperations<ushort>.One => 1;

        uint IUnderlyingOperations<uint>.One => 1U;

        ulong IUnderlyingOperations<ulong>.One => 1UL;
        #endregion

        #region And
        public bool And(bool left, bool right) => left & right;

        public byte And(byte left, byte right) => (byte)(left & right);

        public char And(char left, char right) => (char)(left & right);

        public short And(short left, short right) => (short)(left & right);

        public int And(int left, int right) => left & right;

        public long And(long left, long right) => left & right;

        public sbyte And(sbyte left, sbyte right) => (sbyte)(left & right);

        public ushort And(ushort left, ushort right) => (ushort)(left & right);

        public uint And(uint left, uint right) => left & right;

        public ulong And(ulong left, ulong right) => left & right;
        #endregion

        #region BitCount
        public int BitCount(bool value) => Number.BitCount(Convert.ToByte(value));

        public int BitCount(byte value) => Number.BitCount(value);

        public int BitCount(char value) => Number.BitCount(value);

        public int BitCount(short value) => Number.BitCount(value);

        public int BitCount(int value) => Number.BitCount(value);

        public int BitCount(long value) => Number.BitCount(value);

        public int BitCount(sbyte value) => Number.BitCount(value);

        public int BitCount(ushort value) => Number.BitCount(value);

        public int BitCount(uint value) => Number.BitCount((int)value);

        public int BitCount(ulong value) => Number.BitCount((long)value);
        #endregion

        #region Create
        bool IUnderlyingOperations<bool>.Create(long value) => value != 0L;

        byte IUnderlyingOperations<byte>.Create(long value) => (byte)value;

        char IUnderlyingOperations<char>.Create(long value) => (char)value;

        short IUnderlyingOperations<short>.Create(long value) => (short)value;

        int IUnderlyingOperations<int>.Create(long value) => (int)value;

        long IUnderlyingOperations<long>.Create(long value) => value;

        sbyte IUnderlyingOperations<sbyte>.Create(long value) => (sbyte)value;

        ushort IUnderlyingOperations<ushort>.Create(long value) => (ushort)value;

        uint IUnderlyingOperations<uint>.Create(long value) => (uint)value;

        ulong IUnderlyingOperations<ulong>.Create(long value) => (ulong)value;
        #endregion

        #region InRange
        public bool InRange(bool value, bool minValue, bool maxValue) => value == minValue || value == maxValue;

        public bool InRange(byte value, byte minValue, byte maxValue) => ((uint)value - minValue) <= ((uint)maxValue - minValue);

        public bool InRange(char value, char minValue, char maxValue) => ((uint)value - minValue) <= ((uint)maxValue - minValue);

        public bool InRange(short value, short minValue, short maxValue) => (uint)(value - minValue) <= (uint)(maxValue - minValue);

        public bool InRange(int value, int minValue, int maxValue) => (uint)(value - minValue) <= (uint)(maxValue - minValue);

        public bool InRange(long value, long minValue, long maxValue) => (ulong)(value - minValue) <= (ulong)(maxValue - minValue);

        public bool InRange(sbyte value, sbyte minValue, sbyte maxValue) => (uint)(value - minValue) <= (uint)(maxValue - minValue);

        public bool InRange(ushort value, ushort minValue, ushort maxValue) => ((uint)value - minValue) <= ((uint)maxValue - minValue);

        public bool InRange(uint value, uint minValue, uint maxValue) => (value - minValue) <= (maxValue - minValue);

        public bool InRange(ulong value, ulong minValue, ulong maxValue) => (value - minValue) <= (maxValue - minValue);
        #endregion

        #region IsInValueRange
        bool IUnderlyingOperations<bool>.IsInValueRange(ulong value) => value <= byte.MaxValue;

        bool IUnderlyingOperations<byte>.IsInValueRange(ulong value) => value <= byte.MaxValue;

        bool IUnderlyingOperations<char>.IsInValueRange(ulong value) => value <= char.MaxValue;

        bool IUnderlyingOperations<short>.IsInValueRange(ulong value) => value <= (ulong)short.MaxValue;

        bool IUnderlyingOperations<int>.IsInValueRange(ulong value) => value <= int.MaxValue;

        bool IUnderlyingOperations<long>.IsInValueRange(ulong value) => value <= long.MaxValue;

        bool IUnderlyingOperations<sbyte>.IsInValueRange(ulong value) => value <= (ulong)sbyte.MaxValue;

        bool IUnderlyingOperations<ushort>.IsInValueRange(ulong value) => value <= ushort.MaxValue;

        bool IUnderlyingOperations<uint>.IsInValueRange(ulong value) => value <= uint.MaxValue;

        bool IUnderlyingOperations<ulong>.IsInValueRange(ulong value) => true;

        bool IUnderlyingOperations<bool>.IsInValueRange(long value) => value >= 0L && value <= byte.MaxValue;

        bool IUnderlyingOperations<byte>.IsInValueRange(long value) => value >= byte.MinValue && value <= byte.MaxValue;

        bool IUnderlyingOperations<char>.IsInValueRange(long value) => value >= 0L && value <= char.MaxValue;

        bool IUnderlyingOperations<short>.IsInValueRange(long value) => value >= short.MinValue && value <= short.MaxValue;

        bool IUnderlyingOperations<int>.IsInValueRange(long value) => value >= int.MinValue && value <= int.MaxValue;

        bool IUnderlyingOperations<long>.IsInValueRange(long value) => true;

        bool IUnderlyingOperations<sbyte>.IsInValueRange(long value) => value >= sbyte.MinValue && value <= sbyte.MaxValue;

        bool IUnderlyingOperations<ushort>.IsInValueRange(long value) => value >= ushort.MinValue && value <= ushort.MaxValue;

        bool IUnderlyingOperations<uint>.IsInValueRange(long value) => value >= uint.MinValue && value <= uint.MaxValue;

        bool IUnderlyingOperations<ulong>.IsInValueRange(long value) => value >= 0L;
        #endregion

        #region LeftShift
        public bool LeftShift(bool value, int amount) => (!value) & amount == 1;

        public byte LeftShift(byte value, int amount) => (byte)(value << amount);

        public char LeftShift(char value, int amount) => (char)(value << amount);

        public short LeftShift(short value, int amount) => (short)(value << amount);

        public int LeftShift(int value, int amount) => value << amount;

        public long LeftShift(long value, int amount) => value << amount;

        public sbyte LeftShift(sbyte value, int amount) => (sbyte)(value << amount);

        public ushort LeftShift(ushort value, int amount) => (ushort)(value << amount);

        public uint LeftShift(uint value, int amount) => value << amount;

        public ulong LeftShift(ulong value, int amount) => value << amount;
        #endregion

        #region LessThan
        public bool LessThan(bool left, bool right) => (!left) & right;

        public bool LessThan(byte left, byte right) => left < right;

        public bool LessThan(char left, char right) => left < right;

        public bool LessThan(short left, short right) => left < right;

        public bool LessThan(int left, int right) => left < right;

        public bool LessThan(long left, long right) => left < right;

        public bool LessThan(sbyte left, sbyte right) => left < right;

        public bool LessThan(ushort left, ushort right) => left < right;

        public bool LessThan(uint left, uint right) => left < right;

        public bool LessThan(ulong left, ulong right) => left < right;
        #endregion

        #region Not
        public bool Not(bool value) => !value;

        public byte Not(byte value) => (byte)~value;

        public char Not(char value) => (char)(~value);

        public short Not(short value) => (short)~value;

        public int Not(int value) => ~value;

        public long Not(long value) => ~value;

        public sbyte Not(sbyte value) => (sbyte)~value;

        public ushort Not(ushort value) => (ushort)~value;

        public uint Not(uint value) => ~value;

        public ulong Not(ulong value) => ~value;
        #endregion

        #region Or
        public bool Or(bool left, bool right) => left | right;

        public byte Or(byte left, byte right) => (byte)(left | right);

        public char Or(char left, char right) => (char)(left | right);

        public short Or(short left, short right) => (short)(left | right);

        public int Or(int left, int right) => left | right;

        public long Or(long left, long right) => left | right;

        public sbyte Or(sbyte left, sbyte right) => (sbyte)(left | right);

        public ushort Or(ushort left, ushort right) => (ushort)(left | right);

        public uint Or(uint left, uint right) => left | right;

        public ulong Or(ulong left, ulong right) => left | right;
        #endregion

        #region Subtract
        public bool Subtract(bool left, bool right) => left ^ right;

        public byte Subtract(byte left, byte right) => (byte)(left - right);

        public char Subtract(char left, char right) => (char)(left - right);

        public short Subtract(short left, short right) => (short)(left - right);

        public int Subtract(int left, int right) => left - right;

        public long Subtract(long left, long right) => left - right;

        public sbyte Subtract(sbyte left, sbyte right) => (sbyte)(left - right);

        public ushort Subtract(ushort left, ushort right) => (ushort)(left - right);

        public uint Subtract(uint left, uint right) => left - right;

        public ulong Subtract(ulong left, ulong right) => left - right;
        #endregion

        #region ToHexadecimalString
        public string ToHexadecimalString(bool value) => Convert.ToByte(value).ToString("X2");

        public string ToHexadecimalString(byte value) => value.ToString("X2");

        public string ToHexadecimalString(char value) => ((ushort)value).ToString("X4");

        public string ToHexadecimalString(short value) => value.ToString("X4");

        public string ToHexadecimalString(int value) => value.ToString("X8");

        public string ToHexadecimalString(long value) => value.ToString("X16");

        public string ToHexadecimalString(sbyte value) => value.ToString("X2");

        public string ToHexadecimalString(ushort value) => value.ToString("X4");

        public string ToHexadecimalString(uint value) => value.ToString("X8");

        public string ToHexadecimalString(ulong value) => value.ToString("X16");
        #endregion

        #region ToDecimalString
        public string ToDecimalString(bool value) => Convert.ToByte(value).ToString();

        public string ToDecimalString(byte value) => value.ToString();

        public string ToDecimalString(char value) => ((ushort)value).ToString();

        public string ToDecimalString(short value) => value.ToString();

        public string ToDecimalString(int value) => value.ToString();

        public string ToDecimalString(long value) => value.ToString();

        public string ToDecimalString(sbyte value) => value.ToString();

        public string ToDecimalString(ushort value) => value.ToString();

        public string ToDecimalString(uint value) => value.ToString();

        public string ToDecimalString(ulong value) => value.ToString();
        #endregion

        #region TryParseNumber
        public bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out bool result)
        {
            var success = byte.TryParse(s, style, provider, out var resultAsByte);
            result = Convert.ToBoolean(resultAsByte);
            return success;
        }

        public bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out byte result) => byte.TryParse(s, style, provider, out result);

        public bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out char result)
        {
            var success = ushort.TryParse(s, style, provider, out var resultAsUShort);
            result = (char)resultAsUShort;
            return success;
        }

        public bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out short result) => short.TryParse(s, style, provider, out result);

        public bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out int result) => int.TryParse(s, style, provider, out result);

        public bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out long result) => long.TryParse(s, style, provider, out result);

        public bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out sbyte result) => sbyte.TryParse(s, style, provider, out result);

        public bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out ushort result) => ushort.TryParse(s, style, provider, out result);

        public bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out uint result) => uint.TryParse(s, style, provider, out result);

        public bool TryParseNumber(ParseType s, NumberStyles style, IFormatProvider provider, out ulong result) => ulong.TryParse(s, style, provider, out result);
        #endregion

        #region TryParseNative
        public bool TryParseNative(ParseType s, out bool result) => bool.TryParse(s, out result);

        public bool TryParseNative(ParseType s, out byte result) => byte.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

        public bool TryParseNative(ParseType s, out char result)
        {
#if SPAN
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

        public bool TryParseNative(ParseType s, out short result) => short.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

        public bool TryParseNative(ParseType s, out int result) => int.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

        public bool TryParseNative(ParseType s, out long result) => long.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

        public bool TryParseNative(ParseType s, out sbyte result) => sbyte.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

        public bool TryParseNative(ParseType s, out ushort result) => ushort.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

        public bool TryParseNative(ParseType s, out uint result) => uint.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

        public bool TryParseNative(ParseType s, out ulong result) => ulong.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);
        #endregion

        #region Xor
        public bool Xor(bool left, bool right) => left != right;

        public byte Xor(byte left, byte right) => (byte)(left ^ right);

        public char Xor(char left, char right) => (char)(left ^ right);

        public short Xor(short left, short right) => (short)(left ^ right);

        public int Xor(int left, int right) => left ^ right;

        public long Xor(long left, long right) => left ^ right;

        public sbyte Xor(sbyte left, sbyte right) => (sbyte)(left ^ right);

        public ushort Xor(ushort left, ushort right) => (ushort)(left ^ right);

        public uint Xor(uint left, uint right) => left ^ right;

        public ulong Xor(ulong left, ulong right) => left ^ right;
        #endregion

#if !ICONVERTIBLE
        #region ToSByte
        public sbyte ToSByte(bool value) => Convert.ToSByte(value);

        public sbyte ToSByte(byte value) => Convert.ToSByte(value);

        public sbyte ToSByte(char value) => Convert.ToSByte(value);

        public sbyte ToSByte(short value) => Convert.ToSByte(value);

        public sbyte ToSByte(int value) => Convert.ToSByte(value);

        public sbyte ToSByte(long value) => Convert.ToSByte(value);

        public sbyte ToSByte(sbyte value) => value;

        public sbyte ToSByte(ushort value) => Convert.ToSByte(value);

        public sbyte ToSByte(uint value) => Convert.ToSByte(value);

        public sbyte ToSByte(ulong value) => Convert.ToSByte(value);
        #endregion

        #region ToByte
        public byte ToByte(bool value) => Convert.ToByte(value);

        public byte ToByte(byte value) => value;

        public byte ToByte(char value) => Convert.ToByte(value);

        public byte ToByte(short value) => Convert.ToByte(value);

        public byte ToByte(int value) => Convert.ToByte(value);

        public byte ToByte(long value) => Convert.ToByte(value);

        public byte ToByte(sbyte value) => Convert.ToByte(value);

        public byte ToByte(ushort value) => Convert.ToByte(value);

        public byte ToByte(uint value) => Convert.ToByte(value);

        public byte ToByte(ulong value) => Convert.ToByte(value);
        #endregion

        #region ToInt16
        public short ToInt16(bool value) => Convert.ToInt16(value);

        public short ToInt16(byte value) => value;

        public short ToInt16(char value) => Convert.ToInt16(value);

        public short ToInt16(short value) => value;

        public short ToInt16(int value) => Convert.ToInt16(value);

        public short ToInt16(long value) => Convert.ToInt16(value);

        public short ToInt16(sbyte value) => value;

        public short ToInt16(ushort value) => Convert.ToInt16(value);

        public short ToInt16(uint value) => Convert.ToInt16(value);

        public short ToInt16(ulong value) => Convert.ToInt16(value);
        #endregion

        #region ToUInt16
        public ushort ToUInt16(bool value) => Convert.ToUInt16(value);

        public ushort ToUInt16(byte value) => value;

        public ushort ToUInt16(char value) => value;

        public ushort ToUInt16(short value) => Convert.ToUInt16(value);

        public ushort ToUInt16(int value) => Convert.ToUInt16(value);

        public ushort ToUInt16(long value) => Convert.ToUInt16(value);

        public ushort ToUInt16(sbyte value) => Convert.ToUInt16(value);

        public ushort ToUInt16(ushort value) => value;

        public ushort ToUInt16(uint value) => Convert.ToUInt16(value);

        public ushort ToUInt16(ulong value) => Convert.ToUInt16(value);
        #endregion

        #region ToInt32
        public int ToInt32(bool value) => Convert.ToInt32(value);

        public int ToInt32(byte value) => value;

        public int ToInt32(char value) => value;

        public int ToInt32(short value) => value;

        public int ToInt32(int value) => value;

        public int ToInt32(long value) => Convert.ToInt32(value);

        public int ToInt32(sbyte value) => value;

        public int ToInt32(ushort value) => value;

        public int ToInt32(uint value) => Convert.ToInt32(value);

        public int ToInt32(ulong value) => Convert.ToInt32(value);
        #endregion

        #region ToUInt32
        public uint ToUInt32(bool value) => Convert.ToUInt32(value);

        public uint ToUInt32(byte value) => value;

        public uint ToUInt32(char value) => value;

        public uint ToUInt32(short value) => Convert.ToUInt32(value);

        public uint ToUInt32(int value) => Convert.ToUInt32(value);

        public uint ToUInt32(long value) => Convert.ToUInt32(value);

        public uint ToUInt32(sbyte value) => Convert.ToUInt32(value);

        public uint ToUInt32(ushort value) => value;

        public uint ToUInt32(uint value) => value;

        public uint ToUInt32(ulong value) => Convert.ToUInt32(value);
        #endregion

        #region ToInt64
        public long ToInt64(bool value) => Convert.ToInt64(value);

        public long ToInt64(byte value) => value;

        public long ToInt64(char value) => value;

        public long ToInt64(short value) => value;

        public long ToInt64(int value) => value;

        public long ToInt64(long value) => value;

        public long ToInt64(sbyte value) => value;

        public long ToInt64(ushort value) => value;

        public long ToInt64(uint value) => value;

        public long ToInt64(ulong value) => Convert.ToInt64(value);
        #endregion

        #region ToUInt64
        public ulong ToUInt64(bool  value) => Convert.ToUInt64(value);

        public ulong ToUInt64(byte value) => value;

        public ulong ToUInt64(char value) => value;

        public ulong ToUInt64(short value) => Convert.ToUInt64(value);

        public ulong ToUInt64(int value) => Convert.ToUInt64(value);

        public ulong ToUInt64(long value) => Convert.ToUInt64(value);

        public ulong ToUInt64(sbyte value) => Convert.ToUInt64(value);

        public ulong ToUInt64(ushort value) => value;

        public ulong ToUInt64(uint value) => value;

        public ulong ToUInt64(ulong value) => value;
        #endregion
#endif
    }
}