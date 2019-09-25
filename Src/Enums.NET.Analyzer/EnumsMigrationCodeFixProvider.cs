using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace EnumsNET.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EnumsMigrationCodeFixProvider)), Shared]
    public class EnumsMigrationCodeFixProvider : CodeFixProvider
    {
        private const string title = "Migrate obsoleted method calls";

        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(EnumsMigrationAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var memberAccessNode = (IdentifierNameSyntax)root.FindToken(diagnosticSpan.Start).Parent;

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: title,
                    createChangedDocument: c => MigrateAsync(context.Document, memberAccessNode, c),
                    equivalenceKey: title),
                diagnostic);
        }

        private async Task<Document> MigrateAsync(Document document, IdentifierNameSyntax memberAccessNode, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var syntaxEditor = new SyntaxEditor(root, document.Project.Solution.Workspace);
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            UsingDirectiveSyntax firstEnumsNETUsingDirective = null;
            foreach (var usingDirective in root.ChildNodes().OfType<UsingDirectiveSyntax>())
            {
                switch (usingDirective.Name.ToString())
                {
                    case "EnumsNET.NonGeneric":
                    case "EnumsNET.Unsafe":
                        if (firstEnumsNETUsingDirective == null)
                        {
                            firstEnumsNETUsingDirective = usingDirective;
                        }
                        continue;
                    case "EnumsNET":
                        firstEnumsNETUsingDirective = null;
                        break;
                    default:
                        continue;
                }
                break;
            }
            if (firstEnumsNETUsingDirective != null)
            {
                syntaxEditor.InsertBefore(firstEnumsNETUsingDirective, SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("EnumsNET")));
            }
            var enumsType = semanticModel.Compilation.GetTypeByMetadataName("EnumsNET.Enums");
            var flagEnumsType = semanticModel.Compilation.GetTypeByMetadataName("EnumsNET.FlagEnums");
            SyntaxNode n = memberAccessNode;
            var appendUnsafe = false;
            var type = enumsType;
            switch (memberAccessNode.Identifier.ValueText)
            {
                case "NonGenericEnums":
                    break;
                case "NonGenericFlagEnums":
                    type = flagEnumsType;
                    break;
                case "UnsafeEnums":
                    appendUnsafe = true;
                    break;
                case "UnsafeFlagEnums":
                    appendUnsafe = true;
                    type = flagEnumsType;
                    break;
                default:
                    while (n.Parent is MemberAccessExpressionSyntax m)
                    {
                        n = m;
                        switch (m.Name.Identifier.ValueText)
                        {
                            case "NonGenericEnums":
                                break;
                            case "NonGenericFlagEnums":
                                type = flagEnumsType;
                                break;
                            case "UnsafeEnums":
                                appendUnsafe = true;
                                break;
                            case "UnsafeFlagEnums":
                                appendUnsafe = true;
                                type = flagEnumsType;
                                break;
                            default:
                                continue;
                        }
                        break;
                    }
                    break;
            }
            syntaxEditor.ReplaceNode(n, syntaxEditor.Generator.TypeExpression(type));
            if (appendUnsafe)
            {
                var methodName = ((MemberAccessExpressionSyntax)n.Parent).Name;
                SimpleNameSyntax newName;
                var nonGenericName = SyntaxFactory.Identifier(methodName.Identifier.Text + "Unsafe");
                newName = methodName is GenericNameSyntax genericName
                    ? SyntaxFactory.GenericName(nonGenericName, genericName.TypeArgumentList)
                    : (SimpleNameSyntax)SyntaxFactory.IdentifierName(nonGenericName);
                syntaxEditor.ReplaceNode(methodName, newName);
            }
            return document.WithSyntaxRoot(syntaxEditor.GetChangedRoot());
        }
    }
}