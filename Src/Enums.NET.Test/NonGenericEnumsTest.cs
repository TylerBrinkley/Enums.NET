using System.Collections.Generic;
using System.Linq;
using EnumsNET.Tests.TestEnums;
using Xunit;

namespace EnumsNET.Tests;

public class NonGenericEnumsTest
{
    [Fact]
    public void NonGenericEnumsGetUnderlyingType()
    {
        Assert.Equal(typeof(short), Enums.GetUnderlyingType(typeof(DateFilterOperator)));
        Assert.Equal(typeof(ulong), Enums.GetUnderlyingType(typeof(ContiguousUInt64Enum)));
        Assert.Equal(typeof(long), Enums.GetUnderlyingType(typeof(NonContiguousEnum)));
    }

    [Fact]
    public void NonGenericEnumsNullableTest()
    {
        Assert.Equal("Today", Enums.GetName(typeof(DateFilterOperator), (DateFilterOperator?)DateFilterOperator.Today));
        Assert.Equal("Today", Enums.GetName(typeof(DateFilterOperator), DateFilterOperator.Today));
    }

    [Fact]
    public void GetFlags_ReturnsValidValues_WhenUsingValidValue()
    {
        Assert.Equal([ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet], Enums.GetMember(typeof(ColorFlagEnum), ColorFlagEnum.All).GetFlags().Cast<ColorFlagEnum>());
    }
}