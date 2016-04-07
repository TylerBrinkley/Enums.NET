using System;
using System.Globalization;

namespace EnumsNET.Numerics
{
    internal struct ByteNumericProvider : INumericProvider<byte>
    {
        public string HexFormatString => "X2";

        public byte One => 1;

        public byte Zero => 0;

        public byte And(byte left, byte right) => (byte)(left & right);

        public byte Create(ulong value) => (byte)value;

        public byte Create(long value) => (byte)value;

        public bool IsInValueRange(ulong value) => value <= byte.MaxValue;

        public bool IsInValueRange(long value) => value >= byte.MinValue && value <= byte.MaxValue;

        public byte LeftShift(byte value, int amount) => (byte)(value << amount);

        public bool LessThan(byte left, byte right) => left < right;

        public byte Or(byte left, byte right) => (byte)(left | right);

        public byte Subtract(byte left, byte right) => (byte)(left - right);

        public bool TryParse(string s, NumberStyles style, IFormatProvider provider, out byte result) => byte.TryParse(s, style, provider, out result);

        public byte Xor(byte left, byte right) => (byte)(left ^ right);
    }
}
