using System;

namespace GarageGroup.Infra.Telegram.Bot;

internal readonly record struct ChoiceStepState
{
    public FlatArray<int> MessageIds { get; init; }

    public FlatArray<ChoiceStepStateItem> Items { get; init; }
}