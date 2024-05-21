using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class BotStringLocalizerFactory : ResourceManagerStringLocalizerFactory
{
    private readonly CultureInfo culture;

    private readonly string resourcesPath;

    private readonly ILoggerFactory loggerFactory;

    internal BotStringLocalizerFactory(CultureInfo culture, string resourcesPath, ILoggerFactory loggerFactory)
        : base(CreateOptions(resourcesPath), loggerFactory)
    {
        this.culture = culture;
        this.resourcesPath = resourcesPath;
        this.loggerFactory = loggerFactory;
    }

    internal BotStringLocalizerFactory WithCulture(CultureInfo culture)
        =>
        new(culture, resourcesPath, loggerFactory);

    protected override BotStringLocalizer CreateResourceManagerStringLocalizer(Assembly assembly, string baseName)
        =>
        new(
            culture: culture,
            resourceManager: new(baseName, assembly),
            assembly: assembly,
            baseName: baseName,
            logger: loggerFactory.CreateLogger<BotStringLocalizer>());

    private static IOptions<LocalizationOptions> CreateOptions(string resourcesPath)
        =>
        Options.Create(
            new LocalizationOptions
            {
                ResourcesPath = resourcesPath
            });
}