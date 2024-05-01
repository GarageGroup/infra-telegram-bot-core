using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotMessageTextEditRequest
{
    public BotMessageTextEditRequest(long chatId, int messageId, [AllowNull] string text = BotMessageSendRequest.TelegramEmptyText)
    {
        ChatId = chatId;
        MessageId = messageId;
        InlineMessageId = null;
        Text = text.OrNullIfEmpty() ?? BotMessageSendRequest.TelegramEmptyText;
    }

    public BotMessageTextEditRequest(string inlineMessageId, [AllowNull] string text = BotMessageSendRequest.TelegramEmptyText)
    {
        ChatId = null;
        MessageId = null;
        InlineMessageId = inlineMessageId.OrEmpty();
        Text = text.OrNullIfEmpty() ?? BotMessageSendRequest.TelegramEmptyText;
    }

    public long? ChatId { get; }

    public int? MessageId { get; }

    public string? InlineMessageId { get; }

    public string Text { get; }

    public BotParseMode ParseMode { get; init; }

    public FlatArray<BotMessageEntity> Entities { get; init; }

    public BotInlineKeyboardMarkup? ReplyMarkup { get; init; }
}