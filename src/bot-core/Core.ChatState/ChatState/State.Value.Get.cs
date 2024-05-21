using System;
using System.Text.Json;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatState
{
    public T? GetValue<T>(string key)
    {
        if (values.TryGetValue(key.OrEmpty(), out var jsonValue) is false)
        {
            return default;
        }

        return jsonValue.Deserialize<T>(SerializerOptions);
    }

    public T? GetValue<T>()
        where T : IChatStateValue
        =>
        GetValue<T>(T.Key);
}