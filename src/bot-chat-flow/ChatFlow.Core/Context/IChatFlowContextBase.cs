using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatFlowContextBase
{
    string ChatFlowId { get; }

    ChatFlowStepState StepState { get; }

    ChatUpdate Update { get; }

    IChatApi Api { get; }

    ChatUser User { get; }

    ChatWebApp? WebApp { get; }

    IStringLocalizer Localizer { get; }

    ILogger Logger { get; }
}