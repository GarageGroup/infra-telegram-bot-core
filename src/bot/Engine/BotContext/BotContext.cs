using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class BotContext
{
    private readonly IChatCommandSender commandSender;

    private readonly BotOption option;

    private readonly ILoggerFactory loggerFactory;

    internal BotContext(
        IBotApi botApi,
        IBotStorage botStorage,
        IChatCommandSender commandSender,
        BotOption option,
        ILoggerFactory loggerFactory)
    {
        BotApi = botApi;
        BotStorage = botStorage;
        this.option = option;
        this.loggerFactory = loggerFactory;
        this.commandSender = commandSender;
    }

    public IBotApi BotApi { get; }

    public IBotStorage BotStorage { get; }

    public ILogger<T> GetLogger<T>()
        =>
        loggerFactory.CreateLogger<T>();

    internal ChatContext InitChatContext(ChatUpdate update, [AllowNull] ChatState chatState)
        =>
        new(
            api: new ChatApi(BotApi, update.Chat.Id, option.FileUrlTemplate),
            update: update,
            state: chatState ?? new(),
            user: GetChatUser(update),
            webApp: option.WebAppBaseAddress is null ? null : new(option.WebAppBaseAddress),
            availableCultures: option.AvailableCultures,
            resourcesPath: option.ResourcesPath,
            loggerFactory: loggerFactory,
            commandSender: commandSender);

    private ChatUser GetChatUser(ChatUpdate update)
    {
        var userId = update.User.Id;

        if (option.AvailableCultures.Length is 1)
        {
            return new(userId)
            {
                Culture = option.AvailableCultures[0]
            };
        }

        if (string.IsNullOrWhiteSpace(update.User.LanguageCode))
        {
            return new(userId);
        }

        return new(userId)
        {
            Culture = new(update.User.LanguageCode)
        };
    }
}