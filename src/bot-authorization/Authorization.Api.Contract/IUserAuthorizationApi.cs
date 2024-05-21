namespace GarageGroup.Infra.Telegram.Bot;

public interface IUserAuthorizationApi : IChatUserGetSupplier, IUserAuthorizeSupplier, IUserUnauthorizeSupplier;