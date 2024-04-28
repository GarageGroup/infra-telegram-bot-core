using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotAudio : BotFileBase
{
    [JsonConstructor]
    public BotAudio(int duration, string fileId, string fileUniqueId, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
        =>
        Duration = duration;

    public int Duration { get; }

    public string? Performer { get; init; }

    public string? Title { get; init; }

    public string? FileName { get; init; }

    public string? MimeType { get; init; }

    public BotPhotoSize? Thumb { get; init; }
}