using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial record struct ChatFlowJump<T>
{
    public ChatFlowJump<TNext> Forward<TNext>(
        Func<T, ChatFlowJump<TNext>> next,
        Func<T, ChatFlowJump<TNext>> restart)
        =>
        InnerForward(
            next ?? throw new ArgumentNullException(nameof(next)),
            restart ?? throw new ArgumentNullException(nameof(restart)));

    public Task<ChatFlowJump<TNext>> ForwardAsync<TNext>(
        Func<T, CancellationToken, Task<ChatFlowJump<TNext>>> nextAsync,
        Func<T, CancellationToken, Task<ChatFlowJump<TNext>>> restartAsync,
        CancellationToken cancellationToken)
        =>
        InnerForwardAsync(
            nextAsync ?? throw new ArgumentNullException(nameof(nextAsync)),
            restartAsync ?? throw new ArgumentNullException(nameof(restartAsync)),
            cancellationToken);

    public ValueTask<ChatFlowJump<TNext>> ForwardValueAsync<TNext>(
        Func<T, CancellationToken, ValueTask<ChatFlowJump<TNext>>> nextAsync,
        Func<T, CancellationToken, ValueTask<ChatFlowJump<TNext>>> restartAsync,
        CancellationToken cancellationToken)
        =>
        InnerForwardValueAsync(
            nextAsync ?? throw new ArgumentNullException(nameof(nextAsync)),
            restartAsync ?? throw new ArgumentNullException(nameof(restartAsync)),
            cancellationToken);

    private ChatFlowJump<TNext> InnerForward<TNext>(
        Func<T, ChatFlowJump<TNext>> next,
        Func<T, ChatFlowJump<TNext>> restart)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => next.Invoke(nextState),
            ChatFlowJumpTag.Restart => restart.Invoke(nextState),
            ChatFlowJumpTag.Break => new(breakState),
            _ => default
        };

    private Task<ChatFlowJump<TNext>> InnerForwardAsync<TNext>(
        Func<T, CancellationToken, Task<ChatFlowJump<TNext>>> nextAsync,
        Func<T, CancellationToken, Task<ChatFlowJump<TNext>>> restartAsync,
        CancellationToken cancellationToken)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => nextAsync.Invoke(nextState, cancellationToken),
            ChatFlowJumpTag.Restart => restartAsync.Invoke(nextState, cancellationToken),
            ChatFlowJumpTag.Break => Task.FromResult<ChatFlowJump<TNext>>(new(breakState)),
            _ => Task.FromResult<ChatFlowJump<TNext>>(default)
        };

    private ValueTask<ChatFlowJump<TNext>> InnerForwardValueAsync<TNext>(
        Func<T, CancellationToken, ValueTask<ChatFlowJump<TNext>>> nextAsync,
        Func<T, CancellationToken, ValueTask<ChatFlowJump<TNext>>> restartAsync,
        CancellationToken cancellationToken)
        =>
        Tag switch
        {
            ChatFlowJumpTag.Next => nextAsync.Invoke(nextState, cancellationToken),
            ChatFlowJumpTag.Restart => restartAsync.Invoke(nextState, cancellationToken),
            ChatFlowJumpTag.Break => ValueTask.FromResult<ChatFlowJump<TNext>>(new(breakState)),
            _ => default
        };
}