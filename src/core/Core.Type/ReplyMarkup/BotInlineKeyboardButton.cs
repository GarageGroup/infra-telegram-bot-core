using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotInlineKeyboardButton : BotKeyboardButtonBase
{
    [JsonConstructor]
    public BotInlineKeyboardButton(string text) : base(text)
    {
    }

    public string? SwitchInlineQueryCurrentChat { get; init; }

    public string? SwitchInlineQuery { get; init; }

    public BotWebAppInfo? WebApp { get; init; }

    public string? CallbackData { get; init; }

    public BotLoginUrl? LoginUrl { get; init; }

    public string? Url { get; init; }

    public bool? Pay { get; init; }
}