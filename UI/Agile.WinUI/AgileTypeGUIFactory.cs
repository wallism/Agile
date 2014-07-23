namespace Agile.WinUI
{
    /// <summary>
    /// Factory pattern object used to create GUI items for DatabaseTable types.
    /// </summary>
    public class AgileTypeGUIFactory : AgileGUIFactoryBase
    {
        #region Constructors and Factories

        /// <summary>
        /// Private Constructor. Use the Instantiate method to instantiate.
        /// </summary>
        private AgileTypeGUIFactory()
        {
        }

        /// <summary>
        /// Private Constructor. Use the Instantiate method to instantiate.
        /// </summary>
        private AgileTypeGUIFactory(object item) : base(item)
        {
        }

        /// <summary>
        /// Instantiates a new DatabaseTableGUIFactory.
        /// </summary>
        /// <returns></returns>
        public static AgileTypeGUIFactory Build()
        {
            return new AgileTypeGUIFactory();
        }

        /// <summary>
        /// Instantiates a new DatabaseTableGUIFactory.
        /// </summary>
        /// <returns></returns>
        public static AgileTypeGUIFactory Build(object item)
        {
            return new AgileTypeGUIFactory(item);
        }

        #endregion
    }
}