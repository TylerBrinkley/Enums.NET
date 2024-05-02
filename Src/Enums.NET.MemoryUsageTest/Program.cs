using System;
using System.Diagnostics;
using System.Linq;
using EnumsNET;
using EnumsNET.MemoryUsageTest;

var enumTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsEnum && !type.IsGenericType).ToList();
var methodInfo = typeof(Enums).GetMethod("GetUnderlyingType", []);
var genericMethods = enumTypes.Select(enumType => methodInfo.MakeGenericMethod(enumType)).ToList();
using (new OperationTimer("All Available Enums Caching Performance"))
{
    foreach (var genericMethod in genericMethods)
    //foreach (var enumType in enumTypes)
    {
        genericMethod.Invoke(null, null);
        //Enums.GetUnderlyingType(enumType);
    }
}
Console.WriteLine(enumTypes.Count);
Console.ReadLine();

namespace EnumsNET.MemoryUsageTest
{
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
            Console.WriteLine($"{elapsed} (GCs={collectionCount - _collectionCount,3}) (MemUsed={afterMemorySize64 - _privateMemorySize64}) (MemFreedOnLastGC={priorMemorySize64 - afterMemorySize64}) {_text}");
        }

        private static void PrepareForOperation()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}