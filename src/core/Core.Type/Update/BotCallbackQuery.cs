using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotCallbackQuery
{
    [JsonConstructor]
    public BotCallbackQuery(string id, BotUser from, string chatInstance)
    {
        Id = id.OrEmpty();
        From = from;
        ChatInstance = chatInstance.OrEmpty();
    }

    public string Id { get; }

    public BotUser From { get; }

    public string ChatInstance { get; }

    public BotMessage? Message { get; init; }

    public string? InlineMessageId { get; init; }

    public string? Data { get; init; }
}