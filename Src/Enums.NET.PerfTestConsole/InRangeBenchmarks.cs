using BenchmarkDotNet.Attributes;

namespace EnumsNET.PerfTestConsole
{
    public class InRangeBenchmarks
    {
        [Benchmark]
        [Arguments(-2, 5)]
        public bool Int32_InRange1(int minValue, int maxValue)
        {
            var result = true;
            for (var i = -8; i <= 7; ++i)
            {
                result &= minValue <= i && maxValue >= i;
            }
            return result;
        }

        [Benchmark]
        [Arguments(-2, 5)]
        public bool Int32_InRange2(int minValue, int maxValue)
        {
            var result = true;
            for (var i = -8; i <= 7; ++i)
            {
                result &= (uint)(i - minValue) <= (uint)(maxValue - minValue);
            }
            return result;
        }

        [Benchmark]
        [Arguments(-2, 5)]
        public bool Int32_InRange3(int minValue, int maxValue)
        {
            var result = true;
            for (var i = -8; i <= 7; ++i)
            {
                result &= minValue <= i & maxValue >= i;
            }
            return result;
        }

        [Benchmark]
        [Arguments(-2, 7)]
        public bool Int32_InRange4(int minValue, int maxMinusMin)
        {
            var result = true;
            for (var i = -8; i <= 7; ++i)
            {
                result &= (uint)(i - minValue) <= (uint)maxMinusMin;
            }
            return result;
        }
    }
}