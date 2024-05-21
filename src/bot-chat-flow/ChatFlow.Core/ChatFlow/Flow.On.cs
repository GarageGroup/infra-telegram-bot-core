using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlow<T>
{
    public ChatFlow<T> On(Func<IChatFlowContext<T>, Unit> on)
    {
        ArgumentNullException.ThrowIfNull(on);

        flowSteps.Add(InnerInvokeAsync);
        return this;

        ValueTask<ChatFlowJump<T>> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            _ = on.Invoke(context);
            return new(context.FlowState);
        }
    }

    public ChatFlow<T> On(Action<IChatFlowContext<T>> on)
    {
        ArgumentNullException.ThrowIfNull(on);

        flowSteps.Add(InnerInvokeAsync);
        return this;

        ValueTask<ChatFlowJump<T>> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            on.Invoke(context);
            return new(context.FlowState);
        }
    }

    public ChatFlow<T> On(Func<IChatFlowContext<T>, CancellationToken, Task<Unit>> onAsync)
    {
        ArgumentNullException.ThrowIfNull(onAsync);

        flowSteps.Add(InnerInvokeAsync);
        return this;

        async ValueTask<ChatFlowJump<T>> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            _ = await onAsync.Invoke(context, cancellationToken).ConfigureAwait(false);
            return new(context.FlowState);
        }
    }

    public ChatFlow<T> On(Func<IChatFlowContext<T>, CancellationToken, Task> onAsync)
    {
        ArgumentNullException.ThrowIfNull(onAsync);

        flowSteps.Add(InnerInvokeAsync);
        return this;

        async ValueTask<ChatFlowJump<T>> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            await onAsync.Invoke(context, cancellationToken).ConfigureAwait(false);
            return new(context.FlowState);
        }
    }

    public ChatFlow<T> OnValue(Func<IChatFlowContext<T>, CancellationToken, ValueTask<Unit>> onAsync)
    {
        ArgumentNullException.ThrowIfNull(onAsync);

        flowSteps.Add(InnerInvokeAsync);
        return this;

        async ValueTask<ChatFlowJump<T>> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            _ = await onAsync.Invoke(context, cancellationToken).ConfigureAwait(false);
            return new(context.FlowState);
        }
    }

    public ChatFlow<T> OnValue(Func<IChatFlowContext<T>, CancellationToken, ValueTask> onAsync)
    {
        ArgumentNullException.ThrowIfNull(onAsync);

        flowSteps.Add(InnerInvokeAsync);
        return this;

        async ValueTask<ChatFlowJump<T>> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
        {
            await onAsync.Invoke(context, cancellationToken).ConfigureAwait(false);
            return new(context.FlowState);
        }
    }
}