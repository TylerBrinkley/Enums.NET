using System;
using NUnit.Framework;

namespace EnumsNET.Tests.Issues
{
    [TestFixture]
    public class Issue47
    {
        [Test]
        public void AsString_SuccessfullyReturnsValue_WhenUsingLargeFlagEnumValue()
        {
            Assert.AreEqual("Val1, Val30", (MyEnum.Val1 | MyEnum.Val30).AsString());
        }

#if SPAN
        [Test]
        public void TryFormat_SuccessfullyReturnsValue_WhenUsingLargeFlagEnumValue()
        {
            var destination = new char[20];
            Assert.True((MyEnum.Val1 | MyEnum.Val30).TryFormat(destination, out var charsWritten));
            Assert.AreEqual(11, charsWritten);
            Assert.AreEqual("Val1, Val30", new string(destination[..charsWritten]));
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
}
