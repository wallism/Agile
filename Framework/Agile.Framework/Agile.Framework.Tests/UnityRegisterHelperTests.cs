using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Agile.Diagnostics.Logging;
using Agile.Framework.Services;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Agile.Framework.Tests
{
    [TestFixture]
    public class UnityRegisterHelperTests
    {
        #region Test GetInterfacesThatImplement

        /// <summary>
        /// Test GetInterfacesThatImplement
        /// </summary>
        [Test]
        public void GetInterfacesThatImplementTest()
        {
            var interfaces = UnityRegisterHelper.GetInterfacesThatImplement(typeof (IBizWebService), UnityRegisterHelper.GetSearchAssemblies());

            Assert.IsTrue(interfaces.Contains(typeof(ITestWebService)));
            Assert.IsFalse(interfaces.Contains(typeof(TestWebService)));

            Logger.Testing("repeat but this time do not pass in the search assemblies. This test is valid because sometimes with Assembly methods it matters where the call is made");
            interfaces = UnityRegisterHelper.GetInterfacesThatImplement(typeof(IBizWebService));

            Assert.IsTrue(interfaces.Contains(typeof(ITestWebService)));
            Assert.IsFalse(interfaces.Contains(typeof(TestWebService)));
        }

        #endregion

        #region Test GetConcreteTypesThatImplement

        /// <summary>
        /// Test GetConcreteTypesThatImplement
        /// </summary>
        [Test]
        public void GetConcreteTypesThatImplementTest()
        {
            var types = UnityRegisterHelper.GetConcreteTypesThatImplement(typeof(IBizWebService), UnityRegisterHelper.GetSearchAssemblies());
            var webTypes = UnityRegisterHelper.GetConcreteTypesThatImplement(typeof(ITestWebService), UnityRegisterHelper.GetSearchAssemblies());

            Assert.IsTrue(webTypes.Contains(typeof(TestWebService)));
            // both work because  ITestWebService : IBizWebService
            Assert.IsTrue(types.Contains(typeof(TestWebService)));
        }

        #endregion


        /// <summary>
        /// Test registering of web services
        /// </summary>
        [Test]
        public void RegisterWebServiceTest()
        {
            var container = new UnityContainer();
            UnityRegisterHelper.RegisterContainer(container);
            UnityRegisterHelper.RegisterTypesThatImplement(typeof(IBizWebService), UnityRegisterHelper.GetSearchAssemblies());

            var webService = container.Resolve<ITestWebService>();
            Assert.IsNotNull(webService);
        }

        /// <summary>
        /// Test registering of server services
        /// </summary>
        [Test]
        public void RegisterServerServiceTest()
        {
            var container = new UnityContainer();
            UnityRegisterHelper.RegisterContainer(container);
            UnityRegisterHelper.RegisterTypesThatImplement(typeof(IBizService), UnityRegisterHelper.GetSearchAssemblies());

            var service = container.Resolve<ITestService>();
            Assert.IsNotNull(service);

            Logger.Testing("repeat but this time do not pass in the search assemblies. This test is valid because sometimes with Assembly methods it matters where the call is made");
            container = new UnityContainer();
            UnityRegisterHelper.RegisterContainer(container);
            UnityRegisterHelper.RegisterTypesThatImplement(typeof(IBizService));

            service = container.Resolve<ITestService>();
            Assert.IsNotNull(service);
        }

        #region Test RegisterTypesPassingInAssemblyString

        /// <summary>
        /// Test RegisterTypesPassingInAssemblyString
        /// </summary>
        [Test]
        public void RegisterTypesPassingInAssemblyStringTest()
        {
            var container = new UnityContainer();
            UnityRegisterHelper.RegisterContainer(container);
            UnityRegisterHelper.RegisterTypesThatImplement(typeof(IBizService), new List<string>{"Agile.Framework.Tests"});

            var service = container.Resolve<ITestService>();
            Assert.IsNotNull(service);
        }

        #endregion

        #region Test RegisterTypesPassingInvalidAssemblyString

        /// <summary>
        /// Test RegisterTypesPassingInvalidAssemblyString
        /// </summary>
        [Test, ExpectedException(typeof(FileNotFoundException))]
        public void RegisterTypesPassingInvalidAssemblyStringTest()
        {
            var container = new UnityContainer();
            UnityRegisterHelper.RegisterContainer(container);
            UnityRegisterHelper.RegisterTypesThatImplement(typeof(IBizService), new List<string>{"Invalid.Assembly.Name"});

            var service = container.Resolve<ITestService>();
            Assert.IsNotNull(service);
        }

        #endregion
    }



    public interface ITestWebService : IBizWebService
    { }

    public class TestWebService : ITestWebService 
    { }



    public interface ITestService : IBizService
    { }

    public class TestService : ITestService
    { }
}
