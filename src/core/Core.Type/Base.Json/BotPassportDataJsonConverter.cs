using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class BotPassportDataJsonConverter : JsonConverter<BotPassportData>
{
    public override BotPassportData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        if (jsonDocument is null)
        {
            return null;
        }

        var data = ReadData() ?? Array.Empty<BotEncryptedPassportElement>();
        var credentials = ReadCredentials() ?? new(string.Empty, string.Empty, string.Empty);

        return new(data, credentials);

        BotEncryptedPassportElement[]? ReadData()
        {
            var propertyName = GetJsonPropertyName(options, nameof(BotPassportData.Data));
            if (jsonDocument.RootElement.TryGetProperty(propertyName, out var jsonElement) is false)
            {
                return null;
            }

            return jsonElement.Deserialize<BotEncryptedPassportElement[]>(options);
        }

        BotEncryptedCredentials? ReadCredentials()
        {
            var propertyName = GetJsonPropertyName(options, nameof(BotPassportData.Credentials));
            if (jsonDocument.RootElement.TryGetProperty(propertyName, out var jsonElement) is false)
            {
                return null;
            }

            return jsonElement.Deserialize<BotEncryptedCredentials>(options);
        }
    }

    public override void Write(Utf8JsonWriter writer, BotPassportData value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        writer.WritePropertyName(GetJsonPropertyName(options, nameof(BotPassportData.Data)));
        JsonSerializer.Serialize(writer, value.Data.ToArray(), options);

        writer.WritePropertyName(GetJsonPropertyName(options, nameof(BotPassportData.Credentials)));
        JsonSerializer.Serialize(writer, value.Credentials, options);

        writer.WriteEndObject();
    }

    private static string GetJsonPropertyName(JsonSerializerOptions options, string sourcePropertyName)
        =>
        options.PropertyNamingPolicy?.ConvertName(sourcePropertyName) ?? sourcePropertyName;
}