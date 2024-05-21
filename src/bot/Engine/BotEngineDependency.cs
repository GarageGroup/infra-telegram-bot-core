using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PrimeFuncPack;

namespace GarageGroup.Infra.Telegram.Bot;

public static class BotEngineDependency
{
    private const string BotSectionNameDefault = "Bot";

    public static Dependency<BotProvider> UseBotProvider(
        this Dependency<IBotApi, IBotStorage, BotOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(ResolveBotProvider);

        static BotProvider ResolveBotProvider(IServiceProvider serviceProvider, IBotApi botApi, IBotStorage botStorage, BotOption option)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(botApi);
            ArgumentNullException.ThrowIfNull(botStorage);

            return new(
                botApi: botApi,
                botStorage: botStorage,
                option: option,
                loggerFactory: serviceProvider.GetRequiredService<ILoggerFactory>());
        }
    }

    public static Dependency<BotProvider> UseBotProvider(
        this Dependency<IBotApi, IBotStorage> dependency, string sectionName = BotSectionNameDefault)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(ResolveBotProvider);

        BotProvider ResolveBotProvider(IServiceProvider serviceProvider, IBotApi botApi, IBotStorage botStorage)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(botApi);
            ArgumentNullException.ThrowIfNull(botStorage);

            var section = serviceProvider.GetRequiredService<IConfiguration>().GetRequiredSection(sectionName.OrEmpty());

            return new(
                botApi: botApi,
                botStorage: botStorage,
                option: new(
                    resourcesPath: section["ResourcesPath"],
                    availableCultures: (section.GetSection("AvailableCultures").Get<string[]>()?.Select(CreateCulture)).ToFlatArray(),
                    webAppBaseAddress: section.GetWebAppBaseAddressOrThrow()),
                loggerFactory: serviceProvider.GetRequiredService<ILoggerFactory>());

            static CultureInfo CreateCulture(string name)
                =>
                new(name);
        }
    }

    public static Dependency<IBotSignalHandler> UseBotSignalHandler<TEntityApi>(
        this Dependency<TEntityApi> dependency, string entityName)
        where TEntityApi : IOrchestrationEntitySignalSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<IBotSignalHandler>(CreateHandler);

        BotSignalHandler CreateHandler(IServiceProvider serviceProvider, TEntityApi entityApi)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(entityApi);

            return new(
                entityApi: entityApi,
                entityName: entityName.OrEmpty(),
                logger: serviceProvider.GetService<ILoggerFactory>()?.CreateLogger<BotSignalHandler>());
        }
    }

    public static BotBuilder GetBotBuilder(this Dependency<BotProvider> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return new(dependency);
    }

    private static Uri? GetWebAppBaseAddressOrThrow(this IConfigurationSection section)
    {
        var value = section["WebAppBaseAddress"];

        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            return uri;
        }

        throw new InvalidOperationException($"WebAppBaseAddress '{value}' is an invalid absolute uri");
    }
}