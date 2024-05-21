using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

[JsonConverter(typeof(InnerJsonConverter))]
public sealed partial class ChatState
{
    private static readonly JsonSerializerOptions SerializerOptions;

    static ChatState()
        =>
        SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly ConcurrentDictionary<string, JsonElement> values;

    public ChatState()
        =>
        values = [];

    public ChatState([AllowNull] IReadOnlyDictionary<string, JsonElement> values)
        =>
        this.values = values?.Count is not > 0 ? [] : new(values);

    private sealed class InnerJsonConverter : JsonConverter<ChatState>
    {
        public override ChatState? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is not JsonTokenType.StartObject)
            {
                throw new JsonException("Json must start with object token");
            }

            var values = new Dictionary<string, JsonElement>();
            while (reader.Read())
            {
                if (reader.TokenType is JsonTokenType.EndObject)
                {
                    return new(values);
                }

                if (reader.TokenType is not JsonTokenType.PropertyName)
                {
                    throw new JsonException("Json token must be property name");
                }

                var key = reader.GetString().OrEmpty();

                _ = reader.Read();
                values[key] = JsonElement.ParseValue(ref reader);
            }

            throw new JsonException("Json must end with object token");
        }

        public override void Write(Utf8JsonWriter writer, ChatState value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var item in value.values)
            {
                writer.WritePropertyName(item.Key);
                item.Value.WriteTo(writer);
            }

            writer.WriteEndObject();
        }
    }
}