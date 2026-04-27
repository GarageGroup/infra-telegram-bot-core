using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ChatUserIdentity
{
    [JsonConstructor]
    public ChatUserIdentity(Guid id, [AllowNull] string name)
    {
        Id = id;
        Name = name.OrEmpty();
    }

    public Guid Id { get; }

    public string Name { get; }

    public FlatArray<KeyValuePair<string, string>> Claims { get; init; }
}