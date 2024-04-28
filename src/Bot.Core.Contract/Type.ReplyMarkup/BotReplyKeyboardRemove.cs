namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotReplyKeyboardRemove : BotReplyMarkupBase
{
    public BotReplyKeyboardRemove()
        =>
        RemoveKeyboard = true;

    public bool RemoveKeyboard { get; }
}