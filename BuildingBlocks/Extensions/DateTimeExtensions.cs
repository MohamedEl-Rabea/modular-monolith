using System.Globalization;

namespace WT.B2C.API.BuildingBlocks.Extensions;

public static class DateTimeExtensions
{
    private const string DayMonthYearFormat = "dd MMM yyyy";

    public static string ToDayMonthYearString(this DateOnly date)
        => date.ToString(DayMonthYearFormat, CultureInfo.InvariantCulture);

    public static string ToDayMonthYearString(this DateTime dateTime)
        => dateTime.ToString(DayMonthYearFormat, CultureInfo.InvariantCulture);
}
