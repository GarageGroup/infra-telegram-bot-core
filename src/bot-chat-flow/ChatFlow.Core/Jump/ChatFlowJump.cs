namespace GarageGroup.Infra.Telegram.Bot;

public static class ChatFlowJump
{
    public static ChatFlowJump<T> Next<T>(T nextState)
        =>
        new(nextState, restart: false);

    public static ChatFlowJump<T> Restart<T>(T initialState)
        =>
        new(initialState, restart: true);

    public static ChatFlowJump<T> Break<T>(ChatBreakState breakState)
        =>
        new(breakState);

    public static ChatFlowJump<T> Repeat<T>(ChatRepeatState repeatState)
        =>
        new(repeatState);
}