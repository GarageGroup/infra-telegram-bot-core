using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class BotApi
{
    public async Task<bool> DeleteMessageSetAsync(BotMessageSetDeleteRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var result = await InnerDeleteMessageSetOrFailureAsync(request, cancellationToken).ConfigureAwait(false);
        return result.SuccessOrThrow(ToException);
    }

    public ValueTask<Result<bool, Failure<TelegramBotFailureCode>>> DeleteMessageSetOrFailureAsync(
        BotMessageSetDeleteRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return InnerDeleteMessageSetOrFailureAsync(request, cancellationToken);
    }

    private ValueTask<Result<bool, Failure<TelegramBotFailureCode>>> InnerDeleteMessageSetOrFailureAsync(
        BotMessageSetDeleteRequest request, CancellationToken cancellationToken)
    {
        var botRequest = new BotRequest<BotMessageSetDeleteRequest>(HttpVerb.Post, "deleteMessages", request);
        return InnerSendOrFailureAsync<BotMessageSetDeleteRequest, bool>(botRequest, cancellationToken);
    }
}