using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class UserAuthorizeIn
{
    public UserAuthorizeIn(long botId, string botName, long chatId, string accessToken)
    {
        BotId = botId;
        BotName = botName.OrEmpty();
        ChatId = chatId;
        AccessToken = accessToken.OrEmpty();
    }

    public long BotId { get; }

    public string BotName { get; }

    public long ChatId { get; }

    public string AccessToken { get; }

    public string? LanguageCode { get; init; }
}