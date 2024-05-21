using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlowContextExtensions
{
    internal static async ValueTask<ChatFlowJump<T>> NextAsync<T, TValue>(
        this IChatFlowContext<T> context,
        ChoiceStepItem<TValue> selectedItem,
        ChoiceStepState state,
        ChoiceStepOption<T, TValue> option,
        CancellationToken cancellationToken)
    {
        var resultMessage = option.CreateResultMessage(selectedItem);

        Task[] tasks =
        [
            context.Api.SendMessageAsync(resultMessage, cancellationToken),
            context.Api.DeleteMessageSetAsync(state.MessageIds, cancellationToken)
        ];

        await Task.WhenAll(tasks).ConfigureAwait(false);
        return option.MapFlowState(selectedItem);
    }
}