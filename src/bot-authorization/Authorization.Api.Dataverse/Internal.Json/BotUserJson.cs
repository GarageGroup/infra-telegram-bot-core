using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

internal static class BotUserJson
{
    private const string EntitySetName = "gg_telegram_bot_users";

    private const string ChatIdFieldName = "gg_chat_id";

    private const string UserFieldName = "gg_systemuser_id";

    private const string LanguageCodeFieldName = "gg_language_code";

    private const string TimeZoneFieldName = "gg_time_zone";

    private const string IsUserSignedOutFieldName = "gg_is_user_signed_out";

    private static readonly FlatArray<string> SelectedFields;

    private static readonly FlatArray<DataverseExpandedField> expandedFields;

    static BotUserJson()
    {
        SelectedFields = new(ChatIdFieldName, LanguageCodeFieldName, TimeZoneFieldName, IsUserSignedOutFieldName);
        expandedFields =
        [
            new DataverseExpandedField(UserFieldName, SystemUserJson.SelectedFields)
        ];
    }

    internal static DataverseEntityGetIn BuildDataverseGetInput(long botId, long chatId)
        =>
        new(
            entityPluralName: EntitySetName,
            entityKey: BuildAlternateKey(botId: botId, chatId: chatId),
            selectFields: SelectedFields,
            expandFields: expandedFields);

    internal static DataverseEntityUpdateIn<In> BuildDataverseUpdateInput(In user)
        =>
        new(
            entityPluralName: EntitySetName,
            entityKey: BuildAlternateKey(botId: user.BotId, chatId: user.ChatId),
            entityData: user)
        {
            OperationType = user.IsSignedOut ? DataverseUpdateOperationType.Update : DataverseUpdateOperationType.Upsert
        };

    internal static string BuildUserLookupValue(Guid systemUserId)
        =>
        $"/systemusers({systemUserId:D})";

    internal sealed record class In
    {
        public In(long botId, long chatId)
        {
            BotId = botId;
            ChatId = chatId;
        }

        [JsonPropertyName("gg_bot_id")]
        public long BotId { get; }

        [JsonPropertyName(ChatIdFieldName)]
        public long ChatId { get; }

        [JsonPropertyName("gg_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; init; }

        [JsonPropertyName($"{UserFieldName}@odata.bind")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserLookupValue { get; init; }

        [JsonPropertyName(LanguageCodeFieldName)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LanguageCode { get; init; }

        [JsonPropertyName(TimeZoneFieldName)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TimeZone { get; init; }

        [JsonPropertyName(IsUserSignedOutFieldName)]
        public bool IsSignedOut { get; init; }
    }

    internal readonly record struct Out
    {
        [JsonPropertyName(ChatIdFieldName)]
        public long ChatId { get; init; }

        [JsonPropertyName(UserFieldName)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public SystemUserJson User { get; init; }

        [JsonPropertyName(LanguageCodeFieldName)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LanguageCode { get; init; }

        [JsonPropertyName(TimeZoneFieldName)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TimeZone { get; init; }

        [JsonPropertyName(IsUserSignedOutFieldName)]
        public bool IsSignedOut { get; init; }
    }

    private static DataverseAlternateKey BuildAlternateKey(long botId, long chatId)
        =>
        new("gg_telegram_bot_user_key", $"'{botId}.{chatId}'");
}