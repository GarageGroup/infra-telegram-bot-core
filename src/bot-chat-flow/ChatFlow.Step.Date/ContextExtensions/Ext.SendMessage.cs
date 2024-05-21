using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlowContextExtensions
{
    internal static async ValueTask<ChatFlowJump<T>> SendMessageAsync<T>(
        this IChatFlowContext<T> context, DateStepOption<T> option, CancellationToken cancellationToken)
    {
        var state = new DateStepState
        {
            Suggestions = option.Suggestions.FlatMap(PipeSelf),
            InvalidDateText = option.InvalidDateText
        };

        var request = new ChatMessageSendRequest(option.Text)
        {
            ReplyMarkup = state.Suggestions.IsEmpty ? null : new BotReplyKeyboardMarkup
            {
                Keyboard = option.Suggestions.Map(ToKeyboardButtonRow),
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            }
        };

        context.StepState.Set(state);
        _ = await context.Api.SendMessageAsync(request, cancellationToken).ConfigureAwait(false);

        return default;

        static FlatArray<BotKeyboardButton> ToKeyboardButtonRow(FlatArray<KeyValuePair<string, DateOnly>> row)
            =>
            row.Map(ToKeyboardButton);

        static BotKeyboardButton ToKeyboardButton(KeyValuePair<string, DateOnly> suggesion)
            =>
            new(suggesion.Key);

        static FlatArray<KeyValuePair<string, DateOnly>> PipeSelf(FlatArray<KeyValuePair<string, DateOnly>> row)
            =>
            row;
    }
}