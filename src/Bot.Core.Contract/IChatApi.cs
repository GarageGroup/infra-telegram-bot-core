using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatApi
{
    Task<BotUser> GetMeAsync(CancellationToken cancellationToken)
        =>
        GetMeAsync(default, cancellationToken);

    Task<BotUser> GetMeAsync(Unit request, CancellationToken cancellationToken);

    Task<BotMessage> SendTextAsync(string text, CancellationToken cancellationToken)
        =>
        SendTextAsync(new BotTextSendRequest(text), cancellationToken);

    Task<BotMessage> SendTextAsync(BotTextSendRequest request, CancellationToken cancellationToken);

    Task<BotMessage> ForwardMessageAsync(BotMessageForwardRequest request, CancellationToken cancellationToken);
}