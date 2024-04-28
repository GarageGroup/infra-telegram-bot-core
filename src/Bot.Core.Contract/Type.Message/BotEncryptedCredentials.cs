using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotEncryptedCredentials
{
    [JsonConstructor]
    public BotEncryptedCredentials(string data, string hash, string secret)
    {
        Data = data.OrEmpty();
        Hash = hash.OrEmpty();
        Secret = secret.OrEmpty();
    }

    public string Data { get; }

    public string Hash { get; }

    public string Secret { get; }
}