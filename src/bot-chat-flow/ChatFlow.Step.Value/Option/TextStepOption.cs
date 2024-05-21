using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed class TextStepOption<T>(string text, Func<string, Result<T, ChatRepeatState>> forward)
{
    public string Text { get; } = text.OrNullIfWhiteSpace() ?? "Enter a text";

    public FlatArray<FlatArray<KeyValuePair<string, string>>> Suggestions { get; init; }

    public Result<T, ChatRepeatState> Forward(string value)
        =>
        forward.Invoke(value);
}