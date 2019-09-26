using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace EnumsNET.Analyzer.Tests
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {
        //No diagnostics expected to show up
        [TestMethod]
        public void NoDiagnostic_ForEmptyString()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod2()
        {
            var test = @"
using System;
using EnumsNET.NonGeneric;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)NonGenericEnums.Parse(typeof(DayOfWeek), s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "NonGenericEnums methods have moved to Enums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 72)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using EnumsNET;
using EnumsNET.NonGeneric;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)Enums.Parse(typeof(DayOfWeek), s);
    }
}";
            VerifyCSharpFix(test, fixtest, allowNewCompilerDiagnostics: true);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod3()
        {
            var test = @"
using System;
using EnumsNET;
using EnumsNET.NonGeneric;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)NonGenericEnums.Parse(typeof(DayOfWeek), s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "NonGenericEnums methods have moved to Enums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 10, 72)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using EnumsNET;
using EnumsNET.NonGeneric;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)Enums.Parse(typeof(DayOfWeek), s);
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod4()
        {
            var test = @"
using System;
using EnumsNET;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)NonGeneric.NonGenericEnums.Parse(typeof(DayOfWeek), s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "NonGenericEnums methods have moved to Enums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 72)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using EnumsNET;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)Enums.Parse(typeof(DayOfWeek), s);
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod5()
        {
            var test = @"
using System;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)EnumsNET.NonGeneric.NonGenericEnums.Parse(typeof(DayOfWeek), s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "NonGenericEnums methods have moved to Enums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 8, 72)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)EnumsNET.Enums.Parse(typeof(DayOfWeek), s);
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod6()
        {
            var test = @"
using System;
using EnumsNET.NonGeneric;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)NonGenericFlagEnums.ParseFlags(typeof(DayOfWeek), s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "NonGenericFlagEnums methods have moved to FlagEnums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 72)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using EnumsNET;
using EnumsNET.NonGeneric;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)FlagEnums.ParseFlags(typeof(DayOfWeek), s);
    }
}";
            VerifyCSharpFix(test, fixtest, allowNewCompilerDiagnostics: true);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod7()
        {
            var test = @"
using System;
using EnumsNET;
using EnumsNET.NonGeneric;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)NonGenericFlagEnums.ParseFlags(typeof(DayOfWeek), s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "NonGenericFlagEnums methods have moved to FlagEnums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 10, 72)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using EnumsNET;
using EnumsNET.NonGeneric;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)FlagEnums.ParseFlags(typeof(DayOfWeek), s);
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod8()
        {
            var test = @"
using System;
using EnumsNET;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)NonGeneric.NonGenericFlagEnums.ParseFlags(typeof(DayOfWeek), s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "NonGenericFlagEnums methods have moved to FlagEnums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 72)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using EnumsNET;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)FlagEnums.ParseFlags(typeof(DayOfWeek), s);
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod9()
        {
            var test = @"
using System;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)EnumsNET.NonGeneric.NonGenericFlagEnums.ParseFlags(typeof(DayOfWeek), s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "NonGenericFlagEnums methods have moved to FlagEnums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 8, 72)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => (DayOfWeek)EnumsNET.FlagEnums.ParseFlags(typeof(DayOfWeek), s);
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod10()
        {
            var test = @"
using System;
using EnumsNET.Unsafe;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => UnsafeEnums.Parse<DayOfWeek>(s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "UnsafeEnums methods have moved to Enums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 61)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using EnumsNET;
using EnumsNET.Unsafe;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => Enums.ParseUnsafe<DayOfWeek>(s);
    }
}";
            VerifyCSharpFix(test, fixtest, allowNewCompilerDiagnostics: true);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod11()
        {
            var test = @"
using System;
using EnumsNET;
using EnumsNET.Unsafe;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => UnsafeEnums.Parse<DayOfWeek>(s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "UnsafeEnums methods have moved to Enums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 10, 61)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using EnumsNET;
using EnumsNET.Unsafe;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => Enums.ParseUnsafe<DayOfWeek>(s);
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod12()
        {
            var test = @"
using System;
using EnumsNET;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => Unsafe.UnsafeEnums.Parse<DayOfWeek>(s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "UnsafeEnums methods have moved to Enums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 61)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using EnumsNET;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => Enums.ParseUnsafe<DayOfWeek>(s);
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod13()
        {
            var test = @"
using System;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => EnumsNET.Unsafe.UnsafeEnums.Parse<DayOfWeek>(s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "UnsafeEnums methods have moved to Enums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 8, 61)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => EnumsNET.Enums.ParseUnsafe<DayOfWeek>(s);
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod14()
        {
            var test = @"
using System;
using EnumsNET.Unsafe;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => UnsafeFlagEnums.ParseFlags<DayOfWeek>(s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "UnsafeFlagEnums methods have moved to FlagEnums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 61)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using EnumsNET;
using EnumsNET.Unsafe;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => FlagEnums.ParseFlagsUnsafe<DayOfWeek>(s);
    }
}";
            VerifyCSharpFix(test, fixtest, allowNewCompilerDiagnostics: true);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod15()
        {
            var test = @"
using System;
using EnumsNET;
using EnumsNET.Unsafe;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => UnsafeFlagEnums.ParseFlags<DayOfWeek>(s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "UnsafeFlagEnums methods have moved to FlagEnums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 10, 61)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using EnumsNET;
using EnumsNET.Unsafe;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => FlagEnums.ParseFlagsUnsafe<DayOfWeek>(s);
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod16()
        {
            var test = @"
using System;
using EnumsNET;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => Unsafe.UnsafeFlagEnums.ParseFlags<DayOfWeek>(s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "UnsafeFlagEnums methods have moved to FlagEnums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 61)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using EnumsNET;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => FlagEnums.ParseFlagsUnsafe<DayOfWeek>(s);
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod17()
        {
            var test = @"
using System;
using static System.Math;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => EnumsNET.Unsafe.UnsafeFlagEnums.ParseFlags<DayOfWeek>(s);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "UnsafeFlagEnums methods have moved to FlagEnums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 61)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using static System.Math;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static DayOfWeek ParseDayOfWeek(string s) => EnumsNET.FlagEnums.ParseFlagsUnsafe<DayOfWeek>(s);
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod18()
        {
            var test = @"
using System;
using EnumsNET.Unsafe;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static IReadOnlyList<AttributeTargets> GetAttributeTargetsFlags(AttributeTargets t) => UnsafeFlagEnums.GetFlags(t);
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "ENUMS001",
                Message = "UnsafeFlagEnums methods have moved to FlagEnums",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 103)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using EnumsNET;
using EnumsNET.Unsafe;

namespace ConsoleApplication1
{
    class TypeName
    {
        public static IReadOnlyList<AttributeTargets> GetAttributeTargetsFlags(AttributeTargets t) => FlagEnums.GetFlagsUnsafe(t);
    }
}";
            VerifyCSharpFix(test, fixtest, allowNewCompilerDiagnostics: true);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider() => new ObsoletedMethodMigrationCodeFix();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new ObsoletedMethodMigrationAnalyzer();
    }
}