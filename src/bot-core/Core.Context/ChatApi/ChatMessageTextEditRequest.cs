using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ChatMessageTextEditRequest
{
    public ChatMessageTextEditRequest(int messageId, [AllowNull] string text)
    {
        MessageId = messageId;
        Text = text.OrEmpty();
    }

    public int MessageId { get; }

    public string Text { get; }

    public BotParseMode ParseMode { get; init; }

    public FlatArray<BotMessageEntity> Entities { get; init; }

    public BotInlineKeyboardMarkup? ReplyMarkup { get; init; }
}