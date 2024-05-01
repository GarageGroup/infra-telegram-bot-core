using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotAdministratorChatMember : BotChatMemberBase
{
    [JsonConstructor]
    public BotAdministratorChatMember(
        BotUser user,
        bool canBeEdited,
        bool isAnonymous,
        bool canManageChat,
        bool canDeleteMessages,
        bool canManageVideoChats,
        bool canRestrictMembers,
        bool canPromoteMembers,
        bool canChangeInfo,
        bool canInviteUsers,
        bool canPostStories,
        bool canEditStories,
        bool canDeleteStories)
        : base(BotChatMemberStatus.Administrator, user)
    {
        CanBeEdited = canBeEdited;
        IsAnonymous = isAnonymous;
        CanManageChat = canManageChat;
        CanDeleteMessages = canDeleteMessages;
        CanManageVideoChats = canManageVideoChats;
        CanRestrictMembers = canRestrictMembers;
        CanPromoteMembers = canPromoteMembers;
        CanChangeInfo = canChangeInfo;
        CanInviteUsers = canInviteUsers;
        CanPostStories = canPostStories;
        CanEditStories = canEditStories;
        CanDeleteStories = canDeleteStories;
    }

    public bool CanBeEdited { get; }

    public bool IsAnonymous { get; }

    public bool CanManageChat { get; }

    public bool CanDeleteMessages { get; }

    public bool CanManageVideoChats { get; }

    public bool CanRestrictMembers { get; }

    public bool CanPromoteMembers { get; }

    public bool CanChangeInfo { get; }

    public bool CanInviteUsers { get; }

    public bool CanPostStories { get; }

    public bool CanEditStories { get; }

    public bool CanDeleteStories { get; }

    public bool? CanPostMessages { get; init; }

    public bool? CanEditMessages { get; init; }

    public bool? CanPinMessages { get; init; }

    public bool? CanManageTopics { get; init; }

    public string? CustomTitle { get; init; }
}