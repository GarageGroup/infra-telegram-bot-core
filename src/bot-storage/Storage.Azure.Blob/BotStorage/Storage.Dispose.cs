namespace GarageGroup.Infra.Telegram.Bot;

partial class BlobBotStorage
{
    public void Dispose()
        =>
        hashAlgorithm.Dispose();
}