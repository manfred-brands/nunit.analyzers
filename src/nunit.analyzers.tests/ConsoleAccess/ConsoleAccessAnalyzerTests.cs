using Gu.Roslyn.Asserts;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Analyzers.ConsoleAccess;
using NUnit.Analyzers.Constants;
using NUnit.Framework;

namespace NUnit.Analyzers.Tests.ConsoleAcess
{
    public class ConsoleAccessAnalyzerTests
    {
        private static readonly DiagnosticAnalyzer analyzer = new ConsoleAccessAnalyzer();
        private static readonly ExpectedDiagnostic expectedDiagnostic =
            ExpectedDiagnostic.Create(AnalyzerIdentifiers.ConsoleAccessInTest);
        private Settings releaseModeSettings;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.releaseModeSettings = Settings.Default.WithCompilationOptions(
               Settings.Default.CompilationOptions.WithOptimizationLevel(Microsoft.CodeAnalysis.OptimizationLevel.Release));
        }

        [Test]
        public void AnalyzeNoConsoleAccess()
        {
            var testCode = TestUtility.WrapInTestMethod(@"
                Assert.Fail(""Testing"");
            ");

            RoslynAssert.Valid(analyzer, testCode, this.releaseModeSettings);
        }

        [Test]
        public void AnalyzeConsoleWriteLineInDebug()
        {
            var testCode = TestUtility.WrapInTestMethod(@"
                Console.WriteLine(""Testing"");
            ");

            RoslynAssert.Valid(analyzer, testCode, Settings.Default);
        }

        [Test]
        public void AnalyzeConsoleWriteLineInRelease()
        {
            var testCode = TestUtility.WrapInTestMethod(@"
                Console.WriteLine(""Testing"");
            ");

            RoslynAssert.Diagnostics(analyzer, expectedDiagnostic, testCode, this.releaseModeSettings);
        }

        [Test]
        public void AnalyzeConsoleWriteLineOutSideNUnit()
        {
            var testCode = @"
using System;

namespace Some.Production.Module
{
    public class Info
    {
        public void Show()
        {
            Console.WriteLine(""Testing"");
        }
    }
}
";

            RoslynAssert.Valid(analyzer, testCode, this.releaseModeSettings);
        }

        [Test]
        public void AnalyzeConditionalConsoleWriteLine()
        {
            var testCode = @"
#define VERBOSE
using System;
using NUnit.Framework;

namespace NUnit.Analyzers.Tests.Targets.TestCaseUsage
{
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
#if VERBOSE
            Console.WriteLine(""Testing"");
#endif
        }
    }
}
";

            RoslynAssert.Valid(analyzer, testCode, this.releaseModeSettings);
        }

        [Test]
        public void AnalyzeConditionalCompiledConsoleWriteLine()
        {
            var testCode = @"
using System;
using NUnit.Framework;

namespace NUnit.Analyzers.Tests.Targets.TestCaseUsage
{
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            Show(""Testing"");
        }

        [System.Diagnostics.Conditional(""VERBOSE"")]
        private void Show(string message)
        {
            Console.WriteLine(message);
        }
    }
}
";

            RoslynAssert.Valid(analyzer, testCode, this.releaseModeSettings);
        }

        [Test]
        public void AnalyzeConsoleErrorWrite()
        {
            var testCode = TestUtility.WrapInTestMethod(@"
                Console.Error.Write(""Failures: "");
            ");

            RoslynAssert.Diagnostics(analyzer, expectedDiagnostic, testCode, this.releaseModeSettings);
        }

        [Test]
        public void AnalyzeConsoleFunction()
        {
            var testCode = TestUtility.WrapInTestMethod(@"
                Action<string> reportFn = Console.Error.WriteLine;
                reportFn(""Failures: "");
            ");

            RoslynAssert.Diagnostics(analyzer, expectedDiagnostic, testCode, this.releaseModeSettings);
        }

        [Test]
        public void AnalyzeConsoleReadKey()
        {
            var testCode = TestUtility.WrapInTestMethod(@"
                System.Console.ReadKey(false);
            ");

            RoslynAssert.Diagnostics(analyzer, expectedDiagnostic, testCode, this.releaseModeSettings);
        }

        [Test]
        public void AnalyzeConsoleForegroundColor()
        {
            var testCode = TestUtility.WrapInTestMethod(@"
                Console.ForegroundColor = ConsoleColor.Red;
            ");

            RoslynAssert.Diagnostics(analyzer, expectedDiagnostic, testCode, this.releaseModeSettings);
        }
    }
}
