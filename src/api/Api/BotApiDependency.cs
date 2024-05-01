using System;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace GarageGroup.Infra.Telegram.Bot;

public static class BotApiDependency
{
    private const string DefaultSectionName = "TelegramBot";

    public static Dependency<IBotApi> UseTelegramBotApi(this Dependency<IHttpApi> dependency, string sectionName = DefaultSectionName)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<IBotApi>(CreateApi);

        BotApi CreateApi(IServiceProvider serviceProvider, IHttpApi httpApi)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(httpApi);

            var token = serviceProvider.GetServiceOrThrow<IConfiguration>()[$"{sectionName.OrEmpty()}:Token"];

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException("Telegram Bot Token must be specified in the configuration");
            }

            return new(
                httpApi: httpApi,
                option: new(token));
        }
    }

    public static Dependency<IBotApi> UseTelegramBotApi(this Dependency<IHttpApi, BotApiOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<IBotApi>(CreateApi);

        static BotApi CreateApi(IHttpApi httpApi, BotApiOption option)
        {
            ArgumentNullException.ThrowIfNull(httpApi);
            ArgumentNullException.ThrowIfNull(option);

            return new(httpApi, option);
        }
    }
}