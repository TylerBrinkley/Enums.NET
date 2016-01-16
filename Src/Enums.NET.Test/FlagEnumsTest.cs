// Enums.NET
// Copyright 2015 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//	  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnumsNET.Test
{
	[TestClass]
	public class FlagEnumsTest
	{
		#region "Properties"
		[TestMethod]
		public void IsFlagEnum_Test()
		{
			Assert.IsTrue(FlagEnums.IsFlagEnum<ColorFlagEnum>());
			Assert.IsFalse(FlagEnums.IsFlagEnum<NonContiguousEnum>());
		}

		[TestMethod]
		public void GetAllFlags_Test()
		{
			Assert.AreEqual(ColorFlagEnum.All, FlagEnums.GetAllFlags<ColorFlagEnum>());
		}
		#endregion

		#region Extension Methods
		[TestMethod]
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

		[TestMethod]
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

		[TestMethod]
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

		[TestMethod]
		public void FormatAsFlags_ReturnsNull_WhenUsingInvalidValue()
		{
			Assert.AreEqual(null, ((ColorFlagEnum)16).FormatAsFlags());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FormatAsFlags_ThrowsArgumentNullException_WhenUsingValidValueWithNullDelimiter()
		{
			ColorFlagEnum.Red.FormatAsFlags((string)null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void FormatAsFlags_ThrowsArgumentException_WhenUsingValidValueWithEmptyDelimiter()
		{
			ColorFlagEnum.Red.FormatAsFlags(string.Empty);
		}

		[TestMethod]
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

		[TestMethod]
		public void GetFlags_ReturnsNull_WhenUsingInvalidValue()
		{
			Assert.AreEqual(null, ((ColorFlagEnum)16).GetFlags());
		}

		[TestMethod]
		public void HasAnyFlags()
		{
			Assert.IsFalse(ColorFlagEnum.Black.HasAnyFlags());
			Assert.IsTrue(ColorFlagEnum.Blue.HasAnyFlags());
		}

		[TestMethod]
		public void HasAnyFlags_ThrowsArgumentException_WhenUsingInvalidValue()
		{
			TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).HasAnyFlags());
		}

		[TestMethod]
		public void HasAnyFlags1()
		{
			Assert.IsFalse(ColorFlagEnum.Black.HasAnyFlags(ColorFlagEnum.Blue));
			Assert.IsTrue(ColorFlagEnum.Blue.HasAnyFlags(ColorFlagEnum.Blue));
			Assert.IsTrue((ColorFlagEnum.Green | ColorFlagEnum.Blue).HasAnyFlags(ColorFlagEnum.Blue));
			Assert.IsFalse((ColorFlagEnum.Green | ColorFlagEnum.Blue).HasAnyFlags(ColorFlagEnum.Red));
			Assert.IsTrue(ColorFlagEnum.Red.HasAnyFlags(ColorFlagEnum.Red | ColorFlagEnum.Green));
			Assert.IsFalse(ColorFlagEnum.Blue.HasAnyFlags(ColorFlagEnum.Red | ColorFlagEnum.Green));
		}

		[TestMethod]
		public void HasAnyFlags1_ThrowsArgumentException_WhenUsingInvalidValue()
		{
			TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).HasAnyFlags(ColorFlagEnum.Red));
		}

		[TestMethod]
		public void HasAnyFlags1_ThrowsArgumentException_WhenUsingInvalidFlagMask()
		{
			TestHelper.ExpectException<ArgumentException>(() => ColorFlagEnum.Red.HasAnyFlags((ColorFlagEnum)16));
		}

		[TestMethod]
		public void HasAllFlags()
		{
			Assert.IsTrue(ColorFlagEnum.All.HasAllFlags());
			Assert.IsFalse(ColorFlagEnum.Blue.HasAllFlags());
		}

		[TestMethod]
		public void HasAllFlags_ThrowsArgumentException_WhenUsingInvalidValue()
		{
			TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).HasAllFlags());
		}

		[TestMethod]
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

		[TestMethod]
		public void HasAllFlags1_ThrowsArgumentException_WhenUsingInvalidValue()
		{
			TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).HasAllFlags(ColorFlagEnum.Red));
		}

		[TestMethod]
		public void HasAllFlags1_ThrowsArgumentException_WhenUsingInvalidFlagMask()
		{
			TestHelper.ExpectException<ArgumentException>(() => ColorFlagEnum.Red.HasAllFlags((ColorFlagEnum)16));
		}

		[TestMethod]
		public void InvertFlags()
		{
			Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.UltraViolet, (ColorFlagEnum.Red | ColorFlagEnum.Blue).InvertFlags());
		}

		[TestMethod]
		public void InvertFlags_ThrowsArgumentException_WhenUsingInvalidValue()
		{
			TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).InvertFlags());
		}

		[TestMethod]
		public void InvertFlags1()
		{
			Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).InvertFlags(ColorFlagEnum.Green));
			Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).InvertFlags(ColorFlagEnum.Red | ColorFlagEnum.UltraViolet));
		}

		[TestMethod]
		public void InvertFlags1_ThrowsArgumentException_WhenUsingInvalidValue()
		{
			TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).InvertFlags(ColorFlagEnum.Blue));
		}

		[TestMethod]
		public void InvertFlags1_ThrowsArgumentException_WhenUsingInvalidFlagMask()
		{
			TestHelper.ExpectException<ArgumentException>(() => ColorFlagEnum.Blue.InvertFlags((ColorFlagEnum)16));
		}

		[TestMethod]
		public void CommonFlags()
		{
			Assert.AreEqual(ColorFlagEnum.Red & ColorFlagEnum.Green, ColorFlagEnum.Red.CommonFlags(ColorFlagEnum.Green));
			Assert.AreEqual(ColorFlagEnum.Red & ColorFlagEnum.Green, ColorFlagEnum.Green.CommonFlags(ColorFlagEnum.Red));
			Assert.AreEqual(ColorFlagEnum.Blue, ColorFlagEnum.Blue.CommonFlags(ColorFlagEnum.Blue));
		}

		[TestMethod]
		public void CommonFlags_ThrowsArgumentException_WhenUsingInvalidValue()
		{
			TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).CommonFlags(ColorFlagEnum.Red));
		}

		[TestMethod]
		public void CommonFlags_ThrowsArgumentException_WhenUsingInvalidFlagMask()
		{
			TestHelper.ExpectException<ArgumentException>(() => ColorFlagEnum.Red.CommonFlags((ColorFlagEnum)16));
		}

		[TestMethod]
		public void SetFlags()
		{
			Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, ColorFlagEnum.Red.SetFlags(ColorFlagEnum.Green));
			Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green, ColorFlagEnum.Green.SetFlags(ColorFlagEnum.Red));
			Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, ColorFlagEnum.Green.SetFlags(ColorFlagEnum.Red | ColorFlagEnum.Blue));
			Assert.AreEqual(ColorFlagEnum.Blue, ColorFlagEnum.Blue.SetFlags(ColorFlagEnum.Blue));
		}

		[TestMethod]
		public void SetFlags_ThrowsArgumentException_WhenUsingInvalidValue()
		{
			TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).SetFlags(ColorFlagEnum.Red));
		}

		[TestMethod]
		public void SetFlags_ThrowsArgumentException_WhenUsingInvalidFlagMask()
		{
			TestHelper.ExpectException<ArgumentException>(() => ColorFlagEnum.Red.SetFlags((ColorFlagEnum)16));
		}

		[TestMethod]
		public void ClearFlags()
		{
			Assert.AreEqual(ColorFlagEnum.Red, (ColorFlagEnum.Red | ColorFlagEnum.Green).ClearFlags(ColorFlagEnum.Green));
			Assert.AreEqual(ColorFlagEnum.Green, ColorFlagEnum.Green.ClearFlags(ColorFlagEnum.Red));
			Assert.AreEqual(ColorFlagEnum.Green, (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).ClearFlags(ColorFlagEnum.Red | ColorFlagEnum.Blue));
			Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).ClearFlags(ColorFlagEnum.Red | ColorFlagEnum.UltraViolet));
			Assert.AreEqual(ColorFlagEnum.Black, ColorFlagEnum.Blue.ClearFlags(ColorFlagEnum.Blue));
		}

		[TestMethod]
		public void ClearFlags_ThrowsArgumentException_WhenUsingInvalidValue()
		{
			TestHelper.ExpectException<ArgumentException>(() => ((ColorFlagEnum)16).ClearFlags(ColorFlagEnum.Red));
		}

		[TestMethod]
		public void ClearFlags_ThrowsArgumentException_WhenUsingInvalidFlagMask()
		{
			TestHelper.ExpectException<ArgumentException>(() => ColorFlagEnum.Red.ClearFlags((ColorFlagEnum)16));
		}
		#endregion

		#region Parsing
		[TestMethod]
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

		[TestMethod]
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

		[TestMethod]
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

		[TestMethod]
		public void FlagEnumsParse_ReturnsValidValue_WhenUsingValidStringWithCustomDelimiter()
		{
			Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.Parse<ColorFlagEnum>("Red", "|"));
			Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.Parse<ColorFlagEnum>("1", "|"));
			Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("Blue | Red", "|"));
			Assert.AreEqual(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("Blue | Green", "|"));
			Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.Parse<ColorFlagEnum>("Blue | Green | Red", "|"));
			Assert.AreEqual(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.Parse<ColorFlagEnum>(" Blue |UltraViolet|2 | Red", " | "));
		}

		[TestMethod]
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

		[TestMethod]
		public void FlagEnumsParse_ThrowsArgumentNullException_WhenUsingNullString()
		{
			TestHelper.ExpectException<ArgumentNullException>(() => FlagEnums.Parse<ColorFlagEnum>(null));
		}

		[TestMethod]
		public void FlagEnumsParse_ThrowsArgumentException_WhenUsingWhiteSpaceString()
		{
			TestHelper.ExpectException<ArgumentException>(() => FlagEnums.Parse<ColorFlagEnum>(" "));
		}

		[TestMethod]
		public void FlagEnumsParse_ThrowsArgumentException_WhenUsingUndefinedString()
		{
			TestHelper.ExpectException<ArgumentException>(() => FlagEnums.Parse<ColorFlagEnum>("Turquoise"));
		}

		[TestMethod]
		public void FlagEnumsParse_ThrowsArgumentException_WhenUsingInvalidNumber()
		{
			TestHelper.ExpectException<ArgumentException>(() => FlagEnums.Parse<ColorFlagEnum>("16"));
		}

		[TestMethod]
		public void FlagEnumsParse_ThrowsOverflowException_WhenUsingLargeNumber()
		{
			TestHelper.ExpectException<OverflowException>(() => FlagEnums.Parse<ColorFlagEnum>("128"));
		}

		[TestMethod]
		public void FlagEnumsParse_ThrowsArgumentNullException_WhenUsingNullDelimiter()
		{
			TestHelper.ExpectException<ArgumentNullException>(() => FlagEnums.Parse<ColorFlagEnum>("Red", (string)null));
		}

		[TestMethod]
		public void FlagEnumsParse_ThrowsArgumentException_WhenUsingEmptyStringDelimiter()
		{
			TestHelper.ExpectException<ArgumentException>(() => FlagEnums.Parse<ColorFlagEnum>("Red", string.Empty));
		}

		[TestMethod]
		public void FlagEnumsParseOrDefault_ReturnsValidValue_WhenUsingValidName()
		{
			Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.ParseOrDefault("Blue", ColorFlagEnum.Red));
		}

		[TestMethod]
		public void FlagEnumsParseOrDefault_ReturnsValidValue_WhenUsingValidNumber()
		{
			Assert.AreEqual(ColorFlagEnum.Blue, FlagEnums.ParseOrDefault("+4", ColorFlagEnum.Red));
		}

		[TestMethod]
		public void FlagEnumsParseOrDefault_ReturnsDefaultValue_WhenUsingInvalidName()
		{
			Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.ParseOrDefault(null, ColorFlagEnum.Red));
		}

		[TestMethod]
		public void FlagEnumsParseOrDefault_ReturnsDefaultValue_WhenUsingInvalidNumber()
		{
			Assert.AreEqual(ColorFlagEnum.Red, FlagEnums.ParseOrDefault("16", ColorFlagEnum.Red));
		}

		[TestMethod]
		public void FlagEnumsParseOrDefault_ThrowsArgumentNullException_WhenUsingNullDelimiter()
		{
			TestHelper.ExpectException<ArgumentNullException>(() => FlagEnums.ParseOrDefault("Red", null, ColorFlagEnum.Green));
		}

		[TestMethod]
		public void FlagEnumsParseOrDefault_ThrowsArgumentException_WhenUsingEmptyStringDelimiter()
		{
			TestHelper.ExpectException<ArgumentException>(() => FlagEnums.ParseOrDefault("Red", string.Empty, ColorFlagEnum.Green));
		}

		[TestMethod]
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

		[TestMethod]
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

		[TestMethod]
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

		[TestMethod]
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

		[TestMethod]
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

		[TestMethod]
		public void FlagEnumsTryParse_ReturnsFalse_WhenUsingNullString()
		{
			ColorFlagEnum result;
			Assert.IsFalse(FlagEnums.TryParse(null, out result));
			Assert.AreEqual(default(ColorFlagEnum), result);
		}

		[TestMethod]
		public void FlagEnumsTryParse_ReturnsFalse_WhenUsingWhiteSpaceString()
		{
			ColorFlagEnum result;
			Assert.IsFalse(FlagEnums.TryParse(" ", out result));
			Assert.AreEqual(default(ColorFlagEnum), result);
		}

		[TestMethod]
		public void FlagEnumsTryParse_ReturnsFalse_WhenUsingUndefinedString()
		{
			ColorFlagEnum result;
			Assert.IsFalse(FlagEnums.TryParse("Turquoise", out result));
			Assert.AreEqual(default(ColorFlagEnum), result);
		}

		[TestMethod]
		public void FlagEnumsTryParse_ReturnsFalse_WhenUsingInvalidNumber()
		{
			ColorFlagEnum result;
			Assert.IsFalse(FlagEnums.TryParse("16", out result));
			Assert.AreEqual(default(ColorFlagEnum), result);
		}

		[TestMethod]
		public void FlagEnumsTryParse_ReturnsFalse_WhenUsingLargeNumber()
		{
			ColorFlagEnum result;
			Assert.IsFalse(FlagEnums.TryParse("128", out result));
			Assert.AreEqual(default(ColorFlagEnum), result);
		}

		[TestMethod]
		public void FlagEnumsTryParse_ThrowsArgumentNullException_WhenUsingNullDelimiter()
		{
			ColorFlagEnum result;
			TestHelper.ExpectException<ArgumentNullException>(() => FlagEnums.TryParse("Red", null, out result));
		}

		[TestMethod]
		public void FlagEnumsTryParse_ThrowsArgumentException_WhenUsingEmptyStringDelimiter()
		{
			ColorFlagEnum result;
			TestHelper.ExpectException<ArgumentException>(() => FlagEnums.TryParse("Red", string.Empty, out result));
		}
		#endregion
	}
}
