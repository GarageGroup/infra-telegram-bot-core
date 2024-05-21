using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

public readonly record struct ChatBreakState
{
    public ChatBreakState(
        [AllowNull] string userMessage = default,
        [AllowNull] string logMessage = default)
    {
        UserMessage = userMessage.OrNullIfWhiteSpace();
        LogMessage = logMessage.OrNullIfWhiteSpace();
    }

    public string? UserMessage { get; }

    public string? LogMessage { get; }

    public Exception? SourceException { get; init; }

    public static ChatBreakState From(
        [AllowNull] string userMessage, [AllowNull] string logMessage = default, Exception? sourceException = default)
        =>
        new(userMessage, logMessage)
        {
            SourceException = sourceException
        };
}