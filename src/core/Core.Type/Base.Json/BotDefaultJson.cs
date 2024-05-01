using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public static class BotDefaultJson
{
    public static readonly JsonSerializerOptions SerializerOptions;

    static BotDefaultJson()
        =>
        SerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower),
                new UnixDateTimeJsonConverter(),
                new BotChatMemberJsonConverter(),
                new BotPassportDataJsonConverter()
            },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
}