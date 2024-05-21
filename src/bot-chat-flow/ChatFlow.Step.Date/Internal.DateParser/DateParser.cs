using System;
using System.Text.RegularExpressions;

namespace GarageGroup.Infra.Telegram.Bot;

internal static partial class DateParser
{
    private static readonly Regex DateOnlyRegex;

    static DateParser()
        =>
        DateOnlyRegex = MyRegex();

    private static ReadOnlySpan<char> GetDayValue(this Match match)
        =>
        match.GetStringValue(1);

    private static ReadOnlySpan<char> GetMonthValue(this Match match)
        =>
        match.GetStringValue(3);

    private static ReadOnlySpan<char> GetYearValue(this Match match)
        =>
        match.GetStringValue(5);

    private static ReadOnlySpan<char> GetStringValue(this Match match, int groupNumber)
        =>
        match.Groups.Count >  groupNumber ? match.Groups[groupNumber].ValueSpan : default;

    private static int? ParseInt(this ReadOnlySpan<char> value)
        =>
        value.Length > 0 ? int.Parse(value) : null;

    [GeneratedRegex(@"^(\d{1,2})(\.(\d{1,2})(\.(\d{2,4}))?)$", RegexOptions.CultureInvariant)]
    private static partial Regex MyRegex();
}