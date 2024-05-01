using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

[JsonDerivedType(typeof(BotForceReplyMarkup))]
[JsonDerivedType(typeof(BotInlineKeyboardMarkup))]
[JsonDerivedType(typeof(BotReplyKeyboardMarkup))]
[JsonDerivedType(typeof(BotReplyKeyboardRemove))]
public abstract record class BotReplyMarkupBase
{
    private protected BotReplyMarkupBase()
    {
    }

    public bool? Selective { get; init; }
}