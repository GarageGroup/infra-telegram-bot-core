using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class BlobBotStorage
{
    public async Task<ChatState> GetChatStateAsync(long chatId, CancellationToken cancellationToken)
    {
        var @in = new HttpSendIn(
            method: HttpVerb.Get,
            requestUri: BuildFileUrl(chatId, Role.Reader));

        var result = await httpApi.SendAsync(@in, cancellationToken).ConfigureAwait(false);
        return result.Fold(ReadChatState, ReadFromFailureOrThrow);

        static ChatState ReadChatState(HttpSendOut success)
            =>
            success.Body.Content?.ToObjectFromJson<ChatState>(SerializerOptions) ?? new();

        ChatState ReadFromFailureOrThrow(HttpSendFailure failure)
        {
            if (failure.StatusCode is HttpFailureCode.NotFound)
            {
                return new();
            }

            throw failure.ToStandardFailure($"An unexpected http result was received when trying to read chat {chatId} state").ToException();
        }
    }
}