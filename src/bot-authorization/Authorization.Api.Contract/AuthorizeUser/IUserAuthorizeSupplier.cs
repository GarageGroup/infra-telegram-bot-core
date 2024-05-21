using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IUserAuthorizeSupplier
{
    ValueTask<Result<UserAuthorizeOut, Failure<Unit>>> AuthorizeAsync(
        UserAuthorizeIn input, CancellationToken cancellationToken);
}