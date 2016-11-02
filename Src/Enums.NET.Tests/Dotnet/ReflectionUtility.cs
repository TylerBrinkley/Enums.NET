using System;
#if !TYPE_REFLECTION
using System.Reflection;
#endif

namespace EnumsNET
{
    internal static class ReflectionUtility
    {
#if !TYPE_REFLECTION
        public static Type[] GetGenericArguments(this Type type) => type.GetTypeInfo().GetGenericArguments();

        public static Type[] GetGenericParameterConstraints(this Type type) => type.GetTypeInfo().GetGenericParameterConstraints();
#endif
    }
}