using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotContact
{
    [JsonConstructor]
    public BotContact(string phoneNumber, string firstName)
    {
        PhoneNumber = phoneNumber.OrEmpty();
        FirstName = firstName.OrEmpty();
    }

    public string PhoneNumber { get; }

    public string FirstName { get; }

    public string? LastName { get; init; }

    public long? UserId { get; init; }

    public string? Vcard { get; init; }
}