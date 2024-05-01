using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotVideoNote : BotFileBase
{
    [JsonConstructor]
    public BotVideoNote(string fileId, string fileUniqueId, int length, int duration, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
    {
        Length = length;
        Duration = duration;
    }

    public int Length { get; }

    public int Duration { get; }

    public BotPhotoSize? Thumbnail { get; init; }
}