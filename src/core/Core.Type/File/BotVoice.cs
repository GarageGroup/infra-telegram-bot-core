using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotVoice : BotFileBase
{
    [JsonConstructor]
    public BotVoice(string fileId, string fileUniqueId, int duration, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
        =>
        Duration = duration;

    public int Duration { get; }

    public string? MimeType { get; init; }
}