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

#if !CONCURRENT_DICTIONARY
using System.Collections.Generic;
using System.Threading;
using EnumsNET;

namespace System.Collections.Concurrent
{
    internal sealed class ConcurrentDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            var dictionary = _dictionary;
            if (!dictionary.TryGetValue(key, out var value))
            {
                value = valueFactory(key);
                Dictionary<TKey, TValue> oldDictionary;
                do
                {
                    if (dictionary.TryGetValue(key, out var foundValue))
                    {
                        value = foundValue;
                        break;
                    }
                    oldDictionary = dictionary;
                    dictionary = new Dictionary<TKey, TValue>(dictionary, dictionary.Comparer)
                    {
                        { key, value }
                    };
                } while ((dictionary = Interlocked.CompareExchange(ref _dictionary, dictionary, oldDictionary)) != oldDictionary);
            }
            return value;
        }
    }
}
#endif