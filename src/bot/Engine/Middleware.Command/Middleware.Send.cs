using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class CommandMiddleware
{
    public async ValueTask<ChatCommandResult<TOut>> SendAsync<TIn, TOut>(
        ChatCommandRequest<TIn, TOut> request, CancellationToken cancellationToken)
        where TIn : IChatCommandIn<TOut>
    {
        ArgumentNullException.ThrowIfNull(request);
        var chatContext = (ChatContext)request.Context;

        var childData = chatContext.State.GetValue<CommandData>(ChildStateKey);
        var childState = childData?.State ?? new();

        var command = FindCommandOrThrow<TIn, TOut>(childData is null ? TIn.Type : childData.Type.OrEmpty());
        if (childData is null)
        {
            var jsonValue = JsonSerializer.SerializeToElement(request.Value, SerializerOptions);
            childData = new(command.Type, jsonValue, childState);

            chatContext.State.SetValue(ChildStateKey, childData);
        }

        var childRequest = new ChatCommandRequest<TIn, TOut>(chatContext.WithState(childState), request.Value);
        var result = await command.SendAsync(childRequest, cancellationToken).ConfigureAwait(false);

        if (result.State is TurnState.Waiting)
        {
            var nextData = new CommandData(command.Type, childData.Input, result.Context.State);
            chatContext.State.SetValue(ChildStateKey, nextData);
        }
        else
        {
            chatContext.State.RemoveValue(ChildStateKey);
        }

        return result;
    }
}