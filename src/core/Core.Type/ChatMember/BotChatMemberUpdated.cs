using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotChatMemberUpdated
{
    [JsonConstructor]
    public BotChatMemberUpdated(
        BotChat chat,
        BotUser from,
        DateTime date,
        BotChatMemberBase oldChatMember,
        BotChatMemberBase newChatMember)
    {
        Chat = chat;
        From = from;
        Date = date;
        OldChatMember = oldChatMember;
        NewChatMember = newChatMember;
    }

    public BotChat Chat { get; }

    public BotUser From { get; }

    public DateTime Date { get; }

    public BotChatMemberBase OldChatMember { get; }

    public BotChatMemberBase NewChatMember { get; }

    public BotChatInviteLink? InviteLink { get; init; }

    public bool? ViaChatFolderInviteLink { get; init; }
}