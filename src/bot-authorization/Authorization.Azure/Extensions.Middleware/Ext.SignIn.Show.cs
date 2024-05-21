using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GarageGroup.Infra.Telegram.Bot;

using static AuthorizationResource;

partial class MiddlewareExtensions
{
    internal static async ValueTask<TurnResult> ShowSignInButtonAsync(
        this IChatContext context,
        AuthorizationOption option,
        AuthorizationState authorizationState,
        CancellationToken cancellationToken)
    {
        var localizer = context.GetLocalizer(BaseName);

        var callbackState = context.State.GetValue<CallbackState>() with
        {
            State = GenerateCallbackState()
        };

        context.State.SetValue(callbackState);

        var request = new ChatMessageSendRequest(
            text: localizer.GetValue(SignInTextMessageName, SignInTextMessageDefault))
        {
            ReplyMarkup = new BotInlineKeyboardMarkup
            {
                InlineKeyboard =
                [
                    [
                        new(localizer.GetValue(SignInButtonMessageName, SignInButtonMessageDefault))
                        {
                            Url = option.BuildAuthrorizationUrl(context.Update.Chat.Id, callbackState.State)
                        }
                    ]
                ]
            }
        };

        var message = await context.Api.SendMessageAsync(request, cancellationToken).ConfigureAwait(false);

        var nextAuthorizationState = authorizationState with
        {
            SignInMessageId = message.MessageId
        };

        context.State.SetValue(nextAuthorizationState);
        return context.CreateTurnResult(TurnState.Waiting);
    }

    private static string GenerateCallbackState()
    {
        using var rng = RandomNumberGenerator.Create();

        var randomNumberByteArr = new byte[16];
        rng.GetBytes(randomNumberByteArr);

        return Convert.ToBase64String(randomNumberByteArr);
    }

    private static string BuildAuthrorizationUrl(this AuthorizationOption option, long chatId, string state)
        =>
        new StringBuilder()
        .AppendFormat(
            "https://login.microsoftonline.com/{0}/oauth2/v2.0/authorize", option.TenantId)
        .AppendFormat(
            "?client_id={0}", option.ClientId)
        .Append(
            "&response_type=code")
        .AppendFormat(
            "&redirect_uri={0}", HttpUtility.UrlEncode(option.RedirectUri))
        .Append(
            "&response_mode=form_post")
        .AppendFormat(
            "&scope={0}", AuthorizationOption.Scope)
        .AppendFormat(
            "&state={0}.{1}", chatId, HttpUtility.UrlEncode(state))
        .ToString();
}