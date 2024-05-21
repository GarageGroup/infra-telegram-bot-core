namespace GarageGroup.Infra.Telegram.Bot;

public readonly partial record struct ChatFlowJump<T>
{
    private readonly T nextState;

    private readonly ChatBreakState breakState;

    private readonly ChatRepeatState repeatState;

    public ChatFlowJump(T nextState) : this(nextState: nextState, restart: false)
    {
    }

    public ChatFlowJump(T nextState, bool restart)
    {
        this.nextState = nextState;
        breakState = default;
        Tag = restart ? ChatFlowJumpTag.Restart : ChatFlowJumpTag.Next;
    }

    public ChatFlowJump(ChatBreakState breakState)
    {
        nextState = default!;
        repeatState = default;
        this.breakState = breakState;
        Tag = ChatFlowJumpTag.Break;
    }

    public ChatFlowJump(ChatRepeatState repeatState)
    {
        nextState = default!;
        breakState = default;
        this.repeatState = repeatState;
        Tag = ChatFlowJumpTag.Repeat;
    }

    public ChatFlowJumpTag Tag { get; }
}