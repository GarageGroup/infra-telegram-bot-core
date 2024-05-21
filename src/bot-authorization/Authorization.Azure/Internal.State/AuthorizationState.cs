using System;

namespace GarageGroup.Infra.Telegram.Bot;

internal readonly record struct AuthorizationState : IChatStateValue
{
    public static string Key { get; } = "authorization";

    public required long BotId { get; init; }

    public required string? BotName { get; init; }

    public FlatArray<string> LanguageCodes { get; init; }

    public int? LanguageChoiceMessageId { get; init; }

    public string? ChosenLanguageCode { get; init; }

    public int? SignInMessageId { get; init; }
}