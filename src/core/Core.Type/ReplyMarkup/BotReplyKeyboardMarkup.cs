using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotReplyKeyboardMarkup : BotReplyMarkupBase
{
    public required FlatArray<FlatArray<BotKeyboardButton>> Keyboard { get; init; }

    public bool? ResizeKeyboard { get; init; }

    public bool? OneTimeKeyboard { get; init; }

    public string? InputFieldPlaceholder { get; init; }
}