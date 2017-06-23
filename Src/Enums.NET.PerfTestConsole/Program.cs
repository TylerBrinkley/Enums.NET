using System;
using System.Diagnostics;
using System.Reflection;
using BenchmarkDotNet.Running;
using EnumsNET.Tests.Benchmarks;

namespace EnumsNET.PerfTestConsole
{
    static class Program
    {
        static void Main()
        {
            var version = FileVersionInfo.GetVersionInfo(typeof(Enums).GetTypeInfo().Assembly.Location).FileVersion;
            Console.WriteLine("Enums.NET Version: " + version);

            new BenchmarkSwitcher(new[] { /*typeof(ParseBenchmarks), typeof(ToStringBenchmarks), */typeof(HasFlagBenchmarks)/*, typeof(GetHashCodeBenchmarks), typeof(IsDefinedBenchmarks) */}).Run(new[] { "*" });
        }
    }
}