using System;
using System.Diagnostics;
using EnumsNET.NonGeneric;
using System.Collections.Generic;

#if !NET20
using System.Linq;
#endif

namespace EnumsNET.PerfTestConsole
{
    class Program
    {
        private class EnumInfo
        {
            public Type Type { get; set; }

            public List<string> Names { get; set; }

            public List<object> Values { get; set; }

            public List<string> NumericValues { get; set; }
        }

        static void Main()
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

            Parse(enumTypes);
            
            var dayOfWeekArray = new DayOfWeek[14];
            for (var i = 0; i < dayOfWeekArray.Length; ++i)
            {
                dayOfWeekArray[i] = (DayOfWeek)i;
            }

            ToString(dayOfWeekArray);

            IsDefined(dayOfWeekArray);

            GetHashCode(dayOfWeekArray);

            Console.ReadLine();
        }

        public static void Parse(IEnumerable<Type> enumTypes)
        {
            var list = new List<EnumInfo>();
            foreach (var enumType in enumTypes)
            {
                var names = Enum.GetNames(enumType).ToList();
                var values = new object[names.Count];
                Enum.GetValues(enumType).CopyTo(values, 0);
                var numericValues = new List<string>(names.Count);
                foreach (var value in values)
                {
                    numericValues.Add(Enum.Format(enumType, value, "D"));
                }
                list.Add(new EnumInfo { Type = enumType, Names = names, Values = values.ToList(), NumericValues = numericValues });
            }

            const int parseIterations = 10000;

            using (new OperationTimer("Enum.Parse Names"))
            {
                foreach (var tuple in list)
                {
                    var enumType = tuple.Type;
                    for (var i = 0; i < parseIterations; ++i)
                    {
                        foreach (var name in tuple.Names)
                        {
                            Enum.Parse(enumType, name);
                        }
                    }
                }
            }

            using (new OperationTimer("NonGenericEnums.Parse Names"))
            {
                foreach (var tuple in list)
                {
                    var enumType = tuple.Type;
                    for (var i = 0; i < parseIterations; ++i)
                    {
                        foreach (var name in tuple.Names)
                        {
                            NonGenericEnums.Parse(enumType, name);
                        }
                    }
                }
            }

            using (new OperationTimer("Enum.Parse Decimal Values"))
            {
                foreach (var tuple in list)
                {
                    var enumType = tuple.Type;
                    for (var i = 0; i < parseIterations; ++i)
                    {
                        foreach (var numericValue in tuple.NumericValues)
                        {
                            Enum.Parse(enumType, numericValue);
                        }
                    }
                }
            }

            using (new OperationTimer("NonGenericEnums.Parse Decimal Values"))
            {
                foreach (var tuple in list)
                {
                    var enumType = tuple.Type;
                    for (var i = 0; i < parseIterations; ++i)
                    {
                        foreach (var numericValue in tuple.NumericValues)
                        {
                            NonGenericEnums.Parse(enumType, numericValue);
                        }
                    }
                }
            }
        }

        public static void ToString(DayOfWeek[] dayOfWeekArray)
        {
            const int iterations = 1000000;

            using (new OperationTimer("Enum.ToString"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        dayOfWeekArray[j].ToString();
                    }
                }
            }

            var dayOfWeekType = typeof(DayOfWeek);

            using (new OperationTimer("NonGenericEnums.AsString"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        NonGenericEnums.AsString(dayOfWeekType, dayOfWeekArray[j]);
                    }
                }
            }

            using (new OperationTimer("Enums.AsString"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        dayOfWeekArray[j].AsString();
                    }
                }
            }

            using (new OperationTimer("Enum.ToString Decimal"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        dayOfWeekArray[j].ToString("D");
                    }
                }
            }

            using (new OperationTimer("NonGenericEnums.AsString Decimal"))
            {
                var decimalValueArray = new[] { EnumFormat.DecimalValue };
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        NonGenericEnums.AsString(dayOfWeekType, dayOfWeekArray[j], decimalValueArray);
                    }
                }
            }

            using (new OperationTimer("Enums.AsString Decimal"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        dayOfWeekArray[j].AsString(EnumFormat.DecimalValue);
                    }
                }
            }
        }

        public static void IsDefined(DayOfWeek[] dayOfWeekArray)
        {
            const int iterations = 10000000;

            var dayOfWeekType = typeof(DayOfWeek);

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

            using (new OperationTimer("NonGenericEnums.IsDefined Performance"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        NonGenericEnums.IsDefined(dayOfWeekType, dayOfWeekArray[j]);
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
        }

        public static void GetHashCode(DayOfWeek[] dayOfWeekArray)
        {
            const int iterations = 10000000;

            using (new OperationTimer("Enum.GetHashCode Performance"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        dayOfWeekArray[j].GetHashCode();
                    }
                }
            }

            using (new OperationTimer("Enums.GetHashCode Performance"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        Enums.GetHashCode(dayOfWeekArray[j]);
                    }
                }
            }
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

#if NET20
namespace EnumsNET
{
    internal static class Enumerable
    {
        public static IEnumerable<TResult> SelectMany<T, TResult>(this IEnumerable<T> source, Func<T, IEnumerable<TResult>> selector)
        {
            foreach (var item in source)
            {
                foreach (var selectedItem in selector(item))
                {
                    yield return selectedItem;
                }
            }
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public static List<T> ToList<T>(this IEnumerable<T> source) => new List<T>(source);
    }
}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    internal class ExtensionAttribute : Attribute
    {
    }
}
#endif