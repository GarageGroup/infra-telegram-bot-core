using System.IO;

namespace GarageGroup.Infra.Telegram.Bot;

public record class BotInputFileStream : BotInputFileBase
{
    public BotInputFileStream(Stream content, string? fileName = null)
        : base(BotFileType.Stream)
    {
        Content = content;
        FileName = fileName;
    }

    private protected BotInputFileStream(BotFileType fileType)
        : base(fileType)
    {
    }

    public Stream? Content { get; }

    public string? FileName { get; }
}