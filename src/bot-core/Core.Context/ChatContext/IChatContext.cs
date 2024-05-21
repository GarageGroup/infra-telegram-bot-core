﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatContext
{
    ChatUpdate Update { get; }

    IChatApi Api { get; }

    ChatState State { get; }

    ChatUser User { get; }

    ChatWebApp? WebApp { get; }

    IStringLocalizer GetLocalizer(string baseName);

    ILogger<T> GetLogger<T>();

    TurnResult CreateTurnResult(TurnState state)
        =>
        new(this, state);

    ChatCommandResult<T> CreateCancelledResult<T>()
        =>
        new(this, TurnState.Cancelled);

    ChatCommandResult<T> CreateWaitingResult<T>()
        =>
        new(this, TurnState.Waiting);

    ChatCommandResult<T> CreateCompleteResult<T>(T value)
        =>
        new(this, TurnState.Complete, value);

    ValueTask<ChatCommandResult<TOut>> SendAsync<TIn, TOut>(TIn input, CancellationToken cancellationToken)
        where TIn : IChatCommandIn<TOut>;
}