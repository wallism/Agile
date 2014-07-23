using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agile.Diagnostics.Logging;
using NUnit.Framework;

namespace Agile.Framework.Tests
{
    /// <summary>
    /// SaveableItemTests
    /// </summary>
    [TestFixture]
    public class SaveableItemTests
    {
        #region EqualsTests

        [Test]
        public void EqualsTests()
        {
            Logger.Testing(@"Create 2 saveable items with different AlternateCreationIDs");
            var one = new Saveable();
            var two = new Saveable();

            Logger.Testing(@"AST Equals is false");
            Assert.IsFalse(one.Equals(two));
            Logger.Testing(@"AST EqualsId is false");
            Assert.IsFalse(one.EqualsId(two));

            Logger.Testing(@"ARR now set the Alternate ids to be the same");
            one.AltId = two.AltId;
            Logger.Testing(@"AST Equals is false");
            Assert.IsFalse(one.Equals(two));
            Logger.Testing(@"AST EqualsId is true");
            Assert.IsTrue(one.EqualsId(two));
        }

        #endregion

        #region EqualsWithNullTests

        [Test]
        public void EqualsWithNullTests()
        {
            Logger.Testing(@"ARR create one saveable item");
            var one = new Saveable();
            Logger.Testing(@"AST it is not equal to null");
            Assert.IsFalse(one.EqualsId(null));
        }

        #endregion

        #region EqualsWithEmptyIdTests

        [Test, ExpectedException]
        public void EqualsWithEmptyIdTests()
        {
            Logger.Testing(@"Create 2 saveable items with -one- 'empty' altId");
            var one = new Saveable();
            var two = new Saveable();
            one.AltId = Guid.Empty;

            Logger.Testing(@"ACT compare EqualsId, shoudl throw ex");
            one.EqualsId(two);
        }

        #endregion

        #region EqualsWithEmptyIdTests2

        [Test, ExpectedException]
        public void EqualsWithEmptyIdTests2()
        {
            Logger.Testing(@"Create 2 saveable items with -two- 'empty' altId");
            var one = new Saveable();
            var two = new Saveable();
            two.AltId = Guid.Empty;

            Logger.Testing(@"ACT compare EqualsId, shoudl throw ex");
            one.EqualsId(two);
        }

        #endregion
    }

    public class Saveable : ISaveableItem
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Saveable()
        {
            AltId = Guid.NewGuid();
        }
        /// <summary>
        /// Used for client side saving so we can match up with the right item after saving (especially for Lists)
        /// </summary>
        public Guid AltId { get; set; }

        /// <summary>
        /// Returns true if the item has never been saved, i.e. the id  is 0 (or equivalent)
        /// </summary>
        public bool IsNewItem
        {
            get { return true; }
        }
    }
}
