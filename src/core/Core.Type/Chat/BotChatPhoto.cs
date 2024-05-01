using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotChatPhoto
{
    [JsonConstructor]
    public BotChatPhoto(string smallFileId, string smallFileUniqueId, string bigFileId, string bigFileUniqueId)
    {
        SmallFileId = smallFileId.OrEmpty();
        SmallFileUniqueId = smallFileUniqueId.OrEmpty();
        BigFileId = bigFileId.OrEmpty();
        BigFileUniqueId = bigFileUniqueId.OrEmpty();
    }

    public string SmallFileId { get; }

    public string SmallFileUniqueId { get; }

    public string BigFileId { get; }

    public string BigFileUniqueId { get; }
}