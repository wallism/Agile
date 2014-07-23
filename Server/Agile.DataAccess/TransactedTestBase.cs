using NUnit.Framework;

namespace Agile.DataAccess
{
    /// <summary>
    /// Base class used for testing against a database.
    /// Begins a transaction in the setup, rolls back the transaction
    /// in the teardown.
    /// </summary>
    /// <remarks>Removes the need to cleanup data changes during database
    /// tests.</remarks>
    [TestFixture]
    public class TransactedTestBase
    {
        #region Setup/Teardown

        /// <summary>
        /// Setup for testing
        /// </summary>
        [SetUp]
        public void BaseSetup()
        {
            ActiveTransaction.BeginTesting();

            Setup();
        }

        /// <summary>
        /// Teardown test
        /// </summary>
        [TearDown]
        public void BaseTeardown()
        {
            Teardown();

            ActiveTransaction.FinishTesting();
        }

        /// <summary>
        /// Setup Fixture for testing
        /// </summary>
        [TestFixtureSetUp]
        public void BaseTestFixtureSetup()
        {
            TestFixtureSetup();
        }

        /// <summary>
        /// Teardown Fixture
        /// </summary>
        [TestFixtureTearDown]
        public void BaseTestFixtureTeardown()
        {
            TestFixtureTeardown();
        }

        #endregion

        /// <summary>
        /// Override when the sub class needs to do more in the setup.
        /// </summary>
        /// <remarks>DO NOT put the attribute on the method in the sub class!</remarks>
        public virtual void Setup()
        {
        }

        /// <summary>
        /// Override when the sub class needs to do more in the teardown.
        /// </summary>
        /// <remarks>DO NOT put the attribute on the method in the sub class!</remarks>
        public virtual void Teardown()
        {
        }
        /// <summary>
        /// Override when the sub class needs to do more in the Fixture setup.
        /// </summary>
        /// <remarks>DO NOT put the attribute on the method in the sub class!</remarks>
        public virtual void TestFixtureSetup()
        {
        }

        /// <summary>
        /// Override when the sub class needs to do more in the Fixture teardown.
        /// </summary>
        /// <remarks>DO NOT put the attribute on the method in the sub class!</remarks>
        public virtual void TestFixtureTeardown()
        {
        }
    }
}