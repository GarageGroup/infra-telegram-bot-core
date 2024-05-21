using System;
using PrimeFuncPack;

namespace GarageGroup.Infra.Telegram.Bot;

public static class UserAuthorizationApiDependency
{
    public static Dependency<IUserAuthorizationApi> UseUserAuthorizationApi<TDataverseApi>(
        this Dependency<IHttpApi, TDataverseApi> dependency)
        where TDataverseApi : IDataverseEntityGetSupplier, IDataverseEntityUpdateSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<IUserAuthorizationApi>(CreateApi);

        static UserAuthorizationApi CreateApi(IHttpApi httpApi, TDataverseApi dataverseApi)
        {
            ArgumentNullException.ThrowIfNull(httpApi);
            ArgumentNullException.ThrowIfNull(dataverseApi);

            return new(httpApi, dataverseApi, dataverseApi);
        }
    }
}