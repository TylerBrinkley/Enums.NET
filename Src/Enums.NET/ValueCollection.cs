namespace EnumsNET
{
    // A struct that's foreach-able without allocations and supports up to 3 elements or an array
    internal struct ValueCollection<T>
        where T : struct
    {
        public T? Item1;
        public T? Item2;
        public T? Item3;
        public T[] Items;

        public ValueCollection(T item1, T? item2 = null, T? item3 = null)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Items = null;
        }

        public ValueCollection(T[] items)
        {
            Item1 = null;
            Item2 = null;
            Item3 = null;
            Items = items;
        }

        public bool Any() => Item1.HasValue || Items?.Length > 0;

        public Enumerator GetEnumerator() => new Enumerator(this);

        public struct Enumerator
        {
            private ValueCollection<T> _valueCollection;
            private int _index;

            public T Current { get; private set; }

            public Enumerator(ValueCollection<T> valueCollection)
            {
                _valueCollection = valueCollection;
                Current = default;
                _index = valueCollection.Item1.HasValue ? -1 : 0;
            }

            public bool MoveNext()
            {
                var success = false;
                if (_index >= 0)
                {
                    var items = _valueCollection.Items;
                    if (_index < items?.Length)
                    {
                        Current = items[_index];
                        success = true;
                        ++_index;
                    }
                    else
                    {
                        Current = default;
                    }
                }
                else
                {
                    var item = _index == -1 ? _valueCollection.Item1 : (_index == -2 ? _valueCollection.Item2 : (_index == -3 ? _valueCollection.Item3 : null));
                    if (item.HasValue)
                    {
                        Current = item.GetValueOrDefault();
                        success = true;
                        --_index;
                    }
                    else
                    {
                        Current = default;
                    }
                }
                return success;
            }
        }
    }
}
