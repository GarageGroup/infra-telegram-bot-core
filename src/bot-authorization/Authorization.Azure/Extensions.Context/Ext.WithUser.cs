namespace GarageGroup.Infra.Telegram.Bot;

partial class ContextExtensions
{
    internal static IChatContext WithUser(this IChatContext context, ChatUser user)
        =>
        context is IChatContextWithUserSupplier withUserSupplier ? withUserSupplier.WithUser(user) : context;
}