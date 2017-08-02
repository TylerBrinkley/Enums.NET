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
    internal struct UInt64NumericProvider : INumericProvider<ulong>
    {
        public string ToHexidecimalString(ulong value) => value.ToString("X16");

        public string ToDecimalString(ulong value) => value.ToString();

        public ulong One => 1UL;

        public ulong Zero => 0UL;

        public ulong And(ulong left, ulong right) => left & right;

        public int BitCount(ulong value) => Number.BitCount((long)value);

        public ulong Create(ulong value) => value;

        public ulong Create(long value) => (ulong)value;

        public bool IsInValueRange(ulong value) => true;

        public bool IsInValueRange(long value) => value >= 0L;

        public ulong LeftShift(ulong value, int amount) => value << amount;

        public bool LessThan(ulong left, ulong right) => left < right;

        public ulong Not(ulong value) => ~value;

        public ulong Or(ulong left, ulong right) => left | right;

        public ulong Subtract(ulong left, ulong right) => left - right;

        public bool TryParseNumber(string s, NumberStyles style, IFormatProvider provider, out ulong result) => ulong.TryParse(s, style, provider, out result);

        public bool TryParseNative(string s, out ulong result) => ulong.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

        public ulong Xor(ulong left, ulong right) => left ^ right;

#if !ICONVERTIBLE
        public sbyte ToSByte(ulong value) => Convert.ToSByte(value);

        public byte ToByte(ulong value) => Convert.ToByte(value);

        public short ToInt16(ulong value) => Convert.ToInt16(value);

        public ushort ToUInt16(ulong value) => Convert.ToUInt16(value);

        public int ToInt32(ulong value) => Convert.ToInt32(value);

        public uint ToUInt32(ulong value) => Convert.ToUInt32(value);

        public long ToInt64(ulong value) => Convert.ToInt64(value);

        public ulong ToUInt64(ulong value) => value;
#endif
    }
}
