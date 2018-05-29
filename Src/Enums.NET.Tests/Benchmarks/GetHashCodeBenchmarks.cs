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
using BenchmarkDotNet.Attributes.Jobs;

namespace EnumsNET.Tests.Benchmarks
{
    [ClrJob, CoreJob]
    public class GetHashCodeBenchmarks
    {
        private readonly DayOfWeek[] _values;
        private readonly Type _enumType;

        public GetHashCodeBenchmarks()
        {
            _enumType = typeof(DayOfWeek);
            _values = (DayOfWeek[])Enum.GetValues(_enumType);
        }

        [Benchmark]
        public int Enum_GetHashCode()
        {
            var result = 0;
            foreach (var value in _values)
            {
                result = value.GetHashCode();
            }
            return result;
        }

        [Benchmark]
        public int Enums_GetHashCode()
        {
            var result = 0;
            foreach (var value in _values)
            {
                result = Enums.GetHashCode(value);
            }
            return result;
        }
    }
}
#endif