using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public static class ValueStepChatFlow
{
    public static ChatFlow<T> ExpectValueOrSkip<T, TValue>(
        this ChatFlow<T> chatFlow, Func<IChatFlowContext<T>, ValueStepOption<T, TValue>?> optionFactory)
    {
        ArgumentNullException.ThrowIfNull(chatFlow);
        ArgumentNullException.ThrowIfNull(optionFactory);

        return chatFlow.ForwardValue(InvokeAsync);

        ValueTask<ChatFlowJump<T>> InvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(context);

            var option = optionFactory.Invoke(context);
            if (option is null)
            {
                return new(context.FlowState);
            }

            var state = context.StepState.Get<ValueStepState<TValue>>();
            if (state is null)
            {
                return context.SendMessageAsync(option.Text, option.Suggestions, cancellationToken);
            }

            return new(context.ParseValueOrRepeat(state, option.Parse, option.Forward));
        }
    }

    public static ChatFlow<T> ExpectTextOrSkip<T>(
        this ChatFlow<T> chatFlow, Func<IChatFlowContext<T>, TextStepOption<T>?> optionFactory)
    {
        ArgumentNullException.ThrowIfNull(chatFlow);
        ArgumentNullException.ThrowIfNull(optionFactory);

        return chatFlow.ForwardValue(InvokeAsync);

        ValueTask<ChatFlowJump<T>> InvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(context);

            var option = optionFactory.Invoke(context);
            if (option is null)
            {
                return new(context.FlowState);
            }

            var state = context.StepState.Get<ValueStepState<string>>();
            if (state is null)
            {
                return context.SendMessageAsync(option.Text, option.Suggestions, cancellationToken);
            }

            return new(context.ParseValueOrRepeat(state, ToSuccessResult, option.Forward));
        }

        static Result<string, ChatRepeatState> ToSuccessResult(string text)
            =>
            Result.Success(text);
    }
}