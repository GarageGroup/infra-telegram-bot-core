using System;

namespace GarageGroup.Infra.Telegram.Bot;

public readonly record struct SignOutCommandIn : IChatCommandIn<Unit>
{
    public static string Type { get; } = "SignOut";
}