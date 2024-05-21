using System;

namespace GarageGroup.Infra.Telegram.Bot;

internal interface IUtcProvider
{
    DateTime UtcNow { get; }
}