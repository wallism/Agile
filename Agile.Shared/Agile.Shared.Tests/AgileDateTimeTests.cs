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
    public class AgileDateTimeTests
    {
        #region DefaultIsDateTimeNow

        /// <summary>
        /// DefaultIsDateTimeNow
        /// </summary>
        [Test]
        public void DefaultIsDateTimeNow()
        {
            Logger.Testing("ASS check Now's match, don't include milliseconds");
            Assert.AreEqual(DateTime.Now.ToString("ddMMyyyy HH:mm:f"), AgileDateTime.Now.ToString("ddMMyyyy HH:mm:f"));
        }

        #endregion

        #region MockDateTest

        /// <summary>
        /// MockDateTest
        /// </summary>
        [Test]
        public void MockDateTest()
        {
            Logger.Testing(@"provide a mock dateTime provider, setting the time to 15Apr2011 12:15:23");
            var mock = Substitute.For<IDateTimeProvider>();
            var nowDate = new DateTime(2011, 4, 15, 12, 15, 23);
            mock.Now.Returns(nowDate);

            Logger.Testing(@"ACT set the provider");
            AgileDateTime.SetProvider(mock);

            Logger.Testing("ASS AgileDateTime.Now is 15Apr2011 12:15:23");
            Assert.AreEqual(nowDate, AgileDateTime.Now);
        }

        #endregion
    }
}
