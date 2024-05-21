using System;
using System.Text;
using System.Text.Json.Serialization;
using System.Web;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed partial class CallbackFunc : ICallbackFunc
{
    private const string AccessTokenHttpError
        =
        "An unexpected http failure occured when trying to get access token:";

    private readonly IHttpApi httpApi;

    private readonly IBotStorage botStorage;

    private readonly IHandler<ChatUpdate, Unit> botHandler;

    private readonly AuthorizationOption option;

    internal CallbackFunc(IHttpApi httpApi, IBotStorage botStorage, IHandler<ChatUpdate, Unit> botHandler, AuthorizationOption option)
    {
        this.httpApi = httpApi;
        this.botStorage = botStorage;
        this.botHandler = botHandler;
        this.option = option;
    }

    private string BuildHttpBodyText(string code)
        =>
        new StringBuilder()
        .AppendFormat(
            "client_id={0}", option.ClientId)
        .AppendFormat(
            "&client_secret={0}", HttpUtility.UrlEncode(option.ClientSecret))
        .AppendFormat(
            "&scope={0}", AuthorizationOption.Scope)
        .AppendFormat(
            "&code={0}", HttpUtility.UrlEncode(code))
        .AppendFormat(
            "&redirect_uri={0}", HttpUtility.UrlEncode(option.RedirectUri))
        .Append(
            "&grant_type=authorization_code")
        .ToString();

    private sealed record class AuthorizationData(long ChatId, string State, string Code);

    private readonly record struct TokenJson
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; init; }
    }
}