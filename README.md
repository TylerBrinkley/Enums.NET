![GitHub last commit (master)](https://img.shields.io/github/last-commit/TylerBrinkley/Enums.NET/master.svg?logo=github)
[![NuGet Version](https://img.shields.io/nuget/v/Enums.NET.svg?logo=nuget)](https://www.nuget.org/packages/Enums.NET/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Enums.NET.svg?logo=nuget)](https://www.nuget.org/packages/Enums.NET/)
[![Build status](https://img.shields.io/azure-devops/build/tydude4christ/Public/2.svg?logo=azuredevops)](https://dev.azure.com/tydude4christ/Public/_build?definitionId=2)

## v5.0 Changes
Removed `Boolean` enum support and dropped support for net45 and bumped it to net461 due to updating dependencies. Also dropped support for netstandard 1.x tfms for similar reasons. Added a net7 target which utilizes the built-in generic number interfaces. Also added an EnumValidatorAttribute base class since generic attributes are now supported.

## v4.0 Changes
Removed `NonGenericEnums`, `NonGenericFlagEnums`, `UnsafeEnums`, and `UnsafeFlagEnums` classes which were deprecated in v3.0 and also removed all other deprecated methods in an effort to slim the library size down. It is recommended if upgrading from 2.x and below to update to 3.x first and follow the warnings to migrate any code that's using deprecated methods and classes. Also, a dependency on the `System.Runtime.CompilerServices.Unsafe` package was added for the .NET 4.5 target in order to remove a build dependency on `Fody`.

# Enums.NET
Enums.NET is a high-performance type-safe .NET enum utility library which provides many operations as convenient extension methods. It is compatible with .NET Framework 4.6.1+ and .NET Standard 2.0+.

## What's wrong with `System.Enum`
1. Support for flag enums is limited to just the `HasFlag` method which isn't type-safe~, is inefficient,~ and is ambiguous as to whether it determines if the value has all or any of the specified flags. It's all by the way.
2. Most of its methods use reflection on each call without any sort of caching causing poor performance.
3. The pattern to associate extra data with an enum member using `Attribute`s is not supported and instead requires users to manually retrieve the `Attribute`s via reflection. This pattern is commonly used on enum members with the `DescriptionAttribute`, `EnumMemberAttribute`, and `DisplayAttribute`.

Enums.NET solves all of these issues and more.

## Enums.NET Demo

```c#
using System;
using System.Linq;
using EnumsNET;
using Xunit;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

class EnumsNETDemo
{
    // Enum definitions at bottom

    [Fact]
    public void Enumerate()
    {
        var count = 0;
        // Retrieves all enum members in increasing value order
        foreach (var member in Enums.GetMembers<NumericOperator>())
        {
            NumericOperator value = member.Value;
            string name = member.Name;
            AttributeCollection attributes = member.Attributes;
            ++count;
        }
        Assert.Equal(8, count);

        count = 0;
        // Retrieves distinct values in increasing value order
        foreach (var value in Enums.GetValues<NumericOperator>(EnumMemberSelection.Distinct))
        {
            string name = value.GetName();
            AttributeCollection attributes = value.GetAttributes();
            ++count;
        }
        Assert.Equal(6, count);
    }

    [Fact]
    public void FlagEnumOperations()
    {
        // HasAllFlags
        Assert.True((DaysOfWeek.Monday | DaysOfWeek.Wednesday | DaysOfWeek.Friday).HasAllFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));
        Assert.False(DaysOfWeek.Monday.HasAllFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));

        // HasAnyFlags
        Assert.True(DaysOfWeek.Monday.HasAnyFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));
        Assert.False((DaysOfWeek.Monday | DaysOfWeek.Wednesday).HasAnyFlags(DaysOfWeek.Friday));

        // CombineFlags ~ bitwise OR
        Assert.Equal(DaysOfWeek.Monday | DaysOfWeek.Wednesday, DaysOfWeek.Monday.CombineFlags(DaysOfWeek.Wednesday));
        Assert.Equal(DaysOfWeek.Monday | DaysOfWeek.Wednesday | DaysOfWeek.Friday, FlagEnums.CombineFlags(DaysOfWeek.Monday, DaysOfWeek.Wednesday, DaysOfWeek.Friday));

        // CommonFlags ~ bitwise AND
        Assert.Equal(DaysOfWeek.Monday, DaysOfWeek.Monday.CommonFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));
        Assert.Equal(DaysOfWeek.None, DaysOfWeek.Monday.CommonFlags(DaysOfWeek.Wednesday));

        // RemoveFlags
        Assert.Equal(DaysOfWeek.Wednesday, (DaysOfWeek.Monday | DaysOfWeek.Wednesday).RemoveFlags(DaysOfWeek.Monday));
        Assert.Equal(DaysOfWeek.None, (DaysOfWeek.Monday | DaysOfWeek.Wednesday).RemoveFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));

        // GetFlags, splits out the individual flags in increasing significance bit order
        var flags = DaysOfWeek.Weekend.GetFlags();
        Assert.Equal(2, flags.Count);
        Assert.Equal(DaysOfWeek.Sunday, flags[0]);
        Assert.Equal(DaysOfWeek.Saturday, flags[1]);
    }

    [Fact]
    public void AsString()
    {
        // AsString, equivalent to ToString
        Assert.Equal("Equals", NumericOperator.Equals.AsString());
        Assert.Equal("-1", ((NumericOperator)(-1)).AsString());

        // GetName
        Assert.Equal("Equals", NumericOperator.Equals.GetName());
        Assert.Null(((NumericOperator)(-1)).GetName());

        // Get description
        Assert.Equal("Is", NumericOperator.Equals.AsString(EnumFormat.Description));
        Assert.Null(NumericOperator.LessThan.AsString(EnumFormat.Description));

        // Get description if applied, otherwise the name
        Assert.Equal("LessThan", NumericOperator.LessThan.AsString(EnumFormat.Description, EnumFormat.Name));
    }

    [Fact]
    public void Validate()
    {
        // Standard Enums, checks is defined
        Assert.True(NumericOperator.LessThan.IsValid());
        Assert.False(((NumericOperator)20).IsValid());

        // Flag Enums, checks is valid flag combination or is defined
        Assert.True((DaysOfWeek.Sunday | DaysOfWeek.Wednesday).IsValid());
        Assert.False((DaysOfWeek.Sunday | DaysOfWeek.Wednesday | ((DaysOfWeek)(-1))).IsValid());

        // Custom validation through IEnumValidatorAttribute<TEnum>
        Assert.True(DayType.Weekday.IsValid());
        Assert.True((DayType.Weekday | DayType.Holiday).IsValid());
        Assert.False((DayType.Weekday | DayType.Weekend).IsValid());
    }

    [Fact]
    public void CustomEnumFormat()
    {
        EnumFormat symbolFormat = Enums.RegisterCustomEnumFormat(member => member.Attributes.Get<SymbolAttribute>()?.Symbol);
        Assert.Equal(">", NumericOperator.GreaterThan.AsString(symbolFormat));
        Assert.Equal(NumericOperator.LessThan, Enums.Parse<NumericOperator>("<", ignoreCase: false, symbolFormat));
    }

    [Fact]
    public void Attributes()
    {
        Assert.Equal("!=", NumericOperator.NotEquals.GetAttributes().Get<SymbolAttribute>().Symbol);
        Assert.True(Enums.GetMember<NumericOperator>("GreaterThanOrEquals").Attributes.Has<PrimaryEnumMemberAttribute>());
        Assert.False(NumericOperator.LessThan.GetAttributes().Has<DescriptionAttribute>());
    }

    [Fact]
    public void Parsing()
    {
        Assert.Equal(NumericOperator.GreaterThan, Enums.Parse<NumericOperator>("GreaterThan"));
        Assert.Equal(NumericOperator.NotEquals, Enums.Parse<NumericOperator>("1"));
        Assert.Equal(NumericOperator.Equals, Enums.Parse<NumericOperator>("Is", ignoreCase: false, EnumFormat.Description));

        Assert.Equal(DaysOfWeek.Monday | DaysOfWeek.Wednesday, Enums.Parse<DaysOfWeek>("Monday, Wednesday"));
        Assert.Equal(DaysOfWeek.Tuesday | DaysOfWeek.Thursday, FlagEnums.ParseFlags<DaysOfWeek>("Tuesday | Thursday", ignoreCase: false, delimiter: "|"));
    }

    enum NumericOperator
    {
        [Symbol("="), Description("Is")]
        Equals,
        [Symbol("!="), Description("Is not")]
        NotEquals,
        [Symbol("<")]
        LessThan,
        [Symbol(">="), PrimaryEnumMember] // PrimaryEnumMember indicates enum member as primary duplicate for extension methods
        GreaterThanOrEquals,
        NotLessThan = GreaterThanOrEquals,
        [Symbol(">")]
        GreaterThan,
        [Symbol("<="), PrimaryEnumMember]
        LessThanOrEquals,
        NotGreaterThan = LessThanOrEquals
    }

    [AttributeUsage(AttributeTargets.Field)]
    class SymbolAttribute : Attribute
    {
        public string Symbol { get; }

        public SymbolAttribute(string symbol)
        {
            Symbol = symbol;
        }
    }

    [Flags]
    enum DaysOfWeek
    {
        None = 0,
        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,
        Saturday = 64,
        Weekend = Sunday | Saturday,
        All = Sunday | Monday | Tuesday | Wednesday | Thursday | Friday | Saturday
    }

    [Flags, DayTypeValidator]
    enum DayType
    {
        Weekday = 1,
        Weekend = 2,
        Holiday = 4
    }

    class DayTypeValidatorAttribute : EnumValidatorAttribute<DayType>
    {
        public override bool IsValid(DayType value) => value.GetFlagCount(DayType.Weekday | DayType.Weekend) == 1 && FlagEnums.IsValidFlagCombination(value);
    }
}
```

---
![Performance](performance.png)

Results from running the [PerformanceTestConsole](./Src/Enums.NET.PerfTestConsole/Program.cs) BenchmarkDotNet application.

## Interface
See [fuget](https://www.fuget.org/packages/Enums.NET) for exploring the interface.

## Credits
Inspired by Jon Skeet's [Unconstrained Melody](https://github.com/jskeet/unconstrained-melody).
