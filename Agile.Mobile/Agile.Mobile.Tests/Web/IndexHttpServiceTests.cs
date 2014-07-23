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
    public class IndexHttpServiceTests : HttpTestBase
    {
        public override void TestFixtureSetUp()
        {
            base.TestFixtureSetUp();
            // need to register the service (instance is fine)
            Container.RegisterInstance<IIndexHttpService>(new IndexHttpService());

        }


        #region GetTestJsonString

        /// <summary>
        /// Get a string (still json)
        /// </summary>
        [Test]
        public void GetTestJsonString()
        {
            var service = Container.Resolve<IIndexHttpService>();
            var callResult = CallAsync(service.TestGetTextAsync);
            Logger.Testing("CallAsync returned");
            Assert.AreEqual("This is a test...testing testing.40", callResult.Result.Value);

            Logger.Testing("Test COMPLETE");
        }
        
        #endregion

        #region GetJsonClassTests

        /// <summary>
        /// This test proves:
        ///   - serialization works
        ///     - even though class name is differnt on server and client
        ///     - and even though the classes are different (there is a property client side that is not server side and vice versa)
        ///  - server calls are working with Async 
        /// </summary>
        [Test]
        public void GetJsonClassTests()
        {
            var service = Container.Resolve<IIndexHttpService>();
            var task = CallAsync(service.TestGetClassAsync);
            var person = task.Result.Value;
            Assert.AreEqual(40, person.Age);
            Assert.AreEqual("Bob", person.Name);
            Assert.IsNotNull(person.Created);
            Assert.IsNull(person.NotServerSide);
            Assert.IsNull(person.Updated);
        }

        #endregion
    }
}
