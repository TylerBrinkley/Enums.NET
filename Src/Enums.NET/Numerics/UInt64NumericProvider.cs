using System;
using System.Globalization;

namespace EnumsNET.Numerics
{
    internal struct UInt64NumericProvider : INumericProvider<ulong>
    {
        public string HexFormatString => "X16";

        public ulong One => 1UL;

        public ulong Zero => 0UL;

        public ulong And(ulong left, ulong right) => left & right;

        public ulong Create(ulong value) => value;

        public ulong Create(long value) => (ulong)value;

        public bool IsInValueRange(ulong value) => true;

        public bool IsInValueRange(long value) => value >= 0L;

        public ulong LeftShift(ulong value, int amount) => value << amount;

        public bool LessThan(ulong left, ulong right) => left < right;

        public ulong Or(ulong left, ulong right) => left | right;

        public ulong Subtract(ulong left, ulong right) => left - right;

        public bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ulong result) => ulong.TryParse(s, style, provider, out result);

        public ulong Xor(ulong left, ulong right) => left ^ right;
    }
}
