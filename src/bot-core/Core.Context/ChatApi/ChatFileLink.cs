using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ChatFileLink
{
    public ChatFileLink(string fileId, string fileUniqueId, string filePath, string fileUrl)
    {
        FileId = fileId.OrEmpty();
        FileUniqueId = fileUniqueId.OrEmpty();
        FilePath = filePath.OrEmpty();
        FileUrl = fileUrl.OrEmpty();
    }

    public string FileId { get; }

    public string FileUniqueId { get; }

    public string FilePath { get; }

    public string FileUrl { get; }

    public long? FileSize { get; init; }

}