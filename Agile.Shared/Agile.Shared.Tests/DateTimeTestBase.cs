using System;
using Agile.Diagnostics.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Agile.Shared.Tests
{
    /// <summary>
    /// Base class for tests that must check date calculations/values.
    /// Defaults Now to 4-sep-2011 21:04:05
    /// </summary>
    public class DateTimeTestBase
    {
        // (sunday)
        public static DateTime TestTimeDefault = new DateTime(2011, 9, 4, 21, 4, 5);
        protected static IDateTimeProvider TestTimeProvider = Substitute.For<IDateTimeProvider>();

        [SetUp]
        public void SetUp()
        {
            Logger.Testing(@"ARR set the testTimeProvider time to the default test time: sep 4 2011 21:04:05");
            AgileDateTime.SetProvider(TestTimeProvider);
            TestTimeProvider.Now.Returns(TestTimeDefault);
        }

        [TestFixtureTearDown]
        public void FixtureTeardown()
        {
            AgileDateTime.SetProvider(null);
        }
    }
}