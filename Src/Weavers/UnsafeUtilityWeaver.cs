﻿using System.Collections.Generic;
using System.Linq;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class UnsafeUtilityWeaver : BaseModuleWeaver
{
    public override void Execute()
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

    public override IEnumerable<string> GetAssembliesForScanning() => Enumerable.Empty<string>();
}