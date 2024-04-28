using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotVoice : BotFileBase
{
    [JsonConstructor]
    public BotVoice(int duration, string fileId, string fileUniqueId, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
        =>
        Duration = duration;

    public int Duration { get; }

    public string? MimeType { get; init; }
}