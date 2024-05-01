using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotInvoice
{
    [JsonConstructor]
    public BotInvoice(string title, string description, string startParameter, string currency, int totalAmount)
    {
        Title = title.OrEmpty();
        Description = description.OrEmpty();
        StartParameter = startParameter.OrEmpty();
        Currency = currency.OrEmpty();
        TotalAmount = totalAmount;
    }

    public string Title { get; }

    public string Description { get; }

    public string StartParameter { get; }

    public string Currency { get; }

    public int TotalAmount { get; }
}