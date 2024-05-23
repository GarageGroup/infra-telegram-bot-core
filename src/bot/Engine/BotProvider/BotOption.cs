using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotOption
{
    private const string ResourcesPathDefault = "Resources";

    public BotOption(
        string fileUrlTemplate,
        [AllowNull] string resourcesPath = ResourcesPathDefault,
        FlatArray<CultureInfo> availableCultures = default,
        Uri? webAppBaseAddress = null)
    {
        FileUrlTemplate = fileUrlTemplate.OrEmpty();
        ResourcesPath = resourcesPath.OrNullIfWhiteSpace() ?? ResourcesPathDefault;
        AvailableCultures = availableCultures;
        WebAppBaseAddress = webAppBaseAddress;
    }

    public string FileUrlTemplate { get; }

    public string ResourcesPath { get; }

    public FlatArray<CultureInfo> AvailableCultures { get; }

    public Uri? WebAppBaseAddress { get; }
}