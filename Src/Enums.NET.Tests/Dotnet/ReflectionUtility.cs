using System;
#if !TYPE_REFLECTION || !GET_TYPE_CODE
using System.Collections.Generic;
#endif
#if !TYPE_REFLECTION
using System.Reflection;
#endif

namespace EnumsNET
{
    internal static class ReflectionUtility
    {
#if !TYPE_REFLECTION
        public static IEnumerable<Attribute> GetCustomAttributes(this Type type, bool inherit) => type.GetTypeInfo().GetCustomAttributes(inherit);

        public static IEnumerable<Type> GetInterfaces(this Type type) => type.GetTypeInfo().ImplementedInterfaces;

        public static bool IsDefined(this Type type, Type attributeType, bool inherit) => type.GetTypeInfo().IsDefined(attributeType, inherit);

        public static Type[] GetGenericArguments(this Type type) => type.GenericTypeArguments;

        public static Type[] GetGenericParameterConstraints(this Type type) => type.GetTypeInfo().GetGenericParameterConstraints();
#endif

        public static bool IsEnum(this Type type)
        {
            return type.
#if !TYPE_REFLECTION
                GetTypeInfo().
#endif
                IsEnum;
        }
    }
}