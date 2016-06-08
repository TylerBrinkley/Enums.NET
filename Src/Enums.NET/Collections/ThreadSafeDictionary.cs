using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace EnumsNET.Collections
{
    internal sealed class ThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _dictionary;
        private KeyCollection _keys;
        private ValueCollection _values;

        public int Count => _dictionary.Count;

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

        public KeyCollection Keys => _keys ?? (_keys = new KeyCollection(this));

        public ValueCollection Values => _values ?? (_values = new ValueCollection(this));

        public IEqualityComparer<TKey> Comparer => _dictionary.Comparer;

        public ThreadSafeDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public ThreadSafeDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        public ThreadSafeDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        public ThreadSafeDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

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

        public void Clear()
        {
            _dictionary = new Dictionary<TKey, TValue>(_dictionary.Comparer);
        }

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

        public bool Remove(TKey key)
        {
            if (_dictionary.ContainsKey(key))
            {
                Dictionary<TKey, TValue> oldDictionary;
                var originalDictionary = _dictionary;
                do
                {
                    oldDictionary = originalDictionary;
                    var newDictionary = new Dictionary<TKey, TValue>(oldDictionary, oldDictionary.Comparer);
                    if (!newDictionary.Remove(key))
                    {
                        return false;
                    }
                    originalDictionary = Interlocked.CompareExchange(ref _dictionary, newDictionary, oldDictionary);
                } while (originalDictionary != oldDictionary);
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

        #region Explicit Interface Implementation
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            if (!TryAdd(key, value))
            {
                throw new ArgumentException("key already exists in ThreadSafeDictionary");
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            return _dictionary.TryGetValue(item.Key, out value) && Equals(item.Value, value) && Remove(item.Key);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_dictionary).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            ((IDictionary<TKey, TValue>)this).Add(item.Key, item.Value);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region Nested Types
        public class KeyCollection : ICollection<TKey>
        {
            private ThreadSafeDictionary<TKey, TValue> _dictionary;

            public int Count => _dictionary.Count;

            public KeyCollection(ThreadSafeDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            public bool Contains(TKey item) => _dictionary.ContainsKey(item);

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                _dictionary._dictionary.Keys.CopyTo(array, arrayIndex);
            }

            public IEnumerator<TKey> GetEnumerator() => _dictionary._dictionary.Keys.GetEnumerator();

            bool ICollection<TKey>.IsReadOnly => true;

            void ICollection<TKey>.Add(TKey item)
            {
                throw new NotSupportedException();
            }

            void ICollection<TKey>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<TKey>.Remove(TKey item)
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class ValueCollection : ICollection<TValue>
        {
            private ThreadSafeDictionary<TKey, TValue> _dictionary;

            public int Count => _dictionary.Count;

            public ValueCollection(ThreadSafeDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
            }

            public bool Contains(TValue item) => _dictionary.Values.Contains(item);

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                _dictionary._dictionary.Values.CopyTo(array, arrayIndex);
            }

            public IEnumerator<TValue> GetEnumerator() => _dictionary._dictionary.Values.GetEnumerator();

            bool ICollection<TValue>.IsReadOnly => true;

            void ICollection<TValue>.Add(TValue item)
            {
                throw new NotSupportedException();
            }

            void ICollection<TValue>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<TValue>.Remove(TValue item)
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion
    }
}
