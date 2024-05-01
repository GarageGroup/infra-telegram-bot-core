using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotSticker : BotFileBase
{
    [JsonConstructor]
    public BotSticker(string fileId, string fileUniqueId, BotStickerType type, int width, int height, bool isAnimated, bool isVideo, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
    {
        Type = type;
        Width = width;
        Height = height;
        IsAnimated = isAnimated;
        IsVideo = isVideo;
    }

    public BotStickerType Type { get; }

    public int Width { get; }

    public int Height { get; }

    public bool IsAnimated { get; }

    public bool IsVideo { get; }

    public BotPhotoSize? Thumbnail { get; init; }

    public string? Emoji { get; init; }

    public string? SetName { get; init; }

    public BotMaskPosition? MaskPosition { get; init; }
}