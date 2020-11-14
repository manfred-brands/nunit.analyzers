using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using NUnit.Analyzers.Constants;

namespace NUnit.Analyzers.ConsoleAccess
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ConsoleAccessAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor descriptor = DiagnosticDescriptorCreator.Create(
            id: AnalyzerIdentifiers.ConsoleAccessInTest,
            title: ConsoleAccessConstants.Title,
            messageFormat: ConsoleAccessConstants.Message,
            category: Categories.Structure,
            defaultSeverity: DiagnosticSeverity.Info,
            isEnabledByDefault: true,
            description: ConsoleAccessConstants.Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
            = ImmutableArray.Create(descriptor);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationStartAction(AnalyzeCompilationStart);
        }

        private static void AnalyzeCompilationStart(CompilationStartAnalysisContext context)
        {
            INamedTypeSymbol? systemConsoleType = context.Compilation.GetTypeByMetadataName("System.Console");
            if (systemConsoleType is null)
                return;

            context.RegisterOperationAction(context => AnalyzeMemberAccess(context, systemConsoleType), OperationKind.MethodReference);
            context.RegisterOperationAction(context => AnalyzeMemberAccess(context, systemConsoleType), OperationKind.PropertyReference);
            context.RegisterOperationAction(context => AnalyzeInvocation(context, systemConsoleType), OperationKind.Invocation);
        }

        private static void AnalyzeMemberAccess(OperationAnalysisContext context, INamedTypeSymbol systemConsoleType)
        {
            if (context.Operation is IMemberReferenceOperation memberReferenceOperation)
            {
                if (memberReferenceOperation.Instance is null)
                {
                    ReportIfTypeIsConsole(context, systemConsoleType, memberReferenceOperation.Member.ContainingType);
                }
            }
        }

        private static void AnalyzeInvocation(OperationAnalysisContext context, INamedTypeSymbol systemConsoleType)
        {
            if (context.Operation is IInvocationOperation invocationOperation)
            {
                if (invocationOperation.Instance is null)
                {
                    ReportIfTypeIsConsole(context, systemConsoleType, invocationOperation.TargetMethod.ContainingType);
                }
            }
        }

        private static void ReportIfTypeIsConsole(OperationAnalysisContext context, INamedTypeSymbol systemConsoleType, INamedTypeSymbol type)
        {
            if (SymbolEqualityComparer.Default.Equals(type, systemConsoleType))
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    descriptor,
                    context.Operation.Syntax.GetLocation()));
            }
        }
    }
}
