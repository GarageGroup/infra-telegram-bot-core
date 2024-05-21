using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotInfoCommandIn : IChatCommandIn<Unit>
{
    public static string Type { get; } = "BotInfo";
}