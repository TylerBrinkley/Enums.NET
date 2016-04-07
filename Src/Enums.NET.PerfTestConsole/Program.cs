using System;
using System.Diagnostics;
using System.Linq;
using EnumsNET.NonGeneric;

namespace EnumsNET.PerfTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var enumTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsEnum && !type.IsGenericType).ToList();
            //var methodInfo = typeof(Enums).GetMethod("IsContiguous");
            using (new OperationTimer("All Available Enums Caching Performance"))
            {
                foreach (var enumType in enumTypes)
                {
                    //methodInfo.MakeGenericMethod(enumType).Invoke(null, null);
                    NonGenericEnums.IsContiguous(enumType);
                }
            }
            Console.WriteLine(enumTypes.Count);

            const int iterations = 10000000;

            var dayOfWeekType = typeof(DayOfWeek);
            var dayOfWeekArray = new DayOfWeek[14];
            var dayOfWeekObjectArray = new object[dayOfWeekArray.Length];
            for (var i = 0; i < dayOfWeekArray.Length; ++i)
            {
                dayOfWeekObjectArray[i] = dayOfWeekArray[i] = (DayOfWeek)i;
            }
            using (new OperationTimer("Enum.IsDefined Performance"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        Enum.IsDefined(dayOfWeekType, dayOfWeekArray[j]);
                    }
                }
            }

            using (new OperationTimer("Enums.IsDefined Performance"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        dayOfWeekArray[j].IsDefined();
                    }
                }
            }

            using (new OperationTimer("Enum.IsDefined Performance"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekObjectArray.Length; ++j)
                    {
                        Enum.IsDefined(dayOfWeekType, dayOfWeekObjectArray[j]);
                    }
                }
            }

            using (new OperationTimer("NonGenericEnums.IsDefined Performance"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekObjectArray.Length; ++j)
                    {
                        NonGenericEnums.IsDefined(dayOfWeekType, dayOfWeekObjectArray[j]);
                    }
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
