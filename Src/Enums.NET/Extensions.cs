// Enums.NET
// Copyright 2016 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace EnumsNET
{
    internal static class Extensions
    {
        public static T[] Copy<T>(this T[] array)
        {
            if (array == null)
            {
                return null;
            }
            var copiedArray = new T[array.Length];
            array.CopyTo(copiedArray, 0);
            return copiedArray;
        }
    }
}
