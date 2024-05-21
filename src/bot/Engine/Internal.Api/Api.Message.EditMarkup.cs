using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatApi
{
    public async Task<BotMessage?> EditMessageReplyMarkupAsync(ChatMessageReplyMarkupEditRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var @in = new BotMessageReplyMarkupEditIn(chatId: chatId, messageId: request.MessageId)
        {
            ReplyMarkup = request.ReplyMarkup
        };

        var result = await botApi.EditMessageReplyMarkupAsync(@in, cancellationToken).ConfigureAwait(false);
        return result.SuccessOrThrow(ToException);
    }
}