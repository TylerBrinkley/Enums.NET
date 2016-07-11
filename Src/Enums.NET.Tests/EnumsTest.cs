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
using System.Reflection;
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
        [Test]
        public void TestGenericConstraints()
        {
            var enumTypeArgs = new[] { typeof(Enums), typeof(FlagEnums) }
                .SelectMany(type => type.GetMethods(BindingFlags.Static | BindingFlags.Public))
                .Where(method => method.IsGenericMethod && ((method.Name != nameof(FlagEnums.GetFlags) && method.Name != nameof(FlagEnums.GetFlagMembers)) || method.GetParameters()[0].Name != "member"))
                .Select(method => method.GetGenericArguments()[0])
                .Where(genericArg => genericArg != null)
                .Concat(new[] { typeof(EnumComparer<>).GetGenericArguments()[0] });
            foreach (var enumTypeArg in enumTypeArgs)
            {
                Assert.IsTrue(enumTypeArg.GetGenericParameterConstraints().Any(genericParamConstraint => genericParamConstraint == typeof(Enum)));
            }
        }

        #region "Properties"
        [Test]
        public void IsContiguous()
        {
            Assert.IsTrue(IsContiguous<DateFilterOperator>());
            Assert.IsTrue(IsContiguous<ContiguousUInt64Enum>());
            Assert.IsFalse(IsContiguous<NonContiguousEnum>());
            Assert.IsFalse(IsContiguous<NonContiguousUInt64Enum>());
        }

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
        }

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
        }
        #endregion

        #region Type Methods
        [Test]
        public void GetEnumMemberCount()
        {
            Assert.AreEqual(0, GetEnumMemberCount<ByteEnum>());
            Assert.AreEqual(38, GetEnumMemberCount<DateFilterOperator>());
            Assert.AreEqual(6, GetEnumMemberCount<ColorFlagEnum>());
            Assert.AreEqual(10, GetEnumMemberCount<NumericFilterOperator>());

            Assert.AreEqual(0, GetEnumMemberCount<ByteEnum>(true));
            Assert.AreEqual(38, GetEnumMemberCount<DateFilterOperator>(true));
            Assert.AreEqual(6, GetEnumMemberCount<ColorFlagEnum>(true));
            Assert.AreEqual(8, GetEnumMemberCount<NumericFilterOperator>(true)); // Has 2 duplicates
        }

        [Test]
        public void GetEnumMembers()
        {
            Assert.AreEqual(0, GetEnumMembers<ByteEnum>().Count());
            Assert.AreEqual(38, GetEnumMembers<DateFilterOperator>().Count());
            Assert.AreEqual(6, GetEnumMembers<ColorFlagEnum>().Count());
            Assert.AreEqual(10, GetEnumMembers<NumericFilterOperator>().Count());

            Assert.AreEqual(0, GetEnumMembers<ByteEnum>(true).Count());
            Assert.AreEqual(38, GetEnumMembers<DateFilterOperator>(true).Count());
            Assert.AreEqual(6, GetEnumMembers<ColorFlagEnum>(true).Count());
            Assert.AreEqual(8, GetEnumMembers<NumericFilterOperator>(true).Count()); // Has 2 duplicates

            var enumMembers = GetEnumMembers<ColorFlagEnum>().ToList();
            AssertEnumMemberIsCorrect(enumMembers[0], ColorFlagEnum.Black, "Black");
            AssertEnumMemberIsCorrect(enumMembers[1], ColorFlagEnum.Red, "Red");
            AssertEnumMemberIsCorrect(enumMembers[2], ColorFlagEnum.Green, "Green");
            AssertEnumMemberIsCorrect(enumMembers[3], ColorFlagEnum.Blue, "Blue");
            AssertEnumMemberIsCorrect(enumMembers[4], ColorFlagEnum.UltraViolet, "UltraViolet", new DescriptionAttribute("Ultra-Violet"));
            AssertEnumMemberIsCorrect(enumMembers[5], ColorFlagEnum.All, "All");
        }

        public void AssertEnumMemberIsCorrect<TEnum>(EnumMember<TEnum> member, TEnum value, string name, params Attribute[] attributes)
        {
            Assert.IsNotNull(member);
            Assert.AreEqual(value, member.Value);
            Assert.AreEqual(name, member.Name);
            CollectionAssert.AreEquivalent(attributes, member.Attributes);
        }

        [Test]
        public void GetNames()
        {
            CollectionAssert.AreEqual(new[] { "Black", "Red", "Green", "Blue", "UltraViolet", "All" }, GetNames<ColorFlagEnum>().ToArray());
            CollectionAssert.AreEqual(Enum.GetNames(typeof(DateFilterOperator)), GetNames<DateFilterOperator>().ToArray());
            CollectionAssert.AreEqual(new string[0], GetNames<ByteEnum>().ToArray());
        }

        [Test]
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

        [Test]
        public void GetValues_UniqueValued()
        {
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Black, ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet, ColorFlagEnum.All }, GetValues<ColorFlagEnum>(true).ToArray());
            CollectionAssert.AreEqual((DateFilterOperator[])Enum.GetValues(typeof(DateFilterOperator)), GetValues<DateFilterOperator>(true).ToArray());
            CollectionAssert.AreEqual(new ByteEnum[0], GetValues<ByteEnum>(true).ToArray());
            CollectionAssert.AreEqual(new[] { NumericFilterOperator.Is, NumericFilterOperator.IsNot, NumericFilterOperator.GreaterThan, NumericFilterOperator.LessThan, NumericFilterOperator.GreaterThanOrEqual, NumericFilterOperator.NotGreaterThan, NumericFilterOperator.Between, NumericFilterOperator.NotBetween }, GetValues<NumericFilterOperator>(true).ToArray());
        }

        [Test]
        public void GetDescriptionsAlternative()
        {
            CollectionAssert.AreEqual(new[] { null, null, null, null, "Ultra-Violet", null }, GetEnumMembers<ColorFlagEnum>().Select(member => member.Description).ToArray());
            CollectionAssert.AreEqual(new string[0], GetEnumMembers<ByteEnum>().Select(member => member.Description).ToArray());
        }

        [Test]
        public void GetAttributes1Alternative()
        {
            TestHelper.EnumerableOfEnumerablesAreEqual(new[] { new Attribute[0], new Attribute[0], new Attribute[0], new Attribute[0], new Attribute[] { new DescriptionAttribute("Ultra-Violet") }, new Attribute[0] }, GetEnumMembers<ColorFlagEnum>().Select(member => member.Attributes));
            TestHelper.EnumerableOfEnumerablesAreEqual(new Attribute[0][], GetEnumMembers<ByteEnum>().Select(member => member.Attributes));
        }

        [Test]
        public void GetAttributes2Alternative()
        {
            CollectionAssert.AreEqual(new[] { null, null, null, null, new DescriptionAttribute("Ultra-Violet"), null }, GetEnumMembers<ColorFlagEnum>().Select(member => member.GetAttribute<DescriptionAttribute>()).ToArray());
            CollectionAssert.AreEqual(new DescriptionAttribute[0], GetEnumMembers<ByteEnum>().Select(member => member.GetAttribute<DescriptionAttribute>()).ToArray());
        }
        #endregion

        #region IsValid
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

        [Test]
        public void IsDefined_ReturnsValidResults_WhenUsingValidName()
        {
            Assert.IsTrue(IsDefined<ColorFlagEnum>("UltraViolet"));
        }

        [Test]
        public void IsDefined_ReturnsFalse_WhenUsingInvalidName()
        {
            Assert.IsFalse(IsDefined<ColorFlagEnum>("ulTrAvioLet"));
        }

        [Test]
        public void IsDefined_ReturnsTrue_WhenUsingValidNameWhileIgnoringCase()
        {
            Assert.IsTrue(IsDefined<ColorFlagEnum>("ulTrAvioLet", true));
        }

        [Test]
        public void IsDefined_ReturnsTrue_WhenUsingValidNumber()
        {
            Assert.IsTrue(IsDefined<ColorFlagEnum>(1));
        }

        [Test]
        public void IsDefined_ReturnsTrue_WhenUsingUndefinedNumber()
        {
            Assert.IsFalse(IsDefined<ColorFlagEnum>(-1));
        }

        [Test]
        public void IsDefined_ReturnsFalse_WhenUsingLargeNumber()
        {
            Assert.IsFalse(IsDefined<ColorFlagEnum>(128));
        }
        #endregion

        #region IsInValueRange
        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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
        [Test]
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

        [Test]
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

        [Test]
        public void ToObject_ThrowsArgumentException_WhenUsingInvalidValueAndCheckIsOn()
        {
            TestHelper.ExpectException<ArgumentException>(() => ToObject<SByteEnum>(sbyte.MaxValue, true));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<ByteEnum>(byte.MaxValue, true));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<Int16Enum>(short.MaxValue, true));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<UInt16Enum>(ushort.MaxValue, true));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<Int32Enum>(int.MaxValue, true));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<UInt32Enum>(uint.MaxValue, true));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<Int64Enum>(long.MaxValue, true));
            TestHelper.ExpectException<ArgumentException>(() => ToObject<UInt64Enum>(ulong.MaxValue, true));
        }

        [Test]
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

        [Test]
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

        [Test]
        public void ToObjectOrDefault_ReturnsDefaultValue_WhenUsingValueInRangeButNotValid()
        {
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((sbyte)16, ColorFlagEnum.Blue, true));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((byte)16, ColorFlagEnum.Blue, true));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((short)16, ColorFlagEnum.Blue, true));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault((ushort)16, ColorFlagEnum.Blue, true));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(16, ColorFlagEnum.Blue, true));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(16U, ColorFlagEnum.Blue, true));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(16L, ColorFlagEnum.Blue, true));
            Assert.AreEqual(ColorFlagEnum.Blue, ToObjectOrDefault(16UL, ColorFlagEnum.Blue, true));
        }

        [Test]
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

        [Test]
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

        [Test]
        public void TryToObject_ReturnsFalse_WhenUsingValueInRangeButNotValid()
        {
            ColorFlagEnum result;
            Assert.IsFalse(TryToObject((sbyte)16, out result, true));
            Assert.IsFalse(TryToObject((byte)16, out result, true));
            Assert.IsFalse(TryToObject((short)16, out result, true));
            Assert.IsFalse(TryToObject((ushort)16, out result, true));
            Assert.IsFalse(TryToObject(16, out result, true));
            Assert.IsFalse(TryToObject(16U, out result, true));
            Assert.IsFalse(TryToObject(16L, out result, true));
            Assert.IsFalse(TryToObject(16UL, out result, true));
        }

        [Test]
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

        [Test]
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
        [Test]
        public void Validate()
        {
            Enums.Validate(NonContiguousEnum.Cat, "paramName");
            Enums.Validate(NonContiguousEnum.Dog, "paramName");
            Enums.Validate(NonContiguousEnum.Chimp, "paramName");
            Enums.Validate(NonContiguousEnum.Elephant, "paramName");
            Enums.Validate(NonContiguousEnum.Whale, "paramName");
            Enums.Validate(NonContiguousEnum.Eagle, "paramName");
            TestHelper.ExpectException<ArgumentException>(() => Enums.Validate((NonContiguousEnum)(-5), "paramName"));

            Enums.Validate(UInt64FlagEnum.Flies, "paramName");
            Enums.Validate(UInt64FlagEnum.Hops, "paramName");
            Enums.Validate(UInt64FlagEnum.Runs, "paramName");
            Enums.Validate(UInt64FlagEnum.Slithers, "paramName");
            Enums.Validate(UInt64FlagEnum.Stationary, "paramName");
            Enums.Validate(UInt64FlagEnum.Swims, "paramName");
            Enums.Validate(UInt64FlagEnum.Walks, "paramName");
            Enums.Validate(UInt64FlagEnum.Flies | UInt64FlagEnum.Hops, "paramName");
            Enums.Validate(UInt64FlagEnum.Flies | UInt64FlagEnum.Slithers, "paramName");
            TestHelper.ExpectException<ArgumentException>(() => Enums.Validate((UInt64FlagEnum)8, "paramName"));
            TestHelper.ExpectException<ArgumentException>(() => Enums.Validate((UInt64FlagEnum)8 | UInt64FlagEnum.Hops, "paramName"));

            Enums.Validate(ContiguousUInt64Enum.A, "paramName");
            Enums.Validate(ContiguousUInt64Enum.B, "paramName");
            Enums.Validate(ContiguousUInt64Enum.C, "paramName");
            Enums.Validate(ContiguousUInt64Enum.D, "paramName");
            Enums.Validate(ContiguousUInt64Enum.E, "paramName");
            Enums.Validate(ContiguousUInt64Enum.F, "paramName");
            TestHelper.ExpectException<ArgumentException>(() => Enums.Validate(ContiguousUInt64Enum.A - 1, "paramName"));
            TestHelper.ExpectException<ArgumentException>(() => Enums.Validate(ContiguousUInt64Enum.F + 1, "paramName"));

            Enums.Validate(NonContiguousUInt64Enum.SaintLouis, "paramName");
            Enums.Validate(NonContiguousUInt64Enum.Chicago, "paramName");
            Enums.Validate(NonContiguousUInt64Enum.Cincinnati, "paramName");
            Enums.Validate(NonContiguousUInt64Enum.Pittsburg, "paramName");
            Enums.Validate(NonContiguousUInt64Enum.Milwaukee, "paramName");
            TestHelper.ExpectException<ArgumentException>(() => Enums.Validate((NonContiguousUInt64Enum)5, "paramName"));
            TestHelper.ExpectException<ArgumentException>(() => Enums.Validate((NonContiguousUInt64Enum)50000000UL, "paramName"));

            Enums.Validate(NumericFilterOperator.Is, "paramName");
            Enums.Validate(NumericFilterOperator.IsNot, "paramName");
            Enums.Validate(NumericFilterOperator.GreaterThan, "paramName");
            Enums.Validate(NumericFilterOperator.LessThan, "paramName");
            Enums.Validate(NumericFilterOperator.GreaterThanOrEqual, "paramName");
            Enums.Validate(NumericFilterOperator.NotLessThan, "paramName");
            Enums.Validate(NumericFilterOperator.LessThanOrEqual, "paramName");
            Enums.Validate(NumericFilterOperator.NotGreaterThan, "paramName");
            Enums.Validate(NumericFilterOperator.Between, "paramName");
            Enums.Validate(NumericFilterOperator.NotBetween, "paramName");
            TestHelper.ExpectException<ArgumentException>(() => Enums.Validate(NumericFilterOperator.Is - 1, "paramName"));
            TestHelper.ExpectException<ArgumentException>(() => Enums.Validate(NumericFilterOperator.NotBetween + 1, "paramName"));
        }

        [Test]
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

        [Test]
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

        [Test]
        public void AsString_ThrowsFormatException_WhenUsingInvalidFormat()
        {
            TestHelper.ExpectException<FormatException>(() => ColorFlagEnum.Blue.AsString("a"));
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
            TestHelper.ExpectException<ArgumentNullException>(() => Format(ColorFlagEnum.Blue, (string)null));
        }

        [Test]
        public void Format_ThrowsFormatException_WhenUsingEmptyStringFormat()
        {
            TestHelper.ExpectException<FormatException>(() => Format(ColorFlagEnum.Blue, string.Empty));
        }

        [Test]
        public void Format_ThrowsFormatException_WhenUsingInvalidStringFormat()
        {
            TestHelper.ExpectException<FormatException>(() => Format(ColorFlagEnum.Blue, "a"));
        }

        [Test]
        public void Format_ReturnsExpected_WhenUsingCustomEnumFormat()
        {
            var enumMemberValueFormat = RegisterCustomEnumFormat(member => member.GetAttribute<EnumMemberAttribute>()?.Value);
            Assert.AreEqual("aye", Format(EnumMemberAttributeEnum.A, enumMemberValueFormat));
            var descriptionOrNameFormat = RegisterCustomEnumFormat(member => member.Description ?? member.Name);
            Assert.AreEqual("Ultra-Violet", Format(ColorFlagEnum.UltraViolet, descriptionOrNameFormat));
            Assert.AreEqual("Red", Format(ColorFlagEnum.Red, descriptionOrNameFormat));
        }

        [Test]
        public void GetUnderlyingValue_ReturnsExpected_OnAny()
        {
            Assert.AreEqual(2, GetUnderlyingValue(NumericFilterOperator.GreaterThan));
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

            // Check for main duplicates
            Assert.AreEqual("GreaterThanOrEqual", NumericFilterOperator.GreaterThanOrEqual.GetName());
            Assert.AreEqual("GreaterThanOrEqual", NumericFilterOperator.NotLessThan.GetName());
            Assert.AreEqual("NotGreaterThan", NumericFilterOperator.LessThanOrEqual.GetName());
            Assert.AreEqual("NotGreaterThan", NumericFilterOperator.NotGreaterThan.GetName());
        }

        [Test]
        public void GetDescription_ReturnsDescription_WhenUsingValidValueWithDescription()
        {
            Assert.AreEqual("Ultra-Violet", ColorFlagEnum.UltraViolet.GetDescription());
        }

        [Test]
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
