using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class UserAuthorizationApi
{
    public async ValueTask<Result<Unit, Failure<Unit>>> UnauthorizeAsync(UserUnauthorizeIn input, CancellationToken cancellationToken)
    {
        var @in = BotUserJson.BuildDataverseUpdateInput(
            user: new(
                botId: input.BotId,
                chatId: input.ChatId)
            {
                IsSignedOut = true
            });

        var result = await dataverseUpdateApi.UpdateEntityAsync(@in, cancellationToken).ConfigureAwait(false);
        return result.Recover(RecoverOrFailure);

        static Result<Unit, Failure<Unit>> RecoverOrFailure(Failure<DataverseFailureCode> failure)
            =>
            failure.FailureCode switch
            {
                DataverseFailureCode.RecordNotFound => Result.Success<Unit>(default),
                _ => failure.WithFailureCode<Unit>(default)
            };
    }
}