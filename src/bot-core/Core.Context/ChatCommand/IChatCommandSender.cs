using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatCommandSender
{
    ValueTask<ChatCommandResult<TOut>> SendAsync<TIn, TOut>(ChatCommandRequest<TIn, TOut> request, CancellationToken cancellationToken)
        where TIn : IChatCommandIn<TOut>;
}