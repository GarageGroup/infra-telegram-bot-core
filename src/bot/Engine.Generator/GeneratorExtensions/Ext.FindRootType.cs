using System;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

partial class GeneratorExtensions
{
    internal static RootTypeMetadata? FindRootType(this Compilation compilation, CancellationToken cancellationToken)
    {
        var visitor = new ExportedTypesCollector(cancellationToken);
        visitor.VisitNamespace(compilation.GlobalNamespace);

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

        if (typeSymbol.ContainingNamespace is not { IsGlobalNamespace: false } namespaceSymbol)
        {
            return null;
        }

        return new RootTypeMetadata(
            typeName: typeSymbol.Name,
            @namespace: namespaceSymbol.ToString());
    }
}
