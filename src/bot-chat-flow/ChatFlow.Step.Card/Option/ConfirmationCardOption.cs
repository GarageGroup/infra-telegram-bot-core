namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ConfirmationCardOption<T>
{
    public required EntityCardOption Entity { get; init; }

    public required CardKeyboardOption<T> Keyboard { get; init; }
}