namespace GarageGroup.Infra.Telegram.Bot;

public readonly record struct ChatUserGetIn
{
    public ChatUserGetIn(long botId, long chatId)
    {
        BotId = botId;
        ChatId = chatId;
    }

    public long BotId { get; }

    public long ChatId { get; }
}