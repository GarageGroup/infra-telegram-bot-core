namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ChatMessageReplyMarkupEditRequest
{
    public ChatMessageReplyMarkupEditRequest(int messageId)
        =>
        MessageId = messageId;

    public int MessageId { get; }

    public BotInlineKeyboardMarkup? ReplyMarkup { get; init; }
}