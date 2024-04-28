using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace GarageGroup.Infra.Telegram.Bot;

public record class BotInputTelegramFile : BotInputFileStream
{
    public BotInputTelegramFile(Stream content, string? fileName = null)
        : base(content, fileName)
    { }

    public BotInputTelegramFile(string fileId)
        : base(BotFileType.Id)
        =>
        FileId = fileId;

    protected BotInputTelegramFile(BotFileType fileType)
        : base(fileType)
    {
    }

    public string? FileId { get; private protected init; }

    [return: NotNullIfNotNull(nameof(stream))]
    public static BotInputTelegramFile? FromStream(Stream? stream)
        =>
        stream is null ? default : new(stream);

    [return: NotNullIfNotNull(nameof(fileId))]
    public static BotInputTelegramFile? FromFileId(string? fileId)
        =>
        fileId is null ? default : new(fileId);

    [return: NotNullIfNotNull(nameof(stream))]
    public static implicit operator BotInputTelegramFile?(Stream? stream)
        =>
        FromStream(stream);

    [return: NotNullIfNotNull(nameof(fileId))]
    public static implicit operator BotInputTelegramFile?(string? fileId)
        =>
        FromFileId(fileId);
}