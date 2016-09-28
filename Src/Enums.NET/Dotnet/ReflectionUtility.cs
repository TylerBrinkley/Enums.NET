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
#endif

#if !GET_TYPE_CODE
        private static readonly Dictionary<Type, TypeCode> _typeCodeMap = new Dictionary<Type, TypeCode>
        {
            { typeof(sbyte), TypeCode.SByte },
            { typeof(byte), TypeCode.Byte },
            { typeof(short), TypeCode.Int16 },
            { typeof(ushort), TypeCode.UInt16 },
            { typeof(int), TypeCode.Int32 },
            { typeof(uint), TypeCode.UInt32 },
            { typeof(long), TypeCode.Int64 },
            { typeof(ulong), TypeCode.UInt64 },
            { typeof(string), TypeCode.String }
        };
#endif

        public static TypeCode GetTypeCode(this Type type)
        {
#if GET_TYPE_CODE
            return Type.GetTypeCode(type);
#else
            TypeCode typeCode;
            return _typeCodeMap.TryGetValue(type, out typeCode) ? typeCode : TypeCode.Object;
#endif
        }

        public static bool IsEnum(this Type type)
        {
#if TYPE_REFLECTION
            return type.IsEnum;
#else
            return type.GetTypeInfo().IsEnum;
#endif
        }
    }
}