using System;
using System.Collections.Generic;
using System.Linq;
using EnumsNET;
using NUnit.Framework;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

[TestFixture]
class EnumsNETDemo
{
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
        // Standard Enums
        Assert.IsTrue(NumericFilter.LessThan.IsValid());
        Assert.IsFalse(((NumericFilter)20).IsValid());

        // Flag Enums
        Assert.IsTrue((DaysOfWeek.Sunday | DaysOfWeek.Saturday).IsValid());
        Assert.IsFalse((DaysOfWeek.Sunday | DaysOfWeek.Saturday | (DaysOfWeek.All + 1)).IsValid());
    }

    [Test]
    public void FlagEnumOperations()
    {
        // CombineFlags ~ bitwise OR
        Assert.AreEqual(DaysOfWeek.Monday | DaysOfWeek.Wednesday, DaysOfWeek.Monday.CombineFlags(DaysOfWeek.Wednesday));
        Assert.AreEqual(DaysOfWeek.Monday | DaysOfWeek.Wednesday | DaysOfWeek.Friday, FlagEnums.CombineFlags(DaysOfWeek.Monday, DaysOfWeek.Wednesday, DaysOfWeek.Friday));

        // HasAnyFlags
        Assert.IsTrue((DaysOfWeek.Monday | DaysOfWeek.Wednesday).HasAnyFlags(DaysOfWeek.Wednesday));
        Assert.IsFalse((DaysOfWeek.Monday | DaysOfWeek.Wednesday).HasAnyFlags(DaysOfWeek.Friday));

        // HasAllFlags
        Assert.IsTrue((DaysOfWeek.Monday | DaysOfWeek.Wednesday).HasAllFlags((DaysOfWeek.Monday | DaysOfWeek.Wednesday)));
        Assert.IsFalse(DaysOfWeek.Monday.HasAllFlags((DaysOfWeek.Monday | DaysOfWeek.Wednesday)));

        // CommonFlags ~ bitwise AND
        Assert.AreEqual(DaysOfWeek.Monday, DaysOfWeek.Monday.CommonFlags(DaysOfWeek.Monday | DaysOfWeek.Wednesday));
        Assert.AreEqual(DaysOfWeek.None, DaysOfWeek.Monday.CommonFlags(DaysOfWeek.Wednesday));

        // ExcludeFlags
        Assert.AreEqual(DaysOfWeek.Wednesday, (DaysOfWeek.Monday | DaysOfWeek.Wednesday).ExcludeFlags(DaysOfWeek.Monday));

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
    public void Description()
    {
        Assert.AreEqual("Is", Enums.GetDescription(NumericFilter.Equals));
        Assert.IsNull(Enums.GetDescription(NumericFilter.LessThan));
        Assert.AreEqual("Is", NumericFilter.Equals.GetEnumMember().Description);
        Assert.IsNull(NumericFilter.LessThan.GetEnumMember().Description);
    }

    [Test]
    public void Attributes()
    {
        Assert.IsTrue(NumericFilter.GreaterThanOrEquals.GetEnumMember().HasAttribute<PrimaryEnumMemberAttribute>());
        Assert.IsFalse(Enums.GetEnumMember<NumericFilter>("NotLessThan").HasAttribute<PrimaryEnumMemberAttribute>());
    }
}

enum NumericFilter
{
    [Description("Is")]
    Equals,
    [Description("Is not")]
    NotEquals,
    LessThan,
    [PrimaryEnumMember]
    GreaterThanOrEquals,
    NotLessThan = GreaterThanOrEquals,
    GreaterThan,
    [PrimaryEnumMember]
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