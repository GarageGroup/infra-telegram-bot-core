using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class EntityCardOption
{
    private const string DefaultHeaderText = "Operation data";

    public EntityCardOption([AllowNull] string headerText = DefaultHeaderText)
        =>
        HeaderText = headerText.OrNullIfWhiteSpace() ?? DefaultHeaderText;

    public string HeaderText { get; }

    public required FlatArray<KeyValuePair<string, string?>> FieldValues { get; init; }
}