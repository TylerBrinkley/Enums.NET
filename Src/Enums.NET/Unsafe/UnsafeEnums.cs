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
using System.Collections.Generic;
using System.ComponentModel;

namespace EnumsNET.Unsafe
{
    /// <summary>
    /// A type-unsafe version of <see cref="Enums"/> which is useful when dealing with generics
    /// and instead throws an <see cref="ArgumentException"/> if TEnum is not an enum type.
    /// </summary>
    public static class UnsafeEnums
    {
        internal static IEnumInfo<TEnum> GetInfo<TEnum>()
        {
            if (!UnsafeEnums<TEnum>.IsEnum)
            {
                throw new ArgumentException("Type argument TEnum must be an enum");
            }
            return Enums<TEnum>.Info;
        }

        #region Type Methods
        /// <summary>
        /// Retrieves the underlying type of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>The underlying type of <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static Type GetUnderlyingType<TEnum>() => GetInfo<TEnum>().UnderlyingType;

#if ICONVERTIBLE
        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s underlying type's <see cref="TypeCode"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns><typeparamref name="TEnum"/>'s underlying type's <see cref="TypeCode"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static TypeCode GetTypeCode<TEnum>() => GetInfo<TEnum>().TypeCode;
#endif

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s member count.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns><typeparamref name="TEnum"/>'s member count.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static int GetMemberCount<TEnum>() => GetInfo<TEnum>().GetMemberCount();

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s member count.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><typeparamref name="TEnum"/>'s member count.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="selection"/> is an invalid value.</exception>
        public static int GetMemberCount<TEnum>(EnumMemberSelection selection) => GetInfo<TEnum>().GetMemberCount(selection);

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s member count.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns><typeparamref name="TEnum"/>'s member count.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Renamed to GetMemberCount. This method will be removed in a future version.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static int GetEnumMemberCount<TEnum>() => GetMemberCount<TEnum>();

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s member count.
        /// The parameter <paramref name="excludeDuplicates"/> indicates whether to exclude duplicate value enum members.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="excludeDuplicates">Exclude duplicate value enum members.</param>
        /// <returns><typeparamref name="TEnum"/>'s member count.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Renamed to GetMemberCount and switched to using EnumMemberSelection parameter. This method will be removed in a future version.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static int GetEnumMemberCount<TEnum>(bool excludeDuplicates) => GetMemberCount<TEnum>(excludeDuplicates ? EnumMemberSelection.Distinct : EnumMemberSelection.All);

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members in increasing value order.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns><typeparamref name="TEnum"/>'s members in increasing value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static IEnumerable<EnumMember<TEnum>> GetMembers<TEnum>() => GetInfo<TEnum>().GetMembers();

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members in increasing value order.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><typeparamref name="TEnum"/>'s members in increasing value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="selection"/> is an invalid value.</exception>
        public static IEnumerable<EnumMember<TEnum>> GetMembers<TEnum>(EnumMemberSelection selection) => GetInfo<TEnum>().GetMembers(selection);

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members in increasing value order.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns><typeparamref name="TEnum"/>'s members in increasing value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Renamed to GetMembers. This method will be removed in a future version.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<EnumMember<TEnum>> GetEnumMembers<TEnum>() => GetMembers<TEnum>();

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members in increasing value order.
        /// The parameter <paramref name="excludeDuplicates"/> indicates whether to exclude duplicate value enum members.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="excludeDuplicates">Exclude duplicate value enum members.</param>
        /// <returns><typeparamref name="TEnum"/>'s members in increasing value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Renamed to GetMembers and switched to using EnumMemberSelection parameter. This method will be removed in a future version.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<EnumMember<TEnum>> GetEnumMembers<TEnum>(bool excludeDuplicates) => GetMembers<TEnum>(excludeDuplicates ? EnumMemberSelection.Distinct : EnumMemberSelection.All);

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members' names in increasing value order.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns><typeparamref name="TEnum"/>'s members' names in increasing value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static IEnumerable<string> GetNames<TEnum>() => GetInfo<TEnum>().GetNames();

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members' names in increasing value order.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><typeparamref name="TEnum"/>'s members' names in increasing value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="selection"/> is an invalid value.</exception>
        public static IEnumerable<string> GetNames<TEnum>(EnumMemberSelection selection) => GetInfo<TEnum>().GetNames(selection);

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members' names in increasing value order.
        /// The parameter <paramref name="excludeDuplicates"/> indicates whether to exclude duplicate value enum members.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="excludeDuplicates">Exclude duplicate value enum members.</param>
        /// <returns><typeparamref name="TEnum"/>'s members' names in increasing value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Switched to using EnumMemberSelection parameter. This method will be removed in a future version.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<string> GetNames<TEnum>(bool excludeDuplicates) => GetNames<TEnum>(excludeDuplicates ? EnumMemberSelection.Distinct : EnumMemberSelection.All);

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members' values in increasing value order.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns><typeparamref name="TEnum"/>'s members' values in increasing value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static IEnumerable<TEnum> GetValues<TEnum>() => GetInfo<TEnum>().GetValues();

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members' values in increasing value order.
        /// The parameter <paramref name="selection"/> indicates what members to include.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="selection">Indicates what members to include.</param>
        /// <returns><typeparamref name="TEnum"/>'s members' values in increasing value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="selection"/> is an invalid value.</exception>
        public static IEnumerable<TEnum> GetValues<TEnum>(EnumMemberSelection selection) => GetInfo<TEnum>().GetValues(selection);

        /// <summary>
        /// Retrieves <typeparamref name="TEnum"/>'s members' values in increasing value order.
        /// The parameter <paramref name="excludeDuplicates"/> indicates whether to exclude duplicate value enum members.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="excludeDuplicates">Exclude duplicate value enum members.</param>
        /// <returns><typeparamref name="TEnum"/>'s members' values in increasing value order.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Switched to using EnumMemberSelection parameter. This method will be removed in a future version.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TEnum> GetValues<TEnum>(bool excludeDuplicates) => GetValues<TEnum>(excludeDuplicates ? EnumMemberSelection.Distinct : EnumMemberSelection.All);
        #endregion

        #region ToObject
        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <typeparamref name="TEnum"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not a valid type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(object value) => GetInfo<TEnum>().ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <typeparamref name="TEnum"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not a valid type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(object value, EnumValidation validation) => GetInfo<TEnum>().ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <typeparamref name="TEnum"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is not a valid type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TEnum ToObject<TEnum>(object value, bool validate) => ToObject<TEnum>(value, validate ? EnumValidation.Default : EnumValidation.None);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(sbyte value) => GetInfo<TEnum>().ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(sbyte value, EnumValidation validation) => GetInfo<TEnum>().ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TEnum ToObject<TEnum>(sbyte value, bool validate) => ToObject<TEnum>(value, validate ? EnumValidation.Default : EnumValidation.None);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(byte value) => GetInfo<TEnum>().ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(byte value, EnumValidation validation) => GetInfo<TEnum>().ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TEnum ToObject<TEnum>(byte value, bool validate) => ToObject<TEnum>(value, validate ? EnumValidation.Default : EnumValidation.None);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(short value) => GetInfo<TEnum>().ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(short value, EnumValidation validation) => GetInfo<TEnum>().ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TEnum ToObject<TEnum>(short value, bool validate) => ToObject<TEnum>(value, validate ? EnumValidation.Default : EnumValidation.None);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(ushort value) => GetInfo<TEnum>().ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(ushort value, EnumValidation validation) => GetInfo<TEnum>().ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TEnum ToObject<TEnum>(ushort value, bool validate) => ToObject<TEnum>(value, validate ? EnumValidation.Default : EnumValidation.None);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(int value) => GetInfo<TEnum>().ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(int value, EnumValidation validation) => GetInfo<TEnum>().ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TEnum ToObject<TEnum>(int value, bool validate) => ToObject<TEnum>(value, validate ? EnumValidation.Default : EnumValidation.None);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(uint value) => GetInfo<TEnum>().ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(uint value, EnumValidation validation) => GetInfo<TEnum>().ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TEnum ToObject<TEnum>(uint value, bool validate) => ToObject<TEnum>(value, validate ? EnumValidation.Default : EnumValidation.None);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(long value) => GetInfo<TEnum>().ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        public static TEnum ToObject<TEnum>(long value, EnumValidation validation) => GetInfo<TEnum>().ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TEnum ToObject<TEnum>(long value, bool validate) => ToObject<TEnum>(value, validate ? EnumValidation.Default : EnumValidation.None);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(ulong value) => GetInfo<TEnum>().ToObject(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// the result is invalid with the specified <paramref name="validation"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        public static TEnum ToObject<TEnum>(ulong value, EnumValidation validation) => GetInfo<TEnum>().ToObject(value, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <returns>The specified <paramref name="value"/> converted to a <typeparamref name="TEnum"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validate"/> is <c>true</c> and the result is not a valid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the underlying type's value range.</exception>
        [CLSCompliant(false)]
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TEnum ToObject<TEnum>(ulong value, bool validate) => ToObject<TEnum>(value, validate ? EnumValidation.Default : EnumValidation.None);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <typeparamref name="TEnum"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryToObject<TEnum>(object value, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <typeparamref name="TEnum"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject<TEnum>(object value, EnumValidation validation, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert. Must be an <see cref="sbyte"/>, <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        /// <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <typeparamref name="TEnum"/>, <see cref="string"/>, or Nullable of one of these.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool TryToObject<TEnum>(object value, bool validate, out TEnum result) => TryToObject(value, validate ? EnumValidation.Default : EnumValidation.None, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(sbyte value, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(sbyte value, EnumValidation validation, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool TryToObject<TEnum>(sbyte value, bool validate, out TEnum result) => TryToObject(value, validate ? EnumValidation.Default : EnumValidation.None, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryToObject<TEnum>(byte value, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject<TEnum>(byte value, EnumValidation validation, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool TryToObject<TEnum>(byte value, bool validate, out TEnum result) => TryToObject(value, validate ? EnumValidation.Default : EnumValidation.None, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryToObject<TEnum>(short value, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject<TEnum>(short value, EnumValidation validation, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool TryToObject<TEnum>(short value, bool validate, out TEnum result) => TryToObject(value, validate ? EnumValidation.Default : EnumValidation.None, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(ushort value, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(ushort value, EnumValidation validation, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool TryToObject<TEnum>(ushort value, bool validate, out TEnum result) => TryToObject(value, validate ? EnumValidation.Default : EnumValidation.None, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryToObject<TEnum>(int value, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject<TEnum>(int value, EnumValidation validation, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool TryToObject<TEnum>(int value, bool validate, out TEnum result) => TryToObject(value, validate ? EnumValidation.Default : EnumValidation.None, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(uint value, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(uint value, EnumValidation validation, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool TryToObject<TEnum>(uint value, bool validate, out TEnum result) => TryToObject(value, validate ? EnumValidation.Default : EnumValidation.None, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryToObject<TEnum>(long value, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool TryToObject<TEnum>(long value, EnumValidation validation, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool TryToObject<TEnum>(long value, bool validate, out TEnum result) => TryToObject(value, validate ? EnumValidation.Default : EnumValidation.None, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(ulong value, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result);

        /// <summary>
        /// Tries to convert the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validation"/> specifies the validation to perform on the result.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validation">The validation to perform on the result.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        [CLSCompliant(false)]
        public static bool TryToObject<TEnum>(ulong value, EnumValidation validation, out TEnum result) => GetInfo<TEnum>().TryToObject(value, out result, validation);

        /// <summary>
        /// Tries to converts the specified <paramref name="value"/> to a <typeparamref name="TEnum"/> while checking that it doesn't overflow the
        /// underlying type. The parameter <paramref name="validate"/> specifies whether to check that the result is valid.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">Value to try to convert.</param>
        /// <param name="validate">Indicates whether to check that the result is valid.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> whose value is <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [CLSCompliant(false)]
        [Obsolete("Use EnumValidation overload instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool TryToObject<TEnum>(ulong value, bool validate, out TEnum result) => TryToObject(value, validate ? EnumValidation.Default : EnumValidation.None, out result);
        #endregion

        #region All Values Main Methods
        /// <summary>
        /// Indicates if <paramref name="value"/> is valid. If <typeparamref name="TEnum"/> is a standard enum it returns whether the value is defined.
        /// If <typeparamref name="TEnum"/> is marked with <see cref="FlagsAttribute"/> it returns whether it's a valid flag combination of <typeparamref name="TEnum"/>'s defined values
        /// or is defined. Or if <typeparamref name="TEnum"/> has an attribute that implements <see cref="IEnumValidatorAttribute{TEnum}"/>
        /// then that attribute's <see cref="IEnumValidatorAttribute{TEnum}.IsValid(TEnum)"/> method is used.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Indication if <paramref name="value"/> is valid.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool IsValid<TEnum>(TEnum value) => GetInfo<TEnum>().IsValid(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> is valid using the specified <paramref name="validation"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="validation">The validation to perform on the value.</param>
        /// <returns>Indication if <paramref name="value"/> is valid.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value.</exception>
        public static bool IsValid<TEnum>(TEnum value, EnumValidation validation) => GetInfo<TEnum>().IsValid(value, validation);

        /// <summary>
        /// Indicates if <paramref name="value"/> is defined.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Indication if <paramref name="value"/> is defined.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool IsDefined<TEnum>(TEnum value) => GetInfo<TEnum>().IsDefined(value);

        /// <summary>
        /// Validates that <paramref name="value"/> is valid. If it's not it throws an <see cref="ArgumentException"/> with the specified <paramref name="paramName"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="paramName">The parameter name to be used if throwing an <see cref="ArgumentException"/>.</param>
        /// <returns><paramref name="value"/> for use in fluent API's and base constructor method calls.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> is invalid.</exception>
        public static TEnum Validate<TEnum>(TEnum value, string paramName) => GetInfo<TEnum>().Validate(value, paramName);

        /// <summary>
        /// Validates that <paramref name="value"/> is valid using the specified <paramref name="validation"/>.
        /// If it's not it throws an <see cref="ArgumentException"/> with the specified <paramref name="paramName"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="paramName">The parameter name to be used if throwing an <see cref="ArgumentException"/>.</param>
        /// <param name="validation">The validation to perform on the value.</param>
        /// <returns><paramref name="value"/> for use in fluent API's and base constructor method calls.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="validation"/> is an invalid value
        /// -or-
        /// <paramref name="value"/> is invalid.</exception>
        public static TEnum Validate<TEnum>(TEnum value, string paramName, EnumValidation validation) => GetInfo<TEnum>().Validate(value, paramName, validation);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static string AsString<TEnum>(TEnum value) => GetInfo<TEnum>().AsString(value);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        public static string AsString<TEnum>(TEnum value, string format) => GetInfo<TEnum>().AsString(value, format);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static string AsString<TEnum>(TEnum value, EnumFormat format) => GetInfo<TEnum>().AsString(value, format);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use if using the first resolves to <c>null</c>.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static string AsString<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1) => GetInfo<TEnum>().AsString(value, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format0">The first output format to use.</param>
        /// <param name="format1">The second output format to use if using the first resolves to <c>null</c>.</param>
        /// <param name="format2">The third output format to use if using the first and second both resolve to <c>null</c>.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static string AsString<TEnum>(TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetInfo<TEnum>().AsString(value, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="formats"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="formats">The output formats to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static string AsString<TEnum>(TEnum value, params EnumFormat[] formats) => GetInfo<TEnum>().AsString(value, new ValueCollection<EnumFormat>(formats));

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="format"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="format">The output format to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="FormatException"><paramref name="format"/> is an invalid value.</exception>
        public static string Format<TEnum>(TEnum value, string format) => GetInfo<TEnum>().Format(value, format);

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its string representation using the specified <paramref name="formats"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="formats">The output formats to use.</param>
        /// <returns>A string representation of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="formats"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static string Format<TEnum>(TEnum value, params EnumFormat[] formats)
        {
            Preconditions.NotNull(formats, nameof(formats));

            return AsString(value, formats);
        }

        /// <summary>
        /// Returns <paramref name="value"/>'s underlying integral value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/>'s underlying integral value.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static object GetUnderlyingValue<TEnum>(TEnum value) => GetInfo<TEnum>().GetUnderlyingValue(value);

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="sbyte"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="sbyte"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="sbyte"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static sbyte ToSByte<TEnum>(TEnum value) => GetInfo<TEnum>().ToSByte(value);

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="byte"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="byte"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="byte"/>'s value range without overflowing.</exception>
        public static byte ToByte<TEnum>(TEnum value) => GetInfo<TEnum>().ToByte(value);

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="short"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="short"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="short"/>'s value range without overflowing.</exception>
        public static short ToInt16<TEnum>(TEnum value) => GetInfo<TEnum>().ToInt16(value);

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="ushort"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="ushort"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ushort"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static ushort ToUInt16<TEnum>(TEnum value) => GetInfo<TEnum>().ToUInt16(value);

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="int"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="int"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="int"/>'s value range without overflowing.</exception>
        public static int ToInt32<TEnum>(TEnum value) => GetInfo<TEnum>().ToInt32(value);

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="uint"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="uint"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="uint"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static uint ToUInt32<TEnum>(TEnum value) => GetInfo<TEnum>().ToUInt32(value);

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="long"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to an <see cref="long"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="long"/>'s value range without overflowing.</exception>
        public static long ToInt64<TEnum>(TEnum value) => GetInfo<TEnum>().ToInt64(value);

        /// <summary>
        /// Converts <paramref name="value"/> to a <see cref="ulong"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/> converted to a <see cref="ulong"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> cannot fit within <see cref="ulong"/>'s value range without overflowing.</exception>
        [CLSCompliant(false)]
        public static ulong ToUInt64<TEnum>(TEnum value) => GetInfo<TEnum>().ToUInt64(value);

        /// <summary>
        /// Retrieves the hash code of <paramref name="value"/>. It's more efficient as it doesn't require boxing and unboxing of <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Hash code of <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static int GetHashCode<TEnum>(TEnum value) => GetInfo<TEnum>().GetHashCode(value);

        /// <summary>
        /// Indicates if <paramref name="value"/> equals <paramref name="other"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="other">The other enum value.</param>
        /// <returns>Indication if <paramref name="value"/> equals <paramref name="other"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool Equals<TEnum>(TEnum value, TEnum other) => GetInfo<TEnum>().Equals(value, other);

        /// <summary>
        /// Compares <paramref name="value"/> to <paramref name="other"/> for ordering.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <param name="other">The other enum value.</param>
        /// <returns>1 if <paramref name="value"/> is greater than <paramref name="other"/>, 0 if <paramref name="value"/> equals <paramref name="other"/>,
        /// and -1 if <paramref name="value"/> is less than <paramref name="other"/>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static int CompareTo<TEnum>(TEnum value, TEnum other) => GetInfo<TEnum>().CompareTo(value, other);
        #endregion

        #region Defined Values Main Methods
        /// <summary>
        /// Retrieves <paramref name="value"/>'s enum member name if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/>'s enum member name if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static string GetName<TEnum>(TEnum value) => GetInfo<TEnum>().GetName(value);

        /// <summary>
        /// Retrieves <paramref name="value"/>'s enum member attributes if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns><paramref name="value"/>'s enum member attributes if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static AttributeCollection GetAttributes<TEnum>(TEnum value) => GetInfo<TEnum>().GetAttributes(value);

        /// <summary>
        /// Retrieves an enum member with the specified <paramref name="value"/> if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Enum member with the specified <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static EnumMember<TEnum> GetMember<TEnum>(TEnum value) => GetInfo<TEnum>().GetMember(value);

        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// Is case-sensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="name">The enum member name.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static EnumMember<TEnum> GetMember<TEnum>(string name) => GetInfo<TEnum>().GetMember(name);

        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="name">The enum member name.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static EnumMember<TEnum> GetMember<TEnum>(string name, bool ignoreCase) => GetInfo<TEnum>().GetMember(name, ignoreCase);

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="format"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static EnumMember<TEnum> GetMember<TEnum>(string value, EnumFormat format) => GetMember<TEnum>(value, false, format);

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static EnumMember<TEnum> GetMember<TEnum>(string value, EnumFormat format0, EnumFormat format1) => GetMember<TEnum>(value, false, format0, format1);

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static EnumMember<TEnum> GetMember<TEnum>(string value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetMember<TEnum>(value, false, format0, format1, format2);

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="formats"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static EnumMember<TEnum> GetMember<TEnum>(string value, params EnumFormat[] formats) => GetMember<TEnum>(value, false, formats);

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="format"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static EnumMember<TEnum> GetMember<TEnum>(string value, bool ignoreCase, EnumFormat format) => GetInfo<TEnum>().GetMember(value, ignoreCase, new ValueCollection<EnumFormat>(format));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static EnumMember<TEnum> GetMember<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => GetInfo<TEnum>().GetMember(value, ignoreCase, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified formats is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        public static EnumMember<TEnum> GetMember<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetInfo<TEnum>().GetMember(value, ignoreCase, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="formats"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static EnumMember<TEnum> GetMember<TEnum>(string value, bool ignoreCase, params EnumFormat[] formats) => GetInfo<TEnum>().GetMember(value, ignoreCase, new ValueCollection<EnumFormat>(formats));

        /// <summary>
        /// Retrieves an enum member with the specified <paramref name="value"/> if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>Enum member with the specified <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Renamed to GetMember. This method will be removed in a future version.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static EnumMember<TEnum> GetEnumMember<TEnum>(TEnum value) => GetMember(value);

        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// Is case-sensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="name">The enum member name.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Renamed to GetMember. This method will be removed in a future version.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static EnumMember<TEnum> GetEnumMember<TEnum>(string name) => GetMember<TEnum>(name);

        /// <summary>
        /// Retrieves the enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="name">The enum member name.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>Enum member with the specified <paramref name="name"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        [Obsolete("Renamed to GetMember. This method will be removed in a future version.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static EnumMember<TEnum> GetEnumMember<TEnum>(string name, bool ignoreCase) => GetMember<TEnum>(name, ignoreCase);

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="formats"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        [Obsolete("Renamed to GetMember. This method will be removed in a future version.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static EnumMember<TEnum> GetEnumMember<TEnum>(string value, params EnumFormat[] formats) => GetMember<TEnum>(value, formats);

        /// <summary>
        /// Retrieves an enum member whose string representation using the specified <paramref name="formats"/> is <paramref name="value"/> if defined otherwise <c>null</c>.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member's string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Enum member represented by <paramref name="value"/> if defined otherwise <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        [Obsolete("Renamed to GetMember. This method will be removed in a future version.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static EnumMember<TEnum> GetEnumMember<TEnum>(string value, bool ignoreCase, params EnumFormat[] formats) => GetMember<TEnum>(value, ignoreCase, formats);
        #endregion

        #region Parsing
        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of <typeparamref name="TEnum"/>'s underlying type.</exception>
        public static TEnum Parse<TEnum>(string value) => GetInfo<TEnum>().Parse(value);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, EnumFormat format) => Parse<TEnum>(value, false, format);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, EnumFormat format0, EnumFormat format1) => Parse<TEnum>(value, false, format0, format1);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => Parse<TEnum>(value, false, format0, format1, format2);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>A <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, params EnumFormat[] formats) => Parse<TEnum>(value, false, formats);

        /// <summary>
        /// Converts the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member name or value of <typeparamref name="TEnum"/>.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase) => GetInfo<TEnum>().Parse(value, ignoreCase);

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase, EnumFormat format) => GetInfo<TEnum>().Parse(value, ignoreCase, new ValueCollection<EnumFormat>(format));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1) => GetInfo<TEnum>().Parse(value, ignoreCase, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, or <paramref name="format2"/> is an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetInfo<TEnum>().Parse(value, ignoreCase, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Converts the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies if the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>The <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="value"/> doesn't represent a member or value of <typeparamref name="TEnum"/>
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        /// <exception cref="OverflowException"><paramref name="value"/> is outside the range of the underlying type of <typeparamref name="TEnum"/>.</exception>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase, params EnumFormat[] formats) => GetInfo<TEnum>().Parse(value, ignoreCase, new ValueCollection<EnumFormat>(formats));

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParse<TEnum>(string value, out TEnum result) => GetInfo<TEnum>().TryParse(value, false, out result);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParse<TEnum>(string value, out TEnum result, EnumFormat format) => TryParse(value, false, out result, format);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParse<TEnum>(string value, out TEnum result, EnumFormat format0, EnumFormat format1) => TryParse(value, false, out result, format0, format1);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParse<TEnum>(string value, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => TryParse(value, false, out result, format0, format1, format2);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParse<TEnum>(string value, out TEnum result, params EnumFormat[] formats) => TryParse(value, false, out result, formats);

        /// <summary>
        /// Tries to convert the string representation of one or more member names or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value.
        /// The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum member names or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type.</exception>
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) => GetInfo<TEnum>().TryParse(value, ignoreCase, out result);

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum format. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format">The parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format"/> is an invalid value.</exception>
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result, EnumFormat format) => GetInfo<TEnum>().TryParse(value, ignoreCase, out result, new ValueCollection<EnumFormat>(format));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/> or <paramref name="format1"/> is an invalid value.</exception>
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1) => GetInfo<TEnum>().TryParse(value, ignoreCase, out result, new ValueCollection<EnumFormat>(format0, format1));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="format0">The first parsing enum format.</param>
        /// <param name="format1">The second parsing enum format.</param>
        /// <param name="format2">The third parsing enum format.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="format0"/>, <paramref name="format1"/>, <paramref name="format2"/> is an invalid value.</exception>
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result, EnumFormat format0, EnumFormat format1, EnumFormat format2) => GetInfo<TEnum>().TryParse(value, ignoreCase, out result, new ValueCollection<EnumFormat>(format0, format1, format2));

        /// <summary>
        /// Tries to convert the string representation of one or more members or values of <typeparamref name="TEnum"/> to its respective <typeparamref name="TEnum"/> value
        /// using the specified parsing enum formats. The parameter <paramref name="ignoreCase"/> specifies whether the operation is case-insensitive.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum members or values' string representation.</param>
        /// <param name="ignoreCase">Indicates if the operation is case-insensitive.</param>
        /// <param name="result">If the conversion succeeds this contains a <typeparamref name="TEnum"/> value that is represented by <paramref name="value"/>.</param>
        /// <param name="formats">The parsing enum formats.</param>
        /// <returns>Indication whether the conversion succeeded.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enum type
        /// -or-
        /// <paramref name="formats"/> contains an invalid value.</exception>
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result, params EnumFormat[] formats) => GetInfo<TEnum>().TryParse(value, ignoreCase, out result, new ValueCollection<EnumFormat>(formats));
        #endregion
    }

    internal static class UnsafeEnums<TEnum>
    {
        public static bool IsEnum { get; } = typeof(TEnum).IsEnum();
    }
}
