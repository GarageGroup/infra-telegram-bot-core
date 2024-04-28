using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace GarageGroup.Infra.Telegram.Bot;

public sealed record class BotInputOnlineFile : BotInputTelegramFile
{
    public BotInputOnlineFile(Stream content, string? fileName = null)
        : base(content, fileName)
    { }

    public BotInputOnlineFile(string value)
        : base(GetFileType(value, out var isUrl))
    {
        if (isUrl)
        {
            Url = value;
        }
        else
        {
            FileId = value;
        }
    }

    public BotInputOnlineFile(Uri url) : base(BotFileType.Url)
        =>
        Url = url?.AbsoluteUri ?? throw new ArgumentNullException(nameof(url));

    public string? Url { get; }

    [return: NotNullIfNotNull(nameof(stream))]
    public static new BotInputOnlineFile? FromStream(Stream? stream)
        =>
        stream is null ? null : new(stream);

    [return: NotNullIfNotNull(nameof(value))]
    public static BotInputOnlineFile? FromValue(string? value)
        =>
        FromValue(value);

    [return: NotNullIfNotNull(nameof(stream))]
    public static implicit operator BotInputOnlineFile?(Stream? stream)
        =>
        FromStream(stream);

    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator BotInputOnlineFile?(string? value)
        =>
        FromValue(value);

    private static BotFileType GetFileType(string value, out bool isUrl)
    {
        if (Uri.TryCreate(value, UriKind.Absolute, out _))
        {
            isUrl = true;
            return BotFileType.Url;
        }

        isUrl = false;
        return BotFileType.Id;
    }
}