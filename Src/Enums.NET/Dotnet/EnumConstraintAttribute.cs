using System;

namespace ExtraConstraints
{
    [AttributeUsage(AttributeTargets.GenericParameter)]
    internal sealed class EnumConstraintAttribute : Attribute
    {
    }
}