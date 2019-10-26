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
using System.Runtime.CompilerServices;
using EnumsNET.Numerics;

namespace EnumsNET
{
    internal interface IEnumBridge
    {
        EnumComparer CreateEnumComparer(EnumCache enumCache);
        EnumMember CreateEnumMember(EnumMemberInternal member);
        IReadOnlyList<EnumMember> CreateMembersContainer(IEnumerable<EnumMemberInternal> members, int count, bool cached);
    }

    internal interface IEnumBridge<TUnderlying, TUnderlyingOperations> : IEnumBridge
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        bool CustomValidate(object customValidator, TUnderlying value);
        TUnderlying? IsEnum(object value);
        object ToObjectUnchecked(TUnderlying value);
        IValuesContainer CreateValuesContainer(IEnumerable<EnumMemberInternal<TUnderlying, TUnderlyingOperations>> members, int count, bool cached);
    }

    // Acts as a bridge in the reverse from the underlying type to the enum type
    // through the use of the implemented interface IEnumBridge<TUnderlying, TUnderlyingOperations>.
    internal sealed class EnumBridge<TEnum, TUnderlying, TUnderlyingOperations> : IEnumBridge<TUnderlying, TUnderlyingOperations>
        where TEnum : struct, Enum
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        public object ToObjectUnchecked(TUnderlying value) => UnsafeUtility.As<TUnderlying, TEnum>(ref value);

        public bool CustomValidate(object customValidator, TUnderlying value) => UnsafeUtility.As<IEnumValidatorAttribute<TEnum>>(customValidator).IsValid(UnsafeUtility.As<TUnderlying, TEnum>(ref value));

        public EnumComparer CreateEnumComparer(EnumCache enumCache) => new EnumComparer<TEnum>(enumCache);

        public EnumMember CreateEnumMember(EnumMemberInternal member) => new EnumMember<TEnum>(member);

        public TUnderlying? IsEnum(object value) => value is TEnum e ? UnsafeUtility.As<TEnum, TUnderlying>(ref e) : (TUnderlying?)null;

        public IValuesContainer CreateValuesContainer(IEnumerable<EnumMemberInternal<TUnderlying, TUnderlyingOperations>> members, int count, bool cached) => new ValuesContainer<TEnum, TUnderlying, TUnderlyingOperations>(members, count, cached);

        public IReadOnlyList<EnumMember> CreateMembersContainer(IEnumerable<EnumMemberInternal> members, int count, bool cached) => new MembersContainer<TEnum>(members, count, cached);
    }
}