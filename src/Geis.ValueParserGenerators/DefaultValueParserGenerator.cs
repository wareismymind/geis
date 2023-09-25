using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace Geis.ValueParserGenerators;

[Generator]
public class DefaultValueParserGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        //System.Diagnostics.Debugger.Launch();

        var provider = context
            .SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (c, _) => c is GenericNameSyntax n,
                transform: (n, _) => (GenericNameSyntax)n.Node)
            .Where(x => x is not null);

        var compilation = context.CompilationProvider.Combine(provider.Collect());

        context.RegisterSourceOutput(
            compilation,
            (spc, source) => Execute(spc, source.Left, source.Right));
    }

    private void Execute(
        SourceProductionContext context,
        Compilation compilation,
        ImmutableArray<GenericNameSyntax> genericNames)
    {
        foreach (var name in genericNames)
        {
            // todo: add explicitly constructor value parsers that reference the TryParse or new(string) member of T.

            context.ReportDiagnostic(
                Diagnostic.Create(
                    new DiagnosticDescriptor(
                        id: "GVPG0001",
                        title: "Possible Reference To AddDefaultParser",
                        messageFormat: "Found a possible reference to the AddDefaultParser method.",
                        category: "Accomplishment",
                        defaultSeverity: DiagnosticSeverity.Warning,
                        isEnabledByDefault: true),
                    name.GetLocation()));
        }
    }
}
