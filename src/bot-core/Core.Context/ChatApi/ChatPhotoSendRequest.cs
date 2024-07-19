using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ChatPhotoSendRequest
{
    public ChatPhotoSendRequest(string photo)
        =>
        Photo = photo.OrEmpty();

    public string Photo { get; }

    public string? BusinessConnectionId { get; init; }

    public int? MessageThreadId { get; init; }

    public string? Caption { get; init; }

    public BotParseMode ParseMode { get; init; }

    public FlatArray<BotMessageEntity> CaptionEntities { get; init; }

    public bool? ShowCaptionAboveMedia { get; init; }

    public bool? HasSpoiler { get; init; }

    public bool? DisableNotification { get; init; }

    public bool? ProtectContent { get; init; }

    public string? MessageEffectId { get; init; }

    public BotReplyParameters? ReplyParameters { get; init; }

    public BotReplyMarkupBase? ReplyMarkup { get; init; }
}