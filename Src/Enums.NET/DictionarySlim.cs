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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using EnumsNET.Utilities;

namespace EnumsNET
{
    internal sealed class DictionarySlim<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public readonly struct Entry
        {
            public readonly int Next;
            public readonly TKey Key;
            public readonly TValue Value;

            public Entry(int next, TKey key, TValue value)
            {
                Next = next;
                Key = key;
                Value = value;
            }
        }

        private readonly int[] _buckets;
        internal readonly Entry[] _entries;
        private ValueCollection? _values;

        public int Count { get; }

        public ValueCollection Values => _values ??= new ValueCollection(this);

        public DictionarySlim(IEnumerable<KeyValuePair<TKey, TValue>>? dictionary, int count)
        {
            Count = count;
            var size = HashHelpers.PowerOf2(count);
            var buckets = new int[size];
            var entries = new Entry[size];
            var i = 0;
            if (dictionary != null)
            {
                foreach (var pair in dictionary)
                {
                    var value = pair.Key;
                    ref int bucket = ref buckets[value!.GetHashCode() & (size - 1)];
                    entries[i] = new Entry(bucket - 1, value, pair.Value);
                    bucket = i + 1;
                    ++i;
                }
            }
            Debug.Assert(count == i);
            _buckets = buckets;
            _entries = entries;
        }

        internal bool TryGetValue(TKey key, [NotNullWhen(true)] out TValue value)
        {
            var entries = _entries;
            for (var i = _buckets[key!.GetHashCode() & (_buckets.Length - 1)] - 1; i >= 0; i = entries[i].Next)
            {
                if (EqualityComparer<TKey>.Default.Equals(key, entries[i].Key))
                {
                    value = entries[i].Value;
                    return true;
                }
            }
            value = default!;
            return false;
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly DictionarySlim<TKey, TValue> _dictionarySlim;
            private int _index;

            public KeyValuePair<TKey, TValue> Current { get; private set; }

            object? IEnumerator.Current => Current;

            public Enumerator(DictionarySlim<TKey, TValue> dictionarySlim)
            {
                _dictionarySlim = dictionarySlim;
                Current = default!;
                _index = 0;
            }

            public bool MoveNext()
            {
                if (_index >= _dictionarySlim.Count)
                {
                    return false;
                }
                var entry = _dictionarySlim._entries[_index++];
                Current = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
                return true;
            }

            public void Dispose()
            {
            }

            public void Reset()
            {
                _index = 0;
            }
        }

        public sealed class ValueCollection : IEnumerable<TValue>
        {
            private readonly DictionarySlim<TKey, TValue> _dictionarySlim;

            public ValueCollection(DictionarySlim<TKey, TValue> dictionarySlim)
            {
                _dictionarySlim = dictionarySlim;
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                var count = _dictionarySlim.Count;
                for (var i = 0; i < count; ++i)
                {
                    yield return _dictionarySlim._entries[i].Value;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}