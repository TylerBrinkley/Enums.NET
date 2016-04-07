using System;
using System.Globalization;

namespace EnumsNET.Numerics
{
    internal struct Int16NumericProvider : INumericProvider<short>
    {
        public string HexFormatString => "X4";

        public short One => 1;

        public short Zero => 0;

        public short And(short left, short right) => (short)(left & right);

        public short Create(ulong value) => (short)value;

        public short Create(long value) => (short)value;

        public bool IsInValueRange(ulong value) => value <= (ulong)short.MaxValue;

        public bool IsInValueRange(long value) => value >= short.MinValue && value <= short.MaxValue;

        public short LeftShift(short value, int amount) => (short)(value << amount);

        public bool LessThan(short left, short right) => left < right;

        public short Or(short left, short right) => (short)(left | right);

        public short Subtract(short left, short right) => (short)(left - right);

        public bool TryParse(string s, NumberStyles style, IFormatProvider provider, out short result) => short.TryParse(s, style, provider, out result);

        public short Xor(short left, short right) => (short)(left ^ right);
    }
}
