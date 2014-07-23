using System;
using Agile.Diagnostics.Logging;
using Agile.Mobile.DAL;
using NUnit.Framework;

namespace Agile.Mobile.Tests.DAL
{
    /// <summary>
    /// Inherit from this if it is a 'standard' table
    /// and you want some free tests and useful methods
    /// </summary>
    public abstract class SQLiteTableTestBase<TDB, T>
        : SQLiteTestBase<TDB>
        where TDB : class, IAgileSQLite
        where T : LocalDbRecord, new()
    {
        protected virtual void SetTestRecordValues(T beforeInsertRecord)
        {
        }

        protected T InsertTestRecord()
        {
            Logger.Debug("INSERT");
            var id = Db.GetNextLocalId<T>();
            var record = new T();
            record.SetId(id);
            SetTestRecordValues(record);

            var result = Db.Insert(record);
            Logger.Debug("{0} [result:{1}]", record, result);
            return record;
        }

        /// <summary>
        /// Record gets mapped into Biz object, this tests all values map in properly
        /// </summary>
        public virtual void LoadedItemHasCorrectValuesInBizFromRecord()
        {
            throw new NotImplementedException("Need to implement this for all TableRecord types");
        }

        #region GetFirstRowTests

        /// <summary>
        /// GetFirstRow
        /// </summary>
        [Test]
        public void GetFirstRowTests()
        {
            Logger.Testing("make sure table is emtpy");
            Db.DeleteAllRecords<T>();
            Logger.Testing("First test, make sure NULL is fine...ie does not throw");
            var first = Db.GetFirstRecord<T>();
            Assert.IsNull(first);
            InsertTestRecord();
            first = Db.GetFirstRecord<T>();
            Assert.IsNotNull(first);

            Logger.Testing("OK, that's fine but is only one record");
            Logger.Testing("add another one...");
            InsertTestRecord();
            Logger.Testing("Get first again");
            var secondFirst = Db.GetFirstRecord<T>();
            Logger.Testing("the first and secondFirst should be the same");
            Assert.AreEqual(first, secondFirst);

            Logger.Testing("add another one...");
            InsertTestRecord();
            Logger.Testing("Get first again");
            var thirdFirst = Db.GetFirstRecord<T>();
            Logger.Testing("the first and thirdFirst should be the same");
            Assert.AreEqual(first, thirdFirst);
            Assert.AreEqual(3, Db.GetRecordCount(typeof(T)));
        }

        #endregion

        #region GetLastRowTests

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void GetLastRowTests()
        {
            Logger.Testing("ARRANGE");
            Logger.Testing("make sure table is emtpy");
            Db.DeleteAllRecords<T>();
            Logger.Testing("First test, make sure NULL is fine...ie does not throw");
            var last = Db.GetLastRecord<T>();
            Assert.IsNull(last);

            Logger.Testing("so now add a record");
            InsertTestRecord();

            Logger.Testing("ACT");
            Logger.Testing("get the last record");
            last = Db.GetLastRecord<T>();
            Assert.IsNotNull(last);

            Logger.Testing("OK, that's fine but is only one record");
            Logger.Testing("add another one...");
            InsertTestRecord();
            Logger.Testing("Get last again");
            var secondLast = Db.GetLastRecord<T>();

            Logger.Testing("ASSERT");
            Logger.Testing("the last and secondFirst should NOT be the same");
            Assert.AreNotEqual(last, secondLast);

            Logger.Testing("add another one...");
            InsertTestRecord();

            Logger.Testing("Get last again");
            var thirdLast = Db.GetLastRecord<T>();
            Logger.Testing("the last and thirdLast should NOT be the same");
            Assert.AreNotEqual(last, thirdLast);
            Assert.AreEqual(3, Db.GetRecordCount(typeof(T)));

            Logger.Testing("check the id of the third is > second");
            Assert.IsTrue(thirdLast.GetId() > secondLast.GetId());

        }

        #endregion


        #region SelectNonExistingRecordReturnsNullTests

        /// <summary>
        /// SelectNonExistingRecordReturnsNull
        /// </summary>
        [Test]
        public void SelectNonExistingRecordReturnsNullTests()
        {
            var selected = Db.Find<T>(-1);
            Assert.IsNull(selected);
            Logger.Testing("Value is null...exception didn't occur");
        }

        #endregion

        #region SelectByIdTests

        /// <summary>
        /// Get
        /// </summary>
        [Test]
        public void SelectByIdTests()
        {
            Logger.Testing("first need to create a test record");
            var inserted = InsertTestRecord();
            var selected = Db.Find<T>(inserted.GetId());
            Assert.IsNotNull(selected);
            Assert.AreEqual(inserted.GetId(), selected.GetId());
        }

        #endregion

        #region FindByAltIdTests

        /// <summary>
        /// FindByAltId
        /// </summary>
        [Test]
        public void FindByAltIdTests()
        {
            var record = InsertTestRecord();
            Logger.Testing("make sure we can load the record using the AltId");
            var loaded = Db.FindByAltId<T>(record.AltId);
            Assert.AreEqual(record, loaded);
        }

        #endregion


        #region GetNextLocalIdFromEmptyTests

        /// <summary>
        /// GetNextLocalIdForEmptyTable
        /// </summary>
        [Test]
        public void GetNextLocalIdFromEmptyTests()
        {
            Db.DeleteAllRecords<T>();
            Logger.Testing("Check what the next id will be before insert any records");
            var next = Db.GetNextLocalId<T>();

            Logger.Testing("then create a record then check returns 2");
            var record = InsertTestRecord();
            var nextAfterInsert = Db.GetNextLocalId<T>();
            Assert.Greater(nextAfterInsert, next);
            Assert.AreEqual(record.GetId() + 1, nextAfterInsert);
        }

        #endregion

        #region GetNextLocalIdWhenExistingRecordsOverId10000Tests

        /// <summary>
        /// GetNextLocalIdWhenExistingRecordsOverId10000
        /// </summary>
        [Test]
        public virtual void GetNextLocalIdWhenExistingRecordsOverId10000Tests()
        {
            Logger.Testing("First make sure the table is empty");
            Db.DeleteAllRecords<T>();

            Logger.Testing("Create a record with an id 10000");
            var item = new T();
            item.SetId(10000);
            SetTestRecordValues(item);
            Logger.Testing("save the record");
            Db.Insert(item);

            Logger.Testing("at the moment there are zero records with an id under 10000 so the local id should return 1");
            Assert.AreEqual(1, Db.GetNextLocalId<T>());

            InsertTestRecord();
            Logger.Testing("now should next id should be 2");
            Assert.AreEqual(2, Db.GetNextLocalId<T>());


            Logger.Testing("Create another new record this time id 9998 (this is a local id, one short of max)");
            var highLocal = new T();
            highLocal.SetId(9998);
            SetTestRecordValues(highLocal);
            Logger.Testing("save the record");
            Db.Insert(highLocal);

            Logger.Testing("now should next id should be 9999");
            Assert.AreEqual(9999, Db.GetNextLocalId<T>());
        }

        #endregion

    }
}