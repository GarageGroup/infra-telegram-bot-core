using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotOwnerChatMember : BotChatMemberBase
{
    [JsonConstructor]
    public BotOwnerChatMember(bool isAnonymous, BotUser user) : base(BotChatMemberStatus.Creator, user)
        =>
        IsAnonymous = isAnonymous;

    public bool IsAnonymous { get; }

    public string? CustomTitle { get; init; }
}