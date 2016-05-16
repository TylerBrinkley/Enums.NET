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
