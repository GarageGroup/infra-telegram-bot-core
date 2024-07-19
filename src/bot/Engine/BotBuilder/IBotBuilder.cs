using PrimeFuncPack;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IBotBuilder
{
    Dependency<IBotWebHookHandler> BuildWebHookHandler();
}