using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

using static AuthorizationResource;

partial class MiddlewareExtensions
{
    internal static async ValueTask<TurnResult> ShowCultureChoiceAsync(
        this IChatContext context,
        AuthorizationOption option,
        AuthorizationState state,
        CancellationToken cancellationToken)
    {
        var availableCultures = context.GetAvailableCultures();
        if (availableCultures.Length is not > 1)
        {
            return await context.ShowSignInButtonAsync(option, state, cancellationToken).ConfigureAwait(false);
        }

        var localizer = context.GetLocalizer(BaseName);
        var text = localizer.GetValue(ChooseLanguageMessageName, ChooseLanguageMessageDefault);

        var request = new ChatMessageSendRequest(text)
        {
            ReplyMarkup = new BotInlineKeyboardMarkup
            {
                InlineKeyboard = availableCultures.Map(CreateKeyboardRow)
            }
        };

        var message = await context.Api.SendMessageAsync(request, cancellationToken).ConfigureAwait(false);

        var nextState = state with
        {
            LanguageChoiceMessageId = message.MessageId,
            LanguageCodes = availableCultures.Map(GetIetfLanguageTag)
        };

        context.State.SetValue(nextState);
        return context.CreateTurnResult(TurnState.Waiting);

        static FlatArray<BotInlineKeyboardButton> CreateKeyboardRow(CultureInfo culture)
            =>
            [
                new($"{culture.TwoLetterISOLanguageName} - {culture.NativeName.ToLowerInvariant()}")
                {
                    CallbackData = GetIetfLanguageTag(culture)
                }
            ];

        static string GetIetfLanguageTag(CultureInfo culture)
            =>
            culture.IetfLanguageTag;
    }
}