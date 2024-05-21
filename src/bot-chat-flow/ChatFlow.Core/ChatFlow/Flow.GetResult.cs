using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatFlow<T>
{
    public async ValueTask<ChatCommandResult<TOut>> GetResultAsync<TOut>(
        Func<T, TOut> mapFlowState, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(mapFlowState);

        var jump = await InnerGetFlowJumpAsync(cancellationToken).ConfigureAwait(false);

        return jump.Map(mapFlowState, mapFlowState).Fold(
            mapNextState: chatContext.CreateCompleteResult,
            mapRestartState: CreateCancelledResult,
            mapBreakState: CreateCancelledResult,
            repeatStateFactory: CreateWaitingResult);

        ChatCommandResult<TOut> CreateCancelledResult<TValue>(TValue _)
            =>
            chatContext.CreateCancelledResult<TOut>();

        ChatCommandResult<TOut> CreateWaitingResult(ChatRepeatState _)
            =>
            chatContext.CreateWaitingResult<TOut>();
    }

    private async ValueTask<ChatFlowJump<T>> InnerGetFlowJumpAsync(CancellationToken cancellationToken)
    {
        var flowState = chatContext.State.GetValue<FlowStateJson>() ?? new(
            position: 0,
            flowState: initialFactory.Invoke(),
            stepState: default);

        var currentPosition = flowState.Position;
        var currentJump = new ChatFlowJump<T>(flowState.FlowState!);

        var restartCount = 0;
        while (currentPosition < flowSteps.Count)
        {
            var flowContext = new ChatFlowContext<T>(
                chatFlowId: chatFlowId,
                chatContext: chatContext,
                flowState: flowState.FlowState!,
                stepState: flowState.StepState,
                webApp: chatContext.WebApp);

            currentJump = await flowSteps[currentPosition].Invoke(flowContext, cancellationToken).ConfigureAwait(false);
            if (currentJump.Tag is ChatFlowJumpTag.Break)
            {
                await OnBreakAsync(flowContext, currentJump.BreakStateOrThrow(), cancellationToken).ConfigureAwait(false);
                break;
            }

            if (currentJump.Tag is ChatFlowJumpTag.Next)
            {
                currentPosition++;

                flowState = new(
                    position: currentPosition,
                    flowState: currentJump.NextStateOrThrow(),
                    stepState: default);
            }
            else if (currentJump.Tag is ChatFlowJumpTag.Restart)
            {
                currentPosition = 0;

                flowState = new(
                    position: currentPosition,
                    flowState: currentJump.RestartStateOrThrow(),
                    stepState: default);

                if (restartCount >= MaxRestartCount)
                {
                    throw new InvalidOperationException($"Maximum number of restarts exceeded: {MaxRestartCount}");
                }

                restartCount++;
            }
            else if (currentJump.Tag is ChatFlowJumpTag.Repeat)
            {
                chatContext.State.SetValue(flowState);
                await OnRepeatAsync(flowContext, currentJump.RepeatStateOrThrow(), cancellationToken).ConfigureAwait(false);

                return currentJump;
            }
        }

        chatContext.State.RemoveValue<FlowStateJson>();
        return currentJump;
    }

    private Task OnBreakAsync(ChatFlowContext<T> context, ChatBreakState breakState, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(breakState.LogMessage) is false || breakState.SourceException is not null)
        {
            context.Logger.LogError(breakState.SourceException, "CkatBreak: {logMessage}", breakState.LogMessage);
        }

        var text = HttpUtility.HtmlEncode(breakState.UserMessage);
        if (string.IsNullOrWhiteSpace(text))
        {
            return Task.CompletedTask;
        }

        return context.Api.SendHtmlModeTextAndRemoveReplyKeyboardAsync(text, cancellationToken);
    }

    private Task OnRepeatAsync(ChatFlowContext<T> context, ChatRepeatState repeatState, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(repeatState.LogMessage) is false || repeatState.SourceException is not null)
        {
            context.Logger.LogWarning(repeatState.SourceException, "ChatRepeat: {logMessage}", repeatState.LogMessage);
        }

        var text = HttpUtility.HtmlEncode(repeatState.UserMessage);
        if (string.IsNullOrWhiteSpace(text))
        {
            return Task.CompletedTask;
        }

        return context.Api.SendHtmlModeTextAsync(text, cancellationToken);
    }
}