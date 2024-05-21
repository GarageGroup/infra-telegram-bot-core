using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed partial class ChatFlow<T>
{
    private const int MaxRestartCount = 10;

    private readonly string chatFlowId;

    private IChatContext chatContext;

    private readonly Func<T> initialFactory;

    private readonly List<Func<IChatFlowContext<T>, CancellationToken, ValueTask<ChatFlowJump<T>>>> flowSteps = [];

    internal ChatFlow(string chatFlowId, IChatContext chatContext, Func<T> initialFactory)
    {
        this.chatFlowId = chatFlowId.OrEmpty();
        this.chatContext = chatContext;
        this.initialFactory = initialFactory;
    }

    private sealed record class FlowStateJson : IChatStateValue
    {
        public static string Key { get; } = "flowState";

        public FlowStateJson(int position, T? flowState, [AllowNull] ChatFlowStepState stepState)
        {
            Position = position;
            FlowState = flowState;
            StepState = stepState ?? new();
        }

        public int Position { get; }

        public T? FlowState { get; }

        public ChatFlowStepState StepState { get; }
    }
}