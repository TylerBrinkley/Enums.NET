using System;
using System.Linq;
using EnumsNET;
using NUnit.Framework;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

[TestFixture]
class EnumsNETDemo
{
    // Enum definitions at bottom

    [Test]
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
        Assert.AreEqual(8, count);

        count = 0;
        // Retrieves distinct values in increasing value order
        foreach (var value in Enums.GetValues<NumericOperator>(EnumMemberSelection.Distinct))
        {
            string name = value.GetName();
            AttributeCollection attributes = value.GetAttributes();
            ++count;
        }
        Assert.AreEqual(6, count);
    }

    [Test]
    public void FlagEnumOperations()
    {
        // HasAllFlags
        Assert.IsTrue((DaysOfWeek.Monday | DaysOfWeek.Wednesday | DaysOfWeek.Friday).HasAllFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));
        Assert.IsFalse(DaysOfWeek.Monday.HasAllFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));

        // HasAnyFlags
        Assert.IsTrue(DaysOfWeek.Monday.HasAnyFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));
        Assert.IsFalse((DaysOfWeek.Monday | DaysOfWeek.Wednesday).HasAnyFlags(DaysOfWeek.Friday));

        // CombineFlags ~ bitwise OR
        Assert.AreEqual(DaysOfWeek.Monday | DaysOfWeek.Wednesday, DaysOfWeek.Monday.CombineFlags(DaysOfWeek.Wednesday));
        Assert.AreEqual(DaysOfWeek.Monday | DaysOfWeek.Wednesday | DaysOfWeek.Friday, FlagEnums.CombineFlags(DaysOfWeek.Monday, DaysOfWeek.Wednesday, DaysOfWeek.Friday));

        // CommonFlags ~ bitwise AND
        Assert.AreEqual(DaysOfWeek.Monday, DaysOfWeek.Monday.CommonFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));
        Assert.AreEqual(DaysOfWeek.None, DaysOfWeek.Monday.CommonFlags(DaysOfWeek.Wednesday));

        // RemoveFlags
        Assert.AreEqual(DaysOfWeek.Wednesday, (DaysOfWeek.Monday | DaysOfWeek.Wednesday).RemoveFlags(DaysOfWeek.Monday));
        Assert.AreEqual(DaysOfWeek.None, (DaysOfWeek.Monday | DaysOfWeek.Wednesday).RemoveFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));

        // GetFlags, splits out the individual flags in increasing significance bit order
        var flags = DaysOfWeek.Weekend.GetFlags().ToList();
        Assert.AreEqual(2, flags.Count);
        Assert.AreEqual(DaysOfWeek.Sunday, flags[0]);
        Assert.AreEqual(DaysOfWeek.Saturday, flags[1]);
    }

    [Test]
    public new void ToString()
    {
        // AsString, equivalent to ToString
        Assert.AreEqual("Equals", NumericOperator.Equals.AsString());
        Assert.AreEqual("-1", ((NumericOperator)(-1)).AsString());

        // GetName
        Assert.AreEqual("Equals", NumericOperator.Equals.GetName());
        Assert.IsNull(((NumericOperator)(-1)).GetName());

        // Get description
        Assert.AreEqual("Is", NumericOperator.Equals.AsString(EnumFormat.Description));
        Assert.IsNull(NumericOperator.LessThan.AsString(EnumFormat.Description));

        // Get description if applied, otherwise the name
        Assert.AreEqual("LessThan", NumericOperator.LessThan.AsString(EnumFormat.Description, EnumFormat.Name));
    }

    [Test]
    public void Validate()
    {
        // Standard Enums, checks is defined
        Assert.IsTrue(NumericOperator.LessThan.IsValid());
        Assert.IsFalse(((NumericOperator)20).IsValid());

        // Flag Enums, checks is valid flag combination or is defined
        Assert.IsTrue((DaysOfWeek.Sunday | DaysOfWeek.Wednesday).IsValid());
        Assert.IsFalse((DaysOfWeek.Sunday | DaysOfWeek.Wednesday | ((DaysOfWeek)(-1))).IsValid());

        // Custom validation through IEnumValidatorAttribute<TEnum>
        Assert.IsTrue(DayType.Weekday.IsValid());
        Assert.IsTrue((DayType.Weekday | DayType.Holiday).IsValid());
        Assert.IsFalse((DayType.Weekday | DayType.Weekend).IsValid());
    }

    [Test]
    public void CustomEnumFormat()
    {
        EnumFormat symbolFormat = Enums.RegisterCustomEnumFormat(member => member.Attributes.Get<SymbolAttribute>()?.Symbol);
        Assert.AreEqual(">", NumericOperator.GreaterThan.AsString(symbolFormat));
        Assert.AreEqual(NumericOperator.LessThan, Enums.Parse<NumericOperator>("<", ignoreCase: false, symbolFormat));
    }

    [Test]
    public void Attributes()
    {
        Assert.AreEqual("!=", NumericOperator.NotEquals.GetAttributes().Get<SymbolAttribute>().Symbol);
        Assert.IsTrue(Enums.GetMember<NumericOperator>("GreaterThanOrEquals").Attributes.Has<PrimaryEnumMemberAttribute>());
        Assert.IsFalse(NumericOperator.LessThan.GetAttributes().Has<DescriptionAttribute>());
    }

    [Test]
    public void Parsing()
    {
        Assert.AreEqual(NumericOperator.GreaterThan, Enums.Parse<NumericOperator>("GreaterThan"));
        Assert.AreEqual(NumericOperator.NotEquals, Enums.Parse<NumericOperator>("1"));
        Assert.AreEqual(NumericOperator.Equals, Enums.Parse<NumericOperator>("Is", ignoreCase: false, EnumFormat.Description));

        Assert.AreEqual(DaysOfWeek.Monday | DaysOfWeek.Wednesday, Enums.Parse<DaysOfWeek>("Monday, Wednesday"));
        Assert.AreEqual(DaysOfWeek.Tuesday | DaysOfWeek.Thursday, FlagEnums.ParseFlags<DaysOfWeek>("Tuesday | Thursday", ignoreCase: false, delimiter: "|"));
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

    [AttributeUsage(AttributeTargets.Enum)]
    class DayTypeValidatorAttribute : Attribute, IEnumValidatorAttribute<DayType>
    {
        public bool IsValid(DayType value) => value.GetFlagCount(DayType.Weekday | DayType.Weekend) == 1 && FlagEnums.IsValidFlagCombination(value);
    }
}