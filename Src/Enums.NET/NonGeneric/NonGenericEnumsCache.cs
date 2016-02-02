using System;
using System.Reflection;

#if NET20 || NET35
using System.Collections.Generic;
#else
using System.Collections.Concurrent;
#endif

namespace EnumsNET.NonGeneric
{
    internal struct NonGenericEnumsCache
    {
#if NET20 || NET35
        private static readonly Dictionary<Type, NonGenericEnumsCache> _enumsCacheDictionary = new Dictionary<Type, NonGenericEnumsCache>();
#else
        private static readonly ConcurrentDictionary<Type, NonGenericEnumsCache> _enumsCacheDictionary = new ConcurrentDictionary<Type, NonGenericEnumsCache>();
#endif
        private static readonly MethodInfo _openCreateMethod = typeof(NonGenericEnumsCache).GetMethod(nameof(Create), BindingFlags.Static | BindingFlags.NonPublic);

        internal static NonGenericEnumsCache Get(Type enumType)
        {
            Preconditions.NotNull(enumType, nameof(enumType));
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("must be an enum type", nameof(enumType));
            }

            NonGenericEnumsCache enumsCache;
#if NET20 || NET35
            lock (_enumsCacheDictionary)
            {
#endif
            if (!_enumsCacheDictionary.TryGetValue(enumType, out enumsCache))
            {
                enumsCache = (NonGenericEnumsCache)_openCreateMethod.MakeGenericMethod(enumType).Invoke(null, null);
#if NET20 || NET35
                _enumsCacheDictionary.Add(enumType, enumsCache);
#else
                _enumsCacheDictionary.TryAdd(enumType, enumsCache);
#endif
            }
#if NET20 || NET35
            }
#endif
            return enumsCache;
        }

        private static NonGenericEnumsCache Create<TEnum>()
        {
            var cache = Enums<TEnum>.Cache;
            var typeCode = Enums<TEnum>.TypeCode;
            var toEnum = Enums<TEnum>.ToEnum;
            Delegate newToEnum = null;
            Delegate newToInt = null;
            switch (typeCode)
            {
                case TypeCode.Int32:
                    newToEnum = (Func<int, object>)(value => ((Func<int, TEnum>)toEnum)(value));
                    newToInt = (Func<object, int>)(value =>
                    {
                        int result;
                        if (!((EnumsCache<int>)cache).TryToObject(value, out result, false))
                        {
                            throw new ArgumentException($"value is not of type {typeof(TEnum).Name}", nameof(value));
                        }
                        return result;
                    });
                    break;
                case TypeCode.UInt32:
                    newToEnum = (Func<uint, object>)(value => ((Func<uint, TEnum>)toEnum)(value));
                    newToInt = (Func<object, uint>)(value =>
                    {
                        uint result;
                        if (!((EnumsCache<uint>)cache).TryToObject(value, out result, false))
                        {
                            throw new ArgumentException($"value is not of type {typeof(TEnum).Name}", nameof(value));
                        }
                        return result;
                    });
                    break;
                case TypeCode.Int64:
                    newToEnum = (Func<long, object>)(value => ((Func<long, TEnum>)toEnum)(value));
                    newToInt = (Func<object, long>)(value =>
                    {
                        long result;
                        if (!((EnumsCache<long>)cache).TryToObject(value, out result, false))
                        {
                            throw new ArgumentException($"value is not of type {typeof(TEnum).Name}", nameof(value));
                        }
                        return result;
                    });
                    break;
                case TypeCode.UInt64:
                    newToEnum = (Func<ulong, object>)(value => ((Func<ulong, TEnum>)toEnum)(value));
                    newToInt = (Func<object, ulong>)(value =>
                    {
                        ulong result;
                        if (!((EnumsCache<ulong>)cache).TryToObject(value, out result, false))
                        {
                            throw new ArgumentException($"value is not of type {typeof(TEnum).Name}", nameof(value));
                        }
                        return result;
                    });
                    break;
                case TypeCode.SByte:
                    newToEnum = (Func<sbyte, object>)(value => ((Func<sbyte, TEnum>)toEnum)(value));
                    newToInt = (Func<object, sbyte>)(value =>
                    {
                        sbyte result;
                        if (!((EnumsCache<sbyte>)cache).TryToObject(value, out result, false))
                        {
                            throw new ArgumentException($"value is not of type {typeof(TEnum).Name}", nameof(value));
                        }
                        return result;
                    });
                    break;
                case TypeCode.Byte:
                    newToEnum = (Func<byte, object>)(value => ((Func<byte, TEnum>)toEnum)(value));
                    newToInt = (Func<object, byte>)(value =>
                    {
                        byte result;
                        if (!((EnumsCache<byte>)cache).TryToObject(value, out result, false))
                        {
                            throw new ArgumentException($"value is not of type {typeof(TEnum).Name}", nameof(value));
                        }
                        return result;
                    });
                    break;
                case TypeCode.Int16:
                    newToEnum = (Func<short, object>)(value => ((Func<short, TEnum>)toEnum)(value));
                    newToInt = (Func<object, short>)(value =>
                    {
                        short result;
                        if (!((EnumsCache<short>)cache).TryToObject(value, out result, false))
                        {
                            throw new ArgumentException($"value is not of type {typeof(TEnum).Name}", nameof(value));
                        }
                        return result;
                    });
                    break;
                case TypeCode.UInt16:
                    newToEnum = (Func<ushort, object>)(value => ((Func<ushort, TEnum>)toEnum)(value));
                    newToInt = (Func<object, ushort>)(value =>
                    {
                        ushort result;
                        if (!((EnumsCache<ushort>)cache).TryToObject(value, out result, false))
                        {
                            throw new ArgumentException($"value is not of type {typeof(TEnum).Name}", nameof(value));
                        }
                        return result;
                    });
                    break;
            }
            return new NonGenericEnumsCache(cache, newToEnum, newToInt, typeCode);
        }

        internal readonly object Cache;

        internal readonly Delegate ToEnum;

        internal readonly Delegate ToInt;

        internal readonly TypeCode TypeCode;

        private NonGenericEnumsCache(object cache, Delegate toEnum, Delegate toInt, TypeCode typeCode)
        {
            Cache = cache;
            ToEnum = toEnum;
            ToInt = toInt;
            TypeCode = typeCode;
        }
    }
}
