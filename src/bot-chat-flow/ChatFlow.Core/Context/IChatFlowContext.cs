using System;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatFlowContext<T> : IChatFlowContextBase
{
    T FlowState { get; }

    IChatFlowContext<TResult> MapFlowState<TResult>(Func<T, TResult> mapFlowState);
}