using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

using static AuthorizationResource;

partial class MiddlewareExtensions
{
    internal static async ValueTask<TurnResult> ApplyCultureAsync(
        this IChatContext context,
        AuthorizationOption option,
        AuthorizationState state,
        CancellationToken cancellationToken)
    {
        var culture = context.Update switch
        {
            { CallbackQuery: not null } => FindCulture(context.Update.CallbackQuery.Data),
            { Message: not null } => FindCulture(context.Update.Message.Text),
            _ => null
        };

        if (culture is null)
        {
            return context.CreateTurnResult(TurnState.Waiting);
        }

        var user = context.User with
        {
            Culture = culture
        };

        context = context.WithUser(user);

        var localizer = context.GetLocalizer(BaseName);
        var text = localizer.GetValue(ChosenLanguageMessageName, ChosenLanguageMessageDefault);

        Task[] tasks =
        [
            context.Api.SendHtmlModeTextAndRemoveReplyKeyboardAsync($"<b>{text}</b>: {culture.NativeName.ToLower()}", cancellationToken),
            context.Api.DeleteMessageAsync(state.LanguageChoiceMessageId.GetValueOrDefault(), cancellationToken)
        ];

        await Task.WhenAll(tasks).ConfigureAwait(false);

        var nextState = state with
        {
            LanguageCodes = default,
            LanguageChoiceMessageId = null,
            ChosenLanguageCode = culture.IetfLanguageTag
        };
        context.State.SetValue(nextState);

        return await context.ShowSignInButtonAsync(option, nextState, cancellationToken).ConfigureAwait(false);

        CultureInfo? FindCulture(string? languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                return null;
            }

            foreach (var language in state.LanguageCodes)
            {
                if (string.Equals(language, languageCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    return new(language);
                }
            }

            return null;
        }
    }
}