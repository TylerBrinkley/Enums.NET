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
                Message = "NonGenericEnums members have moved to Enums",
                Severity = DiagnosticSeverity.Error,
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
                Message = "NonGenericEnums members have moved to Enums",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 10, 72)
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
                Message = "NonGenericEnums members have moved to Enums",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 83)
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
                Message = "NonGenericEnums members have moved to Enums",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 8, 92)
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

        protected override CodeFixProvider GetCSharpCodeFixProvider() => new NonGenericEnumsMigrationCodeFixProvider();

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new NonGenericEnumsMigrationAnalyzer();
    }
}
