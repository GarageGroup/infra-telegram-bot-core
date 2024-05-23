using System;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed partial class ChatApi(IBotApi botApi, long chatId, string fileUrlTemplate) : IChatApi
{
    private static Failure<TelegramBotFailureCode>.Exception ToException(Failure<TelegramBotFailureCode> failure)
        =>
        failure.ToException();
}