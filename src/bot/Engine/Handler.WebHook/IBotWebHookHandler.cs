using System;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IBotWebHookHandler : IHandler<ChatUpdate, Unit>;