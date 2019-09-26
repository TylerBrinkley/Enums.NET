using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace EnumsNET.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ObsoletedMethodMigrationCodeFix)), Shared]
    public class ObsoletedMethodMigrationCodeFix : CodeFixProvider
    {
        private const string s_title = "Migrate obsoleted method calls";

        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(ObsoletedMethodMigrationAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() => BatchFixer.Instance;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var identifierName = (IdentifierNameSyntax)root.FindToken(diagnostic.Location.SourceSpan.Start).Parent;

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: s_title,
                    createChangedDocument: async c =>
                    {
                        var syntaxEditor = new SyntaxEditor(root, context.Document.Project.Solution.Workspace);
                        TryAddUsingDirective(root, syntaxEditor);
                        var semanticModel = await context.Document.GetSemanticModelAsync(c).ConfigureAwait(false);
                        UpdateCodeUsage(identifierName, syntaxEditor, semanticModel);
                        return context.Document.WithSyntaxRoot(syntaxEditor.GetChangedRoot());
                    },
                    equivalenceKey: s_title),
                diagnostic);
        }

        private static void TryAddUsingDirective(SyntaxNode root, SyntaxEditor syntaxEditor)
        {
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
        }

        private static void TryReplaceUsingDirectives(SyntaxNode root, SyntaxEditor syntaxEditor)
        {
            var foundUsingDirective = false;
            foreach (var usingDirective in root.ChildNodes().OfType<UsingDirectiveSyntax>())
            {
                switch (usingDirective.Name.ToString())
                {
                    case "EnumsNET.NonGeneric":
                    case "EnumsNET.Unsafe":
                        if (foundUsingDirective)
                        {
                            syntaxEditor.RemoveNode(usingDirective);
                        }
                        else
                        {
                            syntaxEditor.ReplaceNode(usingDirective, SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("EnumsNET")));
                            foundUsingDirective = true;
                        }
                        break;
                    case "EnumsNET":
                        if (foundUsingDirective)
                        {
                            syntaxEditor.RemoveNode(usingDirective);
                        }
                        else
                        {
                            foundUsingDirective = true;
                        }
                        break;
                }
            }
        }

        private static void UpdateCodeUsage(IdentifierNameSyntax identifierName, SyntaxEditor syntaxEditor, SemanticModel semanticModel)
        {
            SyntaxNode n = identifierName;
            var appendUnsafe = false;
            INamedTypeSymbol type = null;
            switch (identifierName.Identifier.ValueText)
            {
                case "NonGenericEnums":
                    type = GetEnumsType(semanticModel);
                    break;
                case "NonGenericFlagEnums":
                    type = GetFlagEnumsType(semanticModel);
                    break;
                case "UnsafeEnums":
                    type = GetEnumsType(semanticModel);
                    appendUnsafe = true;
                    break;
                case "UnsafeFlagEnums":
                    type = GetFlagEnumsType(semanticModel);
                    appendUnsafe = true;
                    break;
                default:
                    while (n.Parent is MemberAccessExpressionSyntax m)
                    {
                        n = m;
                        switch (m.Name.Identifier.ValueText)
                        {
                            case "NonGenericEnums":
                                type = GetEnumsType(semanticModel);
                                break;
                            case "NonGenericFlagEnums":
                                type = GetFlagEnumsType(semanticModel);
                                break;
                            case "UnsafeEnums":
                                type = GetEnumsType(semanticModel);
                                appendUnsafe = true;
                                break;
                            case "UnsafeFlagEnums":
                                type = GetFlagEnumsType(semanticModel);
                                appendUnsafe = true;
                                break;
                            default:
                                continue;
                        }
                        break;
                    }
                    break;
            }
            if (type == null)
            {
                throw new InvalidOperationException("Could not find diagnostic source");
            }

            syntaxEditor.ReplaceNode(n, syntaxEditor.Generator.TypeExpression(type));
            if (appendUnsafe)
            {
                var methodName = ((MemberAccessExpressionSyntax)n.Parent).Name;
                var nonGenericName = SyntaxFactory.Identifier(methodName.Identifier.Text + "Unsafe");
                var newName = methodName is GenericNameSyntax genericName
                    ? SyntaxFactory.GenericName(nonGenericName, genericName.TypeArgumentList)
                    : (SimpleNameSyntax)SyntaxFactory.IdentifierName(nonGenericName);
                syntaxEditor.ReplaceNode(methodName, newName);
            }
        }

        private static INamedTypeSymbol GetEnumsType(SemanticModel semanticModel) => semanticModel.Compilation.GetTypeByMetadataName("EnumsNET.Enums");

        private static INamedTypeSymbol GetFlagEnumsType(SemanticModel semanticModel) => semanticModel.Compilation.GetTypeByMetadataName("EnumsNET.FlagEnums");

        private sealed class BatchFixer : FixAllProvider
        {
            public static BatchFixer Instance { get; } = new BatchFixer();

            public override async Task<CodeAction> GetFixAsync(FixAllContext fixAllContext)
            {
                IEnumerable<Task<KeyValuePair<DocumentId, SyntaxNode>>> tasks = null;

                switch (fixAllContext.Scope)
                {
                    case FixAllScope.Document:
                        tasks = new[] { GetDocumentFixAsync(fixAllContext.Document, fixAllContext) };
                        break;
                    case FixAllScope.Project:
                        tasks = fixAllContext.Project.Documents.Select(d => GetDocumentFixAsync(d, fixAllContext));
                        break;
                    case FixAllScope.Solution:
                        tasks = fixAllContext.Solution.Projects.SelectMany(p => p.Documents.Select(d => GetDocumentFixAsync(d, fixAllContext)));
                        break;
                    case FixAllScope.Custom:
                        return null;
                }

                var currentSolution = fixAllContext.Solution;
                foreach (var pair in await Task.WhenAll(tasks).ConfigureAwait(false))
                {
                    currentSolution = currentSolution.WithDocumentSyntaxRoot(pair.Key, pair.Value);
                }

                return CodeAction.Create(
                    title: s_title,
                    createChangedSolution: _ => Task.FromResult(currentSolution),
                    equivalenceKey: s_title);
            }

            private static async Task<KeyValuePair<DocumentId, SyntaxNode>> GetDocumentFixAsync(Document document, FixAllContext fixAllContext)
            {
                var diagnostics = await fixAllContext.GetDocumentDiagnosticsAsync(document).ConfigureAwait(false);
                var root = await document.GetSyntaxRootAsync(fixAllContext.CancellationToken).ConfigureAwait(false);
                var syntaxEditor = new SyntaxEditor(root, document.Project.Solution.Workspace);
                TryReplaceUsingDirectives(root, syntaxEditor);
                var semanticModel = await document.GetSemanticModelAsync(fixAllContext.CancellationToken).ConfigureAwait(false);
                foreach (var diagnostic in diagnostics)
                {
                    var memberAccessNode = (IdentifierNameSyntax)root.FindToken(diagnostic.Location.SourceSpan.Start).Parent;
                    UpdateCodeUsage(memberAccessNode, syntaxEditor, semanticModel);
                }
                return new KeyValuePair<DocumentId, SyntaxNode>(document.Id, syntaxEditor.GetChangedRoot());
            }
        }
    }
}