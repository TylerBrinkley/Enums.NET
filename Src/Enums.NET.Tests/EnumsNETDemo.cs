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
    enum NumericOperator
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
        foreach (EnumMember<NumericOperator> member in Enums.GetEnumMembers<NumericOperator>())
        {
            NumericOperator value = member.Value;
            string name = member.Name;
            // Do stuff
        }
        Assert.AreEqual(8, Enums.GetEnumMembers<NumericOperator>().Count());
        Assert.AreEqual(6, Enums.GetEnumMembers<NumericOperator>(uniqueValued: true).Count());

        foreach (NumericOperator value in Enums.GetValues<NumericOperator>())
        {
            // Do stuff
        }

        foreach (string name in Enums.GetNames<NumericOperator>())
        {
            // Do stuff
        }
    }

    [Test]
    public void Validate()
    {
        // Standard Enums, checks is defined
        Assert.IsTrue(NumericOperator.LessThan.IsValid());
        Assert.IsFalse(((NumericOperator)20).IsValid());

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
        Assert.AreEqual("Equals", NumericOperator.Equals.GetName());
        Assert.AreEqual("LessThan", NumericOperator.LessThan.AsString(EnumFormat.Name));
        Assert.AreEqual("GreaterThan", NumericOperator.GreaterThan.GetEnumMember().Name);
    }

    [Test]
    public void Description()
    {
        Assert.AreEqual("Is", NumericOperator.Equals.AsString(EnumFormat.Description));
        Assert.IsNull(NumericOperator.LessThan.AsString(EnumFormat.Description));
        Assert.AreEqual("Is", NumericOperator.Equals.GetEnumMember().Description);
        Assert.IsNull(NumericOperator.LessThan.GetEnumMember().Description);
        Assert.AreEqual("Is", Enums.GetDescription(NumericOperator.Equals));
        Assert.IsNull(Enums.GetDescription(NumericOperator.LessThan));
    }

    [Test]
    public void Attributes()
    {
        Assert.IsTrue(NumericOperator.GreaterThanOrEquals.GetEnumMember().HasAttribute<PrimaryEnumMemberAttribute>());
        Assert.IsFalse(Enums.GetEnumMember<NumericOperator>("NotLessThan").HasAttribute<PrimaryEnumMemberAttribute>());
        Assert.AreEqual("Is not", NumericOperator.NotEquals.GetEnumMember().GetAttribute<DescriptionAttribute>().Description);
        Assert.IsNull(NumericOperator.LessThan.GetEnumMember().GetAttributeSelect((DescriptionAttribute attr) => attr.Description));
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
    public void CustomEnumFormat()
    {
        EnumFormat enumMemberValueFormat = Enums.RegisterCustomEnumFormat(member => member.GetAttributeSelect((EnumMemberAttribute attr) => attr.Value));
        Assert.AreEqual(">", NumericOperator.GreaterThan.AsString(enumMemberValueFormat));
        Assert.AreEqual(NumericOperator.LessThan, Enums.Parse<NumericOperator>("<", enumMemberValueFormat));
    }
}