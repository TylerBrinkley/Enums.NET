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
using NUnit.Framework;
using static EnumsNET.Enums;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace EnumsNET.Tests
{
    [TestFixture]
    public class EnumsTest
    {
        #region Type Methods
        [Test]
        public void GetUnderlyingType()
        {
            Assert.AreEqual(typeof(sbyte), GetUnderlyingType<SByteEnum>());
            Assert.AreEqual(typeof(byte), GetUnderlyingType<ByteEnum>());
            Assert.AreEqual(typeof(short), GetUnderlyingType<Int16Enum>());
            Assert.AreEqual(typeof(ushort), GetUnderlyingType<UInt16Enum>());
            Assert.AreEqual(typeof(int), GetUnderlyingType<Int32Enum>());
            Assert.AreEqual(typeof(uint), GetUnderlyingType<UInt32Enum>());
            Assert.AreEqual(typeof(long), GetUnderlyingType<Int64Enum>());
            Assert.AreEqual(typeof(ulong), GetUnderlyingType<UInt64Enum>());
            Assert.AreEqual(typeof(bool), GetUnderlyingType<BooleanEnum>());
            Assert.AreEqual(typeof(char), GetUnderlyingType<CharEnum>());
        }

#if ICONVERTIBLE
        [Test]
        public void GetTypeCode()
        {
            Assert.AreEqual(TypeCode.SByte, GetTypeCode<SByteEnum>());
            Assert.AreEqual(TypeCode.Byte, GetTypeCode<ByteEnum>());
            Assert.AreEqual(TypeCode.Int16, GetTypeCode<Int16Enum>());
            Assert.AreEqual(TypeCode.UInt16, GetTypeCode<UInt16Enum>());
            Assert.AreEqual(TypeCode.Int32, GetTypeCode<Int32Enum>());
            Assert.AreEqual(TypeCode.UInt32, GetTypeCode<UInt32Enum>());
            Assert.AreEqual(TypeCode.Int64, GetTypeCode<Int64Enum>());
            Assert.AreEqual(TypeCode.UInt64, GetTypeCode<UInt64Enum>());
            Assert.AreEqual(TypeCode.Boolean, GetTypeCode<BooleanEnum>());
            Assert.AreEqual(TypeCode.Char, GetTypeCode<CharEnum>());
        }
#endif

        [Test]
        public void GetMemberCount()
        {
            Assert.AreEqual(0, GetMemberCount<ByteEnum>());
            Assert.AreEqual(38, GetMemberCount<DateFilterOperator>());
            Assert.AreEqual(6, GetMemberCount<ColorFlagEnum>());
            Assert.AreEqual(10, GetMemberCount<NumericOperator>());
            Assert.AreEqual(1, GetMemberCount<BooleanEnum>());
            Assert.AreEqual(3, GetMemberCount<CharEnum>());

            Assert.AreEqual(0, GetMemberCount<ByteEnum>(EnumMemberSelection.Distinct));
            Assert.AreEqual(38, GetMemberCount<DateFilterOperator>(EnumMemberSelection.Distinct));
            Assert.AreEqual(6, GetMemberCount<ColorFlagEnum>(EnumMemberSelection.Distinct));
            Assert.AreEqual(8, GetMemberCount<NumericOperator>(EnumMemberSelection.Distinct)); // Has 2 duplicates
            Assert.AreEqual(1, GetMemberCount<BooleanEnum>(EnumMemberSelection.Distinct));
            Assert.AreEqual(3, GetMemberCount<CharEnum>(EnumMemberSelection.Distinct));

            Assert.AreEqual(4, GetMemberCount<ColorFlagEnum>(EnumMemberSelection.Flags));
        }

        [Test]
        public void GetMembers()
        {
            Assert.AreEqual(0, GetMembers<ByteEnum>().Count);
            Assert.AreEqual(38, GetMembers<DateFilterOperator>().Count);
            Assert.AreEqual(6, GetMembers<ColorFlagEnum>().Count);
            Assert.AreEqual(10, GetMembers<NumericOperator>().Count);
            Assert.AreEqual(1, GetMembers<BooleanEnum>().Count);
            Assert.AreEqual(3, GetMembers<CharEnum>().Count);

            Assert.AreEqual(0, GetMembers<ByteEnum>(EnumMemberSelection.Distinct).Count);
            Assert.AreEqual(38, GetMembers<DateFilterOperator>(EnumMemberSelection.Distinct).Count);
            Assert.AreEqual(6, GetMembers<ColorFlagEnum>(EnumMemberSelection.Distinct).Count);
            Assert.AreEqual(8, GetMembers<NumericOperator>(EnumMemberSelection.Distinct).Count); // Has 2 duplicates
            Assert.AreEqual(1, GetMembers<BooleanEnum>(EnumMemberSelection.Distinct).Count);
            Assert.AreEqual(3, GetMembers<CharEnum>(EnumMemberSelection.Distinct).Count);

            Assert.AreEqual(4, GetMembers<ColorFlagEnum>(EnumMemberSelection.Flags).Count);

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

        public static void AssertEnumMemberIsCorrect<TEnum>(EnumMember<TEnum> member, TEnum value, string name, params Attribute[] attributes)
        {
            Assert.IsNotNull(member);
            Assert.AreEqual(value, member.Value);
            Assert.AreEqual(name, member.Name);
            CollectionAssert.AreEquivalent(attributes, member.Attributes);
        }

        [Test]
        public void GetNames()
        {
            CollectionAssert.AreEqual(new[] { "Black", "Red", "Green", "Blue", "UltraViolet", "All" }, GetNames<ColorFlagEnum>());
            CollectionAssert.AreEqual(Enum.GetNames(typeof(DateFilterOperator)), GetNames<DateFilterOperator>());
            CollectionAssert.AreEqual(new string[0], GetNames<ByteEnum>());
            CollectionAssert.AreEqual(new[] { "No" }, GetNames<BooleanEnum>());
            CollectionAssert.AreEqual(new[] { "A", "B", "C" }, GetNames<CharEnum>());
        }

        [Test]
        public void GetValues()
        {
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Black, ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet, ColorFlagEnum.All }, GetValues<ColorFlagEnum>());
            CollectionAssert.AreEqual((DateFilterOperator[])Enum.GetValues(typeof(DateFilterOperator)), GetValues<DateFilterOperator>());
            CollectionAssert.AreEqual(new ByteEnum[0], GetValues<ByteEnum>());
            CollectionAssert.AreEqual(new List<BooleanEnum> { ToObject<BooleanEnum>(0) }, GetValues<BooleanEnum>());
            CollectionAssert.AreEqual(new List<CharEnum> { ToObject<CharEnum>('a'), ToObject<CharEnum>('b'), ToObject<CharEnum>('c') }, GetValues<CharEnum>());

            // Duplicate order check
            var numericFilterOperators = GetValues<NumericOperator>();
            for (var i = 1; i < numericFilterOperators.Count; ++i)
            {
                Assert.IsTrue(numericFilterOperators[i - 1] <= numericFilterOperators[i]);
            }

#if DISPLAY_ATTRIBUTE
            CollectionAssert.AreEqual(new[] { DisplayAttributeEnum.Up, DisplayAttributeEnum.Down, DisplayAttributeEnum.Left, DisplayAttributeEnum.Right }, GetValues<DisplayAttributeEnum>());
            CollectionAssert.AreEqual(new[] { DisplayAttributeEnum.Left, DisplayAttributeEnum.Up, DisplayAttributeEnum.Down, DisplayAttributeEnum.Right }, GetValues<DisplayAttributeEnum>(EnumMemberSelection.DisplayOrder));
#endif
        }

        [Test]
        public void GetValues_ExcludeDuplicates()
        {
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Black, ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet, ColorFlagEnum.All }, GetValues<ColorFlagEnum>(EnumMemberSelection.Distinct));
            CollectionAssert.AreEqual((DateFilterOperator[])Enum.GetValues(typeof(DateFilterOperator)), GetValues<DateFilterOperator>(EnumMemberSelection.Distinct));
            CollectionAssert.AreEqual(new ByteEnum[0], GetValues<ByteEnum>(EnumMemberSelection.Distinct));
            CollectionAssert.AreEqual(new[] { NumericOperator.Equals, NumericOperator.NotEquals, NumericOperator.GreaterThan, NumericOperator.LessThan, NumericOperator.GreaterThanOrEquals, NumericOperator.NotGreaterThan, NumericOperator.Between, NumericOperator.NotBetween }, GetValues<NumericOperator>(EnumMemberSelection.Distinct));
        }
        #endregion

        #region ToObject
        [Test]
        public void ToObject_ReturnsValidValue_WhenUsingValidNumber()
        {
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>((sbyte)1));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>((byte)1));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>((short)1));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>((ushort)1));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>(1));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>(1U));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>(1L));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>(1UL));

            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>((sbyte)1));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>((byte)1));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>((short)1));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>((ushort)1));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>(1));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>(1U));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>(1L));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>(1UL));

            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>((sbyte)1));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>((byte)1));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>((short)1));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>((ushort)1));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>(1));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>(1U));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>(1L));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>(1UL));

            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>((sbyte)1));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>((byte)1));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>((short)1));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>((ushort)1));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>(1));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>(1U));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>(1L));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>(1UL));

            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>((sbyte)1));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>((byte)1));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>((short)1));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>((ushort)1));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>(1));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>(1U));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>(1L));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>(1UL));

            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>((sbyte)1));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>((byte)1));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>((short)1));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>((ushort)1));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>(1));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>(1U));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>(1L));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>(1UL));

            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>((sbyte)1));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>((byte)1));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>((short)1));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>((ushort)1));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>(1));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>(1U));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>(1L));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>(1UL));

            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>((sbyte)1));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>((byte)1));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>((short)1));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>((ushort)1));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>(1));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>(1U));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>(1L));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>(1UL));

            Assert.AreEqual(ToObject<BooleanEnum>(1), ToObject<BooleanEnum>((sbyte)1));
            Assert.AreEqual(ToObject<BooleanEnum>(1), ToObject<BooleanEnum>((byte)1));
            Assert.AreEqual(ToObject<BooleanEnum>(1), ToObject<BooleanEnum>((short)1));
            Assert.AreEqual(ToObject<BooleanEnum>(1), ToObject<BooleanEnum>((ushort)1));
            Assert.AreEqual(ToObject<BooleanEnum>(1), ToObject<BooleanEnum>(1));
            Assert.AreEqual(ToObject<BooleanEnum>(1), ToObject<BooleanEnum>(1U));
            Assert.AreEqual(ToObject<BooleanEnum>(1), ToObject<BooleanEnum>(1L));
            Assert.AreEqual(ToObject<BooleanEnum>(1), ToObject<BooleanEnum>(1UL));

            Assert.AreEqual(ToObject<CharEnum>(1), ToObject<CharEnum>((sbyte)1));
            Assert.AreEqual(ToObject<CharEnum>(1), ToObject<CharEnum>((byte)1));
            Assert.AreEqual(ToObject<CharEnum>(1), ToObject<CharEnum>((short)1));
            Assert.AreEqual(ToObject<CharEnum>(1), ToObject<CharEnum>((ushort)1));
            Assert.AreEqual(ToObject<CharEnum>(1), ToObject<CharEnum>(1));
            Assert.AreEqual(ToObject<CharEnum>(1), ToObject<CharEnum>(1U));
            Assert.AreEqual(ToObject<CharEnum>(1), ToObject<CharEnum>(1L));
            Assert.AreEqual(ToObject<CharEnum>(1), ToObject<CharEnum>(1UL));
        }

        [Test]
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

        [Test]
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
            Assert.Throws<ArgumentException>(() => ToObject<BooleanEnum>(1, EnumValidation.Default));
            Assert.Throws<ArgumentException>(() => ToObject<CharEnum>(ushort.MinValue, EnumValidation.Default));
        }

        [Test]
        public void TryToObject_ReturnsTrueAndValidValue_WhenUsingValidNumber()
        {
            SByteEnum sbyteResult;
            var sbyteValue = (SByteEnum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out sbyteResult));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject((byte)1, out sbyteResult));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject((short)1, out sbyteResult));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject((ushort)1, out sbyteResult));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject(1, out sbyteResult));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject(1U, out sbyteResult));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject(1L, out sbyteResult));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject(1UL, out sbyteResult));
            Assert.AreEqual(sbyteValue, sbyteResult);

            ByteEnum byteResult;
            var byteValue = (ByteEnum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out byteResult));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject((byte)1, out byteResult));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject((short)1, out byteResult));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject((ushort)1, out byteResult));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject(1, out byteResult));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject(1U, out byteResult));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject(1L, out byteResult));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject(1UL, out byteResult));
            Assert.AreEqual(byteValue, byteResult);

            Int16Enum int16Result;
            var int16Value = (Int16Enum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out int16Result));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject((byte)1, out int16Result));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject((short)1, out int16Result));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject((ushort)1, out int16Result));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject(1, out int16Result));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject(1U, out int16Result));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject(1L, out int16Result));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject(1UL, out int16Result));
            Assert.AreEqual(int16Value, int16Result);

            UInt16Enum uint16Result;
            var uint16Value = (UInt16Enum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out uint16Result));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject((byte)1, out uint16Result));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject((short)1, out uint16Result));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject((ushort)1, out uint16Result));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject(1, out uint16Result));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject(1U, out uint16Result));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject(1L, out uint16Result));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject(1UL, out uint16Result));
            Assert.AreEqual(uint16Value, uint16Result);

            Int32Enum int32Result;
            var int32Value = (Int32Enum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out int32Result));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject((byte)1, out int32Result));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject((short)1, out int32Result));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject((ushort)1, out int32Result));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject(1, out int32Result));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject(1U, out int32Result));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject(1L, out int32Result));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject(1UL, out int32Result));
            Assert.AreEqual(int32Value, int32Result);

            UInt32Enum uint32Result;
            var uint32Value = (UInt32Enum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out uint32Result));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject((byte)1, out uint32Result));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject((short)1, out uint32Result));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject((ushort)1, out uint32Result));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject(1, out uint32Result));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject(1U, out uint32Result));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject(1L, out uint32Result));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject(1UL, out uint32Result));
            Assert.AreEqual(uint32Value, uint32Result);

            Int64Enum int64Result;
            var int64Value = (Int64Enum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out int64Result));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject((byte)1, out int64Result));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject((short)1, out int64Result));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject((ushort)1, out int64Result));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject(1, out int64Result));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject(1U, out int64Result));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject(1L, out int64Result));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject(1UL, out int64Result));
            Assert.AreEqual(int64Value, int64Result);

            UInt64Enum uint64Result;
            var uint64Value = (UInt64Enum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out uint64Result));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject((byte)1, out uint64Result));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject((short)1, out uint64Result));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject((ushort)1, out uint64Result));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject(1, out uint64Result));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject(1U, out uint64Result));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject(1L, out uint64Result));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject(1UL, out uint64Result));
            Assert.AreEqual(uint64Value, uint64Result);

            BooleanEnum booleanResult;
            var booleanValue = ToObject<BooleanEnum>(1);
            Assert.IsTrue(TryToObject((sbyte)1, out booleanResult));
            Assert.AreEqual(booleanValue, booleanResult);
            Assert.IsTrue(TryToObject((byte)1, out booleanResult));
            Assert.AreEqual(booleanValue, booleanResult);
            Assert.IsTrue(TryToObject((short)1, out booleanResult));
            Assert.AreEqual(booleanValue, booleanResult);
            Assert.IsTrue(TryToObject((ushort)1, out booleanResult));
            Assert.AreEqual(booleanValue, booleanResult);
            Assert.IsTrue(TryToObject(1, out booleanResult));
            Assert.AreEqual(booleanValue, booleanResult);
            Assert.IsTrue(TryToObject(1U, out booleanResult));
            Assert.AreEqual(booleanValue, booleanResult);
            Assert.IsTrue(TryToObject(1L, out booleanResult));
            Assert.AreEqual(booleanValue, booleanResult);
            Assert.IsTrue(TryToObject(1UL, out booleanResult));
            Assert.AreEqual(booleanValue, booleanResult);

            CharEnum charResult;
            var charValue = ToObject<CharEnum>(1);
            Assert.IsTrue(TryToObject((sbyte)1, out charResult));
            Assert.AreEqual(charValue, charResult);
            Assert.IsTrue(TryToObject((byte)1, out charResult));
            Assert.AreEqual(charValue, charResult);
            Assert.IsTrue(TryToObject((short)1, out charResult));
            Assert.AreEqual(charValue, charResult);
            Assert.IsTrue(TryToObject((ushort)1, out charResult));
            Assert.AreEqual(charValue, charResult);
            Assert.IsTrue(TryToObject(1, out charResult));
            Assert.AreEqual(charValue, charResult);
            Assert.IsTrue(TryToObject(1U, out charResult));
            Assert.AreEqual(charValue, charResult);
            Assert.IsTrue(TryToObject(1L, out charResult));
            Assert.AreEqual(charValue, charResult);
            Assert.IsTrue(TryToObject(1UL, out charResult));
            Assert.AreEqual(charValue, charResult);
        }

        [Test]
        public void TryToObject_ReturnsFalse_WhenUsingValueInRangeButNotValid()
        {
            ColorFlagEnum result;
            Assert.IsFalse(TryToObject((sbyte)16, out result, EnumValidation.Default));
            Assert.IsFalse(TryToObject((byte)16, out result, EnumValidation.Default));
            Assert.IsFalse(TryToObject((short)16, out result, EnumValidation.Default));
            Assert.IsFalse(TryToObject((ushort)16, out result, EnumValidation.Default));
            Assert.IsFalse(TryToObject(16, out result, EnumValidation.Default));
            Assert.IsFalse(TryToObject(16U, out result, EnumValidation.Default));
            Assert.IsFalse(TryToObject(16L, out result, EnumValidation.Default));
            Assert.IsFalse(TryToObject(16UL, out result, EnumValidation.Default));
        }

        [Test]
        public void TryToObject_ReturnsTrueAndValidValue_WhenUsingValueInRangeButNotValidWithoutValidation()
        {
            ColorFlagEnum result;
            var value = (ColorFlagEnum)16;
            Assert.IsTrue(TryToObject((sbyte)16, out result));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject((byte)16, out result));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject((short)16, out result));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject((ushort)16, out result));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject(16, out result));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject(16U, out result));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject(16L, out result));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject(16UL, out result));
            Assert.AreEqual(value, result);
        }

        [Test]
        public void TryToObject_ReturnsFalse_WhenUsingValueOutOfRange()
        {
            ColorFlagEnum result;
            Assert.IsFalse(TryToObject((byte)128, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject((short)128, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject((ushort)128, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject(128, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject(128U, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject(128L, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject(128UL, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);

            Assert.IsFalse(TryToObject((byte)128, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject((short)128, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject((ushort)128, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject(128, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject(128U, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject(128L, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject(128UL, out result));
            Assert.AreEqual(default(ColorFlagEnum), result);
        }
        #endregion

        #region All Values Main Methods
        [Test]
        public void IsValid_ReturnsSameResultAsIsValidFlagCombination_WhenUsingFlagEnum()
        {
            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
            {
                var value = (ColorFlagEnum)i;
                Assert.AreEqual(FlagEnums.IsValidFlagCombination(value), value.IsValid());
            }
        }

        [Test]
        public void IsValid()
        {
            for (int i = short.MinValue; i <= short.MaxValue; ++i)
            {
                var value = (DateFilterOperator)i;
                Assert.AreEqual(Enum.IsDefined(typeof(DateFilterOperator), value), value.IsValid());
            }

            Assert.IsTrue(NonContiguousEnum.Cat.IsValid());
            Assert.IsTrue(NonContiguousEnum.Dog.IsValid());
            Assert.IsTrue(NonContiguousEnum.Chimp.IsValid());
            Assert.IsTrue(NonContiguousEnum.Elephant.IsValid());
            Assert.IsTrue(NonContiguousEnum.Whale.IsValid());
            Assert.IsTrue(NonContiguousEnum.Eagle.IsValid());
            Assert.IsFalse(((NonContiguousEnum)(-5)).IsValid());

            Assert.IsTrue(UInt64FlagEnum.Flies.IsValid());
            Assert.IsTrue(UInt64FlagEnum.Hops.IsValid());
            Assert.IsTrue(UInt64FlagEnum.Runs.IsValid());
            Assert.IsTrue(UInt64FlagEnum.Slithers.IsValid());
            Assert.IsTrue(UInt64FlagEnum.Stationary.IsValid());
            Assert.IsTrue(UInt64FlagEnum.Swims.IsValid());
            Assert.IsTrue(UInt64FlagEnum.Walks.IsValid());
            Assert.IsTrue((UInt64FlagEnum.Flies | UInt64FlagEnum.Hops).IsValid());
            Assert.IsTrue((UInt64FlagEnum.Flies | UInt64FlagEnum.Slithers).IsValid());
            Assert.IsFalse(((UInt64FlagEnum)8).IsValid());
            Assert.IsFalse(((UInt64FlagEnum)8 | UInt64FlagEnum.Hops).IsValid());

            Assert.IsTrue(ContiguousUInt64Enum.A.IsValid());
            Assert.IsTrue(ContiguousUInt64Enum.B.IsValid());
            Assert.IsTrue(ContiguousUInt64Enum.C.IsValid());
            Assert.IsTrue(ContiguousUInt64Enum.D.IsValid());
            Assert.IsTrue(ContiguousUInt64Enum.E.IsValid());
            Assert.IsTrue(ContiguousUInt64Enum.F.IsValid());
            Assert.IsFalse((ContiguousUInt64Enum.A - 1).IsValid());
            Assert.IsFalse((ContiguousUInt64Enum.F + 1).IsValid());

            Assert.IsTrue(NonContiguousUInt64Enum.SaintLouis.IsValid());
            Assert.IsTrue(NonContiguousUInt64Enum.Chicago.IsValid());
            Assert.IsTrue(NonContiguousUInt64Enum.Cincinnati.IsValid());
            Assert.IsTrue(NonContiguousUInt64Enum.Pittsburg.IsValid());
            Assert.IsTrue(NonContiguousUInt64Enum.Milwaukee.IsValid());
            Assert.IsFalse(((NonContiguousUInt64Enum)5).IsValid());
            Assert.IsFalse(((NonContiguousUInt64Enum)50000000UL).IsValid());

            Assert.IsTrue(NumericOperator.Equals.IsValid());
            Assert.IsTrue(NumericOperator.NotEquals.IsValid());
            Assert.IsTrue(NumericOperator.GreaterThan.IsValid());
            Assert.IsTrue(NumericOperator.LessThan.IsValid());
            Assert.IsTrue(NumericOperator.GreaterThanOrEquals.IsValid());
            Assert.IsTrue(NumericOperator.NotLessThan.IsValid());
            Assert.IsTrue(NumericOperator.LessThanOrEquals.IsValid());
            Assert.IsTrue(NumericOperator.NotGreaterThan.IsValid());
            Assert.IsTrue(NumericOperator.Between.IsValid());
            Assert.IsTrue(NumericOperator.NotBetween.IsValid());
            Assert.IsFalse((NumericOperator.Equals - 1).IsValid());
            Assert.IsFalse((NumericOperator.NotBetween + 1).IsValid());
        }

        [Test]
        public void IsValid_UsingValidator()
        {
            Assert.IsTrue(TypeNameHandling.None.IsValid());
            Assert.IsTrue(TypeNameHandling.Objects.IsValid());
            Assert.IsTrue(TypeNameHandling.Arrays.IsValid());
            Assert.IsTrue((TypeNameHandling.Objects | TypeNameHandling.Arrays).IsValid());
            Assert.IsTrue(TypeNameHandling.Auto.IsValid());
            Assert.IsFalse((TypeNameHandling.Auto | TypeNameHandling.Objects).IsValid());
        }

        [Test]
        public void IsDefined()
        {
            for (int i = byte.MinValue; i <= byte.MaxValue; ++i)
            {
                var value = (ColorFlagEnum)i;
                Assert.AreEqual(Enum.IsDefined(typeof(ColorFlagEnum), value), value.IsDefined());
            }

            for (int i = short.MinValue; i <= short.MaxValue; ++i)
            {
                var value = (DateFilterOperator)i;
                Assert.AreEqual(Enum.IsDefined(typeof(DateFilterOperator), value), value.IsDefined());
            }

            Assert.IsTrue(NonContiguousEnum.Cat.IsDefined());
            Assert.IsTrue(NonContiguousEnum.Dog.IsDefined());
            Assert.IsTrue(NonContiguousEnum.Chimp.IsDefined());
            Assert.IsTrue(NonContiguousEnum.Elephant.IsDefined());
            Assert.IsTrue(NonContiguousEnum.Whale.IsDefined());
            Assert.IsTrue(NonContiguousEnum.Eagle.IsDefined());
            Assert.IsFalse(((NonContiguousEnum)(-5)).IsDefined());

            Assert.IsTrue(UInt64FlagEnum.Flies.IsDefined());
            Assert.IsTrue(UInt64FlagEnum.Hops.IsDefined());
            Assert.IsTrue(UInt64FlagEnum.Runs.IsDefined());
            Assert.IsTrue(UInt64FlagEnum.Slithers.IsDefined());
            Assert.IsTrue(UInt64FlagEnum.Stationary.IsDefined());
            Assert.IsTrue(UInt64FlagEnum.Swims.IsDefined());
            Assert.IsTrue(UInt64FlagEnum.Walks.IsDefined());
            Assert.IsFalse((UInt64FlagEnum.Flies | UInt64FlagEnum.Hops).IsDefined());
            Assert.IsFalse((UInt64FlagEnum.Flies | UInt64FlagEnum.Slithers).IsDefined());
            Assert.IsFalse(((UInt64FlagEnum)8).IsDefined());
            Assert.IsFalse(((UInt64FlagEnum)8 | UInt64FlagEnum.Hops).IsDefined());

            Assert.IsTrue(ContiguousUInt64Enum.A.IsDefined());
            Assert.IsTrue(ContiguousUInt64Enum.B.IsDefined());
            Assert.IsTrue(ContiguousUInt64Enum.C.IsDefined());
            Assert.IsTrue(ContiguousUInt64Enum.D.IsDefined());
            Assert.IsTrue(ContiguousUInt64Enum.E.IsDefined());
            Assert.IsTrue(ContiguousUInt64Enum.F.IsDefined());
            Assert.IsFalse((ContiguousUInt64Enum.A - 1).IsDefined());
            Assert.IsFalse((ContiguousUInt64Enum.F + 1).IsDefined());

            Assert.IsTrue(NonContiguousUInt64Enum.SaintLouis.IsDefined());
            Assert.IsTrue(NonContiguousUInt64Enum.Chicago.IsDefined());
            Assert.IsTrue(NonContiguousUInt64Enum.Cincinnati.IsDefined());
            Assert.IsTrue(NonContiguousUInt64Enum.Pittsburg.IsDefined());
            Assert.IsTrue(NonContiguousUInt64Enum.Milwaukee.IsDefined());
            Assert.IsFalse(((NonContiguousUInt64Enum)5).IsDefined());
            Assert.IsFalse(((NonContiguousUInt64Enum)50000000UL).IsDefined());

            Assert.IsTrue(NumericOperator.Equals.IsDefined());
            Assert.IsTrue(NumericOperator.NotEquals.IsDefined());
            Assert.IsTrue(NumericOperator.GreaterThan.IsDefined());
            Assert.IsTrue(NumericOperator.LessThan.IsDefined());
            Assert.IsTrue(NumericOperator.GreaterThanOrEquals.IsDefined());
            Assert.IsTrue(NumericOperator.NotLessThan.IsDefined());
            Assert.IsTrue(NumericOperator.LessThanOrEquals.IsDefined());
            Assert.IsTrue(NumericOperator.NotGreaterThan.IsDefined());
            Assert.IsTrue(NumericOperator.Between.IsDefined());
            Assert.IsTrue(NumericOperator.NotBetween.IsDefined());
            Assert.IsFalse((NumericOperator.Equals - 1).IsDefined());
            Assert.IsFalse((NumericOperator.NotBetween + 1).IsDefined());
        }

        [Test]
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

        [Test]
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

            AssertTryFormat(ToObject<BooleanEnum>(0), "No");
            AssertTryFormat(ToObject<BooleanEnum>(1), "True"); // true.ToString()
            AssertTryFormat(ToObject<CharEnum>('a'), "A");
            AssertTryFormat(ToObject<CharEnum>('b'), "B");
            AssertTryFormat(ToObject<CharEnum>('c'), "C");
            AssertTryFormat(ToObject<CharEnum>('d'), "d");

            void AssertTryFormat<TEnum>(TEnum value, string expected)
                where TEnum : struct, Enum
            {
                Assert.AreEqual(expected, value.ToString());
                Assert.AreEqual(expected, value.AsString());
#if SPAN
                var dest = new char[expected.Length];
                Assert.True(value.TryFormat(dest, out var charsWritten));
                Assert.AreEqual(expected.Length, charsWritten);
                Assert.AreEqual(expected, new string(dest));

                dest = new char[expected.Length - 1];
                Assert.False(value.TryFormat(dest, out charsWritten));
                Assert.AreEqual(0, charsWritten);
                CollectionAssert.AreEqual(new char[dest.Length], dest);
#endif
            }
        }

        [Test]
        public void AsString_ReturnsValidResult_WhenUsingValidEnumFormat()
        {
#if DISPLAY_ATTRIBUTE
            AssertTryFormat(DisplayAttributeEnum.Up, "Arriba", EnumFormat.DisplayName);
            AssertTryFormat(DisplayAttributeEnum.Down, "Abajo", EnumFormat.DisplayName);
            AssertTryFormat(DisplayAttributeEnum.Left, "Izquierda", EnumFormat.DisplayName);
            AssertTryFormat(DisplayAttributeEnum.Right, "Derecho", EnumFormat.DisplayName);
#endif

            AssertTryFormat(ToObject<BooleanEnum>(0), "False", EnumFormat.UnderlyingValue);
            AssertTryFormat(ToObject<BooleanEnum>(0), "0", EnumFormat.DecimalValue);
            AssertTryFormat(ToObject<BooleanEnum>(0), "00", EnumFormat.HexadecimalValue);
            AssertTryFormat(ToObject<CharEnum>('a'), "a", EnumFormat.UnderlyingValue);
            AssertTryFormat(ToObject<CharEnum>('a'), ((ushort)'a').ToString(), EnumFormat.DecimalValue);
            AssertTryFormat(ToObject<CharEnum>('a'), ((ushort)'a').ToString("X4"), EnumFormat.HexadecimalValue);

            void AssertTryFormat<TEnum>(TEnum value, string expected, EnumFormat format)
                where TEnum : struct, Enum
            {
                Assert.AreEqual(expected, value.AsString(format));
#if SPAN
                var dest = new char[expected.Length];
                Assert.True(value.TryFormat(dest, out var charsWritten, format));
                Assert.AreEqual(expected.Length, charsWritten);
                Assert.AreEqual(expected, new string(dest));

                dest = new char[expected.Length - 1];
                Assert.False(value.TryFormat(dest, out charsWritten, format));
                Assert.AreEqual(0, charsWritten);
                CollectionAssert.AreEqual(new char[dest.Length], dest);
#endif
            }
        }

        [Test]
        public void AsString_ReturnsValidResult_WhenUsingValidFormat()
        {
            string[] validFormats = { null, string.Empty, "G", "g", "F", "f", "D", "d", "X", "x" };

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

            void AssertTryFormat<TEnum>(TEnum value, string expected, string format)
                where TEnum : struct, Enum
            {
                Assert.AreEqual(expected, value.AsString(format));
#if SPAN
                var dest = new char[expected.Length];
                Assert.True(value.TryFormat(dest, out var charsWritten, format));
                Assert.AreEqual(expected.Length, charsWritten);
                Assert.AreEqual(expected, new string(dest));

                dest = new char[expected.Length - 1];
                Assert.False(value.TryFormat(dest, out charsWritten, format));
                Assert.AreEqual(0, charsWritten);
                CollectionAssert.AreEqual(new char[dest.Length], dest);
#endif
            }
        }

        [Test]
        public void AsString_ThrowsFormatException_WhenUsingInvalidFormat()
        {
            Assert.Throws<FormatException>(() => ColorFlagEnum.Blue.AsString("a"));
#if SPAN
            Assert.Throws<FormatException>(() => ColorFlagEnum.Blue.TryFormat(new char[20], out _, "a"));
#endif
        }

        [Test]
        public void TryFormatLongFlagName()
        {
            AssertTryFormat((LongFlagNamesEnum)2047);

            void AssertTryFormat<TEnum>(TEnum value)
                where TEnum : struct, Enum
            {
                var expected = value.ToString();
                Assert.IsTrue(expected.Length > 256);
                Assert.AreEqual(expected, value.AsString());
#if SPAN
                var dest = new char[expected.Length];
                Assert.True(value.TryFormat(dest, out var charsWritten));
                Assert.AreEqual(expected.Length, charsWritten);
                Assert.AreEqual(expected, new string(dest));

                dest = new char[expected.Length - 1];
                Assert.False(value.TryFormat(dest, out charsWritten));
                Assert.AreEqual(0, charsWritten);
                CollectionAssert.AreEqual(new char[dest.Length], dest);
#endif
            }
        }

        [Test]
        public void Format_ReturnsValidResult_WhenUsingValidFormat()
        {
            string[] validFormats = { "G", "g", "F", "f", "D", "d", "X", "x" };

            foreach (var format in validFormats)
            {
                for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
                {
                    var value = (ColorFlagEnum)i;
                    Assert.AreEqual(Enum.Format(typeof(ColorFlagEnum), value, format), Format(value, format));
                }

                for (int i = short.MinValue; i <= (int)DateFilterOperator.NextNumberOfBusinessDays; ++i)
                {
                    var value = (DateFilterOperator)i;
                    Assert.AreEqual(Enum.Format(typeof(DateFilterOperator), value, format), Format(value, format));
                }
            }
        }

        [Test]
        public void Format_ThrowsArgumentNullException_WhenUsingNullFormat()
        {
            Assert.Throws<ArgumentNullException>(() => Format(ColorFlagEnum.Blue, (string)null));
        }

        [Test]
        public void Format_ThrowsFormatException_WhenUsingEmptyStringFormat()
        {
            Assert.Throws<FormatException>(() => Format(ColorFlagEnum.Blue, string.Empty));
        }

        [Test]
        public void Format_ThrowsFormatException_WhenUsingInvalidStringFormat()
        {
            Assert.Throws<FormatException>(() => Format(ColorFlagEnum.Blue, "a"));
        }

        [Test]
        public void GetUnderlyingValue_ReturnsExpected_OnAny()
        {
            Assert.AreEqual(2, GetUnderlyingValue(NumericOperator.GreaterThan));
            Assert.AreEqual(false, GetUnderlyingValue(ToObject<BooleanEnum>(0)));
            Assert.AreEqual('b', GetUnderlyingValue(ToObject<CharEnum>('b')));
        }
        #endregion

        #region Defined Values Main Methods
        [Test]
        public void GetName()
        {
            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
            {
                var value = (ColorFlagEnum)i;
                Assert.AreEqual(Enum.GetName(typeof(ColorFlagEnum), value), value.GetName());
            }

            for (int i = short.MinValue; i <= short.MaxValue; ++i)
            {
                var value = (DateFilterOperator)i;
                Assert.AreEqual(Enum.GetName(typeof(DateFilterOperator), value), value.GetName());
            }

            // Check for primary duplicates
            Assert.AreEqual("GreaterThanOrEquals", NumericOperator.GreaterThanOrEquals.GetName());
            Assert.AreEqual("GreaterThanOrEquals", NumericOperator.NotLessThan.GetName());
            Assert.AreEqual("NotGreaterThan", NumericOperator.LessThanOrEquals.GetName());
            Assert.AreEqual("NotGreaterThan", NumericOperator.NotGreaterThan.GetName());

            Assert.AreEqual("No", ToObject<BooleanEnum>(0).GetName());
            Assert.IsNull(ToObject<BooleanEnum>(1).GetName());

            Assert.AreEqual("A", ToObject<CharEnum>('a').GetName());
            Assert.IsNull(ToObject<CharEnum>('d').GetName());

            Assert.AreEqual("No", Enum.GetName(typeof(BooleanEnum), ToObject<BooleanEnum>(0)));
            Assert.IsNull(Enum.GetName(typeof(BooleanEnum), ToObject<BooleanEnum>(1)));
            
            Assert.AreEqual("A", Enum.GetName(typeof(CharEnum), ToObject<CharEnum>('a')));
            Assert.IsNull(Enum.GetName(typeof(CharEnum), ToObject<CharEnum>('d')));
        }
        #endregion

        #region Attributes
        [Test]
        public void HasAttribute()
        {
            Assert.IsTrue(EnumMemberAttributeEnum.A.GetMember().Attributes.Has<EnumMemberAttribute>());
            Assert.IsFalse(ColorFlagEnum.Blue.GetMember().Attributes.Has<DescriptionAttribute>());
            Assert.IsTrue(ColorFlagEnum.UltraViolet.GetMember().Attributes.Has<DescriptionAttribute>());
        }

        [Test]
        public void GetAttribute()
        {
            Assert.AreEqual("aye", EnumMemberAttributeEnum.A.GetMember().Attributes.Get<EnumMemberAttribute>().Value);
            Assert.IsNull(ColorFlagEnum.Blue.GetMember().Attributes.Get<DescriptionAttribute>());
            Assert.AreEqual("Ultra-Violet", ColorFlagEnum.UltraViolet.GetMember().Attributes.Get<DescriptionAttribute>().Description);
        }

        [Test]
        public void GetAllAttributes()
        {
            CollectionAssert.AreEquivalent(new OptionAttribute[0], MultipleAttributeEnum.None.GetMember().Attributes.GetAll<OptionAttribute>());
            CollectionAssert.AreEquivalent(new[] { new OptionAttribute("Mono") }, MultipleAttributeEnum.Single.GetMember().Attributes.GetAll<OptionAttribute>());
            CollectionAssert.AreEquivalent(new[] { new OptionAttribute("Poly"), new OptionAttribute("Plural") }, MultipleAttributeEnum.Multi.GetMember().Attributes.GetAll<OptionAttribute>());
        }

        [Test]
        public void Attributes()
        {
            CollectionAssert.AreEquivalent(new Attribute[0], MultipleAttributeEnum.None.GetMember().Attributes);
            CollectionAssert.AreEquivalent(new Attribute[] { new OptionAttribute("Mono"), new DescriptionAttribute("One") }, MultipleAttributeEnum.Single.GetMember().Attributes);
            CollectionAssert.AreEquivalent(new Attribute[] { new DescriptionAttribute("Many"), new OptionAttribute("Poly"), new OptionAttribute("Plural") }, MultipleAttributeEnum.Multi.GetMember().Attributes);

            Assert.AreEqual(0, MultipleAttributeEnum.None.GetAttributes().Count);
            Assert.AreEqual(2, MultipleAttributeEnum.Single.GetAttributes().Count);
            Assert.AreEqual(3, MultipleAttributeEnum.Multi.GetAttributes().Count);
        }
        #endregion

        #region Parsing
        [Test]
        public void Parse()
        {
            Assert.AreEqual(ToObject<BooleanEnum>(0), Parse<BooleanEnum>("No"));
            Assert.AreEqual(ToObject<BooleanEnum>(0), Parse<BooleanEnum>("False"));
            Assert.AreEqual(ToObject<BooleanEnum>(0), Parse<BooleanEnum>("0", ignoreCase: false, EnumFormat.DecimalValue));
            Assert.AreEqual(ToObject<BooleanEnum>(1), Parse<BooleanEnum>("True"));
            Assert.AreEqual(ToObject<BooleanEnum>(1), Parse<BooleanEnum>("1", ignoreCase: false, EnumFormat.DecimalValue));

            Assert.AreEqual(ToObject<CharEnum>('a'), Parse<CharEnum>("A"));
            Assert.AreEqual(ToObject<CharEnum>('a'), Parse<CharEnum>("a"));
            Assert.AreEqual(ToObject<CharEnum>('a'), Parse<CharEnum>(((ushort)'a').ToString(), ignoreCase: false, EnumFormat.DecimalValue));
            Assert.AreEqual(ToObject<CharEnum>('d'), Parse<CharEnum>("d"));
            Assert.AreEqual(ToObject<CharEnum>('d'), Parse<CharEnum>(((ushort)'d').ToString(), ignoreCase: false, EnumFormat.DecimalValue));
        }
        #endregion
    }
}
