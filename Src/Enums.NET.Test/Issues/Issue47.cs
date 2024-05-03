using System;
using Xunit;

namespace EnumsNET.Tests.Issues;

public class Issue47
{
    [Fact]
    public void AsString_SuccessfullyReturnsValue_WhenUsingLargeFlagEnumValue()
    {
        Assert.Equal("Val1, Val30", (MyEnum.Val1 | MyEnum.Val30).AsString());
    }

#if SPAN
    [Fact]
    public void TryFormat_SuccessfullyReturnsValue_WhenUsingLargeFlagEnumValue()
    {
        var destination = new char[20];
        Assert.True((MyEnum.Val1 | MyEnum.Val30).TryFormat(destination, out var charsWritten));
        Assert.Equal(11, charsWritten);
        Assert.Equal("Val1, Val30", new string(destination[..charsWritten]));
    }
#endif

    [Flags]
    public enum MyEnum
    {
        Unknown = 0,
        Val1 = 1 << 0,
        Val30 = 1 << 30,
    }
}
