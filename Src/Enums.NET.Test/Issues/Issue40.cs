using System;
using Xunit;

namespace EnumsNET.Tests.Issues;

public class Issue40
{
    [Fact]
    public void GetFlags_SuccessfullyEnumerates_WhenUsingLargeCertainEnumValue()
    {
        Assert.Equal([MyEnum.Val1, MyEnum.Val40], ((MyEnum)1610612801).GetFlags());
    }

    [Flags]
    public enum MyEnum
    {
        Unknown = 0x0,
        Val1 = 0x1,
        Val4 = 0x4,
        Val8 = 0x8,
        Val10 = 0x10,
        Val40 = 0x40,
        Val80 = 0x80,
    }
}
