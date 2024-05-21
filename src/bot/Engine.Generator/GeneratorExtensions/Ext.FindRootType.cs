using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

partial class GeneratorExtensions
{
    internal static RootTypeMetadata? FindRootType(this GeneratorExecutionContext context)
    {
        var visitor = new ExportedTypesCollector(context.CancellationToken);
        visitor.VisitNamespace(context.Compilation.GlobalNamespace);

        return visitor.GetExportedTypes().Select(GetRootTypeMetadata).NotNull().FirstOrDefault();
    }

    private static RootTypeMetadata? GetRootTypeMetadata(INamedTypeSymbol typeSymbol)
    {
        if (typeSymbol.IsStatic is false || typeSymbol.TypeArguments.Any())
        {
            return null;
        }

        if (string.Equals(RootTypeName, typeSymbol.Name, StringComparison.InvariantCulture) is false)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(typeSymbol.ContainingNamespace?.ToString()))
        {
            return null;
        }

        return new RootTypeMetadata(
            typeName: typeSymbol.Name,
            @namespace: typeSymbol.ContainingNamespace?.ToString() ?? string.Empty);
    }
}