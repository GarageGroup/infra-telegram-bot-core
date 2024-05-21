using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;

namespace GarageGroup.Infra.Telegram.Bot;

public static class AuthorizationDependency
{
    private const string DefaultSectionName = "Bot:Authorization";

    public static Dependency<IChatMiddleware> UseAuthorizationMiddleware(
        this Dependency<IUserAuthorizationApi, AuthorizationOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<IChatMiddleware>(CreateMiddleware);

        static AuthorizationMiddleware CreateMiddleware(IUserAuthorizationApi authorizationApi, AuthorizationOption option)
        {
            ArgumentNullException.ThrowIfNull(authorizationApi);
            ArgumentNullException.ThrowIfNull(option);

            return new(authorizationApi, option);
        }
    }

    public static Dependency<IChatMiddleware> UseAuthorizationMiddleware(
        this Dependency<IUserAuthorizationApi> dependency, string sectionName = DefaultSectionName)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<IChatMiddleware>(CreateMiddleware);

        AuthorizationMiddleware CreateMiddleware(IServiceProvider serviceProvider, IUserAuthorizationApi authorizationApi)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(authorizationApi);

            return new(authorizationApi, serviceProvider.GetAuthorizationOption(sectionName.OrEmpty()));
        }
    }

    public static Dependency<CallbackEndpoint> UseAuthorizationCallbackEndpoint<THandler>(
        this Dependency<IHttpApi, IBotStorage, THandler, AuthorizationOption> dependency)
        where THandler : IHandler<ChatUpdate, Unit>
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(CallbackEndpoint.Resolve);

        static CallbackFunc CreateFunc(IHttpApi httpApi, IBotStorage botStorage, THandler botHandler, AuthorizationOption option)
        {
            ArgumentNullException.ThrowIfNull(httpApi);
            ArgumentNullException.ThrowIfNull(botStorage);
            ArgumentNullException.ThrowIfNull(botHandler);
            ArgumentNullException.ThrowIfNull(option);

            return new(httpApi, botStorage, botHandler, option);
        }
    }

    public static Dependency<CallbackEndpoint> UseAuthorizationCallbackEndpoint<THandler>(
        this Dependency<IHttpApi, IBotStorage, THandler> dependency, string sectionName = DefaultSectionName)
        where THandler : IHandler<ChatUpdate, Unit>
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold(CreateFunc).Map(CallbackEndpoint.Resolve);

        CallbackFunc CreateFunc(IServiceProvider serviceProvider, IHttpApi httpApi, IBotStorage botStorage, THandler botHandler)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(httpApi);
            ArgumentNullException.ThrowIfNull(botStorage);
            ArgumentNullException.ThrowIfNull(botHandler);

            return new(httpApi, botStorage, botHandler, serviceProvider.GetAuthorizationOption(sectionName.OrEmpty()));
        }
    }

    public static Dependency<IChatCommand<SignOutCommandIn, Unit>> UseSignOutCommand<TUserAuthorizationApi>(
        this Dependency<TUserAuthorizationApi> dependency)
        where TUserAuthorizationApi : IUserUnauthorizeSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<IChatCommand<SignOutCommandIn, Unit>>(CreateCommand);

        static SignOutCommand CreateCommand(TUserAuthorizationApi authorizationApi)
        {
            ArgumentNullException.ThrowIfNull(authorizationApi);
            return new(authorizationApi);
        }
    }

    private static AuthorizationOption GetAuthorizationOption(this IServiceProvider serviceProvider, string sectionName)
    {
        var section = serviceProvider.GetRequiredService<IConfiguration>().GetRequiredSection(sectionName);

        return new(
            tenantId: section["TenantId"].OrEmpty(),
            clientId: section["ClientId"].OrEmpty(),
            clientSecret: section["ClientSecret"].OrEmpty(),
            redirectUri: section["RedirectUri"].OrEmpty());
    }
}