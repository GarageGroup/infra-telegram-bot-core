using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class BotChatMemberJsonConverter : JsonConverter<BotChatMemberBase>
{
    public override BotChatMemberBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);

        if (jsonDocument is null)
        {
            return null;
        }

        var statusJsonElement = GetStatusJsonElement(jsonDocument, options);

        return GetChatMemberStatus(statusJsonElement) switch
        {
            BotChatMemberStatus.Creator => jsonDocument.Deserialize<BotOwnerChatMember>(options),
            BotChatMemberStatus.Administrator => jsonDocument.Deserialize<BotAdministratorChatMember>(options),
            BotChatMemberStatus.Member => jsonDocument.Deserialize<BotMemberChatMember>(options),
            BotChatMemberStatus.Left => jsonDocument.Deserialize<BotLeftChatMemeber>(options),
            BotChatMemberStatus.Kicked => jsonDocument.Deserialize<BotBannedChatMember>(options),
            BotChatMemberStatus.Restricted => jsonDocument.Deserialize<BotRestrictedChatMember>(options),
            var unexpected => throw new JsonException($"Bot chat member status {unexpected} is unexpected")
        };
    }

    public override void Write(Utf8JsonWriter writer, BotChatMemberBase value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        if (value is BotOwnerChatMember ownerChatMember)
        {
            JsonSerializer.Serialize(writer, ownerChatMember, options);
            return;
        }

        if (value is BotAdministratorChatMember administratorChatMember)
        {
            JsonSerializer.Serialize(writer, administratorChatMember, options);
            return;
        }

        if (value is BotMemberChatMember memberChatMember)
        {
            JsonSerializer.Serialize(writer, memberChatMember, options);
            return;
        }

        if (value is BotLeftChatMemeber leftChatMemeber)
        {
            JsonSerializer.Serialize(writer, leftChatMemeber, options);
            return;
        }

        if (value is BotBannedChatMember bannedChatMember)
        {
            JsonSerializer.Serialize(writer, bannedChatMember, options);
            return;
        }

        if (value is BotRestrictedChatMember restrictedChatMember)
        {
            JsonSerializer.Serialize(writer, restrictedChatMember, options);
            return;
        }

        throw new NotSupportedException($"Type {value.GetType()} serialization is not supported");
    }

    private static BotChatMemberStatus GetChatMemberStatus(JsonElement jsonElement)
    {
        if (jsonElement.ValueKind is JsonValueKind.Number)
        {
            return (BotChatMemberStatus)jsonElement.GetInt32();
        }

        if (jsonElement.ValueKind is JsonValueKind.String)
        {
            var statusName = jsonElement.GetString();
            return Enum.Parse<BotChatMemberStatus>(statusName.OrEmpty(), true);
        }

        throw new JsonException($"Status value kind {jsonElement.ValueKind} is unexpected");
    }

    private static JsonElement GetStatusJsonElement(JsonDocument jsonDocument, JsonSerializerOptions options)
    {
        var statusPropertyName = nameof(BotChatMemberBase.Status);
        if (options.PropertyNamingPolicy is not null)
        {
            statusPropertyName = options.PropertyNamingPolicy.ConvertName(statusPropertyName);
        }

        if (jsonDocument.RootElement.TryGetProperty(statusPropertyName, out var jsonElement))
        {
            return jsonElement;
        }

        throw new JsonException($"Property {statusPropertyName} must be specified");
    }
}