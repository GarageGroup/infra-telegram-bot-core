namespace GarageGroup.Infra.Telegram.Bot;

partial class ContextExtensions
{
    internal static bool IsMessageUpdate(this ChatUpdate update)
        =>
        update.Message is not null || update.EditedMessage is not null || update.CallbackQuery is not null;
}