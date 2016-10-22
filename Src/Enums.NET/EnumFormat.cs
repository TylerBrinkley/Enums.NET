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
using System.ComponentModel;

#if ENUM_MEMBER_ATTRIBUTE
using System.Runtime.Serialization;
#endif

namespace EnumsNET
{
    /// <summary>
    /// Specifies the enum string representation formats.
    /// </summary>
    [EnumFormatValidator]
    public enum EnumFormat
    {
        /// <summary>
        /// Enum is represented by its decimal value.
        /// </summary>
        DecimalValue,
        /// <summary>
        /// Enum is represented by its hexadecimal value.
        /// </summary>
        HexadecimalValue,
        /// <summary>
        /// Enum is represented by its name.
        /// </summary>
        Name,
        /// <summary>
        /// Enum is represented by its <see cref="DescriptionAttribute.Description"/>.
        /// </summary>
        Description,
#if ENUM_MEMBER_ATTRIBUTE
        /// <summary>
        /// Enum is represented by its <see cref="EnumMemberAttribute.Value"/>.
        /// </summary>
        EnumMemberValue
#endif
    }

    internal sealed class EnumFormatValidatorAttribute : Attribute, IEnumValidatorAttribute<EnumFormat>
    {
        public bool IsValid(EnumFormat value) => Enums.EnumFormatIsValid(value);
    }
}
