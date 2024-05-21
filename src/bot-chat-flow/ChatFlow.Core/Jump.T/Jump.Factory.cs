namespace GarageGroup.Infra.Telegram.Bot;

partial record struct ChatFlowJump<T>
{
    public static ChatFlowJump<T> Next(T nextState)
        =>
        new(nextState, restart: false);

    public static ChatFlowJump<T> Restart(T nextState)
        =>
        new(nextState, restart: true);

    public static ChatFlowJump<T> Break(ChatBreakState breakState)
        =>
        new(breakState);

    public static ChatFlowJump<T> Repeat(ChatRepeatState repeatState)
        =>
        new(repeatState);
}