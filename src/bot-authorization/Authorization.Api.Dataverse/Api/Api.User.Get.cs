using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class UserAuthorizationApi
{
    public async ValueTask<Result<ChatUserGetOut, Failure<Unit>>> GetChatUserAsync(
        ChatUserGetIn input, CancellationToken cancellationToken)
    {
        var @in = BotUserJson.BuildDataverseGetInput(botId: input.BotId, chatId: input.ChatId);
        var result = await dataverseGetApi.GetEntityAsync<BotUserJson.Out>(@in, cancellationToken).ConfigureAwait(false);

        return result.Recover(MapFailure, MapSuccess);

        static ChatUserGetOut MapSuccess(DataverseEntityGetOut<BotUserJson.Out> success)
            =>
            new()
            {
                User = new ChatUser(success.Value.ChatId)
                {
                    Identity = new(success.Value.User.SystemUserId, success.Value.User.FullName),
                    Culture = GetCultureInfo(success.Value.LanguageCode),
                    TimeZone = TryFindSystemTimeZoneById(success.Value.TimeZone)
                },
                IsDisabled = success.Value.User.IsDisabled,
                IsSignedOut = success.Value.IsSignedOut,
            };

        static Result<ChatUserGetOut, Failure<Unit>> MapFailure(Failure<DataverseFailureCode> failure)
            =>
            failure.FailureCode switch
            {
                DataverseFailureCode.RecordNotFound => default(ChatUserGetOut),
                _ => failure.WithFailureCode<Unit>(default)
            };
    }
}