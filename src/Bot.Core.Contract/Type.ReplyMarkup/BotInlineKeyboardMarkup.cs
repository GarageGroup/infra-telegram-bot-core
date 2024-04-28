using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotInlineKeyboardMarkup : BotReplyMarkupBase
{
    public static BotInlineKeyboardMarkup Empty()
        =>
        new(FlatArray.Empty<BotInlineKeyboardButton[]>());

    public BotInlineKeyboardMarkup(BotInlineKeyboardButton inlineKeyboardButton)
        =>
        InlineKeyboard = new[]
        {
            FlatArray.From(inlineKeyboardButton)
        };

    public BotInlineKeyboardMarkup(FlatArray<BotInlineKeyboardButton> inlineKeyboardRow)
        =>
        InlineKeyboard = new[]
        {
            inlineKeyboardRow
        };

    public BotInlineKeyboardMarkup(FlatArray<FlatArray<BotInlineKeyboardButton>> inlineKeyboard)
        =>
        InlineKeyboard = inlineKeyboard;

    [JsonConstructor]
    public BotInlineKeyboardMarkup([AllowNull] BotInlineKeyboardButton[][] inlineKeyboard)
        =>
        InlineKeyboard = inlineKeyboard?.Select(FlatArray.From).ToFlatArray() ?? [];

    public FlatArray<FlatArray<BotInlineKeyboardButton>> InlineKeyboard { get; }

    [return: NotNullIfNotNull(nameof(button))]
    public static BotInlineKeyboardMarkup? FromButton(BotInlineKeyboardButton? button)
        =>
        button is null ? null : new(button);

    [return: NotNullIfNotNull(nameof(buttonText))]
    public static BotInlineKeyboardMarkup? FromButtonText(string? buttonText)
        =>
        buttonText is null ? null : new(new BotInlineKeyboardButton(buttonText));

    [return: NotNullIfNotNull(nameof(inlineKeyboard))]
    public static BotInlineKeyboardMarkup? FromKeyboard(IEnumerable<BotInlineKeyboardButton>[]? inlineKeyboard)
        =>
        inlineKeyboard is null ? null : new(inlineKeyboard);

    [return: NotNullIfNotNull(nameof(inlineKeyboard))]
    public static BotInlineKeyboardMarkup? FromKeyboardRow(BotInlineKeyboardButton[]? inlineKeyboard)
        =>
        inlineKeyboard is null ? null : new(inlineKeyboard.ToFlatArray());

    [return: NotNullIfNotNull(nameof(button))]
    public static implicit operator BotInlineKeyboardMarkup?(BotInlineKeyboardButton? button)
        =>
        FromButton(button);

    [return: NotNullIfNotNull(nameof(buttonText))]
    public static implicit operator BotInlineKeyboardMarkup?(string? buttonText)
        =>
        FromButtonText(buttonText);

    [return: NotNullIfNotNull(nameof(inlineKeyboard))]
    public static implicit operator BotInlineKeyboardMarkup?(IEnumerable<BotInlineKeyboardButton>[]? inlineKeyboard)
        =>
        FromKeyboard(inlineKeyboard);

    [return: NotNullIfNotNull(nameof(inlineKeyboard))]
    public static implicit operator BotInlineKeyboardMarkup?(BotInlineKeyboardButton[]? inlineKeyboard)
        =>
        FromKeyboardRow(inlineKeyboard.ToFlatArray());
}