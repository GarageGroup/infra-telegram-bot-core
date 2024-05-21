namespace GarageGroup.Infra.Telegram.Bot;

internal readonly record struct CallbackState : IChatStateValue
{
    public static string Key { get; } = "callback";

    public ChatUpdate? SourceUpdate { get; init; }

    public string? BotUrl { get; init; }

    public string? State { get; init; }
}