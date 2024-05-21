namespace GarageGroup.Infra.Telegram.Bot;

public readonly record struct ChatActionSendRequest
{
    public required BotChatAction Action { get; init; }
}