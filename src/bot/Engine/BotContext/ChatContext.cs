using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class ChatContext : IChatContext, IAvailableCulturesProvider, IChatContextWithUserSupplier
{
    private static readonly string LocalizerLocation;

    static ChatContext()
        =>
        LocalizerLocation = (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).FullName.OrEmpty();

    private readonly BotStringLocalizerFactory localizerFactory;

    private readonly ILoggerFactory loggerFactory;

    private readonly IChatCommandSender commandSender;

    internal ChatContext(
        IChatApi api,
        ChatUpdate update,
        ChatState state,
        ChatUser user,
        ChatWebApp? webApp,
        FlatArray<CultureInfo> availableCultures,
        string resourcesPath,
        ILoggerFactory loggerFactory,
        IChatCommandSender commandSender)
    {
        Api = api;
        Update = update;
        State = state;
        User = user;
        WebApp = webApp;
        AvailableCultures = availableCultures;
        localizerFactory = new(user.Culture, resourcesPath, loggerFactory);
        this.loggerFactory = loggerFactory;
        this.commandSender = commandSender;
    }

    private ChatContext(
        IChatApi api,
        ChatUpdate update,
        ChatState state,
        ChatUser user,
        ChatWebApp? webApp,
        FlatArray<CultureInfo> availableCultures,
        BotStringLocalizerFactory localizerFactory,
        ILoggerFactory loggerFactory,
        IChatCommandSender commandSender)
    {
        Api = api;
        Update = update;
        State = state;
        User = user;
        WebApp = webApp;
        AvailableCultures = availableCultures;
        this.localizerFactory = localizerFactory;
        this.loggerFactory = loggerFactory;
        this.commandSender = commandSender;
    }

    public IChatApi Api { get; }

    public ChatUpdate Update { get; }

    public ChatState State { get; }

    public ChatUser User { get; }

    public ChatWebApp? WebApp { get; }

    public FlatArray<CultureInfo> AvailableCultures { get; }

    public IReadOnlyDictionary<string, string> AllCommandTypes
        =>
        commandSender switch
        {
            CommandMiddleware middleware => middleware.AllCommandTypes,
            _ => new Dictionary<string, string>(0)
        };

    public IStringLocalizer GetLocalizer(string baseName)
        =>
        localizerFactory.Create(baseName.OrEmpty(), LocalizerLocation);

    public ILogger<T> GetLogger<T>()
        =>
        loggerFactory.CreateLogger<T>();

    public ChatContext WithState(ChatState state)
        =>
        new(Api, Update, state, User, WebApp, AvailableCultures, localizerFactory, loggerFactory, commandSender);

    public IChatContext WithUser(ChatUser user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var factory = User.Culture == user.Culture ? localizerFactory : localizerFactory.WithCulture(user.Culture);
        return new ChatContext(Api, Update, State, user, WebApp, AvailableCultures, factory, loggerFactory, commandSender);
    }

    public ValueTask<ChatCommandResult<TOut>> SendAsync<TIn, TOut>(TIn input, CancellationToken cancellationToken)
        where TIn : IChatCommandIn<TOut>
    {
        var request = new ChatCommandRequest<TIn, TOut>(this, input);
        return commandSender.SendAsync(request, cancellationToken);
    }
}