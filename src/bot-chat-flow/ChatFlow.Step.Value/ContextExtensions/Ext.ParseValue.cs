using System;
using System.Linq;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlowContextExtensions
{
    internal static ChatFlowJump<T> ParseValueOrRepeat<T, TValue>(
        this IChatFlowContext<T> context,
        ValueStepState<TValue> state,
        Func<string, Result<TValue, ChatRepeatState>> parse,
        Func<TValue, Result<T, ChatRepeatState>> forward)
    {
        var text = context.Update.Message?.Text;

        if (string.IsNullOrWhiteSpace(text))
        {
            return default;
        }

        return state.Suggestions.AsEnumerable()
            .GetValueOrAbsent(text)
            .Fold(GetSuccess, ParseOrFailure)
            .Forward(forward)
            .Fold(ChatFlowJump.Next, ChatFlowJump.Repeat<T>);

        static Result<TValue, ChatRepeatState> GetSuccess(TValue value)
            =>
            Result.Success(value);

        Result<TValue, ChatRepeatState> ParseOrFailure()
            =>
            parse.Invoke(text);
    }
}