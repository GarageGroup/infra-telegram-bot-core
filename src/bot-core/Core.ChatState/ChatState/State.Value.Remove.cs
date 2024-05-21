using System;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatState
{
    public void RemoveValue(string key)
        =>
        values.TryRemove(key.OrEmpty(), out _);

    public void RemoveValue<T>()
        where T : IChatStateValue
        =>
        RemoveValue(T.Key);
}