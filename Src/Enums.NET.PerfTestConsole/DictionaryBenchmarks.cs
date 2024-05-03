using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumsNET.PerfTestConsole;

[SimpleJob(RuntimeMoniker.Net48), SimpleJob(RuntimeMoniker.Net80)]
public class DictionaryBenchmarks
{
    private readonly Dictionary<Type, string> _typeDictionary;
    private readonly DictionarySlim1<Type, string, TypeEqualityComparer> _typeDictionarySlim1;
    private readonly DictionarySlim2<Type, string> _typeDictionarySlim2;
    private readonly DictionarySlim3<Type, string> _typeDictionarySlim3;
    private readonly Dictionary<int, string> _intDictionary;
    private readonly DictionarySlim1<int, string, DefaultEqualityComparer<int>> _intDictionarySlim1;
    private readonly DictionarySlim2<int, string> _intDictionarySlim2;
    private readonly DictionarySlim3<int, string> _intDictionarySlim3;

    public DictionaryBenchmarks()
    {
        _typeDictionary = new Dictionary<Type, string>(11)
        {
            { typeof(AttributeTargets), nameof(AttributeTargets) },
            { typeof(Base64FormattingOptions), nameof(Base64FormattingOptions) },
            { typeof(ConsoleColor), nameof(ConsoleColor) },
            { typeof(ConsoleKey), nameof(ConsoleKey) },
            { typeof(ConsoleModifiers), nameof(ConsoleModifiers) },
            { typeof(ConsoleSpecialKey), nameof(ConsoleSpecialKey) },
            { typeof(DateTimeKind), nameof(DateTimeKind) },
            { typeof(DayOfWeek), nameof(DayOfWeek) },
            { typeof(EnvironmentVariableTarget), nameof(EnvironmentVariableTarget) },
            { typeof(GCCollectionMode), nameof(GCCollectionMode) },
            { typeof(GCNotificationStatus), nameof(GCNotificationStatus) }
        };
        _typeDictionarySlim1 = new DictionarySlim1<Type, string, TypeEqualityComparer>(_typeDictionary, _typeDictionary.Count);
        _typeDictionarySlim2 = new DictionarySlim2<Type, string>(_typeDictionary, _typeDictionary.Count);
        _typeDictionarySlim3 = new DictionarySlim3<Type, string>(_typeDictionary, _typeDictionary.Count);

        _intDictionary = new Dictionary<int, string>(11);
        for (var i = 0; i < 11; ++i)
        {
            _intDictionary.Add(i, i.ToString());
        }
        _intDictionarySlim1 = new DictionarySlim1<int, string, DefaultEqualityComparer<int>>(_intDictionary, _intDictionary.Count);
        _intDictionarySlim2 = new DictionarySlim2<int, string>(_intDictionary, _intDictionary.Count);
        _intDictionarySlim3 = new DictionarySlim3<int, string>(_intDictionary, _intDictionary.Count);
    }

    [Benchmark(Baseline = true)]
    public bool Type_Dictionary() => _typeDictionary.TryGetValue(typeof(DayOfWeek), out _);

    [Benchmark]
    public bool Type_DictionarySlim1() => _typeDictionarySlim1.TryGetValue(typeof(DayOfWeek), out _);

    [Benchmark]
    public bool Type_DictionarySlim2() => _typeDictionarySlim2.TryGetValue(typeof(DayOfWeek), out _);

    [Benchmark]
    public bool Type_DictionarySlim3() => _typeDictionarySlim3.TryGetValue(typeof(DayOfWeek), out _);

    [Benchmark]
    public bool Int_Dictionary() => _intDictionary.TryGetValue(7, out _);

    [Benchmark]
    public bool Int_DictionarySlim1() => _intDictionarySlim1.TryGetValue(7, out _);

    [Benchmark]
    public bool Int_DictionarySlim2() => _intDictionarySlim2.TryGetValue(7, out _);

    [Benchmark]
    public bool Int_DictionarySlim3() => _intDictionarySlim3.TryGetValue(7, out _);

    internal sealed class DictionarySlim1<TKey, TValue, TKeyComparer>
        where TKey : notnull
        where TKeyComparer : struct, IEqualityComparer<TKey>
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

        public int Count { get; }

        public DictionarySlim1(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, int count)
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
                    ref int bucket = ref buckets[value.GetHashCode() & (size - 1)];
                    entries[i] = new Entry(bucket - 1, value, pair.Value);
                    bucket = i + 1;
                    ++i;
                }
            }
            _buckets = buckets;
            _entries = entries;
        }

        internal bool TryGetValue(TKey key, out TValue value)
        {
            var entries = _entries;
            for (var i = _buckets[key.GetHashCode() & (_buckets.Length - 1)] - 1; i >= 0; i = entries[i].Next)
            {
                if (default(TKeyComparer).Equals(key, entries[i].Key))
                {
                    value = entries[i].Value;
                    return true;
                }
            }
            value = default!;
            return false;
        }
    }

    internal sealed class DictionarySlim2<TKey, TValue>
        where TKey : notnull
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

        public int Count { get; }

        public DictionarySlim2(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, int count)
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
                    ref int bucket = ref buckets[value.GetHashCode() & (size - 1)];
                    entries[i] = new Entry(bucket - 1, value, pair.Value);
                    bucket = i + 1;
                    ++i;
                }
            }
            _buckets = buckets;
            _entries = entries;
        }

        internal bool TryGetValue(TKey key, out TValue value)
        {
            var entries = _entries;
            for (var i = _buckets[key.GetHashCode() & (_buckets.Length - 1)] - 1; i >= 0; i = entries[i].Next)
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
    }

    internal sealed class DictionarySlim3<TKey, TValue>
        where TKey : notnull
    {
        public readonly struct Entry
        {
            public readonly int Next;
            public readonly int HashCode;
            public readonly TKey Key;
            public readonly TValue Value;

            public Entry(int next, int hashCode, TKey key, TValue value)
            {
                Next = next;
                HashCode = hashCode;
                Key = key;
                Value = value;
            }
        }

        private readonly int[] _buckets;
        internal readonly Entry[] _entries;

        public int Count { get; }

        public DictionarySlim3(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, int count)
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
                    var hashCode = value.GetHashCode();
                    ref int bucket = ref buckets[hashCode & (size - 1)];
                    entries[i] = new Entry(bucket - 1, hashCode, value, pair.Value);
                    bucket = i + 1;
                    ++i;
                }
            }
            _buckets = buckets;
            _entries = entries;
        }

        internal bool TryGetValue(TKey key, out TValue value)
        {
            var entries = _entries;
            var hashCode = key.GetHashCode();
            for (var i = _buckets[hashCode & (_buckets.Length - 1)] - 1; i >= 0; i = entries[i].Next)
            {
                if (hashCode == entries[i].HashCode && EqualityComparer<TKey>.Default.Equals(key, entries[i].Key))
                {
                    value = entries[i].Value;
                    return true;
                }
            }
            value = default!;
            return false;
        }
    }

    internal struct DefaultEqualityComparer<T> : IEqualityComparer<T> where T : struct, IEquatable<T>
    {
        public readonly bool Equals(T x, T y) => x.Equals(y);

        public readonly int GetHashCode(T obj) => obj.GetHashCode();
    }

    internal struct TypeEqualityComparer : IEqualityComparer<Type>
    {
        public readonly bool Equals(Type x, Type y) => x.Equals(y);

        public readonly int GetHashCode(Type obj) => obj.GetHashCode();
    }

    internal static partial class HashHelpers
    {
        public static int PowerOf2(int v)
        {
            if ((v & (v - 1)) == 0 && v >= 1)
            {
                return v;
            }

            var i = 4;
            while (i < v)
            {
                i <<= 1;
            }

            return i;
        }
    }
}