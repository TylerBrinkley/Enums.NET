using System;
using EnumsNET.NonGeneric;
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
            Assert.AreEqual(typeof(short), NonGenericEnums.GetUnderlyingType(typeof(DateFilterOperator)));
            Assert.AreEqual(typeof(ulong), NonGenericEnums.GetUnderlyingType(typeof(ContiguousUInt64Enum)));
            Assert.AreEqual(typeof(long), NonGenericEnums.GetUnderlyingType(typeof(NonContiguousEnum)));
        }

        [Test]
        public void NonGenericEnumsNullableTest()
        {
            Assert.AreEqual("Today", NonGenericEnums.GetName(typeof(DateFilterOperator?), (DateFilterOperator?)DateFilterOperator.Today));
            Assert.AreEqual("Today", NonGenericEnums.GetName(typeof(DateFilterOperator?), DateFilterOperator.Today));
            Assert.AreEqual(null, NonGenericEnums.GetName(typeof(DateFilterOperator?), null));
        }
    }
}
