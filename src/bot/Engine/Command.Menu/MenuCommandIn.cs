using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class MenuCommandIn : IChatCommandIn<Unit>
{
    public static string Type { get; } = "Menu";
}