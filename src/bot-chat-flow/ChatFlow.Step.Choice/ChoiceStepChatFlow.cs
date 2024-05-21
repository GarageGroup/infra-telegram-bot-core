using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public static class ChoiceStepChatFlow
{
    public static ChatFlow<T> ExpectChoiceValueOrSkip<T, TValue>(
        this ChatFlow<T> chatFlow,
        Func<IChatFlowContext<T>, ChoiceStepOption<T, TValue>?> optionFactory)
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

            var state = context.StepState.Get<ChoiceStepState>();
            if (state.MessageIds.IsEmpty)
            {
                return context.ShowChoiceSetAsync(state, option, cancellationToken);
            }

            var selectedItem = context.GetSelectedItem<TValue>(state);
            if (selectedItem is not null)
            {
                return context.NextAsync(selectedItem, state, option, cancellationToken);
            }

            if (string.IsNullOrEmpty(context.Update.Message?.Text) is false)
            {
                return context.ShowChoiceSetAsync(state, option, cancellationToken);
            }

            return default;
        }
    }
}