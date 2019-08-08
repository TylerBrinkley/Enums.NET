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

namespace EnumsNET.NonGeneric
{
    /// <summary>
    /// A non-generic enum comparer.
    /// </summary>
    public sealed class NonGenericEnumComparer : IEqualityComparer, IComparer
    {
        private readonly NonGenericEnumInfo _info;

        /// <summary>
        /// The <see cref="NonGenericEnumComparer"/> constructor.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public NonGenericEnumComparer(Type enumType)
        {
            _info = NonGenericEnums.GetNonGenericEnumInfo(enumType);
        }

        /// <summary>
        /// Compares <paramref name="x"/> to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first enum value.</param>
        /// <param name="y">The second enum value.</param>
        /// <returns>1 if <paramref name="x"/> is greater than <paramref name="y"/>, 0 if <paramref name="x"/> equals <paramref name="y"/>,
        /// and -1 if <paramref name="x"/> is less than <paramref name="y"/>.</returns>
        public int Compare(object? x, object? y) => NonGenericEnums.CompareToInternal(_info, x, y);

        /// <summary>
        /// Indicates if <paramref name="x"/> equals <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first enum value.</param>
        /// <param name="y">The second enum value.</param>
        /// <returns>Indication if <paramref name="x"/> equals <paramref name="y"/>.</returns>
        public new bool Equals(object? x, object? y) => NonGenericEnums.EqualsInternal(_info, x, y);

        /// <summary>
        /// Retrieves a hash code for <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The enum value.</param>
        /// <returns>Hash code for <paramref name="obj"/>.</returns>
        public int GetHashCode(object obj) => obj?.GetHashCode() ?? 0;
    }
}