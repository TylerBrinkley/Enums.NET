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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using EnumsNET.Numerics;
using EnumsNET.Utilities;

namespace EnumsNET
{
    internal interface IValuesContainer
    {
        IReadOnlyList<object> GetNonGenericContainer();
    }

    internal sealed class ValuesContainer<TEnum, TUnderlying, TUnderlyingOperations> : IReadOnlyList<TEnum>, IValuesContainer
        where TEnum : struct, Enum
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        private readonly IEnumerable<EnumMemberInternal<TUnderlying, TUnderlyingOperations>> _members;
        private TEnum[]? _valuesArray;
        private IReadOnlyList<object>? _nonGenericValuesContainer;

        public int Count { get; }

        public TEnum this[int index] => (_valuesArray ??= ArrayHelper.ToArray(this, Count))[index];

        public ValuesContainer(IEnumerable<EnumMemberInternal<TUnderlying, TUnderlyingOperations>> members, int count, bool cached)
        {
            Debug.Assert(count == members.Count());
            _members = members;
            Count = count;
            if (cached)
            {
                _valuesArray = ArrayHelper.ToArray(this, count);
            }
        }

        public IEnumerator<TEnum> GetEnumerator() => _valuesArray != null ? ((IEnumerable<TEnum>)_valuesArray).GetEnumerator() : Enumerate();

        private IEnumerator<TEnum> Enumerate()
        {
            foreach (var member in _members)
            {
                var v = member.Value;
                yield return UnsafeUtility.As<TUnderlying, TEnum>(ref v);
            }
        }

        public IReadOnlyList<object> GetNonGenericContainer()
        {
            var nonGenericValuesContainer = _nonGenericValuesContainer;
            return nonGenericValuesContainer ?? Interlocked.CompareExchange(ref _nonGenericValuesContainer, (nonGenericValuesContainer = new NonGenericValuesContainer<TEnum, TUnderlying, TUnderlyingOperations>(this)), null) ?? nonGenericValuesContainer;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal sealed class NonGenericValuesContainer<TEnum, TUnderlying, TUnderlyingOperations> : IReadOnlyList<object>
        where TEnum : struct, Enum
        where TUnderlying : struct, IComparable<TUnderlying>, IEquatable<TUnderlying>
#if ICONVERTIBLE
        , IConvertible
#endif
        where TUnderlyingOperations : struct, IUnderlyingOperations<TUnderlying>
    {
        private readonly ValuesContainer<TEnum, TUnderlying, TUnderlyingOperations> _container;

        public object this[int index] => _container[index];

        public int Count => _container.Count;

        public NonGenericValuesContainer(ValuesContainer<TEnum, TUnderlying, TUnderlyingOperations> container)
        {
            _container = container;
        }

        public IEnumerator<object> GetEnumerator()
        {
            foreach (var value in _container)
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}