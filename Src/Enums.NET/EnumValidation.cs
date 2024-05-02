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

namespace EnumsNET;

/// <summary>
/// Specifies the enum validation to perform.
/// </summary>
public enum EnumValidation
{
    /// <summary>
    /// No validation.
    /// </summary>
    None = 0,
    /// <summary>
    /// If the enum is a standard enum returns whether the value is defined.
    /// If the enum is marked with the <see cref="FlagsAttribute"/> it returns whether it's a valid flag combination of the enum's defined values
    /// or is defined. Or if the enum has an attribute that implements <see cref="IEnumValidatorAttribute{TEnum}"/>
    /// then that attribute's <see cref="IEnumValidatorAttribute{TEnum}.IsValid(TEnum)"/> method is used.
    /// </summary>
    Default = 1,
    /// <summary>
    /// Returns if the value is defined.
    /// </summary>
    IsDefined = 2,
    /// <summary>
    /// Returns if the value is a valid flag combination of the enum's defined values.
    /// </summary>
    IsValidFlagCombination = 3
}
