using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class BotApi
{
    public async Task<bool> DeleteMessageAsync(BotMessageDeleteRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var result = await InnerDeleteMessageAsync(request, cancellationToken).ConfigureAwait(false);
        return result.SuccessOrThrow(ToException);
    }

    public ValueTask<Result<bool, Failure<TelegramBotFailureCode>>> DeleteMessageOrFailureAsync(
        BotMessageDeleteRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return InnerDeleteMessageAsync(request, cancellationToken);
    }

    private ValueTask<Result<bool, Failure<TelegramBotFailureCode>>> InnerDeleteMessageAsync(
        BotMessageDeleteRequest request, CancellationToken cancellationToken)
    {
        var botRequest = new BotRequest<BotMessageDeleteRequest>(HttpVerb.Post, "deleteMessage", request);
        return InnerSendOrFailureAsync<BotMessageDeleteRequest, bool>(botRequest, cancellationToken);
    }
}