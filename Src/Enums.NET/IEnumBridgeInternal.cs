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

namespace EnumsNET
{
    internal interface IEnumBridgeInternal<TUnderlying, TUnderlyingOperations>
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        bool HasCustomValidator { get; }

        EnumMember CreateEnumMember(EnumMemberInternal<TUnderlying, TUnderlyingOperations> member);
        bool CustomValidate(TUnderlying value);
        bool IsEnum(object value);
        object ToObjectUnchecked(TUnderlying value);
        object CreateValuesContainer(IEnumerable<TUnderlying> values, int count, bool store = false);
        IReadOnlyList<EnumMember> CreateMembersContainer(IEnumerable<EnumMember> members, int count, bool store = false);
    }
}