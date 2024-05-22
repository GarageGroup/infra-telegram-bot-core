using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class BotWebHookHandler(BotContext botContext, FlatArray<IChatMiddleware> middlewares) : IBotWebHookHandler
{
    private readonly IBotStorage botStorage = botContext.BotStorage;

    private readonly ILogger logger = botContext.GetLogger<BotWebHookHandler>();

    public async ValueTask<Result<Unit, Failure<HandlerFailureCode>>> HandleAsync(ChatUpdate? input, CancellationToken cancellationToken)
    {
        if (input is null)
        {
            logger.LogError("Bot user chat update must be specified");
            return Result.Success<Unit>(default);
        }

        if (middlewares.IsEmpty)
        {
            return Result.Success<Unit>(default);
        }

        var chatState = await TryInvokeMiddleware(input, cancellationToken).ConfigureAwait(false);
        await TrySaveStateAsync(input.Chat.Id, chatState, cancellationToken).ConfigureAwait(false);

        return Result.Success<Unit>(default);
    }

    private async ValueTask<ChatState?> TryInvokeMiddleware(ChatUpdate update, CancellationToken cancellationToken)
    {
        IChatContext? context = null;
        try
        {
            var initialState = await botStorage.GetChatStateAsync(update.Chat.Id, cancellationToken).ConfigureAwait(false);

            context = botContext.InitChatContext(update, initialState);
            await TryDeleteWebAppMessageAsync(context, cancellationToken).ConfigureAwait(false);

            foreach (var middleware in middlewares)
            {
                var result = await middleware.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
                if (result.State is TurnState.Complete)
                {
                    context = result.Context;
                    continue;
                }

                return result.State is TurnState.Waiting ? initialState : null;
            }

            return null;
        }
        catch (Exception ex)
        {
            context ??= botContext.InitChatContext(update, default);
            await TrySendErrorMessageAsync(context, cancellationToken).ConfigureAwait(false);

            logger.LogError(ex, "An unexpected exception was thrown when trying to handle bot update");
            return null;
        }
    }

    private async Task TrySaveStateAsync(long chatId, ChatState? state, CancellationToken cancellationToken)
    {
        try
        {
            await botStorage.SaveChatStateAsync(chatId, state ?? new(), cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected exception was thrown when trying to save bot state");
        }
    }

    private async Task TryDeleteWebAppMessageAsync(IChatContext context, CancellationToken cancellationToken)
    {
        if (context.Update.Message?.WebAppData is null)
        {
            return;
        }

        try
        {
            await context.Api.DeleteMessageAsync(context.Update.Message.MessageId, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected exception was thrown when trying to delete web app message");
        }
    }

    private async Task TrySendErrorMessageAsync(IChatContext context, CancellationToken cancellationToken)
    {
        try
        {
            var errorMessage = context.GetLocalizer(BotCoreResource.BaseName)[BotCoreResource.ErrorMessageName];
            var errorText = errorMessage.ResourceNotFound ? BotCoreResource.ErrorMessageDefault : HttpUtility.HtmlEncode(errorMessage);

            _ = await context.Api.SendHtmlModeTextAndRemoveReplyKeyboardAsync(errorText, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected exception was thrown when trying to send an error message");
        }
    }
}