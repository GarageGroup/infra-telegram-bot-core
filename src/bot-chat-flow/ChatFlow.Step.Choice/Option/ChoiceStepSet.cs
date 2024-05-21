using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ChoiceStepSet<TValue>
{
    private const string DefaultChoiceText = "Select an item";

    public ChoiceStepSet([AllowNull] string choiceText = DefaultChoiceText)
        =>
        ChoiceText = choiceText.OrNullIfWhiteSpace() ?? DefaultChoiceText;

    public string ChoiceText { get; }

    public FlatArray<ChoiceStepItem<TValue>> Items { get; init; }
}