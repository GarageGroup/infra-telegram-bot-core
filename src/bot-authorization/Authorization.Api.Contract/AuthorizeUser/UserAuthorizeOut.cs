namespace GarageGroup.Infra.Telegram.Bot;

public readonly record struct UserAuthorizeOut
{
    public ChatUser? User { get; init; }

    public bool IsDisabled { get; init; }
}