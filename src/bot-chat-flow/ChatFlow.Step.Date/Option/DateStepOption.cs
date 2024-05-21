using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed class DateStepOption<T>(string text, Func<DateOnly, Result<T, ChatRepeatState>> forward)
{
    public string Text { get; } = text.OrNullIfWhiteSpace() ?? "Enter a date";

    public string? InvalidDateText { get; init; }

    public FlatArray<FlatArray<KeyValuePair<string, DateOnly>>> Suggestions { get; init; }

    public Result<T, ChatRepeatState> Forward(DateOnly date)
        =>
        forward.Invoke(date);
}