using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class BotUpdateJsonConverter : JsonConverter<BotUpdate>
{
    public override BotUpdate? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions _)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var updateJson = jsonDocument.Deserialize<BotUpdateJson>(BotDefaultJson.SerializerOptions);

        if (updateJson is null)
        {
            return null;
        }

        return new(updateJson.UpdateId)
        {
            Message = updateJson.Message,
            EditedMessage = updateJson.EditedMessage,
            ChannelPost = updateJson.ChannelPost,
            EditedChannelPost = updateJson.EditedChannelPost,
            InlineQuery = updateJson.InlineQuery,
            ChosenInlineResult = updateJson.ChosenInlineResult,
            CallbackQuery = updateJson.CallbackQuery,
            ShippingQuery = updateJson.ShippingQuery,
            PreCheckoutQuery = updateJson.PreCheckoutQuery,
            Poll = updateJson.Poll,
            PollAnswer = updateJson.PollAnswer,
            MyChatMember = updateJson.MyChatMember,
            ChatMember = updateJson.ChatMember,
            ChatJoinRequest = updateJson.ChatJoinRequest
        };
    }

    public override void Write(Utf8JsonWriter writer, BotUpdate value, JsonSerializerOptions _)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var updateJson = new BotUpdateJson(value.UpdateId)
        {
            Message = value.Message,
            EditedMessage = value.EditedMessage,
            ChannelPost = value.ChannelPost,
            EditedChannelPost = value.EditedChannelPost,
            InlineQuery = value.InlineQuery,
            ChosenInlineResult = value.ChosenInlineResult,
            CallbackQuery = value.CallbackQuery,
            ShippingQuery = value.ShippingQuery,
            PreCheckoutQuery = value.PreCheckoutQuery,
            Poll = value.Poll,
            PollAnswer = value.PollAnswer,
            MyChatMember = value.MyChatMember,
            ChatMember = value.ChatMember,
            ChatJoinRequest = value.ChatJoinRequest
        };

        JsonSerializer.Serialize(writer, updateJson, BotDefaultJson.SerializerOptions);
    }
}