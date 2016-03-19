using System;
using EnumsNET.NonGeneric;
using EnumsNET.Tests.TestEnums;
#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

namespace EnumsNET.Tests
{
    [TestFixture]
    public class NonGenericEnumsTest
    {
        [Test]
        public void NonGenericEnumsIsContiguous()
        {
            Assert.IsTrue(NonGenericEnums.IsContiguous(typeof(DateFilterOperator)));
            Assert.IsTrue(NonGenericEnums.IsContiguous(typeof(ContiguousUInt64Enum)));
            Assert.IsFalse(NonGenericEnums.IsContiguous(typeof(NonContiguousEnum)));
            Assert.IsFalse(NonGenericEnums.IsContiguous(typeof(NonContiguousUInt64Enum)));
        }

        [Test]
        public void NonGenericEnumsNullableTest()
        {
            Assert.AreEqual("Black", NonGenericEnums.GetName(typeof(ConsoleColor?), (ConsoleColor?)ConsoleColor.Black));
            Assert.AreEqual("Black", NonGenericEnums.GetName(typeof(ConsoleColor?), ConsoleColor.Black));
            Assert.AreEqual(null, NonGenericEnums.GetName(typeof(ConsoleColor?), null));
        }
    }
}
