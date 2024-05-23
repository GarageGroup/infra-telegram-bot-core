using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatApi
{
    public async Task<ChatFileLink> GetFileLinkAsync(string fileId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileId);

        var @in = new BotFileGetIn(fileId);
        var result = await botApi.GetFileAsync(@in, cancellationToken).ConfigureAwait(false);

        var file = result.SuccessOrThrow(ToException);
        if (string.IsNullOrWhiteSpace(file.FilePath))
        {
            throw new InvalidOperationException("File path was not specified in the response");
        }

        return new(
            fileId: file.FileId,
            fileUniqueId: file.FileUniqueId,
            filePath: file.FilePath,
            fileUrl: string.Format(fileUrlTemplate, file.FilePath));
    }
}