using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class UserAuthorizationApi
{
    public async ValueTask<Result<UserAuthorizeOut, Failure<Unit>>> AuthorizeAsync(
        UserAuthorizeIn input, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(input);

        var azureUserInput = new HttpSendIn(HttpVerb.Get, AzureUserJson.RequestUrl)
        {
            Headers =
            [
                new("Authorization", $"Bearer {input.AccessToken}")
            ]
        };

        var azureUserResult = await httpApi.SendAsync(azureUserInput, cancellationToken).ConfigureAwait(false);
        if (azureUserResult.IsFailure)
        {
            return azureUserResult.FailureOrThrow().ToStandardFailure(AzureUserErrorBaseMessage).WithFailureCode<Unit>(default);
        }

        var azureUser = azureUserResult.SuccessOrThrow().Body.DeserializeFromJson<AzureUserJson>();

        var userId = azureUser.Id?.Split('@')[0];
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Failure.Create($"Azure userId '{userId}' is invalid");
        }

        var dataverseGetInput = SystemUserJson.BuildDataverseGetInput(userId);
        var dataverseGetResult = await dataverseGetApi.GetEntityAsync<SystemUserJson>(dataverseGetInput, cancellationToken).ConfigureAwait(false);

        if (dataverseGetResult.IsFailure)
        {
            var failure = dataverseGetResult.FailureOrThrow();
            if (failure.FailureCode is DataverseFailureCode.RecordNotFound)
            {
                return default(UserAuthorizeOut);
            }

            return dataverseGetResult.FailureOrThrow().WithFailureCode<Unit>(default);
        }

        var dataverseUser = dataverseGetResult.SuccessOrThrow().Value;
        if (dataverseUser.IsDisabled)
        {
            return new UserAuthorizeOut
            {
                IsDisabled = true
            };
        }

        var botUser = new BotUserJson.In(
            botId: input.BotId,
            chatId: input.ChatId)
        {
            Name = $"{input.BotName} - {dataverseUser.FullName}",
            UserLookupValue = BotUserJson.BuildUserLookupValue(dataverseUser.SystemUserId),
            LanguageCode = input.LanguageCode,
            TimeZone = azureUser.MailboxSettings?.TimeZone,
            IsSignedOut = false
        };

        var dataverseUpdateInput = BotUserJson.BuildDataverseUpdateInput(botUser);
        var dataverseUpdateResult = await dataverseUpdateApi.UpdateEntityAsync(dataverseUpdateInput, cancellationToken).ConfigureAwait(false);

        if (dataverseUpdateResult.IsFailure)
        {
            return dataverseUpdateResult.FailureOrThrow().WithFailureCode<Unit>(default);
        }

        return new UserAuthorizeOut
        {
            User = new(input.ChatId)
            {
                Identity = new(dataverseUser.SystemUserId, dataverseUser.FullName),
                Culture = GetCultureInfo(botUser.LanguageCode),
                TimeZone = TryFindSystemTimeZoneById(botUser.TimeZone)
            },
            IsDisabled = false
        };
    }
}