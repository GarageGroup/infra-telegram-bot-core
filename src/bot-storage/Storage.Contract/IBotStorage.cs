using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IBotStorage
{
    Task<ChatState> GetChatStateAsync(long chatId, CancellationToken cancellationToken);

    Task SaveChatStateAsync(long chatId, ChatState chatState, CancellationToken cancellationToken);
}