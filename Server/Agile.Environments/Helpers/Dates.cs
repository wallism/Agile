using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;

namespace Agile.Common
{
    /// <summary>
    /// Support functions for dates
    /// </summary>
    public class Dates
    {
        /// <summary>
        /// Gets the value of a 'null' date.
        /// i.e. The value the system assigns when a new date is initialised but a specific value is not set.
        /// </summary>
        public static DateTime NullDate
        {
            get { return new DateTime(); }
        }

        #region Business Days Diff

        // ============================================================================
        /// <summary>
        /// Gets the number of business days between dates
        /// </summary>
        /// 
        /// <remarks>This is very primitive - upgrade to sophisticated
        /// algorithm when time permits!</remarks>
        /// 
        /// <param name="StartDate">Start date to compare</param>
        /// <param name="EndDate">End date to compare</param>
        /// 
        /// <returns>int</returns>
        // ============================================================================
        public static int BusinessDaysDiff(DateTime StartDate, DateTime EndDate)
        {
            int Result = 0;

            while (StartDate <= EndDate)
            {
                if (StartDate.DayOfWeek != DayOfWeek.Sunday &&
                    StartDate.DayOfWeek != DayOfWeek.Saturday)
                {
                    Result++;
                }

                StartDate = StartDate.AddDays(1);
            }

            return Result;
        }

        #endregion Methods

        /// <summary>
        /// Returns the given date as a formatted string.
        /// [d MMM yyyy]
        /// </summary>
        /// <param name="date">the date to format</param>
        /// <returns>the given date as a string</returns>
        public static string Format(DateTime date)
        {
            ArgumentValidation.CheckForNullReference(date, "date");
            return date.ToString("d MMM yyyy", CultureInfo.CurrentCulture);
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
        /// Returns the given sql date as a formatted string.
        /// </summary>
        /// <param name="date">the date</param>
        /// <returns>the date time</returns>
        public static string Format(SqlDateTime date)
        {
            ArgumentValidation.CheckForNullReference(date, "date");
            return Format(date.Value);
        }

        /// <summary>
        /// Returns the time of the given datetime as a formatted string.
        /// 24 hour time with hours, minutes and seconds.
        /// </summary>
        /// <param name="date">the date</param>
        /// <returns>the time element of the given date</returns>
        public static string FormatTime(DateTime date)
        {
            ArgumentValidation.CheckForNullReference(date, "date");
            return date.ToString("HH:mm:ss", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns the time of the given datetime as a formatted string.
        /// 24 hour time with just the hours and minutes.
        /// </summary>
        /// <param name="date">the date to format</param>
        /// <returns>the time part</returns>
        public static string FormatTimeShort(DateTime date)
        {
            ArgumentValidation.CheckForNullReference(date, "date");
            return date.ToString("HH:mm", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns true if the two given datetimes have the same date, 
        /// i.e. not including the time!
        /// </summary>
        /// <param name="first">first date to compare</param>
        /// <param name="second">second date to compare</param>
        /// <returns>true if the DATES are the same</returns>
        public static bool SameDate(DateTime first, DateTime second)
        {
            ArgumentValidation.CheckForNullReference(first, "first");
            ArgumentValidation.CheckForNullReference(second, "second");
            return first.Date.Equals(second.Date);
        }
        
        /// <summary>
        /// Returns true if the two given datetimes have the same date, 
        /// i.e. not including the time!
        /// </summary>
        /// <param name="first">first date to compare</param>
        /// <param name="second">second date to compare</param>
        /// <returns>true if the DATES are the same</returns>
        public static bool SameDate(DateTime? first, DateTime second)
        {
            if (!first.HasValue)
                return second.Equals(NullDate);
            return SameDate(first.Value, second);
        }

        /// <summary>
        /// Returns true if the check date is falls between the from and to date.
        /// (toDate may be 'before' the from date and this function will still work.)
        /// </summary>
        /// <param name="checkDate">Check if this date is between the first and second dates.</param>
        /// <param name="fromDate">From date (inclusive).</param>
        /// <param name="toDate">To date (inclusive).</param>
        /// <returns>True if the check date falls on or between the other two dates</returns>
        public static bool Between(DateTime checkDate, DateTime fromDate, DateTime toDate)
        {
            ArgumentValidation.CheckForNullReference(fromDate, "fromDate");
            ArgumentValidation.CheckForNullReference(toDate, "toDate");

            // Make fromDate < toDate
            if (toDate < fromDate)
            {
                DateTime temp = fromDate;
                fromDate = toDate;
                toDate = temp;
            }

            // Compare
            return (checkDate >= fromDate) && (checkDate <= toDate);
        }

//        /// <summary>
//        /// Checks if the given date is 'today'
//        /// </summary>
//        /// <param name="date">the date to check</param>
//        /// <returns>true if the date is today</returns>
//        public static bool IsToday(DateTime date)
//        {
//            ArgumentValidation.CheckForNullReference(date, "date");
//            return SameDate(Habitat.Now, date);
//        }

        /// <summary>
        /// This checks if this string is a valid date
        /// </summary>
        /// <param name="isThisADate">Check if this string is a valid date</param>
        /// <returns>true if the value can be converted to a date</returns>
        /// <remarks>Only use this if you are pretty sure that you already have a date!
        /// (because is determines if it is a date by trying to parse it and then
        /// returning false if an exception occurs)</remarks>
        public static bool IsDate(string isThisADate)
        {
            ArgumentValidation.CheckForNullReference(isThisADate, "isThisADate");
            try
            {
                DateTime.Parse(isThisADate, CultureInfo.CurrentCulture);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Returns the given date with milliseconds set to zero
        /// </summary>
        /// <param name="date">the date to set milliseconds to true</param>
        /// <returns>The source datetime with milliseconds set to zero</returns>
        public static DateTime ZeroMilliseconds(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }
    }
}