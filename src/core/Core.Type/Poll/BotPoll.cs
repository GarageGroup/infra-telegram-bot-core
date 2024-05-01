using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotPoll
{
    public BotPoll(
        string id,
        string question,
        FlatArray<BotPollOption> options,
        int totalVoterCount,
        bool isClosed,
        bool isAnonymous,
        BotPollType type,
        bool allowsMultipleAnswers)
    {
        Id = id.OrEmpty();
        Question = question.OrEmpty();
        Options = options;
        TotalVoterCount = totalVoterCount;
        IsClosed = isClosed;
        IsAnonymous = isAnonymous;
        Type = type;
        AllowsMultipleAnswers = allowsMultipleAnswers;
    }

    public string Id { get; }

    public string Question { get; }

    public FlatArray<BotPollOption> Options { get; }

    public int TotalVoterCount { get; }

    public bool IsClosed { get; }

    public bool IsAnonymous { get; }

    public BotPollType Type { get; }

    public bool AllowsMultipleAnswers { get; }
}