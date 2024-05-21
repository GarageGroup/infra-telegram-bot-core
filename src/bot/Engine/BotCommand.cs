using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;

namespace GarageGroup.Infra.Telegram.Bot;

public static class BotCommand
{
    public static Dependency<IChatCommand<BotInfoCommandIn, Unit>> UseBotInfoCommand()
    {
        return Dependency.From<IChatCommand<BotInfoCommandIn, Unit>>(Resolve);

        static BotInfoCommand Resolve(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

            return new(
                section: serviceProvider.GetRequiredService<IConfiguration>().GetSection("Info"));
        }
    }

    public static Dependency<IChatCommand<MenuCommandIn, Unit>> UseMenuCommand()
        =>
        Dependency.Of<IChatCommand<MenuCommandIn, Unit>>(MenuCommand.Instance);

    public static Dependency<IChatCommand<StopCommandIn, Unit>> UseStopCommand()
        =>
        Dependency.Of<IChatCommand<StopCommandIn, Unit>>(StopCommand.Instance);

    public static Dependency<IBotApi> UseBotApi(this Dependency<BotProvider> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(GetBotApi);

        static IBotApi GetBotApi(BotProvider provider)
            =>
            provider.BotApi;
    }

    public static Dependency<IBotStorage> UseBotStorage(this Dependency<BotProvider> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(GetBotStorage);

        static IBotStorage GetBotStorage(BotProvider provider)
            =>
            provider.BotStorage;
    }
}