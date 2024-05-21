using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial record struct ChatFlowJump<T>
{
    public ChatFlowJump<TNext> Map<TNext>(
        Func<T, TNext> mapNextState,
        Func<T, TNext> mapRestartState)
        =>
        InnerMap(
            mapNextState ?? throw new ArgumentNullException(nameof(mapNextState)),
            mapRestartState ?? throw new ArgumentNullException(nameof(mapRestartState)));

    public Task<ChatFlowJump<TNext>> MapAsync<TNext>(
        Func<T, CancellationToken, Task<TNext>> mapNextAsync,
        Func<T, CancellationToken, Task<TNext>> mapRestartStateAsync,
        CancellationToken cancellationToken = default)
        =>
        InnerMapAsync(
            mapNextAsync ?? throw new ArgumentNullException(nameof(mapNextAsync)),
            mapRestartStateAsync ?? throw new ArgumentNullException(nameof(mapRestartStateAsync)),
            cancellationToken);

    public ValueTask<ChatFlowJump<TNext>> MapValueAsync<TNext>(
        Func<T, CancellationToken, ValueTask<TNext>> mapNextAsync,
        Func<T, CancellationToken, ValueTask<TNext>> mapRestartStateAsync,
        CancellationToken cancellationToken = default)
        =>
        InnerMapValueAsync(
            mapNextAsync ?? throw new ArgumentNullException(nameof(mapNextAsync)),
            mapRestartStateAsync ?? throw new ArgumentNullException(nameof(mapRestartStateAsync)),
            cancellationToken);

    private ChatFlowJump<TNext> InnerMap<TNext>(
        Func<T, TNext> mapNextState,
        Func<T, TNext> mapRestartState)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => new(mapNextState.Invoke(nextState), restart: false),
            ChatFlowJumpTag.Restart => new(mapRestartState.Invoke(nextState), restart: true),
            ChatFlowJumpTag.Break => new(breakState),
            _ => new(repeatState)
        };

    private async Task<ChatFlowJump<TNext>> InnerMapAsync<TNext>(
        Func<T, CancellationToken, Task<TNext>> mapNextAsync,
        Func<T, CancellationToken, Task<TNext>> mapRestartStateAsync,
        CancellationToken cancellationToken)
    {
        if (Tag is ChatFlowJumpTag.Next)
        {
            var state = await mapNextAsync.Invoke(nextState, cancellationToken).ConfigureAwait(false);
            return new(state, restart: false);
        }

        if (Tag is ChatFlowJumpTag.Restart)
        {
            var state = await mapRestartStateAsync.Invoke(nextState, cancellationToken).ConfigureAwait(false);
            return new(state, restart: true);
        }

        if (Tag is ChatFlowJumpTag.Break)
        {
            return new(breakState);
        }

        return new(repeatState);
    }

    private async ValueTask<ChatFlowJump<TNext>> InnerMapValueAsync<TNext>(
        Func<T, CancellationToken, ValueTask<TNext>> mapNextAsync,
        Func<T, CancellationToken, ValueTask<TNext>> mapRestartStateAsync,
        CancellationToken cancellationToken)
    {
        if (Tag is ChatFlowJumpTag.Next)
        {
            var state = await mapNextAsync.Invoke(nextState, cancellationToken).ConfigureAwait(false);
            return new(state, restart: false);
        }

        if (Tag is ChatFlowJumpTag.Restart)
        {
            var state = await mapRestartStateAsync.Invoke(nextState, cancellationToken).ConfigureAwait(false);
            return new(state, restart: true);
        }

        if (Tag is ChatFlowJumpTag.Break)
        {
            return new(breakState);
        }

        return new(repeatState);
    }
}