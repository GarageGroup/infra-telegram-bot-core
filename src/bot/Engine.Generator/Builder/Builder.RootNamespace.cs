using System.Text;

namespace GarageGroup.Infra.Telegram.Bot;

partial class BotEngineBuilder
{
    internal static string BuildRootNamespaceSourceCode(this RootTypeMetadata rootType)
        =>
        new StringBuilder(
            "// Auto-generated code")
        .AppendLine()
        .AppendLine(
            "#nullable enable")
        .AppendLine()
        .AppendLine(
            "using Microsoft.Extensions.Localization;")
        .AppendLine()
        .AppendLine(
            $"[assembly: RootNamespace(\"{rootType.Namespace}\")]")
        .ToString();
}