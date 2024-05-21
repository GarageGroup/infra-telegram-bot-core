namespace GarageGroup.Infra.Telegram.Bot;

public enum CallbackFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.BadRequest, "Code must be specified")]
    AbsentCode,

    [Problem(FailureStatusCode.BadRequest, "State must be specified")]
    AbsentState,

    [Problem(FailureStatusCode.BadRequest, "State is invalid")]
    InvalidState,

    [Problem(FailureStatusCode.Unauthorized, "State is incorrect")]
    IncorrectState,

    [Problem(FailureStatusCode.Unauthorized, detailFromFailureMessage: true)]
    AuthorizationError,
}