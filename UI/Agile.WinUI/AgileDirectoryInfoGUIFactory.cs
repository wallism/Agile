namespace Agile.WinUI
{
    /// <summary>
    /// Factory pattern object used to create GUI items for agile directory info types.
    /// </summary>
    public class AgileDirectoryInfoGUIFactory : AgileGUIFactoryBase
    {
        #region Constructors and Factories

        /// <summary>
        /// Private Constructor. Use the Instantiate method to instantiate.
        /// </summary>
        private AgileDirectoryInfoGUIFactory(object item) : base(item)
        {
        }

        /// <summary>
        /// Private Constructor. Use the Instantiate method to instantiate.
        /// </summary>
        private AgileDirectoryInfoGUIFactory()
        {
        }

        /// <summary>
        /// Instantiates a new AgileDirectoryInfoGUIFactory.
        /// </summary>
        /// <returns></returns>
        public static AgileDirectoryInfoGUIFactory Build(object item)
        {
            return new AgileDirectoryInfoGUIFactory(item);
        }

        /// <summary>
        /// Instantiates a new AgileDirectoryInfoGUIFactory.
        /// </summary>
        /// <returns></returns>
        public static AgileDirectoryInfoGUIFactory Build()
        {
            return new AgileDirectoryInfoGUIFactory();
        }

        #endregion
    }
}