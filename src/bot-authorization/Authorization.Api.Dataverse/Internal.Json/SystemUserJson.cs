using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

internal readonly record struct SystemUserJson
{
    private const string SystemUserEntitySetName = "systemusers";

    private const string ActiveDirectoryObjectIdFieldName = "azureactivedirectoryobjectid";

    private const string SystemUserIdFieldName = "systemuserid";

    private const string FullNameFieldName = "yomifullname";

    private const string IsDisabledFieldName = "isdisabled";

    internal static readonly FlatArray<string> SelectedFields;

    static SystemUserJson()
        =>
        SelectedFields = new(SystemUserIdFieldName, FullNameFieldName, IsDisabledFieldName);

    internal static DataverseEntityGetIn BuildDataverseGetInput(string activeDirectoryUserId)
        =>
        new(
            entityPluralName: SystemUserEntitySetName,
            entityKey: new DataverseAlternateKey(ActiveDirectoryObjectIdFieldName, activeDirectoryUserId),
            selectFields: SelectedFields);

    [JsonPropertyName(SystemUserIdFieldName)]
    public Guid SystemUserId { get; init; }

    [JsonPropertyName(FullNameFieldName)]
    public string? FullName { get; init; }

    [JsonPropertyName(IsDisabledFieldName)]
    public bool IsDisabled { get; init; }
}