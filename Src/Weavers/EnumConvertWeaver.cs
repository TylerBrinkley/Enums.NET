using Mono.Cecil;
using Mono.Cecil.Cil;

public class EnumConvertWeaver
{
    // An instance of Mono.Cecil.ModuleDefinition for processing
    public ModuleDefinition ModuleDefinition { get; set; }

    public void Execute()
    {
        var enumInfoType = ModuleDefinition.GetType("EnumsNET", "EnumInfo`3");
        foreach (var method in enumInfoType.Methods)
        {
            if (method.Name == "ToInt" || method.Name == "ToEnum")
            {
                var processor = method.Body.GetILProcessor();
                processor.Emit(OpCodes.Ldarg_0);
                processor.Emit(OpCodes.Ret);
            }
        }
    }
}