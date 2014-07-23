using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agile.Diagnostics.Logging;
using NUnit.Framework;

namespace Agile.Framework.Tests
{
    /// <summary>
    /// Tests for reconciliation extensions
    /// </summary>
    [TestFixture]
    public class ReconcileExtensionsTests
    {
        private readonly Tzag tagA = new Tzag { Id = 20, Name = "A" };
        private readonly Tzag tagB = new Tzag { Id = 21, Name = "B" };
        private readonly Tzag tagC = new Tzag { Id = 22, Name = "C" };
        private readonly Tzag tagD = new Tzag { Id = 23, Name = "D" };
        private readonly Tzag tagE = new Tzag { Id = 24, Name = "E" };
        // tags that are  not yet saved, id is 0
        private readonly Tzag tagX = new Tzag { Name = "X" };
        private readonly Tzag tagY = new Tzag { Name = "Y" };

        private Tzask taskOne;

        private TaskTag junctionA;
        private TaskTag junctionB;
        private TaskTag junctionC;
        private TaskTag junctionD;
        /// <summary>
        /// Setup test data etc
        /// </summary>
        [SetUp]
        public void Setup()
        {
            taskOne = new Tzask { Id = 1, Name = "One" }; 

            junctionA = new TaskTag { Id = 90, Task = taskOne, Tag = tagA };
            junctionB = new TaskTag { Id = 91, Task = taskOne, Tag = tagB };
            junctionC = new TaskTag { Id = 92, Task = taskOne, Tag = tagC };
            junctionD = new TaskTag { Id = 93, Task = taskOne, Tag = tagD };

            Logger.Testing("setup the test data to more closely reflect the structure of the real biz objects");
            Logger.Testing("also need to add the Tags to the Task");
            taskOne.Tags = new List<Tzag> { tagA, tagB, tagC, tagD };
            Logger.Testing("and we'll also add the task to the tags lists");
            tagA.Tasks = new List<Tzask> { taskOne };
            tagB.Tasks = new List<Tzask> { taskOne };
            tagC.Tasks = new List<Tzask> { taskOne };
            tagD.Tasks = new List<Tzask> { taskOne };
        }

        #region CheckReconciliation

        /// <summary>
        /// CheckReconciliation
        /// </summary>
        [Test]
        public void CheckReconciliation()
        {
            Logger.Testing("we wil have n junction records that need to be reconciled with a list of Tags");
            Logger.Testing("logically all Tags already exist (have an id)");
            var junctions = new List<TaskTag> { junctionA, junctionB, junctionC, junctionD};
            Logger.Testing("so this list contains 2 of the existing tags (from the junction records) plus one other existing tag plus two brand new tags");
            Logger.Testing("so tag C and D have been removed");
            var tags = new List<Tzag> {tagA, tagB, tagE, tagX, tagY };

            Logger.Testing("check Remove contains C and D");
            var remove = junctions.GetRemoveItems(tags, (junc, child)=> junc.Tag == child);
            Assert.AreEqual(2, remove.Count);
            Assert.IsTrue(remove.Contains(junctionC));
            Assert.IsTrue(remove.Contains(junctionD));

            Logger.Testing("check Add contains E, X and Y");
            var add = junctions.GetAddItems(tags, (junc, child) => junc.Tag == child);
            Assert.AreEqual(3, add.Count);
            Assert.IsNotNull(add.Contains(tagE));
            Assert.IsNotNull(add.Contains(tagX));
            Assert.IsNotNull(add.Contains(tagY));

        }


        #endregion

        #region CheckReconciliationWithZeroExistingTags

        /// <summary>
        /// CheckReconciliationWithZeroExistingTags
        /// </summary>
        [Test]
        public void CheckReconciliationWithZeroExistingTags()
        {
            Logger.Testing("check the reconcile helper methods work when there are no existing junction records");
            var junctions = new List<TaskTag>();
            var tags = new List<Tzag> { tagA, tagB, tagE, tagX, tagY };

            Logger.Testing("--- now check junction helper methods ---");

            Logger.Testing("check Remove has 0 items");
            var remove = junctions.GetRemoveItems(tags, (junc, child) => junc.Tag == child);
            Assert.AreEqual(0, remove.Count);

            Logger.Testing("check Add contains A, B, E, X and Y");
            var add = junctions.GetAddItems(tags, (junc, child) => junc.Tag == child);
            Assert.AreEqual(5, add.Count);
            Assert.IsNotNull(add.Contains(tagA));
            Assert.IsNotNull(add.Contains(tagB));
            Assert.IsNotNull(add.Contains(tagE));
            Assert.IsNotNull(add.Contains(tagX));
            Assert.IsNotNull(add.Contains(tagY));
        }

        #endregion

        #region CheckReconciliationWithZeroChildItems

        /// <summary>
        /// CheckReconciliationWithZeroChildItems
        /// </summary>
        [Test]
        public void CheckReconciliationWithZeroChildItems()
        {
            Logger.Testing("check the reconcile helper methods work when there are no new child records");
            var junctions = new List<TaskTag> { junctionA, junctionB, junctionC, junctionD };
            var tags = new List<Tzag>();

            Logger.Testing("--- now check junction helper methods ----");

            Logger.Testing("check Remove contains A, B, C and D");
            var remove = junctions.GetRemoveItems(tags, (junc, child) => junc.Tag == child);
            Assert.AreEqual(4, remove.Count);
            Assert.IsTrue(remove.Contains(junctionA));
            Assert.IsTrue(remove.Contains(junctionB));
            Assert.IsTrue(remove.Contains(junctionC));
            Assert.IsTrue(remove.Contains(junctionD));

            Logger.Testing("check Add contains 0 items");
            var add = junctions.GetAddItems(tags, (junc, child) => junc.Tag == child);
            Assert.AreEqual(0, add.Count);
        }

        #endregion

        #region CheckReconciliationWithZeroZero

        /// <summary>
        /// CheckReconciliationWithZeroZero
        /// </summary>
        [Test]
        public void CheckReconciliationWithZeroZero()
        {
            Logger.Testing("check the reconcile helper methods work when there are no new child records");
            var junctions = new List<TaskTag>();
            var tags = new List<Tzag>();

            Logger.Testing("--- now check junction helper methods ---");
            
            Logger.Testing("check Remove contains 0 items");
            var remove = junctions.GetRemoveItems(tags, (junc, child) => junc.Tag == child);
            Assert.AreEqual(0, remove.Count);

            Logger.Testing("check Add contains 0 items");
            var add = junctions.GetAddItems(tags, (junc, child) => junc.Tag == child);
            Assert.AreEqual(0, add.Count);
        }

        #endregion

        #region CheckExistingItems

        /// <summary>
        /// CheckExistingItems
        /// </summary>
        [Test]
        public void CheckExistingItems()
        {
            Logger.Testing("we wil have n junction records that need to be reconciled with a list of Tags");
            Logger.Testing("logically all Tags already exist (have an id)");
            var junctions = new List<TaskTag> { junctionA, junctionB, junctionC, junctionD };
            Logger.Testing("so this list contains 2 of the existing tags (from the junction records) plus one other existing tag plus two brand new tags");
            Logger.Testing("so tag C and D have been removed");
            var tags = new List<Tzag> { tagA, tagB, tagE, tagX, tagY };

            Logger.Testing("check Remove contains C and D");
            var existing = junctions.GetExistingItems(tags, (junc, child) => junc.Tag == child);
            Assert.AreEqual(2, existing.Count);
            Assert.IsTrue(existing.Contains(tagA));
            Assert.IsTrue(existing.Contains(tagB));
        }

        #endregion

        #region CheckReconWorksInReverseChildToParent

        /// <summary>
        /// CheckReconWorksInReverseChildToParent
        /// </summary>
        [Test]
        public void CheckReconWorksInReverseChildToParent()
        {
            Logger.Testing("taskOne already has tag ABCD, need to add tags to T2 and T3");
            var taskTwo = new Tzask { Id = 2, Name = "Two" };
            var taskThree = new Tzask { Id = 3, Name = "Three" };
            var taskFour = new Tzask { Id = 4, Name = "Four" };
            taskTwo.Tags = new List<Tzag> { tagA, tagB };
            taskThree.Tags = new List<Tzag> { tagA, tagC };
            taskFour.Tags = new List<Tzag>();

            Logger.Testing("create the junction records (for task 2 and 3)");
            var junc2A = new TaskTag { Id = 80, Task = taskTwo, Tag =  tagA};
            var junc2B = new TaskTag { Id = 81, Task = taskTwo, Tag = tagB};
            var junc3A = new TaskTag { Id = 82, Task = taskThree, Tag = tagA };
            var junc3C = new TaskTag { Id = 83, Task = taskThree, Tag = tagC };
            var junctions = new List<TaskTag> { junctionA, junctionB, junctionC, junctionD 
                , junc2A, junc2B
                , junc3A, junc3C };

            var tasks = new List<Tzask> { taskOne, taskTwo, taskThree, taskFour };

//            Logger.Testing("now check junction helper methods");
//            Logger.Testing("check Matches contains  ");
//            var matching = junctions.GetMatches(tasks, tagA);
//            Assert.AreEqual(2, matching.Count);
//            Assert.IsTrue(matching.Contains(junctionA));
//            Assert.IsTrue(matching.Contains(junctionB));
//
//            Logger.Testing("check Remove contains C and D");
//            var remove = junctions.GetRemoveItems(tags, taskOne);
//            Assert.AreEqual(2, remove.Count);
//            Assert.IsTrue(remove.Contains(junctionC));
//            Assert.IsTrue(remove.Contains(junctionD));
//
//            Logger.Testing("check Add contains E, X and Y");
//            var add = junctions.GetAddItems(tags, taskOne);
//            Assert.AreEqual(3, add.Count);
//            Assert.IsNotNull(add.Contains(tagE));
//            Assert.IsNotNull(add.Contains(tagX));
//            Assert.IsNotNull(add.Contains(tagY));
        }

        #endregion
    }
    // for sanity just simulate Task TaskTag and Tag

    /// <summary>
    /// Testing use only
    /// </summary>
    /// <remarks>given a dodgy name because kept coming up first in type list when looking
    /// for Task</remarks>
    public class Tzask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Tzag> Tags { get; set; }
    }

    /// <summary>
    /// Testing use only
    /// </summary>
    /// <remarks>given a dodgy name because kept coming up first in type list when looking
    /// for Tag</remarks>
    public class Tzag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Tzask> Tasks { get; set; }
    }

    /// <summary>
    /// Testing use only
    /// </summary>
    public class TaskTag : IJunctionClass
    {
        public int Id { get; set; }
        public Tzag Tag { get; set; }
        public Tzask Task { get; set; }

        public bool CheckValue(Tzag tag)
        {
            return Tag == tag;
        }

        public bool CheckValue(Tzask task)
        {
            return Task == task;
        }

    }
}
