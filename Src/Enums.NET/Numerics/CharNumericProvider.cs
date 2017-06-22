using System;
using System.Globalization;

namespace EnumsNET.Numerics
{
    internal struct CharNumericProvider : INumericProvider<char>
    {
        public char One => (char)1;

        public char Zero => (char)0;

        public char And(char left, char right) => (char)(left & right);

        public char Create(ulong value) => (char)value;

        public char Create(long value) => (char)value;

        public bool IsInValueRange(ulong value) => value <= char.MaxValue;

        public bool IsInValueRange(long value) => value >= 0L && value <= char.MaxValue;

        public char LeftShift(char value, int amount) => (char)(value << amount);

        public bool LessThan(char left, char right) => left < right;

        public char Not(char value) => (char)(~value);

        public char Or(char left, char right) => (char)(left | right);

        public char Subtract(char left, char right) => (char)(left - right);

        public string ToHexidecimalString(char value) => ((ushort)value).ToString("X4");

        public string ToDecimalString(char value) => ((ushort)value).ToString();

        public bool TryParseNumber(string s, NumberStyles style, IFormatProvider provider, out char result)
        {
            var success = ushort.TryParse(s, style, provider, out var resultAsUShort);
            result = (char)resultAsUShort;
            return success;
        }

        public bool TryParseNative(string s, out char result) => char.TryParse(s, out result);

        public char Xor(char left, char right) => (char)(left ^ right);

#if !ICONVERTIBLE
        public sbyte ToSByte(char value) => Convert.ToSByte(value);

        public byte ToByte(char value) => Convert.ToByte(value);

        public short ToInt16(char value) => Convert.ToInt16(value);

        public ushort ToUInt16(char value) => value;

        public int ToInt32(char value) => value;

        public uint ToUInt32(char value) => value;

        public long ToInt64(char value) => value;

        public ulong ToUInt64(char value) => value;
#endif
    }
}
