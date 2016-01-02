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
using System.Collections.Generic;

namespace EnumsNET
{
	internal struct InternalEnumMemberInfo<TEnum>
	{
		public readonly TEnum Value;
		public readonly string Name;
		private readonly Attribute[] _attributes;

		public Attribute[] Attributes
		{
			get
			{
				if (Name == null)
				{
					return null;
				}
				if (_attributes == null)
				{
					return Enums.ZeroLengthAttributes;
				}
				return _attributes.Copy();
			}
		}

		public string Description => _attributes != null ? Enums.GetDescription(_attributes) : null;

		public string EnumMemberValue => _attributes != null ? Enums.GetEnumMemberValue(_attributes) : null;

		public InternalEnumMemberInfo(Pair<TEnum, NameAndAttributes> pair)
			: this(pair.First, pair.Second.Name, pair.Second.Attributes)
		{
		}

		public InternalEnumMemberInfo(KeyValuePair<string, ValueAndAttributes<TEnum>> pair)
			: this(pair.Value.Value, pair.Key, pair.Value.Attributes)
		{
		}

		public InternalEnumMemberInfo(string name, ValueAndAttributes<TEnum> valueAndAttributes)
			: this(valueAndAttributes.Value, name, valueAndAttributes.Attributes)
		{
		}

		public InternalEnumMemberInfo(TEnum value, string name, Attribute[] attributes)
		{
			Value = value;
			Name = name;
			_attributes = attributes;
		}

		public string GetDescriptionOrName() => Description ?? Name;

		public string GetDescriptionOrName(Func<string, string> nameFormatter) => Name != null ? (Description ?? (nameFormatter != null ? nameFormatter(Name) : Name)) : null;

		public TAttribute GetAttribute<TAttribute>()
			where TAttribute : Attribute
		{
			return _attributes != null ? Enums.GetAttribute<TAttribute>(_attributes) : null;
		}

		public IEnumerable<TAttribute> GetAttributes<TAttribute>()
			where TAttribute : Attribute
		{
			return Name != null ? (_attributes != null ? Enums.GetAttributes<TAttribute>(_attributes) : new TAttribute[0]) : null;
		}

		public EnumMemberInfo<TEnum> ToEnumMemberInfo() => Name != null ? new EnumMemberInfo<TEnum>(Value, Name, _attributes) : null;
	}
}
