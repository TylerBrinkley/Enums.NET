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
using System.Runtime.CompilerServices;

namespace EnumsNET
{
    /// <summary>
    /// An efficient enum comparer.
    /// </summary>
    public abstract class EnumComparer : IEqualityComparer, IComparer
    {
        /// <summary>
        /// Gets a singleton instance of <see cref="EnumComparer"/> for the enum type provided.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <returns>A singleton instance of <see cref="EnumComparer"/> for the enum type provided.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> is not an enum type.</exception>
        public static EnumComparer GetInstance(Type enumType) => Enums.GetCache(enumType).EnumComparer;

        private protected readonly EnumCache _enumCache;

        private protected EnumComparer(EnumCache enumCache)
        {
            _enumCache = enumCache;
        }

        /// <summary>
        /// Indicates if <paramref name="x"/> equals <paramref name="y"/> without boxing the values.
        /// </summary>
        /// <param name="x">The first enum value.</param>
        /// <param name="y">The second enum value.</param>
        /// <returns>Indication if <paramref name="x"/> equals <paramref name="y"/> without boxing the values.</returns>
        public new bool Equals(object? x, object? y) => x is object ? (y is object && _enumCache.Equals(x, y)) : y is null;

        /// <summary>
        /// Retrieves a hash code for <paramref name="obj"/> without boxing the value.
        /// </summary>
        /// <param name="obj">The enum value.</param>
        /// <returns>Hash code for <paramref name="obj"/> without boxing the value.</returns>
        public int GetHashCode(object? obj) => obj is object ? _enumCache.GetHashCode(obj) : 0;

        /// <summary>
        /// Compares <paramref name="x"/> to <paramref name="y"/> without boxing the values.
        /// </summary>
        /// <param name="x">The first enum value.</param>
        /// <param name="y">The second enum value.</param>
        /// <returns>1 if <paramref name="x"/> is greater than <paramref name="y"/>, 0 if <paramref name="x"/> equals <paramref name="y"/>,
        /// and -1 if <paramref name="x"/> is less than <paramref name="y"/>.</returns>
        public int Compare(object? x, object? y) => x is object ? (y is object ? _enumCache.CompareTo(x, y) : 1) : (y is null ? 0 : -1);
    }

    /// <summary>
    /// An efficient enum comparer which doesn't box the values.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    public sealed class EnumComparer<TEnum> : EnumComparer, IEqualityComparer<TEnum>, IComparer<TEnum>
    {
        /// <summary>
        /// The singleton instance of <see cref="EnumComparer{TEnum}"/>. 
        /// </summary>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static EnumComparer<TEnum> Instance => UnsafeUtility.As<EnumComparer<TEnum>>(Enums.GetCacheUnsafe<TEnum>().EnumComparer);

        internal EnumComparer(EnumCache enumCache)
            : base(enumCache)
        {
        }

        /// <summary>
        /// Indicates if <paramref name="x"/> equals <paramref name="y"/> without boxing the values.
        /// </summary>
        /// <param name="x">The first enum value.</param>
        /// <param name="y">The second enum value.</param>
        /// <returns>Indication if <paramref name="x"/> equals <paramref name="y"/> without boxing the values.</returns>
        public bool Equals(TEnum x, TEnum y) => _enumCache.Equals(ref UnsafeUtility.As<TEnum, byte>(ref x), ref UnsafeUtility.As<TEnum, byte>(ref y));

        /// <summary>
        /// Retrieves a hash code for <paramref name="obj"/> without boxing the value.
        /// </summary>
        /// <param name="obj">The enum value.</param>
        /// <returns>Hash code for <paramref name="obj"/> without boxing the value.</returns>
        public int GetHashCode(TEnum obj) => _enumCache.GetHashCode(ref UnsafeUtility.As<TEnum, byte>(ref obj));

        /// <summary>
        /// Compares <paramref name="x"/> to <paramref name="y"/> without boxing the values.
        /// </summary>
        /// <param name="x">The first enum value.</param>
        /// <param name="y">The second enum value.</param>
        /// <returns>1 if <paramref name="x"/> is greater than <paramref name="y"/>, 0 if <paramref name="x"/> equals <paramref name="y"/>,
        /// and -1 if <paramref name="x"/> is less than <paramref name="y"/>.</returns>
        public int Compare(TEnum x, TEnum y) => _enumCache.CompareTo(ref UnsafeUtility.As<TEnum, byte>(ref x), ref UnsafeUtility.As<TEnum, byte>(ref y));
    }
}