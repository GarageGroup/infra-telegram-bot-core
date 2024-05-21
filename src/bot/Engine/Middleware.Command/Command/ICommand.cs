using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

internal interface ICommand
{
    string? Name { get; }

    string Type { get; }

    Optional<object> ParseByName(string command);

    Optional<object> ParseValue(ChatUpdate update);

    Task<TurnResult> InvokeAsync(IChatContext contex, object input, CancellationToken cancellationToken);

    Task<TurnResult> InvokeAsync(IChatContext contex, JsonElement input, JsonSerializerOptions options, CancellationToken cancellationToken);
}