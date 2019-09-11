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
using System.Linq;
using System.Threading;

namespace EnumsNET
{
    internal sealed class TypeDictionary<T>
    {
        private DictionarySlim<Type, T> _dictionary = new DictionarySlim<Type, T>(null, 0);

        public T GetOrAdd(Type key, Func<Type, T> valueFactory)
        {
            var dictionary = _dictionary;
            if (!dictionary.TryGetValue(key, out var value))
            {
                value = valueFactory(key);
                DictionarySlim<Type, T> oldDictionary;
                do
                {
                    if (dictionary.TryGetValue(key, out var foundValue))
                    {
                        value = foundValue;
                        break;
                    }
                    oldDictionary = dictionary;
                    dictionary = new DictionarySlim<Type, T>(dictionary.Concat(new[] { new KeyValuePair<Type, T>(key, value) }), dictionary.Count + 1);
                } while ((dictionary = Interlocked.CompareExchange(ref _dictionary, dictionary, oldDictionary)) != oldDictionary);
            }
            return value;
        }
    }
}