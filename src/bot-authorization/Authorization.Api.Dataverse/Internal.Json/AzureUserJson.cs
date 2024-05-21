namespace GarageGroup.Infra.Telegram.Bot;

internal readonly record struct AzureUserJson
{
    internal const string RequestUrl
        =
        "https://graph.microsoft.com/v1.0/me?$select=id,mailboxsettings";

    public string? Id { get; init; }

    public Mailbox? MailboxSettings { get; init; }

    internal sealed record class Mailbox
    {
        public string? TimeZone { get; init; }
    }
}