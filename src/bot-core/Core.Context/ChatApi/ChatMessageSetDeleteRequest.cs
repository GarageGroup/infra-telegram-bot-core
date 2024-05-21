using System;

namespace GarageGroup.Infra.Telegram.Bot;

public readonly record struct ChatMessageSetDeleteRequest
{
    public required FlatArray<int> MessageIds { get; init; }
}