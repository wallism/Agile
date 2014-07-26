using System;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Mobile.Tests;
using Agile.Mobile.Web;
using Agile.Shared.IoC;
using NUnit.Framework;

namespace Agile.Mobile.PCL.Tests
{

    [TestFixture]
    public class MainHttpServiceTests : HttpTestBase
    {
        public override void TestFixtureSetUp()
        {
            base.TestFixtureSetUp();
            // need to register the service (instance is fine)
            Container.RegisterInstance<ISystemHttpService>(new SystemHttpService("Acoustie")); // ideally would not pass in Acoustie here, but it does need a system name and don't want to spend time fixing this right now...
        }

        #region GetVersionNumberWorks

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void GetVersionNumberWorks()
        {
            Logger.Testing("ARRANGE");
            Logger.Testing("ACT");
            Logger.Testing("ASSERT");
            Assert.AreEqual(false, true);
        }

        #endregion

    }
}
