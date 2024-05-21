using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class BotSignalHandler(IOrchestrationEntitySignalSupplier entityApi, string entityName, ILogger? logger) : IBotSignalHandler
{
    private const string EntityOperationName = "SendTelegramBotRequest";

    public async ValueTask<Result<Unit, Failure<HandlerFailureCode>>> HandleAsync(ChatUpdate? input, CancellationToken cancellationToken)
    {
        if (input is null)
        {
            logger?.LogError("Bot user chat update must be specified");
            return Result.Success<Unit>(default);
        }

        logger?.LogInformation("UserId: '{userId}'. ChatId: '{chatId}'", input.User.Id, input.Chat.Id);

        var result = await TrySignalEntityAsync(input, cancellationToken).ConfigureAwait(false);
        return result.Recover(ProcessFailure);

        Result<Unit, Failure<HandlerFailureCode>> ProcessFailure(Failure<HandlerFailureCode> failure)
        {
            logger?.LogError(failure.SourceException, "Signal failure: '{failureMessage}'", failure.FailureMessage);
            return Result.Success<Unit>(default);
        }
    }

    private async ValueTask<Result<Unit, Failure<HandlerFailureCode>>> TrySignalEntityAsync(ChatUpdate input, CancellationToken cancellationToken)
    {
        try
        {
            var @in = new OrchestrationEntitySignalIn<ChatUpdate>(
                entity: new(
                    name: entityName,
                    key: $"{input.User.Id}:{input.Chat.Id}"),
                operationName: EntityOperationName,
                value: input);

            return await entityApi.SignalEntityAsync(@in, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return ex.ToFailure(HandlerFailureCode.Transient, "An unexpected exception was thrown when trying to signal entity");
        }
    }
}