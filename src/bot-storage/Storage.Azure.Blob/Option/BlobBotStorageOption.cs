using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BlobBotStorageOption
{
    public BlobBotStorageOption(string accountKey, string accountName, string containerName)
    {
        AccountKey = accountKey.OrEmpty();
        AccountName = accountName.OrEmpty();
        ContainerName = containerName.OrEmpty();
    }

    public string AccountKey { get; }

    public string AccountName { get; }

    public string ContainerName { get; }
}