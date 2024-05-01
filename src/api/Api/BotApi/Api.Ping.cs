using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class BotApi
{
    public async ValueTask<Result<Unit, Failure<Unit>>> PingAsync(Unit input, CancellationToken cancellationToken)
    {
        var botRequest = new BotRequest<object>(HttpVerb.Get, "getMe", default);

        var result = await InnerSendOrFailureAsync<object, Unit>(botRequest, cancellationToken).ConfigureAwait(false);
        return result.MapFailure(InnerMapFailure);

        static Failure<Unit> InnerMapFailure(Failure<TelegramBotFailureCode> failure)
            =>
            failure.WithFailureCode<Unit>(default);
    }
}