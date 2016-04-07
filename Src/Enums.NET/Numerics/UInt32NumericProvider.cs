using System;
using System.Globalization;

namespace EnumsNET.Numerics
{
    internal struct UInt32NumericProvider : INumericProvider<uint>
    {
        public string HexFormatString => "X8";

        public uint One => 1U;

        public uint Zero => 0U;

        public uint And(uint left, uint right) => left & right;

        public uint Create(ulong value) => (uint)value;

        public uint Create(long value) => (uint)value;

        public bool IsInValueRange(ulong value) => value <= uint.MaxValue;

        public bool IsInValueRange(long value) => value >= uint.MinValue && value <= uint.MaxValue;

        public uint LeftShift(uint value, int amount) => value << amount;

        public bool LessThan(uint left, uint right) => left < right;

        public uint Or(uint left, uint right) => left | right;

        public uint Subtract(uint left, uint right) => left - right;

        public bool TryParse(string s, NumberStyles style, IFormatProvider provider, out uint result) => uint.TryParse(s, style, provider, out result);

        public uint Xor(uint left, uint right) => left ^ right;
    }
}
