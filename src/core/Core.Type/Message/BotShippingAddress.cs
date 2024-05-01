using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotShippingAddress
{
    [JsonConstructor]
    public BotShippingAddress(string countryCode, string city, string streetLine1, string streetLine2, string postCode)
    {
        CountryCode = countryCode.OrEmpty();
        City = city.OrEmpty();
        StreetLine1 = streetLine1.OrEmpty();
        StreetLine2 = streetLine2.OrEmpty();
        PostCode = postCode.OrEmpty();
    }

    public string CountryCode { get; }

    public string City { get; }

    public string StreetLine1 { get; }

    public string StreetLine2 { get; }

    public string PostCode { get; }

    public string? State { get; init; }
}