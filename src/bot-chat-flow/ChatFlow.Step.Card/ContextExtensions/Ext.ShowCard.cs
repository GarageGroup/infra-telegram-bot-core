using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlowContextExtensions
{
    internal static async ValueTask<ChatFlowJump<T>> ShowCardMessageAsync<T>(
        this IChatFlowContextBase context, ConfirmationCardOption<T> option, CancellationToken cancellationToken)
    {
        var state = new CardStepState
        {
            ConfirmButtonText = option.Keyboard.ConfirmButtonText,
            CancelButtonText = option.Keyboard.CancelButtonText
        };

        context.StepState.Set(state);

        var cardMessage = CardBuilder.BuildCardMessage(option.Entity, option.Keyboard);
        _ = await context.Api.SendMessageAsync(cardMessage, cancellationToken).ConfigureAwait(false);

        return default;
    }
}