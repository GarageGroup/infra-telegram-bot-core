using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

[JsonConverter(typeof(BotChatMemberJsonConverter))]
public abstract record class BotChatMemberBase
{
    private protected BotChatMemberBase(BotChatMemberStatus status, BotUser user)
    {
        Status = status;
        User = user;
    }

    public BotChatMemberStatus Status { get; }

    public BotUser User { get; }
}