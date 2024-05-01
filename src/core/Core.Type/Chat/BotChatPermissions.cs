namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotChatPermissions
{
    public bool? CanSendMessages { get; init; }

    public bool? CanSendAudios { get; init; }

    public bool? CanSendDocuments { get; init; }

    public bool? CanSendPhotos { get; init; }

    public bool? CanSendVideos { get; init; }

    public bool? CanSendVideoNotes { get; init; }

    public bool? CanSendVoiceNotes { get; init; }

    public bool? CanSendPolls { get; init; }

    public bool? CanSendOtherMessages { get; init; }

    public bool? CanAddWebPagePreviews { get; init; }

    public bool? CanChangeInfo { get; init; }

    public bool? CanInviteUsers { get; init; }

    public bool? CanPinMessages { get; init; }

    public bool? CanPinTopics { get; init; }
}