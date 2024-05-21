namespace GarageGroup.Infra.Telegram.Bot;

public interface IChatContextWithUserSupplier
{
    IChatContext WithUser(ChatUser user);
}