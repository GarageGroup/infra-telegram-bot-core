using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IBotState
{
    ValueTask<Result<T?, Unit>> GetValueOrAbsentAsync<T>(string key, CancellationToken cancellationToken);

    ValueTask<T?> GetValueOrThrowAsync<T>(string key, CancellationToken cancellationToken);

    ValueTask<Unit> SetValueAsync<T>(KeyValuePair<string, T> value, CancellationToken cancellationToken);

    ValueTask<Unit> DeleteValueAsync(string key, CancellationToken cancellationToken);

    ValueTask<Unit> ClearAsync(Unit key, CancellationToken cancellationToken);
}