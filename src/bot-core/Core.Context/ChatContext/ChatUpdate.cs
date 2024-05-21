using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

[JsonConverter(typeof(ChatUpdateJsonConverter))]
public sealed record class ChatUpdate
{
    public ChatUpdate(int updateId, BotUser user, BotChat chat)
    {
        UpdateId = updateId;
        User = user;
        Chat = chat;
    }

    public int UpdateId { get; }

    [JsonIgnore]
    public BotUser User { get; }

    [JsonIgnore]
    public BotChat Chat { get; }

    public BotMessage? Message { get; init; }

    public BotMessage? EditedMessage { get; init; }

    public BotMessage? ChannelPost { get; init; }

    public BotMessage? EditedChannelPost { get; init; }

    public BotCallbackQuery? CallbackQuery { get; init; }

    public BotChatMemberUpdated? MyChatMember { get; init; }

    public BotChatMemberUpdated? ChatMember { get; init; }

    public BotChatJoinRequest? ChatJoinRequest { get; init; }
}