using Microsoft.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

[Generator(LanguageNames.CSharp)]
public sealed class RootNamespaceSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var rootTypeProvider = context.CompilationProvider.Select(GeneratorExtensions.FindRootType);
        context.RegisterSourceOutput(rootTypeProvider, AddSource);

        static void AddSource(SourceProductionContext context, RootTypeMetadata? rootType)
        {
            if (rootType is null)
            {
                return;
            }

            var sourceCode = rootType.BuildRootNamespaceSourceCode();
            context.AddSource($"{rootType.TypeName}RootNamespace.g.cs", sourceCode);
        }
    }
}