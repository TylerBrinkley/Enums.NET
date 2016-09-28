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

#if !CONCURRENT_COLLECTIONS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace System.Collections.Concurrent
{
    internal sealed class ConcurrentDictionary<TKey, TValue>// : IDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _dictionary;
        //private KeyCollection _keys;
        //private ValueCollection _values;

        //public int Count => _dictionary.Count;

        public TValue this[TKey key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                Dictionary<TKey, TValue> oldDictionary;
                var originalDictionary = _dictionary;
                do
                {
                    oldDictionary = originalDictionary;
                    var newDictionary = new Dictionary<TKey, TValue>(oldDictionary, oldDictionary.Comparer);
                    newDictionary[key] = value;
                    originalDictionary = Interlocked.CompareExchange(ref _dictionary, newDictionary, oldDictionary);
                } while (originalDictionary != oldDictionary);
            }
        }

        //public KeyCollection Keys => _keys ?? (_keys = new KeyCollection(this));

        //public ValueCollection Values => _values ?? (_values = new ValueCollection(this));

        //public IEqualityComparer<TKey> Comparer => _dictionary.Comparer;

        public ConcurrentDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        //public ConcurrentDictionary(IDictionary<TKey, TValue> dictionary)
        //{
        //    _dictionary = new Dictionary<TKey, TValue>(dictionary);
        //}

        public ConcurrentDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        //public ConcurrentDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        //{
        //    _dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        //}

        public bool TryAdd(TKey key, TValue value)
        {
            Dictionary<TKey, TValue> oldDictionary;
            var originalDictionary = _dictionary;
            do
            {
                oldDictionary = originalDictionary;
                if (oldDictionary.ContainsKey(key))
                {
                    return false;
                }
                var newDictionary = new Dictionary<TKey, TValue>(oldDictionary, oldDictionary.Comparer);
                newDictionary.Add(key, value);
                originalDictionary = Interlocked.CompareExchange(ref _dictionary, newDictionary, oldDictionary);
            } while (originalDictionary != oldDictionary);
            return true;
        }

        //public void Clear()
        //{
        //    _dictionary = new Dictionary<TKey, TValue>(_dictionary.Comparer);
        //}

        //public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        //public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

        //public bool Remove(TKey key)
        //{
        //    if (_dictionary.ContainsKey(key))
        //    {
        //        Dictionary<TKey, TValue> oldDictionary;
        //        var originalDictionary = _dictionary;
        //        do
        //        {
        //            oldDictionary = originalDictionary;
        //            var newDictionary = new Dictionary<TKey, TValue>(oldDictionary, oldDictionary.Comparer);
        //            if (!newDictionary.Remove(key))
        //            {
        //                return false;
        //            }
        //            originalDictionary = Interlocked.CompareExchange(ref _dictionary, newDictionary, oldDictionary);
        //        } while (originalDictionary != oldDictionary);
        //        return true;
        //    }
        //    return false;
        //}

        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

        #region Explicit Interface Implementation
        //bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        //ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        //ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        //void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        //{
        //    if (!TryAdd(key, value))
        //    {
        //        throw new ArgumentException("key already exists in ConcurrentDictionary");
        //    }
        //}

        //bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        //{
        //    TValue value;
        //    return _dictionary.TryGetValue(item.Key, out value) && Equals(item.Value, value) && Remove(item.Key);
        //}

        //void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        //{
        //    ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);
        //}

        //bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        //{
        //    return ((IDictionary<TKey, TValue>)_dictionary).Contains(item);
        //}

        //void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        //{
        //    ((IDictionary<TKey, TValue>)this).Add(item.Key, item.Value);
        //}

        //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region Nested Types
        //public class KeyCollection : ICollection<TKey>
        //{
        //    private ConcurrentDictionary<TKey, TValue> _dictionary;

        //    public int Count => _dictionary.Count;

        //    public KeyCollection(ConcurrentDictionary<TKey, TValue> dictionary)
        //    {
        //        _dictionary = dictionary;
        //    }

        //    public bool Contains(TKey item) => _dictionary.ContainsKey(item);

        //    public void CopyTo(TKey[] array, int arrayIndex)
        //    {
        //        _dictionary._dictionary.Keys.CopyTo(array, arrayIndex);
        //    }

        //    public IEnumerator<TKey> GetEnumerator() => _dictionary._dictionary.Keys.GetEnumerator();

        //    bool ICollection<TKey>.IsReadOnly => true;

        //    void ICollection<TKey>.Add(TKey item)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    void ICollection<TKey>.Clear()
        //    {
        //        throw new NotSupportedException();
        //    }

        //    bool ICollection<TKey>.Remove(TKey item)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        //}

        //public class ValueCollection : ICollection<TValue>
        //{
        //    private ConcurrentDictionary<TKey, TValue> _dictionary;

        //    public int Count => _dictionary.Count;

        //    public ValueCollection(ConcurrentDictionary<TKey, TValue> dictionary)
        //    {
        //        _dictionary = dictionary;
        //    }

        //    public bool Contains(TValue item) => ((ICollection<TValue>)_dictionary._dictionary.Values).Contains(item);

        //    public void CopyTo(TValue[] array, int arrayIndex)
        //    {
        //        _dictionary._dictionary.Values.CopyTo(array, arrayIndex);
        //    }

        //    public IEnumerator<TValue> GetEnumerator() => _dictionary._dictionary.Values.GetEnumerator();

        //    bool ICollection<TValue>.IsReadOnly => true;

        //    void ICollection<TValue>.Add(TValue item)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    void ICollection<TValue>.Clear()
        //    {
        //        throw new NotSupportedException();
        //    }

        //    bool ICollection<TValue>.Remove(TValue item)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        //}
        #endregion
    }
}
#endif