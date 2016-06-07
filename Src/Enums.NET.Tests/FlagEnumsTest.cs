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
        public void FormatAsFlags_ReturnsValidString_WhenUsingValidValue()
        {
            Assert.AreEqual("Black", ColorFlagEnum.Black.FormatAsFlags());
            Assert.AreEqual("Red", ColorFlagEnum.Red.FormatAsFlags());
            Assert.AreEqual("Green", ColorFlagEnum.Green.FormatAsFlags());
            Assert.AreEqual("Blue", ColorFlagEnum.Blue.FormatAsFlags());
            Assert.AreEqual("Red, Green", (ColorFlagEnum.Red | ColorFlagEnum.Green).FormatAsFlags());
            Assert.AreEqual("Red, Blue", (ColorFlagEnum.Red | ColorFlagEnum.Blue).FormatAsFlags());
            Assert.AreEqual("Green, Blue", (ColorFlagEnum.Green | ColorFlagEnum.Blue).FormatAsFlags());
            Assert.AreEqual("Red, Green, Blue", (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).FormatAsFlags());
            Assert.AreEqual("UltraViolet", ColorFlagEnum.UltraViolet.FormatAsFlags());
            Assert.AreEqual("All", (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet).FormatAsFlags());
        }

        [Test]
        public void FormatAsFlags_ReturnsValidString_WhenUsingValidValueWithCustomDelimiter()
        {
            Assert.AreEqual("Black", ColorFlagEnum.Black.FormatAsFlags(" | "));
            Assert.AreEqual("Red", ColorFlagEnum.Red.FormatAsFlags(" | "));
            Assert.AreEqual("Green", ColorFlagEnum.Green.FormatAsFlags(" | "));
            Assert.AreEqual("Blue", ColorFlagEnum.Blue.FormatAsFlags(" | "));
            Assert.AreEqual("Red | Green", (ColorFlagEnum.Red | ColorFlagEnum.Green).FormatAsFlags(" | "));
            Assert.AreEqual("Red | Blue", (ColorFlagEnum.Red | ColorFlagEnum.Blue).FormatAsFlags(" | "));
            Assert.AreEqual("Green | Blue", (ColorFlagEnum.Green | ColorFlagEnum.Blue).FormatAsFlags(" | "));
            Assert.AreEqual("Red | Green | Blue", (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).FormatAsFlags(" | "));
            Assert.AreEqual("UltraViolet", ColorFlagEnum.UltraViolet.FormatAsFlags(" | "));
            Assert.AreEqual("All", (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet).FormatAsFlags(" | "));
        }

        [Test]
        public void FormatAsFlags_ReturnsNull_WhenUsingInvalidValue()
        {
            Assert.AreEqual(null, ((ColorFlagEnum)16).FormatAsFlags());
        }

        [Test]
        public void FormatAsFlags_UsesDefaultDelimiter_WhenUsingValidValueWithNullDelimiter()
        {
            var value = ColorFlagEnum.Red | ColorFlagEnum.Green;
            Assert.AreEqual(value.FormatAsFlags(), value.FormatAsFlags((string)null));
        }

        [Test]
        public void FormatAsFlags_UsesDefaultDelimiter_WhenUsingValidValueWithEmptyDelimiter()
        {
            var value = ColorFlagEnum.Red | ColorFlagEnum.Green;
            Assert.AreEqual(value.FormatAsFlags(), value.FormatAsFlags(string.Empty));
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
        public void GetFlags_ReturnsNull_WhenUsingInvalidValue()
        {
            Assert.AreEqual(null, ((ColorFlagEnum)16).GetFlags());
        }

        [Test]
        public void HasAnyFlags()
        {
            Assert.IsFalse(ColorFlagEnum.Black.HasAnyFlags());
            Assert.IsTrue(ColorFlagEnum.Blue.HasAnyFlags());
        }

        [Test]
        public void HasAnyFlags_ThrowsArgumentException_WhenUsingInvalidValue()
        {
            TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).HasAnyFlags());
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
        public void HasAnyFlags1_ThrowsArgumentException_WhenUsingInvalidValue()
        {
            TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).HasAnyFlags(ColorFlagEnum.Red));
        }

        [Test]
        public void HasAnyFlags1_ThrowsArgumentException_WhenUsingInvalidFlagMask()
        {
            TestHelper.ExpectException<ArgumentException>(() => ColorFlagEnum.Red.HasAnyFlags((ColorFlagEnum)16));
        }

        [Test]
        public void HasAllFlags()
        {
            Assert.IsTrue(ColorFlagEnum.All.HasAllFlags());
            Assert.IsFalse(ColorFlagEnum.Blue.HasAllFlags());
        }

        [Test]
        public void HasAllFlags_ThrowsArgumentException_WhenUsingInvalidValue()
        {
            TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).HasAllFlags());
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
        public void HasAllFlags1_ThrowsArgumentException_WhenUsingInvalidValue()
        {
            TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).HasAllFlags(ColorFlagEnum.Red));
        }

        [Test]
        public void HasAllFlags1_ThrowsArgumentException_WhenUsingInvalidFlagMask()
        {
            TestHelper.ExpectException<ArgumentException>(() => ColorFlagEnum.Red.HasAllFlags((ColorFlagEnum)16));
        }

        [Test]
        public void ToggleFlags()
        {
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.UltraViolet, (ColorFlagEnum.Red | ColorFlagEnum.Blue).ToggleFlags());
        }

        [Test]
        public void ToggleFlags_ThrowsArgumentException_WhenUsingInvalidValue()
        {
            TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).ToggleFlags());
        }

        [Test]
        public void ToggleFlags1()
        {
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).ToggleFlags(ColorFlagEnum.Green));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).ToggleFlags(ColorFlagEnum.Red | ColorFlagEnum.UltraViolet));
        }

        [Test]
        public void ToggleFlags1_ThrowsArgumentException_WhenUsingInvalidValue()
        {
            TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).ToggleFlags(ColorFlagEnum.Blue));
        }

        [Test]
        public void ToggleFlags1_ThrowsArgumentException_WhenUsingInvalidFlagMask()
        {
            TestHelper.ExpectException<ArgumentException>(() => ColorFlagEnum.Blue.ToggleFlags((ColorFlagEnum)16));
        }

        [Test]
        public void CommonFlags()
        {
            Assert.AreEqual(ColorFlagEnum.Red & ColorFlagEnum.Green, ColorFlagEnum.Red.CommonFlags(ColorFlagEnum.Green));
            Assert.AreEqual(ColorFlagEnum.Red & ColorFlagEnum.Green, ColorFlagEnum.Green.CommonFlags(ColorFlagEnum.Red));
            Assert.AreEqual(ColorFlagEnum.Blue, ColorFlagEnum.Blue.CommonFlags(ColorFlagEnum.Blue));
        }

        [Test]
        public void CommonFlags_ThrowsArgumentException_WhenUsingInvalidValue()
        {
            TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).CommonFlags(ColorFlagEnum.Red));
        }

        [Test]
        public void CommonFlags_ThrowsArgumentException_WhenUsingInvalidFlagMask()
        {
            TestHelper.ExpectException<ArgumentException>(() => ColorFlagEnum.Red.CommonFlags((ColorFlagEnum)16));
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
        public void CombineFlags_ThrowsArgumentException_WhenUsingInvalidValue()
        {
            TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).CombineFlags(ColorFlagEnum.Red));
        }

        [Test]
        public void CombineFlags_ThrowsArgumentException_WhenUsingInvalidFlagMask()
        {
            TestHelper.ExpectException<ArgumentException>(() => ColorFlagEnum.Red.CombineFlags((ColorFlagEnum)16));
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
            TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).ExcludeFlags(ColorFlagEnum.Red));
        }

        [Test]
        public void ExcludeFlags_ThrowsArgumentException_WhenUsingInvalidFlagMask()
        {
            TestHelper.ExpectException<ArgumentException>(() => ColorFlagEnum.Red.ExcludeFlags((ColorFlagEnum)16));
        }
        #endregion

        #region Parsing
        [Test]
        public void FlagEnumsParse_ReturnsValidValue_WhenUsingValidName()
        {
            Assert.AreEqual(ColorFlagEnum.Black, FlagEnums.Parse<ColorFlagEnum>("Black"));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.Parse<ColorFlagEnum>("Red"));
            Assert.AreEqual(ColorFlagEnum.Green, FlagEnums.Parse<ColorFlagEnum>("Green"));
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("Blue"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("Blue , Red"));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("Blue , Green"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("Blue , Green , Red"));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>("UltraViolet"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>(" Blue ,UltraViolet,Green , Red"));
        }

        [Test]
        public void FlagEnumsParse_ReturnsValidValue_WhenUsingValidNumber()
        {
            Assert.AreEqual(ColorFlagEnum.Black, FlagEnums.Parse<ColorFlagEnum>("0"));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.Parse<ColorFlagEnum>("1"));
            Assert.AreEqual(ColorFlagEnum.Green, FlagEnums.Parse<ColorFlagEnum>("2"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, FlagEnums.Parse<ColorFlagEnum>("+3"));
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("4"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("5"));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("6"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("7"));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>("+8"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>(" Blue ,UltraViolet,2 , Red"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>("15"));
        }

        [Test]
        public void FlagEnumsParse_ReturnsValidValue_WhenUsingValidStringWhileIgnoringCase()
        {
            Assert.AreEqual(ColorFlagEnum.Black, FlagEnums.Parse<ColorFlagEnum>("black", true));
            Assert.AreEqual(ColorFlagEnum.Black, FlagEnums.Parse<ColorFlagEnum>("0", true));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.Parse<ColorFlagEnum>("ReD", true));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.Parse<ColorFlagEnum>("1", true));
            Assert.AreEqual(ColorFlagEnum.Green, FlagEnums.Parse<ColorFlagEnum>("GrEeN", true));
            Assert.AreEqual(ColorFlagEnum.Green, FlagEnums.Parse<ColorFlagEnum>("2", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, FlagEnums.Parse<ColorFlagEnum>("3", true));
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("blue", true));
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("4", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("BLUE , red", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("5", true));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("BlUe, GReen", true));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("6", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("Red ,    BluE ,greEn", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("7", true));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>("UltRaVioLet", true));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>("8", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>(" GREEN ,blue,UltraViolet , 1", true));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>("15", true));
        }

        [Test]
        public void FlagEnumsParse_ReturnsValidValue_WhenUsingValidStringWithCustomDelimiter()
        {
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.Parse<ColorFlagEnum>("Red", "|"));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.Parse<ColorFlagEnum>("1", "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("Blue | Red", "|"));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("Blue | Green", "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("Blue | Green | Red", "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>(" Blue |UltraViolet|2 | Red", " | "));
        }

        [Test]
        public void FlagEnumsParse_ReturnsValidValue_WhenUsingValidStringWithCustomDelimiterWhileIgnoringCase()
        {
            Assert.AreEqual(ColorFlagEnum.Black, FlagEnums.Parse<ColorFlagEnum>("black", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Black, FlagEnums.Parse<ColorFlagEnum>("0", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.Parse<ColorFlagEnum>("ReD", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.Parse<ColorFlagEnum>("1", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Green, FlagEnums.Parse<ColorFlagEnum>("GrEeN", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Green, FlagEnums.Parse<ColorFlagEnum>("2", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, FlagEnums.Parse<ColorFlagEnum>("RED|green", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, FlagEnums.Parse<ColorFlagEnum>("3", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("blue", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("4", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("BLUE | red", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("5", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("BlUe| GReen", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("6", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("BluE |    greEn |Red", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("7", true, "|"));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>("UltRaVioLet", true, "|"));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>("8", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>(" GREEN |blue|UltraViolet | 1", true, "|"));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>("15", true, "|"));
        }

        [Test]
        public void FlagEnumsParse_ThrowsArgumentNullException_WhenUsingNullString()
        {
            TestHelper.ExpectException<ArgumentNullException>(() => FlagEnums.Parse<ColorFlagEnum>(null));
        }

        [Test]
        public void FlagEnumsParse_ThrowsArgumentException_WhenUsingWhiteSpaceString()
        {
            TestHelper.ExpectException<ArgumentException>(() => FlagEnums.Parse<ColorFlagEnum>(" "));
        }

        [Test]
        public void FlagEnumsParse_ThrowsArgumentException_WhenUsingUndefinedString()
        {
            TestHelper.ExpectException<ArgumentException>(() => FlagEnums.Parse<ColorFlagEnum>("Turquoise"));
        }

        [Test]
        public void FlagEnumsParse_ThrowsArgumentException_WhenUsingInvalidNumber()
        {
            TestHelper.ExpectException<ArgumentException>(() => FlagEnums.Parse<ColorFlagEnum>("16"));
        }

        [Test]
        public void FlagEnumsParse_ThrowsOverflowException_WhenUsingLargeNumber()
        {
            TestHelper.ExpectException<OverflowException>(() => FlagEnums.Parse<ColorFlagEnum>("128"));
        }

        [Test]
        public void FlagEnumsParse_UsesDefaultDelimiter_WhenUsingNullDelimiter()
        {
            Assert.AreEqual(FlagEnums.Parse<ColorFlagEnum>("Red, Green"), FlagEnums.Parse<ColorFlagEnum>("Red, Green", (string)null));
        }

        [Test]
        public void FlagEnumsParse_UsesDefaultDelimiter_WhenUsingEmptyStringDelimiter()
        {
            Assert.AreEqual(FlagEnums.Parse<ColorFlagEnum>("Red, Green"), FlagEnums.Parse<ColorFlagEnum>("Red, Green", string.Empty));
        }

        [Test]
        public void FlagEnumsParseOrDefault_ReturnsValidValue_WhenUsingValidName()
        {
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.ParseOrDefault("Blue", ColorFlagEnum.Red));
        }

        [Test]
        public void FlagEnumsParseOrDefault_ReturnsValidValue_WhenUsingValidNumber()
        {
            Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.ParseOrDefault("+4", ColorFlagEnum.Red));
        }

        [Test]
        public void FlagEnumsParseOrDefault_ReturnsDefaultValue_WhenUsingInvalidName()
        {
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.ParseOrDefault(null, ColorFlagEnum.Red));
        }

        [Test]
        public void FlagEnumsParseOrDefault_ReturnsDefaultValue_WhenUsingInvalidNumber()
        {
            Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.ParseOrDefault("16", ColorFlagEnum.Red));
        }

        [Test]
        public void FlagEnumsParseOrDefault_UsesDefaultDelimiter_WhenUsingNullDelimiter()
        {
            Assert.AreEqual(FlagEnums.ParseOrDefault("Red, Green", ColorFlagEnum.Green), FlagEnums.ParseOrDefault("Red, Green", null, ColorFlagEnum.Green));
        }

        [Test]
        public void FlagEnumsParseOrDefault_UsesDefaultDelimiter_WhenUsingEmptyStringDelimiter()
        {
            Assert.AreEqual(FlagEnums.ParseOrDefault("Red, Green", ColorFlagEnum.Green), FlagEnums.ParseOrDefault("Red, Green", string.Empty, ColorFlagEnum.Green));
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsValidValue_WhenUsingValidName()
        {
            ColorFlagEnum result;
            Assert.IsTrue(FlagEnums.TryParse("Black", out result));
            Assert.AreEqual(ColorFlagEnum.Black, result);
            Assert.IsTrue(FlagEnums.TryParse("Red", out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParse("Green", out result));
            Assert.AreEqual(ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParse("Red, Green", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParse("Blue", out result));
            Assert.AreEqual(ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("Blue , Red", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("Blue , Green", out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("Blue , Green , Red", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("UltraViolet", out result));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParse(" Blue ,UltraViolet,Green , Red", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsValidValue_WhenUsingValidNumber()
        {
            ColorFlagEnum result;
            Assert.IsTrue(FlagEnums.TryParse("0", out result));
            Assert.AreEqual(ColorFlagEnum.Black, result);
            Assert.IsTrue(FlagEnums.TryParse("1", out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParse("+2", out result));
            Assert.AreEqual(ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParse("3", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParse("4", out result));
            Assert.AreEqual(ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("+5", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("6", out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("7", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("8", out result));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParse(" Blue ,UltraViolet,+2 , Red", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParse("15", out result));
            Assert.AreEqual(ColorFlagEnum.All, result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsValidValue_WhenUsingValidStringWhileIgnoringCase()
        {
            ColorFlagEnum result;
            Assert.IsTrue(FlagEnums.TryParse("black", true, out result));
            Assert.AreEqual(ColorFlagEnum.Black, result);
            Assert.IsTrue(FlagEnums.TryParse("0", true, out result));
            Assert.AreEqual(ColorFlagEnum.Black, result);
            Assert.IsTrue(FlagEnums.TryParse("ReD", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParse("1", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParse("GrEeN", true, out result));
            Assert.AreEqual(ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParse("2", true, out result));
            Assert.AreEqual(ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParse("3", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParse("blue", true, out result));
            Assert.AreEqual(ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("4", true, out result));
            Assert.AreEqual(ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("BLUE , red", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("5", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("BlUe, GReen", true, out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("6", true, out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("Red ,    BluE ,greEn", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("7", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("UltRaVioLet", true, out result));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParse("8", true, out result));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParse(" GREEN ,blue,UltraViolet , 1", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParse("15", true, out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsValidValue_WhenUsingValidStringWithCustomDelimiter()
        {
            ColorFlagEnum result;
            Assert.IsTrue(FlagEnums.TryParse("Red", "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParse("1", "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParse("Blue | Red", "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("Blue | Green", "|", out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("Blue | Green | Red", "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse(" Blue |UltraViolet|2 | Red", " | ", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsValidValue_WhenUsingValidStringWithCustomDelimiterWhileIgnoringCase()
        {
            ColorFlagEnum result;
            Assert.IsTrue(FlagEnums.TryParse("black", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Black, result);
            Assert.IsTrue(FlagEnums.TryParse("0", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Black, result);
            Assert.IsTrue(FlagEnums.TryParse("ReD", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParse("1", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red, result);
            Assert.IsTrue(FlagEnums.TryParse("GrEeN", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParse("2", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParse("RED|green", true, "| ", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParse("3", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
            Assert.IsTrue(FlagEnums.TryParse("blue", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("4", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("BLUE | red", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("5", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("BlUe| GReen", true, " | ", out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("6", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("BluE |    greEn |Red", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("7", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
            Assert.IsTrue(FlagEnums.TryParse("UltRaVioLet", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParse("8", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParse(" GREEN |blue|UltraViolet | 1", true, " |", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
            Assert.IsTrue(FlagEnums.TryParse("15", true, "|", out result));
            Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsFalse_WhenUsingNullString()
        {
            ColorFlagEnum result;
            Assert.IsFalse(FlagEnums.TryParse(null, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsFalse_WhenUsingWhiteSpaceString()
        {
            ColorFlagEnum result;
            Assert.IsFalse(FlagEnums.TryParse(" ", out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsFalse_WhenUsingUndefinedString()
        {
            ColorFlagEnum result;
            Assert.IsFalse(FlagEnums.TryParse("Turquoise", out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsFalse_WhenUsingInvalidNumber()
        {
            ColorFlagEnum result;
            Assert.IsFalse(FlagEnums.TryParse("16", out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
        }

        [Test]
        public void FlagEnumsTryParse_ReturnsFalse_WhenUsingLargeNumber()
        {
            ColorFlagEnum result;
            Assert.IsFalse(FlagEnums.TryParse("128", out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
        }

        [Test]
        public void FlagEnumsTryParse_UsesDefaultDelimiter_WhenUsingNullDelimiter()
        {
            ColorFlagEnum result0;
            ColorFlagEnum result1;
            Assert.AreEqual(FlagEnums.TryParse("Red, Green", out result0), FlagEnums.TryParse("Red, Green", null, out result1));
            Assert.AreEqual(result0, result1);
        }

        [Test]
        public void FlagEnumsTryParse_UsesDefaultDelimiter_WhenUsingEmptyStringDelimiter()
        {
            ColorFlagEnum result0;
            ColorFlagEnum result1;
            Assert.AreEqual(FlagEnums.TryParse("Red, Green", out result0), FlagEnums.TryParse("Red, Green", string.Empty, out result1));
            Assert.AreEqual(result0, result1);
        }
        #endregion
    }
}
