using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotSuccessfulPayment
{
    [JsonConstructor]
    public BotSuccessfulPayment(
        string currency,
        int totalAmount,
        string invoicePayload,
        string telegramPaymentChargeId,
        string providerPaymentChargeId)
    {
        Currency = currency.OrEmpty();
        TotalAmount = totalAmount;
        InvoicePayload = invoicePayload.OrEmpty();
        TelegramPaymentChargeId = telegramPaymentChargeId.OrEmpty();
        ProviderPaymentChargeId = providerPaymentChargeId.OrEmpty();
    }

    public string Currency { get; }

    public int TotalAmount { get; }

    public string InvoicePayload { get; }

    public string TelegramPaymentChargeId { get; }

    public string ProviderPaymentChargeId { get; }

    public BotOrderInfo? OrderInfo { get; init; }
}