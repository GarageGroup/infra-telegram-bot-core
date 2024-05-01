namespace GarageGroup.Infra.Telegram.Bot;

public enum TelegramBotFailureCode
{
    Unknown,

    BadRequest = 400,

    Unauthorized = 401,

    Forbidden = 403,

    Conflict = 409,

    TooManyRequests = 429
}