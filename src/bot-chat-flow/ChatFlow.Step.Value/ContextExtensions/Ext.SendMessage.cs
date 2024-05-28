using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlowContextExtensions
{
    internal static async ValueTask<ChatFlowJump<T>> SendMessageAsync<T, TValue>(
        this IChatFlowContext<T> context,
        string messageText,
        FlatArray<FlatArray<KeyValuePair<string, TValue>>> suggestions,
        CancellationToken cancellationToken)
    {
        var state = new ValueStepState<TValue>
        {
            Suggestions = suggestions.FlatMap(PipeSelf)
        };

        var request = new ChatMessageSendRequest(messageText)
        {
            ReplyMarkup = state.Suggestions.IsEmpty ? new BotReplyKeyboardRemove() : new BotReplyKeyboardMarkup
            {
                Keyboard = suggestions.Map(ToKeyboardButtonRow),
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            }
        };

        context.StepState.Set(state);
        _ = await context.Api.SendMessageAsync(request, cancellationToken).ConfigureAwait(false);

        return default;

        static FlatArray<BotKeyboardButton> ToKeyboardButtonRow(FlatArray<KeyValuePair<string, TValue>> row)
            =>
            row.Map(ToKeyboardButton);

        static BotKeyboardButton ToKeyboardButton(KeyValuePair<string, TValue> suggesion)
            =>
            new(suggesion.Key);

        static FlatArray<KeyValuePair<string, TValue>> PipeSelf(FlatArray<KeyValuePair<string, TValue>> row)
            =>
            row;
    }
}