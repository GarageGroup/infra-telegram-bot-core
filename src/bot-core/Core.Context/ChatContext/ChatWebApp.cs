using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ChatWebApp
{
    public ChatWebApp(Uri baseAddress)
        =>
        BaseAddress = baseAddress;

    public Uri BaseAddress { get; }

    public string BuildUrl([AllowNull] string relativePath, params KeyValuePair<string, string>[] queryParams)
    {
        var uri = new Uri(BaseAddress, relativePath);
        var queryParameters = BuildQueryParameters(queryParams);

        var builder = new UriBuilder(uri)
        {
            Query = string.Join('&', queryParameters)
        };

        return builder.Uri.ToString();

        static IEnumerable<string> BuildQueryParameters(KeyValuePair<string, string>[] queryParams)
        {
            if (queryParams?.Length is not > 0)
            {
                yield break;
            }

            foreach (var queryParam in queryParams)
            {
                if (string.IsNullOrWhiteSpace(queryParam.Key) || string.IsNullOrWhiteSpace(queryParam.Value))
                {
                    continue;
                }

                yield return $"{queryParam.Key}={queryParam.Value}";
            }
        }
    }
}