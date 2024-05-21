using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatApi
{
    public async Task<BotMessage> SendMessageAsync(ChatMessageSendRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var @in = new BotMessageSendIn(chatId: chatId, text: request.Text)
        {
            ParseMode = request.ParseMode,
            Entities = request.Entities,
            DisableWebPagePreview = request.DisableWebPagePreview,
            DisableNotification = request.DisableNotification,
            ProtectContent = request.ProtectContent,
            ReplyToMessageId = request.ReplyToMessageId,
            AllowSendingWithoutReply = request.AllowSendingWithoutReply,
            ReplyMarkup = request.ReplyMarkup
        };

        var result = await botApi.SendMessageAsync(@in, cancellationToken).ConfigureAwait(false);
        return result.SuccessOrThrow(ToException);
    }
}