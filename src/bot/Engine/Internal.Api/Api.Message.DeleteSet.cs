using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatApi
{
    public async Task DeleteMessageSetAsync(ChatMessageSetDeleteRequest request, CancellationToken cancellationToken)
    {
        if (request.MessageIds.IsEmpty)
        {
            return;
        }

        var @in = new BotMessageSetDeleteIn(chatId: chatId, messageIds: request.MessageIds);

        var result = await botApi.DeleteMessageSetAsync(@in, cancellationToken).ConfigureAwait(false);
        _ = result.SuccessOrThrow(ToException);
    }
}