using System;
using System.Collections.Generic;
using PrimeFuncPack;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed class BotBuilder
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
}