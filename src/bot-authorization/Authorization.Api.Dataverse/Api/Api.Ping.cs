using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class UserAuthorizationApi
{
    public ValueTask<Result<Unit, Failure<Unit>>> PingAsync(Unit input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
