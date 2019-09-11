using System;

namespace EnumsNET
{
    internal static class ArrayHelper
    {
        public static T[] Empty<T>() =>
#if ARRAY_EMPTY
            Array.Empty<T>();
#else
            new T[0];
#endif
    }
}