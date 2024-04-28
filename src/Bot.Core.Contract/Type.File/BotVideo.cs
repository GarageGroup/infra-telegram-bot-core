using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotVideo : BotFileBase
{
    [JsonConstructor]
    public BotVideo(int width, int height, int duration, string fileId, string fileUniqueId, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
    {
        Width = width;
        Height = height;
        Duration = duration;
    }

    public int Width { get; }

    public int Height { get; }

    public int Duration { get; }

    public BotPhotoSize? Thumb { get; init; }

    public string? FileName { get; init; }

    public string? MimeType { get; init; }
}