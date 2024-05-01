using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotRestrictedChatMember : BotChatMemberBase
{
    [JsonConstructor]
    public BotRestrictedChatMember(
        BotUser user,
        bool isMember,
        bool canSendMessages,
        bool canSendAudios,
        bool canSendDocuments,
        bool canSendPhotos,
        bool canSendVideos,
        bool canSendVideoNotes,
        bool canSendVoiceNotes,
        bool canSendPolls,
        bool canSendOtherMessages,
        bool canAddWebPagePreviews,
        bool canChangeInfo,
        bool canInviteUsers,
        bool canPinMessages,
        bool canManageTopics)
        : base(BotChatMemberStatus.Restricted, user)
    {
        IsMember = isMember;
        CanSendMessages = canSendMessages;
        CanSendAudios = canSendAudios;
        CanSendDocuments = canSendDocuments;
        CanSendPhotos = canSendPhotos;
        CanSendVideos = canSendVideos;
        CanSendVideoNotes = canSendVideoNotes;
        CanSendVoiceNotes = canSendVoiceNotes;
        CanSendPolls = canSendPolls;
        CanSendOtherMessages = canSendOtherMessages;
        CanAddWebPagePreviews = canAddWebPagePreviews;
        CanChangeInfo = canChangeInfo;
        CanInviteUsers = canInviteUsers;
        CanPinMessages = canPinMessages;
        CanManageTopics = canManageTopics;
    }

    public bool IsMember { get; }

    public bool CanSendMessages { get; }

    public bool CanSendAudios { get; }

    public bool CanSendDocuments { get; }

    public bool CanSendPhotos { get; }

    public bool CanSendVideos { get; }

    public bool CanSendVideoNotes { get; }

    public bool CanSendVoiceNotes { get; }

    public bool CanSendPolls { get; }

    public bool CanSendOtherMessages { get; }

    public bool CanAddWebPagePreviews { get; }

    public bool CanChangeInfo { get; }

    public bool CanInviteUsers { get; }

    public bool CanPinMessages { get; }

    public bool CanManageTopics { get; }

    public DateTime? UntilDate { get; init; }
}