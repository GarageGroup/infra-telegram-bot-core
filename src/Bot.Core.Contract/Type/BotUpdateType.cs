namespace GarageGroup.Infra.Telegram.Bot;

public enum BotUpdateType
{
    Unknown,

    Message,

    InlineQuery,

    ChosenInlineResult,

    CallbackQuery,

    EditedMessage,

    ChannelPost,

    EditedChannelPost,

    ShippingQuery,

    PreCheckoutQuery,

    Poll,

    PollAnswer,

    MyChatMember,

    ChatMember,

    ChatJoinRequest
}