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
    internal interface INumericProvider<TInt>
        where TInt : struct, IFormattable, IConvertible, IComparable<TInt>, IEquatable<TInt>
    {
        bool LessThan(TInt left, TInt right);

        TInt And(TInt left, TInt right);

        TInt Or(TInt left, TInt right);

        TInt Xor(TInt left, TInt right);

        TInt Not(TInt value);

        TInt LeftShift(TInt value, int amount);

        TInt Subtract(TInt left, TInt right);

        TInt Create(long value);

        TInt Create(ulong value);

        bool IsInValueRange(long value);

        bool IsInValueRange(ulong value);

        bool TryParse(string s, NumberStyles style, IFormatProvider provider, out TInt result);

        string HexFormatString { get; }

        TInt Zero { get; }

        TInt One { get; }
    }
}
