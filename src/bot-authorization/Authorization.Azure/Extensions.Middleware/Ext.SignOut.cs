using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Telegram.Bot;

partial class MiddlewareExtensions
{
    internal static async ValueTask<TurnResult> SignOutAsync(
        this IChatContext context, IUserUnauthorizeSupplier authorizationApi, CancellationToken cancellationToken)
    {
        var bot = await context.Api.GetMeAsync(cancellationToken).ConfigureAwait(false);

        var @in = new UserUnauthorizeIn(botId: bot.Id, chatId: context.Update.Chat.Id);
        var result = await authorizationApi.UnauthorizeAsync(@in, cancellationToken).ConfigureAwait(false);

        return result.Fold(OnSuccess, OnFailure);

        TurnResult OnSuccess(Unit _)
            =>
            context.CreateTurnResult(TurnState.Cancelled);

        TurnResult OnFailure(Failure<Unit> failure)
        {
            var logger = context.GetLogger<AuthorizationMiddleware>();
            logger.LogError(failure.SourceException, "SignOut error: {failureMessage}", failure.FailureMessage);

            return context.CreateTurnResult(TurnState.Cancelled);
        }
    }
}