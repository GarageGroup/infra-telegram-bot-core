using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlowExtensions
{
    public static ChatFlow<T> EditMessageOrSkip<T>(
        this ChatFlow<T> chatFlow,
        Func<IChatFlowContext<T>, ChatMessageTextEditRequest?> requestFactory,
        Func<IChatFlowContext<T>, BotMessage, T>? messageMapper = null)
    {
        ArgumentNullException.ThrowIfNull(chatFlow);
        ArgumentNullException.ThrowIfNull(requestFactory);

        return chatFlow.NextValue(InnerInvokeAsync);

        async ValueTask<T> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            var request = requestFactory.Invoke(context);
            if (request is null)
            {
                return context.FlowState;
            }

            var message = await context.Api.EditTextMessageAsync(request, cancellationToken).ConfigureAwait(false);
            if (message is null || messageMapper is null)
            {
                return context.FlowState;
            }

            return messageMapper.Invoke(context, message);
        }
    }
}