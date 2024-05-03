using System;

namespace EnumsNET.Tests.TestEnums;

[Flags]
[TypeNameHandlingValidator]
public enum TypeNameHandling
{
    None = 0,
    Objects = 1,
    Arrays = 2,
    Auto = 4
}

public sealed class TypeNameHandlingValidatorAttribute : EnumValidatorAttribute<TypeNameHandling>
{
    public override bool IsValid(TypeNameHandling value) => value is >= TypeNameHandling.None and <= TypeNameHandling.Auto;
}
