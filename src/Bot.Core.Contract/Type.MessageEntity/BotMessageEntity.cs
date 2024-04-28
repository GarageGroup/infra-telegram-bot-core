using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotMessageEntity
{
    [JsonConstructor]
    public BotMessageEntity(BotMessageEntityType type, int offset, int length)
    {
        Type = type;
        Offset = offset;
        Length = length;
    }

    public BotMessageEntityType Type { get; }

    public int Offset { get; }

    public int Length { get; }

    public string? Url { get; init; }

    public BotUser? User { get; init; }

    public string? Language { get; init; }
}