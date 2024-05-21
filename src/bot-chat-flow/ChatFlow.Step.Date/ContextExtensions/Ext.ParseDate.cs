using System;
using System.Linq;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlowContextExtensions
{
    internal static ChatFlowJump<T> ParseDateOrRepeat<T>(
        this IChatFlowContext<T> context, DateStepState state, DateStepOption<T> option)
    {
        var text = context.Update.Message?.Text;

        if (string.IsNullOrWhiteSpace(text))
        {
            return default;
        }

        return state.ParseDateOrFailure(text).Forward(option.Forward).Fold(ChatFlowJump.Next, ChatFlowJump.Repeat<T>);
    }

    private static Result<DateOnly, ChatRepeatState> ParseDateOrFailure(this DateStepState state, string text)
    {
        return state.Suggestions.AsEnumerable().GetValueOrAbsent(text).Fold(Result.Present, ParseOrFailure).MapFailure(CreateFlowFailure);

        Result<DateOnly, Unit> ParseOrFailure()
            =>
            DateParser.ParseOrFailure(text);

        ChatRepeatState CreateFlowFailure(Unit _)
            =>
            ChatRepeatState.From(state.InvalidDateText);
    }
}