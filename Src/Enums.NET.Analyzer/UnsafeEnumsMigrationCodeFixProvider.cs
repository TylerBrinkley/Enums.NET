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

namespace EnumsNET.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(UnsafeEnumsMigrationCodeFixProvider)), Shared]
    public class UnsafeEnumsMigrationCodeFixProvider : CodeFixProvider
    {
        private const string title = "Migrate UnsafeEnums to Enums";

        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(UnsafeEnumsMigrationAnalyzer.DiagnosticId);

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
            var methodName = (GenericNameSyntax)((MemberAccessExpressionSyntax)memberAccessNode.Parent).Name;
            var methodNameString = methodName.Identifier.Text;
            var newName = SyntaxFactory.GenericName(SyntaxFactory.Identifier(methodNameString + "Unsafe"), methodName.TypeArgumentList);
            var newRoot = root.ReplaceNode(methodName, newName);
            newRoot = newRoot.ReplaceNode(newRoot.FindNode(memberAccessNode.Span), SyntaxFactory.IdentifierName("Enums"));
            return document.WithSyntaxRoot(newRoot);
        }
    }
}