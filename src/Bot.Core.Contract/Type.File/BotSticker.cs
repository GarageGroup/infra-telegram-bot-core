using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotSticker : BotFileBase
{
    [JsonConstructor]
    public BotSticker(int width, int height, bool isAnimated, bool isVideo, string fileId, string fileUniqueId, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
    {
        Width = width;
        Height = height;
        IsAnimated = isAnimated;
        IsVideo = isVideo;
    }

    public int Width { get; }

    public int Height { get; }

    public bool IsAnimated { get; }

    public bool IsVideo { get; }

    public BotPhotoSize? Thumb { get; init; }

    public string? Emoji { get; init; }

    public string? SetName { get; init; }

    public BotMaskPosition? MaskPosition { get; init; }
}