# Enums.NET
Enums.NET is an efficient type-safe .NET enum utility library which caches enums' names, values, and attributes and provides most capabilities as C# extension methods for ease of use.

## Supports
Descriptions through the use of the DescriptionAttribute

Flag enums which are decorated with the FlagsAttribute

Custom attribute retrieval

Parsing

## How Is It Type-Safe
As you may know there is no way to constrain a type or method's generic type parameter to an Enum in C#. This is a limitation the C# compiler imposes, not a limitation of the CLR itself thus a valid .NET assembly can contain these constraints. How this library works is that the C# compiler can understand when this constraint is applied, it just can't express it. Utilizing Simon Cropp's Fody.ExtraConstraints, Enums.NET is able to automatically apply a post-processing step on build to the compiled assembly to add these constraints to the assembly, thus achieving type safety.

## Interface
EnumsNET.Enums static class for type-safe standard enum operations, with most exposed as extension methods.

EnumsNET.FlagEnums static class for type-safe flag enum operations, with most exposed as extension methods.

EnumsNET.Unsafe.UnsafeEnums static class for standard enum operations without the enum constraint for use in generic programming.

EnumsNET.Unsafe.UnsafeFlagEnums static class for flag enum operations without the enum constraint for use in generic programming.

EnumsNET.NonGeneric.NonGenericEnums static class for non-generic standard enum operations, mostly a superset of .NET's System.Enum class.

EnumsNET.NonGeneric.NonGenericFlagEnums static class for non-generic flag enum operations.

## Requirements
.NET Framework 4.0 or greater

## Credits
Inspired by Jon Skeet's [Unconstrained Melody] (https://github.com/jskeet/unconstrained-melody)

Utilizes Simon Cropp's [Fody.ExtraConstraints] (https://github.com/Fody/ExtraConstraints)

Contains modified build scripts from James Newton-King's [Json.NET] (https://github.com/JamesNK/Newtonsoft.Json)
