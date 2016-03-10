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
using System.Diagnostics;

namespace EnumsNET.NonGeneric
{
    internal sealed class NonGenericEnumMemberInfo : EnumMemberInfo
    {
        private readonly NonGenericEnumsCache _enumsCache;

        internal NonGenericEnumMemberInfo(IEnumMemberInfo info, NonGenericEnumsCache enumsCache)
            : base(info)
        {
            _enumsCache = enumsCache;
        }

        internal override object GetValue()
        {
            var toEnum = _enumsCache.ToEnum;
            switch (_enumsCache.TypeCode)
            {
                case TypeCode.Int32:
                    return ((Func<int, object>)toEnum)(((InternalEnumMemberInfo<int>)_info).Value);
                case TypeCode.UInt32:
                    return ((Func<uint, object>)toEnum)(((InternalEnumMemberInfo<uint>)_info).Value);
                case TypeCode.Int64:
                    return ((Func<long, object>)toEnum)(((InternalEnumMemberInfo<long>)_info).Value);
                case TypeCode.UInt64:
                    return ((Func<ulong, object>)toEnum)(((InternalEnumMemberInfo<ulong>)_info).Value);
                case TypeCode.SByte:
                    return ((Func<sbyte, object>)toEnum)(((InternalEnumMemberInfo<sbyte>)_info).Value);
                case TypeCode.Byte:
                    return ((Func<byte, object>)toEnum)(((InternalEnumMemberInfo<byte>)_info).Value);
                case TypeCode.Int16:
                    return ((Func<short, object>)toEnum)(((InternalEnumMemberInfo<short>)_info).Value);
                case TypeCode.UInt16:
                    return ((Func<ushort, object>)toEnum)(((InternalEnumMemberInfo<ushort>)_info).Value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }
    }
}
