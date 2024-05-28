using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatCommandApi
{
    ValueTask<ChatCommandResult<TOut>> RunAsync<TIn, TOut>(TIn input, CancellationToken cancellationToken)
        where TIn : IChatCommandIn<TOut>;

    ValueTask<ChatCommandResult<Unit>> RunAsync<TIn>(TIn input, CancellationToken cancellationToken)
        where TIn : IChatCommandIn<Unit>
        =>
        RunAsync<TIn, Unit>(input, cancellationToken);
}