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
    internal struct Int32NumericProvider : INumericProvider<int>
    {
        public string ToHexadecimalString(int value) => value.ToString("X8");

        public string ToDecimalString(int value) => value.ToString();

        public int One => 1;

        public int Zero => 0;

        public int And(int left, int right) => left & right;

        public int BitCount(int value) => Number.BitCount(value);

        public int Create(ulong value) => (int)value;

        public int Create(long value) => (int)value;

        public bool IsInValueRange(ulong value) => value <= int.MaxValue;

        public bool IsInValueRange(long value) => value >= int.MinValue && value <= int.MaxValue;

        public int LeftShift(int value, int amount) => value << amount;

        public bool LessThan(int left, int right) => left < right;

        public int Not(int value) => ~value;

        public int Or(int left, int right) => left | right;

        public int Subtract(int left, int right) => left - right;

        public bool TryParseNumber(string s, NumberStyles style, IFormatProvider provider, out int result) => int.TryParse(s, style, provider, out result);

        public bool TryParseNative(string s, out int result) => int.TryParse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result);

        public int Xor(int left, int right) => left ^ right;

#if !ICONVERTIBLE
        public sbyte ToSByte(int value) => Convert.ToSByte(value);

        public byte ToByte(int value) => Convert.ToByte(value);

        public short ToInt16(int value) => Convert.ToInt16(value);

        public ushort ToUInt16(int value) => Convert.ToUInt16(value);

        public int ToInt32(int value) => value;

        public uint ToUInt32(int value) => Convert.ToUInt32(value);

        public long ToInt64(int value) => value;

        public ulong ToUInt64(int value) => Convert.ToUInt64(value);
#endif
    }
}
