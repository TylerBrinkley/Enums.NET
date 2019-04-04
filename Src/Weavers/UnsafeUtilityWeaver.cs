using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class UnsafeUtilityWeaver
{
    // An instance of Mono.Cecil.ModuleDefinition for processing
    public ModuleDefinition ModuleDefinition { get; set; }

    // Will log an MessageImportance.High message to MSBuild. OPTIONAL
    public Action<string> LogInfo { get; set; }

    public UnsafeUtilityWeaver()
    {
        LogInfo = s => { };
    }

    public void Execute()
    {
        var enumInfoType = ModuleDefinition.GetType("EnumsNET.Utilities.UnsafeUtility");
        foreach (var method in enumInfoType.Methods)
        {
            if (method.Name == "As")
            {
                var processor = method.Body.GetILProcessor();
                processor.Emit(OpCodes.Ldarg_0);
                processor.Emit(OpCodes.Ret);
                LogInfo($"Set implementation of {method.Name}");
            }
        }
    }
}