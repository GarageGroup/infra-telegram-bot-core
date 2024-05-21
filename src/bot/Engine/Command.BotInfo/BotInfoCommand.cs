using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class BotInfoCommand(IConfigurationSection section) : IChatCommand<BotInfoCommandIn, Unit>
{
    private const string TimeFormat = "dd.MM.yyyy HH:mm:ss ('GMT'z)";

    private const string LineBreak = "\n\r";

    public async ValueTask<ChatCommandResult<Unit>> SendAsync(
        ChatCommandRequest<BotInfoCommandIn, Unit> request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var values = GetValues(request.Context);
        var builder = new StringBuilder();

        foreach (var value in values)
        {
            if (string.IsNullOrWhiteSpace(value.Value))
            {
                continue;
            }

            if (builder.Length > 0)
            {
                builder = builder.Append(LineBreak);
            }

            builder = builder.Append(value.Key).Append(": ").Append(value.Value);
        }

        var text = builder.ToString();
        _ = await request.Context.Api.SendHtmlModeTextAndRemoveReplyKeyboardAsync(text, cancellationToken).ConfigureAwait(false);

        return request.Context.CreateCompleteResult<Unit>(default);
    }

    private IEnumerable<KeyValuePair<string, string?>> GetValues(IChatContext context)
    {
        var localizer = context.GetLocalizer(BotCoreResource.BaseName);

        var botName = GetValueOrDefault(BotCoreResource.BotNameMessageName, BotCoreResource.BotNameMessageDefault);
        var botNameValue = GetValueOrFromConfiguration(BotCoreResource.BotNameValueMessageName, "ApiName");

        yield return new(botName, botNameValue);

        var botDescription = GetValueOrDefault(BotCoreResource.BotDescriptionMessageName, BotCoreResource.BotDescriptionMessageDefault);
        var botDescriptionValue = GetValueOrFromConfiguration(BotCoreResource.BotDescriptionValueMessageName, "Description");

        yield return new(botDescription, botDescriptionValue);

        var botVersion = GetValueOrDefault(BotCoreResource.BotVersionMessageName, BotCoreResource.BotVersionMessageDefault);
        yield return new(botVersion, section["ApiVersion"]);

        var botBuildTime = GetValueOrDefault(BotCoreResource.BotBuildTimeMessageName, BotCoreResource.BotBuildTimeMessageDefault);
        var buildTimeValue = GetTimeValue("BuildDateTime");

        yield return new(botBuildTime, buildTimeValue);

        string GetValueOrDefault(string name, string @default)
        {
            var value = localizer[name];
            return value.ResourceNotFound ? @default : value;
        }

        string? GetValueOrFromConfiguration(string name, string configurationKey)
        {
            var value = localizer[name];
            return value.ResourceNotFound ? section[configurationKey] : value;
        }

        string? GetTimeValue(string configurationKey)
        {
            var value = section.GetValue<DateTimeOffset?>(configurationKey);
            if (value is null)
            {
                return null;
            }

            return TimeZoneInfo.ConvertTime(value.Value, context.User.TimeZone).ToString(TimeFormat, context.User.Culture);
        }
    }
}