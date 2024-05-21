using System;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IBotSignalHandler : IHandler<ChatUpdate, Unit>;