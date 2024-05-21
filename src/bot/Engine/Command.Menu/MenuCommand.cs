using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class MenuCommand : IChatCommand<MenuCommandIn, Unit>, IChatCommandParser<MenuCommandIn>
{
    internal static readonly MenuCommand Instance
        =
        new();

    public Optional<MenuCommandIn> Parse(ChatUpdate update)
    {
        if (update.Message is not null || update.CallbackQuery is not null)
        {
            return new MenuCommandIn();
        }

        return default;
    }

    private const string LineBreak = "\n\r";

    private MenuCommand()
    {
    }

    public async ValueTask<ChatCommandResult<Unit>> SendAsync(
        ChatCommandRequest<MenuCommandIn, Unit> request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var text = string.Join(LineBreak, GetTextLines(request));
        _ = await request.Context.Api.SendHtmlModeTextAndRemoveReplyKeyboardAsync(text, cancellationToken).ConfigureAwait(false);

        return request.Context.CreateCompleteResult<Unit>(default);
    }

    private static IEnumerable<string> GetTextLines(ChatCommandRequest<MenuCommandIn, Unit> request)
    {
        var localizer = request.Context.GetLocalizer(BotCoreResource.BaseName);

        var header = localizer[BotCoreResource.MenuHeaderMessageName];
        var headerText = header.ResourceNotFound ? BotCoreResource.MenuHeaderMessageDefault : HttpUtility.HtmlEncode(header.Value);

        if (request.Context is not ChatContext context)
        {
            yield return headerText;
            yield break;
        }

        yield return $"<b>{headerText}</b>";

        foreach (var nameType in context.AllCommandTypes)
        {
            if (string.IsNullOrWhiteSpace(nameType.Key) || string.IsNullOrWhiteSpace(nameType.Value))
            {
                continue;
            }

            var description = localizer[$"{nameType.Value}MenuDescription"];
            if (description.ResourceNotFound)
            {
                continue;
            }

            var descriptionText = description.Value;
            if (string.IsNullOrWhiteSpace(descriptionText))
            {
                continue;
            }

            var encodedName = HttpUtility.HtmlEncode(nameType.Key);
            var encodedDescription = HttpUtility.HtmlEncode(descriptionText);

            yield return $"/{encodedName} - {encodedDescription}";
        }
    }
}