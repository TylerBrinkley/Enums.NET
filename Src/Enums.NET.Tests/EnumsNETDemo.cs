using System;
using System.Collections.Generic;
using System.Linq;
using EnumsNET;
using NUnit.Framework;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

[TestFixture]
class EnumsNETDemo
{
    // Test enum definitions at the bottom

    [Test]
    public void Enumerate()
    {
        // Retrieves enum members in increasing value order
        foreach (EnumMember<NumericOperator> member in Enums.GetMembers<NumericOperator>())
        {
            NumericOperator value = member.Value;
            string name = member.Name;
            // Do stuff
        }
        Assert.AreEqual(8, Enums.GetMembers<NumericOperator>().Count());
        Assert.AreEqual(6, Enums.GetMembers<NumericOperator>(EnumMemberSelection.Distinct).Count());
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

        // Custom validation through IEnumValidatorAttribute
        Assert.IsTrue(DayType.Weekday.IsValid());
        Assert.IsTrue((DayType.Weekday | DayType.Holiday).IsValid());
        Assert.IsFalse((DayType.Weekday | DayType.Weekend).IsValid());
    }

    [Test]
    public void FlagEnumOperations()
    {
        // CombineFlags ~ bitwise OR
        Assert.AreEqual(DaysOfWeek.Monday | DaysOfWeek.Wednesday, DaysOfWeek.Monday.CombineFlags(DaysOfWeek.Wednesday));
        Assert.AreEqual(DaysOfWeek.Monday | DaysOfWeek.Wednesday | DaysOfWeek.Friday, FlagEnums.CombineFlags(DaysOfWeek.Monday, DaysOfWeek.Wednesday, DaysOfWeek.Friday));

        // HasAnyFlags
        Assert.IsTrue(DaysOfWeek.Monday.HasAnyFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));
        Assert.IsFalse((DaysOfWeek.Monday | DaysOfWeek.Wednesday).HasAnyFlags(DaysOfWeek.Friday));

        // HasAllFlags
        Assert.IsTrue((DaysOfWeek.Monday | DaysOfWeek.Wednesday | DaysOfWeek.Friday).HasAllFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));
        Assert.IsFalse(DaysOfWeek.Monday.HasAllFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));

        // CommonFlags ~ bitwise AND
        Assert.AreEqual(DaysOfWeek.Monday, DaysOfWeek.Monday.CommonFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));
        Assert.AreEqual(DaysOfWeek.None, DaysOfWeek.Monday.CommonFlags(DaysOfWeek.Wednesday));

        // RemoveFlags
        Assert.AreEqual(DaysOfWeek.Wednesday, (DaysOfWeek.Monday | DaysOfWeek.Wednesday).RemoveFlags(DaysOfWeek.Monday));
        Assert.AreEqual(DaysOfWeek.None, (DaysOfWeek.Monday | DaysOfWeek.Wednesday).RemoveFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));

        // GetFlags, splits out the individual flags in increasing value order
        List<DaysOfWeek> flags = DaysOfWeek.Weekend.GetFlags().ToList();
        Assert.AreEqual(2, flags.Count);
        Assert.AreEqual(DaysOfWeek.Sunday, flags[0]);
        Assert.AreEqual(DaysOfWeek.Saturday, flags[1]);

        // GetAllFlags
        Assert.AreEqual(DaysOfWeek.All, FlagEnums.GetAllFlags<DaysOfWeek>());
    }

    [Test]
    public void Name()
    {
        Assert.AreEqual("Equals", NumericOperator.Equals.GetName());
        Assert.IsNull(((NumericOperator)(-1)).GetName());
    }

    [Test]
    public void Attributes()
    {
        Assert.IsTrue(NumericOperator.GreaterThanOrEquals.GetMember().Attributes.Has<PrimaryEnumMemberAttribute>());
        Assert.IsFalse(Enums.GetMember<NumericOperator>("NotLessThan").Attributes.Has<PrimaryEnumMemberAttribute>());
        Assert.AreEqual("Is not", NumericOperator.NotEquals.GetMember().Attributes.Get<DescriptionAttribute>().Description);
        Assert.IsNull(NumericOperator.LessThan.GetMember().Attributes.Get<DescriptionAttribute>());
    }

    [Test]
    public void Parsing()
    {
        Assert.AreEqual(NumericOperator.GreaterThan, Enums.Parse<NumericOperator>("GreaterThan"));
        Assert.AreEqual(NumericOperator.NotEquals, Enums.Parse<NumericOperator>("1"));
        Assert.AreEqual(NumericOperator.Equals, Enums.Parse<NumericOperator>("Is", EnumFormat.Description));

        Assert.AreEqual(DaysOfWeek.Monday | DaysOfWeek.Wednesday, Enums.Parse<DaysOfWeek>("Monday, Wednesday"));
        Assert.AreEqual(DaysOfWeek.Tuesday | DaysOfWeek.Thursday, FlagEnums.ParseFlags<DaysOfWeek>("Tuesday | Thursday", delimiter: "|"));
    }

    [Test]
    public void Description()
    {
        Assert.AreEqual("Is", NumericOperator.Equals.AsString(EnumFormat.Description));
        Assert.IsNull(NumericOperator.LessThan.AsString(EnumFormat.Description));
        // Gets the description if applied, otherwise the name
        Assert.AreEqual("LessThan", NumericOperator.LessThan.AsString(EnumFormat.Description, EnumFormat.Name));
    }

    [Test]
    public void CustomEnumFormat()
    {
        EnumFormat symbolFormat = Enums.RegisterCustomEnumFormat(member => member.Attributes.Get<SymbolAttribute>()?.Symbol);
        Assert.AreEqual(">", NumericOperator.GreaterThan.AsString(symbolFormat));
        Assert.AreEqual(NumericOperator.LessThan, Enums.Parse<NumericOperator>("<", symbolFormat));
    }

    enum NumericOperator
    {
        [Description("Is")]
        [Symbol("=")]
        Equals,
        [Description("Is not")]
        [Symbol("!=")]
        NotEquals,
        [Symbol("<")]
        LessThan,
        [PrimaryEnumMember] // Indicates enum member as primary duplicate for extension methods
        [Symbol(">=")]
        GreaterThanOrEquals,
        NotLessThan = GreaterThanOrEquals,
        [Symbol(">")]
        GreaterThan,
        [PrimaryEnumMember]
        [Symbol("<=")]
        LessThanOrEquals,
        NotGreaterThan = LessThanOrEquals
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

    [Flags]
    [DayTypeValidator]
    enum DayType
    {
        Weekday = 1,
        Weekend = 2,
        Holiday = 4
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

    [AttributeUsage(AttributeTargets.Enum)]
    class DayTypeValidatorAttribute : Attribute, IEnumValidatorAttribute<DayType>
    {
        public bool IsValid(DayType value) => value == DayType.Weekday || value == DayType.Weekend || value == (DayType.Weekday | DayType.Holiday) || value == (DayType.Weekend | DayType.Holiday);
    }
}