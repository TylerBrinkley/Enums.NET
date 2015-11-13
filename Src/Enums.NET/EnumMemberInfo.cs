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
using System.ComponentModel;
using System.Runtime.Serialization;

namespace EnumsNET
{
    /// <summary>
    /// Class that provides efficient defined enum value operations
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public class EnumMemberInfo<TEnum>
    {
        /// <summary>
        /// The defined enum member's value
        /// </summary>
        public TEnum Value { get; }

        /// <summary>
        /// The defined enum member's name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The defined enum member's attributes
        /// </summary>
        public Attribute[] Attributes { get; }

        /// <summary>
        /// The defined enum member's <see cref="DescriptionAttribute.Description"/> if applied else null.
        /// </summary>
        public string Description => Enums.GetDescription(Attributes);

        /// <summary>
        /// The defined enum member's <see cref="EnumMemberAttribute.Value"/> if applied else null.
        /// </summary>
        public string EnumMemberValue => Enums.GetEnumMemberValue(Attributes);

        internal EnumMemberInfo(TEnum value, string name, Attribute[] attributes)
        {
            Value = value;
            Name = name;
            Attributes = attributes;
        }

        /// <summary>
        /// Retrieves the <see cref="Description"/> if not null else the <see cref="Name"/>.
        /// </summary>
        /// <returns></returns>
        public string GetDescriptionOrName() => Description ?? Name;

        /// <summary>
        /// Retrieves the <see cref="Description"/> if not null else the <see cref="Name"/> that's been formatted with <paramref name="nameFormatter"/>.
        /// </summary>
        /// <param name="nameFormatter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="nameFormatter"/> is null</exception>
        public string GetDescriptionOrName(Func<string, string> nameFormatter)
        {
            Preconditions.NotNull(nameFormatter, nameof(nameFormatter));

            return Description ?? nameFormatter(Name);
        }

        /// <summary>
        /// Retrieves the first <typeparamref name="TAttribute"/> in <see cref="Attributes"/> if defined else null.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public TAttribute GetAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            return Enums.GetAttribute<TAttribute>(Attributes);
        }

        /// <summary>
        /// Retrieves all <typeparamref name="TAttribute"/>'s in <see cref="Attributes"/>.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public TAttribute[] GetAttributes<TAttribute>()
            where TAttribute : Attribute
        {
            return Enums.GetAttributes<TAttribute>(Attributes);
        }
    }
}
