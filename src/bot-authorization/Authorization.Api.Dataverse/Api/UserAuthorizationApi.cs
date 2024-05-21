using System;
using System.Globalization;
using TimeZoneConverter;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed partial class UserAuthorizationApi : IUserAuthorizationApi
{
    private const string AzureUserErrorBaseMessage
        =
        "An unexpected http failure occured when trying to get Azure user by access token:";

    private readonly IHttpApi httpApi;

    private readonly IDataverseEntityGetSupplier dataverseGetApi;

    private readonly IDataverseEntityUpdateSupplier dataverseUpdateApi;

    internal UserAuthorizationApi(
        IHttpApi httpApi,
        IDataverseEntityGetSupplier dataverseGetApi,
        IDataverseEntityUpdateSupplier dataverseUpdateApi)
    {
        this.httpApi = httpApi;
        this.dataverseGetApi = dataverseGetApi;
        this.dataverseUpdateApi = dataverseUpdateApi;
    }

    private static TimeZoneInfo? TryFindSystemTimeZoneById(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        return TZConvert.TryGetTimeZoneInfo(id, out var timeZone) ? timeZone : null;
    }

    private static CultureInfo? GetCultureInfo(string? languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
        {
            return null;
        }

        return new(languageCode);
    }
}