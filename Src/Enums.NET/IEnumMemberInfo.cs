// Enums.NET
// Copyright 2015 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//	  http://www.apache.org/licenses/LICENSE-2.0
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
	internal interface IEnumMemberInfo : IFormattable, IConvertible, IComparable
	{
		object Value { get; }
		string Name { get; }
		Attribute[] Attributes { get; }
		string Description { get; }
		string EnumMemberValue { get; }
		object UnderlyingValue { get; }

		string GetDescriptionOrName();
		string GetDescriptionOrName(Func<string, string> nameFormatter);
		string ToString();
		string ToString(string format);
		string ToString(params EnumFormat[] formats);
		string AsString();
		string AsString(string format);
		string AsString(params EnumFormat[] formats);
		string Format(string format);
		string Format(params EnumFormat[] formats);
		bool HasAttribute<TAttribute>() where TAttribute : Attribute;
		TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute;
		TResult GetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, TResult defaultValue = default(TResult)) where TAttribute : Attribute;
		bool TryGetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, out TResult result) where TAttribute : Attribute;
		IEnumerable<TAttribute> GetAttributes<TAttribute>() where TAttribute : Attribute;
		sbyte ToSByte();
		byte ToByte();
		short ToInt16();
		ushort ToUInt16();
		int ToInt32();
		uint ToUInt32();
		long ToInt64();
		ulong ToUInt64();
	}
}