using EnumsNET.NonGeneric;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnumsNET.Test
{
    [TestClass]
    public class NonGenericEnumsTests
    {
        [TestMethod]
        public void NonGenericEnumsIsContiguous()
        {
            Assert.IsTrue(NonGenericEnums.IsContiguous(typeof(DateFilterOperator)));
            Assert.IsTrue(NonGenericEnums.IsContiguous(typeof(ContiguousUInt64Enum)));
            Assert.IsFalse(NonGenericEnums.IsContiguous(typeof(NonContiguousEnum)));
            Assert.IsFalse(NonGenericEnums.IsContiguous(typeof(NonContiguousUInt64Enum)));
        }
    }
}
