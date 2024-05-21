namespace GarageGroup.Infra.Telegram.Bot;

public static class AuthorizationResource
{
    public const string BaseName = "Authorization";

    public const string UserDisabledMessageName = "UserDisabled";

    public const string UserDisabledMessageDefault = "Your account is blocked";

    public const string ChooseLanguageMessageName = "ChooseLanguage";

    public const string ChooseLanguageMessageDefault = "Choose the language";

    public const string ChosenLanguageMessageName = "ChosenLanguage";

    public const string ChosenLanguageMessageDefault = "Chosen language";

    public const string SignInTextMessageName = "SignInText";

    public const string SignInTextMessageDefault = "Sign in to your account Azure";

    public const string SignInButtonMessageName = "SignInButton";

    public const string SignInButtonMessageDefault = "Sign in";

    public const string AuthorizationErrorMessageName = "AuthorizationError";

    public const string AuthorizationErrorMessageDefault = "Failed to authenticate. Please try again later or contact the administrator";

    public const string UserNotFoundMessageName = "UserNotFound";

    public const string UserNotFoundMessageDefault = "User was not found in Dataverse. Please contact the administrator";

    public const string UnexpectedErrorMessageName = "UnexpectedError";

    public const string UnexpectedErrorMessageDefault = "An unexpected authorization error occurred. Please contact the administrator";

    public const string AuthorizationSuccessTemplateName = "AuthorizationSuccess";

    public const string AuthorizationSuccessTemplateDefault = "{0}, authorization was successful!";

    public const string SignOutSuccessMessageName = "SignOutSuccess";

    public const string SignOutSuccessMessageDefault = "You have signed out of your account";
}