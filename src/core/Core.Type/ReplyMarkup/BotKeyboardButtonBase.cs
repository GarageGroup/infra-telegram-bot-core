using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

[JsonDerivedType(typeof(BotInlineKeyboardButton))]
[JsonDerivedType(typeof(BotKeyboardButton))]
public abstract record class BotKeyboardButtonBase
{
    private protected BotKeyboardButtonBase(string text)
        =>
        Text = text.OrEmpty();

    public string Text { get; }
}