using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class CommandMiddleware
{
    public ValueTask<TurnResult> InvokeAsync(IChatContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);
        var chatContext = (ChatContext)context;

        var commandName = GetCommandName(context.Update);
        if (string.IsNullOrWhiteSpace(commandName) is false)
        {
            var commandIn = FindCommandInput(commandName);
            if (commandIn is not null)
            {
                return InnerStartCommandAsync(chatContext, commandIn.Value, cancellationToken);
            }

            return InnerStartWithoutNameAsync(chatContext, cancellationToken);
        }

        var commandData = context.State.GetValue<CommandData>(RootStateKey);
        if (commandData is not null)
        {
            return InnerContinueCommandAsync(chatContext, commandData, cancellationToken);
        }

        return InnerStartWithoutNameAsync(chatContext, cancellationToken);
    }

    private ValueTask<TurnResult> InnerStartWithoutNameAsync(ChatContext context, CancellationToken cancellationToken)
    {
        var input = FindCommandInput(context.Update);
        if (input is not null)
        {
            return InnerStartCommandAsync(context, input.Value, cancellationToken);
        }

        var result = new TurnResult(context, TurnState.Waiting);
        return new(result);
    }

    private static async ValueTask<TurnResult> InnerStartCommandAsync(
        ChatContext context, (ICommand Command, object Value) input, CancellationToken cancellationToken)
    {
        var jsonValue = JsonSerializer.SerializeToElement(input.Value, SerializerOptions);
        var chatState = new ChatState();

        var data = new CommandData(input.Command.Type, jsonValue, chatState);
        context.State.SetValue(RootStateKey, data);

        var result = await input.Command.InvokeAsync(context.WithState(chatState), input.Value, cancellationToken).ConfigureAwait(false);

        if (result.State is TurnState.Waiting)
        {
            var nextData = new CommandData(input.Command.Type, data.Input, result.Context.State);
            context.State.SetValue(RootStateKey, nextData);
        }
        else
        {
            context.State.RemoveValue(RootStateKey);
        }

        return result;
    }

    private async ValueTask<TurnResult> InnerContinueCommandAsync(
        ChatContext context, CommandData data, CancellationToken cancellationToken)
    {
        var command = FindCommandOrThrow(data.Type.OrEmpty());
        var commandContext = context.WithState(data.State ?? new());

        var result = await command.InvokeAsync(commandContext, data.Input, SerializerOptions, cancellationToken).ConfigureAwait(false);
        if (result.State is TurnState.Waiting)
        {
            var nextData = new CommandData(command.Type, data.Input, result.Context.State);
            context.State.SetValue(RootStateKey, nextData);
        }
        else
        {
            context.State.RemoveValue(RootStateKey);
        }

        return result;
    }
}