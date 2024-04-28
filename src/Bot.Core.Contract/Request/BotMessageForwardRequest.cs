namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotMessageForwardRequest
{
    public BotMessageForwardRequest(BotChatId destinationChatId, int messageId)
    {
        DestinationChatId = destinationChatId;
        MessageId = messageId;
    }

    public BotChatId DestinationChatId { get; }

    public int MessageId { get; }

    public bool? DisableNotification { get; init; }

    public bool? ProtectContent { get; init; }
}