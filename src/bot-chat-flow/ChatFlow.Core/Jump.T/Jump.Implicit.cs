namespace GarageGroup.Infra.Telegram.Bot;

partial record struct ChatFlowJump<T>
{
    public static implicit operator ChatFlowJump<T>(T nextState)
        =>
        new(nextState);

    public static implicit operator ChatFlowJump<T>(ChatBreakState breakState)
        =>
        new(breakState);

    public static implicit operator ChatFlowJump<T>(ChatRepeatState repeatState)
        =>
        new(repeatState);
}