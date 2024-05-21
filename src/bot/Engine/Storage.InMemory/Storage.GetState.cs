using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class InMemoryBotStorage
{
    public Task<ChatState> GetChatStateAsync(long chatId, CancellationToken cancellationToken)
        =>
        Task.FromResult(
            chatStates.TryGetValue(chatId, out var chatState) ? chatState : new());
}