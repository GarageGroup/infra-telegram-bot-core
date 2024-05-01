using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class BotApi
{
    public async Task<bool> EditTextMessageAsync(BotMessageTextEditRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var result = await InnerEditTextMessageAsync(request, cancellationToken).ConfigureAwait(false);
        return result.SuccessOrThrow(ToException);
    }

    public ValueTask<Result<bool, Failure<TelegramBotFailureCode>>> EditTextMessageOrFailureAsync(
        BotMessageTextEditRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return InnerEditTextMessageAsync(request, cancellationToken);
    }

    private ValueTask<Result<bool, Failure<TelegramBotFailureCode>>> InnerEditTextMessageAsync(
        BotMessageTextEditRequest request, CancellationToken cancellationToken)
    {
        var botRequest = new BotRequest<BotMessageTextEditRequest>(HttpVerb.Post, "editMessageText", request);
        return InnerSendOrFailureAsync<BotMessageTextEditRequest, bool>(botRequest, cancellationToken);
    }
}