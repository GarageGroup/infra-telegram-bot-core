namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class TurnResult
{
    public TurnResult(IChatContext context, TurnState state)
    {
        Context = context;
        State = state;
    }

    public IChatContext Context { get; }

    public TurnState State { get; }
}