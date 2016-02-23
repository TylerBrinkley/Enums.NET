// Enums.NET
// Copyright 2016 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using EnumsNET.Tests.TestEnums;
using EnumsNET.Unsafe;
#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#elif DNXCORE50
using Xunit;
using Test = Xunit.FactAttribute;
using Assert = Newtonsoft.Json.Tests.XUnitAssert;
#else
using NUnit.Framework;
#endif

namespace EnumsNET.Tests
{
    [TestFixture]
    public class UnsafeEnumsTest
    {
        [Test]
        public void UnsafeEnumsBasicTest()
        {
            TestHelper.ExpectException<ArgumentException>(() => UnsafeEnums.IsContiguous<int>());
            TestHelper.ExpectException<ArgumentException>(() => UnsafeEnums.IsContiguous<Enum>());
            TestHelper.ExpectException<ArgumentException>(() => UnsafeEnums.IsContiguous<string>());
            Assert.IsTrue(UnsafeEnums.IsContiguous<DateFilterOperator>());
        }

        [Test]
        public void UnsafeFlagEnumsBasicTest()
        {
            TestHelper.ExpectException<ArgumentException>(() => UnsafeFlagEnums.GetAllFlags<int>());
            TestHelper.ExpectException<ArgumentException>(() => UnsafeFlagEnums.GetAllFlags<Enum>());
            TestHelper.ExpectException<ArgumentException>(() => UnsafeFlagEnums.GetAllFlags<string>());
            Assert.AreEqual(ColorFlagEnum.All, UnsafeFlagEnums.GetAllFlags<ColorFlagEnum>());
        }
    }
}
