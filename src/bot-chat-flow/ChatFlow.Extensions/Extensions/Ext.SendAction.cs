using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlowExtensions
{
    public static ChatFlow<T> SendChatAction<T>(
        this ChatFlow<T> chatFlow,
        BotChatAction chatAction,
        Func<IChatFlowContext<T>, LocalizedString?>? temporaryMessageFactory = null,
        Func<IChatFlowContext<T>, BotMessage, T>? messageMapper = null)
    {
        ArgumentNullException.ThrowIfNull(chatFlow);
        return chatFlow.InnerSendChatActionOrSkip(InnerGetChatAction, temporaryMessageFactory, messageMapper);

        BotChatAction? InnerGetChatAction(IChatFlowContext<T> _)
            =>
            chatAction;
    }

    public static ChatFlow<T> SendChatActionOrSkip<T>(
        this ChatFlow<T> chatFlow,
        Func<IChatFlowContext<T>, BotChatAction?> chatActionFactory,
        Func<IChatFlowContext<T>, LocalizedString?>? temporaryMessageFactory = null,
        Func<IChatFlowContext<T>, BotMessage, T>? messageMapper = null)
    {
        ArgumentNullException.ThrowIfNull(chatFlow);
        ArgumentNullException.ThrowIfNull(chatActionFactory);

        return chatFlow.InnerSendChatActionOrSkip(chatActionFactory, temporaryMessageFactory, messageMapper);
    }

    private static ChatFlow<T> InnerSendChatActionOrSkip<T>(
        this ChatFlow<T> chatFlow,
        Func<IChatFlowContext<T>, BotChatAction?> chatActionFactory,
        Func<IChatFlowContext<T>, LocalizedString?>? temporaryMessageFactory,
        Func<IChatFlowContext<T>, BotMessage, T>? messageMapper)
    {
        return chatFlow.NextValue(InnerInvokeAsync);

        async ValueTask<T> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            var chatAction = chatActionFactory.Invoke(context);
            if (chatAction is null)
            {
                return context.FlowState;
            }

            var temporaryMessage = temporaryMessageFactory?.Invoke(context);
            var temporaryMessageSendTask = temporaryMessage switch
            {
                { ResourceNotFound: false } => context.Api.SendHtmlModeTextAndRemoveReplyKeyboardAsync(temporaryMessage, cancellationToken),
                _ => null
            };

            var chatActionSendTask = context.Api.SendChatActionAsync(chatAction.Value, cancellationToken);
            if (temporaryMessageSendTask is null)
            {
                await chatActionSendTask.ConfigureAwait(false);
                return context.FlowState;
            }

            await Task.WhenAll(chatActionSendTask, temporaryMessageSendTask).ConfigureAwait(false);

            if (messageMapper is null)
            {
                return context.FlowState;
            }

            var message = await temporaryMessageSendTask.ConfigureAwait(false);
            return messageMapper.Invoke(context, message);
        }
    }
}