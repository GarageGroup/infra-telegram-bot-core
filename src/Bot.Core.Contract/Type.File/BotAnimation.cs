using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotAnimation : BotFileBase
{
    [JsonConstructor]
    public BotAnimation(int width, int height, string fileId, string fileUniqueId, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }

    public int Height { get; }

    public int Duration { get; }

    public BotPhotoSize? Thumb { get; init; }

    public string? FileName { get; init; }

    public string? MimeType { get; init; }
}