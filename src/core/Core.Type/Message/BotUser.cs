using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotUser
{
    [JsonConstructor]
    public BotUser(long id, bool isBot, string firstName)
    {
        Id = id;
        IsBot = isBot;
        FirstName = firstName.OrEmpty();
    }

    public long Id { get; }

    public bool IsBot { get; }

    public string FirstName { get; }

    public string? LastName { get; init; }

    public string? Username { get; init; }

    public string? LanguageCode { get; init; }

    public bool? IsPremium { get; init; }

    public bool? CanJoinGroups { get; init; }

    public bool? CanReadAllGroupMessages { get; init; }

    public bool? SupportsInlineQueries { get; init; }
}