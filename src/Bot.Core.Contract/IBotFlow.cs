using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IBotFlow
{
    ValueTask<Unit> NextAsync(Unit next, CancellationToken cancellationToken);

    ValueTask<Unit> NextAsync(BotUpdate nextUpdate, CancellationToken cancellationToken);

    ValueTask<Unit> StartAsync(BotUpdate update, CancellationToken cancellationToken);

    ValueTask<Unit> EndAsync(Unit end, CancellationToken cancellationToken);
}