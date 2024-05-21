using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class StopCommandIn : IChatCommandIn<Unit>
{
    public static string Type { get; } = "Stop";
}