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
