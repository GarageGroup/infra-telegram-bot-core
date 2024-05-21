using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlow<T>
{
    public ChatFlow<T> Forward(Func<IChatFlowContext<T>, ChatFlowJump<T>> forward)
    {
        ArgumentNullException.ThrowIfNull(forward);

        flowSteps.Add(InnerInvokeAsync);
        return this;

        ValueTask<ChatFlowJump<T>> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
            =>
            new(forward.Invoke(context));
    }

    public ChatFlow<T> Forward(Func<IChatFlowContext<T>, CancellationToken, Task<ChatFlowJump<T>>> forwardAsync)
    {
        ArgumentNullException.ThrowIfNull(forwardAsync);

        flowSteps.Add(InnerInvokeAsync);
        return this;

        ValueTask<ChatFlowJump<T>> InnerInvokeAsync(IChatFlowContext<T> context, CancellationToken cancellationToken)
            =>
            new(forwardAsync.Invoke(context, cancellationToken));
    }

    public ChatFlow<T> ForwardValue(Func<IChatFlowContext<T>, CancellationToken, ValueTask<ChatFlowJump<T>>> forwardAsync)
    {
        ArgumentNullException.ThrowIfNull(forwardAsync);

        flowSteps.Add(forwardAsync);
        return this;
    }
}