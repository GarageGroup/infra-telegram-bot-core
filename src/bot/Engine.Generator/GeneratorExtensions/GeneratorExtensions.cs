using System.Collections.Generic;

namespace GarageGroup.Infra.Telegram.Bot;

internal static partial class GeneratorExtensions
{
    private const string RootTypeName = "Program";

    private static IEnumerable<T> NotNull<T>(this IEnumerable<T?> source)
    {
        foreach (var item in source)
        {
            if (item is null)
            {
                continue;
            }

            yield return item;
        }
    }
}