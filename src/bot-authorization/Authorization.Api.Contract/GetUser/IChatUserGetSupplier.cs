using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatUserGetSupplier
{
    ValueTask<Result<ChatUserGetOut, Failure<Unit>>> GetChatUserAsync(
        ChatUserGetIn input, CancellationToken cancellationToken);
}