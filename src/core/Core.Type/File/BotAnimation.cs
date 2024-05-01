using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotAnimation : BotFileBase
{
    [JsonConstructor]
    public BotAnimation(string fileId, string fileUniqueId, int width, int height, int duration, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
    {
        Width = width;
        Height = height;
        Duration = duration;
    }

    public int Width { get; }

    public int Height { get; }

    public int Duration { get; }

    public BotPhotoSize? Thumbnail { get; init; }

    public string? FileName { get; init; }

    public string? MimeType { get; init; }
}