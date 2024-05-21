using System;
using System.Collections.Generic;
using System.Linq;
using PrimeFuncPack;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed class BotCommandBuilder
{
    private readonly Dependency<BotProvider> botProvider;

    private readonly List<Dependency<IChatMiddleware>> middlewares;

    private readonly List<Dependency<ICommand>> commands;

    internal BotCommandBuilder(Dependency<BotProvider> botProvider, List<Dependency<IChatMiddleware>> middlewares)
    {
        this.botProvider = botProvider;
        this.middlewares = middlewares;
        commands = [];
    }

    public BotCommandBuilder With<TIn, TOut>(string name, Dependency<IChatCommand<TIn, TOut>> command)
        where TIn : IChatCommandIn<TOut>, new()
    {
        ArgumentNullException.ThrowIfNull(command);

        commands.Add(command.Map<ICommand>(CreateCommand));
        return this;

        Command<TIn, TOut> CreateCommand(IChatCommand<TIn, TOut> chatCommand)
        {
            ArgumentNullException.ThrowIfNull(chatCommand);
            return new(chatCommand, Init, chatCommand as IChatCommandParser<TIn>, name);
        }

        static TIn Init(string _)
            =>
            new();
    }

    public BotCommandBuilder With<TIn, TOut>(string name, Dependency<IChatCommand<TIn, TOut>> command, bool isWeekMatch)
        where TIn : IChatCommandIn<TIn, TOut>
    {
        ArgumentNullException.ThrowIfNull(command);

        commands.Add(command.Map<ICommand>(CreateCommand));
        return this;

        Command<TIn, TOut> CreateCommand(IChatCommand<TIn, TOut> chatCommand)
        {
            ArgumentNullException.ThrowIfNull(chatCommand);
            return new(chatCommand, TIn.Init, chatCommand as IChatCommandParser<TIn>, name, isWeekMatch);
        }
    }

    public BotCommandBuilder With<TIn, TOut>(Dependency<IChatCommand<TIn, TOut>> command)
        where TIn : IChatCommandIn<TOut>
    {
        ArgumentNullException.ThrowIfNull(command);

        commands.Add(command.Map<ICommand>(CreateCommand));
        return this;

        static Command<TIn, TOut> CreateCommand(IChatCommand<TIn, TOut> chatCommand)
        {
            ArgumentNullException.ThrowIfNull(chatCommand);
            return new(chatCommand, default, chatCommand as IChatCommandParser<TIn>);
        }
    }

    public Dependency<IBotWebHookHandler> BuildWebHookHandler()
    {
        return Dependency.From<IBotWebHookHandler>(ResolveHandler);

        BotWebHookHandler ResolveHandler(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

            var commandMiddleware = new CommandMiddleware(commands.Select(ResolveValue).ToFlatArray());

            return new(
                botContext: botProvider.Resolve(serviceProvider).GetBotContext(commandMiddleware),
                middlewares: middlewares.Select(ResolveValue).ToFlatArray().Concat(commandMiddleware));

            T ResolveValue<T>(Dependency<T> dependency)
                =>
                dependency.Resolve(serviceProvider);
        }
    }
}