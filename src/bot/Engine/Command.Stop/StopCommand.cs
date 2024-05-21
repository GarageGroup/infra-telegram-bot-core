using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class StopCommand : IChatCommand<StopCommandIn, Unit>
{
    internal static readonly StopCommand Instance
        =
        new();

    private StopCommand()
    {
    }

    public async ValueTask<ChatCommandResult<Unit>> SendAsync(
        ChatCommandRequest<StopCommandIn, Unit> request, CancellationToken cancellationToken)
    {
        var message = request.Context.GetLocalizer(BotCoreResource.BaseName)[BotCoreResource.CommandStoppedMessageName];
        var text = message.ResourceNotFound ? BotCoreResource.CommandStoppedMessageDefault : HttpUtility.HtmlEncode(message.Value);

        _ = await request.Context.Api.SendHtmlModeTextAndRemoveReplyKeyboardAsync(text, cancellationToken).ConfigureAwait(false);
        return request.Context.CreateCancelledResult<Unit>();
    }
}