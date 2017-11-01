using System;
using System.Globalization;

namespace EnumsNET.Numerics
{
    internal struct BooleanNumericProvider : INumericProvider<bool>
    {
        public bool One => true;

        public bool Zero => false;

        public bool And(bool left, bool right) => left & right;

        public int BitCount(bool value) => Number.BitCount(Convert.ToByte(value));

        public bool Create(ulong value) => value != 0UL;

        public bool Create(long value) => value != 0L;

        public bool IsInValueRange(ulong value) => value <= byte.MaxValue;

        public bool IsInValueRange(long value) => value >= 0L && value <= byte.MaxValue;

        public bool LeftShift(bool value, int amount) => (!value) & amount == 1;

        public bool LessThan(bool left, bool right) => (!left) & right;

        public bool Not(bool value) => !value;

        public bool Or(bool left, bool right) => left | right;

        public bool Subtract(bool left, bool right) => left ^ right;

        public string ToHexadecimalString(bool value) => Convert.ToByte(value).ToString("X2");

        public string ToDecimalString(bool value) => Convert.ToByte(value).ToString();

        public bool TryParseNumber(string s, NumberStyles style, IFormatProvider provider, out bool result)
        {
            var success = byte.TryParse(s, style, provider, out var resultAsByte);
            result = Convert.ToBoolean(resultAsByte);
            return success;
        }

        public bool TryParseNative(string s, out bool result) => bool.TryParse(s, out result);

        public bool Xor(bool left, bool right) => left != right;

#if !ICONVERTIBLE
        public sbyte ToSByte(bool value) => Convert.ToSByte(value);

        public byte ToByte(bool value) => Convert.ToByte(value);

        public short ToInt16(bool value) => Convert.ToInt16(value);

        public ushort ToUInt16(bool value) => Convert.ToUInt16(value);

        public int ToInt32(bool value) => Convert.ToInt32(value);

        public uint ToUInt32(bool value) => Convert.ToUInt32(value);

        public long ToInt64(bool value) => Convert.ToInt64(value);

        public ulong ToUInt64(bool  value) => Convert.ToUInt64(value);
#endif
    }
}
