using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotMessageSendRequest
{
    internal const string TelegramEmptyText = "ㅤ";

    public BotMessageSendRequest(long chatId, [AllowNull] string text = TelegramEmptyText)
    {
        ChatId = chatId;
        Text = text.OrNullIfEmpty() ?? TelegramEmptyText;
    }

    public long ChatId { get; }

    public string Text { get; }

    public BotParseMode ParseMode { get; init; }

    public FlatArray<BotMessageEntity> Entities { get; init; }

    public bool? DisableWebPagePreview { get; init; }

    public bool? DisableNotification { get; init; }

    public bool? ProtectContent { get; init; }

    public int? ReplyToMessageId { get; init; }

    public bool? AllowSendingWithoutReply { get; init; }

    public BotReplyMarkupBase? ReplyMarkup { get; init; }
}