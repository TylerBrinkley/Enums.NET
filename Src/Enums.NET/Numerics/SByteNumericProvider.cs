using System;
using System.Globalization;

namespace EnumsNET.Numerics
{
    internal struct SByteNumericProvider : INumericProvider<sbyte>
    {
        public string HexFormatString => "X2";

        public sbyte One => 1;

        public sbyte Zero => 0;

        public sbyte And(sbyte left, sbyte right) => (sbyte)(left & right);

        public sbyte Create(ulong value) => (sbyte)value;

        public sbyte Create(long value) => (sbyte)value;

        public bool IsInValueRange(ulong value) => value <= (ulong)sbyte.MaxValue;

        public bool IsInValueRange(long value) => value >= sbyte.MinValue && value <= sbyte.MaxValue;

        public sbyte LeftShift(sbyte value, int amount) => (sbyte)(value << amount);

        public bool LessThan(sbyte left, sbyte right) => left < right;

        public sbyte Or(sbyte left, sbyte right) => (sbyte)(left | right);

        public sbyte Subtract(sbyte left, sbyte right) => (sbyte)(left - right);

        public bool TryParse(string s, NumberStyles style, IFormatProvider provider, out sbyte result) => sbyte.TryParse(s, style, provider, out result);

        public sbyte Xor(sbyte left, sbyte right) => (sbyte)(left ^ right);
    }
}
