// Enums.NET
// Copyright 2015 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

namespace EnumsNET
{
	internal static class Preconditions
	{
		public static void NotNull<T>(T value, string paramName)
			where T : class
		{
			if (value == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		public static void NotNullOrEmpty(string value, string paramName)
		{
			NotNull(value, paramName);
			if (value.Length == 0)
			{
				throw new ArgumentException("cannot be empty", paramName);
			}
		}

		public static void NotNullOrEmpty<T>(T[] array, string paramName)
		{
			NotNull(array, paramName);
			if (array.Length == 0)
			{
				throw new ArgumentException("cannot be empty", paramName);
			}
		}

		public static void GreaterThanOrEqual(int value, string paramName, int comparisonValue)
		{
			if (value < comparisonValue)
			{
				throw new ArgumentOutOfRangeException(paramName, $"must be greater than or equal to {comparisonValue}");
			}
		}
	}
}