using System;

namespace WSharp.Extensions
{
    /// <summary>Extension methods for the <see cref="DateTime"/> struct.</summary>
    public static class DateTimeExtensions
    {
        /// <summary>Changes the time of a <see cref="DateTime"/>.</summary>
        /// <remarks>
        ///     Note that <see cref="DateTime"/> is a struct and its fields are readonly. The return
        ///     value is a new value, the original is not changed.
        /// </remarks>
        /// <param name="dateTime">DateTime of which the time should be changed.</param>
        /// <param name="hours">The hours of the time.</param>
        /// <param name="minutes">The minutes of the time.</param>
        /// <param name="seconds">The seconds of the time.</param>
        /// <param name="milliseconds">The milliseconds of the time.</param>
        /// <returns>The modified <see cref="DateTime"/>.</returns>
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

        /// <summary>Changes the time of a <see cref="DateTime"/>.</summary>
        /// <remarks>
        ///     Note that <see cref="DateTime"/> is a struct and its fields are readonly. The return
        ///     value is a new value, the original is not changed.
        /// </remarks>
        /// <param name="dateTime">DateTime of which the time should be changed.</param>
        /// <param name="timeSpan">The new time.</param>
        /// <returns>The modified <see cref="DateTime"/>.</returns>
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

        /// <summary>Changes the time of a <see cref="DateTime"/>.</summary>
        /// <remarks>
        ///     Note that <see cref="DateTime"/> is a struct and its fields are readonly. The return
        ///     value is a new value, the original is not changed.
        /// </remarks>
        /// <param name="dateTime">DateTime of which the time should be changed.</param>
        /// <param name="time">The new time.</param>
        /// <returns>The modified <see cref="DateTime"/>.</returns>
        public static DateTime ChangeTime(this DateTime dateTime, DateTime? time) => dateTime.ChangeTime(time?.TimeOfDay);
    }
}