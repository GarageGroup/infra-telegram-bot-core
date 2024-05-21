namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatCommandIn<TIn, TOut> : IChatCommandIn<TOut>
{
    static abstract TIn Init(string command);
}