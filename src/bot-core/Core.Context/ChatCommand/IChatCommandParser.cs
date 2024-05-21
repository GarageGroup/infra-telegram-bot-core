using System;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatCommandParser<TIn>
{
    Optional<TIn> Parse(ChatUpdate update);
}