using EnumsNET.Tests.TestEnums;
using NUnit.Framework;

namespace EnumsNET.Tests
{
    [TestFixture]
    public class NonGenericEnumsTest
    {
        [Test]
        public void NonGenericEnumsGetUnderlyingType()
        {
            Assert.AreEqual(typeof(short), Enums.GetUnderlyingType(typeof(DateFilterOperator)));
            Assert.AreEqual(typeof(ulong), Enums.GetUnderlyingType(typeof(ContiguousUInt64Enum)));
            Assert.AreEqual(typeof(long), Enums.GetUnderlyingType(typeof(NonContiguousEnum)));
        }

        [Test]
        public void NonGenericEnumsNullableTest()
        {
            Assert.AreEqual("Today", Enums.GetName(typeof(DateFilterOperator?), (DateFilterOperator?)DateFilterOperator.Today));
            Assert.AreEqual("Today", Enums.GetName(typeof(DateFilterOperator?), DateFilterOperator.Today));
            Assert.AreEqual(null, Enums.GetName(typeof(DateFilterOperator?), null));
        }

        [Test]
        public void GetFlags_ReturnsValidValues_WhenUsingValidValue()
        {
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet }, Enums.GetMember(typeof(ColorFlagEnum), ColorFlagEnum.All).GetFlags());
        }
    }
}