using System;

namespace EnumsNET.Tests.TestEnums
{
    [Flags]
    [TypeNameHandlingValidator]
    public enum TypeNameHandling
    {
        None = 0,
        Objects = 1,
        Arrays = 2,
        Auto = 4
    }

    public class TypeNameHandlingValidatorAttribute : Attribute, IEnumValidatorAttribute<TypeNameHandling>
    {
        public bool IsValid(TypeNameHandling value) => value >= TypeNameHandling.None && value <= TypeNameHandling.Auto;
    }
}
