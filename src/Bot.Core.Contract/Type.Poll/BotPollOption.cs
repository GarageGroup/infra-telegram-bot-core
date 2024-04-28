using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotPollOption
{
    [JsonConstructor]
    public BotPollOption(string text, int voterCount)
    {
        Text = text.OrEmpty();
        VoterCount = voterCount;
    }

    public string Text { get; }

    public int VoterCount { get; }
}