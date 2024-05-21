using System;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

namespace GarageGroup.Infra.Telegram.Bot;

public static class BlobBotStorageDependency
{
    private const string DefaultSectionName = "BlobBotStorage";

    public static Dependency<IBlobBotStorage> UseBlobBotStorage(this Dependency<IHttpApi, BlobBotStorageOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<IBlobBotStorage>(CreateStorage);

        static BlobBotStorage CreateStorage(IHttpApi httpApi, BlobBotStorageOption option)
        {
            ArgumentNullException.ThrowIfNull(httpApi);
            ArgumentNullException.ThrowIfNull(option);

            return new(httpApi, UtcProvider.Instance, option);
        }
    }

    public static Dependency<IBlobBotStorage> UseBlobBotStorage(this Dependency<IHttpApi> dependency, string sectionName = DefaultSectionName)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<IBlobBotStorage>(CreateStorage);

        BlobBotStorage CreateStorage(IServiceProvider serviceProvider, IHttpApi httpApi)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(httpApi);

            var section = serviceProvider.GetServiceOrThrow<IConfiguration>().GetRequiredSection(sectionName.OrEmpty());

            return new(
                httpApi: httpApi,
                utcProvider: UtcProvider.Instance,
                option: new(
                    accountKey: section["AccountKey"].OrEmpty(),
                    accountName: section["AccountName"].OrEmpty(),
                    containerName: section["ContainerName"].OrEmpty()));
        }
    }
}