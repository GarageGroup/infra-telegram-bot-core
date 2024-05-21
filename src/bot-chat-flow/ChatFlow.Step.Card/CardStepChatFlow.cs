using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public static class CardStepChatFlow
{
    public static ChatFlow<T> ExpectConfirmationOrSkip<T>(
        this ChatFlow<T> chatFlow, Func<IChatFlowContext<T>, ConfirmationCardOption<T>?> optionFactory)
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

            var state = context.StepState.Get<CardStepState>();
            if (state is null)
            {
                return context.ShowCardMessageAsync(option, cancellationToken);
            }

            return new(context.ParseDecision(state, option));
        }
    }

    public static ChatFlow<T> ShowEntityCardOrSkip<T>(
        this ChatFlow<T> chatFlow, Func<IChatFlowContext<T>, EntityCardOption?> optionFactory)
    {
        ArgumentNullException.ThrowIfNull(chatFlow);
        ArgumentNullException.ThrowIfNull(optionFactory);

        return chatFlow.On(SendEntityCardAsync);

        Task SendEntityCardAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(context);

            var option = optionFactory.Invoke(context);
            if (option is null)
            {
                return Task.CompletedTask;
            }

            var message = CardBuilder.BuildCardMessage<T>(option);
            return context.Api.SendMessageAsync(message, cancellationToken);
        }
    }
}