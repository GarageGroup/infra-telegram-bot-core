using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatApi
{
    Task<BotMessage> SendHtmlModeTextAsync([AllowNull] string text, CancellationToken cancellationToken)
        =>
        SendMessageAsync(
            request: new(text)
            {
                ParseMode = BotParseMode.Html
            },
            cancellationToken: cancellationToken);

    Task<BotMessage> SendHtmlModeTextAndRemoveReplyKeyboardAsync([AllowNull] string text, CancellationToken cancellationToken)
        =>
        SendMessageAsync(
            request: new(text)
            {
                ParseMode = BotParseMode.Html,
                ReplyMarkup = new BotReplyKeyboardRemove()
            },
            cancellationToken: cancellationToken);

    Task<BotMessage> SendMessageAsync(ChatMessageSendRequest request, CancellationToken cancellationToken);

    Task<BotMessage?> EditTextMessageAsync(ChatMessageTextEditRequest request, CancellationToken cancellationToken);

    Task<BotMessage?> EditMessageReplyMarkupAsync(ChatMessageReplyMarkupEditRequest request, CancellationToken cancellationToken);

    Task DeleteMessageAsync(int messageId, CancellationToken cancellationToken)
        =>
        DeleteMessageAsync(
            request: new(messageId),
            cancellationToken: cancellationToken);

    Task DeleteMessageAsync(ChatMessageDeleteRequest request, CancellationToken cancellationToken);

    Task DeleteMessageSetAsync(FlatArray<int> messageIds, CancellationToken cancellationToken)
        =>
        DeleteMessageSetAsync(
            request: new()
            {
                MessageIds = messageIds
            },
            cancellationToken: cancellationToken);

    Task DeleteMessageSetAsync(ChatMessageSetDeleteRequest request, CancellationToken cancellationToken);

    Task SendChatActionAsync(BotChatAction chatAction, CancellationToken cancellationToken)
        =>
        SendChatActionAsync(
            request: new()
            {
                Action = chatAction
            },
            cancellationToken: cancellationToken);

    Task SendChatActionAsync(ChatActionSendRequest request, CancellationToken cancellationToken);

    Task<BotUser> GetMeAsync(CancellationToken cancellationToken);

    Task<ChatFileLink> GetFileLinkAsync(string fileId, CancellationToken cancellationToken);

    Task<BotMessage> SendPhotoAsync(ChatPhotoSendRequest request, CancellationToken cancellationToken);
}