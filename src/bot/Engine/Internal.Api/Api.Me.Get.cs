using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatApi
{
    public async Task<BotUser> GetMeAsync(CancellationToken cancellationToken)
    {
        var result = await botApi.GetMeAsync(default, cancellationToken).ConfigureAwait(false);
        return result.SuccessOrThrow(ToException);
    }
}