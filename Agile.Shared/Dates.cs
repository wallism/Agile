using System;
using System.Diagnostics;
using System.Globalization;

namespace Agile.Shared
{
    /// <summary>
    /// Dates
    /// </summary>
    public static class Dates
    {
        /// <summary>
        /// Returns true if the date is the same
        /// </summary>
        public static bool SameDate(DateTime? one, DateTime two)
        {
            if (!one.HasValue)
                return (two == NullDate);

            return one.Value.CompareTo(two) ==0;
        }

        /// <summary>
        /// Returns an 'unset' or null equivalent DateTime (new DateTime)
        /// </summary>
        /// <remarks>dont use delete later</remarks>
        public static DateTime NullDate
        {
            // to avoid this error SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.
            get { return new DateTime(1799, 1, 1);}
        }

        /// <summary>
        /// Returns the given date and time as a formatted string.
        /// [d MMM yyyy HH:mm:ss]
        /// </summary>
        /// <param name="date">the date to format</param>
        /// <returns>the date as a string</returns>
        public static string FormatWithTime(DateTime date)
        {
            ArgumentValidation.CheckForNullReference(date, "date");
            return date.ToString("d MMM yyyy HH:mm:ss", CultureInfo.CurrentCulture);
        }
        
        /// <summary>
        /// Format with time if it has a time (ie if it's not the very end or very start of the day)
        /// </summary>
        public static string FormatBizzy(this DateTime? date)
        {
            if (!date.HasValue)
                return string.Empty;
            return FormatBizzy(date.Value);
        }

        /// <summary>
        /// Format with time if it has a time (ie if it's not the very end or very start of the day)
        /// </summary>
        public static string FormatBizzy(this DateTime date)
        {
            // TODAY
            if (date.Date == AgileDateTime.Now.Date)
            {
                if (date.IsEndOfDay() || date.IsStartOfDay())
                    return "Today";

                var result = date - AgileDateTime.Now;
                var hoursTillDue = result.Hours;

                if (hoursTillDue < 0)
                    return string.Format("{0} Hour{1} ago", hoursTillDue*-1, ((hoursTillDue == -1) ? string.Empty : "s")); // times -1 to remove the negative
                if(hoursTillDue == 0)
                    return "NOW";

                return string.Format("{0} Hour{1}", hoursTillDue, ((hoursTillDue == 1) ? string.Empty : "s"));
            }

            // Tomorrow
            if (date.Date == AgileDateTime.Now.AddDays(1).Date)
            {
                if (date.IsEndOfDay() || date.IsStartOfDay())
                    return "Tomorrow";

                return string.Format("Tomorrow {0}", date.ToString("HH:mm"));
            }

            // Yesterday
            if (date.Date == AgileDateTime.Now.Subtract(TimeSpan.FromDays(1)).Date)
            {
                if (date.IsEndOfDay() || date.IsStartOfDay())
                    return "Yesterday";

                return string.Format("Yesterday {0}", date.ToString("HH:mm"));
            }

            // Late
            if (date < AgileDateTime.Now.Subtract(TimeSpan.FromDays(1)).Date)
            {
                var days = (date - AgileDateTime.Now).Days * -1;
                if(days < 7)
                    return string.Format("{0} Day{1} ago", days, ((days > 1) ? "s" : string.Empty));

                int weeks = days / 7;
                return string.Format("{0} Week{1} ago", weeks, ((weeks > 1) ? "s" : string.Empty));
            }


            if (date.IsStartOfDay() || date.IsEndOfDay()) // then dont include time in the string
                return date.ToString("dd-MMM-yy");
                
            return date.ToString("dd-MMM-yy HH:mm");
        }
    }

    /// <summary>
    /// Method extension for dates
    /// </summary>
    public static class DateExtensions
    {
        /// <summary>
        /// Returns a new date, with the same date but the time change to 23:59:59
        /// </summary>
        public static DateTime GetAsEndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        /// <summary>
        /// Returns a new date, with the same date but the time change to 00:00:00
        /// </summary>
        public static DateTime GetAsStartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        /// <summary>
        /// Returns true if the time is 23:59:59
        /// </summary>
        public static bool IsEndOfDay(this DateTime date)
        {
            return date.Hour == 23 && date.Minute == 59 && date.Second == 59;
        }

        /// <summary>
        /// Returns true if the time is 00:00:00
        /// </summary>
        public static bool IsStartOfDay(this DateTime date)
        {
            return date.Hour == 0 && date.Minute == 0 && date.Second == 0;
        }

        /// <summary>
        /// Returns a new date, with the same date but the time change to 23:59:59
        /// </summary>
        public static DateTime GetAsEndOfPreviousDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day).Subtract(TimeSpan.FromSeconds(1));
        }


        /// <summary>
        /// Returns the date as the start of its month, ie day will be 1st
        /// </summary>
        public static DateTime GetAsStartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Returns the date as the start of its month
        /// </summary>
        public static DateTime GetAsEndOfMonth(this DateTime date)
        {
            return date.AddMonths(1).GetAsStartOfMonth().Subtract(TimeSpan.FromSeconds(1));
        }

    }
    
    /// <summary>
    /// Components to intialize time for a DateTime
    /// </summary>
    public class Time
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Time()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public Time(int hour, int minute, int second, int millisecond)
        {
            Hour = (hour > 23) ? 23 : hour;
            Minute = (minute > 59) ? 59 : minute;
            Second = (second > 59) ? 59 : second;
            Millisecond = (millisecond > 9999) ? 9999 : millisecond;

        }

        /// <summary>
        /// Hour
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// Minute
        /// </summary>
        public int Minute { get; set; }

        /// <summary>
        /// Second
        /// </summary>
        public int Second { get; set; }

        /// <summary>
        /// Millisecond
        /// </summary>
        public int Millisecond { get; set; }

        /// <summary>
        /// Returns the full time as a string
        /// </summary>
        public string FullTime
        {
            get { return string.Format("{0}:{1}:{2}:{3}"
                , Hour, Minute, Second, Millisecond); }
        }
    }

}