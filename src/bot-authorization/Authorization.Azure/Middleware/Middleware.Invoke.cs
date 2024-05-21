using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

using static AuthorizationResource;

partial class AuthorizationMiddleware
{
    public ValueTask<TurnResult> InvokeAsync(IChatContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Update.Chat.Type is not BotChatType.Private)
        {
            return new(context.CreateTurnResult(TurnState.Complete));
        }

        if (context.Update.MyChatMember?.NewChatMember is BotBannedChatMember)
        {
            return context.SignOutAsync(authorizationApi, cancellationToken);
        }

        if (context.Update.IsMessageUpdate() is false)
        {
            return new(context.CreateTurnResult(TurnState.Waiting));
        }

        var userState = context.State.GetValue<UserState>();
        if (userState.User is null)
        {
            return SignInAsync(context, cancellationToken);
        }

        if (userState.User.Identity is null)
        {
            return SignInAsync(context.WithUser(userState.User), cancellationToken);
        }

        if (DateTimeOffset.UtcNow > userState.ExpirationTime)
        {
            var user = userState.User with
            {
                Identity = null
            };

            return SignInAsync(context.WithUser(user), cancellationToken);
        }

        return new(context.WithUser(userState.User).CreateTurnResult(TurnState.Complete));
    }

    private async ValueTask<TurnResult> SignInAsync(IChatContext context, CancellationToken cancellationToken)
    {
        var authorizationState = context.State.GetValue<AuthorizationState>(AuthorizationState.Key);
        if (authorizationState.SignInMessageId is not null)
        {
            return await context.AuthorizeAsync(authorizationApi, authorizationState, cancellationToken).ConfigureAwait(false);
        }

        if (authorizationState.LanguageChoiceMessageId is not null)
        {
            return await context.ApplyCultureAsync(option, authorizationState, cancellationToken).ConfigureAwait(false);
        }

        var bot = await context.Api.GetMeAsync(cancellationToken).ConfigureAwait(false);

        var chatUserGetInput = new ChatUserGetIn(botId: bot.Id, chatId: context.Update.Chat.Id);
        var chatUserResult = await authorizationApi.GetChatUserAsync(chatUserGetInput, cancellationToken).ConfigureAwait(false);

        var chatUser = chatUserResult.SuccessOrThrow(ToException);
        if (chatUser.User is not null)
        {
            context = context.WithUser(chatUser.User with
            {
                Identity = null
            });
        }

        var localizer = context.GetLocalizer(BaseName);
        if (chatUser.IsDisabled)
        {
            var disabledMessage = localizer.GetValue(UserDisabledMessageName, UserDisabledMessageDefault);
            _ = await context.Api.SendHtmlModeTextAndRemoveReplyKeyboardAsync(disabledMessage, cancellationToken).ConfigureAwait(false);

            return context.CreateTurnResult(TurnState.Cancelled);
        }

        if (chatUser.User is not null && chatUser.IsSignedOut is false)
        {
            var nextContext = context.WithUser(chatUser.User);

            var userState = new UserState
            {
                User = chatUser.User,
                ExpirationTime = DateTimeOffset.UtcNow.AddMinutes(UserState.TtlInMinutes)
            };

            nextContext.State.SetValue(userState);
            return nextContext.CreateTurnResult(TurnState.Complete);
        }

        var callbackState = new CallbackState
        {
            SourceUpdate = context.Update,
            BotUrl = $"https://t.me/{bot.Username}"
        };
        context.State.SetValue(callbackState);

        var state = new AuthorizationState
        {
            BotId = bot.Id,
            BotName = bot.Username
        };
        context.State.SetValue(state);

        return await context.ShowCultureChoiceAsync(option, state, cancellationToken).ConfigureAwait(false);
    }
}