using System.Text.Json;

namespace GarageGroup.Infra.Telegram.Bot;

internal static partial class ChatFlowContextExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions;

    static ChatFlowContextExtensions()
        =>
        SerializerOptions = new(JsonSerializerDefaults.Web);
}