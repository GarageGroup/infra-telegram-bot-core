using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ChoiceStepItem<TValue>
{
    public ChoiceStepItem(string id, [AllowNull] string title, TValue value)
    {
        Id = id.OrEmpty();
        Title = title.OrEmpty();
        Value = value;
    }

    public string Id { get; }

    public string Title { get; }

    public TValue Value { get; }
}