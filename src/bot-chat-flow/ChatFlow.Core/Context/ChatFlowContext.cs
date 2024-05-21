using System;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class ChatFlowContext<T> : IChatFlowContext<T>
{
    private readonly IChatContext chatContext;

    internal ChatFlowContext(
        string chatFlowId,
        IChatContext chatContext,
        T flowState,
        ChatFlowStepState stepState,
        ChatWebApp? webApp)
    {
        ChatFlowId = chatFlowId.OrEmpty();
        this.chatContext = chatContext;
        Localizer = chatContext.GetLocalizer(chatFlowId);
        Logger = chatContext.GetLogger<ChatFlowContext<T>>();
        FlowState = flowState;
        StepState = stepState;
        WebApp = webApp;
    }

    public string ChatFlowId { get; }

    public ChatUpdate Update
        =>
        chatContext.Update;

    public IChatApi Api
        =>
        chatContext.Api;

    public ChatUser User
        =>
        chatContext.User;

    public IStringLocalizer Localizer { get; }

    public ILogger Logger { get; }

    public T FlowState { get; }

    public ChatFlowStepState StepState { get; }

    public ChatWebApp? WebApp { get; }

    public IChatFlowContext<TResult> MapFlowState<TResult>(Func<T, TResult> mapFlowState)
    {
        ArgumentNullException.ThrowIfNull(mapFlowState);

        var nextFlowState = mapFlowState.Invoke(FlowState);
        return new ChatFlowContext<TResult>(ChatFlowId, chatContext, nextFlowState, StepState, WebApp);
    }
}