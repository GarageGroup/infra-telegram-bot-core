using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotMemberChatMember : BotChatMemberBase
{
    [JsonConstructor]
    public BotMemberChatMember(BotUser user) : base(BotChatMemberStatus.Member, user)
    {
    }
}