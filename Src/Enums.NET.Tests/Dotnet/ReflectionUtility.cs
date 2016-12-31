using System;
using System.Reflection;

namespace EnumsNET
{
    internal static class ReflectionUtility
    {
#if !TYPE_REFLECTION
        public static Type[] GetGenericArguments(this Type type) => type.GetTypeInfo().GetGenericArguments();

        public static Type[] GetGenericParameterConstraints(this Type type) => type.GetTypeInfo().GetGenericParameterConstraints();
#endif

        public static Assembly GetAssembly(this Type type) => type.
#if !TYPE_REFLECTION
            GetTypeInfo().
#endif
            Assembly;
    }
}