using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class BlobBotStorage
{
    public async Task SaveChatStateAsync(long chatId, ChatState chatState, CancellationToken cancellationToken)
    {
        var @in = new HttpSendIn(
            method: HttpVerb.Put,
            requestUri: BuildFileUrl(chatId, Role.Writer))
        {
            Headers =
            [
                new(BlobTypeHeaderName, BlobTypeHeaderValue)
            ],
            Body = HttpBody.SerializeAsJson(chatState ?? new(), SerializerOptions),
            SuccessType = HttpSuccessType.OnlyStatusCode
        };

        var result = await httpApi.SendAsync(@in, cancellationToken).ConfigureAwait(false);
        _ = result.SuccessOrThrow(ToException);

        Exception ToException(HttpSendFailure failure)
            =>
            failure.ToStandardFailure($"An unexpected http result was received when trying to write chat {chatId} state").ToException();
    }
}