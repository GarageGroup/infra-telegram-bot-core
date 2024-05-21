using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IUserUnauthorizeSupplier
{
    ValueTask<Result<Unit, Failure<Unit>>> UnauthorizeAsync(
        UserUnauthorizeIn input, CancellationToken cancellationToken);
}