using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotRestrictedChatMember : BotChatMemberBase
{
    [JsonConstructor]
    public BotRestrictedChatMember(
        bool isMember,
        bool canChangeInfo,
        bool canInviteUsers,
        bool canPinMessages,
        bool canSendMessages,
        bool canSendMediaMessages,
        bool canSendPolls,
        bool canSendOtherMessages,
        bool canAddWebPagePreviews,
        BotUser user)
        : base(BotChatMemberStatus.Restricted, user)
    {
        IsMember = isMember;
        CanChangeInfo = canChangeInfo;
        CanInviteUsers = canInviteUsers;
        CanPinMessages = canPinMessages;
        CanSendMessages = canSendMessages;
        CanSendMediaMessages = canSendMediaMessages;
        CanSendPolls = canSendPolls;
        CanSendOtherMessages = canSendOtherMessages;
        CanAddWebPagePreviews = canAddWebPagePreviews;
    }

    public bool IsMember { get; }

    public bool CanChangeInfo { get; }

    public bool CanInviteUsers { get; }

    public bool CanPinMessages { get; }

    public bool CanSendMessages { get; }

    public bool CanSendMediaMessages { get; }

    public bool CanSendPolls { get; }

    public bool CanSendOtherMessages { get; }

    public bool CanAddWebPagePreviews { get; }

    public DateTime? UntilDate { get; init; }
}