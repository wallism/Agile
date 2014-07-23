using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Diagnostics.Logging;
using Agile.Mobile.Web;
using Agile.Shared.IoC;
using Agile.Testing;
using NUnit.Framework;

namespace Agile.Mobile.Tests
{
    public class HttpTestBase : TestBase
    {
        public override void TestFixtureSetUp()
        {
            base.TestFixtureSetUp();

            Logger.Info("default to having a connection for these tests");
            Container.RegisterInstance<INetworkConnectionManager>
                (new NetworkConnectionManager(() => ConnectionState.Ethernet));
        }

        protected async Task<ServiceCallResult<T>> CallAsync<T>(Func<Task<ServiceCallResult<T>>> webMethod)
        {
            Logger.Testing("Start call to async: {0}", webMethod.Method.Name);
            var result = await webMethod();
            Logger.Testing("await returned...ASSERT the call was successful");
            Assert.IsTrue(result.WasSuccessful);
            return result;
        }

        protected async Task<ServiceCallResult<T>> CallAsync<T>(Func<int, Task<ServiceCallResult<T>>> webMethod, int id)
        {
            Logger.Testing("Start call to async: {0}({1})", webMethod.Method.Name, id);
            var result = await webMethod(id);
            Logger.Testing("await returned...ASSERT the call was successful");
            Assert.IsTrue(result.WasSuccessful);
            return result;
        }
    }
}
