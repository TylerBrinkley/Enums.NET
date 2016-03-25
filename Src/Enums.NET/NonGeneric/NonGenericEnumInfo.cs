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
    internal abstract class NonGenericEnumInfo
    {
#if NET20 || NET35
        private static readonly Dictionary<Type, NonGenericEnumInfo> _enumInfosDictionary = new Dictionary<Type, NonGenericEnumInfo>();
#else
        private static readonly ConcurrentDictionary<Type, NonGenericEnumInfo> _enumInfosDictionary = new ConcurrentDictionary<Type, NonGenericEnumInfo>();
#endif
        private static readonly MethodInfo _openCreateMethod = typeof(NonGenericEnumInfo).GetMethod(nameof(Create), BindingFlags.Static | BindingFlags.NonPublic);

        internal static NonGenericEnumInfo Get(Type enumType, OptionalOutParameter<bool> isNullable = null)
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

            NonGenericEnumInfo enumInfo;
#if NET20 || NET35
            lock (_enumInfosDictionary)
            {
#endif
            if (!_enumInfosDictionary.TryGetValue(enumType, out enumInfo))
            {
                var underlyingType = Enum.GetUnderlyingType(enumType);
                enumInfo = (NonGenericEnumInfo)_openCreateMethod.MakeGenericMethod(enumType, underlyingType).Invoke(null, new object[] { enumType.Name, Type.GetTypeCode(underlyingType) });
#if NET20 || NET35
                _enumInfosDictionary.Add(enumType, enumInfo);
#else
                _enumInfosDictionary.TryAdd(enumType, enumInfo);
#endif
            }
#if NET20 || NET35
            }
#endif
            return enumInfo;
        }

        private static NonGenericEnumInfo Create<TEnum, TInt>(string enumName, TypeCode typeCode)
            where TInt : struct
        {
            Delegate toEnum = Enums<TEnum, TInt>.ToEnum;
            return new NonGenericEnumInfo<TInt>(Enums<TEnum, TInt>.Cache,
                value => ((Func<TInt, TEnum>)toEnum)(value),
                typeCode,
                Enums.InternalRegisterCustomEnumFormat<TEnum>);
        }

        internal readonly TypeCode TypeCode;

        internal readonly Func<Func<EnumMemberInfo, string>, EnumFormat> RegisterCustomEnumFormat;

        internal NonGenericEnumInfo(TypeCode typeCode, Func<Func<EnumMemberInfo, string>, EnumFormat> registerCustomEnumFormat)
        {
            TypeCode = typeCode;
            RegisterCustomEnumFormat = registerCustomEnumFormat;
        }
    }

    internal class NonGenericEnumInfo<TInt> : NonGenericEnumInfo
        where TInt : struct
    {
        internal readonly EnumsCache<TInt> Cache;

        internal readonly Func<TInt, object> ToEnum;

        internal NonGenericEnumInfo(EnumsCache<TInt> cache, Func<TInt, object> toEnum, TypeCode typeCode, Func<Func<EnumMemberInfo, string>, EnumFormat> registerCustomEnumFormat)
            : base(typeCode, registerCustomEnumFormat)
        {
            Cache = cache;
            ToEnum = toEnum;
        }
    }
}
