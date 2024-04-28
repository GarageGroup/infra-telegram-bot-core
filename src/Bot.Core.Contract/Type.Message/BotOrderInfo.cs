namespace GarageGroup.Infra.Telegram.Bot;

public readonly record struct BotOrderInfo
{
    public string? Name { get; init; }

    public string? PhoneNumber { get; init; }

    public string? Email { get; init; }

    public BotShippingAddress? ShippingAddress { get; init; }
}