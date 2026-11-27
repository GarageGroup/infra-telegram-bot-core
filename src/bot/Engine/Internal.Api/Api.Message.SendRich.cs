using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatApi
{
    public async Task<BotMessage> SendRichMessageAsync(ChatRichMessageSendRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var @in = new BotRichMessageSendIn(chatId: chatId, richMessage: request.RichMessage)
        {
            BusinessConnectionId = request.BusinessConnectionId,
            MessageThreadId = request.MessageThreadId,
            DisableNotification = request.DisableNotification,
            ProtectContent = request.ProtectContent,
            MessageEffectId = request.MessageEffectId,
            ReplyParameters = request.ReplyParameters,
            ReplyMarkup = request.ReplyMarkup
        };

        var result = await botApi.SendRichMessageAsync(@in, cancellationToken).ConfigureAwait(false);
        return result.SuccessOrThrow(ToException);
    }
}
