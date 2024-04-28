using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotPreCheckoutQuery
{
    [JsonConstructor]
    public BotPreCheckoutQuery(string id, BotUser from, string currency, int totalAmount, string invoicePayload)
    {
        Id = id.OrEmpty();
        From = from;
        Currency = currency.OrEmpty();
        TotalAmount = totalAmount;
        InvoicePayload = invoicePayload.OrEmpty();
    }

    public string Id { get; }

    public BotUser From { get; }

    public string Currency { get; }

    public int TotalAmount { get; }

    public string InvoicePayload { get; }

    public string? ShippingOptionId { get; init; }

    public BotOrderInfo? OrderInfo { get; init; }
}