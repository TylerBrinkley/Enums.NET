#region License
// Copyright (c) 2016 Tyler Brinkley
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace EnumsNET.Tests.Benchmarks
{
    public class ParseBenchmarks
    {
        private class EnumData
        {
            public Type Type { get; set; }

            public string[] Names { get; set; }

            public string[] NumericValues { get; set; }

            public Parser GenericParser { get; set; }
        }

        private abstract class Parser
        {
            public abstract void Parse(string value);
        }

        private sealed class Parser<TEnum> : Parser
            where TEnum : struct, Enum
        {
            public override void Parse(string value) => Enums.Parse<TEnum>(value);
        }

        private readonly EnumData[] _enumDatas;

        public ParseBenchmarks()
        {
            var enumTypes = typeof(string).GetTypeInfo().Assembly.GetTypes().Where(type => type.GetTypeInfo().IsEnum && !type.GetTypeInfo().IsGenericType).ToList();
            _enumDatas = new EnumData[enumTypes.Count];
            for (var i = 0; i < _enumDatas.Length; ++i)
            {
                var enumType = enumTypes[i];
                var names = Enum.GetNames(enumType);
                var numericValues = new string[names.Length];
                var values = Enum.GetValues(enumType);
                for (var j = 0; j < values.Length; ++j)
                {
                    numericValues[j] = Enum.Format(enumType, values.GetValue(j), "D");
                }
                var parser = (Parser)Activator.CreateInstance(typeof(Parser<>).MakeGenericType(enumType));
                _enumDatas[i] = new EnumData { Type = enumType, Names = names, NumericValues = numericValues, GenericParser = parser };
                if (names.Length > 0)
                {
                    Enums.Parse(enumType, names[0]); // Warmup
                    parser.Parse(names[0]); // Warmup
                }
            }
        }

        [Benchmark(Baseline = true)]
        public object Enum_Parse_Names()
        {
            object result = null;
            foreach (var enumData in _enumDatas)
            {
                var enumType = enumData.Type;
                foreach (var value in enumData.Names)
                {
                    result = Enum.Parse(enumType, value);
                }
            }
            return result;
        }

        [Benchmark]
        public object NonGenericEnums_Parse_Names()
        {
            object result = null;
            foreach (var enumData in _enumDatas)
            {
                var enumType = enumData.Type;
                foreach (var value in enumData.Names)
                {
                    result = Enums.Parse(enumType, value);
                }
            }
            return result;
        }

        [Benchmark]
        public int Enums_Parse_Names()
        {
            var i = 0;
            foreach (var enumData in _enumDatas)
            {
                var genericParser = enumData.GenericParser;
                foreach (var value in enumData.Names)
                {
                    genericParser.Parse(value);
                    ++i;
                }
            }
            return i;
        }

        [Benchmark]
        public object Enum_Parse_Decimals()
        {
            object result = null;
            foreach (var enumData in _enumDatas)
            {
                var enumType = enumData.Type;
                foreach (var value in enumData.NumericValues)
                {
                    result = Enum.Parse(enumType, value);
                }
            }
            return result;
        }

        [Benchmark]
        public object NonGenericEnums_Parse_Decimals()
        {
            object result = null;
            foreach (var enumData in _enumDatas)
            {
                var enumType = enumData.Type;
                foreach (var value in enumData.NumericValues)
                {
                    result = Enums.Parse(enumType, value);
                }
            }
            return result;
        }

        [Benchmark]
        public int Enums_Parse_Decimals()
        {
            var i = 0;
            foreach (var enumData in _enumDatas)
            {
                var genericParser = enumData.GenericParser;
                foreach (var value in enumData.NumericValues)
                {
                    genericParser.Parse(value);
                    ++i;
                }
            }
            return i;
        }
    }
}