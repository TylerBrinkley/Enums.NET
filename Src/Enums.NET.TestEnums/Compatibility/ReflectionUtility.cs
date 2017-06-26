using System;
using System.Reflection;

namespace EnumsNET
{
    internal static class ReflectionUtility
    {
        public static Assembly GetAssembly(this Type type) => type.
#if !TYPE_REFLECTION
            GetTypeInfo().
#endif
            Assembly;
    }
}