using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class CardKeyboardOption<T>
{
    public CardKeyboardOption(string confirmButtonText, string cancelButtonText, Func<Result<T, ChatBreakState>> forwardCancellation)
    {
        ConfirmButtonText = confirmButtonText.OrNullIfWhiteSpace() ?? "Confirm";
        CancelButtonText = cancelButtonText.OrNullIfWhiteSpace() ?? "Cancel";
        ForwardCancellation = forwardCancellation;
    }

    public string ConfirmButtonText { get; }

    public string CancelButtonText { get; }

    public Func<Result<T, ChatBreakState>> ForwardCancellation { get; }

    public CardWebAppButton<T>? WebAppButton { get; init; }
}