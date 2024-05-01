using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotInlineKeyboardMarkup : BotReplyMarkupBase
{
    public required FlatArray<FlatArray<BotInlineKeyboardButton>> InlineKeyboard { get; init; }
}