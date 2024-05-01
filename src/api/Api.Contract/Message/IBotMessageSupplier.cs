using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IBotMessageSupplier
{
    Task<BotMessage> SendMessageAsync(BotMessageSendRequest request, CancellationToken cancellationToken);

    ValueTask<Result<BotMessage, Failure<TelegramBotFailureCode>>> SendMessageOrFailureAsync(
        BotMessageSendRequest request, CancellationToken cancellationToken);

    Task<bool> EditTextMessageAsync(BotMessageTextEditRequest request, CancellationToken cancellationToken);

    ValueTask<Result<bool, Failure<TelegramBotFailureCode>>> EditTextMessageOrFailureAsync(
        BotMessageTextEditRequest request, CancellationToken cancellationToken);

    Task<bool> DeleteMessageAsync(BotMessageDeleteRequest request, CancellationToken cancellationToken);

    ValueTask<Result<bool, Failure<TelegramBotFailureCode>>> DeleteMessageOrFailureAsync(
        BotMessageDeleteRequest request, CancellationToken cancellationToken);

    Task<bool> DeleteMessageSetAsync(BotMessageSetDeleteRequest request, CancellationToken cancellationToken);

    ValueTask<Result<bool, Failure<TelegramBotFailureCode>>> DeleteMessageSetOrFailureAsync(
        BotMessageSetDeleteRequest request, CancellationToken cancellationToken);
}