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

#if NET20
using System.Collections;
using System.Collections.Generic;
using EnumsNET;

namespace System.Linq
{
    internal static class Enumerable
    {
        public static IEnumerable<TResult> Select<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
        {
            Preconditions.NotNull(source, nameof(source));
            Preconditions.NotNull(selector, nameof(selector));

            foreach (var item in source)
            {
                yield return selector(item);
            }
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            Preconditions.NotNull(source, nameof(source));
            Preconditions.NotNull(predicate, nameof(predicate));

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public static T[] ToArray<T>(this IEnumerable<T> source)
        {
            Preconditions.NotNull(source, nameof(source));

            var collection = source as ICollection<T>;
            var length = (source as ICollection<T>)?.Count ?? (source as ICollection)?.Count;
            if (length.HasValue)
            {
                var array = new T[length.GetValueOrDefault()];
                var i = 0;
                foreach (var item in source)
                {
                    array[i++] = item;
                }
                return array;
            }
            var list = new List<T>();
            foreach (var item in source)
            {
                list.Add(item);
            }
            return list.ToArray();
        }

        public static int Count<T>(this IEnumerable<T> source)
        {
            Preconditions.NotNull(source, nameof(source));

            if (source is ICollection<T> collection)
            {
                return collection.Count;
            }
            if (source is ICollection collection2)
            {
                return collection2.Count;
            }
            var count = 0;
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ++count;
                }
            }
            return count;
        }
    }
}
#endif