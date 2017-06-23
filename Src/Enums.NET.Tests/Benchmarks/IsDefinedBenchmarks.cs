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

#if BENCHMARKS
using System;
using BenchmarkDotNet.Attributes;
using EnumsNET.NonGeneric;

namespace EnumsNET.Tests.Benchmarks
{
    public class IsDefinedBenchmarks
    {
        private readonly DayOfWeek[] _values;
        private readonly DayOfWeek[] _undefinedValues;
        private readonly Type _enumType;

        public IsDefinedBenchmarks()
        {
            _enumType = typeof(DayOfWeek);
            _values = (DayOfWeek[])Enum.GetValues(_enumType);
            _undefinedValues = new DayOfWeek[7];
            for (var i = 1; i <= _undefinedValues.Length; ++i)
            {
                _undefinedValues[i - 1] = DayOfWeek.Saturday + i;
            }
        }

        [Benchmark]
        public bool Enum_IsDefined_True()
        {
            var result = false;
            foreach (var value in _values)
            {
                result |= Enum.IsDefined(_enumType, value);
            }
            return result;
        }

        [Benchmark]
        public bool Enum_IsDefined_False()
        {
            var result = false;
            foreach (var value in _undefinedValues)
            {
                result |= Enum.IsDefined(_enumType, value);
            }
            return result;
        }

        [Benchmark]
        public bool NonGenericEnums_IsDefined_True()
        {
            var result = false;
            foreach (var value in _values)
            {
                result |= NonGenericEnums.IsDefined(_enumType, value);
            }
            return result;
        }

        [Benchmark]
        public bool NonGenericEnums_IsDefined_False()
        {
            var result = false;
            foreach (var value in _undefinedValues)
            {
                result |= NonGenericEnums.IsDefined(_enumType, value);
            }
            return result;
        }

        [Benchmark]
        public bool Enums_IsDefined_True()
        {
            var result = false;
            foreach (var value in _values)
            {
                result |= value.IsDefined();
            }
            return result;
        }

        [Benchmark]
        public bool Enums_IsDefined_False()
        {
            var result = false;
            foreach (var value in _undefinedValues)
            {
                result |= value.IsDefined();
            }
            return result;
        }

        [Benchmark]
        public bool Enums_IsValid_True()
        {
            var result = false;
            foreach (var value in _values)
            {
                result |= value.IsValid();
            }
            return result;
        }

        [Benchmark]
        public bool Enums_IsValid_False()
        {
            var result = false;
            foreach (var value in _undefinedValues)
            {
                result |= value.IsValid();
            }
            return result;
        }
    }
}
#endif