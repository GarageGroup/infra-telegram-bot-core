using System;
using System.Collections.Generic;
using System.Linq;
using PrimeFuncPack;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed class BotBuilder : IBotBuilder
{
    private readonly Dependency<BotProvider> botProvider;

    private readonly List<Dependency<IChatMiddleware>> middlewares = [];

    internal BotBuilder(Dependency<BotProvider> botProvider)
        =>
        this.botProvider = botProvider;

    public BotBuilder Next(Dependency<IChatMiddleware> middleware)
    {
        ArgumentNullException.ThrowIfNull(middleware);

        middlewares.Add(middleware);
        return this;
    }

    public BotCommandBuilder UseCommands()
        =>
        new(botProvider, middlewares);

    public Dependency<IBotWebHookHandler> BuildWebHookHandler()
    {
        return Dependency.From<IBotWebHookHandler>(ResolveHandler);

        BotWebHookHandler ResolveHandler(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

            var commandSender = new CommandMiddleware(default);

            return new(
                botContext: botProvider.Resolve(serviceProvider).GetBotContext(commandSender),
                middlewares: middlewares.Select(ResolveValue).ToFlatArray());

            T ResolveValue<T>(Dependency<T> dependency)
                =>
                dependency.Resolve(serviceProvider);
        }
    }
}