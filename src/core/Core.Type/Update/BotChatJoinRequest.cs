using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotChatJoinRequest
{
    [JsonConstructor]
    public BotChatJoinRequest(BotChat chat, BotUser from, DateTime date)
    {
        Chat = chat;
        From = from;
        Date = date;
    }

    public BotChat Chat { get; }

    public BotUser From { get; }

    public DateTime Date { get; }

    public string? Bio { get; init; }

    public BotChatInviteLink? InviteLink { get; init; }
}