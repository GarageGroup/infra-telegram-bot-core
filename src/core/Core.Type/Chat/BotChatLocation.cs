using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotChatLocation
{
    [JsonConstructor]
    public BotChatLocation(BotLocation location, string address)
    {
        Location = location;
        Address = address.OrEmpty();
    }

    public BotLocation Location { get; }

    public string Address { get; }
}