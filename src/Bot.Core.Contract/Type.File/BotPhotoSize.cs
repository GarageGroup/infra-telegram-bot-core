using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotPhotoSize : BotFileBase
{
    [JsonConstructor]
    public BotPhotoSize(int width, int height, string fileId, string fileUniqueId, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }

    public int Height { get; }
}