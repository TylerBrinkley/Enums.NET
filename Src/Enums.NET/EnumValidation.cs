using System;

namespace EnumsNET
{
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
        /// If the enum is a standard enum returns returns whether the value is defined.
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
}
