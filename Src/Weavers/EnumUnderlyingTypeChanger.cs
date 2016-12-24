using System;
using System.Linq;
using Mono.Cecil;

public class EnumUnderlyingTypeChanger
{
    // An instance of Mono.Cecil.ModuleDefinition for processing
    public ModuleDefinition ModuleDefinition { get; set; }

    // Will log an MessageImportance.High message to MSBuild. OPTIONAL
    public Action<string> LogInfo { get; set; }

    public EnumUnderlyingTypeChanger()
    {
        LogInfo = s => { };
    }

    public void Execute()
    {
        foreach (var enumType in ModuleDefinition.Types.Where(type => type.BaseType?.Name == "Enum"))
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