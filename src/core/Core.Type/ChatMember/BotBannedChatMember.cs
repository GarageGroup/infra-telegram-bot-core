using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotBannedChatMember : BotChatMemberBase
{
    [JsonConstructor]
    public BotBannedChatMember(BotUser user) : base(BotChatMemberStatus.Kicked, user)
    {
    }

    public DateTime? UntilDate { get; init; }
}