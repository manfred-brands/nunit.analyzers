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

        [Test]
        public void AnalyzeNoConsoleAccess()
        {
            var testCode = TestUtility.WrapInTestMethod(@"
                Assert.Fail(""Testing"");
            ");

            RoslynAssert.Valid(analyzer, testCode);
        }

        [Test]
        public void AnalyzeConsoleWriteLine()
        {
            var testCode = TestUtility.WrapInTestMethod(@"
                Console.WriteLine(""Testing"");
            ");

            RoslynAssert.Diagnostics(analyzer, expectedDiagnostic, testCode);
        }

        [Test]
        public void AnalyzeConsoleErrorWrite()
        {
            var testCode = TestUtility.WrapInTestMethod(@"
                Console.Error.Write(""Failures: "");
            ");

            RoslynAssert.Diagnostics(analyzer, expectedDiagnostic, testCode);
        }

        [Test]
        public void AnalyzeConsoleFunction()
        {
            var testCode = TestUtility.WrapInTestMethod(@"
                Action<string> reportFn = Console.Error.WriteLine;
                reportFn(""Failures: "");
            ");

            RoslynAssert.Diagnostics(analyzer, expectedDiagnostic, testCode);
        }

        [Test]
        public void AnalyzeConsoleReadKey()
        {
            var testCode = TestUtility.WrapInTestMethod(@"
                System.Console.ReadKey(false);
            ");

            RoslynAssert.Diagnostics(analyzer, expectedDiagnostic, testCode);
        }

        [Test]
        public void AnalyzeConsoleForegroundColor()
        {
            var testCode = TestUtility.WrapInTestMethod(@"
                Console.ForegroundColor = ConsoleColor.Red;
            ");

            RoslynAssert.Diagnostics(analyzer, expectedDiagnostic, testCode);
        }
    }
}
