using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EnumsNET.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ObsoletedMethodMigrationAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ENUMS001";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString s_title = new LocalizableResourceString(nameof(Resources.ENUMS001Title), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString s_messageFormat = new LocalizableResourceString(nameof(Resources.ENUMS001MessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString s_description = new LocalizableResourceString(nameof(Resources.ENUMS001Description), Resources.ResourceManager, typeof(Resources));
        private const string s_category = "Library";

        private static readonly DiagnosticDescriptor s_rule = new DiagnosticDescriptor(DiagnosticId, s_title, s_messageFormat, s_category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: s_description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(s_rule);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.SimpleMemberAccessExpression);

        private static void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            var memberAccessNode = (MemberAccessExpressionSyntax)context.Node;
            Diagnostic diagnostic = null;
            switch (memberAccessNode.Expression)
            {
                case IdentifierNameSyntax identifierName:
                    switch (identifierName.Identifier.ValueText)
                    {
                        case "NonGenericEnums":
                        case "UnsafeEnums":
                            diagnostic = Diagnostic.Create(s_rule, memberAccessNode.GetLocation(), identifierName.Identifier.ValueText, "Enums");
                            break;
                        case "NonGenericFlagEnums":
                        case "UnsafeFlagEnums":
                            diagnostic = Diagnostic.Create(s_rule, memberAccessNode.GetLocation(), identifierName.Identifier.ValueText, "FlagEnums");
                            break;
                    }
                    break;
                case MemberAccessExpressionSyntax memberAccessExpression:
                    switch (memberAccessExpression.Name.Identifier.ValueText)
                    {
                        case "NonGenericEnums":
                        case "UnsafeEnums":
                            diagnostic = Diagnostic.Create(s_rule, memberAccessExpression.GetLocation(), memberAccessExpression.Name.Identifier.ValueText, "Enums");
                            break;
                        case "NonGenericFlagEnums":
                        case "UnsafeFlagEnums":
                            diagnostic = Diagnostic.Create(s_rule, memberAccessExpression.GetLocation(), memberAccessExpression.Name.Identifier.ValueText, "FlagEnums");
                            break;
                    }
                    break;
            }
            if (diagnostic != null)
            {
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}