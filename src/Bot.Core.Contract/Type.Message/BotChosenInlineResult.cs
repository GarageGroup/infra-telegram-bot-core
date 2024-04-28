using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotChosenInlineResult
{
    [JsonConstructor]
    public BotChosenInlineResult(string resultId, BotUser from, string query)
    {
        ResultId = resultId;
        From = from;
        Query = query.OrEmpty();
    }

    public string ResultId { get; }

    public BotUser From { get; }

    public string Query { get; }

    public BotLocation? Location { get; init; }

    public string? InlineMessageId { get; init; }
}