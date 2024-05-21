namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatCommandIn<TOut>
{
    static abstract string Type { get; }
}