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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace EnumsNET.Collections
{
    //[DebuggerTypeProxy(typeof(OrderedBiDirectionalDictionaryDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    internal sealed class OrderedBiDirectionalDictionary<TFirst, TSecond> : IEnumerable<Pair<TFirst, TSecond>>//, IList<Pair<TFirst, TSecond>>
    {
        #region Static
        //private static readonly bool _cannotUseIndexIndexer = typeof(TFirst) == typeof(int) || typeof(TSecond) == typeof(int);
        //private static readonly bool _cannotUseFirstIndexer = typeof(TFirst) == typeof(int) || typeof(TSecond) == typeof(TFirst);
        //private static readonly bool _cannotUseSecondIndexer = typeof(TFirst) == typeof(TSecond) || typeof(TSecond) == typeof(int);
        #endregion

        #region Fields
        private int[] _firstBuckets;
        private int[] _secondBuckets;
        private Entry[] _entries;
        //private FirstCollection _firstItems;
        //private SecondCollection _secondItems;
        //private ForwardDictionary _forward;
        //private ReverseDictionary _reverse;
        private int _version;
        #endregion

        #region Properties
        public IEqualityComparer<TFirst> FirstComparer { get; }

        public IEqualityComparer<TSecond> SecondComparer { get; }

        public int Count { get; private set; }

        public int Capacity
        {
            get
            {
                return _entries.Length;
            }
            set
            {
                if (value < Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(Capacity), "must be greater than or equal to Count");
                }
                Resize(value, true);
            }
        }

        //public FirstCollection FirstItems => _firstItems ?? (_firstItems = new FirstCollection(this));

        //public SecondCollection SecondItems => _secondItems ?? (_secondItems = new SecondCollection(this));

        //public ForwardDictionary Forward => _forward ?? (_forward = new ForwardDictionary(this));

        //public ReverseDictionary Reverse => _reverse ?? (_reverse = new ReverseDictionary(this));

        //public TSecond this[TFirst first]
        //{
        //    get
        //    {
        //        if (_cannotUseFirstIndexer)
        //        {
        //            throw new InvalidOperationException("Use accessor methods instead due to potential indexer conflicts");
        //        }

        //        return GetSecond(first);
        //    }
        //    set
        //    {
        //        if (_cannotUseFirstIndexer)
        //        {
        //            throw new InvalidOperationException("Use accessor methods instead due to potential indexer conflicts");
        //        }
                
        //        if (!SetSecondByFirst(first, value))
        //        {
        //            throw new ArgumentException("value already found in collection", nameof(value));
        //        }
        //    }
        //}

        //public TFirst this[TSecond second]
        //{
        //    get
        //    {
        //        if (_cannotUseSecondIndexer)
        //        {
        //            throw new InvalidOperationException("Use accessor methods instead due to potential indexer conflicts");
        //        }

        //        return GetFirst(second);
        //    }
        //    set
        //    {
        //        if (_cannotUseSecondIndexer)
        //        {
        //            throw new InvalidOperationException("Use accessor methods instead due to potential indexer conflicts");
        //        }
                
        //        if (!SetFirstBySecond(second, value))
        //        {
        //            throw new ArgumentException("value already found in collection", nameof(value));
        //        }
        //    }
        //}

        //public Pair<TFirst, TSecond> this[int index]
        //{
        //    get
        //    {
        //        if (_cannotUseIndexIndexer)
        //        {
        //            throw new InvalidOperationException("Use accessor methods instead due to potential indexer conflicts");
        //        }

        //        return GetAt(index);
        //    }
        //    set
        //    {
        //        if (_cannotUseIndexIndexer)
        //        {
        //            throw new InvalidOperationException("Use accessor methods instead due to potential indexer conflicts");
        //        }

        //        if (!SetAt(index, value))
        //        {
        //            throw new ArgumentException("value already found in collection", nameof(value));
        //        }
        //    }
        //}
        #endregion

        #region Constructors
        //public OrderedBiDirectionalDictionary()
        //    : this(0, null, null)
        //{
        //}

        public OrderedBiDirectionalDictionary(int capacity)
            : this(capacity, null, null)
        {
        }

        //public OrderedBiDirectionalDictionary(IEqualityComparer<TFirst> firstComparer, IEqualityComparer<TSecond> secondComparer)
        //    : this(0, firstComparer, secondComparer)
        //{
        //}

        public OrderedBiDirectionalDictionary(int capacity, IEqualityComparer<TFirst> firstComparer, IEqualityComparer<TSecond> secondComparer)
        {
            Preconditions.GreaterThanOrEqual(capacity, nameof(capacity), 0);

            var size = HashHelper.GetPrimeGTE(capacity);
            _entries = new Entry[size];
            _firstBuckets = new int[size];
            _secondBuckets = new int[size];
            Count = 0;
            FirstComparer = firstComparer ?? EqualityComparer<TFirst>.Default;
            SecondComparer = secondComparer ?? EqualityComparer<TSecond>.Default;
        }
        #endregion

        #region Nested Types
        private struct Entry
        {
            public TFirst First;
            public TSecond Second;
            public int FirstHashCode;
            public int SecondHashCode;
            public int FirstBucketNext;
            public int SecondBucketNext;

            public Entry(TFirst first, int firstHashCode, int firstBucketNext, TSecond second, int secondHashCode, int secondBucketNext)
                : this()
            {
                First = first;
                FirstHashCode = firstHashCode;
                FirstBucketNext = firstBucketNext;
                Second = second;
                SecondHashCode = secondHashCode;
                SecondBucketNext = secondBucketNext;
            }
        }

        //public sealed class FirstCollection : IList<TFirst>
        //{
        //    private readonly OrderedBiDirectionalDictionary<TFirst, TSecond> _map;

        //    public int Count => _map.Count;

        //    public TFirst this[int index] => _map.GetFirstAt(index);

        //    public FirstCollection(OrderedBiDirectionalDictionary<TFirst, TSecond> map)
        //    {
        //        Preconditions.NotNull(map, nameof(map));

        //        _map = map;
        //    }

        //    public bool Contains(TFirst item) => _map.ContainsFirst(item);

        //    public void CopyTo(TFirst[] array, int arrayIndex)
        //    {
        //        if (arrayIndex < 0 || arrayIndex >= array.Length)
        //        {
        //            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "arrayIndex must be greater than or equal to 0 and less than array.Length");
        //        }
        //        if (arrayIndex + Count > array.Length)
        //        {
        //            throw new ArgumentOutOfRangeException(nameof(array), "array.Length - arrayIndex must be greater than or equal to Count");
        //        }

        //        var entries = _map._entries;
        //        var count = _map.Count;
        //        for (var i = 0; i < count; ++i)
        //        {
        //            array[arrayIndex++] = entries[i].First;
        //        }
        //    }

        //    public int IndexOf(TFirst item) => _map.IndexOfFirst(item);

        //    public IEnumerator<TFirst> GetEnumerator()
        //    {
        //        var version = _map._version;
        //        var entries = _map._entries;
        //        var count = _map.Count;
        //        for (int i = 0; i < count; ++i)
        //        {
        //            if (version != _map._version)
        //            {
        //                throw new InvalidOperationException("Collection was modified; enumeration operation may not execute");
        //            }
        //            yield return entries[i].First;
        //        }
        //    }

        //    #region Explicit Interface Implementation
        //    bool ICollection<TFirst>.IsReadOnly => true;

        //    TFirst IList<TFirst>.this[int index]
        //    {
        //        get
        //        {
        //            return this[index];
        //        }
        //        set
        //        {
        //            throw new NotSupportedException();
        //        }
        //    }

        //    void ICollection<TFirst>.Add(TFirst item)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    void ICollection<TFirst>.Clear()
        //    {
        //        throw new NotSupportedException();
        //    }

        //    bool ICollection<TFirst>.Remove(TFirst item)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //    void IList<TFirst>.Insert(int index, TFirst item)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    void IList<TFirst>.RemoveAt(int index)
        //    {
        //        throw new NotSupportedException();
        //    }
        //    #endregion
        //}

        //public sealed class SecondCollection : IList<TSecond>
        //{
        //    private readonly OrderedBiDirectionalDictionary<TFirst, TSecond> _map;

        //    public int Count => _map.Count;

        //    public TSecond this[int index] => _map.GetSecondAt(index);

        //    public SecondCollection(OrderedBiDirectionalDictionary<TFirst, TSecond> map)
        //    {
        //        Preconditions.NotNull(map, nameof(map));

        //        _map = map;
        //    }

        //    public bool Contains(TSecond item) => _map.ContainsSecond(item);

        //    public void CopyTo(TSecond[] array, int arrayIndex)
        //    {
        //        if (arrayIndex < 0 || arrayIndex >= array.Length)
        //        {
        //            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "arrayIndex must be greater than or equal to 0 and less than array.Length");
        //        }
        //        if (arrayIndex + Count > array.Length)
        //        {
        //            throw new ArgumentOutOfRangeException(nameof(array), "array.Length - arrayIndex must be greater than or equal to Count");
        //        }

        //        var entries = _map._entries;
        //        var count = _map.Count;
        //        for (int i = 0; i < count; ++i)
        //        {
        //            array[arrayIndex++] = entries[i].Second;
        //        }
        //    }

        //    public int IndexOf(TSecond item) => _map.IndexOfSecond(item);

        //    public IEnumerator<TSecond> GetEnumerator()
        //    {
        //        var version = _map._version;
        //        var entries = _map._entries;
        //        var count = _map.Count;
        //        for (int i = 0; i < count; ++i)
        //        {
        //            if (version != _map._version)
        //            {
        //                throw new InvalidOperationException("Collection was modified; enumeration operation may not execute");
        //            }
        //            yield return entries[i].Second;
        //        }
        //    }

        //    #region Explicit Interface Implementation
        //    bool ICollection<TSecond>.IsReadOnly => true;

        //    TSecond IList<TSecond>.this[int index]
        //    {
        //        get
        //        {
        //            return this[index];
        //        }
        //        set
        //        {
        //            throw new NotSupportedException();
        //        }
        //    }

        //    void ICollection<TSecond>.Add(TSecond item)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    void ICollection<TSecond>.Clear()
        //    {
        //        throw new NotSupportedException();
        //    }

        //    bool ICollection<TSecond>.Remove(TSecond item)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //    void IList<TSecond>.Insert(int index, TSecond item)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    void IList<TSecond>.RemoveAt(int index)
        //    {
        //        throw new NotSupportedException();
        //    }
        //    #endregion
        //}

        //public sealed class ForwardDictionary : IDictionary<TFirst, TSecond>
        //{
        //    private readonly OrderedBiDirectionalDictionary<TFirst, TSecond> _map;

        //    public TSecond this[TFirst key] => _map.GetSecond(key);

        //    public int Count => _map.Count;

        //    public ICollection<TFirst> Keys => _map.FirstItems;

        //    public ICollection<TSecond> Values => _map.SecondItems;

        //    public ForwardDictionary(OrderedBiDirectionalDictionary<TFirst, TSecond> map)
        //    {
        //        Preconditions.NotNull(map, nameof(map));

        //        _map = map;
        //    }

        //    public void Clear()
        //    {
        //        _map.Clear();
        //    }

        //    public bool ContainsKey(TFirst key) => _map.ContainsFirst(key);

        //    public IEnumerator<KeyValuePair<TFirst, TSecond>> GetEnumerator()
        //    {
        //        var version = _map._version;
        //        var entries = _map._entries;
        //        var count = _map.Count;
        //        for (int i = 0; i < count; ++i)
        //        {
        //            if (version != _map._version)
        //            {
        //                throw new InvalidOperationException("Collection was modified; enumeration operation may not execute");
        //            }

        //            var entry = entries[i];
        //            yield return new KeyValuePair<TFirst, TSecond>(entry.First, entry.Second);
        //        }
        //    }

        //    public bool Remove(TFirst key) => _map.RemoveByFirst(key);

        //    public bool TryGetValue(TFirst key, out TSecond value) => _map.TryGetSecond(key, out value);

        //    #region Explicit Interface Implementation
        //    TSecond IDictionary<TFirst, TSecond>.this[TFirst key]
        //    {
        //        get
        //        {
        //            return this[key];
        //        }
        //        set
        //        {
        //            throw new NotSupportedException();
        //        }
        //    }

        //    bool ICollection<KeyValuePair<TFirst, TSecond>>.IsReadOnly => false;

        //    void ICollection<KeyValuePair<TFirst, TSecond>>.Add(KeyValuePair<TFirst, TSecond> item)
        //    {
        //        if (!_map.Add(item.Key, item.Value))
        //        {
        //            throw new ArgumentException("Could not add value", nameof(item));
        //        }
        //    }

        //    void IDictionary<TFirst, TSecond>.Add(TFirst key, TSecond value)
        //    {
        //        if (!_map.Add(key, value))
        //        {
        //            throw new ArgumentException("Could not add values");
        //        }
        //    }

        //    bool ICollection<KeyValuePair<TFirst, TSecond>>.Contains(KeyValuePair<TFirst, TSecond> item)
        //    {
        //        var index = _map.IndexOfFirst(item.Key);
        //        return index >= 0 && _map.SecondComparer.Equals(_map._entries[index].Second, item.Value);
        //    }

        //    void ICollection<KeyValuePair<TFirst, TSecond>>.CopyTo(KeyValuePair<TFirst, TSecond>[] array, int arrayIndex)
        //    {
        //        if (arrayIndex < 0 || arrayIndex >= array.Length)
        //        {
        //            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "arrayIndex must be greater than or equal to 0 and less than array.Length");
        //        }
        //        if (arrayIndex + Count > array.Length)
        //        {
        //            throw new ArgumentOutOfRangeException(nameof(array), "array.Length - arrayIndex must be greater than or equal to Count");
        //        }

        //        var count = _map.Count;
        //        var entries = _map._entries;
        //        for (int i = 0; i < count; ++i)
        //        {
        //            var entry = entries[i];
        //            array[arrayIndex++] = new KeyValuePair<TFirst, TSecond>(entry.First, entry.Second);
        //        }
        //    }

        //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //    bool ICollection<KeyValuePair<TFirst, TSecond>>.Remove(KeyValuePair<TFirst, TSecond> item)
        //    {
        //        var index = _map.IndexOfFirst(item.Key);
        //        if (index >= 0 && _map.SecondComparer.Equals(_map._entries[index].Second, item.Value))
        //        {
        //            _map.RemoveAt(index);
        //            return true;
        //        }
        //        return false;
        //    }
        //    #endregion
        //}

        //public sealed class ReverseDictionary : IDictionary<TSecond, TFirst>
        //{
        //    private readonly OrderedBiDirectionalDictionary<TFirst, TSecond> _map;

        //    public TFirst this[TSecond key] => _map.GetFirst(key);

        //    public int Count => _map.Count;

        //    public ICollection<TFirst> Values => _map.FirstItems;

        //    public ICollection<TSecond> Keys => _map.SecondItems;

        //    public ReverseDictionary(OrderedBiDirectionalDictionary<TFirst, TSecond> map)
        //    {
        //        Preconditions.NotNull(map, nameof(map));

        //        _map = map;
        //    }

        //    public void Clear()
        //    {
        //        _map.Clear();
        //    }

        //    public bool ContainsKey(TSecond key) => _map.ContainsSecond(key);

        //    public IEnumerator<KeyValuePair<TSecond, TFirst>> GetEnumerator()
        //    {
        //        var version = _map._version;
        //        var entries = _map._entries;
        //        var count = _map.Count;
        //        for (int i = 0; i < count; ++i)
        //        {
        //            if (version != _map._version)
        //            {
        //                throw new InvalidOperationException("Collection was modified; enumeration operation may not execute");
        //            }

        //            var entry = entries[i];
        //            yield return new KeyValuePair<TSecond, TFirst>(entry.Second, entry.First);
        //        }
        //    }

        //    public bool Remove(TSecond key) => _map.RemoveBySecond(key);

        //    public bool TryGetValue(TSecond key, out TFirst value) => _map.TryGetFirst(key, out value);

        //    #region Explicit Interface Implementation
        //    TFirst IDictionary<TSecond, TFirst>.this[TSecond key]
        //    {
        //        get
        //        {
        //            return this[key];
        //        }
        //        set
        //        {
        //            throw new NotSupportedException();
        //        }
        //    }

        //    bool ICollection<KeyValuePair<TSecond, TFirst>>.IsReadOnly => false;

        //    void ICollection<KeyValuePair<TSecond, TFirst>>.Add(KeyValuePair<TSecond, TFirst> item)
        //    {
        //        if (!_map.Add(item.Value, item.Key))
        //        {
        //            throw new ArgumentException("Could not add value", nameof(item));
        //        }
        //    }

        //    void IDictionary<TSecond, TFirst>.Add(TSecond key, TFirst value)
        //    {
        //        if (!_map.Add(value, key))
        //        {
        //            throw new ArgumentException("Could not add values");
        //        }
        //    }

        //    bool ICollection<KeyValuePair<TSecond, TFirst>>.Contains(KeyValuePair<TSecond, TFirst> item)
        //    {
        //        var index = _map.IndexOfFirst(item.Value);
        //        return index >= 0 && _map.SecondComparer.Equals(_map._entries[index].Second, item.Key);
        //    }

        //    void ICollection<KeyValuePair<TSecond, TFirst>>.CopyTo(KeyValuePair<TSecond, TFirst>[] array, int arrayIndex)
        //    {
        //        if (arrayIndex < 0 || arrayIndex >= array.Length)
        //        {
        //            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "arrayIndex must be greater than or equal to 0 and less than array.Length");
        //        }
        //        if (arrayIndex + Count > array.Length)
        //        {
        //            throw new ArgumentOutOfRangeException(nameof(array), "array.Length - arrayIndex must be greater than or equal to Count");
        //        }

        //        var entries = _map._entries;
        //        var count = _map.Count;
        //        for (int i = 0; i < count; ++i)
        //        {
        //            var entry = entries[i];
        //            array[arrayIndex++] = new KeyValuePair<TSecond, TFirst>(entry.Second, entry.First);
        //        }
        //    }

        //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //    bool ICollection<KeyValuePair<TSecond, TFirst>>.Remove(KeyValuePair<TSecond, TFirst> item)
        //    {
        //        var index = _map.IndexOfFirst(item.Value);
        //        if (index >= 0 && _map.SecondComparer.Equals(_map._entries[index].Second, item.Key))
        //        {
        //            _map.RemoveAt(index);
        //            return true;
        //        }
        //        return false;
        //    }
        //    #endregion
        //}
        #endregion

        #region Public Methods
        //public TSecond GetSecond(TFirst first)
        //{
        //    var index = IndexOfFirst(first);
        //    if (index < 0)
        //    {
        //        throw new KeyNotFoundException();
        //    }
        //    return _entries[index].Second;
        //}

        //public TFirst GetFirst(TSecond second)
        //{
        //    var index = IndexOfSecond(second);
        //    if (index < 0)
        //    {
        //        throw new KeyNotFoundException();
        //    }
        //    return _entries[index].First;
        //}

        public Pair<TFirst, TSecond> GetAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "value must be greater than or equal to 0 and less than Count");
            }

            var entry = _entries[index];
            return Pair.Create(entry.First, entry.Second);
        }

        public TFirst GetFirstAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "value must be greater than or equal to 0 and less than Count");
            }

            return _entries[index].First;
        }

        public TSecond GetSecondAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "value must be greater than or equal to 0 and less than Count");
            }

            return _entries[index].Second;
        }

        //public bool TryGetSecond(TFirst first, out TSecond second)
        //{
        //    var index = IndexOfFirst(first);
        //    if (index >= 0)
        //    {
        //        second = _entries[index].Second;
        //        return true;
        //    }
        //    second = default(TSecond);
        //    return false;
        //}

        //public bool TryGetFirst(TSecond second, out TFirst first)
        //{
        //    var index = IndexOfSecond(second);
        //    if (index >= 0)
        //    {
        //        first = _entries[index].First;
        //        return true;
        //    }
        //    first = default(TFirst);
        //    return false;
        //}

        public bool ContainsFirst(TFirst first) => IndexOfFirst(first) >= 0;

        //public bool ContainsSecond(TSecond second) => IndexOfSecond(second) >= 0;

        //public bool RemoveByFirst(TFirst first)
        //{
        //    var index = IndexOfFirst(first);
        //    if (index < 0)
        //    {
        //        return false;
        //    }
        //    RemoveAt(index);
        //    return true;
        //}

        //public bool RemoveBySecond(TSecond second)
        //{
        //    var index = IndexOfSecond(second);
        //    if (index < 0)
        //    {
        //        return false;
        //    }
        //    RemoveAt(index);
        //    return true;
        //}

        //public void RemoveAt(int index)
        //{
        //    if (index < 0 || index >= Count)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(index), "value must be greater than or equal to 0 and less than Count");
        //    }

        //    var entry = _entries[index];
        //    RemoveFirstAt(index, entry);
        //    RemoveSecondAt(index, entry);
        //    ShiftEntries(index + 1, Count - 1, -1);
        //    ++_version;
        //}

        //public bool SetFirstBySecond(TSecond second, TFirst first)
        //{
        //    var index = IndexOfSecond(second);
        //    return index >= 0 ? ReplaceFirstAt(index, first) : Add(first, second);
        //}

        //public bool SetSecondByFirst(TFirst first, TSecond second)
        //{
        //    var index = IndexOfFirst(first);
        //    return index >= 0 ? ReplaceSecondAt(index, second) : Add(first, second);
        //}

        //public bool SetAt(int index, TFirst first, TSecond second) => index == Count ? Add(first, second) : ReplaceAt(index, first, second);

        //public bool SetAt(int index, Pair<TFirst, TSecond> pair) => SetAt(index, pair.First, pair.Second);

        //public bool ReplaceFirstBySecond(TSecond second, TFirst first)
        //{
        //    var index = IndexOfSecond(second);
        //    return index >= 0 && ReplaceFirstAt(index, first);
        //}

        //public bool ReplaceFirstAt(int index, TFirst first)
        //{
        //    if (index < 0 || index >= Count)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(index), "value must be greater than or equal to 0 and less than Count");
        //    }

        //    var firstHashCode = GetFirstHashCode(first);
        //    var firstBucket = GetFirstBucket(firstHashCode);
        //    if (IndexOfFirst(first, firstHashCode, firstBucket) >= 0)
        //    {
        //        return false;
        //    }

        //    var entry = _entries[index];
        //    RemoveFirstAt(index, entry);
        //    SetFirst(index, first, firstHashCode, firstBucket, entry);
        //    return true;
        //}

        //public bool ReplaceSecondByFirst(TFirst first, TSecond second)
        //{
        //    var index = IndexOfFirst(first);
        //    return index >= 0 && ReplaceSecondAt(index, second);
        //}

        public bool ReplaceSecondAt(int index, TSecond second)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "value must be greater than or equal to 0 and less than Count");
            }

            var secondHashCode = GetSecondHashCode(second);
            var secondBucket = GetSecondBucket(secondHashCode);
            if (IndexOfSecond(second, secondHashCode, secondBucket) >= 0)
            {
                return false;
            }

            var entry = _entries[index];
            RemoveSecondAt(index, entry);
            SetSecond(index, second, secondHashCode, secondBucket, entry);
            return true;
        }

        //public bool ReplaceAt(int index, TFirst first, TSecond second)
        //{
        //    if (index < 0 || index >= Count)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(index), "value must be greater than or equal to 0 and less than Count");
        //    }

        //    var firstHashCode = GetFirstHashCode(first);
        //    var firstBucket = GetFirstBucket(firstHashCode);
        //    if (IndexOfFirst(first, firstHashCode, firstBucket) >= 0)
        //    {
        //        return false;
        //    }

        //    var secondHashCode = GetSecondHashCode(second);
        //    var secondBucket = GetSecondBucket(secondHashCode);
        //    if (IndexOfSecond(second, secondHashCode, secondBucket) >= 0)
        //    {
        //        return false;
        //    }

        //    var entry = _entries[index];
        //    RemoveFirstAt(index, entry);
        //    RemoveSecondAt(index, entry);
        //    SetEntry(index, first, firstHashCode, firstBucket, second, secondHashCode, secondBucket);
        //    ++_version;
        //    return true;
        //}

        //public bool ReplaceAt(int index, Pair<TFirst, TSecond> pair) => ReplaceAt(index, pair.First, pair.Second);

        //public bool Add(TFirst first, TSecond second) => Insert(Count, first, second);

        //public bool Add(Pair<TFirst, TSecond> pair) => Insert(Count, pair.First, pair.Second);

        public bool Insert(int index, TFirst first, TSecond second)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "value must be greater than or equal to 0 and less than or equal to Count");
            }

            var firstHashCode = GetFirstHashCode(first);
            var firstBucket = GetFirstBucket(firstHashCode);
            if (IndexOfFirst(first, firstHashCode, firstBucket) >= 0)
            {
                return false;
            }

            var secondHashCode = GetSecondHashCode(second);
            var secondBucket = GetSecondBucket(secondHashCode);
            if (IndexOfSecond(second, secondHashCode, secondBucket) >= 0)
            {
                return false;
            }

            if (_entries.Length == Count)
            {
                Resize(Count << 1, false);
                firstBucket = GetFirstBucket(firstHashCode);
                secondBucket = GetSecondBucket(secondHashCode);
            }
            ShiftEntries(index, Count - 1, 1);
            SetEntry(index, first, firstHashCode, firstBucket, second, secondHashCode, secondBucket);
            ++_version;
            return true;
        }

        private void SetEntry(int index, TFirst first, int firstHashCode, int firstBucket, TSecond second, int secondHashCode, int secondBucket)
        {
            _entries[index] = new Entry(first, firstHashCode, _firstBuckets[firstBucket] - 1, second, secondHashCode, _secondBuckets[secondBucket] - 1);
            _firstBuckets[firstBucket] = index + 1;
            _secondBuckets[secondBucket] = index + 1;
        }

        //private void SetFirst(int index, TFirst first, int firstHashCode, int firstBucket, Entry entry)
        //{
        //    entry.First = first;
        //    entry.FirstHashCode = firstHashCode;
        //    entry.FirstBucketNext = _firstBuckets[firstBucket] - 1;
        //    _entries[index] = entry;
        //    _firstBuckets[firstBucket] = index + 1;
        //}

        private void SetSecond(int index, TSecond second, int secondHashCode, int secondBucket, Entry entry)
        {
            entry.Second = second;
            entry.SecondHashCode = secondHashCode;
            entry.SecondBucketNext = _secondBuckets[secondBucket] - 1;
            _entries[index] = entry;
            _secondBuckets[secondBucket] = index + 1;
        }

        //public bool Insert(int index, Pair<TFirst, TSecond> pair) => Insert(index, pair.First, pair.Second);

        //public void Clear()
        //{
        //    Array.Clear(_entries, 0, Count);
        //    Array.Clear(_firstBuckets, 0, _firstBuckets.Length);
        //    Array.Clear(_secondBuckets, 0, _secondBuckets.Length);
        //    Count = 0;
        //    ++_version;
        //}

        //public TFirst GetFirstByFirst(TFirst first)
        //{
        //    var index = IndexOfFirst(first);
        //    if (index < 0)
        //    {
        //        throw new KeyNotFoundException();
        //    }

        //    return _entries[index].First;
        //}

        //public TSecond GetSecondBySecond(TSecond second)
        //{
        //    var index = IndexOfSecond(second);
        //    if (index < 0)
        //    {
        //        throw new KeyNotFoundException();
        //    }

        //    return _entries[index].Second;
        //}

        //public bool TryGetFirstByFirst(TFirst first, out TFirst actual)
        //{
        //    var index = IndexOfFirst(first);
        //    if (index >= 0)
        //    {
        //        actual = _entries[index].First;
        //        return true;
        //    }
        //    actual = default(TFirst);
        //    return false;
        //}

        //public bool TryGetSecondBySecond(TSecond second, out TSecond actual)
        //{
        //    var index = IndexOfSecond(second);
        //    if (index >= 0)
        //    {
        //        actual = _entries[index].Second;
        //        return true;
        //    }
        //    actual = default(TSecond);
        //    return false;
        //}

        public int IndexOfFirst(TFirst first)
        {
            var firstHashCode = GetFirstHashCode(first);
            var firstBucket = GetFirstBucket(firstHashCode);
            return IndexOfFirst(first, firstHashCode, firstBucket);
        }

        public int IndexOfSecond(TSecond second)
        {
            var secondHashCode = GetSecondHashCode(second);
            var secondBucket = GetSecondBucket(secondHashCode);
            return IndexOfSecond(second, secondHashCode, secondBucket);
        }

        public int TrimExcess() => Capacity = Count;

        public IEnumerator<Pair<TFirst, TSecond>> GetEnumerator()
        {
            var version = _version;
            for (var i = 0; i < Count; ++i)
            {
                if (version != _version)
                {
                    throw new InvalidOperationException("Collection was modified; enumeration operation may not execute");
                }

                var entry = _entries[i];
                yield return Pair.Create(entry.First, entry.Second);
            }
        }
        #endregion

        #region Private Methods
        private int GetFirstHashCode(TFirst first)
        {
            if (first == null)
            {
                return 0;
            }
            return FirstComparer.GetHashCode(first) & 0x7FFFFFFF;
        }

        private int GetSecondHashCode(TSecond second)
        {
            if (second == null)
            {
                return 0;
            }
            return SecondComparer.GetHashCode(second) & 0x7FFFFFFF;
        }

        private int GetFirstBucket(int firstHashCode) => firstHashCode % _firstBuckets.Length;

        private int GetSecondBucket(int secondHashCode) => secondHashCode % _secondBuckets.Length;

        private void Resize(int minCapacity, bool getMin)
        {
            if (minCapacity < 0)
            {
                minCapacity = int.MaxValue;
            }
            if (minCapacity == int.MaxValue && _entries.Length == int.MaxValue)
            {
                throw new InvalidOperationException("cannot increase capacity past int.MaxValue");
            }
            var newCapacity = HashHelper.GetPrimeGTE(minCapacity, getMin);
            if (_entries.Length != newCapacity)
            {
                var entries = new Entry[newCapacity];
                _firstBuckets = new int[newCapacity];
                _secondBuckets = new int[newCapacity];
                for (var i = 0; i < Count; ++i)
                {
                    var entry = _entries[i];
                    var firstBucket = GetFirstBucket(entry.FirstHashCode);
                    entry.FirstBucketNext = _firstBuckets[firstBucket] - 1;
                    _firstBuckets[firstBucket] = i + 1;
                    var secondBucket = GetSecondBucket(entry.SecondHashCode);
                    entry.SecondBucketNext = _secondBuckets[secondBucket] - 1;
                    _secondBuckets[secondBucket] = i + 1;
                    // assign to new array
                    entries[i] = entry;
                }
                _entries = entries;
            }
        }

        private int IndexOfFirst(TFirst first, int firstHashCode, int firstBucket)
        {
            Entry entry;
            for (var i = _firstBuckets[firstBucket] - 1; i >= 0; i = entry.FirstBucketNext)
            {
                entry = _entries[i];
                if (entry.FirstHashCode == firstHashCode && FirstComparer.Equals(entry.First, first))
                {
                    return i;
                }
            }
            return -1;
        }

        private int IndexOfSecond(TSecond second, int secondHashCode, int secondBucket)
        {
            Entry entry;
            for (var i = _secondBuckets[secondBucket] - 1; i >= 0; i = entry.SecondBucketNext)
            {
                entry = _entries[i];
                if (entry.SecondHashCode == secondHashCode && SecondComparer.Equals(entry.Second, second))
                {
                    return i;
                }
            }
            return -1;
        }

        //private void RemoveFirstAt(int index, Entry entry)
        //{
        //    var firstHashCode = entry.FirstHashCode;
        //    var firstBucket = GetFirstBucket(firstHashCode);
        //    var firstIndex = _firstBuckets[firstBucket] - 1;
        //    if (firstIndex == index)
        //    {
        //        _firstBuckets[firstBucket] = entry.FirstBucketNext + 1;
        //    }
        //    else
        //    {
        //        int prevIndex;
        //        do
        //        {
        //            prevIndex = firstIndex;
        //            firstIndex = _entries[firstIndex].FirstBucketNext;
        //        } while (firstIndex != index);
        //        _entries[prevIndex].FirstBucketNext = entry.FirstBucketNext;
        //    }
        //}

        private void RemoveSecondAt(int index, Entry entry)
        {
            var secondHashCode = entry.SecondHashCode;
            var secondBucket = GetSecondBucket(secondHashCode);
            var secondIndex = _secondBuckets[secondBucket] - 1;
            if (secondIndex == index)
            {
                _secondBuckets[secondBucket] = entry.SecondBucketNext + 1;
            }
            else
            {
                int prevIndex;
                do
                {
                    prevIndex = secondIndex;
                    secondIndex = _entries[secondIndex].SecondBucketNext;
                } while (secondIndex != index);
                _entries[prevIndex].SecondBucketNext = entry.SecondBucketNext;
            }
        }
        
        private void ShiftEntries(int startIndex, int endIndex, int amount)
        {
            if (endIndex >= startIndex)
            {
                if (amount > 0)
                {
                    for (var i = endIndex; i >= startIndex; --i)
                    {
                        ShiftEntry(i, amount);
                    }
                }
                else
                {
                    for (var i = startIndex; i <= endIndex; ++i)
                    {
                        ShiftEntry(i, amount);
                    }
                }
            }
            Count += amount;
        }

        private void ShiftEntry(int index, int amount)
        {
            var entry = _entries[index];

            // Patches Up First Bucket List
            var firstBucket = GetFirstBucket(entry.FirstHashCode);
            var firstIndex = _firstBuckets[firstBucket] - 1;
            if (firstIndex == index)
            {
                _firstBuckets[firstBucket] += amount;
            }
            else
            {
                int prevIndex;
                do
                {
                    prevIndex = firstIndex;
                    firstIndex = _entries[firstIndex].FirstBucketNext;
                } while (firstIndex != index);
                _entries[prevIndex].FirstBucketNext += amount;
            }

            // Patches Up Second Bucket List
            var secondBucket = GetSecondBucket(entry.SecondHashCode);
            var secondIndex = _secondBuckets[secondBucket] - 1;
            if (secondIndex == index)
            {
                _secondBuckets[secondBucket] += amount;
            }
            else
            {
                int prevIndex;
                do
                {
                    prevIndex = secondIndex;
                    secondIndex = _entries[secondIndex].SecondBucketNext;
                } while (secondIndex != index);
                _entries[prevIndex].SecondBucketNext += amount;
            }

            // Copies Entry To New Index
            _entries[index + amount] = entry;
        }
        #endregion

        #region Explicit Interface Implementation
        //bool ICollection<Pair<TFirst, TSecond>>.IsReadOnly => false;

        //Pair<TFirst, TSecond> IList<Pair<TFirst, TSecond>>.this[int index]
        //{
        //    get
        //    {
        //        return GetAt(index);
        //    }
        //    set
        //    {
        //        throw new NotSupportedException();
        //    }
        //}

        //int IList<Pair<TFirst, TSecond>>.IndexOf(Pair<TFirst, TSecond> item)
        //{
        //    var index = IndexOfFirst(item.First);
        //    if (index >= 0 && !SecondComparer.Equals(_entries[index].Second, item.Second))
        //    {
        //        index = -1;
        //    }
        //    return index;
        //}

        //void IList<Pair<TFirst, TSecond>>.Insert(int index, Pair<TFirst, TSecond> item)
        //{
        //    if (!Insert(index, item))
        //    {
        //        throw new ArgumentException("Cannot insert value", nameof(item));
        //    }
        //}

        //void ICollection<Pair<TFirst, TSecond>>.Add(Pair<TFirst, TSecond> item)
        //{
        //    if (!Add(item))
        //    {
        //        throw new ArgumentException("Cannot add value", nameof(item));
        //    }
        //}

        //bool ICollection<Pair<TFirst, TSecond>>.Contains(Pair<TFirst, TSecond> item)
        //{
        //    var index = IndexOfFirst(item.First);
        //    return index >= 0 && SecondComparer.Equals(_entries[index].Second, item.Second);
        //}

        //void ICollection<Pair<TFirst, TSecond>>.CopyTo(Pair<TFirst, TSecond>[] array, int arrayIndex)
        //{
        //    if (arrayIndex < 0 || arrayIndex >= array.Length)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(arrayIndex), "arrayIndex must be greater than or equal to 0 and less than array.Length");
        //    }
        //    if (arrayIndex + Count > array.Length)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(array), "array.Length - arrayIndex must be greater than or equal to Count");
        //    }

        //    for (int i = 0; i < Count; ++i)
        //    {
        //        var entry = _entries[i];
        //        array[arrayIndex++] = Pair.Create(entry.First, entry.Second);
        //    }
        //}

        //bool ICollection<Pair<TFirst, TSecond>>.Remove(Pair<TFirst, TSecond> item)
        //{
        //    var index = IndexOfFirst(item.First);
        //    if (index >= 0 && SecondComparer.Equals(_entries[index].Second, item.Second))
        //    {
        //        RemoveAt(index);
        //        return true;
        //    }
        //    return false;
        //}

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }

    internal static class Pair
    {
        public static Pair<TFirst, TSecond> Create<TFirst, TSecond>(TFirst first, TSecond second) => new Pair<TFirst, TSecond>(first, second);
    }

    internal struct Pair<TFirst, TSecond>
    {
        public TFirst First { get; }

        public TSecond Second { get; }

        public Pair(TFirst first, TSecond second)
        {
            First = first;
            Second = second;
        }

        public Pair<TSecond, TFirst> GetReversed() => Pair.Create(Second, First);

        public override string ToString() => $"[{First}, {Second}]";
    }

    //internal sealed class OrderedBiDirectionalDictionaryDebugView<TFirst, TSecond>
    //{
    //    private readonly OrderedBiDirectionalDictionary<TFirst, TSecond> _map;

    //    internal OrderedBiDirectionalDictionaryDebugView(OrderedBiDirectionalDictionary<TFirst, TSecond> map)
    //    {
    //        Preconditions.NotNull(map, nameof(map));

    //        _map = map;
    //    }

    //    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    //    internal Pair<TFirst, TSecond>[] Items
    //    {
    //        get
    //        {
    //            var items = new Pair<TFirst, TSecond>[_map.Count];
    //            ((ICollection<Pair<TFirst, TSecond>>)_map).CopyTo(items, 0);
    //            return items;
    //        }
    //    }
    //}
}
