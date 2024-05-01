using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotChat
{
    [JsonConstructor]
    public BotChat(long id, BotChatType type)
    {
        Id = id;
        Type = type;
    }

    public long Id { get; }

    public BotChatType Type { get; }

    public string? Title { get; init; }

    public string? Username { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public BotChatPhoto? Photo { get; init; }

    public string? Bio { get; init; }

    public bool? HasPrivateForwards { get; init; }

    public string? Description { get; init; }

    public string? InviteLink { get; init; }

    public BotMessage? PinnedMessage { get; init; }

    public BotChatPermissions? Permissions { get; init; }

    public int? SlowModeDelay { get; init; }

    public int? MessageAutoDeleteTime { get; init; }

    public bool? HasProtectedContent { get; init; }

    public string? StickerSetName { get; init; }

    public bool? CanSetStickerSet { get; init; }

    public long? LinkedChatId { get; init; }

    public BotChatLocation? Location { get; init; }
}