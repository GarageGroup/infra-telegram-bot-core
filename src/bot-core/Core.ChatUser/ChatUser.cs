using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ChatUser
{
    private readonly CultureInfo culture = CultureInfo.InvariantCulture;

    private readonly TimeZoneInfo timeZone = TimeZoneInfo.Utc;

    [JsonConstructor]
    public ChatUser(long telegramId)
        =>
        TelegramId = telegramId;

    public long TelegramId { get; }

    [JsonConverter(typeof(CultureInfoJsonSerializer))]
    [AllowNull]
    public CultureInfo Culture
    {
        get => culture;
        init => culture = value ?? CultureInfo.InvariantCulture;
    }

    [JsonConverter(typeof(TimeZoneInfoJsonConverter))]
    [AllowNull]
    public TimeZoneInfo TimeZone
    {
        get => timeZone;
        init => timeZone = value ?? TimeZoneInfo.Utc;
    }

    public ChatUserIdentity? Identity { get; init; }
}