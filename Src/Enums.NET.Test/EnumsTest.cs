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
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static EnumsNET.Enums;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace EnumsNET.Test
{
    [TestClass]
    public class EnumsTest
    {
        #region "Properties"
        [TestMethod]
        public void IsContiguous()
        {
            Assert.IsTrue(IsContiguous<DateFilterOperator>());
            Assert.IsTrue(IsContiguous<ContiguousUInt64Enum>());
            Assert.IsFalse(IsContiguous<NonContiguousEnum>());
            Assert.IsFalse(IsContiguous<NonContiguousUInt64Enum>());
        }

        [TestMethod]
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
        }

        [TestMethod]
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
        }
        #endregion

        #region Type Methods
        [TestMethod]
        public void GetDefinedCount()
        {
            Assert.AreEqual(0, GetDefinedCount<ByteEnum>());
            Assert.AreEqual(38, GetDefinedCount<DateFilterOperator>());
            Assert.AreEqual(6, GetDefinedCount<ColorFlagEnum>());
            Assert.AreEqual(10, GetDefinedCount<NumericFilterOperator>());

            Assert.AreEqual(0, GetDefinedCount<ByteEnum>(true));
            Assert.AreEqual(38, GetDefinedCount<DateFilterOperator>(true));
            Assert.AreEqual(6, GetDefinedCount<ColorFlagEnum>(true));
            Assert.AreEqual(8, GetDefinedCount<NumericFilterOperator>(true)); // Has 2 duplicates
        }

        [TestMethod]
        public void GetEnumMemberInfos()
        {
            Assert.AreEqual(0, GetEnumMemberInfos<ByteEnum>().Count());
            Assert.AreEqual(38, GetEnumMemberInfos<DateFilterOperator>().Count());
            Assert.AreEqual(6, GetEnumMemberInfos<ColorFlagEnum>().Count());
            Assert.AreEqual(10, GetEnumMemberInfos<NumericFilterOperator>().Count());

            Assert.AreEqual(0, GetEnumMemberInfos<ByteEnum>(true).Count());
            Assert.AreEqual(38, GetEnumMemberInfos<DateFilterOperator>(true).Count());
            Assert.AreEqual(6, GetEnumMemberInfos<ColorFlagEnum>(true).Count());
            Assert.AreEqual(8, GetEnumMemberInfos<NumericFilterOperator>(true).Count()); // Has 2 duplicates

            var enumMemberInfos = GetEnumMemberInfos<ColorFlagEnum>().ToList();
            AssertEnumMemberInfoIsCorrect(enumMemberInfos[0], ColorFlagEnum.Black, "Black");
            AssertEnumMemberInfoIsCorrect(enumMemberInfos[1], ColorFlagEnum.Red, "Red");
            AssertEnumMemberInfoIsCorrect(enumMemberInfos[2], ColorFlagEnum.Green, "Green");
            AssertEnumMemberInfoIsCorrect(enumMemberInfos[3], ColorFlagEnum.Blue, "Blue");
            AssertEnumMemberInfoIsCorrect(enumMemberInfos[4], ColorFlagEnum.UltraViolet, "UltraViolet", new DescriptionAttribute("Ultra-Violet"));
            AssertEnumMemberInfoIsCorrect(enumMemberInfos[5], ColorFlagEnum.All, "All");
        }

        public void AssertEnumMemberInfoIsCorrect<TEnum>(EnumMemberInfo<TEnum> info, TEnum value, string name, params Attribute[] attributes)
        {
            Assert.IsNotNull(info);
            Assert.AreEqual(value, info.Value);
            Assert.AreEqual(name, info.Name);
            CollectionAssert.AreEquivalent(attributes, info.Attributes);
        }

        [TestMethod]
        public void GetNames()
        {
            CollectionAssert.AreEqual(new[] { "Black", "Red", "Green", "Blue", "UltraViolet", "All" }, GetNames<ColorFlagEnum>().ToArray());
            CollectionAssert.AreEqual(Enum.GetNames(typeof(DateFilterOperator)), GetNames<DateFilterOperator>().ToArray());
            CollectionAssert.AreEqual(new string[0], GetNames<ByteEnum>().ToArray());
        }

        [TestMethod]
        public void GetValues()
        {
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Black, ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet, ColorFlagEnum.All }, GetValues<ColorFlagEnum>().ToArray());
            CollectionAssert.AreEqual((DateFilterOperator[])Enum.GetValues(typeof(DateFilterOperator)), GetValues<DateFilterOperator>().ToArray());
            CollectionAssert.AreEqual(new ByteEnum[0], GetValues<ByteEnum>().ToArray());

            // Duplicate order check
            var numericFilterOperators = GetValues<NumericFilterOperator>().ToArray();
            for (var i = 1; i < numericFilterOperators.Length; ++i)
            {
                Assert.IsTrue(numericFilterOperators[i - 1] <= numericFilterOperators[i]);
            }
        }

        [TestMethod]
        public void GetValues_UniqueValued()
        {
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Black, ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet, ColorFlagEnum.All }, GetValues<ColorFlagEnum>(true).ToArray());
            CollectionAssert.AreEqual((DateFilterOperator[])Enum.GetValues(typeof(DateFilterOperator)), GetValues<DateFilterOperator>(true).ToArray());
            CollectionAssert.AreEqual(new ByteEnum[0], GetValues<ByteEnum>(true).ToArray());
            CollectionAssert.AreEqual(new[] { NumericFilterOperator.Is, NumericFilterOperator.IsNot, NumericFilterOperator.GreaterThan, NumericFilterOperator.LessThan, NumericFilterOperator.GreaterThanOrEqual, NumericFilterOperator.NotGreaterThan, NumericFilterOperator.Between, NumericFilterOperator.NotBetween }, GetValues<NumericFilterOperator>(true).ToArray());
        }

        [TestMethod]
        public void GetDescriptions()
        {
            CollectionAssert.AreEqual(new[] { null, null, null, null, "Ultra-Violet", null }, GetDescriptions<ColorFlagEnum>().ToArray());
            CollectionAssert.AreEqual(new string[0], GetDescriptions<ByteEnum>().ToArray());
        }

        [TestMethod]
        public void GetAllAttributes()
        {
            TestHelper.EnumerableOfEnumerablesAreEqual(new[] { new Attribute[0], new Attribute[0], new Attribute[0], new Attribute[0], new Attribute[] { new DescriptionAttribute("Ultra-Violet") }, new Attribute[0] }, GetAllAttributes<ColorFlagEnum>());
            TestHelper.EnumerableOfEnumerablesAreEqual(new Attribute[0][], GetAllAttributes<ByteEnum>());
        }

        [TestMethod]
        public void GetAttributes()
        {
            CollectionAssert.AreEqual(new[] { null, null, null, null, new DescriptionAttribute("Ultra-Violet"), null }, GetAttributes<ColorFlagEnum, DescriptionAttribute>().ToArray());
            CollectionAssert.AreEqual(new DescriptionAttribute[0], GetAttributes<ByteEnum, DescriptionAttribute>().ToArray());
        }
        #endregion

        #region IsValid
        [TestMethod]
        public void IsValid_ReturnsSameResultAsIsValidFlagCombination_WhenUsingFlagEnum()
        {
            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
            {
                var value = (ColorFlagEnum)i;
                Assert.AreEqual(FlagEnums.IsValidFlagCombination(value), value.IsValid());
            }
        }

        [TestMethod]
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

            Assert.IsTrue(NumericFilterOperator.Is.IsValid());
            Assert.IsTrue(NumericFilterOperator.IsNot.IsValid());
            Assert.IsTrue(NumericFilterOperator.GreaterThan.IsValid());
            Assert.IsTrue(NumericFilterOperator.LessThan.IsValid());
            Assert.IsTrue(NumericFilterOperator.GreaterThanOrEqual.IsValid());
            Assert.IsTrue(NumericFilterOperator.NotLessThan.IsValid());
            Assert.IsTrue(NumericFilterOperator.LessThanOrEqual.IsValid());
            Assert.IsTrue(NumericFilterOperator.NotGreaterThan.IsValid());
            Assert.IsTrue(NumericFilterOperator.Between.IsValid());
            Assert.IsTrue(NumericFilterOperator.NotBetween.IsValid());
            Assert.IsFalse((NumericFilterOperator.Is - 1).IsValid());
            Assert.IsFalse((NumericFilterOperator.NotBetween + 1).IsValid());
        }
        #endregion

        #region IsDefined
        [TestMethod]
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

            Assert.IsTrue(NumericFilterOperator.Is.IsDefined());
            Assert.IsTrue(NumericFilterOperator.IsNot.IsDefined());
            Assert.IsTrue(NumericFilterOperator.GreaterThan.IsDefined());
            Assert.IsTrue(NumericFilterOperator.LessThan.IsDefined());
            Assert.IsTrue(NumericFilterOperator.GreaterThanOrEqual.IsDefined());
            Assert.IsTrue(NumericFilterOperator.NotLessThan.IsDefined());
            Assert.IsTrue(NumericFilterOperator.LessThanOrEqual.IsDefined());
            Assert.IsTrue(NumericFilterOperator.NotGreaterThan.IsDefined());
            Assert.IsTrue(NumericFilterOperator.Between.IsDefined());
            Assert.IsTrue(NumericFilterOperator.NotBetween.IsDefined());
            Assert.IsFalse((NumericFilterOperator.Is - 1).IsDefined());
            Assert.IsFalse((NumericFilterOperator.NotBetween + 1).IsDefined());
        }

        [TestMethod]
        public void IsDefined_ReturnsValidResults_WhenUsingValidName()
        {
            Assert.IsTrue(IsDefined<ColorFlagEnum>("UltraViolet"));
        }

        [TestMethod]
        public void IsDefined_ReturnsFalse_WhenUsingInvalidName()
        {
            Assert.IsFalse(IsDefined<ColorFlagEnum>("ulTrAvioLet"));
        }

        [TestMethod]
        public void IsDefined_ReturnsTrue_WhenUsingValidNameWhileIgnoringCase()
        {
            Assert.IsTrue(IsDefined<ColorFlagEnum>("ulTrAvioLet", true));
        }

        [TestMethod]
        public void IsDefined_ReturnsTrue_WhenUsingValidNumber()
        {
            Assert.IsTrue(IsDefined<ColorFlagEnum>(1));
        }

        [TestMethod]
        public void IsDefined_ReturnsTrue_WhenUsingUndefinedNumber()
        {
            Assert.IsFalse(IsDefined<ColorFlagEnum>(-1));
        }

        [TestMethod]
        public void IsDefined_ReturnsFalse_WhenUsingLargeNumber()
        {
            Assert.IsFalse(IsDefined<ColorFlagEnum>(128));
        }
        #endregion

        #region IsInValueRange
        [TestMethod]
        public void IsInValueRange_ReturnsValidResults_WhenUsingSByteOverload()
        {
            Assert.IsTrue(IsInValueRange<SByteEnum>(sbyte.MinValue));
            Assert.IsTrue(IsInValueRange<SByteEnum>(sbyte.MaxValue));
            Assert.IsFalse(IsInValueRange<ByteEnum>(sbyte.MinValue));
            Assert.IsTrue(IsInValueRange<ByteEnum>(sbyte.MaxValue));
            Assert.IsTrue(IsInValueRange<Int16Enum>(sbyte.MinValue));
            Assert.IsTrue(IsInValueRange<Int16Enum>(sbyte.MaxValue));
            Assert.IsFalse(IsInValueRange<UInt16Enum>(sbyte.MinValue));
            Assert.IsTrue(IsInValueRange<UInt16Enum>(sbyte.MaxValue));
            Assert.IsTrue(IsInValueRange<Int32Enum>(sbyte.MinValue));
            Assert.IsTrue(IsInValueRange<Int32Enum>(sbyte.MaxValue));
            Assert.IsFalse(IsInValueRange<UInt32Enum>(sbyte.MinValue));
            Assert.IsTrue(IsInValueRange<UInt32Enum>(sbyte.MaxValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(sbyte.MinValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(sbyte.MaxValue));
            Assert.IsFalse(IsInValueRange<UInt64Enum>(sbyte.MinValue));
            Assert.IsTrue(IsInValueRange<UInt64Enum>(sbyte.MaxValue));
        }

        [TestMethod]
        public void IsInValueRange_ReturnsValidResults_WhenUsingByteOverload()
        {
            Assert.IsTrue(IsInValueRange<SByteEnum>(byte.MinValue));
            Assert.IsFalse(IsInValueRange<SByteEnum>(byte.MaxValue));
            Assert.IsTrue(IsInValueRange<ByteEnum>(byte.MinValue));
            Assert.IsTrue(IsInValueRange<ByteEnum>(byte.MaxValue));
            Assert.IsTrue(IsInValueRange<Int16Enum>(byte.MinValue));
            Assert.IsTrue(IsInValueRange<Int16Enum>(byte.MaxValue));
            Assert.IsTrue(IsInValueRange<UInt16Enum>(byte.MinValue));
            Assert.IsTrue(IsInValueRange<UInt16Enum>(byte.MaxValue));
            Assert.IsTrue(IsInValueRange<Int32Enum>(byte.MinValue));
            Assert.IsTrue(IsInValueRange<Int32Enum>(byte.MaxValue));
            Assert.IsTrue(IsInValueRange<UInt32Enum>(byte.MinValue));
            Assert.IsTrue(IsInValueRange<UInt32Enum>(byte.MaxValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(byte.MinValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(byte.MaxValue));
            Assert.IsTrue(IsInValueRange<UInt64Enum>(byte.MinValue));
            Assert.IsTrue(IsInValueRange<UInt64Enum>(byte.MaxValue));
        }

        [TestMethod]
        public void IsInValueRange_ReturnsValidResults_WhenUsingShortOverload()
        {
            Assert.IsFalse(IsInValueRange<SByteEnum>(short.MinValue));
            Assert.IsFalse(IsInValueRange<SByteEnum>(short.MaxValue));
            Assert.IsFalse(IsInValueRange<ByteEnum>(short.MinValue));
            Assert.IsFalse(IsInValueRange<ByteEnum>(short.MaxValue));
            Assert.IsTrue(IsInValueRange<Int16Enum>(short.MinValue));
            Assert.IsTrue(IsInValueRange<Int16Enum>(short.MaxValue));
            Assert.IsFalse(IsInValueRange<UInt16Enum>(short.MinValue));
            Assert.IsTrue(IsInValueRange<UInt16Enum>(short.MaxValue));
            Assert.IsTrue(IsInValueRange<Int32Enum>(short.MinValue));
            Assert.IsTrue(IsInValueRange<Int32Enum>(short.MaxValue));
            Assert.IsFalse(IsInValueRange<UInt32Enum>(short.MinValue));
            Assert.IsTrue(IsInValueRange<UInt32Enum>(short.MaxValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(short.MinValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(short.MaxValue));
            Assert.IsFalse(IsInValueRange<UInt64Enum>(short.MinValue));
            Assert.IsTrue(IsInValueRange<UInt64Enum>(short.MaxValue));
        }

        [TestMethod]
        public void IsInValueRange_ReturnsValidResults_WhenUsingUShortOverload()
        {
            Assert.IsTrue(IsInValueRange<SByteEnum>(ushort.MinValue));
            Assert.IsFalse(IsInValueRange<SByteEnum>(ushort.MaxValue));
            Assert.IsTrue(IsInValueRange<ByteEnum>(ushort.MinValue));
            Assert.IsFalse(IsInValueRange<ByteEnum>(ushort.MaxValue));
            Assert.IsTrue(IsInValueRange<Int16Enum>(ushort.MinValue));
            Assert.IsFalse(IsInValueRange<Int16Enum>(ushort.MaxValue));
            Assert.IsTrue(IsInValueRange<UInt16Enum>(ushort.MinValue));
            Assert.IsTrue(IsInValueRange<UInt16Enum>(ushort.MaxValue));
            Assert.IsTrue(IsInValueRange<Int32Enum>(ushort.MinValue));
            Assert.IsTrue(IsInValueRange<Int32Enum>(ushort.MaxValue));
            Assert.IsTrue(IsInValueRange<UInt32Enum>(ushort.MinValue));
            Assert.IsTrue(IsInValueRange<UInt32Enum>(ushort.MaxValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(ushort.MinValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(ushort.MaxValue));
            Assert.IsTrue(IsInValueRange<UInt64Enum>(ushort.MinValue));
            Assert.IsTrue(IsInValueRange<UInt64Enum>(ushort.MaxValue));
        }

        [TestMethod]
        public void IsInValueRange_ReturnsValidResults_WhenUsingIntOverload()
        {
            Assert.IsFalse(IsInValueRange<SByteEnum>(int.MinValue));
            Assert.IsFalse(IsInValueRange<SByteEnum>(int.MaxValue));
            Assert.IsFalse(IsInValueRange<ByteEnum>(int.MinValue));
            Assert.IsFalse(IsInValueRange<ByteEnum>(int.MaxValue));
            Assert.IsFalse(IsInValueRange<Int16Enum>(int.MinValue));
            Assert.IsFalse(IsInValueRange<Int16Enum>(int.MaxValue));
            Assert.IsFalse(IsInValueRange<UInt16Enum>(int.MinValue));
            Assert.IsFalse(IsInValueRange<UInt16Enum>(int.MaxValue));
            Assert.IsTrue(IsInValueRange<Int32Enum>(int.MinValue));
            Assert.IsTrue(IsInValueRange<Int32Enum>(int.MaxValue));
            Assert.IsFalse(IsInValueRange<UInt32Enum>(int.MinValue));
            Assert.IsTrue(IsInValueRange<UInt32Enum>(int.MaxValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(int.MinValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(int.MaxValue));
            Assert.IsFalse(IsInValueRange<UInt64Enum>(int.MinValue));
            Assert.IsTrue(IsInValueRange<UInt64Enum>(int.MaxValue));
        }

        [TestMethod]
        public void IsInValueRange_ReturnsValidResults_WhenUsingUIntOverload()
        {
            Assert.IsTrue(IsInValueRange<SByteEnum>(uint.MinValue));
            Assert.IsFalse(IsInValueRange<SByteEnum>(uint.MaxValue));
            Assert.IsTrue(IsInValueRange<ByteEnum>(uint.MinValue));
            Assert.IsFalse(IsInValueRange<ByteEnum>(uint.MaxValue));
            Assert.IsTrue(IsInValueRange<Int16Enum>(uint.MinValue));
            Assert.IsFalse(IsInValueRange<Int16Enum>(uint.MaxValue));
            Assert.IsTrue(IsInValueRange<UInt16Enum>(uint.MinValue));
            Assert.IsFalse(IsInValueRange<UInt16Enum>(uint.MaxValue));
            Assert.IsTrue(IsInValueRange<Int32Enum>(uint.MinValue));
            Assert.IsFalse(IsInValueRange<Int32Enum>(uint.MaxValue));
            Assert.IsTrue(IsInValueRange<UInt32Enum>(uint.MinValue));
            Assert.IsTrue(IsInValueRange<UInt32Enum>(uint.MaxValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(uint.MinValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(uint.MaxValue));
            Assert.IsTrue(IsInValueRange<UInt64Enum>(uint.MinValue));
            Assert.IsTrue(IsInValueRange<UInt64Enum>(uint.MaxValue));
        }

        [TestMethod]
        public void IsInValueRange_ReturnsValidResults_WhenUsingLongOverload()
        {
            Assert.IsFalse(IsInValueRange<SByteEnum>(long.MinValue));
            Assert.IsFalse(IsInValueRange<SByteEnum>(long.MaxValue));
            Assert.IsFalse(IsInValueRange<ByteEnum>(long.MinValue));
            Assert.IsFalse(IsInValueRange<ByteEnum>(long.MaxValue));
            Assert.IsFalse(IsInValueRange<Int16Enum>(long.MinValue));
            Assert.IsFalse(IsInValueRange<Int16Enum>(long.MaxValue));
            Assert.IsFalse(IsInValueRange<UInt16Enum>(long.MinValue));
            Assert.IsFalse(IsInValueRange<UInt16Enum>(long.MaxValue));
            Assert.IsFalse(IsInValueRange<Int32Enum>(long.MinValue));
            Assert.IsFalse(IsInValueRange<Int32Enum>(long.MaxValue));
            Assert.IsFalse(IsInValueRange<UInt32Enum>(long.MinValue));
            Assert.IsFalse(IsInValueRange<UInt32Enum>(long.MaxValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(long.MinValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(long.MaxValue));
            Assert.IsFalse(IsInValueRange<UInt64Enum>(long.MinValue));
            Assert.IsTrue(IsInValueRange<UInt64Enum>(long.MaxValue));
        }

        [TestMethod]
        public void IsInValueRange_ReturnsValidResults_WhenUsingULongOverload()
        {
            Assert.IsTrue(IsInValueRange<SByteEnum>(ulong.MinValue));
            Assert.IsFalse(IsInValueRange<SByteEnum>(ulong.MaxValue));
            Assert.IsTrue(IsInValueRange<ByteEnum>(ulong.MinValue));
            Assert.IsFalse(IsInValueRange<ByteEnum>(ulong.MaxValue));
            Assert.IsTrue(IsInValueRange<Int16Enum>(ulong.MinValue));
            Assert.IsFalse(IsInValueRange<Int16Enum>(ulong.MaxValue));
            Assert.IsTrue(IsInValueRange<UInt16Enum>(ulong.MinValue));
            Assert.IsFalse(IsInValueRange<UInt16Enum>(ulong.MaxValue));
            Assert.IsTrue(IsInValueRange<Int32Enum>(ulong.MinValue));
            Assert.IsFalse(IsInValueRange<Int32Enum>(ulong.MaxValue));
            Assert.IsTrue(IsInValueRange<UInt32Enum>(ulong.MinValue));
            Assert.IsFalse(IsInValueRange<UInt32Enum>(ulong.MaxValue));
            Assert.IsTrue(IsInValueRange<Int64Enum>(ulong.MinValue));
            Assert.IsFalse(IsInValueRange<Int64Enum>(ulong.MaxValue));
            Assert.IsTrue(IsInValueRange<UInt64Enum>(ulong.MinValue));
            Assert.IsTrue(IsInValueRange<UInt64Enum>(ulong.MaxValue));
        }
        #endregion

        #region ToObject
        [TestMethod]
        public void ToObject_ReturnsValidValue_WhenUsingValidNumber()
        {
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>((sbyte)1, false));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>((byte)1, false));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>((short)1, false));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>((ushort)1, false));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>(1, false));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>(1U, false));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>(1L, false));
            Assert.AreEqual((SByteEnum)1, ToObject<SByteEnum>(1UL, false));

            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>((sbyte)1, false));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>((byte)1, false));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>((short)1, false));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>((ushort)1, false));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>(1, false));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>(1U, false));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>(1L, false));
            Assert.AreEqual((ByteEnum)1, ToObject<ByteEnum>(1UL, false));

            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>((sbyte)1, false));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>((byte)1, false));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>((short)1, false));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>((ushort)1, false));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>(1, false));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>(1U, false));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>(1L, false));
            Assert.AreEqual((Int16Enum)1, ToObject<Int16Enum>(1UL, false));

            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>((sbyte)1, false));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>((byte)1, false));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>((short)1, false));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>((ushort)1, false));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>(1, false));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>(1U, false));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>(1L, false));
            Assert.AreEqual((UInt16Enum)1, ToObject<UInt16Enum>(1UL, false));

            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>((sbyte)1, false));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>((byte)1, false));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>((short)1, false));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>((ushort)1, false));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>(1, false));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>(1U, false));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>(1L, false));
            Assert.AreEqual((Int32Enum)1, ToObject<Int32Enum>(1UL, false));

            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>((sbyte)1, false));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>((byte)1, false));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>((short)1, false));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>((ushort)1, false));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>(1, false));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>(1U, false));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>(1L, false));
            Assert.AreEqual((UInt32Enum)1, ToObject<UInt32Enum>(1UL, false));

            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>((sbyte)1, false));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>((byte)1, false));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>((short)1, false));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>((ushort)1, false));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>(1, false));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>(1U, false));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>(1L, false));
            Assert.AreEqual((Int64Enum)1, ToObject<Int64Enum>(1UL, false));

            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>((sbyte)1, false));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>((byte)1, false));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>((short)1, false));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>((ushort)1, false));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>(1, false));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>(1U, false));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>(1L, false));
            Assert.AreEqual((UInt64Enum)1, ToObject<UInt64Enum>(1UL, false));
        }

        [TestMethod]
        public void ToObject_ThrowsOverflowException_WhenUsingValuesOutsideValueRange()
        {
            TestHelper.ExpectException<OverflowException>(() => ToObject<SByteEnum>(byte.MaxValue, false));
            TestHelper.ExpectException<OverflowException>(() => ToObject<ByteEnum>(sbyte.MinValue, false));
            TestHelper.ExpectException<OverflowException>(() => ToObject<Int16Enum>(ushort.MaxValue, false));
            TestHelper.ExpectException<OverflowException>(() => ToObject<UInt16Enum>(short.MinValue, false));
            TestHelper.ExpectException<OverflowException>(() => ToObject<Int32Enum>(uint.MaxValue, false));
            TestHelper.ExpectException<OverflowException>(() => ToObject<UInt32Enum>(int.MinValue, false));
            TestHelper.ExpectException<OverflowException>(() => ToObject<Int64Enum>(ulong.MaxValue, false));
            TestHelper.ExpectException<OverflowException>(() => ToObject<UInt64Enum>(long.MinValue, false));
        }

        [TestMethod]
        public void ToObject_ThrowsArgumentException_WhenUsingInvalidValueAndCheckIsOn()
        {
            TestHelper.ExpectException<ArgumentException>(() => ToObject<SByteEnum>(sbyte.MaxValue));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<ByteEnum>(byte.MaxValue));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<Int16Enum>(short.MaxValue));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<UInt16Enum>(ushort.MaxValue));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<Int32Enum>(int.MaxValue));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<UInt32Enum>(uint.MaxValue));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<Int64Enum>(long.MaxValue));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<UInt64Enum>(ulong.MaxValue));
        }

        [TestMethod]
        public void ToObjectOrDefault_ReturnsValidResult_WhenUsingValidValue()
        {
            Assert.AreEqual(ColorFlagEnum.Red, ToObjectOrDefault((sbyte)1, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Red, ToObjectOrDefault((byte)1, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Red, ToObjectOrDefault((short)1, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Red, ToObjectOrDefault((ushort)1, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Red, ToObjectOrDefault(1, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Red, ToObjectOrDefault(1U, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Red, ToObjectOrDefault(1L, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Red, ToObjectOrDefault(1UL, ColorFlagEnum.Blue));
        }

        [TestMethod]
        public void ToObjectOrDefault_ReturnsDefaultValue_WhenUsingValueInRangeButNotValidButCheckIsOff()
        {
            Assert.AreEqual((ColorFlagEnum)16, ToObjectOrDefault((sbyte)16, ColorFlagEnum.Blue, false));
            Assert.AreEqual((ColorFlagEnum)16, ToObjectOrDefault((byte)16, ColorFlagEnum.Blue, false));
            Assert.AreEqual((ColorFlagEnum)16, ToObjectOrDefault((short)16, ColorFlagEnum.Blue, false));
            Assert.AreEqual((ColorFlagEnum)16, ToObjectOrDefault((ushort)16, ColorFlagEnum.Blue, false));
            Assert.AreEqual((ColorFlagEnum)16, ToObjectOrDefault(16, ColorFlagEnum.Blue, false));
            Assert.AreEqual((ColorFlagEnum)16, ToObjectOrDefault(16U, ColorFlagEnum.Blue, false));
            Assert.AreEqual((ColorFlagEnum)16, ToObjectOrDefault(16L, ColorFlagEnum.Blue, false));
            Assert.AreEqual((ColorFlagEnum)16, ToObjectOrDefault(16UL, ColorFlagEnum.Blue, false));
        }

        [TestMethod]
        public void ToObjectOrDefault_ReturnsDefaultValue_WhenUsingValueInRangeButNotValid()
        {
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((sbyte)16, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((byte)16, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((short)16, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((ushort)16, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(16, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(16U, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(16L, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(16UL, ColorFlagEnum.Blue));
        }

        [TestMethod]
        public void ToObjectOrDefault_ReturnsDefaultValue_WhenUsingValueOutOfRange()
        {
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((byte)128, ColorFlagEnum.Blue, false));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((short)128, ColorFlagEnum.Blue, false));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((ushort)128, ColorFlagEnum.Blue, false));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(128, ColorFlagEnum.Blue, false));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(128U, ColorFlagEnum.Blue, false));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(128L, ColorFlagEnum.Blue, false));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(128UL, ColorFlagEnum.Blue, false));

            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((byte)128, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((short)128, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((ushort)128, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(128, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(128U, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(128L, ColorFlagEnum.Blue));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(128UL, ColorFlagEnum.Blue));
        }

        [TestMethod]
        public void TryToObject_ReturnsTrueAndValidValue_WhenUsingValidNumber()
        {
            SByteEnum sbyteResult;
            var sbyteValue = (SByteEnum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out sbyteResult, false));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject((byte)1, out sbyteResult, false));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject((short)1, out sbyteResult, false));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject((ushort)1, out sbyteResult, false));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject(1, out sbyteResult, false));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject(1U, out sbyteResult, false));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject(1L, out sbyteResult, false));
            Assert.AreEqual(sbyteValue, sbyteResult);
            Assert.IsTrue(TryToObject(1UL, out sbyteResult, false));
            Assert.AreEqual(sbyteValue, sbyteResult);

            ByteEnum byteResult;
            var byteValue = (ByteEnum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out byteResult, false));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject((byte)1, out byteResult, false));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject((short)1, out byteResult, false));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject((ushort)1, out byteResult, false));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject(1, out byteResult, false));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject(1U, out byteResult, false));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject(1L, out byteResult, false));
            Assert.AreEqual(byteValue, byteResult);
            Assert.IsTrue(TryToObject(1UL, out byteResult, false));
            Assert.AreEqual(byteValue, byteResult);

            Int16Enum int16Result;
            var int16Value = (Int16Enum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out int16Result, false));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject((byte)1, out int16Result, false));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject((short)1, out int16Result, false));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject((ushort)1, out int16Result, false));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject(1, out int16Result, false));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject(1U, out int16Result, false));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject(1L, out int16Result, false));
            Assert.AreEqual(int16Value, int16Result);
            Assert.IsTrue(TryToObject(1UL, out int16Result, false));
            Assert.AreEqual(int16Value, int16Result);

            UInt16Enum uint16Result;
            var uint16Value = (UInt16Enum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out uint16Result, false));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject((byte)1, out uint16Result, false));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject((short)1, out uint16Result, false));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject((ushort)1, out uint16Result, false));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject(1, out uint16Result, false));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject(1U, out uint16Result, false));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject(1L, out uint16Result, false));
            Assert.AreEqual(uint16Value, uint16Result);
            Assert.IsTrue(TryToObject(1UL, out uint16Result, false));
            Assert.AreEqual(uint16Value, uint16Result);

            Int32Enum int32Result;
            var int32Value = (Int32Enum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out int32Result, false));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject((byte)1, out int32Result, false));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject((short)1, out int32Result, false));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject((ushort)1, out int32Result, false));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject(1, out int32Result, false));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject(1U, out int32Result, false));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject(1L, out int32Result, false));
            Assert.AreEqual(int32Value, int32Result);
            Assert.IsTrue(TryToObject(1UL, out int32Result, false));
            Assert.AreEqual(int32Value, int32Result);

            UInt32Enum uint32Result;
            var uint32Value = (UInt32Enum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out uint32Result, false));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject((byte)1, out uint32Result, false));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject((short)1, out uint32Result, false));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject((ushort)1, out uint32Result, false));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject(1, out uint32Result, false));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject(1U, out uint32Result, false));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject(1L, out uint32Result, false));
            Assert.AreEqual(uint32Value, uint32Result);
            Assert.IsTrue(TryToObject(1UL, out uint32Result, false));
            Assert.AreEqual(uint32Value, uint32Result);

            Int64Enum int64Result;
            var int64Value = (Int64Enum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out int64Result, false));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject((byte)1, out int64Result, false));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject((short)1, out int64Result, false));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject((ushort)1, out int64Result, false));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject(1, out int64Result, false));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject(1U, out int64Result, false));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject(1L, out int64Result, false));
            Assert.AreEqual(int64Value, int64Result);
            Assert.IsTrue(TryToObject(1UL, out int64Result, false));
            Assert.AreEqual(int64Value, int64Result);

            UInt64Enum uint64Result;
            var uint64Value = (UInt64Enum)1;
            Assert.IsTrue(TryToObject((sbyte)1, out uint64Result, false));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject((byte)1, out uint64Result, false));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject((short)1, out uint64Result, false));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject((ushort)1, out uint64Result, false));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject(1, out uint64Result, false));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject(1U, out uint64Result, false));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject(1L, out uint64Result, false));
            Assert.AreEqual(uint64Value, uint64Result);
            Assert.IsTrue(TryToObject(1UL, out uint64Result, false));
            Assert.AreEqual(uint64Value, uint64Result);
        }

        [TestMethod]
        public void TryToObject_ReturnsFalse_WhenUsingValueInRangeButNotValid()
        {
            ColorFlagEnum result;
            Assert.IsFalse(TryToObject((sbyte)16, out result));
            Assert.IsFalse(TryToObject((byte)16, out result));
            Assert.IsFalse(TryToObject((short)16, out result));
            Assert.IsFalse(TryToObject((ushort)16, out result));
            Assert.IsFalse(TryToObject(16, out result));
            Assert.IsFalse(TryToObject(16U, out result));
            Assert.IsFalse(TryToObject(16L, out result));
            Assert.IsFalse(TryToObject(16UL, out result));
        }

        [TestMethod]
        public void TryToObject_ReturnsTrueAndValidValue_WhenUsingValueInRangeButNotValidButCheckIsOff()
        {
            ColorFlagEnum result;
            var value = (ColorFlagEnum)16;
            Assert.IsTrue(TryToObject((sbyte)16, out result, false));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject((byte)16, out result, false));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject((short)16, out result, false));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject((ushort)16, out result, false));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject(16, out result, false));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject(16U, out result, false));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject(16L, out result, false));
            Assert.AreEqual(value, result);
            Assert.IsTrue(TryToObject(16UL, out result, false));
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void TryToObject_ReturnsFalse_WhenUsingValueOutOfRange()
        {
            ColorFlagEnum result;
            Assert.IsFalse(TryToObject((byte)128, out result, false));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject((short)128, out result, false));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject((ushort)128, out result, false));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject(128, out result, false));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject(128U, out result, false));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject(128L, out result, false));
            Assert.AreEqual(default(ColorFlagEnum), result);
            Assert.IsFalse(TryToObject(128UL, out result, false));
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
        [TestMethod]
        public void Validate()
        {
            NonContiguousEnum.Cat.Validate("paramName");
            NonContiguousEnum.Dog.Validate("paramName");
            NonContiguousEnum.Chimp.Validate("paramName");
            NonContiguousEnum.Elephant.Validate("paramName");
            NonContiguousEnum.Whale.Validate("paramName");
            NonContiguousEnum.Eagle.Validate("paramName");
            TestHelper.ExpectException<ArgumentException>(() => ((NonContiguousEnum)(-5)).Validate("paramName"));

            UInt64FlagEnum.Flies.Validate("paramName");
            UInt64FlagEnum.Hops.Validate("paramName");
            UInt64FlagEnum.Runs.Validate("paramName");
            UInt64FlagEnum.Slithers.Validate("paramName");
            UInt64FlagEnum.Stationary.Validate("paramName");
            UInt64FlagEnum.Swims.Validate("paramName");
            UInt64FlagEnum.Walks.Validate("paramName");
            (UInt64FlagEnum.Flies | UInt64FlagEnum.Hops).Validate("paramName");
            (UInt64FlagEnum.Flies | UInt64FlagEnum.Slithers).Validate("paramName");
            TestHelper.ExpectException<ArgumentException>(() => ((UInt64FlagEnum)8).Validate("paramName"));
            TestHelper.ExpectException<ArgumentException>(() => ((UInt64FlagEnum)8 | UInt64FlagEnum.Hops).Validate("paramName"));

            ContiguousUInt64Enum.A.Validate("paramName");
            ContiguousUInt64Enum.B.Validate("paramName");
            ContiguousUInt64Enum.C.Validate("paramName");
            ContiguousUInt64Enum.D.Validate("paramName");
            ContiguousUInt64Enum.E.Validate("paramName");
            ContiguousUInt64Enum.F.Validate("paramName");
            TestHelper.ExpectException<ArgumentException>(() => (ContiguousUInt64Enum.A - 1).Validate("paramName"));
            TestHelper.ExpectException<ArgumentException>(() => (ContiguousUInt64Enum.F + 1).Validate("paramName"));

            NonContiguousUInt64Enum.SaintLouis.Validate("paramName");
            NonContiguousUInt64Enum.Chicago.Validate("paramName");
            NonContiguousUInt64Enum.Cincinnati.Validate("paramName");
            NonContiguousUInt64Enum.Pittsburg.Validate("paramName");
            NonContiguousUInt64Enum.Milwaukee.Validate("paramName");
            TestHelper.ExpectException<ArgumentException>(() => ((NonContiguousUInt64Enum)5).Validate("paramName"));
            TestHelper.ExpectException<ArgumentException>(() => ((NonContiguousUInt64Enum)50000000UL).Validate("paramName"));

            NumericFilterOperator.Is.Validate("paramName");
            NumericFilterOperator.IsNot.Validate("paramName");
            NumericFilterOperator.GreaterThan.Validate("paramName");
            NumericFilterOperator.LessThan.Validate("paramName");
            NumericFilterOperator.GreaterThanOrEqual.Validate("paramName");
            NumericFilterOperator.NotLessThan.Validate("paramName");
            NumericFilterOperator.LessThanOrEqual.Validate("paramName");
            NumericFilterOperator.NotGreaterThan.Validate("paramName");
            NumericFilterOperator.Between.Validate("paramName");
            NumericFilterOperator.NotBetween.Validate("paramName");
            TestHelper.ExpectException<ArgumentException>(() => (NumericFilterOperator.Is - 1).Validate("paramName"));
            TestHelper.ExpectException<ArgumentException>(() => (NumericFilterOperator.NotBetween + 1).Validate("paramName"));
        }

        [TestMethod]
        public void AsString()
        {
            for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
            {
                var value = (ColorFlagEnum)i;
                Assert.AreEqual(value.ToString(), value.AsString());
            }

            for (int i = short.MinValue; i <= short.MaxValue; ++i)
            {
                var value = (DateFilterOperator)i;
                Assert.AreEqual(value.ToString(), value.AsString());
            }
        }

        [TestMethod]
        public void AsString_ReturnsValidResult_WhenUsingValidFormat()
        {
            string[] validFormats = { null, string.Empty, "G", "g", "F", "f", "D", "d", "X", "x" };

            foreach (var format in validFormats)
            {
                for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
                {
                    var value = (ColorFlagEnum)i;
                    Assert.AreEqual(value.ToString(format), value.AsString(format));
                }

                for (int i = short.MinValue; i <= (int)DateFilterOperator.NextNumberOfBusinessDays; ++i)
                {
                    var value = (DateFilterOperator)i;
                    Assert.AreEqual(value.ToString(format), value.AsString(format));
                }
            }
        }

        [TestMethod]
        public void AsString_ThrowsFormatException_WhenUsingInvalidFormat()
        {
            TestHelper.ExpectException<FormatException>(() => ColorFlagEnum.Blue.AsString("a"));
        }

        [TestMethod]
        public void Format_ReturnsValidResult_WhenUsingValidFormat()
        {
            string[] validFormats = { "G", "g", "F", "f", "D", "d", "X", "x" };

            foreach (var format in validFormats)
            {
                for (int i = sbyte.MinValue; i <= sbyte.MaxValue; ++i)
                {
                    var value = (ColorFlagEnum)i;
                    Assert.AreEqual(Enum.Format(typeof(ColorFlagEnum), value, format), value.Format(format));
                }

                for (int i = short.MinValue; i <= (int)DateFilterOperator.NextNumberOfBusinessDays; ++i)
                {
                    var value = (DateFilterOperator)i;
                    Assert.AreEqual(Enum.Format(typeof(DateFilterOperator), value, format), value.Format(format));
                }
            }
        }

        [TestMethod]
        public void Format_ThrowsArgumentNullException_WhenUsingNullFormat()
        {
            TestHelper.ExpectException<ArgumentNullException>(() => ColorFlagEnum.Blue.Format((string)null));
        }

        [TestMethod]
        public void Format_ThrowsFormatException_WhenUsingEmptyStringFormat()
        {
            TestHelper.ExpectException<FormatException>(() => ColorFlagEnum.Blue.Format(string.Empty));
        }

        [TestMethod]
        public void Format_ThrowsFormatException_WhenUsingInvalidStringFormat()
        {
            TestHelper.ExpectException<FormatException>(() => ColorFlagEnum.Blue.Format("a"));
        }

        [TestMethod]
        public void Format_ReturnsExpected_WhenUsingCustomEnumFormat()
        {
            var customFormat = RegisterCustomEnumFormat(info => info.GetAttribute<EnumMemberAttribute>().Value);
            Assert.AreEqual("a", EnumMemberAttributeEnum.A.Format(customFormat));
        }

        [TestMethod]
        public void GetUnderlyingValue_ReturnsExpected_OnAny()
        {
            Assert.AreEqual(2, NumericFilterOperator.GreaterThan.GetUnderlyingValue());
        }
        #endregion

        #region Defined Values Main Methods
        [TestMethod]
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

            // Check for main duplicates
            Assert.AreEqual("GreaterThanOrEqual", NumericFilterOperator.GreaterThanOrEqual.GetName());
            Assert.AreEqual("GreaterThanOrEqual", NumericFilterOperator.NotLessThan.GetName());
            Assert.AreEqual("NotGreaterThan", NumericFilterOperator.LessThanOrEqual.GetName());
            Assert.AreEqual("NotGreaterThan", NumericFilterOperator.NotGreaterThan.GetName());
        }

        [TestMethod]
        public void GetDescription_ReturnsDescription_WhenUsingValidValueWithDescription()
        {
            Assert.AreEqual("Ultra-Violet", ColorFlagEnum.UltraViolet.GetDescription());
        }

        [TestMethod]
        public void GetDescription_ReturnsNull_WhenUsingValidValueWithoutDescription()
        {
            Assert.IsNull(ColorFlagEnum.Black.GetDescription());
            Assert.IsNull(ColorFlagEnum.Red.GetDescription());
            Assert.IsNull(ColorFlagEnum.Green.GetDescription());
            Assert.IsNull(ColorFlagEnum.Blue.GetDescription());
        }
        #endregion

        // TODO
        #region Attributes
        #endregion

        // TODO
        #region Parsing
        #endregion
    }
}
