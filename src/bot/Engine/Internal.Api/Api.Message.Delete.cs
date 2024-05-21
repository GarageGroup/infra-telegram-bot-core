using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatApi
{
    public async Task DeleteMessageAsync(ChatMessageDeleteRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // If message with ID doesn't exist method deleteMessage throws exception but deleteMessages returns success
        var @in = new BotMessageSetDeleteIn(chatId: chatId, messageIds: [request.MessageId]);

        var result = await botApi.DeleteMessageSetAsync(@in, cancellationToken).ConfigureAwait(false);
        _ = result.SuccessOrThrow(ToException);
    }
}