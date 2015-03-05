using System;

namespace Agile.Shared
{
    public interface IDateTimeProvider
    {
        /// <summary>
        /// The current date
        /// </summary>
        DateTime Now { get; }

        DateTimeOffset UtcNow { get; }
    }

    /// <summary>
    /// datetime class to allow easier testing of datetime functionality, 
    /// allows us to mock the returned time by setting a different providers
    /// </summary>
    public static class AgileDateTime
    {
        public const string ShortMonthDayTime = "MMM dd @ HH:mm";

        private static IDateTimeProvider provider;
        /// <summary>
        /// ctor
        /// </summary>
        static AgileDateTime()
        {
            provider = new DefaultDateTimeProvider();
        }

        /// <summary>
        /// Set the dateTime provider (typically will be a mock provider for unit testing).
        /// Set to null to returns to default provider (which basically returns DateTime.Now)
        /// </summary>
        public static void SetProvider(IDateTimeProvider mockProvider)
        {
            provider = mockProvider ?? new DefaultDateTimeProvider();
        }

        /// <summary>
        /// Gets the current datetime
        /// </summary>
        public static DateTime Now
        {
            get { return provider.Now; }
        }

        /// <summary>
        /// Gets the current UTC datetime
        /// </summary>
        public static DateTimeOffset UtcNow
        {
            get { return provider.UtcNow; }
        }
    }

    /// <summary>
    /// Default DateTime provider simply returns DateTime.Now
    /// </summary>
    public class DefaultDateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// The current date
        /// </summary>
        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// The current date (utc)
        /// </summary>
        public DateTimeOffset UtcNow
        {
            get { return DateTimeOffset.UtcNow; }
        }
    }
}
