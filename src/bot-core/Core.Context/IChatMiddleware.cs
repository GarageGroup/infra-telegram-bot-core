using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatMiddleware
{
    ValueTask<TurnResult> InvokeAsync(IChatContext context, CancellationToken cancellationToken);
}