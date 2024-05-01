using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotDocument : BotFileBase
{
    [JsonConstructor]
    public BotDocument(string fileId, string fileUniqueId, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
    {
    }

    public BotPhotoSize? Thumbnail { get; init; }

    public string? FileName { get; init; }

    public string? MimeType { get; init; }
}