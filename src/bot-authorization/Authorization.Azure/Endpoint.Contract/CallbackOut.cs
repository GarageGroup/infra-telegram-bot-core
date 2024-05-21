using System;

namespace GarageGroup.Infra.Telegram.Bot;

[Success((SuccessStatusCode)302)]
internal sealed record class CallbackOut
{
    public CallbackOut(string botUrl)
        =>
        BotUrl = botUrl.OrEmpty();

    [HeaderOut("Location")]
    public string BotUrl { get; }
}