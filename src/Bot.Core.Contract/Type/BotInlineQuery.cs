using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotInlineQuery
{
    [JsonConstructor]
    public BotInlineQuery(string id, BotUser from, string query, string offset)
    {
        Id = id.OrEmpty();
        From = from;
        Query = query.OrEmpty();
        Offset = offset.OrEmpty();
    }

    public string Id { get; }

    public BotUser From { get; }

    public string Query { get; }

    public string Offset { get; }

    public BotChatType? ChatType { get; init; }

    public BotLocation? Location { get; init; }
}