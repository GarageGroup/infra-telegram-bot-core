using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotLoginUrl
{
    [JsonConstructor]
    public BotLoginUrl(string url)
        =>
        Url = url.OrEmpty();

    public string Url { get; }

    public string? ForwardText { get; init; }

    public string? BotUsername { get; init; }

    public bool? RequestWriteAccess { get; init; }
}