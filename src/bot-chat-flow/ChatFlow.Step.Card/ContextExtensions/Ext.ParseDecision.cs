using System;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlowContextExtensions
{
    internal static ChatFlowJump<T> ParseDecision<T>(
        this IChatFlowContext<T> context, CardStepState state, ConfirmationCardOption<T> option)
    {
        if (context.Update.Message is null)
        {
            return default;
        }

        if (context.Update.Message.WebAppData is not null && option.Keyboard.WebAppButton is not null)
        {
            return option.Keyboard.WebAppButton.Forward(context.Update.Message.WebAppData).Fold(ChatFlowJump.Next, ChatFlowJump.Repeat<T>);
        }

        if (string.IsNullOrWhiteSpace(context.Update.Message.Text))
        {
            return default;
        }

        if (string.Equals(state.ConfirmButtonText, context.Update.Message.Text, StringComparison.InvariantCultureIgnoreCase))
        {
            return context.FlowState;
        }

        if (string.Equals(state.CancelButtonText, context.Update.Message.Text, StringComparison.InvariantCultureIgnoreCase))
        {
            return ChatBreakState.From(option.Keyboard.CancelText);
        }

        return default;
    }
}