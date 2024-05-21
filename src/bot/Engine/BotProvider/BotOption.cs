using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace GarageGroup.Infra.Telegram.Bot;

public readonly record struct BotOption
{
    private const string ResourcesPathDefault = "Resources";

    private readonly string? resourcesPath;

    public BotOption(
        [AllowNull] string resourcesPath = ResourcesPathDefault,
        FlatArray<CultureInfo> availableCultures = default,
        Uri? webAppBaseAddress = null)
    {
        this.resourcesPath = resourcesPath;
        AvailableCultures = availableCultures;
        WebAppBaseAddress = webAppBaseAddress;
    }

    public string ResourcesPath
        =>
        resourcesPath.OrNullIfWhiteSpace() ?? ResourcesPathDefault;

    public FlatArray<CultureInfo> AvailableCultures { get; }

    public Uri? WebAppBaseAddress { get; }
}