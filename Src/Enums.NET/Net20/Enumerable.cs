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

        public static bool Any<T>(this IEnumerable<T> source)
        {
            Preconditions.NotNull(source, nameof(source));

            using (var enumerator = source.GetEnumerator())
            {
                return enumerator.MoveNext();
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
    }
}
#endif