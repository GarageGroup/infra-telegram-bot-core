using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotPassportFile : BotFileBase
{
    [JsonConstructor]
    public BotPassportFile(DateTime fileDate, string fileId, string fileUniqueId, long? fileSize)
        : base(fileId: fileId, fileUniqueId: fileUniqueId, fileSize: fileSize)
        =>
        FileDate = fileDate;

    public DateTime FileDate { get; }
}