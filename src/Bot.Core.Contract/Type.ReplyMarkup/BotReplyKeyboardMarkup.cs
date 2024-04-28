using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotReplyKeyboardMarkup : BotReplyMarkupBase
{
    public BotReplyKeyboardMarkup(BotKeyboardButton button)
        =>
        Keyboard = new[]
        {
            FlatArray.From(button)
        };

    public BotReplyKeyboardMarkup(FlatArray<BotKeyboardButton> keyboardRow)
        =>
        Keyboard = new[]
        {
            keyboardRow
        };

    public BotReplyKeyboardMarkup(FlatArray<FlatArray<BotKeyboardButton>> keyboard)
        =>
        Keyboard = keyboard;

    [JsonConstructor]
    public BotReplyKeyboardMarkup(BotKeyboardButton[][] keyboard)
        =>
        Keyboard = keyboard?.Select(FlatArray.From).ToFlatArray() ?? [];

    public FlatArray<FlatArray<BotKeyboardButton>> Keyboard { get; init; }

    public bool? ResizeKeyboard { get; init; }

    public bool? OneTimeKeyboard { get; init; }

    public string? InputFieldPlaceholder { get; init; }

    [return: NotNullIfNotNull(nameof(text))]
    public static BotReplyKeyboardMarkup? FromText(string? text)
        =>
        text is null ? null : new(new BotKeyboardButton(text));

    [return: NotNullIfNotNull(nameof(texts))]
    public static BotReplyKeyboardMarkup? FromTexts(string[]? texts)
        =>
        texts is null ? null : new[] { texts };

    [return: NotNullIfNotNull(nameof(textsItems))]
    public static BotReplyKeyboardMarkup? FromTextsItems(string[][]? textsItems)
        =>
        textsItems is null ? null : new(textsItems.Select(FromArray).ToArray());

    [return: NotNullIfNotNull(nameof(text))]
    public static implicit operator BotReplyKeyboardMarkup?(string? text)
        =>
        FromText(text);

    [return: NotNullIfNotNull(nameof(texts))]
    public static implicit operator BotReplyKeyboardMarkup?(string[]? texts)
        =>
        FromTexts(texts);

    [return: NotNullIfNotNull(nameof(textsItems))]
    public static implicit operator BotReplyKeyboardMarkup?(string[][]? textsItems)
        =>
        FromTextsItems(textsItems);

    private static BotKeyboardButton[] FromArray(string[] texts)
        =>
        texts.Select(ButtonFromText).ToArray();

    private static BotKeyboardButton ButtonFromText(string text)
        =>
        new(text);
}