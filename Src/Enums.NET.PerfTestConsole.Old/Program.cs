﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EnumsNET.NonGeneric;
using EnumsNET.Unsafe;

namespace EnumsNET.PerfTestConsole
{
    static class Program
    {
        private class EnumInfo
        {
            public Type Type { get; set; }

            public List<string> Names { get; set; }

            public List<string> NumericValues { get; set; }

            public Parser GenericParser { get; set; }
        }

        static void Main()
        {
            var enumTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsEnum && !type.IsGenericType).ToList();
            //var methodInfo = typeof(Enums).GetMethod("GetUnderlyingType");
            using (new OperationTimer("All Available Enums Caching Performance"))
            {
                foreach (var enumType in enumTypes)
                {
                    //methodInfo.MakeGenericMethod(enumType).Invoke(null, null);
                    NonGenericEnums.GetUnderlyingType(enumType);
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

            var attributeTargetsArray = new AttributeTargets[15];
            attributeTargetsArray[0] = (AttributeTargets)0;
            attributeTargetsArray[1] = (AttributeTargets)1;
            for (var i = 2; i < attributeTargetsArray.Length; ++i)
            {
                attributeTargetsArray[i] = (AttributeTargets)(1 << (i - 1)) | (AttributeTargets)(1 << (i - 2));
            }
            var allAttributeTargets = (AttributeTargets[])Enum.GetValues(typeof(AttributeTargets));

            HasFlag(attributeTargetsArray, allAttributeTargets);

            Console.ReadLine();
        }

        public static void Parse(IEnumerable<Type> enumTypes)
        {
            var list = new List<EnumInfo>();
            foreach (var enumType in enumTypes)
            {
                var names = Enum.GetNames(enumType).ToList();
                var numericValues = new List<string>(names.Count);
                foreach (var value in Enum.GetValues(enumType))
                {
                    numericValues.Add(Enum.Format(enumType, value, "D"));
                }
                var parser = (Parser)Activator.CreateInstance(typeof(Parser<>).MakeGenericType(enumType));
                list.Add(new EnumInfo { Type = enumType, Names = names, NumericValues = numericValues, GenericParser = parser });
            }

            const int parseIterations = 375;

            using (new OperationTimer("Enum.Parse (Names)"))
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

            // Primes the pump
            foreach (var tuple in list)
            {
                if (tuple.Names.Count > 0)
                {
                    NonGenericEnums.Parse(tuple.Type, tuple.Names[0]);
                }
            }

            using (new OperationTimer("NonGenericEnums.Parse (Names)"))
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

            // Primes the pump
            foreach (var tuple in list)
            {
                if (tuple.Names.Count > 0)
                {
                    tuple.GenericParser.Parse(tuple.Names[0]);
                }
            }

            using (new OperationTimer("UnsafeEnums.Parse (Names)"))
            {
                foreach (var tuple in list)
                {
                    var parser = tuple.GenericParser;
                    for (var i = 0; i < parseIterations; ++i)
                    {
                        foreach (var name in tuple.Names)
                        {
                            parser.Parse(name);
                        }
                    }
                }
            }

            using (new OperationTimer("Enum.Parse (Decimal)"))
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

            using (new OperationTimer("NonGenericEnums.Parse (Decimal)"))
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

            using (new OperationTimer("UnsafeEnums.Parse (Decimal)"))
            {
                foreach (var tuple in list)
                {
                    var parser = tuple.GenericParser;
                    for (var i = 0; i < parseIterations; ++i)
                    {
                        foreach (var numericValue in tuple.NumericValues)
                        {
                            parser.Parse(numericValue);
                        }
                    }
                }
            }
        }

        public abstract class Parser
        {
            public abstract void Parse(string value);
        }

        public sealed class Parser<TEnum> : Parser
        {
            public override void Parse(string value)
            {
                UnsafeEnums.Parse<TEnum>(value);
            }
        }

        public static void ToString(DayOfWeek[] dayOfWeekArray)
        {
            const int iterations = 50000;

            using (new OperationTimer("Enum.ToString (Name)"))
            {
                for (var i = 0; i < (iterations << 1); ++i)
                {
                    for (var j = 0; j < 7; ++j)
                    {
                        dayOfWeekArray[j].ToString();
                    }
                }
            }

            var dayOfWeekType = typeof(DayOfWeek);

            using (new OperationTimer("NonGenericEnums.AsString (Name)"))
            {
                for (var i = 0; i < (iterations << 1); ++i)
                {
                    for (var j = 0; j < 7; ++j)
                    {
                        NonGenericEnums.AsString(dayOfWeekType, dayOfWeekArray[j]);
                    }
                }
            }

            using (new OperationTimer("Enums.AsString (Name)"))
            {
                for (var i = 0; i < (iterations << 1); ++i)
                {
                    for (var j = 0; j < 7; ++j)
                    {
                        dayOfWeekArray[j].AsString();
                    }
                }
            }

            using (new OperationTimer("Enum.ToString (Decimal)"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        dayOfWeekArray[j].ToString("D");
                    }
                }
            }

            using (new OperationTimer("NonGenericEnums.AsString (Decimal)"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        NonGenericEnums.AsString(dayOfWeekType, dayOfWeekArray[j], EnumFormat.DecimalValue);
                    }
                }
            }

            using (new OperationTimer("Enums.AsString (Decimal)"))
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
            const int iterations = 250000;

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

            using (new OperationTimer("Enums.IsValid Performance"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    for (var j = 0; j < dayOfWeekArray.Length; ++j)
                    {
                        dayOfWeekArray[j].IsValid();
                    }
                }
            }
        }

        public static void GetHashCode(DayOfWeek[] dayOfWeekArray)
        {
            const int iterations = 5000000;

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

        private static void HasFlag(AttributeTargets[] attributeTargetsArray, AttributeTargets[] allAttributeTargets)
        {
            const int iterations = 160000;

#if !NET20 && !NET35
            using (new OperationTimer("Enum.HasFlag"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    foreach (var attributeTargets in attributeTargetsArray)
                    {
                        foreach (var otherAttributeTargets in allAttributeTargets)
                        {
                            attributeTargets.HasFlag(otherAttributeTargets);
                        }
                    }
                }
            }
#endif

            var attributeTargetsType = typeof(AttributeTargets);

            using (new OperationTimer("NonGenericFlagEnums.HasAllFlags"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    foreach (var attributeTargets in attributeTargetsArray)
                    {
                        foreach (var otherAttributeTargets in allAttributeTargets)
                        {
                            NonGenericFlagEnums.HasAllFlags(attributeTargetsType, attributeTargets, otherAttributeTargets);
                        }
                    }
                }
            }

            using (new OperationTimer("Enums.HasAllFlags"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    foreach (var attributeTargets in attributeTargetsArray)
                    {
                        foreach (var otherAttributeTargets in allAttributeTargets)
                        {
                            attributeTargets.HasAllFlags(otherAttributeTargets);
                        }
                    }
                }
            }

            var temp = false;
            using (new OperationTimer("Manual HasAllFlags"))
            {
                for (var i = 0; i < iterations; ++i)
                {
                    foreach (var attributeTargets in attributeTargetsArray)
                    {
                        foreach (var otherAttributeTargets in allAttributeTargets)
                        {
                            temp |= attributeTargets.ManualHasAllFlags(otherAttributeTargets);
                        }
                    }
                }
            }
            if (temp)
            {
                Console.WriteLine();
            }
        }

        public static bool ManualHasAllFlags(this AttributeTargets flags, AttributeTargets otherFlags) => (flags & otherFlags) == otherFlags;
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
namespace System.Linq
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

namespace System
{
    //public delegate void Action();
    //public delegate void Action<in T1, in T2>(T1 arg1, T2 arg2);
    //public delegate void Action<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);
    //public delegate void Action<in T1, in T2, in T3, in T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    //public delegate TResult Func<out TResult>();
    public delegate TResult Func<in T, out TResult>(T arg);
    //public delegate TResult Func<in T1, in T2, out TResult>(T1 arg1, T2 arg2);
    //public delegate TResult Func<in T1, in T2, in T3, out TResult>(T1 arg1, T2 arg2, T3 arg3);
    //public delegate TResult Func<in T1, in T2, in T3, in T4, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
}
#endif