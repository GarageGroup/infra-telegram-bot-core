using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotLeftChatMemeber : BotChatMemberBase
{
    [JsonConstructor]
    public BotLeftChatMemeber(BotUser user) : base(BotChatMemberStatus.Left, user)
    {
    }
}