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
using System.Linq;
using EnumsNET.Utilities;

namespace EnumsNET
{
    internal sealed class MembersContainer<TEnum> : IReadOnlyList<EnumMember<TEnum>>
        where TEnum : struct, Enum
    {
        private readonly IEnumerable<EnumMember> _members;
        private EnumMember<TEnum>[]? _membersArray;

        public int Count { get; }

        public EnumMember<TEnum> this[int index] => (_membersArray ??= this.ToArray())[index];

        public MembersContainer(IEnumerable<EnumMember> members, int count)
        {
            _members = members;
            Count = count;
        }

        public IEnumerator<EnumMember<TEnum>> GetEnumerator() => _membersArray != null ? ((IEnumerable<EnumMember<TEnum>>)_membersArray).GetEnumerator() : Enumerate();

        private IEnumerator<EnumMember<TEnum>> Enumerate()
        {
            foreach (var member in _members)
            {
                yield return UnsafeUtility.As<EnumMember<TEnum>>(member);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}