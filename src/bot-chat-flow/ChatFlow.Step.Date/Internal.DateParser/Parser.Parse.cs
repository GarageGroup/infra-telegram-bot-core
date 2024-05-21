using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra.Telegram.Bot;

partial class DateParser
{
    internal static Result<DateOnly, Unit> ParseOrFailure([AllowNull] string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return default;
        }

        var match = DateOnlyRegex.Match(text.Trim());
        if (match.Success is false)
        {
            return default;
        }

        var now = DateTime.Now;
        var yearResult = match.GetYearValue().ParseYearNumber(now.Year);
        if (yearResult.IsFailure)
        {
            return default;
        }

        var year = yearResult.SuccessOrThrow();

        var monthResult = match.GetMonthValue().ParseMonthNumber(now.Month);
        if (monthResult.IsFailure)
        {
            return default;
        }

        var month = monthResult.SuccessOrThrow();

        var dayResult = match.GetDayValue().ParseDayNumber(month, year);
        if (dayResult.IsFailure)
        {
            return default;
        }

        var day = dayResult.SuccessOrThrow();
        return new DateOnly(year, month, day);
    }

    private static Result<int, Unit> ParseYearNumber(this ReadOnlySpan<char> yearValue, int todayYear)
        =>
        (yearValue.ParseInt(), yearValue.Length) switch
        {
            (null, _) => todayYear,
            (<= 0, _) => Result.Absent<int>(),
            (int year, < 4) => 2000 + year,
            (var year, _) => year.Value
        };

    private static Result<int, Unit> ParseMonthNumber(this ReadOnlySpan<char> monthValue, int todayMonth)
        =>
        monthValue.ParseInt() switch
        {
            null => todayMonth,
            <= 0 or > 12 => Result.Absent<int>(),
            var month => month.Value
        };

    private static Result<int, Unit> ParseDayNumber(this ReadOnlySpan<char> dayValue, int month, int year)
        =>
        dayValue.ParseInt() switch
        {
            null or <= 0 or > 31 => Result.Absent<int>(),
            int invalid when invalid > DateTime.DaysInMonth(year, month) => Result.Absent<int>(),
            var day => day.Value
        };
}