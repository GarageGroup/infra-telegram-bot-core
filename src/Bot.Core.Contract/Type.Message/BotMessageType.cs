namespace GarageGroup.Infra.Telegram.Bot;

public enum BotMessageType
{
    Unknown,

    Text,

    Photo,

    Audio,

    Video,

    Voice,

    Document,

    Sticker,

    Location,

    Contact,

    Venue,

    Game,

    VideoNote,

    Invoice,

    SuccessfulPayment,

    WebsiteConnected,

    ChatMembersAdded,

    ChatMemberLeft,

    ChatTitleChanged,

    ChatPhotoChanged,

    MessagePinned,

    ChatPhotoDeleted,

    GroupCreated,

    SupergroupCreated,

    ChannelCreated,

    MigratedToSupergroup,

    MigratedFromGroup,

    Poll,

    Dice,

    MessageAutoDeleteTimerChanged,

    ProximityAlertTriggered,

    WebAppData,

    VideoChatScheduled,

    VideoChatStarted,

    VideoChatEnded,

    VideoChatParticipantsInvited
}