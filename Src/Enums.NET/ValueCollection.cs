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

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace EnumsNET
{
    internal static class ValueCollection
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueCollection<T> Create<T>(T item1) => new ValueCollection<T>(item1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueCollection<T> Create<T>(T item1, T item2) => new ValueCollection<T>(item1, item2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueCollection<T> Create<T>(T item1, T item2, T item3) => new ValueCollection<T>(item1, item2, item3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueCollection<T> Create<T>(params T[] items) => new ValueCollection<T>(items);
    }

    // A struct that's foreach-able without allocations and supports up to 3 elements or an array
    // only supports iterating once
    internal struct ValueCollection<T>
    {
        private static readonly T[] s_size1Array = new T[1];
        private static readonly T[] s_size2Array = new T[2];
        private static readonly T[] s_size3Array = new T[3];

        private readonly T _item1;
        private readonly T _item2;
        private readonly T _item3;
        private readonly T[] _items;
        private int _index;

        public int Count => _items.Length;

        public T Current => (uint)_index >= (uint)_items.Length
            ? default!
            : _index switch
        {
            0 => _item1,
            1 => _item2,
            2 => _item3,
            _ => _items[_index]
        };

        public ValueCollection(T item1)
        {
            _item1 = item1;
            _item2 = default!;
            _item3 = default!;
            _items = s_size1Array;
            _index = 0;
        }

        public ValueCollection(T item1, T item2)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = default!;
            _items = s_size2Array;
            _index = 0;
        }

        public ValueCollection(T item1, T item2, T item3)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _items = s_size3Array;
            _index = 0;
        }

        public ValueCollection(params T[] items)
        {
            Debug.Assert(items.Length > 0);
            _item1 = items[0];
            _item2 = items.Length > 1 ? items[1] : default!;
            _item3 = items.Length > 2 ? items[2] : default!;
            _items = items;
            _index = 0;
        }

        public ValueCollection<T> GetEnumerator()
        {
            _index = -1;
            return this;
        }

        public bool MoveNext() => ++_index < _items.Length;
    }
}