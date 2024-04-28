using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotAdministratorChatMember : BotChatMemberBase
{
    [JsonConstructor]
    public BotAdministratorChatMember(
        bool canBeEdited,
        bool isAnonymous,
        bool canManageChat,
        bool canDeleteMessages,
        bool canManageVoiceChats,
        bool canManageVideoChats,
        bool canRestrictMembers,
        bool canPromoteMembers,
        bool canChangeInfo,
        bool canInviteUsers,
        BotUser user)
        : base(BotChatMemberStatus.Administrator, user)
    {
        CanBeEdited = canBeEdited;
        IsAnonymous = isAnonymous;
        CanManageChat = canManageChat;
        CanDeleteMessages = canDeleteMessages;
        CanManageVoiceChats = canManageVoiceChats;
        CanManageVideoChats = canManageVideoChats;
        CanRestrictMembers = canRestrictMembers;
        CanPromoteMembers = canPromoteMembers;
        CanChangeInfo = canChangeInfo;
        CanInviteUsers = canInviteUsers;
    }

    public bool CanBeEdited { get; }

    public bool IsAnonymous { get; }

    public bool CanManageChat { get; }

    public bool CanDeleteMessages { get; }

    public bool CanManageVoiceChats { get; }

    public bool CanManageVideoChats { get; }

    public bool CanRestrictMembers { get; }

    public bool CanPromoteMembers { get; }

    public bool CanChangeInfo { get; }

    public bool CanInviteUsers { get; }

    public string? CustomTitle { get; init; }

    public bool? CanPostMessages { get; init; }

    public bool? CanEditMessages { get; init; }

    public bool? CanPinMessages { get; init; }
}