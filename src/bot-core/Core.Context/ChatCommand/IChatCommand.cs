using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatCommand<TIn, TOut>
    where TIn : IChatCommandIn<TOut>
{
    ValueTask<ChatCommandResult<TOut>> SendAsync(ChatCommandRequest<TIn, TOut> request, CancellationToken cancellationToken);
}