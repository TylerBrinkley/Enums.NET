#region License
// Copyright (c) 2016 Tyler Brinkley
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EnumsNET.Numerics;
using EnumsNET.Utilities;

namespace EnumsNET
{
    internal abstract class EnumBridge<TEnum>
    {
        public abstract TEnum CombineFlags(IEnumerable<TEnum>? flags);
        public abstract EnumCache GetCache();
    }

    // Class that acts as a bridge from the enum type to the underlying type
    // through the use of the implemented interfaces IEnumInfo<TEnum> and IEnumInfo.
    // Also acts as a bridge in the reverse from the underlying type to the enum type
    // through the use of the implemented interface IEnumInfoInternal<TUnderlying, TUnderlyingOperations>.
    // Putting the logic in EnumCache<TUnderlying, TUnderlyingOperations> reduces memory usage
    // because having the enum type as a generic type parameter causes code explosion
    // due to how .NET generics are handled with enums.
    internal sealed class EnumBridge<TEnum, TUnderlying, TUnderlyingOperations> : EnumBridge<TEnum>, IEnumBridgeInternal<TUnderlying, TUnderlyingOperations>
        where TEnum : struct, Enum
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        private readonly EnumCache<TUnderlying, TUnderlyingOperations> _cache;
        private readonly IEnumValidatorAttribute<TEnum>? _customEnumValidator = (IEnumValidatorAttribute<TEnum>?)Enums.GetInterfaceAttribute(typeof(TEnum), typeof(IEnumValidatorAttribute<TEnum>));

        public EnumBridge()
        {
            _cache = new EnumCache<TUnderlying, TUnderlyingOperations>(typeof(TEnum), this);
        }

        public override TEnum CombineFlags(IEnumerable<TEnum>? flags)
        {
            TUnderlying result = default;
            TUnderlyingOperations operations = default;
            if (flags != null)
            {
                TEnum value;
                foreach (var flag in flags)
                {
                    value = flag;
                    result = operations.Or(result, UnsafeUtility.As<TEnum, TUnderlying>(ref value));
                }
            }
            return UnsafeUtility.As<TUnderlying, TEnum>(ref result);
        }

        public override EnumCache GetCache() => _cache;

        public object ToObjectUnchecked(TUnderlying value) => UnsafeUtility.As<TUnderlying, TEnum>(ref value);

        public bool HasCustomValidator => _customEnumValidator != null;

        public bool CustomValidate(TUnderlying value) => _customEnumValidator!.IsValid(UnsafeUtility.As<TUnderlying, TEnum>(ref value));

        public EnumMember CreateEnumMember(EnumMemberInternal<TUnderlying, TUnderlyingOperations> member) => new EnumMember<TEnum, TUnderlying, TUnderlyingOperations>(member);

        public bool IsEnum(object value) => value is TEnum || value is TEnum?;

        public object CreateValuesContainer(IEnumerable<TUnderlying> values, int count, bool store = false)
        {
            if (store)
            {
                var a = new TEnum[count];
                var i = 0;
                foreach (var value in values)
                {
                    var v = value;
                    a[i++] = UnsafeUtility.As<TUnderlying, TEnum>(ref v);
                }
                return new ReadOnlyCollection<TEnum>(a);
            }
            return new ValuesContainer<TEnum, TUnderlying>(values, count);
        }

        public IReadOnlyList<EnumMember> CreateMembersContainer(IEnumerable<EnumMember> members, int count, bool store = false)
        {
            if (count == 0)
            {
                return ArrayHelper.Empty<EnumMember<TEnum>>();
            }
            if (store)
            {
                var a = new EnumMember<TEnum>[count];
                var i = 0;
                foreach (var member in members)
                {
                    a[i++] = UnsafeUtility.As<EnumMember<TEnum>>(member);
                }
                return new ReadOnlyCollection<EnumMember<TEnum>>(a);
            }
            return new MembersContainer<TEnum>(members, count);
        }
    }
}