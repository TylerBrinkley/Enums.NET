// Enums.NET
// Copyright 2016 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
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
    public class FlagEnumsTest
    {
        #region "Properties"
        [Test]
        public void IsFlagEnum_Test()
        {
            Assert.IsTrue(FlagEnums.IsFlagEnum<ColorFlagEnum>());
            Assert.IsFalse(FlagEnums.IsFlagEnum<NonContiguousEnum>());
        }

        [Test]
        public void GetAllFlags_Test()
        {
            Assert.AreEqual(ColorFlagEnum.All, FlagEnums.GetAllFlags<ColorFlagEnum>());
        }
        #endregion

        #region Main Methods
        [Test]
        public void IsValidFlagCombination_Test()
        {
            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
            {
                if (i >= 0 && i <= 15)
                {
                    Assert.IsTrue(FlagEnums.IsValidFlagCombination((ColorFlagEnum)i));
                }
                else
                {
                    Assert.IsFalse(FlagEnums.IsValidFlagCombination((ColorFlagEnum)i));
                }
            }
        }

        [Test]
        public void FormatFlags_ReturnsValidString_WhenUsingValidValue()
        {
            Assert.AreEqual("Black", FlagEnums.FormatFlags(ColorFlagEnum.Black));
            Assert.AreEqual("Red", FlagEnums.FormatFlags(ColorFlagEnum.Red));
            Assert.AreEqual("Green", FlagEnums.FormatFlags(ColorFlagEnum.Green));
            Assert.AreEqual("Blue", FlagEnums.FormatFlags(ColorFlagEnum.Blue));
            Assert.AreEqual("Red, Green", FlagEnums.FormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Green));
            Assert.AreEqual("Red, Blue", FlagEnums.FormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Blue));
            Assert.AreEqual("Green, Blue", FlagEnums.FormatFlags(ColorFlagEnum.Green | ColorFlagEnum.Blue));
            Assert.AreEqual("Red, Green, Blue", FlagEnums.FormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue));
            Assert.AreEqual("UltraViolet", FlagEnums.FormatFlags(ColorFlagEnum.UltraViolet));
            Assert.AreEqual("All", FlagEnums.FormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet));
        }

        [Test]
        public void FormatFlags_ReturnsValidString_WhenUsingValidValueWithCustomDelimiter()
        {
            Assert.AreEqual("Black", FlagEnums.FormatFlags(ColorFlagEnum.Black, " | "));
            Assert.AreEqual("Red", FlagEnums.FormatFlags(ColorFlagEnum.Red, " | "));
            Assert.AreEqual("Green", FlagEnums.FormatFlags(ColorFlagEnum.Green, " | "));
            Assert.AreEqual("Blue", FlagEnums.FormatFlags(ColorFlagEnum.Blue, " | "));
            Assert.AreEqual("Red | Green", FlagEnums.FormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Green, " | "));
            Assert.AreEqual("Red | Blue", FlagEnums.FormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Blue, " | "));
            Assert.AreEqual("Green | Blue", FlagEnums.FormatFlags(ColorFlagEnum.Green | ColorFlagEnum.Blue, " | "));
            Assert.AreEqual("Red | Green | Blue", FlagEnums.FormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, " | "));
            Assert.AreEqual("UltraViolet", FlagEnums.FormatFlags(ColorFlagEnum.UltraViolet, " | "));
            Assert.AreEqual("All", FlagEnums.FormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, " | "));
        }

        [Test]
        public void FormatFlags_ReturnsValidString_WhenUsingInvalidValue()
        {
            Assert.AreEqual("16", FlagEnums.FormatFlags((ColorFlagEnum)16));
        }

        [Test]
        public void FormatFlags_UsesDefaultDelimiter_WhenUsingValidValueWithNullDelimiter()
        {
            var value = ColorFlagEnum.Red | ColorFlagEnum.Green;
            Assert.AreEqual(FlagEnums.FormatFlags(value), FlagEnums.FormatFlags(value, (string)null));
        }

        [Test]
        public void FormatFlags_UsesDefaultDelimiter_WhenUsingValidValueWithEmptyDelimiter()
        {
            var value = ColorFlagEnum.Red | ColorFlagEnum.Green;
            Assert.AreEqual(FlagEnums.FormatFlags(value), FlagEnums.FormatFlags(value, string.Empty));
        }

        [Test]
        public void GetFlags_ReturnsValidArray_WhenUsingValidValue()
        {
            CollectionAssert.AreEqual(new ColorFlagEnum[0], ColorFlagEnum.Black.GetFlags());
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Red }, ColorFlagEnum.Red.GetFlags());
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Green }, ColorFlagEnum.Green.GetFlags());
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Blue }, ColorFlagEnum.Blue.GetFlags());
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Red, ColorFlagEnum.Green }, (ColorFlagEnum.Red | ColorFlagEnum.Green).GetFlags());
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Red, ColorFlagEnum.Blue }, (ColorFlagEnum.Red | ColorFlagEnum.Blue).GetFlags());
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Green, ColorFlagEnum.Blue }, (ColorFlagEnum.Green | ColorFlagEnum.Blue).GetFlags());
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue }, (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).GetFlags());
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.UltraViolet }, ColorFlagEnum.UltraViolet.GetFlags());
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet }, (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet).GetFlags());
        }

        [Test]
        public void GetFlags_ReturnsValidArray_WhenUsingInvalidValue()
        {
            CollectionAssert.AreEqual(new ColorFlagEnum[0], ((ColorFlagEnum)16).GetFlags());
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Blue }, (ColorFlagEnum.Blue | (ColorFlagEnum)16).GetFlags());
        }

        [Test]
        public void HasAnyFlags()
        {
            Assert.IsFalse(ColorFlagEnum.Black.HasAnyFlags());
            Assert.IsTrue(ColorFlagEnum.Blue.HasAnyFlags());
        }

        [Test]
        public void HasAnyFlags_ReturnsExpected_WhenUsingInvalidValue()
        {
            Assert.IsTrue(((ColorFlagEnum)16).HasAnyFlags());
        }

        [Test]
        public void HasAnyFlags1()
        {
            Assert.IsFalse(ColorFlagEnum.Black.HasAnyFlags(ColorFlagEnum.Blue));
            Assert.IsTrue(ColorFlagEnum.Blue.HasAnyFlags(ColorFlagEnum.Blue));
            Assert.IsTrue((ColorFlagEnum.Green | ColorFlagEnum.Blue).HasAnyFlags(ColorFlagEnum.Blue));
            Assert.IsFalse((ColorFlagEnum.Green | ColorFlagEnum.Blue).HasAnyFlags(ColorFlagEnum.Red));
            Assert.IsTrue(ColorFlagEnum.Red.HasAnyFlags(ColorFlagEnum.Red | ColorFlagEnum.Green));
            Assert.IsFalse(ColorFlagEnum.Blue.HasAnyFlags(ColorFlagEnum.Red | ColorFlagEnum.Green));
        }

        [Test]
        public void HasAnyFlags1_ReturnsExpected_WhenUsingInvalidValue()
        {
            Assert.IsFalse(((ColorFlagEnum)16).HasAnyFlags(ColorFlagEnum.Red));
            Assert.IsTrue((((ColorFlagEnum)16) | ColorFlagEnum.Red).HasAnyFlags(ColorFlagEnum.Red));
        }

        [Test]
        public void HasAnyFlags1_ReturnsExpected_WhenUsingInvalidOtherFlags()
        {
            Assert.IsFalse(ColorFlagEnum.Red.HasAnyFlags((ColorFlagEnum)16));
            Assert.IsTrue(ColorFlagEnum.Red.HasAnyFlags(((ColorFlagEnum)16) | ColorFlagEnum.Red));
        }

        [Test]
        public void HasAllFlags()
        {
            Assert.IsTrue(ColorFlagEnum.All.HasAllFlags());
            Assert.IsFalse(ColorFlagEnum.Blue.HasAllFlags());
        }

        [Test]
        public void HasAllFlags_ReturnsExpected_WhenUsingInvalidValue()
        {
            Assert.IsFalse(((ColorFlagEnum)16).HasAllFlags());
            Assert.IsTrue((ColorFlagEnum.All | ((ColorFlagEnum)16)).HasAllFlags());
        }

        [Test]
        public void HasAllFlags1()
        {
            Assert.IsFalse(ColorFlagEnum.Black.HasAllFlags(ColorFlagEnum.Blue));
            Assert.IsTrue(ColorFlagEnum.Blue.HasAllFlags(ColorFlagEnum.Blue));
            Assert.IsTrue((ColorFlagEnum.Green | ColorFlagEnum.Blue).HasAllFlags(ColorFlagEnum.Blue));
            Assert.IsFalse((ColorFlagEnum.Green | ColorFlagEnum.Blue).HasAllFlags(ColorFlagEnum.Red));
            Assert.IsTrue((ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).HasAllFlags(ColorFlagEnum.Green | ColorFlagEnum.Blue));
            Assert.IsFalse(ColorFlagEnum.Red.HasAllFlags(ColorFlagEnum.Red | ColorFlagEnum.Green));
            Assert.IsFalse(ColorFlagEnum.Blue.HasAllFlags(ColorFlagEnum.Red | ColorFlagEnum.Green));
        }

        [Test]
        public void HasAllFlags1_ReturnsExpected_WhenUsingInvalidValue()
        {
            Assert.IsFalse(((ColorFlagEnum)16).HasAllFlags(ColorFlagEnum.Red));
            Assert.IsTrue((((ColorFlagEnum)16) | ColorFlagEnum.Red).HasAllFlags(ColorFlagEnum.Red));
        }

        [Test]
        public void HasAllFlags1_ReturnsExpected_WhenUsingInvalidOtherFlags()
        {
            Assert.IsFalse(ColorFlagEnum.Red.HasAllFlags((ColorFlagEnum)16));
            Assert.IsTrue((ColorFlagEnum.Red | (ColorFlagEnum)16).HasAllFlags((ColorFlagEnum)16));
        }

        [Test]
        public void ToggleFlags()
        {
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.UltraViolet, FlagEnums.ToggleFlags(ColorFlagEnum.Red | ColorFlagEnum.Blue));
        }

        [Test]
        public void ToggleFlags_ReturnsExpected_WhenUsingInvalidValue()
        {
            Assert.AreEqual(ColorFlagEnum.All | ((ColorFlagEnum)16), FlagEnums.ToggleFlags((ColorFlagEnum)16));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue | ((ColorFlagEnum)16), FlagEnums.ToggleFlags(((ColorFlagEnum)16) | ColorFlagEnum.Red | ColorFlagEnum.UltraViolet));
        }

        [Test]
        public void ToggleFlags1()
        {
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ToggleFlags(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, ColorFlagEnum.Green));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ToggleFlags(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, ColorFlagEnum.Red | ColorFlagEnum.UltraViolet));
        }

        [Test]
        public void ToggleFlags1_ReturnsExpected_WhenUsingInvalidValue()
        {
            Assert.AreEqual((((ColorFlagEnum)16) | ColorFlagEnum.Blue), FlagEnums.ToggleFlags((ColorFlagEnum)16, ColorFlagEnum.Blue));
        }

        [Test]
        public void ToggleFlags1_ReturnsExpected_WhenUsingInvalidOtherFlags()
        {
            Assert.AreEqual((((ColorFlagEnum)16) | ColorFlagEnum.Blue), FlagEnums.ToggleFlags(ColorFlagEnum.Blue, (ColorFlagEnum)16));
        }

        [Test]
        public void CommonFlags()
        {
            Assert.AreEqual(ColorFlagEnum.Red & ColorFlagEnum.Green, ColorFlagEnum.Red.CommonFlags(ColorFlagEnum.Green));
            Assert.AreEqual(ColorFlagEnum.Red & ColorFlagEnum.Green, ColorFlagEnum.Green.CommonFlags(ColorFlagEnum.Red));
            Assert.AreEqual(ColorFlagEnum.Blue, ColorFlagEnum.Blue.CommonFlags(ColorFlagEnum.Blue));
        }

        [Test]
        public void CommonFlags_ReturnsExpected_WhenUsingInvalidValue()
        {
            Assert.AreEqual(ColorFlagEnum.Black, ((ColorFlagEnum)16).CommonFlags(ColorFlagEnum.Red));
            Assert.AreEqual(ColorFlagEnum.Red, (((ColorFlagEnum)16) | ColorFlagEnum.Red).CommonFlags(ColorFlagEnum.Red));
        }

        [Test]
        public void CommonFlags_ReturnsExpected_WhenUsingInvalidOtherFlags()
        {
            Assert.AreEqual(ColorFlagEnum.Black, ColorFlagEnum.Red.CommonFlags((ColorFlagEnum)16));
            Assert.AreEqual(ColorFlagEnum.Red, ColorFlagEnum.Red.CommonFlags(((ColorFlagEnum)16) | ColorFlagEnum.Red));
        }

        [Test]
        public void CombineFlags()
        {
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, ColorFlagEnum.Red.CombineFlags(ColorFlagEnum.Green));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, ColorFlagEnum.Green.CombineFlags(ColorFlagEnum.Red));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, ColorFlagEnum.Green.CombineFlags(ColorFlagEnum.Red | ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ColorFlagEnum.Blue.CombineFlags(ColorFlagEnum.Blue));
        }

        [Test]
        public void CombineFlags_ReturnsExpected_WhenUsingInvalidValue()
        {
            Assert.AreEqual(((ColorFlagEnum)16) | ColorFlagEnum.Red, ((ColorFlagEnum)16).CombineFlags(ColorFlagEnum.Red));
        }

        [Test]
        public void CombineFlags_ReturnsExpected_WhenUsingInvalidOtherFlags()
        {
            Assert.AreEqual(((ColorFlagEnum)16) | ColorFlagEnum.Red, ColorFlagEnum.Red.CombineFlags((ColorFlagEnum)16));
        }

        [Test]
        public void ExcludeFlags()
        {
            Assert.AreEqual(ColorFlagEnum.Red, (ColorFlagEnum.Red | ColorFlagEnum.Green).ExcludeFlags(ColorFlagEnum.Green));
            Assert.AreEqual(ColorFlagEnum.Green, ColorFlagEnum.Green.ExcludeFlags(ColorFlagEnum.Red));
            Assert.AreEqual(ColorFlagEnum.Green, (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).ExcludeFlags(ColorFlagEnum.Red | ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).ExcludeFlags(ColorFlagEnum.Red | ColorFlagEnum.UltraViolet));
            Assert.AreEqual(ColorFlagEnum.Black, ColorFlagEnum.Blue.ExcludeFlags(ColorFlagEnum.Blue));
        }

        [Test]
        public void ExcludeFlags_ThrowsArgumentException_WhenUsingInvalidValue()
        {
            Assert.AreEqual(((ColorFlagEnum)16), ((ColorFlagEnum)16).ExcludeFlags(ColorFlagEnum.Red));
            Assert.AreEqual(((ColorFlagEnum)16), (((ColorFlagEnum)16) | ColorFlagEnum.Red).ExcludeFlags(ColorFlagEnum.Red));
        }

        [Test]
        public void ExcludeFlags_ThrowsArgumentException_WhenUsingInvalidOtherFlags()
        {
            Assert.AreEqual(ColorFlagEnum.Red, ColorFlagEnum.Red.ExcludeFlags((ColorFlagEnum)16));
            Assert.AreEqual(ColorFlagEnum.Black, ColorFlagEnum.Red.ExcludeFlags(((ColorFlagEnum)16) | ColorFlagEnum.Red));
        }
        #endregion

        #region Parsing
        [Test]
        public void FlagEnumsParse_ReturnsValidValue_WhenUsingValidName()
        {
            Assert.AreEqual(ColorFlagEnum.Black, FlagEnums.ParseFlags<ColorFlagEnum>("Black"));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("Red"));
            Assert.AreEqual(ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("Green"));
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue , Red"));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue , Green"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue , Green , Red"));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("UltraViolet"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>(" Blue ,UltraViolet,Green , Red"));
        }

        [Test]
        public void FlagEnumsParse_ReturnsValidValue_WhenUsingValidNumber()
        {
            Assert.AreEqual(ColorFlagEnum.Black, FlagEnums.ParseFlags<ColorFlagEnum>("0"));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("1"));
            Assert.AreEqual(ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("2"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("+3"));
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("4"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("5"));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("6"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("7"));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("+8"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>(" Blue ,UltraViolet,2 , Red"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("15"));
        }

        [Test]
        public void FlagEnumsParse_ReturnsValidValue_WhenUsingValidStringWhileIgnoringCase()
        {
            Assert.AreEqual(ColorFlagEnum.Black, FlagEnums.ParseFlags<ColorFlagEnum>("black", true));
            Assert.AreEqual(ColorFlagEnum.Black, FlagEnums.ParseFlags<ColorFlagEnum>("0", true));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("ReD", true));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("1", true));
            Assert.AreEqual(ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("GrEeN", true));
            Assert.AreEqual(ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("2", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("3", true));
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("blue", true));
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("4", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("BLUE , red", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("5", true));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("BlUe, GReen", true));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("6", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Red ,    BluE ,greEn", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("7", true));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("UltRaVioLet", true));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("8", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>(" GREEN ,blue,UltraViolet , 1", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("15", true));
        }

        [Test]
        public void FlagEnumsParse_ReturnsValidValue_WhenUsingValidStringWithCustomDelimiter()
        {
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("Red", "|"));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("1", "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue | Red", "|"));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue | Green", "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue | Green | Red", "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>(" Blue |UltraViolet|2 | Red", " | "));
        }

        [Test]
        public void FlagEnumsParse_ReturnsValidValue_WhenUsingValidStringWithCustomDelimiterWhileIgnoringCase()
        {
            Assert.AreEqual(ColorFlagEnum.Black, FlagEnums.ParseFlags<ColorFlagEnum>("black", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Black, FlagEnums.ParseFlags<ColorFlagEnum>("0", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("ReD", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("1", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("GrEeN", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("2", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("RED|green", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("3", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("blue", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("4", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("BLUE | red", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("5", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("BlUe| GReen", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("6", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("BluE |    greEn |Red", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("7", true, "|"));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("UltRaVioLet", true, "|"));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("8", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>(" GREEN |blue|UltraViolet | 1", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("15", true, "|"));
        }

        [Test]
        public void FlagEnumsParse_ThrowsArgumentNullException_WhenUsingNullString()
        {
            TestHelper.ExpectException<ArgumentNullException>(() => FlagEnums.ParseFlags<ColorFlagEnum>(null));
        }

        [Test]
        public void FlagEnumsParse_ThrowsArgumentException_WhenUsingWhiteSpaceString()
        {
            TestHelper.ExpectException<ArgumentException>(() => FlagEnums.ParseFlags<ColorFlagEnum>(" "));
        }

        [Test]
        public void FlagEnumsParse_ThrowsArgumentException_WhenUsingUndefinedString()
        {
            TestHelper.ExpectException<ArgumentException>(() => FlagEnums.ParseFlags<ColorFlagEnum>("Turquoise"));
        }

        [Test]
        public void FlagEnumsParse_ReturnsValidValue_WhenUsingInvalidNumber()
        {
            Assert.AreEqual((ColorFlagEnum)16, FlagEnums.ParseFlags<ColorFlagEnum>("16"));
        }

        [Test]
        public void FlagEnumsParse_ThrowsOverflowException_WhenUsingLargeNumber()
        {
            TestHelper.ExpectException<OverflowException>(() => FlagEnums.ParseFlags<ColorFlagEnum>("128"));
        }

        [Test]
        public void FlagEnumsParse_UsesDefaultDelimiter_WhenUsingNullDelimiter()
        {
            Assert.AreEqual(FlagEnums.ParseFlags<ColorFlagEnum>("Red, Green"), FlagEnums.ParseFlags<ColorFlagEnum>("Red, Green", (string)null));
        }

        [Test]
        public void FlagEnumsParse_UsesDefaultDelimiter_WhenUsingEmptyStringDelimiter()
        {
            Assert.AreEqual(FlagEnums.ParseFlags<ColorFlagEnum>("Red, Green"), FlagEnums.ParseFlags<ColorFlagEnum>("Red, Green", string.Empty));
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsValidValue_WhenUsingValidName()
        {
            ColorFlagEnum result;
            Assert.IsTrue(FlagEnums.TryParseFlags("Black", out result));
            Assert.AreEqual(ColorFlagEnum.Black, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("Red", out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("Green", out result));
            Assert.AreEqual(ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("Red, Green", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("Blue", out result));
            Assert.AreEqual(ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("Blue , Red", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("Blue , Green", out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("Blue , Green , Red", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("UltraViolet", out result));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParseFlags(" Blue ,UltraViolet,Green , Red", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsValidValue_WhenUsingValidNumber()
        {
            ColorFlagEnum result;
            Assert.IsTrue(FlagEnums.TryParseFlags("0", out result));
            Assert.AreEqual(ColorFlagEnum.Black, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("1", out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("+2", out result));
            Assert.AreEqual(ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("3", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("4", out result));
            Assert.AreEqual(ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("+5", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("6", out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("7", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("8", out result));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParseFlags(" Blue ,UltraViolet,+2 , Red", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("15", out result));
            Assert.AreEqual(ColorFlagEnum.All, result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsValidValue_WhenUsingValidStringWhileIgnoringCase()
        {
            ColorFlagEnum result;
            Assert.IsTrue(FlagEnums.TryParseFlags("black", true, out result));
            Assert.AreEqual(ColorFlagEnum.Black, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("0", true, out result));
            Assert.AreEqual(ColorFlagEnum.Black, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("ReD", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("1", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("GrEeN", true, out result));
            Assert.AreEqual(ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("2", true, out result));
            Assert.AreEqual(ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("3", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("blue", true, out result));
            Assert.AreEqual(ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("4", true, out result));
            Assert.AreEqual(ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("BLUE , red", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("5", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("BlUe, GReen", true, out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("6", true, out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("Red ,    BluE ,greEn", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("7", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("UltRaVioLet", true, out result));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("8", true, out result));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParseFlags(" GREEN ,blue,UltraViolet , 1", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("15", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsValidValue_WhenUsingValidStringWithCustomDelimiter()
        {
            ColorFlagEnum result;
            Assert.IsTrue(FlagEnums.TryParseFlags("Red", "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("1", "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("Blue | Red", "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("Blue | Green", "|", out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("Blue | Green | Red", "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags(" Blue |UltraViolet|2 | Red", " | ", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsValidValue_WhenUsingValidStringWithCustomDelimiterWhileIgnoringCase()
        {
            ColorFlagEnum result;
            Assert.IsTrue(FlagEnums.TryParseFlags("black", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Black, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("0", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Black, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("ReD", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("1", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("GrEeN", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("2", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("RED|green", true, "| ", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("3", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("blue", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("4", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("BLUE | red", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("5", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("BlUe| GReen", true, " | ", out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("6", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("BluE |    greEn |Red", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("7", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("UltRaVioLet", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("8", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParseFlags(" GREEN |blue|UltraViolet | 1", true, " |", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParseFlags("15", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsFalse_WhenUsingNullString()
        {
            ColorFlagEnum result;
            Assert.IsFalse(FlagEnums.TryParseFlags(null, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsFalse_WhenUsingWhiteSpaceString()
        {
            ColorFlagEnum result;
            Assert.IsFalse(FlagEnums.TryParseFlags(" ", out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsFalse_WhenUsingUndefinedString()
        {
            ColorFlagEnum result;
            Assert.IsFalse(FlagEnums.TryParseFlags("Turquoise", out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsTrue_WhenUsingInvalidNumber()
        {
            ColorFlagEnum result;
            Assert.IsTrue(FlagEnums.TryParseFlags("16", out result));
            Assert.AreEqual((ColorFlagEnum)16, result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsFalse_WhenUsingLargeNumber()
        {
            ColorFlagEnum result;
            Assert.IsFalse(FlagEnums.TryParseFlags("128", out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
        }

        [Test]
        public void FlagEnumsTryParse_UsesDefaultDelimiter_WhenUsingNullDelimiter()
        {
            ColorFlagEnum result0;
            ColorFlagEnum result1;
            Assert.AreEqual(FlagEnums.TryParseFlags("Red, Green", out result0), FlagEnums.TryParseFlags("Red, Green", null, out result1));
            Assert.AreEqual(result0, result1);
        }

        [Test]
        public void FlagEnumsTryParse_UsesDefaultDelimiter_WhenUsingEmptyStringDelimiter()
        {
            ColorFlagEnum result0;
            ColorFlagEnum result1;
            Assert.AreEqual(FlagEnums.TryParseFlags("Red, Green", out result0), FlagEnums.TryParseFlags("Red, Green", string.Empty, out result1));
            Assert.AreEqual(result0, result1);
        }
        #endregion
    }
}
