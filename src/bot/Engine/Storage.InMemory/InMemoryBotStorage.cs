using System.Collections.Concurrent;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed partial class InMemoryBotStorage : IBotStorage
{
    public static InMemoryBotStorage Instance { get; }
        =
        new();

    private InMemoryBotStorage()
    {
    }

    private readonly ConcurrentDictionary<long, ChatState> chatStates
        =
        [];
}