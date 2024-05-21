using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

public readonly record struct ChatRepeatState
{
    public static ChatRepeatState From(
        [AllowNull] string userMessage,
        [AllowNull] string logMessage = null,
        Exception? sourceException = null)
        =>
        new(userMessage, logMessage)
        {
            SourceException = sourceException
        };

    public ChatRepeatState(
        [AllowNull] string userMessage,
        [AllowNull] string logMessage = null)
    {
        UserMessage = userMessage.OrNullIfWhiteSpace();
        LogMessage = logMessage.OrNullIfWhiteSpace();
    }

    public string? UserMessage { get; }

    public string? LogMessage { get; }

    public Exception? SourceException { get; init; } = null;
}