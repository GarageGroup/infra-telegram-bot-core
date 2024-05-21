using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class ChatUpdateJsonConverter : JsonConverter<ChatUpdate>
{
    public override ChatUpdate? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);

        var updateJson = jsonDocument.Deserialize<ChatUpdateJson>(BotDefaultJson.SerializerOptions);
        if (updateJson is null)
        {
            return null;
        }

        var userChat = GetUserChat(updateJson);
        if (userChat is null)
        {
            return null;
        }

        return new(updateJson.UpdateId, userChat.User, userChat.Chat)
        {
            Message = updateJson.Message,
            EditedMessage = updateJson.EditedMessage,
            ChannelPost = updateJson.ChannelPost,
            EditedChannelPost = updateJson.EditedChannelPost,
            CallbackQuery = updateJson.CallbackQuery,
            MyChatMember = updateJson.MyChatMember,
            ChatMember = updateJson.ChatMember,
            ChatJoinRequest = updateJson.ChatJoinRequest
        };
    }

    public override void Write(Utf8JsonWriter writer, ChatUpdate value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var updateJson = new ChatUpdateJson(value.UpdateId)
        {
            Message = value.Message,
            EditedMessage = value.EditedMessage,
            ChannelPost = value.ChannelPost,
            EditedChannelPost = value.EditedChannelPost,
            CallbackQuery = value.CallbackQuery,
            MyChatMember = value.MyChatMember,
            ChatMember = value.ChatMember,
            ChatJoinRequest = value.ChatJoinRequest
        };

        JsonSerializer.Serialize(writer, updateJson, BotDefaultJson.SerializerOptions);
    }

    private static BotUserChat? GetUserChat(ChatUpdateJson update)
        =>
        update switch
        {
            { Message: not null }               => FromMessage(update.Message),
            { EditedMessage: not null }         => FromMessage(update.EditedMessage),
            { ChannelPost: not null }           => FromMessage(update.ChannelPost),
            { EditedChannelPost: not null }     => FromMessage(update.EditedChannelPost),
            { CallbackQuery: not null }         => FromCallbackQuery(update.CallbackQuery),
            { MyChatMember: not null }          => FromMemberUpdated(update.MyChatMember),
            { ChatMember: not null }            => FromMemberUpdated(update.ChatMember),
            { ChatJoinRequest: not null }       => FromJoinRequest(update.ChatJoinRequest),
            _                                   => null
        };

    private static BotUserChat? FromMessage(BotMessage message)
        =>
        message.From is null ? null : new(message.From, message.Chat);

    private static BotUserChat? FromCallbackQuery(BotCallbackQuery query)
        =>
        query.Message is null ? null : new(query.From, query.Message.Chat);

    private static BotUserChat FromMemberUpdated(BotChatMemberUpdated member)
        =>
        new(member.From, member.Chat);

    private static BotUserChat FromJoinRequest(BotChatJoinRequest request)
        =>
        new(request.From, request.Chat);

    private sealed record class BotUserChat(BotUser User, BotChat Chat);
}