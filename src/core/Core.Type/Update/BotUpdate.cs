using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

[JsonConverter(typeof(BotUpdateJsonConverter))]
public sealed record class BotUpdate
{
    public BotUpdate(int updateId)
        =>
        UpdateId = updateId;

    public int UpdateId { get; }

    public BotMessage? Message { get; init; }

    public BotMessage? EditedMessage { get; init; }

    public BotMessage? ChannelPost { get; init; }

    public BotMessage? EditedChannelPost { get; init; }

    public BotInlineQuery? InlineQuery { get; init; }

    public BotChosenInlineResult? ChosenInlineResult { get; init; }

    public BotCallbackQuery? CallbackQuery { get; init; }

    public BotShippingQuery? ShippingQuery { get; init; }

    public BotPreCheckoutQuery? PreCheckoutQuery { get; init; }

    public BotPoll? Poll { get; init; }

    public BotPollAnswer? PollAnswer { get; init; }

    public BotChatMemberUpdated? MyChatMember { get; init; }

    public BotChatMemberUpdated? ChatMember { get; init; }

    public BotChatJoinRequest? ChatJoinRequest { get; init; }
}