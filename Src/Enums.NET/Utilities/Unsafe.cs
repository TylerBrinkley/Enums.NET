﻿#region License
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

using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace System.Runtime.CompilerServices
{
    internal static class UnsafeUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static int SizeOf<T>() => Unsafe.SizeOf<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static ref TTo As<TFrom, TTo>(ref TFrom source) => ref Unsafe.As<TFrom, TTo>(ref source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        [return: NotNullIfNotNull("value")]
        public static T? As<T>(object? value) where T : class => Unsafe.As<T>(value);
    }
}