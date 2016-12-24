using System;

namespace EnumsNET.Tests.TestEnums
{
    [AttributeUsage(AttributeTargets.Enum)]
    internal sealed class EnumUnderlyingTypeAttribute : Attribute
    {
        public EnumUnderlyingTypeAttribute(Type underlyingType)
        {
        }
    }
}
