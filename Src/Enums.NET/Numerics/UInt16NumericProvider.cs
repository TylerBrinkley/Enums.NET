using System;
using System.Globalization;

namespace EnumsNET.Numerics
{
    internal struct UInt16NumericProvider : INumericProvider<ushort>
    {
        public string HexFormatString => "X4";

        public ushort One => 1;

        public ushort Zero => 0;

        public ushort And(ushort left, ushort right) => (ushort)(left & right);

        public ushort Create(ulong value) => (ushort)value;

        public ushort Create(long value) => (ushort)value;

        public bool IsInValueRange(ulong value) => value <= ushort.MaxValue;

        public bool IsInValueRange(long value) => value >= ushort.MinValue && value <= ushort.MaxValue;

        public ushort LeftShift(ushort value, int amount) => (ushort)(value << amount);

        public bool LessThan(ushort left, ushort right) => left < right;

        public ushort Or(ushort left, ushort right) => (ushort)(left | right);

        public ushort Subtract(ushort left, ushort right) => (ushort)(left - right);

        public bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ushort result) => ushort.TryParse(s, style, provider, out result);

        public ushort Xor(ushort left, ushort right) => (ushort)(left ^ right);
    }
}
