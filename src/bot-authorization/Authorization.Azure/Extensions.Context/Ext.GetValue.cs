using Microsoft.Extensions.Localization;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ContextExtensions
{
    internal static string GetValue(this IStringLocalizer localizer, string key, string defaultValue)
    {
        var value = localizer[key];

        if (value.ResourceNotFound || string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        return value.Value;
    }
}