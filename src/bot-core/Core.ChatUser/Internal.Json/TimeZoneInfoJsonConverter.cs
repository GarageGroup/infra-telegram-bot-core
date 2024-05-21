using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TimeZoneConverter;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class TimeZoneInfoJsonConverter : JsonConverter<TimeZoneInfo>
{
    public override TimeZoneInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
        {
            throw new JsonException("Token type must be String");
        }

        var id = reader.GetString();
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        return TZConvert.TryGetTimeZoneInfo(id, out var timeZone) ? timeZone : null;
    }

    public override void Write(Utf8JsonWriter writer, TimeZoneInfo value, JsonSerializerOptions options)
        =>
        writer.WriteStringValue(value.Id);
}