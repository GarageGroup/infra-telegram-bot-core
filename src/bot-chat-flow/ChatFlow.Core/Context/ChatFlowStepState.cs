using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

[JsonConverter(typeof(InnerJsonConverter))]
public sealed class ChatFlowStepState
{
    private static readonly JsonSerializerOptions SerializerOptions;

    static ChatFlowStepState()
        =>
        SerializerOptions = new(JsonSerializerDefaults.Web);

    private JsonElement? state = null;

    public T? Get<T>()
    {
        if (state is null)
        {
            return default;
        }

        return state.Value.Deserialize<T>(SerializerOptions);
    }

    public void Set<T>(T value)
    {
        if (value is null)
        {
            state = null;
        }

        state = JsonSerializer.SerializeToElement(value, SerializerOptions);
    }

    private sealed class InnerJsonConverter : JsonConverter<ChatFlowStepState>
    {
        public override ChatFlowStepState? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            =>
            new()
            {
                state = JsonElement.ParseValue(ref reader)
            };

        public override void Write(Utf8JsonWriter writer, ChatFlowStepState value, JsonSerializerOptions options)
        {
            if (value.state is null)
            {
                writer.WriteNullValue();
                return;
            }

            value.state.Value.WriteTo(writer);
        }
    }
}