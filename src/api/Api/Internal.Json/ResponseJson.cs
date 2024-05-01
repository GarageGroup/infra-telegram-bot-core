namespace GarageGroup.Infra.Telegram.Bot;

internal readonly record struct ResponseJson<T>
{
    public bool Ok { get; init; }

    public TelegramBotFailureCode? ErrorCode { get; init; }

    public string? Description { get; init; }

    public ResponseParametersJson? Parameters { get; init; }

    public T? Result { get; init; }
}