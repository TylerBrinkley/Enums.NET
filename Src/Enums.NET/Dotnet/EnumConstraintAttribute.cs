using System;

namespace ExtraConstraints
{
    /// <summary>
    /// Adds an <see cref="Enum"/> constraint to a <see cref="AttributeTargets.GenericParameter"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.GenericParameter)]
    public class EnumConstraintAttribute : Attribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EnumConstraintAttribute()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type">A <see cref="Type"/> representing the type of enum to constrain the generic parameter to</param>.
        // ReSharper disable once UnusedParameter.Local
        public EnumConstraintAttribute(Type type)
        {
        }
    }
}