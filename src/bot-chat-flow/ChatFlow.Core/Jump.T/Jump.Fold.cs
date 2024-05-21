using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial record struct ChatFlowJump<T>
{
    public TResult Fold<TResult>(
        Func<T, TResult> mapNextState,
        Func<T, TResult> mapRestartState,
        Func<ChatBreakState, TResult> mapBreakState,
        Func<ChatRepeatState, TResult> repeatStateFactory)
        =>
        InnerFold(
            mapNextState ?? throw new ArgumentNullException(nameof(mapNextState)),
            mapRestartState ?? throw new ArgumentNullException(nameof(mapRestartState)),
            mapBreakState ?? throw new ArgumentNullException(nameof(mapBreakState)),
            repeatStateFactory ?? throw new ArgumentNullException(nameof(repeatStateFactory)));

    public Task<TResult> FoldAsync<TResult>(
        Func<T, CancellationToken, Task<TResult>> mapNextStateAsync,
        Func<T, CancellationToken, Task<TResult>> mapRestartStateAsync,
        Func<ChatBreakState, CancellationToken, Task<TResult>> mapBeakStateAsync,
        Func<ChatRepeatState, CancellationToken, Task<TResult>> repeatStateFactoryAsync,
        CancellationToken cancellationToken)
        =>
        InnerFoldAsync(
            mapNextStateAsync ?? throw new ArgumentNullException(nameof(mapNextStateAsync)),
            mapRestartStateAsync ?? throw new ArgumentNullException(nameof(mapRestartStateAsync)),
            mapBeakStateAsync ?? throw new ArgumentNullException(nameof(mapBeakStateAsync)),
            repeatStateFactoryAsync ?? throw new ArgumentNullException(nameof(repeatStateFactoryAsync)),
            cancellationToken);

    public ValueTask<TResult> FoldValueAsync<TResult>(
        Func<T, CancellationToken, ValueTask<TResult>> mapNextStateAsync,
        Func<T, CancellationToken, ValueTask<TResult>> mapRestartStateAsync,
        Func<ChatBreakState, CancellationToken, ValueTask<TResult>> mapBeakStateAsync,
        Func<ChatRepeatState, CancellationToken, ValueTask<TResult>> repeatStateFactoryAsync,
        CancellationToken cancellationToken)
        =>
        InnerFoldValueAsync(
            mapNextStateAsync ?? throw new ArgumentNullException(nameof(mapNextStateAsync)),
            mapRestartStateAsync ?? throw new ArgumentNullException(nameof(mapRestartStateAsync)),
            mapBeakStateAsync ?? throw new ArgumentNullException(nameof(mapBeakStateAsync)),
            repeatStateFactoryAsync ?? throw new ArgumentNullException(nameof(repeatStateFactoryAsync)),
            cancellationToken);

    private TResult InnerFold<TResult>(
        Func<T, TResult> mapNextState,
        Func<T, TResult> mapRestartState,
        Func<ChatBreakState, TResult> mapBreakState,
        Func<ChatRepeatState, TResult> repeatStateFactory)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => mapNextState.Invoke(nextState),
            ChatFlowJumpTag.Restart => mapRestartState.Invoke(nextState),
            ChatFlowJumpTag.Break => mapBreakState.Invoke(breakState),
            _ => repeatStateFactory.Invoke(repeatState)
        };

    private Task<TResult> InnerFoldAsync<TResult>(
        Func<T, CancellationToken, Task<TResult>> mapNextStateAsync,
        Func<T, CancellationToken, Task<TResult>> mapRestartStateAsync,
        Func<ChatBreakState, CancellationToken, Task<TResult>> mapBeakStateAsync,
        Func<ChatRepeatState, CancellationToken, Task<TResult>> repeatStateFactoryAsync,
        CancellationToken cancellationToken)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => mapNextStateAsync.Invoke(nextState, cancellationToken),
            ChatFlowJumpTag.Restart => mapRestartStateAsync.Invoke(nextState, cancellationToken),
            ChatFlowJumpTag.Break => mapBeakStateAsync.Invoke(breakState, cancellationToken),
            _ => repeatStateFactoryAsync.Invoke(repeatState, cancellationToken)
        };

    private ValueTask<TResult> InnerFoldValueAsync<TResult>(
        Func<T, CancellationToken, ValueTask<TResult>> mapNextStateAsync,
        Func<T, CancellationToken, ValueTask<TResult>> mapRestartStateAsync,
        Func<ChatBreakState, CancellationToken, ValueTask<TResult>> mapBeakStateAsync,
        Func<ChatRepeatState, CancellationToken, ValueTask<TResult>> repeatStateFactoryAsync,
        CancellationToken cancellationToken)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => mapNextStateAsync.Invoke(nextState, cancellationToken),
            ChatFlowJumpTag.Restart => mapRestartStateAsync.Invoke(nextState, cancellationToken),
            ChatFlowJumpTag.Break => mapBeakStateAsync.Invoke(breakState, cancellationToken),
            _ => repeatStateFactoryAsync.Invoke(repeatState, cancellationToken)
        };
}