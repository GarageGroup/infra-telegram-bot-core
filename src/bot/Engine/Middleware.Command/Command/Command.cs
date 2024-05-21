using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra.Telegram.Bot;

internal sealed class Command<TIn, TOut> : ICommand
    where TIn : IChatCommandIn<TOut>
{
    private readonly IChatCommand<TIn, TOut> chatCommand;

    private readonly Func<string, TIn>? initializer;

    private readonly IChatCommandParser<TIn>? parser;

    private readonly bool isWeekMatch;

    internal Command(
        IChatCommand<TIn, TOut> chatCommand,
        Func<string, TIn>? initializer = null,
        IChatCommandParser<TIn>? parser = null,
        string? name = null,
        bool isWeekMatch = false)
    {
        this.chatCommand = chatCommand;
        this.initializer = initializer;
        this.parser = parser;
        Name = name;
        this.isWeekMatch = isWeekMatch;
        Type = TIn.Type;
    }

    public string? Name { get; }

    public string Type { get; }

    public Optional<object> ParseByName(string command)
    {
        if ((initializer is null) || string.IsNullOrEmpty(Name))
        {
            return default;
        }

        if (isWeekMatch)
        {
            if (command.StartsWith(Name, StringComparison.InvariantCultureIgnoreCase))
            {
                return initializer.Invoke(command);
            }

            return default;
        }

        if (string.Equals(Name, command, StringComparison.InvariantCultureIgnoreCase))
        {
            return initializer.Invoke(command);
        }

        return default;
    }

    public Optional<object> ParseValue(ChatUpdate update)
    {
        if (parser is null)
        {
            return default;
        }

        return parser.Parse(update).Map(AsObject);

        static object AsObject(TIn value)
            =>
            value;
    }

    public async Task<TurnResult> InvokeAsync(
        IChatContext contex, object input, CancellationToken cancellationToken)
    {
        var request = new ChatCommandRequest<TIn, TOut>(contex, (TIn)input);

        var result = await chatCommand.SendAsync(request, cancellationToken).ConfigureAwait(false);
        return result.Context.CreateTurnResult(result.State);
    }

    public async Task<TurnResult> InvokeAsync(
        IChatContext contex, JsonElement input, JsonSerializerOptions options, CancellationToken cancellationToken)
    {
        var @in = input.Deserialize<TIn>(options);
        var request = new ChatCommandRequest<TIn, TOut>(contex, @in!);

        var result = await chatCommand.SendAsync(request, cancellationToken).ConfigureAwait(false);
        return result.Context.CreateTurnResult(result.State);
    }

    public ValueTask<ChatCommandResult<TOut>> SendAsync(
        ChatCommandRequest<TIn, TOut> request, CancellationToken cancellationToken)
        =>
        chatCommand.SendAsync(request, cancellationToken);
}