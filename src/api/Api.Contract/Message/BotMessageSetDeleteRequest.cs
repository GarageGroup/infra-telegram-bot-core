using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotMessageSetDeleteRequest
{
    public BotMessageSetDeleteRequest(long chatId, FlatArray<int> messageIds)
    {
        ChatId = chatId;
        MessageIds = messageIds;
    }

    public long ChatId { get; init; }

    public FlatArray<int> MessageIds { get; init; }
}