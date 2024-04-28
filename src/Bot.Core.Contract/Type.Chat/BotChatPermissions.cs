namespace GarageGroup.Infra.Telegram.Bot;

public readonly record struct BotChatPermissions
{
    public bool? CanSendMessages { get; init; }

    public bool? CanSendMediaMessages { get; init; }

    public bool? CanSendPolls { get; init; }

    public bool? CanSendOtherMessages { get; init; }

    public bool? CanAddWebPagePreviews { get; init; }

    public bool? CanChangeInfo { get; init; }

    public bool? CanInviteUsers { get; init; }

    public bool? CanPinMessages { get; init; }
}