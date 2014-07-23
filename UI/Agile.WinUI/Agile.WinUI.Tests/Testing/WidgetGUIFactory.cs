namespace Agile.WinUI
{
    /// <summary>
    /// GUI factory that is used for Widgets....TESTING
    /// </summary>
    public class WidgetGUIFactory : AgileGUIFactoryBase
    {
        #region Constructors and Factories

        /// <summary>
        /// Private Constructor. Use the Instantiate method to instantiate.
        /// </summary>
        private WidgetGUIFactory()
        {
        }

        /// <summary>
        /// Private Constructor. Use the Instantiate method to instantiate.
        /// </summary>
        private WidgetGUIFactory(object item) : base(item)
        {
        }

        /// <summary>
        /// Instantiates a new WidgetGUIFactory.
        /// </summary>
        /// <returns></returns>
        public static WidgetGUIFactory Build()
        {
            return new WidgetGUIFactory();
        }

        /// <summary>
        /// Instantiates a new WidgetGUIFactory.
        /// </summary>
        /// <returns></returns>
        public static WidgetGUIFactory Build(object item)
        {
            return new WidgetGUIFactory(item);
        }

        #endregion
    }
}