using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlow<T>
{
    public ChatFlow<T> Next(Func<IChatFlowContext<T>, T> next)
    {
        ArgumentNullException.ThrowIfNull(next);

        flowSteps.Add(InnerInvokeAsync);
        return this;

        ValueTask<ChatFlowJump<T>> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
            =>
            new(next.Invoke(context));
    }

    public ChatFlow<T> Next(Func<IChatFlowContext<T>, CancellationToken, Task<T>> nextAsync)
    {
        ArgumentNullException.ThrowIfNull(nextAsync);

        flowSteps.Add(InnerInvokeAsync);
        return this;

        async ValueTask<ChatFlowJump<T>> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
            =>
            await nextAsync.Invoke(context, cancellationToken).ConfigureAwait(false);
    }

    public ChatFlow<T> NextValue(Func<IChatFlowContext<T>, CancellationToken, ValueTask<T>> nextAsync)
    {
        ArgumentNullException.ThrowIfNull(nextAsync);

        flowSteps.Add(InnerInvokeAsync);
        return this;

        async ValueTask<ChatFlowJump<T>> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
            =>
            await nextAsync.Invoke(context, cancellationToken).ConfigureAwait(false);
    }
}