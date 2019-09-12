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
using System.Threading;
using EnumsNET.Utilities;

namespace EnumsNET.Unsafe
{
    /// <summary>
    /// A type-unsafe version of <see cref="EnumComparer{TEnum}"/> which is useful when dealing with generics
    /// and instead throws an <see cref="ArgumentException"/> if TEnum is not an enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    public sealed class UnsafeEnumComparer<TEnum> : IEqualityComparer<TEnum>, IComparer<TEnum>, IEqualityComparer, IComparer
    {
        private static UnsafeEnumComparer<TEnum>? s_instance;

        /// <summary>
        /// The singleton instance of <see cref="UnsafeEnumComparer{TEnum}"/>.
        /// </summary>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static UnsafeEnumComparer<TEnum> Instance
        {
            get
            {
                var instance = s_instance;
                return instance ?? Interlocked.CompareExchange(ref s_instance, (instance = new UnsafeEnumComparer<TEnum>()), null) ?? instance;
            }
        }

        /// <summary>
        /// <see cref="UnsafeEnumComparer{TEnum}"/> constructor, should use singleton property <see cref="Instance"/> instead.
        /// This constructor's public for visibility and serialization.
        /// </summary>
        public UnsafeEnumComparer()
        {
            UnsafeEnums.GetCache<TEnum>(); // Validates TEnum is an enum
        }

        /// <summary>
        /// Indicates if <paramref name="x"/> equals <paramref name="y"/> without boxing the values.
        /// </summary>
        /// <param name="x">The first enum value.</param>
        /// <param name="y">The second enum value.</param>
        /// <returns>Indication if <paramref name="x"/> equals <paramref name="y"/> without boxing the values.</returns>
        public bool Equals(TEnum x, TEnum y) => UnsafeEnums<TEnum>.Cache!.Equals(ref UnsafeUtility.As<TEnum, byte>(ref x), ref UnsafeUtility.As<TEnum, byte>(ref y));

        /// <summary>
        /// Retrieves a hash code for <paramref name="obj"/> without boxing the value.
        /// </summary>
        /// <param name="obj">The enum value.</param>
        /// <returns>Hash code for <paramref name="obj"/> without boxing the value.</returns>
        public int GetHashCode(TEnum obj) => UnsafeEnums<TEnum>.Cache!.GetHashCode(ref UnsafeUtility.As<TEnum, byte>(ref obj));

        /// <summary>
        /// Compares <paramref name="x"/> to <paramref name="y"/> without boxing the values.
        /// </summary>
        /// <param name="x">The first enum value.</param>
        /// <param name="y">The second enum value.</param>
        /// <returns>1 if <paramref name="x"/> is greater than <paramref name="y"/>, 0 if <paramref name="x"/> equals <paramref name="y"/>,
        /// and -1 if <paramref name="x"/> is less than <paramref name="y"/>.</returns>
        public int Compare(TEnum x, TEnum y) => UnsafeEnums<TEnum>.Cache!.CompareTo(ref UnsafeUtility.As<TEnum, byte>(ref x), ref UnsafeUtility.As<TEnum, byte>(ref y));

        #region Explicit Interface Implementation
        bool IEqualityComparer.Equals(object? x, object? y) => x is TEnum xEnum && y is TEnum yEnum && Equals(xEnum, yEnum);

        int IEqualityComparer.GetHashCode(object? obj) => obj is TEnum objEnum ? GetHashCode(objEnum) : 0;

        int IComparer.Compare(object? x, object? y) => x is TEnum xEnum && y is TEnum yEnum ? Compare(xEnum, yEnum) : 0;
        #endregion
    }
}
