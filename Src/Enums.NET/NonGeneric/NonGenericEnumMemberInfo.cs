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
