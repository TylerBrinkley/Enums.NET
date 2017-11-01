#region License
// Copyright (c) 2016 Tyler Brinkley
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Globalization;

namespace EnumsNET.Numerics
{
    internal struct UInt16NumericProvider : INumericProvider<ushort>
    {
        public string ToHexadecimalString(ushort value) => value.ToString("X4");

        public string ToDecimalString(ushort value) => value.ToString();

        public ushort One => 1;

        public ushort Zero => 0;

        public ushort And(ushort left, ushort right) => (ushort)(left & right);

        public int BitCount(ushort value) => Number.BitCount(value);

        public ushort Create(ulong value) => (ushort)value;

        public ushort Create(long value) => (ushort)value;

        public bool IsInValueRange(ulong value) => value <= ushort.MaxValue;

        public bool IsInValueRange(long value) => value >= ushort.MinValue && value <= ushort.MaxValue;

        public ushort LeftShift(ushort value, int amount) => (ushort)(value << amount);

        public bool LessThan(ushort left, ushort right) => left < right;

        public ushort Not(ushort value) => (ushort)~value;

        public ushort Or(ushort left, ushort right) => (ushort)(left | right);

        public ushort Subtract(ushort left, ushort right) => (ushort)(left - right);

        public bool TryParseNumber(string s, NumberStyles style, IFormatProvider provider, out ushort result) => ushort.TryParse(s, style, provider, out result);

        public bool TryParseNative(string s, out ushort result) => ushort.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

        public ushort Xor(ushort left, ushort right) => (ushort)(left ^ right);

#if !ICONVERTIBLE
        public sbyte ToSByte(ushort value) => Convert.ToSByte(value);

        public byte ToByte(ushort value) => Convert.ToByte(value);

        public short ToInt16(ushort value) => Convert.ToInt16(value);

        public ushort ToUInt16(ushort value) => value;

        public int ToInt32(ushort value) => value;

        public uint ToUInt32(ushort value) => value;

        public long ToInt64(ushort value) => value;

        public ulong ToUInt64(ushort value) => value;
#endif
    }
}
