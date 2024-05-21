using System;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed record class CallbackIn
{
    public CallbackIn(
        [FormBodyIn] string code,
        [FormBodyIn] string state,
        [FormBodyIn] string? error,
        [FormBodyIn("error_description")] string? errorDescription)
    {
        Code = code.OrEmpty();
        State = state.OrEmpty();
        Error = error.OrNullIfEmpty();
        ErrorDescription = errorDescription.OrNullIfEmpty();
    }

    public string Code { get; }

    public string State { get; }

    public string? Error { get; }

    public string? ErrorDescription { get; }
}