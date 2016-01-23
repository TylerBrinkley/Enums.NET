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
            var namesSize = 0;
            var valuesSize = 0;
            var attributesSize = 0;
            using (new OperationTimer("All Available Enums Caching Performance"))
            {
                foreach (var enumType in enumTypes)
                {
                    namesSize += NonGeneric.NonGenericEnums.GetNames(enumType).Sum(name => name.Length << 1);
                    int typeSize = 0;
                    switch (NonGeneric.NonGenericEnums.GetTypeCode(enumType))
                    {
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                            typeSize = 1;
                            break;
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                            typeSize = 2;
                            break;
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                            typeSize = 4;
                            break;
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                            typeSize = 8;
                            break;
                    }
                    valuesSize += NonGeneric.NonGenericEnums.GetDefinedCount(enumType) * typeSize;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            Console.WriteLine(enumTypes.Count);
            Console.WriteLine(namesSize + valuesSize);

            using (new OperationTimer("Enum.IsDefined Performance"))
            {
                var dayOfWeekType = typeof(DayOfWeek);
                for (var i = 0; i < 1000000; ++i)
                {
                    Enum.IsDefined(dayOfWeekType, (DayOfWeek)(i % 14));
                }
            }

            using (new OperationTimer("Enums.IsDefined Performance"))
            {
                for (var i = 0; i < 1000000; ++i)
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
            Console.WriteLine("{0} (GCs={1,3}) (MemUsed={2}) {3}", _stopwatch.Elapsed,
               GC.CollectionCount(0) - _collectionCount, Process.GetCurrentProcess().PagedMemorySize64 - _privateMemorySize64, _text);
        }

        private static void PrepareForOperation()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
