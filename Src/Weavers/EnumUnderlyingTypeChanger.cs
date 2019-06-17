using System;
using System.Collections.Generic;
using System.Linq;
using Fody;
using Mono.Cecil;

public class EnumUnderlyingTypeChanger : BaseModuleWeaver
{
    public override void Execute()
    {
        foreach (var enumType in ModuleDefinition.Types.Where(type => type.BaseType?.FullName == "System.Enum"))
        {
            var enumUnderlyingTypeAttr = enumType.CustomAttributes.FirstOrDefault(attr => attr.AttributeType.Name == "EnumUnderlyingTypeAttribute");
            if (enumUnderlyingTypeAttr != null)
            {
                var underlyingType = (TypeReference)enumUnderlyingTypeAttr.ConstructorArguments[0].Value;
                var originalUnderlyingType = ChangeEnumUnderlyingType(enumType, underlyingType);
                LogInfo($"Changed {enumType.Name}'s underlying type from {originalUnderlyingType.Name} to {underlyingType.Name}");
            }
        }
    }

    public override IEnumerable<string> GetAssembliesForScanning() => Enumerable.Empty<string>();

    private TypeReference ChangeEnumUnderlyingType(TypeDefinition enumType, TypeReference underlyingType)
    {
        var underlyingSystemType = Type.GetType(underlyingType.FullName, true);
        TypeReference originalUnderlyingType = null;
        foreach (var field in enumType.Fields)
        {
            if (field.Name == "value__")
            {
                originalUnderlyingType = field.FieldType;
                field.FieldType = underlyingType;
            }
            else
            {
                field.Constant = Convert.ChangeType(field.Constant, underlyingSystemType);
            }
        }
        return originalUnderlyingType;
    }
}