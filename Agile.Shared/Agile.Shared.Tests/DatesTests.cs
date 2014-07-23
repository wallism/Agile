using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agile.Diagnostics.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Agile.Shared.Tests
{
    [TestFixture]
    public class DatesTests : DateTimeTestBase
    {

        #region AddMonth

        /// <summary>
        /// Created to double check the behaviour of the built in AddMonth
        /// </summary>
        [Test]
        public void AddMonth()
        {
            Assert.AreEqual(10, AgileDateTime.Now.AddMonths(1).Month);
            Assert.AreEqual(11, AgileDateTime.Now.AddMonths(2).Month);
            Assert.AreEqual(12, AgileDateTime.Now.AddMonths(3).Month);
            Assert.AreEqual(1, AgileDateTime.Now.AddMonths(4).Month);

            TestTimeProvider.Now.Returns(new DateTime(2011, 1, 31));
            Assert.AreEqual(2, AgileDateTime.Now.AddMonths(1).Month);
            Assert.AreEqual(3, AgileDateTime.Now.AddMonths(2).Month);
            Assert.AreEqual(4, AgileDateTime.Now.AddMonths(3).Month);
            Assert.AreEqual(5, AgileDateTime.Now.AddMonths(4).Month);

            TestTimeProvider.Now.Returns(new DateTime(2011, 2, 28));
            Assert.AreEqual(3, AgileDateTime.Now.AddMonths(1).Month);
            Assert.AreEqual(4, AgileDateTime.Now.AddMonths(2).Month);
            Assert.AreEqual(5, AgileDateTime.Now.AddMonths(3).Month);
            Assert.AreEqual(6, AgileDateTime.Now.AddMonths(4).Month);
        }

        #endregion

        #region GetAsEndOfMonth

        [Test]
        public void GetAsEndOfMonth()
        {
            Assert.AreEqual(new DateTime(2011, 9, 30, 23, 59, 59), AgileDateTime.Now.GetAsEndOfMonth());
            Assert.AreEqual(new DateTime(2011, 10, 31, 23, 59, 59), AgileDateTime.Now.AddMonths(1).GetAsEndOfMonth());
            Assert.AreEqual(new DateTime(2012, 1, 31, 23, 59, 59), new DateTime(2012, 1, 1).GetAsEndOfMonth());
        }

        #endregion


        private static void SetTestingDateTime()
        {
            Logger.Testing(@"provide a mock dateTime provider, setting the time to 15Apr2011 15:15:23");
            var mock = Substitute.For<IDateTimeProvider>();
            var nowDate = new DateTime(2011, 4, 15, 12, 15, 23);
            mock.Now.Returns(nowDate);

            Logger.Testing(@"ACT set the provider");
            AgileDateTime.SetProvider(mock);
        }


        #region FormatBizzyToday

        [Test]
        public void FormatBizzyToday()
        {
            SetTestingDateTime();

            Logger.Testing("ASSERT Today very end of day ");
            Assert.AreEqual("Today", new DateTime(2011, 4, 15, 23, 59, 59).FormatBizzy());
            Logger.Testing("ASSERT Today very beginning of day ");
            Assert.AreEqual("Today", new DateTime(2011, 4, 15, 0, 0, 0).FormatBizzy());

            Logger.Testing("because there is a time, then we change the result based on hours");
            Logger.Testing("ASSERT Today due at 16:00 ");
            Assert.AreEqual("3 Hours", new DateTime(2011, 4, 15, 16, 0, 0).FormatBizzy());
            Assert.AreEqual("1 Hour", new DateTime(2011, 4, 15, 14, 0, 0).FormatBizzy());
            Assert.AreEqual("3 Hours", new DateTime(2011, 4, 15, 16, 0, 0).FormatBizzy());

            Logger.Testing("ASSERT Today 1pm (so due in 45m))");
            Assert.AreEqual("NOW", new DateTime(2011, 4, 15, 13, 00, 00).FormatBizzy());
            Assert.AreEqual("NOW", new DateTime(2011, 4, 15, 12, 24, 00).FormatBizzy());
            Assert.AreEqual("NOW", new DateTime(2011, 4, 15, 12, 10, 0).FormatBizzy());
            Assert.AreEqual("1 Hour", new DateTime(2011, 4, 15, 13, 24, 00).FormatBizzy());


            Logger.Testing("ASSERT Today 5pm (an hour after due) ");
            Assert.AreEqual("1 Hour ago", new DateTime(2011, 4, 15, 11, 12, 0).FormatBizzy());

            Logger.Testing("ASSERT Today 23:58 ");
            Assert.AreEqual("7 Hours ago", new DateTime(2011, 4, 15, 5, 5, 0).FormatBizzy());
        }

        #endregion

        #region FormatBizzyTomorrow

        [Test]
        public void FormatBizzyTomorrow()
        {
            SetTestingDateTime();

            Assert.AreEqual("Tomorrow 12:15", AgileDateTime.Now.AddDays(1).FormatBizzy());
            Assert.AreEqual("Tomorrow", new DateTime(2011, 4, 16, 23, 59, 59).FormatBizzy());
            Assert.AreEqual("Tomorrow", new DateTime(2011, 4, 16, 0, 0, 0).FormatBizzy());

            Assert.AreEqual("Tomorrow 07:45", new DateTime(2011, 4, 16, 7, 45, 0).FormatBizzy());
            Assert.AreEqual("Tomorrow 23:00", new DateTime(2011, 4, 16, 23, 0, 0).FormatBizzy());

        }

        #endregion

        #region FormatBizzyYesterday

        [Test]
        public void FormatBizzyYesterday()
        {
            SetTestingDateTime();

            Assert.AreEqual("Yesterday 12:15", AgileDateTime.Now.Subtract(TimeSpan.FromDays(1)).FormatBizzy());
            Assert.AreEqual("Yesterday", new DateTime(2011, 4, 14, 23, 59, 59).FormatBizzy());
            Assert.AreEqual("Yesterday", new DateTime(2011, 4, 14, 0, 0, 0).FormatBizzy());

            Assert.AreEqual("Yesterday 08:30", new DateTime(2011, 4, 14, 8, 30, 0).FormatBizzy());
            Assert.AreEqual("Yesterday 16:00", new DateTime(2011, 4, 14, 16, 0, 0).FormatBizzy());
        }

        #endregion

        #region FormatBizzyPastDue

        [Test]
        public void FormatBizzyPastDue()
        {
            SetTestingDateTime();
            Assert.AreEqual("2 Days ago", new DateTime(2011, 4, 13, 0, 0, 0).FormatBizzy());

            Assert.AreEqual("6 Days ago", new DateTime(2011, 4, 9, 0, 0, 0).FormatBizzy());

            Assert.AreEqual("1 Week ago", new DateTime(2011, 4, 8, 0, 0, 0).FormatBizzy());
            Assert.AreEqual("2 Weeks ago", new DateTime(2011, 4, 1, 0, 0, 0).FormatBizzy());
            Assert.AreEqual("5 Weeks ago", new DateTime(2011, 3, 10, 0, 0, 0).FormatBizzy());
        }

        #endregion


    }
}
