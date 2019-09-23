using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EnumsNET.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NonGenericFlagEnumsMigrationAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ENUMS002";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.ENUMS002Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.ENUMS002MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.ENUMS002Description), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Library";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.SimpleMemberAccessExpression);

        private static void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            var memberAccessNode = (MemberAccessExpressionSyntax)context.Node;

            if (memberAccessNode.Expression is IdentifierNameSyntax identifierNameSyntax && identifierNameSyntax.Identifier.ValueText == "NonGenericFlagEnums")
            {
                var diagnostic = Diagnostic.Create(Rule, memberAccessNode.GetLocation(), memberAccessNode.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}