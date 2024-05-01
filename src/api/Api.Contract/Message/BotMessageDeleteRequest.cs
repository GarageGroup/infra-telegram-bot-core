namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotMessageDeleteRequest
{
    public BotMessageDeleteRequest(long chatId, int messageId)
    {
        ChatId = chatId;
        MessageId = messageId;
    }

    public long ChatId { get; init; }

    public int MessageId { get; init; }
}