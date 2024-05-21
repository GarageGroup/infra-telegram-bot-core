using System;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class AuthorizationOption
{
    internal const string Scope = "user.read mailboxSettings.read";

    public AuthorizationOption(string tenantId, string clientId, string clientSecret, string redirectUri)
    {
        TenantId = tenantId.OrEmpty();
        ClientId = clientId.OrEmpty();
        ClientSecret = clientSecret.OrEmpty();
        RedirectUri = redirectUri.OrEmpty();
    }

    public string TenantId { get; }

    public string ClientId { get; }

    public string ClientSecret { get; }

    public string RedirectUri { get; }
}