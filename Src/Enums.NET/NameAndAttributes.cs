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

namespace EnumsNET
{
    internal struct NameAndAttributes : IEquatable<NameAndAttributes>
    {
        public readonly string Name;
        // Guaranteed to be null or not empty
        public readonly Attribute[] Attributes;

        public NameAndAttributes(string name, Attribute[] attributes = null)
        {
            Name = name;
            Attributes = attributes?.Length > 0 ? attributes : null;
        }

        public override int GetHashCode() => Name.GetHashCode();

        public override bool Equals(object obj) => obj is NameAndAttributes && Equals((NameAndAttributes)obj);

        // Is case sensitive
        public bool Equals(NameAndAttributes other) => Name == other.Name;
    }
}
