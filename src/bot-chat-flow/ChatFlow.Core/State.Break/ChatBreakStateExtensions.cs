using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

public static class ChatBreakStateExtensions
{
    public static ChatBreakState ToChatBreakState(
        this Exception? sourceException, [AllowNull] string userMessage, [AllowNull] string logMessage)
        =>
        new(userMessage, logMessage)
        {
            SourceException = sourceException
        };

    public static ChatBreakState ToChatBreakState<TFailureCode>(
        this Failure<TFailureCode> failure, [AllowNull] string userMessage)
        where TFailureCode : struct
        =>
        new(userMessage, failure.FailureMessage)
        {
            SourceException = failure.SourceException
        };
}