using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotDice
{
    [JsonConstructor]
    public BotDice(string emoji, int value)
    {
        Emoji = emoji.OrEmpty();
        Value = value;
    }

    public string Emoji { get; }

    public int Value { get; }
}