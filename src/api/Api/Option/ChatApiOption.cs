using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotApiOption
{
    public BotApiOption(string token)
        =>
        Token = token.OrEmpty();

    public string Token { get; }
}