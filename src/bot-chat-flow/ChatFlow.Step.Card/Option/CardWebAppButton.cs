using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed class CardWebAppButton<T>(string buttonName, string webAppUrl, Func<BotWebAppData, Result<T, ChatRepeatState>> forward)
{
    public string WebAppUrl { get; } = webAppUrl.OrEmpty();

    public string ButtonName { get; } = buttonName.OrNullIfWhiteSpace() ?? "Edit";

    public Result<T, ChatRepeatState> Forward(BotWebAppData webAppData)
        =>
        forward.Invoke(webAppData);
}