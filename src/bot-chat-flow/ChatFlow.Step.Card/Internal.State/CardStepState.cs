namespace GarageGroup.Infra.Telegram.Bot;

internal sealed record class CardStepState
{
    public string? ConfirmButtonText { get; init; }

    public string? CancelButtonText { get; init; }
}