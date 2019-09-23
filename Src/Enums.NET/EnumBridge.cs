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
using EnumsNET.Numerics;
using UnsafeUtility = System.Runtime.CompilerServices.Unsafe;

namespace EnumsNET
{
    internal interface IEnumBridge
    {
        bool HasCustomValidator { get; }

        EnumMember CreateEnumMember(EnumMemberInternal member);
        bool IsEnum(object value);
        IReadOnlyList<EnumMember> CreateMembersContainer(IEnumerable<EnumMemberInternal> members, int count, bool cached);
    }

    internal interface IEnumBridge<TUnderlying, TUnderlyingOperations> : IEnumBridge
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        bool CustomValidate(TUnderlying value);
        object ToObjectUnchecked(TUnderlying value);
        IValuesContainer CreateValuesContainer(IEnumerable<TUnderlying> values, int count, bool cached);
    }

    // Acts as a bridge in the reverse from the underlying type to the enum type
    // through the use of the implemented interface IEnumInfoInternal<TUnderlying, TUnderlyingOperations>.
    // Putting the logic in EnumCache<TUnderlying, TUnderlyingOperations> reduces memory usage
    // because having the enum type as a generic type parameter causes code explosion
    // due to how .NET generics are handled with enums.
    internal sealed class EnumBridge<TEnum, TUnderlying, TUnderlyingOperations> : IEnumBridge<TUnderlying, TUnderlyingOperations>
        where TEnum : struct, Enum
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        private readonly IEnumValidatorAttribute<TEnum>? _customEnumValidator = (IEnumValidatorAttribute<TEnum>?)Enums.GetInterfaceAttribute(typeof(TEnum), typeof(IEnumValidatorAttribute<TEnum>));

        public object ToObjectUnchecked(TUnderlying value) => UnsafeUtility.As<TUnderlying, TEnum>(ref value);

        public bool HasCustomValidator => _customEnumValidator != null;

        public bool CustomValidate(TUnderlying value) => _customEnumValidator!.IsValid(UnsafeUtility.As<TUnderlying, TEnum>(ref value));

        public EnumMember CreateEnumMember(EnumMemberInternal member) => new EnumMember<TEnum>(member);

        public bool IsEnum(object value) => value is TEnum || value is TEnum?;

        public IValuesContainer CreateValuesContainer(IEnumerable<TUnderlying> values, int count, bool cached) => new ValuesContainer<TEnum, TUnderlying>(values, count, cached);

        public IReadOnlyList<EnumMember> CreateMembersContainer(IEnumerable<EnumMemberInternal> members, int count, bool cached) => new MembersContainer<TEnum>(members, count, cached);
    }
}