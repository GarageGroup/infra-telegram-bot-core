using System;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed record class ResultState : IChatStateValue
{
    public static string Key { get; } = "authorizationResult";

    public ResultState(string accessToken)
        =>
        AccessToken = accessToken.OrEmpty();

    public string AccessToken { get; }
}