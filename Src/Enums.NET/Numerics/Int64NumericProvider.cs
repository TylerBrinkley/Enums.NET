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
