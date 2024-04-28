using System;

namespace GarageGroup.Infra.Telegram.Bot;

public abstract record class BotKeyboardButtonBase
{
    private protected BotKeyboardButtonBase(string text)
        =>
        Text = text.OrEmpty();

    public string Text { get; }
}