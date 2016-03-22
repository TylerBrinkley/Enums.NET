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
        private readonly NonGenericEnumInfo _enumInfo;

        internal NonGenericEnumMemberInfo(IEnumMemberInfo info, NonGenericEnumInfo enumInfo)
            : base(info)
        {
            _enumInfo = enumInfo;
        }

        internal override object GetValue()
        {
            switch (_enumInfo.TypeCode)
            {
                case TypeCode.Int32:
                    return ((NonGenericEnumInfo<int>)_enumInfo).ToEnum(((InternalEnumMemberInfo<int>)_info).Value);
                case TypeCode.UInt32:
                    return ((NonGenericEnumInfo<uint>)_enumInfo).ToEnum(((InternalEnumMemberInfo<uint>)_info).Value);
                case TypeCode.Int64:
                    return ((NonGenericEnumInfo<long>)_enumInfo).ToEnum(((InternalEnumMemberInfo<long>)_info).Value);
                case TypeCode.UInt64:
                    return ((NonGenericEnumInfo<ulong>)_enumInfo).ToEnum(((InternalEnumMemberInfo<ulong>)_info).Value);
                case TypeCode.SByte:
                    return ((NonGenericEnumInfo<sbyte>)_enumInfo).ToEnum(((InternalEnumMemberInfo<sbyte>)_info).Value);
                case TypeCode.Byte:
                    return ((NonGenericEnumInfo<byte>)_enumInfo).ToEnum(((InternalEnumMemberInfo<byte>)_info).Value);
                case TypeCode.Int16:
                    return ((NonGenericEnumInfo<short>)_enumInfo).ToEnum(((InternalEnumMemberInfo<short>)_info).Value);
                case TypeCode.UInt16:
                    return ((NonGenericEnumInfo<ushort>)_enumInfo).ToEnum(((InternalEnumMemberInfo<ushort>)_info).Value);
            }
            Debug.Fail("Unknown Enum TypeCode");
            return null;
        }
    }
}
