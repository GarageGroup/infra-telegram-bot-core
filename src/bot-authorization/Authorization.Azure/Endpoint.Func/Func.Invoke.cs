using System;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class CallbackFunc
{
    public ValueTask<Result<CallbackOut, Failure<CallbackFailureCode>>> InvokeAsync(
        CallbackIn input, CancellationToken cancellationToken)
    {
        return ValidateInput(input).Forward(ParseAuthorizationData).ForwardValueAsync(InnerGetAccessTokenAsync);

        ValueTask<Result<CallbackOut, Failure<CallbackFailureCode>>> InnerGetAccessTokenAsync(AuthorizationData data)
            =>
            GetAccessTokenAsync(data, cancellationToken);
    }

    private static Result<CallbackIn, Failure<CallbackFailureCode>> ValidateInput(CallbackIn input)
    {
        if (string.IsNullOrEmpty(input.Error) is false)
        {
            var failureMessage = $"Authorization error: {input.Error}. Description: {input.ErrorDescription}";
            return Failure.Create(CallbackFailureCode.AuthorizationError, failureMessage);
        }

        if (string.IsNullOrWhiteSpace(input.Code))
        {
            return Failure.Create(CallbackFailureCode.AuthorizationError, "Authorization code must be specified");
        }

        if (string.IsNullOrWhiteSpace(input.State))
        {
            return Failure.Create(CallbackFailureCode.AuthorizationError, "Authorization state must be specified");
        }

        return Result.Success(input);
    }

    private static Result<AuthorizationData, Failure<CallbackFailureCode>> ParseAuthorizationData(CallbackIn input)
    {
        var index = input.State.IndexOf('.');
        if (index > 0 && index < input.State.Length - 1)
        {
            var chatIdText = input.State[..index];
            if (long.TryParse(chatIdText, out var chatId))
            {
                var state = input.State[(index + 1)..];
                return new AuthorizationData(chatId, state, input.Code);
            }
        }

        return Failure.Create(CallbackFailureCode.InvalidState, $"Authorization state '{input.State}' is invalid");
    }

    private async ValueTask<Result<CallbackOut, Failure<CallbackFailureCode>>> GetAccessTokenAsync(
        AuthorizationData data, CancellationToken cancellationToken)
    {
        var chatState = await botStorage.GetChatStateAsync(data.ChatId, cancellationToken).ConfigureAwait(false);
        if (chatState is null)
        {
            return Failure.Create(CallbackFailureCode.IncorrectState, $"ChatState was not found by id {data.ChatId}");
        }

        var callbackState = chatState.GetValue<CallbackState>();
        if (string.IsNullOrEmpty(callbackState.State))
        {
            return Failure.Create(CallbackFailureCode.IncorrectState, "CallbackState is absent");
        }

        var actualState = callbackState.State;
        if (string.Equals(data.State, actualState, StringComparison.InvariantCulture) is false)
        {
            return Failure.Create(CallbackFailureCode.IncorrectState, $"Input state '{data.State}' does not match chat state '{actualState}'");
        }

        var httpInput = new HttpSendIn(HttpVerb.Post, $"https://login.microsoftonline.com/{option.TenantId}/oauth2/v2.0/token")
        {
            Body = new HttpBody
            {
                Type = new(MediaTypeNames.Application.FormUrlEncoded),
                Content = new(BuildHttpBodyText(data.Code))
            }
        };

        var httpResult = await httpApi.SendAsync(httpInput, cancellationToken).ConfigureAwait(false);
        if (httpResult.IsFailure)
        {
            return httpResult.FailureOrThrow().ToStandardFailure(AccessTokenHttpError).WithFailureCode(CallbackFailureCode.Unknown);
        }

        var resultState = new ResultState(httpResult.SuccessOrThrow().Body.DeserializeFromJson<TokenJson>().AccessToken.OrEmpty());

        chatState.SetValue(resultState);
        await botStorage.SaveChatStateAsync(data.ChatId, chatState, cancellationToken).ConfigureAwait(false);

        var handlerResult = await botHandler.HandleAsync(callbackState.SourceUpdate, cancellationToken).ConfigureAwait(false);
        if (handlerResult.IsFailure)
        {
            return handlerResult.FailureOrThrow().WithFailureCode(CallbackFailureCode.Unknown);
        }

        return new CallbackOut(
            botUrl: callbackState.BotUrl.OrEmpty());
    }
}