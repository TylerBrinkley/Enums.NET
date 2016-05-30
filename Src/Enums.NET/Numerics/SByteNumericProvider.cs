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

        public sbyte Not(sbyte value) => (sbyte)~value;

        public sbyte Or(sbyte left, sbyte right) => (sbyte)(left | right);

        public sbyte Subtract(sbyte left, sbyte right) => (sbyte)(left - right);

        public bool TryParse(string s, NumberStyles style, IFormatProvider provider, out sbyte result) => sbyte.TryParse(s, style, provider, out result);

        public sbyte Xor(sbyte left, sbyte right) => (sbyte)(left ^ right);
    }
}
