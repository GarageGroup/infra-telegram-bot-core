using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatApi
{
    public async Task<BotMessage?> EditTextMessageAsync(ChatMessageTextEditRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var @in = new BotMessageTextEditIn(chatId: chatId, messageId: request.MessageId, text: request.Text)
        {
            ParseMode = request.ParseMode,
            Entities = request.Entities,
            ReplyMarkup = request.ReplyMarkup
        };

        var result = await botApi.EditTextMessageAsync(@in, cancellationToken).ConfigureAwait(false);
        return result.SuccessOrThrow(ToException);
    }
}