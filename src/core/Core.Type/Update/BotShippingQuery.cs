using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotShippingQuery
{
    [JsonConstructor]
    public BotShippingQuery(string id, BotUser from, string invoicePayload, BotShippingAddress shippingAddress)
    {
        Id = id.OrEmpty();
        From = from;
        InvoicePayload = invoicePayload.OrEmpty();
        ShippingAddress = shippingAddress;
    }

    public string Id { get; }

    public BotUser From { get; }

    public string InvoicePayload { get; }

    public BotShippingAddress ShippingAddress { get; }
}