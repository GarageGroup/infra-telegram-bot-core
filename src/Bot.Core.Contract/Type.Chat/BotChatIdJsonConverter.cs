using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class BotChatIdJsonConverter : JsonConverter<BotChatId>
{
    public override BotChatId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType is JsonTokenType.String)
        {
            var username = reader.GetString();
            return string.IsNullOrEmpty(username) ? null : new(username);
        }

        if (reader.TokenType is JsonTokenType.Number)
        {
            var identifier = reader.GetInt64();
            return new(identifier);
        }

        throw new JsonException($"Token type must be JsonTokenType.Null, JsonTokenType.String or JsonTokenType.Number");
    }

    public override void Write(Utf8JsonWriter writer, BotChatId value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        if (value.Username is not null)
        {
            writer.WriteStringValue(value.Username);
            return;
        }

        if (value.Identifier is not null)
        {
            writer.WriteNumberValue(value.Identifier.Value);
            return;
        }

        writer.WriteNullValue();
    }
}