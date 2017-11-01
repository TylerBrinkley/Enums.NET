using System;
using System.Linq;
using Mono.Cecil;

public class EnumConstraintWeaver
{
    // An instance of Mono.Cecil.ModuleDefinition for processing
    public ModuleDefinition ModuleDefinition { get; set; }

    // Will log an MessageImportance.High message to MSBuild. OPTIONAL
    public Action<string> LogInfo { get; set; }

    private const string _enumConstraintAttributeName = "EnumConstraintAttribute";

    public EnumConstraintWeaver()
    {
        LogInfo = s => { };
    }

    public void Execute()
    {
        var corlib = ModuleDefinition.AssemblyReferences.FirstOrDefault(a => a.Name == "mscorlib" || a.Name == "System.Runtime" || a.Name == "netstandard");
        if (!ModuleDefinition.AssemblyResolver.Resolve(corlib).MainModule.Types.Any(x => x.FullName == "System.Enum"))
        {
            throw new Exception("Could not find System.Enum in referenced assemblies");
        }
        var allTypes = ModuleDefinition.GetTypes().Where(x => x.IsClass || x.IsInterface).ToList();
        foreach (var typeDefinition in allTypes)
        {
            Process(typeDefinition, corlib);
            foreach (var methodDefinition in typeDefinition.Methods)
            {
                Process(methodDefinition, corlib);
            }
        }

        foreach (var typeDefinition in allTypes.Where(x => x.Name == _enumConstraintAttributeName))
        {
            ModuleDefinition.Types.Remove(typeDefinition);
            break;
        }
    }

    private void Process(IGenericParameterProvider provider, AssemblyNameReference corlib)
    {
        if (provider.HasGenericParameters)
        {
            foreach (var parameter in provider.GenericParameters.Where(x => x.HasCustomAttributes))
            {
                var attributes = parameter.CustomAttributes;
                for (var i = 0; i < attributes.Count; ++i)
                {
                    var attribute = attributes[i];
                    if (attribute.AttributeType.Name == _enumConstraintAttributeName)
                    {
                        LogInfo($"Added Enum Constraint to {((MemberReference)provider).FullName}");
                        parameter.Attributes = GenericParameterAttributes.NonVariant | GenericParameterAttributes.NotNullableValueTypeConstraint;
                        parameter.Constraints.Clear();
                        parameter.Constraints.Add(new TypeReference("System", "Enum", parameter.Module, corlib, false));
                        attributes.RemoveAt(i--);
                        break;
                    }
                }
            }
        }
    }
}