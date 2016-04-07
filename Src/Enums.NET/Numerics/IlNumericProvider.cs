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
