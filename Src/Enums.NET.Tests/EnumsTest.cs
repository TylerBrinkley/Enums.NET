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
                .SelectMany(type => type.
#if TYPE_REFLECTION
                        GetMethods(BindingFlags.Static | BindingFlags.Public)
#else
                        GetTypeInfo().DeclaredMethods.Where(method => method.IsPublic)
#endif
                )
                .Where(method => method.IsGenericMethod && ((method.Name != nameof(FlagEnums.GetFlags) && method.Name != nameof(FlagEnums.GetFlagMembers)) || method.GetParameters()[0].Name != "member"))
                .Select(method => method.GetGenericArguments()[0])
                .Where(genericArg => genericArg != null)
                .Concat(new[] { typeof(EnumComparer<>).GetGenericArguments()[0], typeof(IEnumValidatorAttribute<>).GetGenericArguments()[0] });
            foreach (var enumTypeArg in enumTypeArgs)
            {
                Assert.IsTrue(enumTypeArg.GetGenericParameterConstraints().Any(genericParamConstraint => genericParamConstraint == typeof(Enum)));
            }
        }

        #region Type Methods
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
        }
#endif

        [Test]
        public void GetEnumMemberCount()
        {
            Assert.AreEqual(0, GetEnumMemberCount<ByteEnum>());
            Assert.AreEqual(38, GetEnumMemberCount<DateFilterOperator>());
            Assert.AreEqual(6, GetEnumMemberCount<ColorFlagEnum>());
            Assert.AreEqual(10, GetEnumMemberCount<NumericOperator>());

            Assert.AreEqual(0, GetEnumMemberCount<ByteEnum>(true));
            Assert.AreEqual(38, GetEnumMemberCount<DateFilterOperator>(true));
            Assert.AreEqual(6, GetEnumMemberCount<ColorFlagEnum>(true));
            Assert.AreEqual(8, GetEnumMemberCount<NumericOperator>(true)); // Has 2 duplicates
        }

        [Test]
        public void GetEnumMembers()
        {
            Assert.AreEqual(0, GetEnumMembers<ByteEnum>().Count());
            Assert.AreEqual(38, GetEnumMembers<DateFilterOperator>().Count());
            Assert.AreEqual(6, GetEnumMembers<ColorFlagEnum>().Count());
            Assert.AreEqual(10, GetEnumMembers<NumericOperator>().Count());

            Assert.AreEqual(0, GetEnumMembers<ByteEnum>(true).Count());
            Assert.AreEqual(38, GetEnumMembers<DateFilterOperator>(true).Count());
            Assert.AreEqual(6, GetEnumMembers<ColorFlagEnum>(true).Count());
            Assert.AreEqual(8, GetEnumMembers<NumericOperator>(true).Count()); // Has 2 duplicates

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
            CollectionAssert.AreEqual(new[] { "Black", "Red", "Green", "Blue", "UltraViolet", "All" }, GetNames<ColorFlagEnum>());
            CollectionAssert.AreEqual(Enum.GetNames(typeof(DateFilterOperator)), GetNames<DateFilterOperator>());
            CollectionAssert.AreEqual(new string[0], GetNames<ByteEnum>());
        }

        [Test]
        public void GetValues()
        {
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Black, ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet, ColorFlagEnum.All }, GetValues<ColorFlagEnum>());
            CollectionAssert.AreEqual((DateFilterOperator[])Enum.GetValues(typeof(DateFilterOperator)), GetValues<DateFilterOperator>());
            CollectionAssert.AreEqual(new ByteEnum[0], GetValues<ByteEnum>());

            // Duplicate order check
            var numericFilterOperators = GetValues<NumericOperator>().ToArray();
            for (var i = 1; i < numericFilterOperators.Length; ++i)
            {
                Assert.IsTrue(numericFilterOperators[i - 1] <= numericFilterOperators[i]);
            }
        }

        [Test]
        public void GetValues_ExcludeDuplicates()
        {
            CollectionAssert.AreEqual(new[] { ColorFlagEnum.Black, ColorFlagEnum.Red, ColorFlagEnum.Green, ColorFlagEnum.Blue, ColorFlagEnum.UltraViolet, ColorFlagEnum.All }, GetValues<ColorFlagEnum>(true));
            CollectionAssert.AreEqual((DateFilterOperator[])Enum.GetValues(typeof(DateFilterOperator)), GetValues<DateFilterOperator>(true));
            CollectionAssert.AreEqual(new ByteEnum[0], GetValues<ByteEnum>(true));
            CollectionAssert.AreEqual(new[] { NumericOperator.Equals, NumericOperator.NotEquals, NumericOperator.GreaterThan, NumericOperator.LessThan, NumericOperator.GreaterThanOrEquals, NumericOperator.NotGreaterThan, NumericOperator.Between, NumericOperator.NotBetween }, GetValues<NumericOperator>(true));
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
        }

        [Test]
        public void ToObject_ThrowsOverflowException_WhenUsingValuesOutsideValueRange()
        {
            TestHelper.ExpectException<OverflowException>(() => ToObject<SByteEnum>(byte.MaxValue));
            TestHelper.ExpectException<OverflowException>(() => ToObject<ByteEnum>(sbyte.MinValue));
            TestHelper.ExpectException<OverflowException>(() => ToObject<Int16Enum>(ushort.MaxValue));
            TestHelper.ExpectException<OverflowException>(() => ToObject<UInt16Enum>(short.MinValue));
            TestHelper.ExpectException<OverflowException>(() => ToObject<Int32Enum>(uint.MaxValue));
            TestHelper.ExpectException<OverflowException>(() => ToObject<UInt32Enum>(int.MinValue));
            TestHelper.ExpectException<OverflowException>(() => ToObject<Int64Enum>(ulong.MaxValue));
            TestHelper.ExpectException<OverflowException>(() => ToObject<UInt64Enum>(long.MinValue));
        }

        [Test]
        public void ToObject_ThrowsArgumentException_WhenUsingInvalidValueWithValidation()
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
        }

        [Test]
        public void TryToObject_ReturnsFalse_WhenUsingValueInRangeButNotValid()
        {
            ColorFlagEnum result;
            Assert.IsFalse(TryToObject((sbyte)16, true, out result));
            Assert.IsFalse(TryToObject((byte)16, true, out result));
            Assert.IsFalse(TryToObject((short)16, true, out result));
            Assert.IsFalse(TryToObject((ushort)16, true, out result));
            Assert.IsFalse(TryToObject(16, true, out result));
            Assert.IsFalse(TryToObject(16U, true, out result));
            Assert.IsFalse(TryToObject(16L, true, out result));
            Assert.IsFalse(TryToObject(16UL, true, out result));
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
            TestHelper.ExpectException<ArgumentException>(() => (NumericOperator.Equals - 1).Validate("paramName"));
            TestHelper.ExpectException<ArgumentException>(() => (NumericOperator.NotBetween + 1).Validate("paramName"));
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
            // Custom enum member formatter
            var descriptionOrNameFormat = RegisterCustomEnumFormat(member => member.AsString(EnumFormat.Description) ?? member.Name);
            Assert.IsTrue(descriptionOrNameFormat.IsValid());
            Assert.IsFalse((descriptionOrNameFormat + 2).IsValid());
            Assert.AreEqual("Ultra-Violet", Format(ColorFlagEnum.UltraViolet, descriptionOrNameFormat));
            Assert.AreEqual(nameof(ColorFlagEnum.Red), Format(ColorFlagEnum.Red, descriptionOrNameFormat));
#if !NET20
            Assert.AreEqual(nameof(EnumMemberAttributeEnum.B), Format(EnumMemberAttributeEnum.B, descriptionOrNameFormat));
#endif
        }

        [Test]
        public void GetUnderlyingValue_ReturnsExpected_OnAny()
        {
            Assert.AreEqual(2, GetUnderlyingValue(NumericOperator.GreaterThan));
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
        }
        #endregion

        #region Attributes
        [Test]
        public void HasAttribute()
        {
#if !NET20
            Assert.IsTrue(EnumMemberAttributeEnum.A.GetEnumMember().HasAttribute<EnumMemberAttribute>());
#endif
            Assert.IsFalse(ColorFlagEnum.Blue.GetEnumMember().HasAttribute<DescriptionAttribute>());
            Assert.IsTrue(ColorFlagEnum.UltraViolet.GetEnumMember().HasAttribute<DescriptionAttribute>());
        }

        [Test]
        public void GetAttribute()
        {
#if !NET20
            Assert.AreEqual("aye", EnumMemberAttributeEnum.A.GetEnumMember().GetAttribute<EnumMemberAttribute>().Value);
#endif
            Assert.IsNull(ColorFlagEnum.Blue.GetEnumMember().GetAttribute<DescriptionAttribute>());
            Assert.AreEqual("Ultra-Violet", ColorFlagEnum.UltraViolet.GetEnumMember().GetAttribute<DescriptionAttribute>().Description);
        }

        [Test]
        public void GetAttributes()
        {
            CollectionAssert.AreEquivalent(new OptionAttribute[0], MultipleAttributeEnum.None.GetEnumMember().GetAttributes<OptionAttribute>());
            CollectionAssert.AreEquivalent(new[] { new OptionAttribute("Mono") }, MultipleAttributeEnum.Single.GetEnumMember().GetAttributes<OptionAttribute>());
            CollectionAssert.AreEquivalent(new[] { new OptionAttribute("Poly"), new OptionAttribute("Plural") }, MultipleAttributeEnum.Multi.GetEnumMember().GetAttributes<OptionAttribute>());
        }

        [Test]
        public void GetAllAttributes()
        {
            CollectionAssert.AreEquivalent(new Attribute[0], MultipleAttributeEnum.None.GetEnumMember().Attributes);
            CollectionAssert.AreEquivalent(new Attribute[] { new OptionAttribute("Mono"), new DescriptionAttribute("One") }, MultipleAttributeEnum.Single.GetEnumMember().Attributes);
            CollectionAssert.AreEquivalent(new Attribute[] { new DescriptionAttribute("Many"), new OptionAttribute("Poly"), new OptionAttribute("Plural") }, MultipleAttributeEnum.Multi.GetEnumMember().Attributes);
        }
#endregion

        // TODO
        #region Parsing
        #endregion
    }
}
