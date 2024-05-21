using System;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed partial class AuthorizationMiddleware : IChatMiddleware
{
    private readonly IUserAuthorizationApi authorizationApi;

    private readonly AuthorizationOption option;

    internal AuthorizationMiddleware(IUserAuthorizationApi authorizationApi, AuthorizationOption option)
    {
        this.authorizationApi = authorizationApi;
        this.option = option;
    }

    private static Failure<Unit>.Exception ToException(Failure<Unit> failure)
        =>
        failure.ToException();
}