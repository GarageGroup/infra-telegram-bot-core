using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    private static readonly ReadOnlyDictionary<string, string> EmptyCommandTypes;

    static ChatContext()
    {
        LocalizerLocation = (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).FullName.OrEmpty();
        EmptyCommandTypes = new(new Dictionary<string, string>(0));
    }

    private readonly BotStringLocalizerFactory localizerFactory;

    private readonly ILoggerFactory loggerFactory;

    internal ChatContext(
        IChatApi api,
        ChatUpdate update,
        ChatState state,
        ChatUser user,
        ChatWebApp? webApp,
        FlatArray<CultureInfo> availableCultures,
        IChatCommandSender commandSender,
        string resourcesPath,
        ILoggerFactory loggerFactory)
    {
        Api = api;
        Update = update;
        State = state;
        User = user;
        WebApp = webApp;
        AvailableCultures = availableCultures;
        Command = new ChatCommandApi(this, commandSender);
        AllCommandTypes = commandSender switch
        {
            CommandMiddleware middleware => middleware.AllCommandTypes,
            _ => EmptyCommandTypes
        };
        localizerFactory = new(user.Culture, resourcesPath, loggerFactory);
        this.loggerFactory = loggerFactory;
    }

    private ChatContext(
        IChatApi api,
        ChatUpdate update,
        ChatState state,
        ChatUser user,
        ChatWebApp? webApp,
        FlatArray<CultureInfo> availableCultures,
        IChatCommandApi command,
        IReadOnlyDictionary<string, string> allCommandTypes,
        BotStringLocalizerFactory localizerFactory,
        ILoggerFactory loggerFactory)
    {
        Api = api;
        Update = update;
        State = state;
        User = user;
        WebApp = webApp;
        AvailableCultures = availableCultures;
        Command = command;
        AllCommandTypes = allCommandTypes;
        this.localizerFactory = localizerFactory;
        this.loggerFactory = loggerFactory;
    }

    public IChatApi Api { get; }

    public ChatUpdate Update { get; }

    public ChatState State { get; }

    public ChatUser User { get; }

    public ChatWebApp? WebApp { get; }

    public FlatArray<CultureInfo> AvailableCultures { get; }

    public IReadOnlyDictionary<string, string> AllCommandTypes { get; }

    public IChatCommandApi Command { get; }

    public IStringLocalizer GetLocalizer(string baseName)
        =>
        localizerFactory.Create(baseName.OrEmpty(), LocalizerLocation);

    public ILogger<T> GetLogger<T>()
        =>
        loggerFactory.CreateLogger<T>();

    public ChatContext WithState(ChatState state)
        =>
        new(Api, Update, state, User, WebApp, AvailableCultures, Command, AllCommandTypes, localizerFactory, loggerFactory);

    public IChatContext WithUser(ChatUser user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var factory = User.Culture == user.Culture ? localizerFactory : localizerFactory.WithCulture(user.Culture);
        return new ChatContext(Api, Update, State, user, WebApp, AvailableCultures, Command, AllCommandTypes, factory, loggerFactory);
    }

    private sealed class ChatCommandApi(IChatContext context, IChatCommandSender commandSender) : IChatCommandApi
    {
        public ValueTask<ChatCommandResult<TOut>> RunAsync<TIn, TOut>(
            TIn input, CancellationToken cancellationToken) where TIn : IChatCommandIn<TOut>
        {
            var request = new ChatCommandRequest<TIn, TOut>(context, input);
            return commandSender.SendAsync(request, cancellationToken);
        }
    }
}