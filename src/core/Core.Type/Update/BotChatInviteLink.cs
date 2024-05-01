using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotChatInviteLink
{
    [JsonConstructor]
    public BotChatInviteLink(string inviteLink, BotUser creator, bool createsJoinRequest, bool isPrimary)
    {
        InviteLink = inviteLink.OrEmpty();
        Creator = creator;
        CreatesJoinRequest = createsJoinRequest;
        IsPrimary = isPrimary;
    }

    public string InviteLink { get; }

    public BotUser Creator { get; }

    public bool CreatesJoinRequest { get; }

    public bool IsPrimary { get; }

    public string? Name { get; init; }

    public DateTime? ExpireDate { get; init; }

    public int? MemberLimit { get; init; }

    public int? PendingJoinRequestCount { get; init; }
}