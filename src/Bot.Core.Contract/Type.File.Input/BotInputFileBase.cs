namespace GarageGroup.Infra.Telegram.Bot;

public abstract record class BotInputFileBase
{
    private protected BotInputFileBase(BotFileType fileType)
        =>
        FileType = fileType;

    public BotFileType FileType { get; }
}