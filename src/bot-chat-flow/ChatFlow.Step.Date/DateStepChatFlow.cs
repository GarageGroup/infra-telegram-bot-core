using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public static class DateStepChatFlow
{
    public static ChatFlow<T> ExpectDateOrSkip<T>(
        this ChatFlow<T> chatFlow, Func<IChatFlowContext<T>, DateStepOption<T>?> optionFactory)
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

            var state = context.StepState.Get<DateStepState>();
            if (state is null)
            {
                return context.SendMessageAsync(option, cancellationToken);
            }

            return new(context.ParseDateOrRepeat(state, option));
        }
    }
}