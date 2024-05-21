using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

[Endpoint(EndpointMethod.Post, "/oauth2/callback")]
[EndpointTag("Bot Authorization")]
internal interface ICallbackFunc
{
    ValueTask<Result<CallbackOut, Failure<CallbackFailureCode>>> InvokeAsync(CallbackIn input, CancellationToken cancellationToken);
}