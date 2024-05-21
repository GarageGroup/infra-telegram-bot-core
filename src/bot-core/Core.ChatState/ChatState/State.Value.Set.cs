using System;
using System.Text.Json;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatState
{
    public void SetValue<T>(string key, T value)
        =>
        values[key.OrEmpty()] = JsonSerializer.SerializeToElement(value, SerializerOptions);

    public void SetValue<T>(T value)
        where T : IChatStateValue
        =>
        SetValue(T.Key, value);
}