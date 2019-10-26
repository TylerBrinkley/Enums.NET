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
using NUnit.Framework;

namespace EnumsNET.Tests
{
    [TestFixture]
    public class UnsafeEnumsTest
    {
        [Test]
        public void UnsafeEnumsBasicTest()
        {
            Assert.Throws<ArgumentException>(() => Enums.GetUnderlyingTypeUnsafe<int>());
            Assert.Throws<ArgumentException>(() => Enums.GetUnderlyingTypeUnsafe<Enum>());
            Assert.Throws<ArgumentException>(() => Enums.GetUnderlyingTypeUnsafe<string>());
            Assert.AreEqual(typeof(short), Enums.GetUnderlyingTypeUnsafe<DateFilterOperator>());
        }

        [Test]
        public void UnsafeFlagEnumsBasicTest()
        {
            Assert.Throws<ArgumentException>(() => FlagEnums.GetAllFlagsUnsafe<int>());
            Assert.Throws<ArgumentException>(() => FlagEnums.GetAllFlagsUnsafe<Enum>());
            Assert.Throws<ArgumentException>(() => FlagEnums.GetAllFlagsUnsafe<string>());
            Assert.AreEqual(ColorFlagEnum.All, FlagEnums.GetAllFlagsUnsafe<ColorFlagEnum>());
        }
    }
}
