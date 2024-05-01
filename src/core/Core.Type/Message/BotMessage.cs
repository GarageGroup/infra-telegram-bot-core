using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotMessage
{
    [JsonConstructor]
    public BotMessage(int messageId, DateTime date, BotChat chat)
    {
        MessageId = messageId;
        Date = date;
        Chat = chat;
    }

    public int MessageId { get; }

    public DateTime Date { get; }

    public BotChat Chat { get; }

    public BotContact? Contact { get; init; }

    public BotDice? Dice { get; init; }

    public BotPoll? Poll { get; init; }

    public BotVenue? Venue { get; init; }

    public BotLocation? Location { get; init; }

    public FlatArray<BotUser> NewChatMembers { get; init; }

    public BotUser? LeftChatMember { get; init; }

    public string? NewChatTitle { get; init; }

    public FlatArray<BotPhotoSize> NewChatPhoto { get; init; }

    public bool? DeleteChatPhoto { get; init; }

    public bool? GroupChatCreated { get; init; }

    public bool? SupergroupChatCreated { get; init; }

    public FlatArray<string> CaptionEntityValues { get; init; }

    public bool? ChannelChatCreated { get; init; }

    public long? MigrateToChatId { get; init; }

    public long? MigrateFromChatId { get; init; }

    public BotMessage? PinnedMessage { get; init; }

    public BotInvoice? Invoice { get; init; }

    public BotSuccessfulPayment? SuccessfulPayment { get; init; }

    public string? ConnectedWebsite { get; init; }

    public BotPassportData? PassportData { get; init; }

    public BotWebAppData? WebAppData { get; init; }

    public BotInlineKeyboardMarkup? ReplyMarkup { get; init; }

    public FlatArray<BotMessageEntity> CaptionEntities { get; init; }

    public BotVoice? Voice { get; init; }

    public BotUser? From { get; init; }

    public BotChat? SenderChat { get; init; }

    public BotUser? ForwardFrom { get; init; }

    public BotChat? ForwardFromChat { get; init; }

    public int? ForwardFromMessageId { get; init; }

    public string? ForwardSignature { get; init; }

    public string? ForwardSenderName { get; init; }

    public DateTime? ForwardDate { get; init; }

    public bool? IsAutomaticForward { get; init; }

    public BotMessage? ReplyToMessage { get; init; }

    public string? Caption { get; init; }

    public BotUser? ViaBot { get; init; }

    public bool? HasProtectedContent { get; init; }

    public string? MediaGroupId { get; init; }

    public string? AuthorSignature { get; init; }

    public string? Text { get; init; }

    public FlatArray<BotMessageEntity> Entities { get; init; }

    public FlatArray<string> EntityValues { get; init; }

    public BotAnimation? Animation { get; init; }

    public BotAudio? Audio { get; init; }

    public BotDocument? Document { get; init; }

    public FlatArray<BotPhotoSize> Photo { get; init; }

    public BotSticker? Sticker { get; init; }

    public BotVideo? Video { get; init; }

    public BotVideoNote? VideoNote { get; init; }

    public DateTime? EditDate { get; init; }
}