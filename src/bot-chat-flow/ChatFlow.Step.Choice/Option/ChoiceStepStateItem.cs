using System.Text.Json;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed record class ChoiceStepStateItem
{
    public string? Id { get; init; }

    public string? Title { get; init; }

    public JsonElement Value { get; init; }
}