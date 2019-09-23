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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NonGenericEnumsMigrationCodeFixProvider)), Shared]
    public class NonGenericEnumsMigrationCodeFixProvider : CodeFixProvider
    {
        private const string title = "Migrate NonGenericEnums to Enums";

        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NonGenericEnumsMigrationAnalyzer.DiagnosticId);

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
                    createChangedDocument: c => MigrateToEnumsAsync(context.Document, memberAccessNode, c),
                    equivalenceKey: title),
                diagnostic);
        }

        private async Task<Document> MigrateToEnumsAsync(Document document, IdentifierNameSyntax memberAccessNode, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var syntaxEditor = new SyntaxEditor(root, document.Project.Solution.Workspace);
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            var hasNewNamespace = false;
            foreach (var usingDirective in root.ChildNodes().OfType<UsingDirectiveSyntax>())
            {
                switch (usingDirective.Name.ToString())
                {
                    case "EnumsNET.NonGeneric":
                        if (hasNewNamespace)
                        {
                            syntaxEditor.RemoveNode(usingDirective);
                        }
                        else
                        {
                            syntaxEditor.ReplaceNode(usingDirective.Name, SyntaxFactory.IdentifierName("EnumsNET"));
                            hasNewNamespace = true;
                        }
                        break;
                    case "EnumsNET":
                        if (hasNewNamespace)
                        {
                            syntaxEditor.RemoveNode(usingDirective);
                        }
                        else
                        {
                            hasNewNamespace = true;
                        }
                        break;
                }
            }
            var enumsType = semanticModel.Compilation.GetTypeByMetadataName("EnumsNET.Enums");
            syntaxEditor.ReplaceNode(memberAccessNode, syntaxEditor.Generator.TypeExpression(enumsType));
            return document.WithSyntaxRoot(syntaxEditor.GetChangedRoot());
        }
    }
}