using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotWebAppInfo
{
    [JsonConstructor]
    public BotWebAppInfo(string url)
        =>
        Url = url.OrEmpty();

    public string Url { get; }
}