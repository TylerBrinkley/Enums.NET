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
using Xunit;

namespace EnumsNET.Tests;

public class FlagEnumsTest
{
    #region "Properties"
    [Fact]
    public void IsFlagEnum_Test()
    {
        Assert.True(FlagEnums.IsFlagEnum<ColorFlagEnum>());
        Assert.False(FlagEnums.IsFlagEnum<NonContiguousEnum>());
    }

    [Fact]
    public void GetAllFlags_Test()
    {
        Assert.Equal(ColorFlagEnum.All, FlagEnums.GetAllFlags<ColorFlagEnum>());
    }
    #endregion

    #region Main Methods
    [Fact]
    public void IsValidFlagCombination_Test()
    {
        for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
        {
            if (i is >= 0 and <= 15)
            {
                Assert.True(FlagEnums.IsValidFlagCombination((ColorFlagEnum)i));
            }
            else
            {
                Assert.False(FlagEnums.IsValidFlagCombination((ColorFlagEnum)i));
            }
        }
    }

    [Fact]
    public void FormatFlags_ReturnsValidString_WhenUsingValidValue()
    {
        AssertTryFormatFlags(ColorFlagEnum.Black, "Black");
        AssertTryFormatFlags(ColorFlagEnum.Red, "Red");
        AssertTryFormatFlags(ColorFlagEnum.Green, "Green");
        AssertTryFormatFlags(ColorFlagEnum.Blue, "Blue");
        AssertTryFormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Green, "Red, Green");
        AssertTryFormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Blue, "Red, Blue");
        AssertTryFormatFlags(ColorFlagEnum.Green | ColorFlagEnum.Blue, "Green, Blue");
        AssertTryFormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, "Red, Green, Blue");
        AssertTryFormatFlags(ColorFlagEnum.UltraViolet, "UltraViolet");
        AssertTryFormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, "All");

        static void AssertTryFormatFlags(ColorFlagEnum value, string expected)
        {
            Assert.Equal(expected, FlagEnums.FormatFlags(value));
#if SPAN
            var dest = new char[expected.Length];
            Assert.True(FlagEnums.TryFormatFlags(value, dest, out var charsWritten));
            Assert.Equal(expected.Length, charsWritten);
            Assert.Equal(expected, new string(dest));

            dest = new char[expected.Length - 1];
            Assert.False(FlagEnums.TryFormatFlags(value, dest, out charsWritten));
            Assert.Equal(0, charsWritten);
            Assert.Equal(new char[dest.Length], dest);
#endif
        }
    }

    [Fact]
    public void FormatFlags_ReturnsValidString_WhenUsingValidValueWithCustomDelimiter()
    {
        AssertTryFormatFlags(ColorFlagEnum.Black, " | ", "Black");
        AssertTryFormatFlags(ColorFlagEnum.Red, " | ", "Red");
        AssertTryFormatFlags(ColorFlagEnum.Green, " | ", "Green");
        AssertTryFormatFlags(ColorFlagEnum.Blue, " | ", "Blue");
        AssertTryFormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Green, " | ", "Red | Green");
        AssertTryFormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Blue, " | ", "Red | Blue");
        AssertTryFormatFlags(ColorFlagEnum.Green | ColorFlagEnum.Blue, " | ", "Green | Blue");
        AssertTryFormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, " | ", "Red | Green | Blue");
        AssertTryFormatFlags(ColorFlagEnum.UltraViolet, " | ", "UltraViolet");
        AssertTryFormatFlags(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, " | ", "All");

        static void AssertTryFormatFlags(ColorFlagEnum value, string delimiter, string expected)
        {
            Assert.Equal(expected, FlagEnums.FormatFlags(value, delimiter));
#if SPAN
            var dest = new char[expected.Length];
            Assert.True(FlagEnums.TryFormatFlags(value, dest, out var charsWritten, delimiter));
            Assert.Equal(expected.Length, charsWritten);
            Assert.Equal(expected, new string(dest));

            dest = new char[expected.Length - 1];
            Assert.False(FlagEnums.TryFormatFlags(value, dest, out charsWritten, delimiter));
            Assert.Equal(0, charsWritten);
            Assert.Equal(new char[dest.Length], dest);
#endif
        }
    }

    [Fact]
    public void FormatFlags_ReturnsValidString_WhenUsingInvalidValue()
    {
        AssertTryFormatFlags((ColorFlagEnum)16, "16");

        static void AssertTryFormatFlags(ColorFlagEnum value, string expected)
        {
            Assert.Equal(expected, FlagEnums.FormatFlags(value));
#if SPAN
            var dest = new char[expected.Length];
            Assert.True(FlagEnums.TryFormatFlags(value, dest, out var charsWritten));
            Assert.Equal(expected.Length, charsWritten);
            Assert.Equal(expected, new string(dest));

            dest = new char[expected.Length - 1];
            Assert.False(FlagEnums.TryFormatFlags(value, dest, out charsWritten));
            Assert.Equal(0, charsWritten);
            Assert.Equal(new char[dest.Length], dest);
#endif
        }
    }

    [Fact]
    public void FormatFlags_UsesDefaultDelimiter_WhenUsingValidValueWithNullDelimiter()
    {
        var value = ColorFlagEnum.Red | ColorFlagEnum.Green;
        AssertTryFormatFlags(value, FlagEnums.FormatFlags(value));

        static void AssertTryFormatFlags(ColorFlagEnum value, string expected)
        {
            Assert.Equal(expected, FlagEnums.FormatFlags(value, null));
#if SPAN
            var dest = new char[expected.Length];
            Assert.True(FlagEnums.TryFormatFlags(value, dest, out var charsWritten, (string)null));
            Assert.Equal(expected.Length, charsWritten);
            Assert.Equal(expected, new string(dest));

            dest = new char[expected.Length - 1];
            Assert.False(FlagEnums.TryFormatFlags(value, dest, out charsWritten, (string)null));
            Assert.Equal(0, charsWritten);
            Assert.Equal(new char[dest.Length], dest);
#endif
        }
    }

    [Fact]
    public void FormatFlags_UsesDefaultDelimiter_WhenUsingValidValueWithEmptyDelimiter()
    {
        var value = ColorFlagEnum.Red | ColorFlagEnum.Green;
        AssertTryFormatFlags(value, FlagEnums.FormatFlags(value));

        static void AssertTryFormatFlags(ColorFlagEnum value, string expected)
        {
            Assert.Equal(expected, FlagEnums.FormatFlags(value, string.Empty));
#if SPAN
            var dest = new char[expected.Length];
            Assert.True(FlagEnums.TryFormatFlags(value, dest, out var charsWritten, string.Empty));
            Assert.Equal(expected.Length, charsWritten);
            Assert.Equal(expected, new string(dest));

            dest = new char[expected.Length - 1];
            Assert.False(FlagEnums.TryFormatFlags(value, dest, out charsWritten, string.Empty));
            Assert.Equal(0, charsWritten);
            Assert.Equal(new char[dest.Length], dest);
#endif
        }
    }

    [Fact]
    public void GetFlags_ReturnsValidArray_WhenUsingValidValue()
    {
        Assert.Equal([], ColorFlagEnum.Black.GetFlags());
        Assert.Equal([ColorFlagEnum.Red], ColorFlagEnum.Red.GetFlags());
        Assert.Equal([ColorFlagEnum.Green], ColorFlagEnum.Green.GetFlags());
        Assert.Equal([ColorFlagEnum.Blue], ColorFlagEnum.Blue.GetFlags());
        Assert.Equal([ColorFlagEnum.Red, ColorFlagEnum.Green], (ColorFlagEnum.Red | ColorFlagEnum.Green).GetFlags());
        Assert.Equal([ColorFlagEnum.Red, ColorFlagEnum.Blue], (ColorFlagEnum.Red | ColorFlagEnum.Blue).GetFlags());
        Assert.Equal([ColorFlagEnum.Green, ColorFlagEnum.Blue], (ColorFlagEnum.Green | ColorFlagEnum.Blue).GetFlags());
        Assert.Equal([ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue], (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).GetFlags());
        Assert.Equal([ColorFlagEnum.UltraViolet], ColorFlagEnum.UltraViolet.GetFlags());
        Assert.Equal([ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet], (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet).GetFlags());
    }

    [Fact]
    public void GetFlags_ReturnsValidArray_WhenUsingInvalidValue()
    {
        Assert.Equal([], ((ColorFlagEnum)16).GetFlags());
        Assert.Equal([ColorFlagEnum.Blue], (ColorFlagEnum.Blue | (ColorFlagEnum)16).GetFlags());
    }

    [Fact]
    public void GetFlagCount()
    {
        Assert.Equal(4, FlagEnums.GetFlagCount<ColorFlagEnum>());
        Assert.Equal(6, FlagEnums.GetFlagCount<UInt64FlagEnum>());

        Assert.Equal(0, ColorFlagEnum.Black.GetFlagCount());
        Assert.Equal(2, (ColorFlagEnum.Red | ColorFlagEnum.Blue).GetFlagCount());
        Assert.Equal(3, (UInt64FlagEnum.Hops | UInt64FlagEnum.Runs | UInt64FlagEnum.Walks).GetFlagCount());
        Assert.Equal(0, ((UInt64FlagEnum)8).GetFlagCount());

        Assert.Equal(1, ColorFlagEnum.All.GetFlagCount(ColorFlagEnum.Blue));
        Assert.Equal(2, ColorFlagEnum.All.GetFlagCount(ColorFlagEnum.Blue | ColorFlagEnum.Red));
        Assert.Equal(0, ColorFlagEnum.Green.GetFlagCount(ColorFlagEnum.Blue | ColorFlagEnum.Red));
    }

    [Fact]
    public void HasAnyFlags()
    {
        Assert.False(ColorFlagEnum.Black.HasAnyFlags());
        Assert.True(ColorFlagEnum.Blue.HasAnyFlags());
    }

    [Fact]
    public void HasAnyFlags_ReturnsExpected_WhenUsingInvalidValue()
    {
        Assert.True(((ColorFlagEnum)16).HasAnyFlags());
    }

    [Fact]
    public void HasAnyFlags1()
    {
        Assert.False(ColorFlagEnum.Black.HasAnyFlags(ColorFlagEnum.Blue));
        Assert.True(ColorFlagEnum.Blue.HasAnyFlags(ColorFlagEnum.Blue));
        Assert.True((ColorFlagEnum.Green | ColorFlagEnum.Blue).HasAnyFlags(ColorFlagEnum.Blue));
        Assert.False((ColorFlagEnum.Green | ColorFlagEnum.Blue).HasAnyFlags(ColorFlagEnum.Red));
        Assert.True(ColorFlagEnum.Red.HasAnyFlags(ColorFlagEnum.Red | ColorFlagEnum.Green));
        Assert.False(ColorFlagEnum.Blue.HasAnyFlags(ColorFlagEnum.Red | ColorFlagEnum.Green));
    }

    [Fact]
    public void HasAnyFlags1_ReturnsExpected_WhenUsingInvalidValue()
    {
        Assert.False(((ColorFlagEnum)16).HasAnyFlags(ColorFlagEnum.Red));
        Assert.True((((ColorFlagEnum)16) | ColorFlagEnum.Red).HasAnyFlags(ColorFlagEnum.Red));
    }

    [Fact]
    public void HasAnyFlags1_ReturnsExpected_WhenUsingInvalidOtherFlags()
    {
        Assert.False(ColorFlagEnum.Red.HasAnyFlags((ColorFlagEnum)16));
        Assert.True(ColorFlagEnum.Red.HasAnyFlags(((ColorFlagEnum)16) | ColorFlagEnum.Red));
    }

    [Fact]
    public void HasAllFlags()
    {
        Assert.True(ColorFlagEnum.All.HasAllFlags());
        Assert.False(ColorFlagEnum.Blue.HasAllFlags());
    }

    [Fact]
    public void HasAllFlags_ReturnsExpected_WhenUsingInvalidValue()
    {
        Assert.False(((ColorFlagEnum)16).HasAllFlags());
        Assert.True((ColorFlagEnum.All | ((ColorFlagEnum)16)).HasAllFlags());
    }

    [Fact]
    public void HasAllFlags1()
    {
        Assert.False(ColorFlagEnum.Black.HasAllFlags(ColorFlagEnum.Blue));
        Assert.True(ColorFlagEnum.Blue.HasAllFlags(ColorFlagEnum.Blue));
        Assert.True((ColorFlagEnum.Green | ColorFlagEnum.Blue).HasAllFlags(ColorFlagEnum.Blue));
        Assert.False((ColorFlagEnum.Green | ColorFlagEnum.Blue).HasAllFlags(ColorFlagEnum.Red));
        Assert.True((ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).HasAllFlags(ColorFlagEnum.Green | ColorFlagEnum.Blue));
        Assert.False(ColorFlagEnum.Red.HasAllFlags(ColorFlagEnum.Red | ColorFlagEnum.Green));
        Assert.False(ColorFlagEnum.Blue.HasAllFlags(ColorFlagEnum.Red | ColorFlagEnum.Green));
    }

    [Fact]
    public void HasAllFlags1_ReturnsExpected_WhenUsingInvalidValue()
    {
        Assert.False(((ColorFlagEnum)16).HasAllFlags(ColorFlagEnum.Red));
        Assert.True((((ColorFlagEnum)16) | ColorFlagEnum.Red).HasAllFlags(ColorFlagEnum.Red));
    }

    [Fact]
    public void HasAllFlags1_ReturnsExpected_WhenUsingInvalidOtherFlags()
    {
        Assert.False(ColorFlagEnum.Red.HasAllFlags((ColorFlagEnum)16));
        Assert.True((ColorFlagEnum.Red | (ColorFlagEnum)16).HasAllFlags((ColorFlagEnum)16));
    }

    [Fact]
    public void ToggleFlags()
    {
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.UltraViolet, FlagEnums.ToggleFlags(ColorFlagEnum.Red | ColorFlagEnum.Blue));
    }

    [Fact]
    public void ToggleFlags_ReturnsExpected_WhenUsingInvalidValue()
    {
        Assert.Equal(ColorFlagEnum.All | ((ColorFlagEnum)16), FlagEnums.ToggleFlags((ColorFlagEnum)16));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue | ((ColorFlagEnum)16), FlagEnums.ToggleFlags(((ColorFlagEnum)16) | ColorFlagEnum.Red | ColorFlagEnum.UltraViolet));
    }

    [Fact]
    public void ToggleFlags1()
    {
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ToggleFlags(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, ColorFlagEnum.Green));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ToggleFlags(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, ColorFlagEnum.Red | ColorFlagEnum.UltraViolet));
    }

    [Fact]
    public void ToggleFlags1_ReturnsExpected_WhenUsingInvalidValue()
    {
        Assert.Equal(((ColorFlagEnum)16) | ColorFlagEnum.Blue, FlagEnums.ToggleFlags((ColorFlagEnum)16, ColorFlagEnum.Blue));
    }

    [Fact]
    public void ToggleFlags1_ReturnsExpected_WhenUsingInvalidOtherFlags()
    {
        Assert.Equal(((ColorFlagEnum)16) | ColorFlagEnum.Blue, FlagEnums.ToggleFlags(ColorFlagEnum.Blue, (ColorFlagEnum)16));
    }

    [Fact]
    public void CommonFlags()
    {
        Assert.Equal(ColorFlagEnum.Red & ColorFlagEnum.Green, ColorFlagEnum.Red.CommonFlags(ColorFlagEnum.Green));
        Assert.Equal(ColorFlagEnum.Red & ColorFlagEnum.Green, ColorFlagEnum.Green.CommonFlags(ColorFlagEnum.Red));
        Assert.Equal(ColorFlagEnum.Blue, ColorFlagEnum.Blue.CommonFlags(ColorFlagEnum.Blue));
    }

    [Fact]
    public void CommonFlags_ReturnsExpected_WhenUsingInvalidValue()
    {
        Assert.Equal(ColorFlagEnum.Black, ((ColorFlagEnum)16).CommonFlags(ColorFlagEnum.Red));
        Assert.Equal(ColorFlagEnum.Red, (((ColorFlagEnum)16) | ColorFlagEnum.Red).CommonFlags(ColorFlagEnum.Red));
    }

    [Fact]
    public void CommonFlags_ReturnsExpected_WhenUsingInvalidOtherFlags()
    {
        Assert.Equal(ColorFlagEnum.Black, ColorFlagEnum.Red.CommonFlags((ColorFlagEnum)16));
        Assert.Equal(ColorFlagEnum.Red, ColorFlagEnum.Red.CommonFlags(((ColorFlagEnum)16) | ColorFlagEnum.Red));
    }

    [Fact]
    public void CombineFlags()
    {
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green, ColorFlagEnum.Red.CombineFlags(ColorFlagEnum.Green));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green, ColorFlagEnum.Green.CombineFlags(ColorFlagEnum.Red));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, ColorFlagEnum.Green.CombineFlags(ColorFlagEnum.Red | ColorFlagEnum.Blue));
        Assert.Equal(ColorFlagEnum.Blue, ColorFlagEnum.Blue.CombineFlags(ColorFlagEnum.Blue));
    }

    [Fact]
    public void CombineFlags_ReturnsExpected_WhenUsingInvalidValue()
    {
        Assert.Equal(((ColorFlagEnum)16) | ColorFlagEnum.Red, ((ColorFlagEnum)16).CombineFlags(ColorFlagEnum.Red));
    }

    [Fact]
    public void CombineFlags_ReturnsExpected_WhenUsingInvalidOtherFlags()
    {
        Assert.Equal(((ColorFlagEnum)16) | ColorFlagEnum.Red, ColorFlagEnum.Red.CombineFlags((ColorFlagEnum)16));
    }

    [Fact]
    public void RemoveFlags()
    {
        Assert.Equal(ColorFlagEnum.Red, (ColorFlagEnum.Red | ColorFlagEnum.Green).RemoveFlags(ColorFlagEnum.Green));
        Assert.Equal(ColorFlagEnum.Green, ColorFlagEnum.Green.RemoveFlags(ColorFlagEnum.Red));
        Assert.Equal(ColorFlagEnum.Green, (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).RemoveFlags(ColorFlagEnum.Red | ColorFlagEnum.Blue));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, (ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue).RemoveFlags(ColorFlagEnum.Red | ColorFlagEnum.UltraViolet));
        Assert.Equal(ColorFlagEnum.Black, ColorFlagEnum.Blue.RemoveFlags(ColorFlagEnum.Blue));
    }

    [Fact]
    public void RemoveFlags_ThrowsArgumentException_WhenUsingInvalidValue()
    {
        Assert.Equal((ColorFlagEnum)16, ((ColorFlagEnum)16).RemoveFlags(ColorFlagEnum.Red));
        Assert.Equal((ColorFlagEnum)16, (((ColorFlagEnum)16) | ColorFlagEnum.Red).RemoveFlags(ColorFlagEnum.Red));
    }

    [Fact]
    public void RemoveFlags_ThrowsArgumentException_WhenUsingInvalidOtherFlags()
    {
        Assert.Equal(ColorFlagEnum.Red, ColorFlagEnum.Red.RemoveFlags((ColorFlagEnum)16));
        Assert.Equal(ColorFlagEnum.Black, ColorFlagEnum.Red.RemoveFlags(((ColorFlagEnum)16) | ColorFlagEnum.Red));
    }
    #endregion

    #region Parsing
    [Fact]
    public void ParseFlags_ReturnsValidValue_WhenUsingValidName()
    {
        Assert.Equal(ColorFlagEnum.Black, FlagEnums.ParseFlags<ColorFlagEnum>("Black"));
        Assert.Equal(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("Red"));
        Assert.Equal(ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("Green"));
        Assert.Equal(ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue , Red"));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue , Green"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue , Green , Red"));
        Assert.Equal(ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("UltraViolet"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>(" Blue ,UltraViolet,Green , Red"));
    }

    [Fact]
    public void ParseFlags_ReturnsValidValue_WhenUsingValidNumber()
    {
        Assert.Equal(ColorFlagEnum.Black, FlagEnums.ParseFlags<ColorFlagEnum>("0"));
        Assert.Equal(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("1"));
        Assert.Equal(ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("2"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("+3"));
        Assert.Equal(ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("4"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("5"));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("6"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("7"));
        Assert.Equal(ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("+8"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>(" Blue ,UltraViolet,2 , Red"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("15"));
    }

    [Fact]
    public void ParseFlags_ReturnsValidValue_WhenUsingValidStringWhileIgnoringCase()
    {
        Assert.Equal(ColorFlagEnum.Black, FlagEnums.ParseFlags<ColorFlagEnum>("black", true));
        Assert.Equal(ColorFlagEnum.Black, FlagEnums.ParseFlags<ColorFlagEnum>("0", true));
        Assert.Equal(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("ReD", true));
        Assert.Equal(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("1", true));
        Assert.Equal(ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("GrEeN", true));
        Assert.Equal(ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("2", true));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("3", true));
        Assert.Equal(ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("blue", true));
        Assert.Equal(ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("4", true));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("BLUE , red", true));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("5", true));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("BlUe, GReen", true));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("6", true));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Red ,    BluE ,greEn", true));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("7", true));
        Assert.Equal(ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("UltRaVioLet", true));
        Assert.Equal(ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("8", true));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>(" GREEN ,blue,UltraViolet , 1", true));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("15", true));
    }

    [Fact]
    public void ParseFlags_ReturnsValidValue_WhenUsingValidStringWithCustomDelimiter()
    {
        Assert.Equal(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("Red", ignoreCase: false, "|"));
        Assert.Equal(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("1", ignoreCase: false, "|"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue | Red", ignoreCase: false, "|"));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue | Green", ignoreCase: false, "|"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("Blue | Green | Red", ignoreCase: false, "|"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>(" Blue |UltraViolet|2 | Red", ignoreCase: false, " | "));
    }

    [Fact]
    public void ParseFlags_ReturnsValidValue_WhenUsingValidStringWithCustomDelimiterWhileIgnoringCase()
    {
        Assert.Equal(ColorFlagEnum.Black, FlagEnums.ParseFlags<ColorFlagEnum>("black", true, "|"));
        Assert.Equal(ColorFlagEnum.Black, FlagEnums.ParseFlags<ColorFlagEnum>("0", true, "|"));
        Assert.Equal(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("ReD", true, "|"));
        Assert.Equal(ColorFlagEnum.Red, FlagEnums.ParseFlags<ColorFlagEnum>("1", true, "|"));
        Assert.Equal(ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("GrEeN", true, "|"));
        Assert.Equal(ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("2", true, "|"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("RED|green", true, "|"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green, FlagEnums.ParseFlags<ColorFlagEnum>("3", true, "|"));
        Assert.Equal(ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("blue", true, "|"));
        Assert.Equal(ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("4", true, "|"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("BLUE | red", true, "|"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("5", true, "|"));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("BlUe| GReen", true, "|"));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("6", true, "|"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("BluE |    greEn |Red", true, "|"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, FlagEnums.ParseFlags<ColorFlagEnum>("7", true, "|"));
        Assert.Equal(ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("UltRaVioLet", true, "|"));
        Assert.Equal(ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("8", true, "|"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>(" GREEN |blue|UltraViolet | 1", true, "|"));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, FlagEnums.ParseFlags<ColorFlagEnum>("15", true, "|"));
    }

    [Fact]
    public void ParseFlags_ThrowsArgumentNullException_WhenUsingNullString()
    {
        Assert.Throws<ArgumentNullException>(() => FlagEnums.ParseFlags<ColorFlagEnum>(null));
    }

    [Fact]
    public void ParseFlags_ThrowsArgumentException_WhenUsingWhiteSpaceString()
    {
        Assert.Throws<ArgumentException>(() => FlagEnums.ParseFlags<ColorFlagEnum>(" "));
    }

    [Fact]
    public void ParseFlags_ThrowsArgumentException_WhenUsingUndefinedString()
    {
        Assert.Throws<ArgumentException>(() => FlagEnums.ParseFlags<ColorFlagEnum>("Turquoise"));
    }

    [Fact]
    public void ParseFlags_ReturnsValidValue_WhenUsingInvalidNumber()
    {
        Assert.Equal((ColorFlagEnum)16, FlagEnums.ParseFlags<ColorFlagEnum>("16"));
    }

    [Fact]
    public void ParseFlags_ThrowsOverflowException_WhenUsingLargeNumber()
    {
        Assert.Throws<OverflowException>(() => FlagEnums.ParseFlags<ColorFlagEnum>("128"));
    }

    [Fact]
    public void ParseFlags_UsesDefaultDelimiter_WhenUsingNullDelimiter()
    {
        Assert.Equal(FlagEnums.ParseFlags<ColorFlagEnum>("Red, Green"), FlagEnums.ParseFlags<ColorFlagEnum>("Red, Green", ignoreCase: false, (string)null));
    }

    [Fact]
    public void ParseFlags_UsesDefaultDelimiter_WhenUsingEmptyStringDelimiter()
    {
        Assert.Equal(FlagEnums.ParseFlags<ColorFlagEnum>("Red, Green"), FlagEnums.ParseFlags<ColorFlagEnum>("Red, Green", ignoreCase: false, string.Empty));
    }

    [Fact]
    public void TryParseFlags_ReturnsValidValue_WhenUsingValidName()
    {
        ColorFlagEnum result;
        Assert.True(FlagEnums.TryParseFlags("Black", out result));
        Assert.Equal(ColorFlagEnum.Black, result);
        Assert.True(FlagEnums.TryParseFlags("Red", out result));
        Assert.Equal(ColorFlagEnum.Red, result);
        Assert.True(FlagEnums.TryParseFlags("Green", out result));
        Assert.Equal(ColorFlagEnum.Green, result);
        Assert.True(FlagEnums.TryParseFlags("Red, Green", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
        Assert.True(FlagEnums.TryParseFlags("Blue", out result));
        Assert.Equal(ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("Blue , Red", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("Blue , Green", out result));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("Blue , Green , Red", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("UltraViolet", out result));
        Assert.Equal(ColorFlagEnum.UltraViolet, result);
        Assert.True(FlagEnums.TryParseFlags(" Blue ,UltraViolet,Green , Red", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
    }

    [Fact]
    public void TryParseFlags_ReturnsValidValue_WhenUsingValidNumber()
    {
        ColorFlagEnum result;
        Assert.True(FlagEnums.TryParseFlags("0", out result));
        Assert.Equal(ColorFlagEnum.Black, result);
        Assert.True(FlagEnums.TryParseFlags("1", out result));
        Assert.Equal(ColorFlagEnum.Red, result);
        Assert.True(FlagEnums.TryParseFlags("+2", out result));
        Assert.Equal(ColorFlagEnum.Green, result);
        Assert.True(FlagEnums.TryParseFlags("3", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
        Assert.True(FlagEnums.TryParseFlags("4", out result));
        Assert.Equal(ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("+5", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("6", out result));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("7", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("8", out result));
        Assert.Equal(ColorFlagEnum.UltraViolet, result);
        Assert.True(FlagEnums.TryParseFlags(" Blue ,UltraViolet,+2 , Red", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
        Assert.True(FlagEnums.TryParseFlags("15", out result));
        Assert.Equal(ColorFlagEnum.All, result);
    }

    [Fact]
    public void TryParseFlags_ReturnsValidValue_WhenUsingValidStringWhileIgnoringCase()
    {
        ColorFlagEnum result;
        Assert.True(FlagEnums.TryParseFlags("black", true, out result));
        Assert.Equal(ColorFlagEnum.Black, result);
        Assert.True(FlagEnums.TryParseFlags("0", true, out result));
        Assert.Equal(ColorFlagEnum.Black, result);
        Assert.True(FlagEnums.TryParseFlags("ReD", true, out result));
        Assert.Equal(ColorFlagEnum.Red, result);
        Assert.True(FlagEnums.TryParseFlags("1", true, out result));
        Assert.Equal(ColorFlagEnum.Red, result);
        Assert.True(FlagEnums.TryParseFlags("GrEeN", true, out result));
        Assert.Equal(ColorFlagEnum.Green, result);
        Assert.True(FlagEnums.TryParseFlags("2", true, out result));
        Assert.Equal(ColorFlagEnum.Green, result);
        Assert.True(FlagEnums.TryParseFlags("3", true, out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
        Assert.True(FlagEnums.TryParseFlags("blue", true, out result));
        Assert.Equal(ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("4", true, out result));
        Assert.Equal(ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("BLUE , red", true, out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("5", true, out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("BlUe, GReen", true, out result));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("6", true, out result));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("Red ,    BluE ,greEn", true, out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("7", true, out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("UltRaVioLet", true, out result));
        Assert.Equal(ColorFlagEnum.UltraViolet, result);
        Assert.True(FlagEnums.TryParseFlags("8", true, out result));
        Assert.Equal(ColorFlagEnum.UltraViolet, result);
        Assert.True(FlagEnums.TryParseFlags(" GREEN ,blue,UltraViolet , 1", true, out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
        Assert.True(FlagEnums.TryParseFlags("15", true, out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
    }

    [Fact]
    public void TryParseFlags_ReturnsValidValue_WhenUsingValidStringWithCustomDelimiter()
    {
        ColorFlagEnum result;
        Assert.True(FlagEnums.TryParseFlags("Red", ignoreCase: false, "|", out result));
        Assert.Equal(ColorFlagEnum.Red, result);
        Assert.True(FlagEnums.TryParseFlags("1", ignoreCase: false, "|", out result));
        Assert.Equal(ColorFlagEnum.Red, result);
        Assert.True(FlagEnums.TryParseFlags("Blue | Red", ignoreCase: false, "|", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("Blue | Green", ignoreCase: false, "|", out result));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("Blue | Green | Red", ignoreCase: false, "|", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags(" Blue |UltraViolet|2 | Red", ignoreCase: false, " | ", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
    }

    [Fact]
    public void TryParseFlags_ReturnsValidValue_WhenUsingValidStringWithCustomDelimiterWhileIgnoringCase()
    {
        ColorFlagEnum result;
        Assert.True(FlagEnums.TryParseFlags("black", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Black, result);
        Assert.True(FlagEnums.TryParseFlags("0", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Black, result);
        Assert.True(FlagEnums.TryParseFlags("ReD", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Red, result);
        Assert.True(FlagEnums.TryParseFlags("1", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Red, result);
        Assert.True(FlagEnums.TryParseFlags("GrEeN", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Green, result);
        Assert.True(FlagEnums.TryParseFlags("2", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Green, result);
        Assert.True(FlagEnums.TryParseFlags("RED|green", true, "| ", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
        Assert.True(FlagEnums.TryParseFlags("3", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green, result);
        Assert.True(FlagEnums.TryParseFlags("blue", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("4", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("BLUE | red", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("5", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("BlUe| GReen", true, " | ", out result));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("6", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("BluE |    greEn |Red", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("7", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue, result);
        Assert.True(FlagEnums.TryParseFlags("UltRaVioLet", true, "|", out result));
        Assert.Equal(ColorFlagEnum.UltraViolet, result);
        Assert.True(FlagEnums.TryParseFlags("8", true, "|", out result));
        Assert.Equal(ColorFlagEnum.UltraViolet, result);
        Assert.True(FlagEnums.TryParseFlags(" GREEN |blue|UltraViolet | 1", true, " |", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
        Assert.True(FlagEnums.TryParseFlags("15", true, "|", out result));
        Assert.Equal(ColorFlagEnum.Red | ColorFlagEnum.Green | ColorFlagEnum.Blue | ColorFlagEnum.UltraViolet, result);
    }

    [Fact]
    public void TryParseFlags_ReturnsFalse_WhenUsingNullString()
    {
        ColorFlagEnum result;
        Assert.False(FlagEnums.TryParseFlags(null, out result));
        Assert.Equal(default, result);
    }

    [Fact]
    public void TryParseFlags_ReturnsFalse_WhenUsingEmptyString()
    {
        ColorFlagEnum result;
        Assert.False(FlagEnums.TryParseFlags(string.Empty, out result));
        Assert.Equal(default, result);
    }

    [Fact]
    public void TryParseFlags_ReturnsFalse_WhenUsingWhiteSpaceString()
    {
        ColorFlagEnum result;
        Assert.False(FlagEnums.TryParseFlags(" ", out result));
        Assert.Equal(default, result);
    }

    [Fact]
    public void TryParseFlags_ReturnsFalse_WhenUsingUndefinedString()
    {
        ColorFlagEnum result;
        Assert.False(FlagEnums.TryParseFlags("Turquoise", out result));
        Assert.Equal(default, result);
    }

    [Fact]
    public void TryParseFlags_ReturnsTrue_WhenUsingInvalidNumber()
    {
        ColorFlagEnum result;
        Assert.True(FlagEnums.TryParseFlags("16", out result));
        Assert.Equal((ColorFlagEnum)16, result);
    }

    [Fact]
    public void TryParseFlags_ReturnsFalse_WhenUsingLargeNumber()
    {
        ColorFlagEnum result;
        Assert.False(FlagEnums.TryParseFlags("128", out result));
        Assert.Equal(default, result);
    }

    [Fact]
    public void TryParseFlags_UsesDefaultDelimiter_WhenUsingNullDelimiter()
    {
        ColorFlagEnum result0;
        ColorFlagEnum result1;
        Assert.Equal(FlagEnums.TryParseFlags("Red, Green", out result0), FlagEnums.TryParseFlags("Red, Green", ignoreCase: false, null, out result1));
        Assert.Equal(result0, result1);
    }

    [Fact]
    public void TryParseFlags_UsesDefaultDelimiter_WhenUsingEmptyStringDelimiter()
    {
        ColorFlagEnum result0;
        ColorFlagEnum result1;
        Assert.Equal(FlagEnums.TryParseFlags("Red, Green", out result0), FlagEnums.TryParseFlags("Red, Green", ignoreCase: false, string.Empty, out result1));
        Assert.Equal(result0, result1);
    }
    #endregion

    #region EnumMember Extensions
    [Fact]
    public void GetFlags_ReturnsValidValues_WhenUsingValidValue()
    {
        Assert.Equal([ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet], ColorFlagEnum.All.GetMember().GetFlags());
    }
    #endregion
}