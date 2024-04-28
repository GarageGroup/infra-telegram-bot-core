namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotForceReplyMarkup : BotReplyMarkupBase
{
    public BotForceReplyMarkup()
        =>
        ForceReply = true;

    public bool ForceReply { get; }

    public string? InputFieldPlaceholder { get; init; }
}