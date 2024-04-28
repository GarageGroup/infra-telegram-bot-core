using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotPollAnswer
{
    public BotPollAnswer(string pollId, BotUser user, FlatArray<int> optionIds)
    {
        PollId = pollId.OrEmpty();
        User = user ?? new(0, false, string.Empty);
        OptionIds = optionIds;
    }

    public string PollId { get; }

    public BotUser User { get; }

    public FlatArray<int> OptionIds { get; }
}