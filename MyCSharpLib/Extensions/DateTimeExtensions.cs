using System;

namespace MyCSharpLib.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                dateTime.Kind);
        }

        public static DateTime ChangeTime(this DateTime dateTime, TimeSpan? timeSpan)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                timeSpan?.Hours ?? 0,
                timeSpan?.Minutes ?? 0,
                timeSpan?.Seconds ?? 0,
                timeSpan?.Milliseconds ?? 0,
                dateTime.Kind);
        }

        public static DateTime ChangeTime(this DateTime dateTime, DateTime? time)
        {
            return dateTime.ChangeTime(time?.TimeOfDay);
        }
    }
}
