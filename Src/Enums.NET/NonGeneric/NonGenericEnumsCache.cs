// Enums.NET
// Copyright 2016 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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

        internal static NonGenericEnumsCache Get(Type enumType, OptionalOutParameter<bool> isNullable = null)
        {
            Preconditions.NotNull(enumType, nameof(enumType));
            if (!enumType.IsEnum)
            {
                enumType = Nullable.GetUnderlyingType(enumType);
                if (enumType?.IsEnum != true)
                {
                    throw new ArgumentException("must be an enum type", nameof(enumType));
                }
                else if (isNullable != null)
                {
                    isNullable.Result = true;
                }
            }
            else if (isNullable != null)
            {
                isNullable.Result = false;
            }

            NonGenericEnumsCache enumsCache;
#if NET20 || NET35
            lock (_enumsCacheDictionary)
            {
#endif
            if (!_enumsCacheDictionary.TryGetValue(enumType, out enumsCache))
            {
                var underlyingType = Enum.GetUnderlyingType(enumType);
                enumsCache = (NonGenericEnumsCache)_openCreateMethod.MakeGenericMethod(enumType, underlyingType).Invoke(null, new object[] { enumType.Name, Type.GetTypeCode(underlyingType) });
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

        private static bool IsEnum<TEnum>(object value) where TEnum : struct => value is TEnum || value is TEnum?;

        private static NonGenericEnumsCache Create<TEnum, TInt>(string enumName, TypeCode typeCode)
            where TEnum : struct
            where TInt : struct
        {
            Delegate toEnum = Enums<TEnum, TInt>.ToEnum;
            Func<object, bool> isEnum = IsEnum<TEnum>;
            return new NonGenericEnumsCache(Enums<TEnum, TInt>.Cache,
                (Func<TInt, object>)(value => ((Func<TInt, TEnum>)toEnum)(value)),
                (Func<object, TInt>)(value =>
                {
                    if (isEnum(value))
                    {
                        return (TInt)value;
                    }
                    throw new ArgumentException($"value is not of type {enumName}", nameof(value));
                }),
                typeCode,
                Enums.InternalRegisterCustomEnumFormat<TEnum>);
        }

        internal readonly object Cache;

        internal readonly Delegate ToEnum;

        internal readonly Delegate ToInt;

        internal readonly TypeCode TypeCode;

        internal readonly Func<Func<EnumMemberInfo, string>, EnumFormat> RegisterCustomEnumFormat;

        private NonGenericEnumsCache(object cache, Delegate toEnum, Delegate toInt, TypeCode typeCode, Func<Func<EnumMemberInfo, string>, EnumFormat> registerCustomEnumFormat)
        {
            Cache = cache;
            ToEnum = toEnum;
            ToInt = toInt;
            TypeCode = typeCode;
            RegisterCustomEnumFormat = registerCustomEnumFormat;
        }
    }
}
