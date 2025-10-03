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
using System.Collections.Generic;
using System.Runtime.Serialization;
using EnumsNET.Tests.TestEnums;
using Xunit;
using static EnumsNET.Enums;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace EnumsNET.Tests;

public class EnumsTest
{
    #region Type Methods
    [Fact]
    public void GetUnderlyingType()
    {
        Assert.Equal(typeof(sbyte), GetUnderlyingType<SByteEnum>());
        Assert.Equal(typeof(byte), GetUnderlyingType<ByteEnum>());
        Assert.Equal(typeof(short), GetUnderlyingType<Int16Enum>());
        Assert.Equal(typeof(ushort), GetUnderlyingType<UInt16Enum>());
        Assert.Equal(typeof(int), GetUnderlyingType<Int32Enum>());
        Assert.Equal(typeof(uint), GetUnderlyingType<UInt32Enum>());
        Assert.Equal(typeof(long), GetUnderlyingType<Int64Enum>());
        Assert.Equal(typeof(ulong), GetUnderlyingType<UInt64Enum>());
        Assert.Equal(typeof(char), GetUnderlyingType<CharEnum>());
    }

    [Fact]
    public void GetTypeCode()
    {
        Assert.Equal(TypeCode.SByte, GetTypeCode<SByteEnum>());
        Assert.Equal(TypeCode.Byte, GetTypeCode<ByteEnum>());
        Assert.Equal(TypeCode.Int16, GetTypeCode<Int16Enum>());
        Assert.Equal(TypeCode.UInt16, GetTypeCode<UInt16Enum>());
        Assert.Equal(TypeCode.Int32, GetTypeCode<Int32Enum>());
        Assert.Equal(TypeCode.UInt32, GetTypeCode<UInt32Enum>());
        Assert.Equal(TypeCode.Int64, GetTypeCode<Int64Enum>());
        Assert.Equal(TypeCode.UInt64, GetTypeCode<UInt64Enum>());
        Assert.Equal(TypeCode.Char, GetTypeCode<CharEnum>());
    }

    [Fact]
    public void GetMemberCount()
    {
        Assert.Equal(0, GetMemberCount<ByteEnum>());
        Assert.Equal(38, GetMemberCount<DateFilterOperator>());
        Assert.Equal(6, GetMemberCount<ColorFlagEnum>());
        Assert.Equal(10, GetMemberCount<NumericOperator>());
        Assert.Equal(3, GetMemberCount<CharEnum>());

        Assert.Equal(0, GetMemberCount<ByteEnum>(EnumMemberSelection.Distinct));
        Assert.Equal(38, GetMemberCount<DateFilterOperator>(EnumMemberSelection.Distinct));
        Assert.Equal(6, GetMemberCount<ColorFlagEnum>(EnumMemberSelection.Distinct));
        Assert.Equal(8, GetMemberCount<NumericOperator>(EnumMemberSelection.Distinct)); // Has 2 duplicates
        Assert.Equal(3, GetMemberCount<CharEnum>(EnumMemberSelection.Distinct));

        Assert.Equal(4, GetMemberCount<ColorFlagEnum>(EnumMemberSelection.Flags));
    }

    [Fact]
    public void GetMembers()
    {
        Assert.Empty(GetMembers<ByteEnum>());
        Assert.Equal(38, GetMembers<DateFilterOperator>().Count);
        Assert.Equal(6, GetMembers<ColorFlagEnum>().Count);
        Assert.Equal(10, GetMembers<NumericOperator>().Count);
        Assert.Equal(3, GetMembers<CharEnum>().Count);

        Assert.Empty(GetMembers<ByteEnum>(EnumMemberSelection.Distinct));
        Assert.Equal(38, GetMembers<DateFilterOperator>(EnumMemberSelection.Distinct).Count);
        Assert.Equal(6, GetMembers<ColorFlagEnum>(EnumMemberSelection.Distinct).Count);
        Assert.Equal(8, GetMembers<NumericOperator>(EnumMemberSelection.Distinct).Count); // Has 2 duplicates
        Assert.Equal(3, GetMembers<CharEnum>(EnumMemberSelection.Distinct).Count);

        Assert.Equal(4, GetMembers<ColorFlagEnum>(EnumMemberSelection.Flags).Count);

        var members = GetMembers<ColorFlagEnum>();
        AssertEnumMemberIsCorrect(members[0], ColorFlagEnum.Black, "Black");
        AssertEnumMemberIsCorrect(members[1], ColorFlagEnum.Red, "Red");
        AssertEnumMemberIsCorrect(members[2], ColorFlagEnum.Green, "Green");
        AssertEnumMemberIsCorrect(members[3], ColorFlagEnum.Blue, "Blue");
        AssertEnumMemberIsCorrect(members[4], ColorFlagEnum.UltraViolet, "UltraViolet", new DescriptionAttribute("Ultra-Violet"));
        AssertEnumMemberIsCorrect(members[5], ColorFlagEnum.All, "All");

        members = GetMembers<ColorFlagEnum>(EnumMemberSelection.Flags);
        AssertEnumMemberIsCorrect(members[0], ColorFlagEnum.Red, "Red");
        AssertEnumMemberIsCorrect(members[1], ColorFlagEnum.Green, "Green");
        AssertEnumMemberIsCorrect(members[2], ColorFlagEnum.Blue, "Blue");
        AssertEnumMemberIsCorrect(members[3], ColorFlagEnum.UltraViolet, "UltraViolet", new DescriptionAttribute("Ultra-Violet"));
    }

    private static void AssertEnumMemberIsCorrect<TEnum>(EnumMember<TEnum> member, TEnum value, string name, params Attribute[] attributes)
    {
        Assert.NotNull(member);
        Assert.Equal(value, member.Value);
        Assert.Equal(name, member.Name);
        Assert.Equivalent(attributes, member.Attributes);
    }

    [Fact]
    public void GetNames()
    {
        Assert.Equal(["Black", "Red", "Green", "Blue", "UltraViolet", "All"], GetNames<ColorFlagEnum>());
        Assert.Equal(Enum.GetNames(typeof(DateFilterOperator)), GetNames<DateFilterOperator>());
        Assert.Equal([], GetNames<ByteEnum>());
        Assert.Equal(["A", "B", "C"], GetNames<CharEnum>());
    }

    [Fact]
    public void GetValues()
    {
        Assert.Equal([ColorFlagEnum.Black, ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet, ColorFlagEnum.All], GetValues<ColorFlagEnum>());
        Assert.Equal((DateFilterOperator[])Enum.GetValues(typeof(DateFilterOperator)), GetValues<DateFilterOperator>());
        Assert.Equal([], GetValues<ByteEnum>());
        Assert.Equal(new List<CharEnum> { ToObject<CharEnum>('a'), ToObject<CharEnum>('b'), ToObject<CharEnum>('c') }, GetValues<CharEnum>());

        // Duplicate order check
        var numericFilterOperators = GetValues<NumericOperator>();
        for (var i = 1; i < numericFilterOperators.Count; ++i)
        {
            Assert.True(numericFilterOperators[i - 1] <= numericFilterOperators[i]);
        }

        Assert.Equal([DisplayAttributeEnum.Up, DisplayAttributeEnum.Down, DisplayAttributeEnum.Left, DisplayAttributeEnum.Right], GetValues<DisplayAttributeEnum>());
        Assert.Equal([DisplayAttributeEnum.Left, DisplayAttributeEnum.Up, DisplayAttributeEnum.Down, DisplayAttributeEnum.Right], GetValues<DisplayAttributeEnum>(EnumMemberSelection.DisplayOrder));
    }

    [Fact]
    public void GetValues_ExcludeDuplicates()
    {
        Assert.Equal([ColorFlagEnum.Black, ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet, ColorFlagEnum.All], GetValues<ColorFlagEnum>(EnumMemberSelection.Distinct));
        Assert.Equal((DateFilterOperator[])Enum.GetValues(typeof(DateFilterOperator)), GetValues<DateFilterOperator>(EnumMemberSelection.Distinct));
        Assert.Equal([], GetValues<ByteEnum>(EnumMemberSelection.Distinct));
        Assert.Equal([NumericOperator.Equals, NumericOperator.NotEquals, NumericOperator.GreaterThan, NumericOperator.LessThan, NumericOperator.GreaterThanOrEquals, NumericOperator.NotGreaterThan, NumericOperator.Between, NumericOperator.NotBetween], GetValues<NumericOperator>(EnumMemberSelection.Distinct));
    }
    #endregion

    #region ToObject
    [Fact]
    public void ToObject_ReturnsValidValue_WhenUsingValidNumber()
    {
        Assert.Equal((SByteEnum)1, ToObject<SByteEnum>((sbyte)1));
        Assert.Equal((SByteEnum)1, ToObject<SByteEnum>((byte)1));
        Assert.Equal((SByteEnum)1, ToObject<SByteEnum>((short)1));
        Assert.Equal((SByteEnum)1, ToObject<SByteEnum>((ushort)1));
        Assert.Equal((SByteEnum)1, ToObject<SByteEnum>(1));
        Assert.Equal((SByteEnum)1, ToObject<SByteEnum>(1U));
        Assert.Equal((SByteEnum)1, ToObject<SByteEnum>(1L));
        Assert.Equal((SByteEnum)1, ToObject<SByteEnum>(1UL));

        Assert.Equal((ByteEnum)1, ToObject<ByteEnum>((sbyte)1));
        Assert.Equal((ByteEnum)1, ToObject<ByteEnum>((byte)1));
        Assert.Equal((ByteEnum)1, ToObject<ByteEnum>((short)1));
        Assert.Equal((ByteEnum)1, ToObject<ByteEnum>((ushort)1));
        Assert.Equal((ByteEnum)1, ToObject<ByteEnum>(1));
        Assert.Equal((ByteEnum)1, ToObject<ByteEnum>(1U));
        Assert.Equal((ByteEnum)1, ToObject<ByteEnum>(1L));
        Assert.Equal((ByteEnum)1, ToObject<ByteEnum>(1UL));

        Assert.Equal((Int16Enum)1, ToObject<Int16Enum>((sbyte)1));
        Assert.Equal((Int16Enum)1, ToObject<Int16Enum>((byte)1));
        Assert.Equal((Int16Enum)1, ToObject<Int16Enum>((short)1));
        Assert.Equal((Int16Enum)1, ToObject<Int16Enum>((ushort)1));
        Assert.Equal((Int16Enum)1, ToObject<Int16Enum>(1));
        Assert.Equal((Int16Enum)1, ToObject<Int16Enum>(1U));
        Assert.Equal((Int16Enum)1, ToObject<Int16Enum>(1L));
        Assert.Equal((Int16Enum)1, ToObject<Int16Enum>(1UL));

        Assert.Equal((UInt16Enum)1, ToObject<UInt16Enum>((sbyte)1));
        Assert.Equal((UInt16Enum)1, ToObject<UInt16Enum>((byte)1));
        Assert.Equal((UInt16Enum)1, ToObject<UInt16Enum>((short)1));
        Assert.Equal((UInt16Enum)1, ToObject<UInt16Enum>((ushort)1));
        Assert.Equal((UInt16Enum)1, ToObject<UInt16Enum>(1));
        Assert.Equal((UInt16Enum)1, ToObject<UInt16Enum>(1U));
        Assert.Equal((UInt16Enum)1, ToObject<UInt16Enum>(1L));
        Assert.Equal((UInt16Enum)1, ToObject<UInt16Enum>(1UL));

        Assert.Equal((Int32Enum)1, ToObject<Int32Enum>((sbyte)1));
        Assert.Equal((Int32Enum)1, ToObject<Int32Enum>((byte)1));
        Assert.Equal((Int32Enum)1, ToObject<Int32Enum>((short)1));
        Assert.Equal((Int32Enum)1, ToObject<Int32Enum>((ushort)1));
        Assert.Equal((Int32Enum)1, ToObject<Int32Enum>(1));
        Assert.Equal((Int32Enum)1, ToObject<Int32Enum>(1U));
        Assert.Equal((Int32Enum)1, ToObject<Int32Enum>(1L));
        Assert.Equal((Int32Enum)1, ToObject<Int32Enum>(1UL));

        Assert.Equal((UInt32Enum)1, ToObject<UInt32Enum>((sbyte)1));
        Assert.Equal((UInt32Enum)1, ToObject<UInt32Enum>((byte)1));
        Assert.Equal((UInt32Enum)1, ToObject<UInt32Enum>((short)1));
        Assert.Equal((UInt32Enum)1, ToObject<UInt32Enum>((ushort)1));
        Assert.Equal((UInt32Enum)1, ToObject<UInt32Enum>(1));
        Assert.Equal((UInt32Enum)1, ToObject<UInt32Enum>(1U));
        Assert.Equal((UInt32Enum)1, ToObject<UInt32Enum>(1L));
        Assert.Equal((UInt32Enum)1, ToObject<UInt32Enum>(1UL));

        Assert.Equal((Int64Enum)1, ToObject<Int64Enum>((sbyte)1));
        Assert.Equal((Int64Enum)1, ToObject<Int64Enum>((byte)1));
        Assert.Equal((Int64Enum)1, ToObject<Int64Enum>((short)1));
        Assert.Equal((Int64Enum)1, ToObject<Int64Enum>((ushort)1));
        Assert.Equal((Int64Enum)1, ToObject<Int64Enum>(1));
        Assert.Equal((Int64Enum)1, ToObject<Int64Enum>(1U));
        Assert.Equal((Int64Enum)1, ToObject<Int64Enum>(1L));
        Assert.Equal((Int64Enum)1, ToObject<Int64Enum>(1UL));

        Assert.Equal((UInt64Enum)1, ToObject<UInt64Enum>((sbyte)1));
        Assert.Equal((UInt64Enum)1, ToObject<UInt64Enum>((byte)1));
        Assert.Equal((UInt64Enum)1, ToObject<UInt64Enum>((short)1));
        Assert.Equal((UInt64Enum)1, ToObject<UInt64Enum>((ushort)1));
        Assert.Equal((UInt64Enum)1, ToObject<UInt64Enum>(1));
        Assert.Equal((UInt64Enum)1, ToObject<UInt64Enum>(1U));
        Assert.Equal((UInt64Enum)1, ToObject<UInt64Enum>(1L));
        Assert.Equal((UInt64Enum)1, ToObject<UInt64Enum>(1UL));

        Assert.Equal(ToObject<CharEnum>(1), ToObject<CharEnum>((sbyte)1));
        Assert.Equal(ToObject<CharEnum>(1), ToObject<CharEnum>((byte)1));
        Assert.Equal(ToObject<CharEnum>(1), ToObject<CharEnum>((short)1));
        Assert.Equal(ToObject<CharEnum>(1), ToObject<CharEnum>((ushort)1));
        Assert.Equal(ToObject<CharEnum>(1), ToObject<CharEnum>(1));
        Assert.Equal(ToObject<CharEnum>(1), ToObject<CharEnum>(1U));
        Assert.Equal(ToObject<CharEnum>(1), ToObject<CharEnum>(1L));
        Assert.Equal(ToObject<CharEnum>(1), ToObject<CharEnum>(1UL));
    }

    [Fact]
    public void ToObject_ThrowsOverflowException_WhenUsingValuesOutsideValueRange()
    {
        Assert.Throws<OverflowException>(() => ToObject<SByteEnum>(byte.MaxValue));
        Assert.Throws<OverflowException>(() => ToObject<ByteEnum>(sbyte.MinValue));
        Assert.Throws<OverflowException>(() => ToObject<Int16Enum>(ushort.MaxValue));
        Assert.Throws<OverflowException>(() => ToObject<UInt16Enum>(short.MinValue));
        Assert.Throws<OverflowException>(() => ToObject<Int32Enum>(uint.MaxValue));
        Assert.Throws<OverflowException>(() => ToObject<UInt32Enum>(int.MinValue));
        Assert.Throws<OverflowException>(() => ToObject<Int64Enum>(ulong.MaxValue));
        Assert.Throws<OverflowException>(() => ToObject<UInt64Enum>(long.MinValue));
        Assert.Throws<OverflowException>(() => ToObject<CharEnum>(short.MinValue));
    }

    [Fact]
    public void ToObject_ThrowsArgumentException_WhenUsingInvalidValueWithValidation()
    {
        Assert.Throws<ArgumentException>(() => ToObject<SByteEnum>(sbyte.MaxValue, EnumValidation.Default));
        Assert.Throws<ArgumentException>(() => ToObject<ByteEnum>(byte.MaxValue, EnumValidation.Default));
        Assert.Throws<ArgumentException>(() => ToObject<Int16Enum>(short.MaxValue, EnumValidation.Default));
        Assert.Throws<ArgumentException>(() => ToObject<UInt16Enum>(ushort.MaxValue, EnumValidation.Default));
        Assert.Throws<ArgumentException>(() => ToObject<Int32Enum>(int.MaxValue, EnumValidation.Default));
        Assert.Throws<ArgumentException>(() => ToObject<UInt32Enum>(uint.MaxValue, EnumValidation.Default));
        Assert.Throws<ArgumentException>(() => ToObject<Int64Enum>(long.MaxValue, EnumValidation.Default));
        Assert.Throws<ArgumentException>(() => ToObject<UInt64Enum>(ulong.MaxValue, EnumValidation.Default));
        Assert.Throws<ArgumentException>(() => ToObject<CharEnum>(ushort.MinValue, EnumValidation.Default));
    }

    [Fact]
    public void TryToObject_ReturnsTrueAndValidValue_WhenUsingValidNumber()
    {
        SByteEnum sbyteResult;
        var sbyteValue = (SByteEnum)1;
        Assert.True(TryToObject((sbyte)1, out sbyteResult));
        Assert.Equal(sbyteValue, sbyteResult);
        Assert.True(TryToObject((byte)1, out sbyteResult));
        Assert.Equal(sbyteValue, sbyteResult);
        Assert.True(TryToObject((short)1, out sbyteResult));
        Assert.Equal(sbyteValue, sbyteResult);
        Assert.True(TryToObject((ushort)1, out sbyteResult));
        Assert.Equal(sbyteValue, sbyteResult);
        Assert.True(TryToObject(1, out sbyteResult));
        Assert.Equal(sbyteValue, sbyteResult);
        Assert.True(TryToObject(1U, out sbyteResult));
        Assert.Equal(sbyteValue, sbyteResult);
        Assert.True(TryToObject(1L, out sbyteResult));
        Assert.Equal(sbyteValue, sbyteResult);
        Assert.True(TryToObject(1UL, out sbyteResult));
        Assert.Equal(sbyteValue, sbyteResult);

        ByteEnum byteResult;
        var byteValue = (ByteEnum)1;
        Assert.True(TryToObject((sbyte)1, out byteResult));
        Assert.Equal(byteValue, byteResult);
        Assert.True(TryToObject((byte)1, out byteResult));
        Assert.Equal(byteValue, byteResult);
        Assert.True(TryToObject((short)1, out byteResult));
        Assert.Equal(byteValue, byteResult);
        Assert.True(TryToObject((ushort)1, out byteResult));
        Assert.Equal(byteValue, byteResult);
        Assert.True(TryToObject(1, out byteResult));
        Assert.Equal(byteValue, byteResult);
        Assert.True(TryToObject(1U, out byteResult));
        Assert.Equal(byteValue, byteResult);
        Assert.True(TryToObject(1L, out byteResult));
        Assert.Equal(byteValue, byteResult);
        Assert.True(TryToObject(1UL, out byteResult));
        Assert.Equal(byteValue, byteResult);

        Int16Enum int16Result;
        var int16Value = (Int16Enum)1;
        Assert.True(TryToObject((sbyte)1, out int16Result));
        Assert.Equal(int16Value, int16Result);
        Assert.True(TryToObject((byte)1, out int16Result));
        Assert.Equal(int16Value, int16Result);
        Assert.True(TryToObject((short)1, out int16Result));
        Assert.Equal(int16Value, int16Result);
        Assert.True(TryToObject((ushort)1, out int16Result));
        Assert.Equal(int16Value, int16Result);
        Assert.True(TryToObject(1, out int16Result));
        Assert.Equal(int16Value, int16Result);
        Assert.True(TryToObject(1U, out int16Result));
        Assert.Equal(int16Value, int16Result);
        Assert.True(TryToObject(1L, out int16Result));
        Assert.Equal(int16Value, int16Result);
        Assert.True(TryToObject(1UL, out int16Result));
        Assert.Equal(int16Value, int16Result);

        UInt16Enum uint16Result;
        var uint16Value = (UInt16Enum)1;
        Assert.True(TryToObject((sbyte)1, out uint16Result));
        Assert.Equal(uint16Value, uint16Result);
        Assert.True(TryToObject((byte)1, out uint16Result));
        Assert.Equal(uint16Value, uint16Result);
        Assert.True(TryToObject((short)1, out uint16Result));
        Assert.Equal(uint16Value, uint16Result);
        Assert.True(TryToObject((ushort)1, out uint16Result));
        Assert.Equal(uint16Value, uint16Result);
        Assert.True(TryToObject(1, out uint16Result));
        Assert.Equal(uint16Value, uint16Result);
        Assert.True(TryToObject(1U, out uint16Result));
        Assert.Equal(uint16Value, uint16Result);
        Assert.True(TryToObject(1L, out uint16Result));
        Assert.Equal(uint16Value, uint16Result);
        Assert.True(TryToObject(1UL, out uint16Result));
        Assert.Equal(uint16Value, uint16Result);

        Int32Enum int32Result;
        var int32Value = (Int32Enum)1;
        Assert.True(TryToObject((sbyte)1, out int32Result));
        Assert.Equal(int32Value, int32Result);
        Assert.True(TryToObject((byte)1, out int32Result));
        Assert.Equal(int32Value, int32Result);
        Assert.True(TryToObject((short)1, out int32Result));
        Assert.Equal(int32Value, int32Result);
        Assert.True(TryToObject((ushort)1, out int32Result));
        Assert.Equal(int32Value, int32Result);
        Assert.True(TryToObject(1, out int32Result));
        Assert.Equal(int32Value, int32Result);
        Assert.True(TryToObject(1U, out int32Result));
        Assert.Equal(int32Value, int32Result);
        Assert.True(TryToObject(1L, out int32Result));
        Assert.Equal(int32Value, int32Result);
        Assert.True(TryToObject(1UL, out int32Result));
        Assert.Equal(int32Value, int32Result);

        UInt32Enum uint32Result;
        var uint32Value = (UInt32Enum)1;
        Assert.True(TryToObject((sbyte)1, out uint32Result));
        Assert.Equal(uint32Value, uint32Result);
        Assert.True(TryToObject((byte)1, out uint32Result));
        Assert.Equal(uint32Value, uint32Result);
        Assert.True(TryToObject((short)1, out uint32Result));
        Assert.Equal(uint32Value, uint32Result);
        Assert.True(TryToObject((ushort)1, out uint32Result));
        Assert.Equal(uint32Value, uint32Result);
        Assert.True(TryToObject(1, out uint32Result));
        Assert.Equal(uint32Value, uint32Result);
        Assert.True(TryToObject(1U, out uint32Result));
        Assert.Equal(uint32Value, uint32Result);
        Assert.True(TryToObject(1L, out uint32Result));
        Assert.Equal(uint32Value, uint32Result);
        Assert.True(TryToObject(1UL, out uint32Result));
        Assert.Equal(uint32Value, uint32Result);

        Int64Enum int64Result;
        var int64Value = (Int64Enum)1;
        Assert.True(TryToObject((sbyte)1, out int64Result));
        Assert.Equal(int64Value, int64Result);
        Assert.True(TryToObject((byte)1, out int64Result));
        Assert.Equal(int64Value, int64Result);
        Assert.True(TryToObject((short)1, out int64Result));
        Assert.Equal(int64Value, int64Result);
        Assert.True(TryToObject((ushort)1, out int64Result));
        Assert.Equal(int64Value, int64Result);
        Assert.True(TryToObject(1, out int64Result));
        Assert.Equal(int64Value, int64Result);
        Assert.True(TryToObject(1U, out int64Result));
        Assert.Equal(int64Value, int64Result);
        Assert.True(TryToObject(1L, out int64Result));
        Assert.Equal(int64Value, int64Result);
        Assert.True(TryToObject(1UL, out int64Result));
        Assert.Equal(int64Value, int64Result);

        UInt64Enum uint64Result;
        var uint64Value = (UInt64Enum)1;
        Assert.True(TryToObject((sbyte)1, out uint64Result));
        Assert.Equal(uint64Value, uint64Result);
        Assert.True(TryToObject((byte)1, out uint64Result));
        Assert.Equal(uint64Value, uint64Result);
        Assert.True(TryToObject((short)1, out uint64Result));
        Assert.Equal(uint64Value, uint64Result);
        Assert.True(TryToObject((ushort)1, out uint64Result));
        Assert.Equal(uint64Value, uint64Result);
        Assert.True(TryToObject(1, out uint64Result));
        Assert.Equal(uint64Value, uint64Result);
        Assert.True(TryToObject(1U, out uint64Result));
        Assert.Equal(uint64Value, uint64Result);
        Assert.True(TryToObject(1L, out uint64Result));
        Assert.Equal(uint64Value, uint64Result);
        Assert.True(TryToObject(1UL, out uint64Result));
        Assert.Equal(uint64Value, uint64Result);

        CharEnum charResult;
        var charValue = ToObject<CharEnum>(1);
        Assert.True(TryToObject((sbyte)1, out charResult));
        Assert.Equal(charValue, charResult);
        Assert.True(TryToObject((byte)1, out charResult));
        Assert.Equal(charValue, charResult);
        Assert.True(TryToObject((short)1, out charResult));
        Assert.Equal(charValue, charResult);
        Assert.True(TryToObject((ushort)1, out charResult));
        Assert.Equal(charValue, charResult);
        Assert.True(TryToObject(1, out charResult));
        Assert.Equal(charValue, charResult);
        Assert.True(TryToObject(1U, out charResult));
        Assert.Equal(charValue, charResult);
        Assert.True(TryToObject(1L, out charResult));
        Assert.Equal(charValue, charResult);
        Assert.True(TryToObject(1UL, out charResult));
        Assert.Equal(charValue, charResult);
    }

    [Fact]
    public void TryToObject_ReturnsFalse_WhenUsingValueInRangeButNotValid()
    {
        ColorFlagEnum result;
        Assert.False(TryToObject((sbyte)16, out result, EnumValidation.Default));
        Assert.False(TryToObject((byte)16, out result, EnumValidation.Default));
        Assert.False(TryToObject((short)16, out result, EnumValidation.Default));
        Assert.False(TryToObject((ushort)16, out result, EnumValidation.Default));
        Assert.False(TryToObject(16, out result, EnumValidation.Default));
        Assert.False(TryToObject(16U, out result, EnumValidation.Default));
        Assert.False(TryToObject(16L, out result, EnumValidation.Default));
        Assert.False(TryToObject(16UL, out result, EnumValidation.Default));
    }

    [Fact]
    public void TryToObject_ReturnsTrueAndValidValue_WhenUsingValueInRangeButNotValidWithoutValidation()
    {
        ColorFlagEnum result;
        var value = (ColorFlagEnum)16;
        Assert.True(TryToObject((sbyte)16, out result));
        Assert.Equal(value, result);
        Assert.True(TryToObject((byte)16, out result));
        Assert.Equal(value, result);
        Assert.True(TryToObject((short)16, out result));
        Assert.Equal(value, result);
        Assert.True(TryToObject((ushort)16, out result));
        Assert.Equal(value, result);
        Assert.True(TryToObject(16, out result));
        Assert.Equal(value, result);
        Assert.True(TryToObject(16U, out result));
        Assert.Equal(value, result);
        Assert.True(TryToObject(16L, out result));
        Assert.Equal(value, result);
        Assert.True(TryToObject(16UL, out result));
        Assert.Equal(value, result);
    }

    [Fact]
    public void TryToObject_ReturnsFalse_WhenUsingValueOutOfRange()
    {
        ColorFlagEnum result;
        Assert.False(TryToObject((byte)128, out result));
        Assert.Equal(default, result);
        Assert.False(TryToObject((short)128, out result));
        Assert.Equal(default, result);
        Assert.False(TryToObject((ushort)128, out result));
        Assert.Equal(default, result);
        Assert.False(TryToObject(128, out result));
        Assert.Equal(default, result);
        Assert.False(TryToObject(128U, out result));
        Assert.Equal(default, result);
        Assert.False(TryToObject(128L, out result));
        Assert.Equal(default, result);
        Assert.False(TryToObject(128UL, out result));
        Assert.Equal(default, result);

        Assert.False(TryToObject((byte)128, out result));
        Assert.Equal(default, result);
        Assert.False(TryToObject((short)128, out result));
        Assert.Equal(default, result);
        Assert.False(TryToObject((ushort)128, out result));
        Assert.Equal(default, result);
        Assert.False(TryToObject(128, out result));
        Assert.Equal(default, result);
        Assert.False(TryToObject(128U, out result));
        Assert.Equal(default, result);
        Assert.False(TryToObject(128L, out result));
        Assert.Equal(default, result);
        Assert.False(TryToObject(128UL, out result));
        Assert.Equal(default, result);
    }
    #endregion

    #region All Values Main Methods
    [Fact]
    public void IsValid_ReturnsSameResultAsIsValidFlagCombination_WhenUsingFlagEnum()
    {
        for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
        {
            var value = (ColorFlagEnum)i;
            Assert.Equal(FlagEnums.IsValidFlagCombination(value), value.IsValid());
        }
    }

    [Fact]
    public void IsValid()
    {
        for (int i = short.MinValue; i <= short.MaxValue; ++i)
        {
            var value = (DateFilterOperator)i;
            Assert.Equal(Enum.IsDefined(typeof(DateFilterOperator), value), value.IsValid());
        }

        Assert.True(NonContiguousEnum.Cat.IsValid());
        Assert.True(NonContiguousEnum.Dog.IsValid());
        Assert.True(NonContiguousEnum.Chimp.IsValid());
        Assert.True(NonContiguousEnum.Elephant.IsValid());
        Assert.True(NonContiguousEnum.Whale.IsValid());
        Assert.True(NonContiguousEnum.Eagle.IsValid());
        Assert.False(((NonContiguousEnum)(-5)).IsValid());

        Assert.True(UInt64FlagEnum.Flies.IsValid());
        Assert.True(UInt64FlagEnum.Hops.IsValid());
        Assert.True(UInt64FlagEnum.Runs.IsValid());
        Assert.True(UInt64FlagEnum.Slithers.IsValid());
        Assert.True(UInt64FlagEnum.Stationary.IsValid());
        Assert.True(UInt64FlagEnum.Swims.IsValid());
        Assert.True(UInt64FlagEnum.Walks.IsValid());
        Assert.True((UInt64FlagEnum.Flies | UInt64FlagEnum.Hops).IsValid());
        Assert.True((UInt64FlagEnum.Flies | UInt64FlagEnum.Slithers).IsValid());
        Assert.False(((UInt64FlagEnum)8).IsValid());
        Assert.False(((UInt64FlagEnum)8 | UInt64FlagEnum.Hops).IsValid());

        Assert.True(ContiguousUInt64Enum.A.IsValid());
        Assert.True(ContiguousUInt64Enum.B.IsValid());
        Assert.True(ContiguousUInt64Enum.C.IsValid());
        Assert.True(ContiguousUInt64Enum.D.IsValid());
        Assert.True(ContiguousUInt64Enum.E.IsValid());
        Assert.True(ContiguousUInt64Enum.F.IsValid());
        Assert.False((ContiguousUInt64Enum.A - 1).IsValid());
        Assert.False((ContiguousUInt64Enum.F + 1).IsValid());

        Assert.True(NonContiguousUInt64Enum.SaintLouis.IsValid());
        Assert.True(NonContiguousUInt64Enum.Chicago.IsValid());
        Assert.True(NonContiguousUInt64Enum.Cincinnati.IsValid());
        Assert.True(NonContiguousUInt64Enum.Pittsburg.IsValid());
        Assert.True(NonContiguousUInt64Enum.Milwaukee.IsValid());
        Assert.False(((NonContiguousUInt64Enum)5).IsValid());
        Assert.False(((NonContiguousUInt64Enum)50000000UL).IsValid());

        Assert.True(NumericOperator.Equals.IsValid());
        Assert.True(NumericOperator.NotEquals.IsValid());
        Assert.True(NumericOperator.GreaterThan.IsValid());
        Assert.True(NumericOperator.LessThan.IsValid());
        Assert.True(NumericOperator.GreaterThanOrEquals.IsValid());
        Assert.True(NumericOperator.NotLessThan.IsValid());
        Assert.True(NumericOperator.LessThanOrEquals.IsValid());
        Assert.True(NumericOperator.NotGreaterThan.IsValid());
        Assert.True(NumericOperator.Between.IsValid());
        Assert.True(NumericOperator.NotBetween.IsValid());
        Assert.False((NumericOperator.Equals - 1).IsValid());
        Assert.False((NumericOperator.NotBetween + 1).IsValid());
    }

    [Fact]
    public void IsValid_UsingValidator()
    {
        Assert.True(TypeNameHandling.None.IsValid());
        Assert.True(TypeNameHandling.Objects.IsValid());
        Assert.True(TypeNameHandling.Arrays.IsValid());
        Assert.True((TypeNameHandling.Objects | TypeNameHandling.Arrays).IsValid());
        Assert.True(TypeNameHandling.Auto.IsValid());
        Assert.False((TypeNameHandling.Auto | TypeNameHandling.Objects).IsValid());
    }

    [Fact]
    public void IsDefined()
    {
        for (int i = byte.MinValue; i <= byte.MaxValue; ++i)
        {
            var value = (ColorFlagEnum)i;
            Assert.Equal(Enum.IsDefined(typeof(ColorFlagEnum), value), value.IsDefined());
        }

        for (int i = short.MinValue; i <= short.MaxValue; ++i)
        {
            var value = (DateFilterOperator)i;
            Assert.Equal(Enum.IsDefined(typeof(DateFilterOperator), value), value.IsDefined());
        }

        Assert.True(NonContiguousEnum.Cat.IsDefined());
        Assert.True(NonContiguousEnum.Dog.IsDefined());
        Assert.True(NonContiguousEnum.Chimp.IsDefined());
        Assert.True(NonContiguousEnum.Elephant.IsDefined());
        Assert.True(NonContiguousEnum.Whale.IsDefined());
        Assert.True(NonContiguousEnum.Eagle.IsDefined());
        Assert.False(((NonContiguousEnum)(-5)).IsDefined());

        Assert.True(UInt64FlagEnum.Flies.IsDefined());
        Assert.True(UInt64FlagEnum.Hops.IsDefined());
        Assert.True(UInt64FlagEnum.Runs.IsDefined());
        Assert.True(UInt64FlagEnum.Slithers.IsDefined());
        Assert.True(UInt64FlagEnum.Stationary.IsDefined());
        Assert.True(UInt64FlagEnum.Swims.IsDefined());
        Assert.True(UInt64FlagEnum.Walks.IsDefined());
        Assert.False((UInt64FlagEnum.Flies | UInt64FlagEnum.Hops).IsDefined());
        Assert.False((UInt64FlagEnum.Flies | UInt64FlagEnum.Slithers).IsDefined());
        Assert.False(((UInt64FlagEnum)8).IsDefined());
        Assert.False(((UInt64FlagEnum)8 | UInt64FlagEnum.Hops).IsDefined());

        Assert.True(ContiguousUInt64Enum.A.IsDefined());
        Assert.True(ContiguousUInt64Enum.B.IsDefined());
        Assert.True(ContiguousUInt64Enum.C.IsDefined());
        Assert.True(ContiguousUInt64Enum.D.IsDefined());
        Assert.True(ContiguousUInt64Enum.E.IsDefined());
        Assert.True(ContiguousUInt64Enum.F.IsDefined());
        Assert.False((ContiguousUInt64Enum.A - 1).IsDefined());
        Assert.False((ContiguousUInt64Enum.F + 1).IsDefined());

        Assert.True(NonContiguousUInt64Enum.SaintLouis.IsDefined());
        Assert.True(NonContiguousUInt64Enum.Chicago.IsDefined());
        Assert.True(NonContiguousUInt64Enum.Cincinnati.IsDefined());
        Assert.True(NonContiguousUInt64Enum.Pittsburg.IsDefined());
        Assert.True(NonContiguousUInt64Enum.Milwaukee.IsDefined());
        Assert.False(((NonContiguousUInt64Enum)5).IsDefined());
        Assert.False(((NonContiguousUInt64Enum)50000000UL).IsDefined());

        Assert.True(NumericOperator.Equals.IsDefined());
        Assert.True(NumericOperator.NotEquals.IsDefined());
        Assert.True(NumericOperator.GreaterThan.IsDefined());
        Assert.True(NumericOperator.LessThan.IsDefined());
        Assert.True(NumericOperator.GreaterThanOrEquals.IsDefined());
        Assert.True(NumericOperator.NotLessThan.IsDefined());
        Assert.True(NumericOperator.LessThanOrEquals.IsDefined());
        Assert.True(NumericOperator.NotGreaterThan.IsDefined());
        Assert.True(NumericOperator.Between.IsDefined());
        Assert.True(NumericOperator.NotBetween.IsDefined());
        Assert.False((NumericOperator.Equals - 1).IsDefined());
        Assert.False((NumericOperator.NotBetween + 1).IsDefined());
    }

    [Fact]
    public void Validate()
    {
        NonContiguousEnum.Cat.Validate("paramName");
        NonContiguousEnum.Dog.Validate("paramName");
        NonContiguousEnum.Chimp.Validate("paramName");
        NonContiguousEnum.Elephant.Validate("paramName");
        NonContiguousEnum.Whale.Validate("paramName");
        NonContiguousEnum.Eagle.Validate("paramName");
        Assert.Throws<ArgumentException>(() => ((NonContiguousEnum)(-5)).Validate("paramName"));

        UInt64FlagEnum.Flies.Validate("paramName");
        UInt64FlagEnum.Hops.Validate("paramName");
        UInt64FlagEnum.Runs.Validate("paramName");
        UInt64FlagEnum.Slithers.Validate("paramName");
        UInt64FlagEnum.Stationary.Validate("paramName");
        UInt64FlagEnum.Swims.Validate("paramName");
        UInt64FlagEnum.Walks.Validate("paramName");
        (UInt64FlagEnum.Flies | UInt64FlagEnum.Hops).Validate("paramName");
        (UInt64FlagEnum.Flies | UInt64FlagEnum.Slithers).Validate("paramName");
        Assert.Throws<ArgumentException>(() => ((UInt64FlagEnum)8).Validate("paramName"));
        Assert.Throws<ArgumentException>(() => ((UInt64FlagEnum)8 | UInt64FlagEnum.Hops).Validate("paramName"));

        ContiguousUInt64Enum.A.Validate("paramName");
        ContiguousUInt64Enum.B.Validate("paramName");
        ContiguousUInt64Enum.C.Validate("paramName");
        ContiguousUInt64Enum.D.Validate("paramName");
        ContiguousUInt64Enum.E.Validate("paramName");
        ContiguousUInt64Enum.F.Validate("paramName");
        Assert.Throws<ArgumentException>(() => (ContiguousUInt64Enum.A - 1).Validate("paramName"));
        Assert.Throws<ArgumentException>(() => (ContiguousUInt64Enum.F + 1).Validate("paramName"));

        NonContiguousUInt64Enum.SaintLouis.Validate("paramName");
        NonContiguousUInt64Enum.Chicago.Validate("paramName");
        NonContiguousUInt64Enum.Cincinnati.Validate("paramName");
        NonContiguousUInt64Enum.Pittsburg.Validate("paramName");
        NonContiguousUInt64Enum.Milwaukee.Validate("paramName");
        Assert.Throws<ArgumentException>(() => ((NonContiguousUInt64Enum)5).Validate("paramName"));
        Assert.Throws<ArgumentException>(() => ((NonContiguousUInt64Enum)50000000UL).Validate("paramName"));

        NumericOperator.Equals.Validate("paramName");
        NumericOperator.NotEquals.Validate("paramName");
        NumericOperator.GreaterThan.Validate("paramName");
        NumericOperator.LessThan.Validate("paramName");
        NumericOperator.GreaterThanOrEquals.Validate("paramName");
        NumericOperator.NotLessThan.Validate("paramName");
        NumericOperator.LessThanOrEquals.Validate("paramName");
        NumericOperator.NotGreaterThan.Validate("paramName");
        NumericOperator.Between.Validate("paramName");
        NumericOperator.NotBetween.Validate("paramName");
        Assert.Throws<ArgumentException>(() => (NumericOperator.Equals - 1).Validate("paramName"));
        Assert.Throws<ArgumentException>(() => (NumericOperator.NotBetween + 1).Validate("paramName"));
    }

    [Fact]
    public void AsString()
    {
        for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
        {
            var value = (ColorFlagEnum)i;
            AssertTryFormat(value, value.ToString());
        }

        for (int i = short.MinValue; i <= short.MaxValue; ++i)
        {
            var value = (DateFilterOperator)i;
            AssertTryFormat(value, value.ToString());
        }

        AssertTryFormat(ToObject<CharEnum>('a'), "A");
        AssertTryFormat(ToObject<CharEnum>('b'), "B");
        AssertTryFormat(ToObject<CharEnum>('c'), "C");
        AssertTryFormat(ToObject<CharEnum>('d'), "d");

        static void AssertTryFormat<TEnum>(TEnum value, string expected)
            where TEnum : struct, Enum
        {
            Assert.Equal(expected, value.ToString());
            Assert.Equal(expected, value.AsString());
#if SPAN
            var dest = new char[expected.Length];
            Assert.True(value.TryFormat(dest, out var charsWritten));
            Assert.Equal(expected.Length, charsWritten);
            Assert.Equal(expected, new string(dest));

            dest = new char[expected.Length - 1];
            Assert.False(value.TryFormat(dest, out charsWritten));
            Assert.Equal(0, charsWritten);
            Assert.Equal(new char[dest.Length], dest);
#endif
        }
    }

    [Fact]
    public void AsString_ReturnsValidResult_WhenUsingValidEnumFormat()
    {
        AssertTryFormat(DisplayAttributeEnum.Up, "Arriba", EnumFormat.DisplayName);
        AssertTryFormat(DisplayAttributeEnum.Down, "Abajo", EnumFormat.DisplayName);
        AssertTryFormat(DisplayAttributeEnum.Left, "Izquierda", EnumFormat.DisplayName);
        AssertTryFormat(DisplayAttributeEnum.Right, "Derecho", EnumFormat.DisplayName);

        AssertTryFormat(CardinalDirection.North, "N", EnumFormat.DisplayDescription);
        AssertTryFormat(CardinalDirection.South, "S", EnumFormat.DisplayDescription);
        AssertTryFormat(CardinalDirection.East, "E", EnumFormat.DisplayDescription);
        AssertTryFormat(CardinalDirection.West, "W", EnumFormat.DisplayDescription);

        AssertTryFormat(ToObject<CharEnum>('a'), "a", EnumFormat.UnderlyingValue);
        AssertTryFormat(ToObject<CharEnum>('a'), ((ushort)'a').ToString(), EnumFormat.DecimalValue);
        AssertTryFormat(ToObject<CharEnum>('a'), ((ushort)'a').ToString("X4"), EnumFormat.HexadecimalValue);

        static void AssertTryFormat<TEnum>(TEnum value, string expected, EnumFormat format)
            where TEnum : struct, Enum
        {
            Assert.Equal(expected, value.AsString(format));
#if SPAN
            var dest = new char[expected.Length];
            Assert.True(value.TryFormat(dest, out var charsWritten, format));
            Assert.Equal(expected.Length, charsWritten);
            Assert.Equal(expected, new string(dest));

            dest = new char[expected.Length - 1];
            Assert.False(value.TryFormat(dest, out charsWritten, format));
            Assert.Equal(0, charsWritten);
            Assert.Equal(new char[dest.Length], dest);
#endif
        }
    }

    [Fact]
    public void AsString_ReturnsValidResult_WhenUsingValidFormat()
    {
        string[] validFormats = [null, string.Empty, "G", "g", "F", "f", "D", "d", "X", "x"];

        foreach (var format in validFormats)
        {
            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
            {
                var value = (ColorFlagEnum)i;
                AssertTryFormat(value, value.ToString(format), format);
            }

            for (int i = short.MinValue; i <= (int)DateFilterOperator.NextNumberOfBusinessDays; ++i)
            {
                var value = (DateFilterOperator)i;
                AssertTryFormat(value, value.ToString(format), format);
            }
        }

        static void AssertTryFormat<TEnum>(TEnum value, string expected, string format)
            where TEnum : struct, Enum
        {
            Assert.Equal(expected, value.AsString(format));
#if SPAN
            var dest = new char[expected.Length];
            Assert.True(value.TryFormat(dest, out var charsWritten, format));
            Assert.Equal(expected.Length, charsWritten);
            Assert.Equal(expected, new string(dest));

            dest = new char[expected.Length - 1];
            Assert.False(value.TryFormat(dest, out charsWritten, format));
            Assert.Equal(0, charsWritten);
            Assert.Equal(new char[dest.Length], dest);
#endif
        }
    }

    [Fact]
    public void AsString_ThrowsFormatException_WhenUsingInvalidFormat()
    {
        Assert.Throws<FormatException>(() => ColorFlagEnum.Blue.AsString("a"));
#if SPAN
        Assert.Throws<FormatException>(() => ColorFlagEnum.Blue.TryFormat(new char[20], out _, "a"));
#endif
    }

    [Fact]
    public void TryFormatLongFlagName()
    {
        AssertTryFormat((LongFlagNamesEnum)2047);

        static void AssertTryFormat<TEnum>(TEnum value)
            where TEnum : struct, Enum
        {
            var expected = value.ToString();
            Assert.True(expected.Length > 256);
            Assert.Equal(expected, value.AsString());
#if SPAN
            var dest = new char[expected.Length];
            Assert.True(value.TryFormat(dest, out var charsWritten));
            Assert.Equal(expected.Length, charsWritten);
            Assert.Equal(expected, new string(dest));

            dest = new char[expected.Length - 1];
            Assert.False(value.TryFormat(dest, out charsWritten));
            Assert.Equal(0, charsWritten);
            Assert.Equal(new char[dest.Length], dest);
#endif
        }
    }

    [Fact]
    public void Format_ReturnsValidResult_WhenUsingValidFormat()
    {
        string[] validFormats = ["G", "g", "F", "f", "D", "d", "X", "x"];

        foreach (var format in validFormats)
        {
            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
            {
                var value = (ColorFlagEnum)i;
                Assert.Equal(Enum.Format(typeof(ColorFlagEnum), value, format), Format(value, format));
            }

            for (int i = short.MinValue; i <= (int)DateFilterOperator.NextNumberOfBusinessDays; ++i)
            {
                var value = (DateFilterOperator)i;
                Assert.Equal(Enum.Format(typeof(DateFilterOperator), value, format), Format(value, format));
            }
        }
    }

    [Fact]
    public void Format_ThrowsArgumentNullException_WhenUsingNullFormat()
    {
        Assert.Throws<ArgumentNullException>(() => Format(ColorFlagEnum.Blue, (string)null));
    }

    [Fact]
    public void Format_ThrowsFormatException_WhenUsingEmptyStringFormat()
    {
        Assert.Throws<FormatException>(() => Format(ColorFlagEnum.Blue, string.Empty));
    }

    [Fact]
    public void Format_ThrowsFormatException_WhenUsingInvalidStringFormat()
    {
        Assert.Throws<FormatException>(() => Format(ColorFlagEnum.Blue, "a"));
    }

    [Fact]
    public void GetUnderlyingValue_ReturnsExpected_OnAny()
    {
        Assert.Equal(2, GetUnderlyingValue(NumericOperator.GreaterThan));
        Assert.Equal('b', GetUnderlyingValue(ToObject<CharEnum>('b')));
    }
    #endregion

    #region Defined Values Main Methods
    [Fact]
    public void GetName()
    {
        for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
        {
            var value = (ColorFlagEnum)i;
            Assert.Equal(Enum.GetName(typeof(ColorFlagEnum), value), value.GetName());
        }

        for (int i = short.MinValue; i <= short.MaxValue; ++i)
        {
            var value = (DateFilterOperator)i;
            Assert.Equal(Enum.GetName(typeof(DateFilterOperator), value), value.GetName());
        }

        // Check for primary duplicates
        Assert.Equal("GreaterThanOrEquals", NumericOperator.GreaterThanOrEquals.GetName());
        Assert.Equal("GreaterThanOrEquals", NumericOperator.NotLessThan.GetName());
        Assert.Equal("NotGreaterThan", NumericOperator.LessThanOrEquals.GetName());
        Assert.Equal("NotGreaterThan", NumericOperator.NotGreaterThan.GetName());

        Assert.Equal("A", ToObject<CharEnum>('a').GetName());
        Assert.Null(ToObject<CharEnum>('d').GetName());

        Assert.Equal("A", Enum.GetName(typeof(CharEnum), ToObject<CharEnum>('a')));
        Assert.Null(Enum.GetName(typeof(CharEnum), ToObject<CharEnum>('d')));
    }
    #endregion

    #region Attributes
    [Fact]
    public void HasAttribute()
    {
        Assert.True(EnumMemberAttributeEnum.A.GetMember().Attributes.Has<EnumMemberAttribute>());
        Assert.False(ColorFlagEnum.Blue.GetMember().Attributes.Has<DescriptionAttribute>());
        Assert.True(ColorFlagEnum.UltraViolet.GetMember().Attributes.Has<DescriptionAttribute>());
    }

    [Fact]
    public void GetAttribute()
    {
        Assert.Equal("aye", EnumMemberAttributeEnum.A.GetMember().Attributes.Get<EnumMemberAttribute>().Value);
        Assert.Null(ColorFlagEnum.Blue.GetMember().Attributes.Get<DescriptionAttribute>());
        Assert.Equal("Ultra-Violet", ColorFlagEnum.UltraViolet.GetMember().Attributes.Get<DescriptionAttribute>().Description);
    }

    [Fact]
    public void GetAllAttributes()
    {
        Assert.Equivalent(Array.Empty<OptionAttribute>(), MultipleAttributeEnum.None.GetMember().Attributes.GetAll<OptionAttribute>());
        Assert.Equivalent(new[] { new OptionAttribute("Mono") }, MultipleAttributeEnum.Single.GetMember().Attributes.GetAll<OptionAttribute>());
        Assert.Equivalent(new[] { new OptionAttribute("Poly"), new OptionAttribute("Plural") }, MultipleAttributeEnum.Multi.GetMember().Attributes.GetAll<OptionAttribute>());
    }

    [Fact]
    public void Attributes()
    {
        Assert.Equivalent(Array.Empty<Attribute>(), MultipleAttributeEnum.None.GetMember().Attributes);
        Assert.Equivalent(new Attribute[] { new OptionAttribute("Mono"), new DescriptionAttribute("One") }, MultipleAttributeEnum.Single.GetMember().Attributes);
        Assert.Equivalent(new Attribute[] { new DescriptionAttribute("Many"), new OptionAttribute("Poly"), new OptionAttribute("Plural") }, MultipleAttributeEnum.Multi.GetMember().Attributes);

        Assert.Empty(MultipleAttributeEnum.None.GetAttributes());
        Assert.Equal(2, MultipleAttributeEnum.Single.GetAttributes().Count);
        Assert.Equal(3, MultipleAttributeEnum.Multi.GetAttributes().Count);
    }
    #endregion

    #region Parsing
    [Fact]
    public void Parse()
    {
        Assert.Equal(ToObject<CharEnum>('a'), Parse<CharEnum>("A"));
        Assert.Equal(ToObject<CharEnum>('a'), Parse<CharEnum>("a"));
        Assert.Equal(ToObject<CharEnum>('a'), Parse<CharEnum>(((ushort)'a').ToString(), ignoreCase: false, EnumFormat.DecimalValue));
        Assert.Equal(ToObject<CharEnum>('d'), Parse<CharEnum>("d"));
        Assert.Equal(ToObject<CharEnum>('d'), Parse<CharEnum>(((ushort)'d').ToString(), ignoreCase: false, EnumFormat.DecimalValue));
    }
    #endregion
}
