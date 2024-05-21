using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class CardKeyboardOption<T>
{
    public CardKeyboardOption(string confirmButtonText, string cancelButtonText, string cancelText)
    {
        ConfirmButtonText = confirmButtonText.OrNullIfWhiteSpace() ?? "Confirm";
        CancelButtonText = cancelButtonText.OrNullIfWhiteSpace() ?? "Cancel";
        CancelText = cancelText.OrNullIfWhiteSpace() ?? "Operation was canceled";
    }

    public string ConfirmButtonText { get; }

    public string CancelButtonText { get; }

    public string CancelText { get; }

    public CardWebAppButton<T>? WebAppButton { get; init; }
}