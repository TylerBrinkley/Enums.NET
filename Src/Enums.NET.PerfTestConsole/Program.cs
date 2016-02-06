using System;
using System.Diagnostics;
using System.Linq;

namespace EnumsNET.PerfTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var enumTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsEnum && !type.IsGenericType).ToList();
            using (new OperationTimer("All Available Enums Caching Performance"))
            {
                foreach (var enumType in enumTypes)
                {
                    NonGeneric.NonGenericEnums.IsContiguous(enumType);
                }
            }
            Console.WriteLine(enumTypes.Count);

            const int iterations = 100000000;

            var dayOfWeekType = typeof(DayOfWeek);
            using (new OperationTimer("Enum.IsDefined Performance"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    Enum.IsDefined(dayOfWeekType, (DayOfWeek)(i % 14));
                }
            }

            using (new OperationTimer("NonGenericEnums.IsDefined Performance"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    NonGeneric.NonGenericEnums.IsDefined(dayOfWeekType, (DayOfWeek)(i % 14));
                }
            }

            using (new OperationTimer("Enums.IsDefined Performance"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    ((DayOfWeek)(i % 14)).IsDefined();
                }
            }

            Console.ReadLine();
        }
    }

    // This class is useful for doing operation performance timing
    internal sealed class OperationTimer : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _text;
        private readonly int _collectionCount;
        private readonly long _privateMemorySize64;

        public OperationTimer(string text)
        {
            PrepareForOperation();

            _text = text;
            _collectionCount = GC.CollectionCount(0);
            _privateMemorySize64 = Process.GetCurrentProcess().PagedMemorySize64;

            // This should be the last statement in this
            // method to keep timing as accurate as possible
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            var elapsed = _stopwatch.Elapsed;
            var collectionCount = GC.CollectionCount(0);
            var currentProcess = Process.GetCurrentProcess();
            var priorMemorySize64 = currentProcess.PagedMemorySize64;
            PrepareForOperation();
            var afterMemorySize64 = currentProcess.PagedMemorySize64;
            Console.WriteLine("{0} (GCs={1,3}) (MemUsed={2}) (MemFreedOnLastGC={3}) {4}", elapsed,
               collectionCount - _collectionCount, afterMemorySize64 - _privateMemorySize64, priorMemorySize64 - afterMemorySize64, _text);
        }

        private static void PrepareForOperation()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
