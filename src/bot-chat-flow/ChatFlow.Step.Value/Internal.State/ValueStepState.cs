using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed record class ValueStepState<TValue>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FlatArray<KeyValuePair<string, TValue>> Suggestions { get; init; }
}