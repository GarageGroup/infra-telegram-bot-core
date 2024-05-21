namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ChatCommandRequest<TIn, TOut>
    where TIn : IChatCommandIn<TOut>
{
    public ChatCommandRequest(IChatContext context, TIn value)
    {
        Context = context;
        Value = value;
    }

    public IChatContext Context { get; }

    public TIn Value { get; }
}