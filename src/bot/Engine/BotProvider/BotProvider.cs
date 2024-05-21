using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed class BotProvider
{
    private readonly BotOption option;

    private readonly ILoggerFactory loggerFactory;

    internal BotProvider(
        IBotApi botApi,
        IBotStorage botStorage,
        BotOption option,
        ILoggerFactory loggerFactory)
    {
        BotApi = botApi;
        BotStorage = botStorage;
        this.option = option;
        this.loggerFactory = loggerFactory;
    }

    public IBotApi BotApi { get; }

    public IBotStorage BotStorage { get; }

    internal BotContext GetBotContext(IChatCommandSender commandSender)
        =>
        new(
            botApi: BotApi,
            botStorage: BotStorage,
            option: option,
            loggerFactory: loggerFactory,
            commandSender: commandSender);
}