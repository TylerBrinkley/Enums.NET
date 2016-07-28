using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using EnumsNET;
using NUnit.Framework;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

[TestFixture]
class EnumsNETDemo
{
    enum NumericFilter
    {
        [Description("Is")]
        [EnumMember(Value = "=")]
        Equals,
        [Description("Is not")]
        [EnumMember(Value = "!=")]
        NotEquals,
        [EnumMember(Value = "<")]
        LessThan,
        [PrimaryEnumMember]
        [EnumMember(Value = ">=")]
        GreaterThanOrEquals,
        NotLessThan = GreaterThanOrEquals,
        [EnumMember(Value = ">")]
        GreaterThan,
        [PrimaryEnumMember]
        [EnumMember(Value = "<=")]
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

    [Test]
    public void Enumerate()
    {
        foreach (EnumMember<NumericFilter> member in Enums.GetEnumMembers<NumericFilter>())
        {
            NumericFilter value = member.Value;
            string name = member.Name;
            // Do stuff
        }

        foreach (NumericFilter value in Enums.GetValues<NumericFilter>())
        {
            // Do stuff
        }

        foreach (string name in Enums.GetNames<NumericFilter>())
        {
            // Do stuff
        }
    }

    [Test]
    public void Validate()
    {
        // Standard Enums, checks for defined
        Assert.IsTrue(NumericFilter.LessThan.IsValid());
        Assert.IsFalse(((NumericFilter)20).IsValid());

        // Flag Enums, checks is valid flag combination or is defined
        Assert.IsTrue((DaysOfWeek.Sunday | DaysOfWeek.Wednesday).IsValid());
        Assert.IsFalse((DaysOfWeek.Sunday | DaysOfWeek.Wednesday | (DaysOfWeek.All + 1)).IsValid());
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

        // ExcludeFlags
        Assert.AreEqual(DaysOfWeek.Wednesday, (DaysOfWeek.Monday | DaysOfWeek.Wednesday).ExcludeFlags(DaysOfWeek.Monday));
        Assert.AreEqual(DaysOfWeek.None, (DaysOfWeek.Monday | DaysOfWeek.Wednesday).ExcludeFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));

        // GetFlags
        foreach (DaysOfWeek dayOfWeek in DaysOfWeek.Weekdays.GetFlags())
        {
            // Do Stuff
        }
        List<DaysOfWeek> flags = DaysOfWeek.Weekend.GetFlags().ToList();
        Assert.AreEqual(2, flags.Count);
        Assert.AreEqual(DaysOfWeek.Sunday, flags[0]);
        Assert.AreEqual(DaysOfWeek.Saturday, flags[1]);
    }

    [Test]
    public void Name()
    {
        Assert.AreEqual("Equals", NumericFilter.Equals.GetName());
        Assert.AreEqual("LessThan", NumericFilter.LessThan.AsString(EnumFormat.Name));
        Assert.AreEqual("GreaterThan", NumericFilter.GreaterThan.GetEnumMember().Name);
    }

    [Test]
    public void Description()
    {
        Assert.AreEqual("Is", NumericFilter.Equals.AsString(EnumFormat.Description));
        Assert.IsNull(NumericFilter.LessThan.AsString(EnumFormat.Description));
        Assert.AreEqual("Is", NumericFilter.Equals.GetEnumMember().Description);
        Assert.IsNull(NumericFilter.LessThan.GetEnumMember().Description);
        Assert.AreEqual("Is", Enums.GetDescription(NumericFilter.Equals));
        Assert.IsNull(Enums.GetDescription(NumericFilter.LessThan));
    }

    [Test]
    public void Attributes()
    {
        Assert.IsTrue(NumericFilter.GreaterThanOrEquals.GetEnumMember().HasAttribute<PrimaryEnumMemberAttribute>());
        Assert.IsFalse(Enums.GetEnumMember<NumericFilter>("NotLessThan").HasAttribute<PrimaryEnumMemberAttribute>());
        Assert.AreEqual("Is not", NumericFilter.NotEquals.GetEnumMember().GetAttribute<DescriptionAttribute>().Description);
        Assert.IsNull(NumericFilter.LessThan.GetEnumMember().GetAttributeSelect((DescriptionAttribute attr) => attr.Description));
    }

    [Test]
    public void Parsing()
    {
        Assert.AreEqual(NumericFilter.GreaterThan, Enums.Parse<NumericFilter>("GreaterThan"));
        Assert.AreEqual(NumericFilter.NotEquals, Enums.Parse<NumericFilter>("1"));
        Assert.AreEqual(NumericFilter.Equals, Enums.Parse<NumericFilter>("Is", EnumFormat.Description));

        Assert.AreEqual(DaysOfWeek.Monday | DaysOfWeek.Wednesday, Enums.Parse<DaysOfWeek>("Monday, Wednesday"));
        Assert.AreEqual(DaysOfWeek.Tuesday | DaysOfWeek.Thursday, FlagEnums.ParseFlags<DaysOfWeek>("Tuesday | Thursday", delimiter: "|"));
    }

    [Test]
    public void CustomEnumFormat()
    {
        EnumFormat enumMemberValueFormat = Enums.RegisterCustomEnumFormat(member => member.GetAttributeSelect((EnumMemberAttribute attr) => attr.Value));
        Assert.AreEqual(">", NumericFilter.GreaterThan.AsString(enumMemberValueFormat));
        Assert.AreEqual(NumericFilter.LessThan, Enums.Parse<NumericFilter>("<", enumMemberValueFormat));
    }
}