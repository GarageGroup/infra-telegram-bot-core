using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Telegram.Bot;

using static AuthorizationResource;

partial class MiddlewareExtensions
{
    internal static async ValueTask<TurnResult> AuthorizeAsync(
        this IChatContext context,
        IUserAuthorizeSupplier authorizationApi,
        AuthorizationState authorizationState,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultState = context.State.GetValue<ResultState>();
            if (string.IsNullOrEmpty(resultState?.AccessToken))
            {
                return context.CreateTurnResult(TurnState.Waiting);
            }

            var callbackState = context.State.GetValue<CallbackState>();
            if (callbackState.SourceUpdate != context.Update)
            {
                return context.CreateTurnResult(TurnState.Waiting);
            }

            if (string.IsNullOrWhiteSpace(authorizationState.ChosenLanguageCode) is false)
            {
                context = context.WithUser(context.User with
                {
                    Culture = new(authorizationState.ChosenLanguageCode)
                });
            }

            var @in = new UserAuthorizeIn(
                botId: authorizationState.BotId,
                botName: authorizationState.BotName.OrEmpty(),
                chatId: context.Update.Chat.Id,
                accessToken: resultState.AccessToken)
            {
                LanguageCode = authorizationState.ChosenLanguageCode
            };

            var result = await authorizationApi.AuthorizeAsync(@in, cancellationToken).ConfigureAwait(false);
            var localizer = context.GetLocalizer(BaseName);

            if (result.IsFailure)
            {
                var failure = result.FailureOrThrow();
                var logger = context.GetLogger<AuthorizationMiddleware>();

                logger.LogError(failure.SourceException, "SignIn error: {failureMessage}", failure.FailureMessage);

                var errorText = localizer.GetValue(AuthorizationErrorMessageName, AuthorizationErrorMessageDefault);
                await context.Api.SendHtmlModeTextAsync(errorText, cancellationToken).ConfigureAwait(false);

                return context.CreateTurnResult(TurnState.Waiting);
            }

            var user = result.SuccessOrThrow();
            if (user.IsDisabled)
            {
                var disabledMessage = localizer.GetValue(UserDisabledMessageName, UserDisabledMessageDefault);
                await SendTextAndClearStateAsync(disabledMessage).ConfigureAwait(false);

                return context.CreateTurnResult(TurnState.Cancelled);
            }

            if (user.User?.Identity is null)
            {
                var notFoundText = localizer.GetValue(UserNotFoundMessageName, UserNotFoundMessageDefault);
                await SendTextAndClearStateAsync(notFoundText).ConfigureAwait(false);

                return context.CreateTurnResult(TurnState.Cancelled);
            }

            context = context.WithUser(user.User);
            var userName = user.User.Identity.Name.OrNullIfWhiteSpace() ?? context.Update.User.Username;

            var template = localizer.GetValue(AuthorizationSuccessTemplateName, AuthorizationSuccessTemplateDefault);
            var successText = string.Format(context.User.Culture, template, HttpUtility.HtmlEncode(userName));

            await SendTextAndClearStateAsync(successText).ConfigureAwait(false);

            var userState = new UserState
            {
                User = user.User,
                ExpirationTime = DateTimeOffset.UtcNow.AddMinutes(UserState.TtlInMinutes)
            };
            context.State.SetValue(userState);

            return context.CreateTurnResult(TurnState.Complete);
        }
        catch (Exception ex)
        {
            context.GetLogger<AuthorizationMiddleware>().LogError(ex, "An unexpected authorization error");

            var localizer = context.GetLocalizer(BaseName);
            var notFoundText = localizer.GetValue(UnexpectedErrorMessageName, UnexpectedErrorMessageDefault);

            await SendTextAndClearStateAsync(notFoundText).ConfigureAwait(false);
            return context.CreateTurnResult(TurnState.Cancelled);
        }

        Task SendTextAndClearStateAsync(string text)
        {
            context.State.RemoveValue<AuthorizationState>();
            context.State.RemoveValue<CallbackState>();
            context.State.RemoveValue<ResultState>();

            return Task.WhenAll(
                context.Api.SendHtmlModeTextAndRemoveReplyKeyboardAsync(text, cancellationToken),
                context.Api.DeleteMessageAsync(authorizationState.SignInMessageId.GetValueOrDefault(), cancellationToken));
        }
    }
}