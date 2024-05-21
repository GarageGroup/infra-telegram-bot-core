using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatApi
{
    public async Task SendChatActionAsync(ChatActionSendRequest request, CancellationToken cancellationToken)
    {
        var @in = new BotChatActionSendIn(chatId, request.Action);

        var result = await botApi.SendChatActionAsync(@in, cancellationToken).ConfigureAwait(false);
        _ = result.SuccessOrThrow(ToException);
    }
}