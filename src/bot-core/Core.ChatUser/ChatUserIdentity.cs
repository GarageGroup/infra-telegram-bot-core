using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class ChatUserIdentity
{
    [JsonConstructor]
    public ChatUserIdentity(Guid systemId, [AllowNull] string name)
    {
        SystemId = systemId;
        Name = name.OrEmpty();
    }

    public Guid SystemId { get; }

    public string Name { get; }

    public FlatArray<KeyValuePair<string, string>> Claims { get; init; }
}