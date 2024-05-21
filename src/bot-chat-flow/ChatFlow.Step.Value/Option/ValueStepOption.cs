using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed class ValueStepOption<T, TValue>(
    string text, Func<string, Result<TValue, ChatRepeatState>> parse, Func<TValue, Result<T, ChatRepeatState>> forward)
{
    public string Text { get; } = text.OrNullIfWhiteSpace() ?? "Enter a value";

    public FlatArray<FlatArray<KeyValuePair<string, TValue>>> Suggestions { get; init; }

    public Result<TValue, ChatRepeatState> Parse(string text)
        =>
        parse.Invoke(text);

    public Result<T, ChatRepeatState> Forward(TValue value)
        =>
        forward.Invoke(value);
}