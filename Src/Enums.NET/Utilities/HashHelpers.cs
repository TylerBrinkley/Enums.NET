// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace EnumsNET.Utilities
{
    internal static partial class HashHelpers
    {
        public static int PowerOf2(int v)
        {
            if ((v & (v - 1)) == 0 && v >= 2)
            {
                return v;
            }

            int i = 2;
            while (i < v)
            {
                i <<= 1;
            }

            return i;
        }
    }
}