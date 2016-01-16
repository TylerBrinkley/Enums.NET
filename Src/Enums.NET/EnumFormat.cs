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

using System.ComponentModel;

namespace EnumsNET
{
	/// <summary>
	/// Defines the various enum formats
	/// </summary>
	public enum EnumFormat
	{
		/// <summary>
		/// Enum is represented by its decimal value
		/// </summary>
		DecimalValue,
		/// <summary>
		/// Enum is represented by its hexadecimal value
		/// </summary>
		HexadecimalValue,
		/// <summary>
		/// Enum is represented by its name
		/// </summary>
		Name,
		/// <summary>
		/// Enum is represented by its <see cref="DescriptionAttribute.Description"/>
		/// </summary>
		Description
	}
}
