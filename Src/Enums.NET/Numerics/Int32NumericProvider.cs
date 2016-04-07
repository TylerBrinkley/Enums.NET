using System;
using System.Globalization;

namespace EnumsNET.Numerics
{
    internal struct Int32NumericProvider : INumericProvider<int>
    {
        public string HexFormatString => "X8";

        public int One => 1;

        public int Zero => 0;

        public int And(int left, int right) => left & right;

        public int Create(ulong value) => (int)value;

        public int Create(long value) => (int)value;

        public bool IsInValueRange(ulong value) => value <= int.MaxValue;

        public bool IsInValueRange(long value) => value >= int.MinValue && value <= int.MaxValue;

        public int LeftShift(int value, int amount) => value << amount;

        public bool LessThan(int left, int right) => left < right;

        public int Or(int left, int right) => left | right;

        public int Subtract(int left, int right) => left - right;

        public bool TryParse(string s, NumberStyles style, IFormatProvider provider, out int result) => int.TryParse(s, style, provider, out result);

        public int Xor(int left, int right) => left ^ right;
    }
}
