using System;
using Agile.Diagnostics.Logging;
using Agile.Shared;
using Agile.Shared.IoC;
using NSubstitute;
using NUnit.Framework;

namespace Agile.Testing
{
    public abstract class TestBase
    {

        /// <summary>
        /// fixture setup
        /// </summary>
        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
            Logger.Debug("TestFixtureSetUp...");

        }

        /// <summary>
        /// fixture teardown
        /// </summary>
        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {
            Container.Reset();

            Logger.Debug("TestFixtureTearDown...");
        }

        /// <summary>
        /// setup
        /// </summary>
        [SetUp]
        public virtual void SetUp()
        {
            Logger.Debug("SetUp...");
        }

        /// <summary>
        /// teardown
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
            Logger.Debug("TearDown...");
        }

        /// <summary>
        /// Register NLog loggers for testing
        /// </summary>
        protected static void RegisterLoggers()
        {
        }

        /// <summary>
        /// Create a mock with nsubstitute and register it in the Container.
        /// </summary>
        protected T RegisterMock<T>() where T : class
        {
            Logger.Debug("Register MOCK: {0}", typeof(T).Name);
            var mock = Substitute.For<T>();
            Container.RegisterInstance(mock);
            return mock;
        }

        protected void AssertContains(string contains, string message)
        {
            Assert.IsNotNull(message, "message cannot be null");
            Logger.Debug(@"ASSERT:-
Value: {0} 
Contains: {1}", message, contains);
            Assert.IsTrue(message.Contains(contains));
        }

        /// <summary>
        /// Make 'Now' be n mins ahead (or behind with -)
        /// </summary>
        /// <remarks>dont' forget to revert to normal time</remarks>
        public static void SetNowToFuture(int addHours)
        {
            var dateProvider = Substitute.For<IDateTimeProvider>();
            dateProvider.Now.Returns(DateTime.Now.AddHours(addHours));
            dateProvider.UtcNow.Returns(DateTime.UtcNow.AddHours(addHours));
            AgileDateTime.SetProvider(dateProvider);
        }

        /// <summary>
        /// Make 'Now' be n mins ahead (or behind with -)
        /// </summary>
        /// <remarks>dont' forget to revert to normal time</remarks>
        public static void SetNowToFutureMin(int addMins)
        {
            var dateProvider = Substitute.For<IDateTimeProvider>();
            dateProvider.Now.Returns(DateTime.Now.AddMinutes(addMins));
            dateProvider.UtcNow.Returns(DateTime.UtcNow.AddMinutes(addMins));
            AgileDateTime.SetProvider(dateProvider);
        }

        public string GetUniqueIshStringId(string prefix = "Test")
        {
            var suffix = AgileDateTime.Now.ToString("HHmmssffff");
            var accession = string.Format("{0}{1}", prefix, suffix);
            return accession;
        }
    }
}
