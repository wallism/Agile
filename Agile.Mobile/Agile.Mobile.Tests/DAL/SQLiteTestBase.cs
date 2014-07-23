using System;
using Agile.Diagnostics.Logging;
using Agile.Mobile.DAL;
using Agile.Shared.IoC;
using Agile.Testing;
using NUnit.Framework;

namespace Agile.Mobile.Tests.DAL
{
    /// <summary>
    /// All SQLite tests can inherit from this
    /// </summary>
    /// <typeparam name="TDB"></typeparam>
    /// <typeparam name="T">specific (main) type that is being tested.
    /// Using this allows us to run the same standard tests against all tables</typeparam>
    public abstract class SQLiteTestBase<TDB> : TestBase
        where TDB : class, IAgileSQLite
    {

        public override void TestFixtureSetUp()
        {
            base.TestFixtureSetUp();
            Container.Register(RegisterDb());

            Logger.Testing("At the start of every fixture DROP and CREATE all tables");
            Db.DropThenCreateAllTables();
        }

        public override void SetUp()
        {
            base.SetUp();
            Container.Register(RegisterDb());
        }

        protected abstract Func<TDB> RegisterDb();
        
        private TDB db;
        /// <summary>
        /// Gets the SQLite database
        /// </summary>
        protected TDB Db
        {
            get { return db ?? (db = Container.Resolve<TDB>()); }
        }

    }
}