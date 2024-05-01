using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class BotApi
{
    public async Task<BotMessage> SendMessageAsync(BotMessageSendRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var result = await InnerSendMessageAsync(request, cancellationToken).ConfigureAwait(false);
        return result.SuccessOrThrow(ToException);
    }

    public ValueTask<Result<BotMessage, Failure<TelegramBotFailureCode>>> SendMessageOrFailureAsync(
        BotMessageSendRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return InnerSendMessageAsync(request, cancellationToken);
    }

    private ValueTask<Result<BotMessage, Failure<TelegramBotFailureCode>>> InnerSendMessageAsync(
        BotMessageSendRequest request, CancellationToken cancellationToken)
    {
        var botRequest = new BotRequest<BotMessageSendRequest>(HttpVerb.Post, "sendMessage", request);
        return InnerSendOrFailureAsync<BotMessageSendRequest, BotMessage>(botRequest, cancellationToken);
    }
}