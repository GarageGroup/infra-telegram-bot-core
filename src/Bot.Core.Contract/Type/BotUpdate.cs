using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotUpdate
{
    [JsonConstructor]
    public BotUpdate(BotUpdateType type, int updateId)
    {
        Type = type;
        UpdateId = updateId;
    }

    public BotUpdateType Type { get; }

    public int UpdateId { get; }

    public BotChatMemberUpdated? ChatMember { get; init; }

    public BotChatMemberUpdated? MyChatMember { get; init; }

    public BotPollAnswer? PollAnswer { get; init; }

    public BotPoll? Poll { get; init; }

    public BotPreCheckoutQuery? PreCheckoutQuery { get; init; }

    public BotShippingQuery? ShippingQuery { get; init; }

    public BotChatJoinRequest? ChatJoinRequest { get; init; }

    public BotCallbackQuery? CallbackQuery { get; init; }

    public BotInlineQuery? InlineQuery { get; init; }

    public BotMessage? EditedChannelPost { get; init; }

    public BotMessage? ChannelPost { get; init; }

    public BotMessage? EditedBotMessage { get; init; }

    public BotMessage? BotMessage { get; init; }

    public BotChosenInlineResult? ChosenInlineResult { get; init; }
}