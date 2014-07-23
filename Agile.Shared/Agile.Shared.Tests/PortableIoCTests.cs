using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agile.Diagnostics.Logging;
using Agile.Shared.IoC;
using NSubstitute;
using NUnit.Framework;

namespace Agile.Shared.Tests
{
    [TestFixture]
    public class PortableIoCTests
    {
        #region SimpleRegisterAndResolveTests

        /// <summary>
        /// SimpleRegisterAndResolve
        /// </summary>
        [Test]
        public void SimpleRegisterAndResolveTests()
        {
            try
            {
                Logger.Testing("ARR register the concrete factory");
                Container.Register(LittleService.ConcreteFactory);
                var little = Container.Resolve<ILittleService>();
                Assert.IsNotNull(little);
                Logger.Testing("ASSERT 1 is returned");
                Assert.AreEqual(1, little.GetInt());


                Logger.Testing("ACT now REGISTER a second item");
                Container.Register(BigService.ConcreteFactory);
                Logger.Testing("ACT resolve it");
                var big = Container.Resolve<IBigService>();
                Assert.IsNotNull(big);
                Assert.AreEqual("big", big.GetString());

                Logger.Testing("ARR change to the Mock ");
                Container.Register(LittleService.MockFactory);
                Logger.Testing("ACT resolve");
                little = Container.Resolve<ILittleService>();
                Logger.Testing("ASSERT 2 is returned");
                Assert.AreEqual(2, little.GetInt());

                Logger.Testing("ARR change to the Other");
                Container.Register(LittleService.ConcreteOtherFactory);
                Logger.Testing("ACT resolve");
                little = Container.Resolve<ILittleService>();
                Assert.AreEqual(3, little.GetInt());

                Logger.Testing("ACT finally, Register a concrete service");
                var concrete = new ConcreteService();
                Container.RegisterInstance<IConcreteService>(concrete);
                Assert.AreEqual(3, Container.Count);
                var resolvedConcrete = Container.Resolve<IConcreteService>();
                Assert.IsNotNull(resolvedConcrete);
                Assert.AreEqual(10, resolvedConcrete.Number);
            }
            finally
            {
                Container.Reset();
            }
        }

        #endregion

        #region ResetTests

        /// <summary>
        /// Reset
        /// </summary>
        [Test]
        public void ResetTests()
        {
            Logger.Testing("ACT first call reset, to make sure it is safe to call on an empty container");
            Container.Reset();
            Logger.Testing("ARR register the concrete factory");
            Container.Register(LittleService.ConcreteFactory);
            var resolved = Container.Resolve<ILittleService>();
            Assert.IsNotNull(resolved);

            Logger.Testing("ASSERT check there is 1 item");
            Assert.AreEqual(1, Container.Count);
            Logger.Testing("ACT reset");
            Container.Reset();
            Logger.Testing("ASSERT check there are 0 items");
            Assert.AreEqual(0, Container.Count);

        }

        #endregion
    }


    public interface ILittleService
    {
        int GetInt();
    }
    
    public class LittleService : ILittleService
    {
        /// <summary>
        /// Method for instantiating a concrete service
        /// </summary>
        public static ILittleService ConcreteFactory()
        {
            return new LittleService();
        }
        /// <summary>
        /// Method for instantiating a concrete service
        /// </summary>
        public static ILittleService ConcreteOtherFactory()
        {
            return new LittleOtherService();
        }

        /// <summary>
        /// Method for instantiating a mock service
        /// </summary>
        public static ILittleService MockFactory()
        {
            var service = Substitute.For<ILittleService>();
            service.GetInt().Returns(2);
            return service;
        }

        public int GetInt()
        {
            return 1;
        }
    }

    public class LittleOtherService : ILittleService, IDisposable
    {
        public int GetInt()
        {
            return 3;
        }

        public void Dispose()
        {
            Logger.Debug("Disposing!");
        }
    }


    public interface IBigService
    {
        string GetString();
    }

    public class BigService : IBigService
    {
        /// <summary>
        /// Method for instantiating a concrete service
        /// </summary>
        public static IBigService ConcreteFactory()
        {
            return new BigService();
        }

        public string GetString()
        {
            return "big";
        }
    }

    public interface IConcreteService
    {
        int Number { get; set; }
    }

    public class ConcreteService : IConcreteService
    {
        public ConcreteService()
        {
            Number = 10;
        }

        public int Number { get; set; }
    }

}
