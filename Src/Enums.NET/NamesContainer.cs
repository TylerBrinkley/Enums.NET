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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EnumsNET.Utilities;

namespace EnumsNET
{
    internal sealed class NamesContainer : IReadOnlyList<string>
    {
        private readonly IEnumerable<EnumMemberInternal> _members;
        private string[]? _namesArray;

        public int Count { get; }

        public string this[int index] => (_namesArray ??= ArrayHelper.ToArray(this, Count))[index];

        public NamesContainer(IEnumerable<EnumMemberInternal> members, int count, bool cached)
        {
            Debug.Assert(count == members.Count());
            _members = members;
            Count = count;
            if (cached)
            {
                _namesArray = ArrayHelper.ToArray(this, count);
            }
        }

        public IEnumerator<string> GetEnumerator() => _namesArray != null ? ((IEnumerable<string>)_namesArray).GetEnumerator() : Enumerate();

        private IEnumerator<string> Enumerate()
        {
            foreach (var member in _members)
            {
                yield return member.Name;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}