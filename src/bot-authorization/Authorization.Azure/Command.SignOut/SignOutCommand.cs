using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

using static AuthorizationResource;

internal sealed class SignOutCommand(IUserUnauthorizeSupplier authorizationApi) : IChatCommand<SignOutCommandIn, Unit>
{
    public async ValueTask<ChatCommandResult<Unit>> SendAsync(
        ChatCommandRequest<SignOutCommandIn, Unit> request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bot = await request.Context.Api.GetMeAsync(cancellationToken).ConfigureAwait(false);
        var @in = new UserUnauthorizeIn(botId: bot.Id, chatId: request.Context.Update.Chat.Id);

        var result = await authorizationApi.UnauthorizeAsync(@in, cancellationToken).ConfigureAwait(false);
        var next = await result.MapSuccessValueAsync(SendSuccessMessageAsync).ConfigureAwait(false);

        return next.SuccessOrThrow(ToException);


        async ValueTask<ChatCommandResult<Unit>> SendSuccessMessageAsync(Unit _)
        {
            var localizer = request.Context.GetLocalizer(BaseName);
            var text = localizer.GetValue(SignOutSuccessMessageName, SignOutSuccessMessageDefault);

            await request.Context.Api.SendHtmlModeTextAndRemoveReplyKeyboardAsync(text, cancellationToken).ConfigureAwait(false);
            return request.Context.CreateCompleteResult<Unit>(default);
        }

        static Failure<Unit>.Exception ToException(Failure<Unit> failure)
            =>
            failure.ToException();
    }
}