// Enums.NET
// Copyright 2016 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Globalization;

namespace EnumsNET
{
    internal static class IntegralOperators<T>
    {
        internal static Func<T, T, bool> Equal;

        internal static Func<T, T, bool> GreaterThan;

        internal static Func<T, T, T> And;

        internal static Func<T, T, T> Or;

        internal static Func<T, T, T> Xor;

        internal static Func<T, int, T> LeftShift;

        //internal static Func<T, int, T> RightShift;

        internal static Func<T, T, T> Add;

        internal static Func<T, T, T> Subtract;

        internal static bool IsPowerOfTwo(T x) => Equal(And(x, Subtract(x, One)), Zero);

        internal static Func<long, T> FromInt64;

        internal static Func<ulong, T> FromUInt64;

        internal static Func<long, bool> Int64IsInValueRange;

        internal static Func<ulong, bool> UInt64IsInValueRange;

        internal static Func<T, sbyte> ToSByte;

        internal static Func<T, byte> ToByte;

        internal static Func<T, short> ToInt16;

        internal static Func<T, ushort> ToUInt16;

        internal static Func<T, int> ToInt32;

        internal static Func<T, uint> ToUInt32;

        internal static Func<T, long> ToInt64;

        internal static Func<T, ulong> ToUInt64;

        internal static Func<T, string, string> ToStringFormat;

        internal static IntegralTryParseDelegate<T> TryParse;

        internal static string HexFormatString;

        internal static T Zero;

        internal static T One;

        internal static T MaxValue;

        internal static T MinValue;

        static IntegralOperators()
        {
            IntegralOperators.Populate(Type.GetTypeCode(typeof(T)));
        }
    }

    internal delegate bool IntegralTryParseDelegate<T>(string value, NumberStyles styles, IFormatProvider provider, out T result);

    internal static class IntegralOperators
    {
        internal static void Populate(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Int32:
                    IntegralOperators<int>.Equal = (x, y) => x == y;
                    IntegralOperators<int>.GreaterThan = (x, y) => x > y;
                    IntegralOperators<int>.And = (x, y) => x & y;
                    IntegralOperators<int>.Or = (x, y) => x | y;
                    IntegralOperators<int>.Xor = (x, y) => x ^ y;
                    IntegralOperators<int>.LeftShift = (x, n) => x << n;
                    //IntegralOperators<int>.RightShift = (x, n) => x >> n;
                    IntegralOperators<int>.Add = (x, y) => x + y;
                    IntegralOperators<int>.Subtract = (x, y) => x - y;
                    IntegralOperators<int>.FromInt64 = x => (int)x;
                    IntegralOperators<int>.FromUInt64 = x => (int)x;
                    IntegralOperators<int>.Int64IsInValueRange = x => x >= int.MinValue && x <= int.MaxValue;
                    IntegralOperators<int>.UInt64IsInValueRange = x => x <= int.MaxValue;
                    IntegralOperators<int>.ToSByte = Convert.ToSByte;
                    IntegralOperators<int>.ToByte = Convert.ToByte;
                    IntegralOperators<int>.ToInt16 = Convert.ToInt16;
                    IntegralOperators<int>.ToUInt16 = Convert.ToUInt16;
                    IntegralOperators<int>.ToInt32 = Convert.ToInt32;
                    IntegralOperators<int>.ToUInt32 = Convert.ToUInt32;
                    IntegralOperators<int>.ToInt64 = Convert.ToInt64;
                    IntegralOperators<int>.ToUInt64 = Convert.ToUInt64;
                    IntegralOperators<int>.ToStringFormat = (x, format) => x.ToString(format);
                    IntegralOperators<int>.TryParse = int.TryParse;
                    IntegralOperators<int>.HexFormatString = "X8";
                    IntegralOperators<int>.Zero = 0;
                    IntegralOperators<int>.One = 1;
                    break;
                case TypeCode.UInt32:
                    IntegralOperators<uint>.Equal = (x, y) => x == y;
                    IntegralOperators<uint>.GreaterThan = (x, y) => x > y;
                    IntegralOperators<uint>.And = (x, y) => x & y;
                    IntegralOperators<uint>.Or = (x, y) => x | y;
                    IntegralOperators<uint>.Xor = (x, y) => x ^ y;
                    IntegralOperators<uint>.LeftShift = (x, n) => x << n;
                    //IntegralOperators<uint>.RightShift = (x, n) => x >> n;
                    IntegralOperators<uint>.Add = (x, y) => x + y;
                    IntegralOperators<uint>.Subtract = (x, y) => x - y;
                    IntegralOperators<uint>.FromInt64 = x => (uint)x;
                    IntegralOperators<uint>.FromUInt64 = x => (uint)x;
                    IntegralOperators<uint>.Int64IsInValueRange = x => x >= uint.MinValue && x <= uint.MaxValue;
                    IntegralOperators<uint>.UInt64IsInValueRange = x => x <= uint.MaxValue;
                    IntegralOperators<uint>.ToSByte = Convert.ToSByte;
                    IntegralOperators<uint>.ToByte = Convert.ToByte;
                    IntegralOperators<uint>.ToInt16 = Convert.ToInt16;
                    IntegralOperators<uint>.ToUInt16 = Convert.ToUInt16;
                    IntegralOperators<uint>.ToInt32 = Convert.ToInt32;
                    IntegralOperators<uint>.ToUInt32 = Convert.ToUInt32;
                    IntegralOperators<uint>.ToInt64 = Convert.ToInt64;
                    IntegralOperators<uint>.ToUInt64 = Convert.ToUInt64;
                    IntegralOperators<uint>.ToStringFormat = (x, format) => x.ToString(format);
                    IntegralOperators<uint>.TryParse = uint.TryParse;
                    IntegralOperators<uint>.HexFormatString = "X8";
                    IntegralOperators<uint>.Zero = 0U;
                    IntegralOperators<uint>.One = 1U;
                    break;
                case TypeCode.Int64:
                    IntegralOperators<long>.Equal = (x, y) => x == y;
                    IntegralOperators<long>.GreaterThan = (x, y) => x > y;
                    IntegralOperators<long>.And = (x, y) => x & y;
                    IntegralOperators<long>.Or = (x, y) => x | y;
                    IntegralOperators<long>.Xor = (x, y) => x ^ y;
                    IntegralOperators<long>.LeftShift = (x, n) => x << n;
                    //IntegralOperators<long>.RightShift = (x, n) => x >> n;
                    IntegralOperators<long>.Add = (x, y) => x + y;
                    IntegralOperators<long>.Subtract = (x, y) => x - y;
                    IntegralOperators<long>.FromInt64 = x => x;
                    IntegralOperators<long>.FromUInt64 = x => (long)x;
                    IntegralOperators<long>.Int64IsInValueRange = x => true;
                    IntegralOperators<long>.UInt64IsInValueRange = x => x <= long.MaxValue;
                    IntegralOperators<long>.ToSByte = Convert.ToSByte;
                    IntegralOperators<long>.ToByte = Convert.ToByte;
                    IntegralOperators<long>.ToInt16 = Convert.ToInt16;
                    IntegralOperators<long>.ToUInt16 = Convert.ToUInt16;
                    IntegralOperators<long>.ToInt32 = Convert.ToInt32;
                    IntegralOperators<long>.ToUInt32 = Convert.ToUInt32;
                    IntegralOperators<long>.ToInt64 = Convert.ToInt64;
                    IntegralOperators<long>.ToUInt64 = Convert.ToUInt64;
                    IntegralOperators<long>.ToStringFormat = (x, format) => x.ToString(format);
                    IntegralOperators<long>.TryParse = long.TryParse;
                    IntegralOperators<long>.HexFormatString = "X16";
                    IntegralOperators<long>.Zero = 0L;
                    IntegralOperators<long>.One = 1L;
                    break;
                case TypeCode.UInt64:
                    IntegralOperators<ulong>.Equal = (x, y) => x == y;
                    IntegralOperators<ulong>.GreaterThan = (x, y) => x > y;
                    IntegralOperators<ulong>.And = (x, y) => x & y;
                    IntegralOperators<ulong>.Or = (x, y) => x | y;
                    IntegralOperators<ulong>.Xor = (x, y) => x ^ y;
                    IntegralOperators<ulong>.LeftShift = (x, n) => x << n;
                    //IntegralOperators<ulong>.RightShift = (x, n) => x >> n;
                    IntegralOperators<ulong>.Add = (x, y) => x + y;
                    IntegralOperators<ulong>.Subtract = (x, y) => x - y;
                    IntegralOperators<ulong>.FromInt64 = x => (ulong)x;
                    IntegralOperators<ulong>.FromUInt64 = x => x;
                    IntegralOperators<ulong>.Int64IsInValueRange = x => x >= 0L;
                    IntegralOperators<ulong>.UInt64IsInValueRange = x => true;
                    IntegralOperators<ulong>.ToSByte = Convert.ToSByte;
                    IntegralOperators<ulong>.ToByte = Convert.ToByte;
                    IntegralOperators<ulong>.ToInt16 = Convert.ToInt16;
                    IntegralOperators<ulong>.ToUInt16 = Convert.ToUInt16;
                    IntegralOperators<ulong>.ToInt32 = Convert.ToInt32;
                    IntegralOperators<ulong>.ToUInt32 = Convert.ToUInt32;
                    IntegralOperators<ulong>.ToInt64 = Convert.ToInt64;
                    IntegralOperators<ulong>.ToUInt64 = Convert.ToUInt64;
                    IntegralOperators<ulong>.ToStringFormat = (x, format) => x.ToString(format);
                    IntegralOperators<ulong>.TryParse = ulong.TryParse;
                    IntegralOperators<ulong>.HexFormatString = "X16";
                    IntegralOperators<ulong>.Zero = 0UL;
                    IntegralOperators<ulong>.One = 1UL;
                    break;
                case TypeCode.SByte:
                    IntegralOperators<sbyte>.Equal = (x, y) => x == y;
                    IntegralOperators<sbyte>.GreaterThan = (x, y) => x > y;
                    IntegralOperators<sbyte>.And = (x, y) => (sbyte)(x & y);
                    IntegralOperators<sbyte>.Or = (x, y) => (sbyte)(x | y);
                    IntegralOperators<sbyte>.Xor = (x, y) => (sbyte)(x ^ y);
                    IntegralOperators<sbyte>.LeftShift = (x, n) => (sbyte)(x << n);
                    //IntegralOperators<sbyte>.RightShift = (x, n) => (sbyte)(x >> n);
                    IntegralOperators<sbyte>.Add = (x, y) => (sbyte)(x + y);
                    IntegralOperators<sbyte>.Subtract = (x, y) => (sbyte)(x - y);
                    IntegralOperators<sbyte>.FromInt64 = x => (sbyte)x;
                    IntegralOperators<sbyte>.FromUInt64 = x => (sbyte)x;
                    IntegralOperators<sbyte>.Int64IsInValueRange = x => x >= sbyte.MinValue && x <= sbyte.MaxValue;
                    IntegralOperators<sbyte>.UInt64IsInValueRange = x => x <= (ulong)sbyte.MaxValue;
                    IntegralOperators<sbyte>.ToSByte = Convert.ToSByte;
                    IntegralOperators<sbyte>.ToByte = Convert.ToByte;
                    IntegralOperators<sbyte>.ToInt16 = Convert.ToInt16;
                    IntegralOperators<sbyte>.ToUInt16 = Convert.ToUInt16;
                    IntegralOperators<sbyte>.ToInt32 = Convert.ToInt32;
                    IntegralOperators<sbyte>.ToUInt32 = Convert.ToUInt32;
                    IntegralOperators<sbyte>.ToInt64 = Convert.ToInt64;
                    IntegralOperators<sbyte>.ToUInt64 = Convert.ToUInt64;
                    IntegralOperators<sbyte>.ToStringFormat = (x, format) => x.ToString(format);
                    IntegralOperators<sbyte>.TryParse = sbyte.TryParse;
                    IntegralOperators<sbyte>.HexFormatString = "X2";
                    IntegralOperators<sbyte>.Zero = 0;
                    IntegralOperators<sbyte>.One = 1;
                    break;
                case TypeCode.Byte:
                    IntegralOperators<byte>.Equal = (x, y) => x == y;
                    IntegralOperators<byte>.GreaterThan = (x, y) => x > y;
                    IntegralOperators<byte>.And = (x, y) => (byte)(x & y);
                    IntegralOperators<byte>.Or = (x, y) => (byte)(x | y);
                    IntegralOperators<byte>.Xor = (x, y) => (byte)(x ^ y);
                    IntegralOperators<byte>.LeftShift = (x, n) => (byte)(x << n);
                    //IntegralOperators<byte>.RightShift = (x, n) => (byte)(x >> n);
                    IntegralOperators<byte>.Add = (x, y) => (byte)(x + y);
                    IntegralOperators<byte>.Subtract = (x, y) => (byte)(x - y);
                    IntegralOperators<byte>.FromInt64 = x => (byte)x;
                    IntegralOperators<byte>.FromUInt64 = x => (byte)x;
                    IntegralOperators<byte>.Int64IsInValueRange = x => x >= byte.MinValue && x <= byte.MaxValue;
                    IntegralOperators<byte>.UInt64IsInValueRange = x => x <= byte.MaxValue;
                    IntegralOperators<byte>.ToSByte = Convert.ToSByte;
                    IntegralOperators<byte>.ToByte = Convert.ToByte;
                    IntegralOperators<byte>.ToInt16 = Convert.ToInt16;
                    IntegralOperators<byte>.ToUInt16 = Convert.ToUInt16;
                    IntegralOperators<byte>.ToInt32 = Convert.ToInt32;
                    IntegralOperators<byte>.ToUInt32 = Convert.ToUInt32;
                    IntegralOperators<byte>.ToInt64 = Convert.ToInt64;
                    IntegralOperators<byte>.ToUInt64 = Convert.ToUInt64;
                    IntegralOperators<byte>.ToStringFormat = (x, format) => x.ToString(format);
                    IntegralOperators<byte>.TryParse = byte.TryParse;
                    IntegralOperators<byte>.HexFormatString = "X2";
                    IntegralOperators<byte>.Zero = 0;
                    IntegralOperators<byte>.One = 1;
                    break;
                case TypeCode.Int16:
                    IntegralOperators<short>.Equal = (x, y) => x == y;
                    IntegralOperators<short>.GreaterThan = (x, y) => x > y;
                    IntegralOperators<short>.And = (x, y) => (short)(x & y);
                    IntegralOperators<short>.Or = (x, y) => (short)(x | y);
                    IntegralOperators<short>.Xor = (x, y) => (short)(x ^ y);
                    IntegralOperators<short>.LeftShift = (x, n) => (short)(x << n);
                    //IntegralOperators<short>.RightShift = (x, n) => (short)(x >> n);
                    IntegralOperators<short>.Add = (x, y) => (short)(x + y);
                    IntegralOperators<short>.Subtract = (x, y) => (short)(x - y);
                    IntegralOperators<short>.FromInt64 = x => (short)x;
                    IntegralOperators<short>.FromUInt64 = x => (short)x;
                    IntegralOperators<short>.Int64IsInValueRange = x => x >= short.MinValue && x <= short.MaxValue;
                    IntegralOperators<short>.UInt64IsInValueRange = x => x <= (ulong)short.MaxValue;
                    IntegralOperators<short>.ToSByte = Convert.ToSByte;
                    IntegralOperators<short>.ToByte = Convert.ToByte;
                    IntegralOperators<short>.ToInt16 = Convert.ToInt16;
                    IntegralOperators<short>.ToUInt16 = Convert.ToUInt16;
                    IntegralOperators<short>.ToInt32 = Convert.ToInt32;
                    IntegralOperators<short>.ToUInt32 = Convert.ToUInt32;
                    IntegralOperators<short>.ToInt64 = Convert.ToInt64;
                    IntegralOperators<short>.ToUInt64 = Convert.ToUInt64;
                    IntegralOperators<short>.ToStringFormat = (x, format) => x.ToString(format);
                    IntegralOperators<short>.TryParse = short.TryParse;
                    IntegralOperators<short>.HexFormatString = "X4";
                    IntegralOperators<short>.Zero = 0;
                    IntegralOperators<short>.One = 1;
                    break;
                case TypeCode.UInt16:
                    IntegralOperators<ushort>.Equal = (x, y) => x == y;
                    IntegralOperators<ushort>.GreaterThan = (x, y) => x > y;
                    IntegralOperators<ushort>.And = (x, y) => (ushort)(x & y);
                    IntegralOperators<ushort>.Or = (x, y) => (ushort)(x | y);
                    IntegralOperators<ushort>.Xor = (x, y) => (ushort)(x ^ y);
                    IntegralOperators<ushort>.LeftShift = (x, n) => (ushort)(x << n);
                    //IntegralOperators<ushort>.RightShift = (x, n) => (ushort)(x >> n);
                    IntegralOperators<ushort>.Add = (x, y) => (ushort)(x + y);
                    IntegralOperators<ushort>.Subtract = (x, y) => (ushort)(x - y);
                    IntegralOperators<ushort>.FromInt64 = x => (ushort)x;
                    IntegralOperators<ushort>.FromUInt64 = x => (ushort)x;
                    IntegralOperators<ushort>.Int64IsInValueRange = x => x >= ushort.MinValue && x <= ushort.MaxValue;
                    IntegralOperators<ushort>.UInt64IsInValueRange = x => x <= ushort.MaxValue;
                    IntegralOperators<ushort>.ToSByte = Convert.ToSByte;
                    IntegralOperators<ushort>.ToByte = Convert.ToByte;
                    IntegralOperators<ushort>.ToInt16 = Convert.ToInt16;
                    IntegralOperators<ushort>.ToUInt16 = Convert.ToUInt16;
                    IntegralOperators<ushort>.ToInt32 = Convert.ToInt32;
                    IntegralOperators<ushort>.ToUInt32 = Convert.ToUInt32;
                    IntegralOperators<ushort>.ToInt64 = Convert.ToInt64;
                    IntegralOperators<ushort>.ToUInt64 = Convert.ToUInt64;
                    IntegralOperators<ushort>.ToStringFormat = (x, format) => x.ToString(format);
                    IntegralOperators<ushort>.TryParse = ushort.TryParse;
                    IntegralOperators<ushort>.HexFormatString = "X4";
                    IntegralOperators<ushort>.Zero = 0;
                    IntegralOperators<ushort>.One = 1;
                    break;
            }
        }
    }
}
