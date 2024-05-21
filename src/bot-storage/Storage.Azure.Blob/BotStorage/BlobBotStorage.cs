using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed partial class BlobBotStorage(IHttpApi httpApi, IUtcProvider utcProvider, BlobBotStorageOption option) : IBlobBotStorage
{
    private const char BlobResource = 'b';

    private const char PermissionsRead = 'r';

    private const char PermissionsUpload = 'w';

    private const string Protocol = "https";

    private const string SasVersion = "2022-11-02";

    private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";

    private const string BlobTypeHeaderName = "x-ms-blob-type";

    private const string BlobTypeHeaderValue = "BlockBlob";

    private const int TokenTtlInSeconds = 60;

    private static readonly JsonSerializerOptions SerializerOptions
        =
        new(JsonSerializerDefaults.Web);

    private readonly HMACSHA256 hashAlgorithm
        =
        new(Convert.FromBase64String(option.AccountKey));

    private string BuildFileUrl(long chatId, Role role)
    {
        var permissions = role is Role.Reader ? PermissionsRead : PermissionsUpload;
        var expiryTime = utcProvider.UtcNow.AddSeconds(TokenTtlInSeconds).ToString(DateTimeFormat);

        var fileName = $"{chatId}.json";
        string[] signParameters =
        [
            permissions.ToString(),
            string.Empty,
            expiryTime,
            $"/blob/{option.AccountName}/{option.ContainerName}/{fileName}",
            string.Empty,
            string.Empty,
            Protocol,
            SasVersion,
            BlobResource.ToString(),
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        ];

        var dataToSign = Encoding.UTF8.GetBytes(string.Join('\n', signParameters));
        var signature = Convert.ToBase64String(hashAlgorithm.ComputeHash(dataToSign));

        return new StringBuilder()
            .Append("https://").Append(option.AccountName).Append(".blob.core.windows.net/")
            .Append(option.ContainerName).Append('/').Append(fileName)
            .Append("?sv=").Append(SasVersion)
            .Append("&spr=").Append(Protocol)
            .Append("&se=").Append(expiryTime)
            .Append("&sr=").Append(BlobResource)
            .Append("&sp=").Append(permissions)
            .Append("&sig=").Append(Uri.EscapeDataString(signature))
            .ToString();
    }

    private enum Role
    {
        Reader,

        Writer
    }
}