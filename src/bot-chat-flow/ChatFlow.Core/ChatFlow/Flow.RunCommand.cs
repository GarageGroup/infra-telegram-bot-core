using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlow<T>
{
    public ChatFlow<T> RunCommandOrSkip<TIn, TOut>(
        Func<IChatFlowContext<T>, Optional<TIn>> inputFactory,
        Func<IChatFlowContext<T>, TOut, T> mapFlowState)
        where TIn : IChatCommandIn<TOut>
    {
        ArgumentNullException.ThrowIfNull(inputFactory);
        ArgumentNullException.ThrowIfNull(mapFlowState);

        return InnerRunCommandOrSkip(inputFactory, mapFlowState);
    }

    public ChatFlow<T> RunCommandOrSkip<TIn>(
        Func<IChatFlowContext<T>, Optional<TIn>> inputFactory)
        where TIn : IChatCommandIn<Unit>
    {
        ArgumentNullException.ThrowIfNull(inputFactory);

        return InnerRunCommandOrSkip<TIn, Unit>(inputFactory, GetFlowState);
    }

    public ChatFlow<T> RunCommand<TIn, TOut>(
        Func<IChatFlowContext<T>, TIn> inputFactory,
        Func<IChatFlowContext<T>, TOut, T> mapFlowState)
        where TIn : IChatCommandIn<TOut>
    {
        ArgumentNullException.ThrowIfNull(inputFactory);
        ArgumentNullException.ThrowIfNull(mapFlowState);

        return InnerRunCommand(inputFactory, mapFlowState);
    }

    public ChatFlow<T> RunCommand<TIn>(
        Func<IChatFlowContext<T>, TIn> inputFactory)
        where TIn : IChatCommandIn<Unit>
    {
        ArgumentNullException.ThrowIfNull(inputFactory);

        return InnerRunCommand<TIn, Unit>(inputFactory, GetFlowState);
    }

    private ChatFlow<T> InnerRunCommandOrSkip<TIn, TOut>(
        Func<IChatFlowContext<T>, Optional<TIn>> inputFactory,
        Func<IChatFlowContext<T>, TOut, T> mapFlowState)
        where TIn : IChatCommandIn<TOut>
    {
        flowSteps.Add(InnerInvokeAsync);
        return this;

        async ValueTask<ChatFlowJump<T>> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(context);

            var @in = inputFactory.Invoke(context);
            if (@in.IsAbsent)
            {
                return context.FlowState;
            }

            var result = await chatContext.SendAsync<TIn, TOut>(@in.OrThrow(), cancellationToken).ConfigureAwait(false);
            if (chatContext is IChatContextWithUserSupplier withSupplier)
            {
                chatContext = withSupplier.WithUser(result.Context.User);
            }

            return result.State switch
            {
                TurnState.Complete => mapFlowState.Invoke(context, result.CompleteValueOrThrow()),
                TurnState.Cancelled => default(ChatBreakState),
                _ => default
            };
        }
    }

    private ChatFlow<T> InnerRunCommand<TIn, TOut>(
        Func<IChatFlowContext<T>, TIn> inputFactory,
        Func<IChatFlowContext<T>, TOut, T> mapFlowState)
        where TIn : IChatCommandIn<TOut>
    {
        flowSteps.Add(InnerInvokeAsync);
        return this;

        async ValueTask<ChatFlowJump<T>> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(context);

            var @in = inputFactory.Invoke(context);
            var result = await chatContext.SendAsync<TIn, TOut>(@in, cancellationToken).ConfigureAwait(false);

            return result.State switch
            {
                TurnState.Complete => mapFlowState.Invoke(context, result.CompleteValueOrThrow()),
                TurnState.Cancelled => default(ChatBreakState),
                _ => default
            };
        }
    }

    private static T GetFlowState(IChatFlowContext<T> context, Unit _)
        =>
        context.FlowState;
}