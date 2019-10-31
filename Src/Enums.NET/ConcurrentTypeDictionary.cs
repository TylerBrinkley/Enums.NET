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
using System.Runtime.CompilerServices;

namespace EnumsNET
{
    internal sealed class ConcurrentTypeDictionary<T>
    {
        private sealed class Entry
        {
            public readonly Type Key;
            public readonly T Value;
            public Entry? Next;

            public Entry(Type key, T value, Entry? next)
            {
                Key = key;
                Value = value;
                Next = next;
            }
        }

        private Entry?[] _buckets = new Entry?[4];
        private Entry?[] _entries = new Entry?[4];
        private int _count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetOrAdd(Type key, Func<Type, T> valueFactory)
        {
            var buckets = _buckets;
            var entry = buckets[key.GetHashCode() & (buckets.Length - 1)];
            while (entry != null)
            {
                if (entry.Key.Equals(key))
                {
                    return entry.Value;
                }
                entry = entry.Next;
            }
            return TryAdd(key, valueFactory);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private T TryAdd(Type key, Func<Type, T> valueFactory)
        {
            lock (this)
            {
                var buckets = _buckets;
                ref var next = ref buckets[key.GetHashCode() & (buckets.Length - 1)];
                var n = next;
                while (n != null)
                {
                    if (n.Key.Equals(key))
                    {
                        return n.Value;
                    }
                    n = n.Next;
                }
                var value = valueFactory(key);
                var entries = _entries;
                var count = _count;
                if (entries.Length == count)
                {
                    var newSize = count << 1;
                    var newEntries = new Entry?[newSize];
                    entries.CopyTo(newEntries, 0);
                    entries = newEntries;
                    buckets = new Entry?[newSize];
                    for (var i = 0; i < count; ++i)
                    {
                        ref var e = ref entries[i]!;
                        next = ref buckets[e.Key.GetHashCode() & (newSize - 1)];
                        e.Next = next;
                        next = e;
                    }
                    _entries = entries;
                    _buckets = buckets;
                    next = ref buckets[key.GetHashCode() & (buckets.Length - 1)];
                }
                var entry = new Entry(key, value, next);
                entries[count] = entry;
                next = entry;
                ++_count;
                return value;
            }
        }
    }
}