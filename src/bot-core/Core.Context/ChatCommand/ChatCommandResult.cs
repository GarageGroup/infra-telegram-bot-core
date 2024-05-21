using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ChatCommandResult<TOut>
{
    private readonly TOut value;

    internal ChatCommandResult(IChatContext context, TurnState state, TOut value)
    {
        Context = context;
        State = state;
        this.value = value;
    }

    internal ChatCommandResult(IChatContext context, TurnState state)
    {
        Context = context;
        State = state;
        value = default!;
    }

    public IChatContext Context { get; }

    public TurnState State { get; }

    public TOut CompleteValueOrThrow()
    {
        if (State is TurnState.Complete)
        {
            return value;
        }

        throw new InvalidOperationException($"The command value cannot be obtained because TurnState is {State}");
    }
}