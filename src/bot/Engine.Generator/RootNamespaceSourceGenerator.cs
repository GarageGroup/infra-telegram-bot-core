using Microsoft.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

[Generator]
public class RootNamespaceSourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var rootType = context.FindRootType();

        if (rootType is null)
        {
            return;
        }

        var sourceCode = rootType.BuildRootNamespaceSourceCode();
        context.AddSource($"{rootType.TypeName}RootNamespace.g.cs", sourceCode);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required for this one
    }
}