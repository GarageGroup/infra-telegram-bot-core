using System;

namespace GarageGroup.Infra.Telegram.Bot;

public abstract record class BotFileBase
{
    protected BotFileBase(string fileId, string fileUniqueId, long? fileSize)
    {
        FileId = fileId.OrEmpty();
        FileUniqueId = fileUniqueId.OrEmpty();
        FileSize = fileSize;
    }

    public string FileId { get; }

    public string FileUniqueId { get; }

    public long? FileSize { get; }
}