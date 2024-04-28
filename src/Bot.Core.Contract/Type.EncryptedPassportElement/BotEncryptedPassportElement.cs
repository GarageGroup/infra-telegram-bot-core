using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotEncryptedPassportElement
{
    [JsonConstructor]
    public BotEncryptedPassportElement(BotEncryptedPassportElementType type, string hash)
    {
        Type = type;
        Hash = hash.OrEmpty();
    }

    public BotEncryptedPassportElementType Type { get; }

    public string Hash { get; }

    public string? Data { get; init; }

    public string? PhoneNumber { get; init; }

    public string? Email { get; init; }

    public FlatArray<BotPassportFile> Files { get; init; }

    public BotPassportFile? FrontSide { get; init; }

    public BotPassportFile? ReverseSide { get; init; }

    public BotPassportFile? Selfie { get; init; }

    public FlatArray<BotPassportFile> Translation { get; init; }
}