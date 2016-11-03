using System;

namespace ExtraConstraints
{
    /// <summary>
    /// Adds an <see cref="Enum"/> constraint to a <see cref="AttributeTargets.GenericParameter"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.GenericParameter)]
    internal sealed class EnumConstraintAttribute : Attribute
    {
    }
}