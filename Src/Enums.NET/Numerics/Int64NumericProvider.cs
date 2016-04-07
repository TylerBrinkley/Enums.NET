using System;
using System.Globalization;

namespace EnumsNET.Numerics
{
    internal struct Int64NumericProvider : INumericProvider<long>
    {
        public string HexFormatString => "X16";

        public long One => 1L;

        public long Zero => 0L;

        public long And(long left, long right) => left & right;

        public long Create(ulong value) => (long)value;

        public long Create(long value) => value;

        public bool IsInValueRange(ulong value) => value <= long.MaxValue;

        public bool IsInValueRange(long value) => true;

        public long LeftShift(long value, int amount) => value << amount;

        public bool LessThan(long left, long right) => left < right;

        public long Or(long left, long right) => left | right;

        public long Subtract(long left, long right) => left - right;

        public bool TryParse(string s, NumberStyles style, IFormatProvider provider, out long result) => long.TryParse(s, style, provider, out result);

        public long Xor(long left, long right) => left ^ right;
    }
}
