# Enums.NET
Enums.NET is an efficient type-safe .NET enum utility library which caches enums' names, values, and attributes and provides most capabilities as extension methods for ease of use.

## Supports
Descriptions through the use of the DescriptionAttribute

Flag enums which are decorated with the FlagsAttribute

Custom attribute retrieval

Parsing

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
