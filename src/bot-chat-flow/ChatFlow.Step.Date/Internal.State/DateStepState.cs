using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed record class DateStepState
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FlatArray<KeyValuePair<string, DateOnly>> Suggestions { get; init; }

    public string? InvalidDateText { get; init; }
}