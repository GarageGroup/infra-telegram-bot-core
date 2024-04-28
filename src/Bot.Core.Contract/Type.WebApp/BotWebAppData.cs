using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotWebAppData
{
    [JsonConstructor]
    public BotWebAppData(string data, string buttonText)
    {
        Data = data.OrEmpty();
        ButtonText = buttonText.OrEmpty();
    }

    public string Data { get; }

    public string ButtonText { get; }
}