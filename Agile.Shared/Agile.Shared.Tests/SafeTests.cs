using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Testing;
using NUnit.Framework;

namespace Agile.Shared.Tests
{
    [TestFixture]
    public class SafeTests : TestBase
    {
        #region NullableDateTimeOffsetTests

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void NullableDateTimeOffsetTests()
        {
            var now = AgileDateTime.UtcNow;
            Logger.Testing("try a number of different date formats to know what works");

            var day = now.ToString("yyyy-MM-dd");
            var time = now.ToString("HH:mm:ss.fffffff");
            var zz = now.ToString("zzz");

            var test = string.Format("{0}T{1}{2}", day, time, zz);
            Logger.Debug(test);
            Assert.AreEqual(now, Safe.NullableDateTimeOffset(test));

            day = now.ToString("yyyyMMdd");
            time = now.ToString("HH:mm:ss.fffffff");
            zz = now.ToString("zzz");

            test = string.Format("{0}T{1}{2}", day, time, zz);
            Logger.Debug("[{0}]fails because the date DOES need the -s ", test);
            Assert.AreEqual(null, Safe.NullableDateTimeOffset(test));
        }

        #endregion
    }
}
