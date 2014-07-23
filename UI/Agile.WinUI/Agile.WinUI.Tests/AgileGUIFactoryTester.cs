namespace Agile.WinUI.Tests
{
    /// <summary>
    /// Summary description for AgileGUIFactoryTester.
    /// </summary>
    public class AgileGUIFactoryTester : AgileGUIFactoryBase
    {
        #region Constructors and Factories

        /// <summary>
        /// Private Constructor. Use the Instantiate method to instantiate.
        /// </summary>
        private AgileGUIFactoryTester()
        {
        }

        /// <summary>
        /// Private Constructor. Use the Instantiate method to instantiate.
        /// </summary>
        private AgileGUIFactoryTester(object item) : base(item)
        {
        }

        /// <summary>
        /// Instantiates a new AgileGUIFactoryTester.
        /// </summary>
        /// <returns></returns>
        public static AgileGUIFactoryTester Build()
        {
            return new AgileGUIFactoryTester();
        }

        /// <summary>
        /// Instantiates a new AgileGUIFactoryTester.
        /// </summary>
        /// <returns></returns>
        public static AgileGUIFactoryTester Build(object item)
        {
            return new AgileGUIFactoryTester(item);
        }

        #endregion
    }
}