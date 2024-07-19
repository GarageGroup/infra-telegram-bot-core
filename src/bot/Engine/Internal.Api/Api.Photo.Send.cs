using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

partial class ChatApi
{
    public async Task<BotMessage> SendPhotoAsync(ChatPhotoSendRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var @in = new BotPhotoSendIn(chatId: chatId, photo: request.Photo)
        {
            BusinessConnectionId = request.BusinessConnectionId,
            MessageThreadId = request.MessageThreadId,
            Caption = request.Caption,
            ParseMode = request.ParseMode,
            CaptionEntities = request.CaptionEntities,
            ShowCaptionAboveMedia = request.ShowCaptionAboveMedia,
            HasSpoiler = request.HasSpoiler,
            DisableNotification = request.DisableNotification,
            ProtectContent = request.ProtectContent,
            MessageEffectId = request.MessageEffectId,
            ReplyParameters = request.ReplyParameters,
            ReplyMarkup = request.ReplyMarkup
        };

        var result = await botApi.SendPhotoAsync(@in, cancellationToken).ConfigureAwait(false);
        return result.SuccessOrThrow(ToException);
    }
}