using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlowExtensions
{
    public static ChatFlow<T> SendHtmlModeTextOrSkip<T>(
        this ChatFlow<T> chatFlow,
        Func<IChatFlowContext<T>, string?> textFactory,
        Func<IChatFlowContext<T>, BotMessage, T>? messageMapper = null)
    {
        ArgumentNullException.ThrowIfNull(chatFlow);
        ArgumentNullException.ThrowIfNull(textFactory);

        return chatFlow.NextValue(InnerInvokeAsync);

        async ValueTask<T> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            var text = textFactory.Invoke(context);
            if (string.IsNullOrWhiteSpace(text))
            {
                return context.FlowState;
            }

            var message = await context.Api.SendHtmlModeTextAndRemoveReplyKeyboardAsync(text, cancellationToken).ConfigureAwait(false);
            if (messageMapper is null)
            {
                return context.FlowState;
            }

            return messageMapper.Invoke(context, message);
        }
    }
}