using System;

namespace GarageGroup.Infra.Telegram.Bot;

internal readonly record struct UserState : IChatStateValue
{
    public static string Key { get; } = "user";

    internal const int TtlInMinutes = 5;

    public ChatUser? User { get; init; }

    public DateTimeOffset ExpirationTime { get; init; }
}