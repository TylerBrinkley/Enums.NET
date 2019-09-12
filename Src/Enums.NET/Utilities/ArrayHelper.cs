using System;
using System.Collections.Generic;

namespace EnumsNET.Utilities
{
    internal static class ArrayHelper
    {
        public static T[] Empty<T>() =>
#if ARRAY_EMPTY
            Array.Empty<T>();
#else
            new T[0];
#endif

        public static T[] ToArray<T>(IEnumerable<T> items, int count)
        {
            var a = new T[count];
            var i = 0;
            foreach (var item in items)
            {
                a[i++] = item;
            }
            return a;
        }
    }
}