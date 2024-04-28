namespace GarageGroup.Infra.Telegram.Bot;

public abstract record class BotReplyMarkupBase
{
    private protected BotReplyMarkupBase()
    {
    }

    public bool? Selective { get; init; }
}