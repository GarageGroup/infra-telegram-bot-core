using System;
using System.Collections.Generic;
using System.Text.Json;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed partial class CommandMiddleware(FlatArray<ICommand> commands) : IChatMiddleware, IChatCommandSender
{
    private const string RootStateKey = "root";

    private const string ChildStateKey = "child";

    private static readonly JsonSerializerOptions SerializerOptions
        =
        new(JsonSerializerDefaults.Web);

    public IReadOnlyDictionary<string, string> AllCommandTypes
    {
        get
        {
            var types = new Dictionary<string, string>(commands.Length);

            foreach (var command in commands)
            {
                types[command.Name.OrEmpty()] = command.Type;
            }

            return types;
        }
    }

    private static string? GetCommandName(ChatUpdate update)
    {
        if (string.IsNullOrWhiteSpace(update.Message?.Text) || update.Message.Entities.IsEmpty)
        {
            return null;
        }

        var entity = update.Message.Entities[0];
        if ((entity.Type is not BotMessageEntityType.BotCommand) || (entity.Offset is not 0))
        {
            return null;
        }

        return update.Message.Text[1..entity.Length];
    }

    private (ICommand, object)? FindCommandInput(string name)
    {
        foreach (var command in commands)
        {
            var value = command.ParseByName(name);
            if (value.IsPresent)
            {
                return (command, value.OrThrow());
            }
        }

        return null;
    }

    private (ICommand, object)? FindCommandInput(ChatUpdate update)
    {
        foreach (var command in commands)
        {
            var value = command.ParseValue(update);
            if (value.IsPresent)
            {
                return (command, value.OrThrow());
            }
        }

        return null;
    }

    private ICommand FindCommandOrThrow(string type)
    {
        foreach (var command in commands)
        {
            if (string.Equals(type, command.Type, StringComparison.InvariantCultureIgnoreCase) is false)
            {
                continue;
            }

            return command;
        }

        throw new InvalidOperationException($"Command by type '{type}' was not found");
    }

    private Command<TIn, TOut> FindCommandOrThrow<TIn, TOut>(string type)
        where TIn : IChatCommandIn<TOut>
    {
        foreach (var command in commands)
        {
            if (command is not Command<TIn, TOut> possibleCommand)
            {
                continue;
            }

            if (string.Equals(type, command.Type, StringComparison.InvariantCultureIgnoreCase))
            {
                return possibleCommand;
            }
        }

        throw new InvalidOperationException($"Command '{typeof(Command<TIn, TOut>)}' with type '{type}' was not found");
    }

    private sealed record class CommandData(string? Type, JsonElement Input, ChatState? State);
}