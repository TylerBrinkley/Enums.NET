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

		public override bool Equals(object obj) => obj is NameAndAttributes ? Equals((NameAndAttributes)obj) : false;

		// Is case sensitive
		public bool Equals(NameAndAttributes other) => Name == other.Name;
	}
}
