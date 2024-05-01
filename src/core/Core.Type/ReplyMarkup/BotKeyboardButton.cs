using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotKeyboardButton : BotKeyboardButtonBase
{
    [JsonConstructor]
    public BotKeyboardButton(string text) : base(text)
    {
    }

    public bool? RequestContact { get; init; }

    public bool? RequestLocation { get; init; }

    public BotKeyboardButtonPollType? RequestPoll { get; init; }

    public BotWebAppInfo? WebApp { get; init; }
}