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
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace EnumsNET.Utilities
{
    internal static class ArrayHelper
    {
#if !ARRAY_EMPTY
        private static class Cache<T>
        {
            public static readonly T[] Empty = new T[0];
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] Empty<T>() =>
#if ARRAY_EMPTY
            Array.Empty<T>();
#else
            Cache<T>.Empty;
#endif

        public static T[] ToArray<T>(IEnumerable<T> items, int count)
        {
            var a = new T[count];
            var i = 0;
            foreach (var item in items)
            {
                a[i++] = item;
            }
            Debug.Assert(i == count);
            return a;
        }
    }
}