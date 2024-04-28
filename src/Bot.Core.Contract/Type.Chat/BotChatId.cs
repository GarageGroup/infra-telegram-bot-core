using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

[JsonConverter(typeof(BotChatIdJsonConverter))]
public sealed class BotChatId : IEquatable<BotChatId>
{
    public BotChatId(long identifier)
        =>
        Identifier = identifier;

    public BotChatId(string username)
    {
        if (username is null)
        {
            throw new ArgumentNullException(nameof(username));
        }

        if (username.Length > 1 && username.StartsWith("@", StringComparison.InvariantCulture))
        {
            Username = username;
        }
        else if (long.TryParse(username, out var identifier))
        {
            Identifier = identifier;
        }
        else
        {
            throw new ArgumentException("Username value should be Identifier or Username that starts with @");
        }
    }

    public long? Identifier { get; }

    public string? Username { get; }

    private static Type EqualityContract
        =>
        typeof(BotChatId);

    private static EqualityComparer<Type> EqualityContractComparer
        =>
        EqualityComparer<Type>.Default;

    private static EqualityComparer<long> IdentifierEqualityComparer
        =>
        EqualityComparer<long>.Default;

    private static StringComparer UsernameEqualityComparer
        =>
        StringComparer.InvariantCulture;

    public override string ToString()
        =>
        Identifier?.ToString(CultureInfo.InvariantCulture) ?? Username ?? string.Empty;

    public override int GetHashCode()
    {
        var equalityContractComparer = EqualityContractComparer.GetHashCode(EqualityContract);

        if (Identifier is not null)
        {
            return HashCode.Combine(equalityContractComparer, true, IdentifierEqualityComparer.GetHashCode(Identifier.Value));
        }

        if (Username is not null)
        {
            return HashCode.Combine(equalityContractComparer, false, UsernameEqualityComparer.GetHashCode(Username));
        }

        return HashCode.Combine(equalityContractComparer, false);
    }

    public override bool Equals(object? obj)
        =>
        obj is BotChatId chatId && Equals(chatId);

    public bool Equals(BotChatId? other)
        =>
        Equals(this, other);

    public static bool operator ==(BotChatId? left, BotChatId? right)
        =>
        Equals(left, right);

    public static bool operator !=(BotChatId? left, BotChatId? right)
        =>
        Equals(left, right) is not true;

    public static implicit operator BotChatId(long identifier)
        =>
        new(identifier);

    public static implicit operator BotChatId(string username)
        =>
        new(username);

    public static implicit operator string?(BotChatId? chatId)
        =>
        chatId?.ToString();

    [return: NotNullIfNotNull(nameof(chat))]
    public static implicit operator BotChatId?(BotChat? chat)
        =>
        chat?.Id;

    public static bool Equals(BotChatId? left, BotChatId? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        if (left.Identifier is not null && right.Identifier is not null)
        {
            return IdentifierEqualityComparer.Equals(left.Identifier.Value, right.Identifier.Value);
        }

        return UsernameEqualityComparer.Equals(left.Username, right.Username);
    }
}