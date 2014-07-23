using System.Collections;

namespace Agile.Common.UI
{
    /// <summary>
    /// Implementation of IComparer for 
    /// objects that implement IAgileControlDetails.
    /// </summary>
    public class AgileControlReverseSorter : IComparer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private AgileControlReverseSorter()
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
            return (AgileControlSorter.Build()).Compare(y, x);

//            if (!(x is IAgileControlDetails))
//                throw new ArgumentException(string.Format("{0} does not implement IAgileControlDetails.", x.GetType()));
//            if (!(y is IAgileControlDetails))
//                throw new ArgumentException(string.Format("{0} does not implement IAgileControlDetails.", y.GetType()));
//            
//            IAgileControlDetails apple = x as IAgileControlDetails;
//            IAgileControlDetails orange = y as IAgileControlDetails;
//
//            return orange.DisplayValue.CompareTo(apple.DisplayValue);
        }

        #endregion

        /// <summary>
        /// Instantiate a new AgileControlReverseSorter.
        /// </summary>
        /// <returns></returns>
        public static AgileControlReverseSorter Build()
        {
            return new AgileControlReverseSorter();
        }
    }
}