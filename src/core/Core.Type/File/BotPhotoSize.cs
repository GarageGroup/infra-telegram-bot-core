using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotPhotoSize : BotFileBase
{
    [JsonConstructor]
    public BotPhotoSize(string fileId, string fileUniqueId, int width, int height, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }

    public int Height { get; }
}