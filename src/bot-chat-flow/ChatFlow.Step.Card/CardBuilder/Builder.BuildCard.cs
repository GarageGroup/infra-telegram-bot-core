using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace GarageGroup.Infra.Telegram.Bot;

partial class CardBuilder
{
    internal static ChatMessageSendRequest BuildCardMessage<T>(EntityCardOption option, CardKeyboardOption<T>? keyboard = null)
        =>
        new(option.BuildCardMessageText())
        {
            ParseMode = BotParseMode.Html,
            ReplyMarkup = keyboard.BuildCardMessageKeyboard()
        };

    private static string BuildCardMessageText(this EntityCardOption option)
    {
        var builder = new StringBuilder($"<b>{HttpUtility.HtmlEncode(option.HeaderText)}</b>");

        if (option.FieldValues.IsEmpty)
        {
            return builder.ToString();
        }

        builder = builder.Append("\n\r");

        foreach (var field in option.FieldValues)
        {
            var fieldName = HttpUtility.HtmlEncode(field.Key);
            var fieldValue = HttpUtility.HtmlEncode(field.Value);

            var hasName = string.IsNullOrEmpty(fieldName) is false;
            var hasValue = string.IsNullOrEmpty(fieldValue) is false;

            if (hasName is false && hasValue is false)
            {
                continue;
            }

            builder = builder.Append("\n\r");

            if (hasName)
            {
                builder = builder.AppendFormat("<b>{0}</b>:", fieldName);

                if (hasValue)
                {
                    builder = builder.Append(' ');
                }
            }

            if (hasValue)
            {
                builder = builder.Append(fieldValue);
            }
        }

        return builder.ToString();
    }

    private static BotReplyMarkupBase BuildCardMessageKeyboard<T>(this CardKeyboardOption<T>? keyboard)
    {
        if (keyboard is null)
        {
            return new BotReplyKeyboardRemove();
        }

        return new BotReplyKeyboardMarkup
        {
            Keyboard = InnerGetButtons(keyboard).ToFlatArray(),
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        static IEnumerable<FlatArray<BotKeyboardButton>> InnerGetButtons(CardKeyboardOption<T> keyboard)
        {
            yield return
            [
                new(keyboard.CancelButtonText),
                new(keyboard.ConfirmButtonText)
            ];

            if (string.IsNullOrWhiteSpace(keyboard.WebAppButton?.WebAppUrl))
            {
                yield break;
            }

            yield return
            [
                new(keyboard.WebAppButton.ButtonName)
                {
                    WebApp = new(keyboard.WebAppButton.WebAppUrl)
                }
            ];
        }
    }
}