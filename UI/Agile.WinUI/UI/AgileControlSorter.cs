using System;
using System.Collections;

namespace Agile.Common.UI
{
    /// <summary>
    /// Implementation of IComparer for 
    /// objects that implement IAgileControlDetails.
    /// </summary>
    public class AgileControlSorter : IComparer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private AgileControlSorter()
        {
        }

        #region IComparer Members

        /// <summary>
        /// Compare one object with the other for sorting.
        /// </summary>
        /// <param name="x">any object but must implement IAgileControlDetails.</param>
        /// <param name="y">any object but must implement IAgileControlDetails.</param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            if (!(x is IAgileControlDetails))
                throw new ArgumentException(string.Format("{0} does not implement IAgileControlDetails.", x.GetType()));
            if (!(y is IAgileControlDetails))
                throw new ArgumentException(string.Format("{0} does not implement IAgileControlDetails.", y.GetType()));

            var apple = x as IAgileControlDetails;
            var orange = y as IAgileControlDetails;

            return apple.DisplayValue.CompareTo(orange.DisplayValue);
        }

        #endregion

        /// <summary>
        /// Instantiate a new AgileControlSorter.
        /// </summary>
        /// <returns></returns>
        public static AgileControlSorter Build()
        {
            return new AgileControlSorter();
        }
    }
}