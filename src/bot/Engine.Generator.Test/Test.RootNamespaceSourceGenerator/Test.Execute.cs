using System.Linq;
using Xunit;

namespace GarageGroup.Infra.Telegram.Bot.Engine.Generator.Test;

partial class RootNamespaceSourceGeneratorTest
{
    [Theory]
    [MemberData(nameof(RootTypeSourceCodes))]
    public static void Execute_SourceCodeContainsRootType_GeneratesRootNamespaceSource(
        string sourceCode, string expectedNamespace)
    {
        var result = RunGenerator(sourceCode);
        var generatorResult = result.Results.Single();

        Assert.Null(generatorResult.Exception);
        Assert.Empty(result.Diagnostics);

        var generatedSource = generatorResult.GeneratedSources.Single();

        Assert.Equal("ProgramRootNamespace.g.cs", generatedSource.HintName);

        var source = NormalizeNewLines(generatedSource.SourceText.ToString());
        Assert.Equal(
            NormalizeNewLines(
                $$"""
                // Auto-generated code
                #nullable enable

                using Microsoft.Extensions.Localization;

                [assembly: RootNamespace("{{expectedNamespace}}")]
                """),
            source);
    }

    [Theory]
    [MemberData(nameof(NonRootTypeSourceCodes))]
    public static void Execute_SourceCodeDoesNotContainRootType_DoesNotGenerateSources(
        string sourceCode)
    {
        var result = RunGenerator(sourceCode);
        var generatorResult = result.Results.Single();

        Assert.Null(generatorResult.Exception);
        Assert.Empty(result.Diagnostics);
        Assert.Empty(generatorResult.GeneratedSources);
    }

    public static TheoryData<string, string> RootTypeSourceCodes
        =>
        new()
        {
            {
                """
                namespace Some.Test;

                public static class Program
                {
                }
                """,
                "Some.Test"
            },
            {
                """
                namespace Some.Internal.Test;

                static class Program
                {
                }
                """,
                "Some.Internal.Test"
            }
        };

    public static TheoryData<string> NonRootTypeSourceCodes
        =>
        [
            string.Empty,
            """
            namespace Some.Test;

            public static class Stub
            {
            }
            """,
            """
            public static class Program
            {
            }
            """,
            """
            namespace Some.Test;

            public sealed class Program
            {
            }
            """,
            """
            namespace Some.Test;

            public static class Program<T>
            {
            }
            """
        ];
}