namespace NUnit.Analyzers.Constants
{
    internal static class ConsoleAccessConstants
    {
        internal const string Title = "Console Access in test";
        internal const string Message = "Console Access slows down tests and pollutes CI logs";
        internal const string Description = "A test should not access the Console.";
    }
}
