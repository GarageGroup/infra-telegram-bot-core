using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotVideoNote : BotFileBase
{
    [JsonConstructor]
    public BotVideoNote(int length, int duration, string fileId, string fileUniqueId, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
    {
        Length = length;
        Duration = duration;
    }

    public int Length { get; }

    public int Duration { get; }

    public BotPhotoSize? Thumb { get; init; }
}