using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotVenue
{
    [JsonConstructor]
    public BotVenue(BotLocation location, string title, string address)
    {
        Location = location;
        Title = title.OrEmpty();
        Address = address.OrEmpty();
    }

    public BotLocation Location { get; }

    public string Title { get; }

    public string Address { get; }

    public string? FoursquareId { get; init; }

    public string? FoursquareType { get; init; }

    public string? GooglePlaceId { get; init; }

    public string? GooglePlaceType { get; init; }
}