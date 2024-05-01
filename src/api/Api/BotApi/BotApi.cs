using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed partial class BotApi(IHttpApi httpApi, BotApiOption option) : IBotApi
{
    private const string TelegramBaseUrl = "https://api.telegram.org";

    private sealed record class BotRequest<TRequest>(HttpVerb HttpMethod, string BotMethod, TRequest? Request);

    private async ValueTask<Result<TResult, Failure<TelegramBotFailureCode>>> InnerSendOrFailureAsync<TRequest, TResult>(
        BotRequest<TRequest> request, CancellationToken cancellationToken)
    {
        var @in = new HttpSendIn(request.HttpMethod, $"{TelegramBaseUrl}/bot{option.Token}/{request.BotMethod}")
        {
            Body = request.Request is null ? default : HttpBody.SerializeAsJson(request.Request, BotDefaultJson.SerializerOptions)
        };

        var result = await httpApi.SendAsync(@in, cancellationToken).ConfigureAwait(false);
        return result.Fold(FromSuccess, FromFailure);

        static Result<TResult, Failure<TelegramBotFailureCode>> FromSuccess(HttpSendOut success)
            =>
            ParseResponseOrFailure(success.Body);

        static Result<TResult, Failure<TelegramBotFailureCode>> FromFailure(HttpSendFailure failure)
        {
            if (failure.Body.Type.IsJsonMediaType(false) is false)
            {
                return failure.ToStandardFailure("An unexpected Telegram bot http failure occured:").WithFailureCode(TelegramBotFailureCode.Unknown);
            }

            return ParseResponseOrFailure(failure.Body);
        }

        static Result<TResult, Failure<TelegramBotFailureCode>> ParseResponseOrFailure(HttpBody body)
        {
            var response = body.DeserializeFromJson<ResponseJson<TResult>>(BotDefaultJson.SerializerOptions);

            if (response.Ok && response.Result is not null)
            {
                return response.Result;
            }

            if (string.IsNullOrWhiteSpace(response.Description))
            {
                return Failure.Create(response.ErrorCode.GetValueOrDefault(), $"An unexpected Telegram bot response: {body.Content}");
            }

            if (response.Parameters?.ExtensionData?.Count is not > 0)
            {
                return Failure.Create(response.ErrorCode.GetValueOrDefault(), response.Description);
            }

            var messageBuilder = new StringBuilder(response.Description).Append('.');
            foreach (var parameter in response.Parameters.ExtensionData)
            {
                _ = messageBuilder.Append(' ').Append(parameter.Key).Append(": ").Append(parameter.Value.ToString());
            }

            return Failure.Create(response.ErrorCode.GetValueOrDefault(), messageBuilder.ToString());
        }
    }

    private static Failure<TelegramBotFailureCode>.Exception ToException(Failure<TelegramBotFailureCode> failure)
        =>
        failure.ToException();
}