namespace GarageGroup.Infra.Telegram.Bot;

public readonly record struct ChatUserGetOut
{
    public ChatUser? User { get; init; }

    public bool IsDisabled { get; init; }

    public bool IsSignedOut { get; init; }
}