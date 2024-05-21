namespace GarageGroup.Infra.Telegram.Bot;

public readonly record struct ChoiceStepRequest
{
    public string? Text { get; init; }
}