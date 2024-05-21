namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ChatMessageDeleteRequest
{
    public ChatMessageDeleteRequest(int messageId)
        =>
        MessageId = messageId;

    public int MessageId { get; }
}