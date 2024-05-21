using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class BotStringLocalizer : ResourceManagerStringLocalizer
{
    private static readonly ResourceNamesCache ResourceNamesCache = new();

    private readonly CultureInfo culture;

    private readonly string baseName;

    internal BotStringLocalizer(CultureInfo culture, ResourceManager resourceManager, Assembly assembly, string baseName, ILogger logger)
        : base(resourceManager, assembly, baseName, ResourceNamesCache, logger)
    {
        this.culture = culture;
        this.baseName = baseName;
    }

    public override LocalizedString this[string name]
    {
        get
        {
            var resourceName = name.OrEmpty();
            var stringSafely = GetStringSafely(resourceName, culture);

            return new(
                name: resourceName,
                value: stringSafely ?? resourceName,
                resourceNotFound: stringSafely is null,
                searchedLocation: baseName);
        }
    }

    public override LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var resourceName = name.OrEmpty();
            var stringSafely = GetStringSafely(resourceName, culture);

            return new(
                name: resourceName,
                value: string.Format(culture, stringSafely ?? resourceName, arguments),
                resourceNotFound: stringSafely is null,
                searchedLocation: baseName);
        }
    }

    public override IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        =>
        GetAllStrings(includeParentCultures, culture);
}