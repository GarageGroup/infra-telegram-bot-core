using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class InMemoryBotStorage
{
    public Task SaveChatStateAsync(long chatId, ChatState chatState, CancellationToken cancellationToken)
    {
        chatStates[chatId] = chatState ?? new();
        return Task.CompletedTask;
    }
}